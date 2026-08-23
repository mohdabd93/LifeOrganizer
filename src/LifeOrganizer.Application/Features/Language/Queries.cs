using LifeOrganizer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LifeOrganizer.Application.Features.Language;

public record GetLanguageWordsQuery : IRequest<List<LanguageWordDto>>;

public class GetLanguageWordsQueryHandler : IRequestHandler<GetLanguageWordsQuery, List<LanguageWordDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLanguageWordsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<LanguageWordDto>> Handle(GetLanguageWordsQuery request, CancellationToken cancellationToken)
    {
        return await _context.LanguageWords.AsNoTracking()
            .Select(w => new LanguageWordDto
            {
                Id = w.Id, TargetLanguageText = w.TargetLanguageText, TranslationText = w.TranslationText
            })
            .ToListAsync(cancellationToken);
    }
}

public record GetLanguageProgressQuery : IRequest<LanguageProgressDto>;

public class GetLanguageProgressQueryHandler : IRequestHandler<GetLanguageProgressQuery, LanguageProgressDto>
{
    private readonly IApplicationDbContext _context;

    public GetLanguageProgressQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<LanguageProgressDto> Handle(GetLanguageProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await _context.LanguageProgress.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

        return new LanguageProgressDto
        {
            CurrentLevel = progress?.CurrentLevel ?? "A1",
            TargetLevel = progress?.TargetLevel ?? "A2",
            ProgressPercent = progress?.ProgressPercent ?? 0
        };
    }
}
