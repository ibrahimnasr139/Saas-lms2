using Application.Features.Tenants.Dtos;

namespace Application.Features.Tenants.Queries.GetContentLibraryStatistics
{
    public record GetContentLibraryStatisticsQuery : IRequest<ContentLibraryStatisticsDto> { }
}
