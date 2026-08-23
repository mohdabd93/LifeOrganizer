using FluentValidation;
using LifeOrganizer.Application.Common.Interfaces;
using LifeOrganizer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Language;

public record CreateLanguageWordCommand(string TargetLanguageText, string TranslationText) : IRequest<Guid>;

public class CreateLanguageWordCommandValidator : AbstractValidator<CreateLanguageWordCommand>
{
    public CreateLanguageWordCommandValidator()
    {
        RuleFor(x => x.TargetLanguageText).NotEmpty().MaximumLength(150);
        RuleFor(x => x.TranslationText).NotEmpty().MaximumLength(150);
    }
}

public class CreateLanguageWordCommandHandler : IRequestHandler<CreateLanguageWordCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateLanguageWordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateLanguageWordCommand request, CancellationToken cancellationToken)
    {
        var entity = new LanguageWord
        {
            UserId = _currentUser.UserId!.Value,
            TargetLanguageText = request.TargetLanguageText,
            TranslationText = request.TranslationText
        };
        _context.LanguageWords.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

public record DeleteLanguageWordCommand(Guid Id) : IRequest;

public class DeleteLanguageWordCommandHandler : IRequestHandler<DeleteLanguageWordCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLanguageWordCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteLanguageWordCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LanguageWords.FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
        if (entity is null) return;

        _context.LanguageWords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record UpdateLanguageProgressCommand(string CurrentLevel, string TargetLevel, int ProgressPercent) : IRequest;

public class UpdateLanguageProgressCommandHandler : IRequestHandler<UpdateLanguageProgressCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public UpdateLanguageProgressCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateLanguageProgressCommand request, CancellationToken cancellationToken)
    {
        var progress = await _context.LanguageProgress.FirstOrDefaultAsync(cancellationToken);

        if (progress is null)
        {
            progress = new LanguageProgress { UserId = _currentUser.UserId!.Value };
            _context.LanguageProgress.Add(progress);
        }

        progress.CurrentLevel = request.CurrentLevel;
        progress.TargetLevel = request.TargetLevel;
        progress.ProgressPercent = request.ProgressPercent;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
