using FluentValidation;
using LifeOrganizer.Application.Common.Exceptions;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Auth;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber) : IRequest<AuthResultDto>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IApplicationDbContext _context;

    public RegisterCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256)
            .MustAsync(BeUniqueEmail).WithMessage("Email Already Exists");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long")
            .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).MaximumLength(30);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        => !await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email.Trim(),
            PasswordHash = _passwordHasher.Hash(request.Password),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber.Trim()
        };

        _context.Users.Add(user);

         _context.ScheduleBlocks.AddRange(
            new ScheduleBlock { UserId = user.Id, Name = "Work", StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(17, 0), ColorHex = "#0A84FF", SortOrder = 1 },
            new ScheduleBlock { UserId = user.Id, Name = "Gym", StartTime = new TimeOnly(17, 15), EndTime = new TimeOnly(18, 15), ColorHex = "#FF375F", SortOrder = 2 },
            new ScheduleBlock { UserId = user.Id, Name = "Language Learning", StartTime = new TimeOnly(18, 45), EndTime = new TimeOnly(19, 45), ColorHex = "#30D158", SortOrder = 3 },
            new ScheduleBlock { UserId = user.Id, Name = "Other Activities", StartTime = new TimeOnly(20, 0), EndTime = new TimeOnly(21, 0), ColorHex = "#FF9F0A", SortOrder = 4 }
        );
        _context.AppSettings.Add(new AppSettings { UserId = user.Id });
        _context.LanguageProgress.Add(new LanguageProgress { UserId = user.Id });

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}

public record LoginCommand(string Email, string Password) : IRequest<AuthResultDto>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower() == request.Email.Trim().ToLower(), cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new AuthenticationException("Invalid email or password");
        }

        return new AuthResultDto
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
