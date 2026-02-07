using Application.Features.Tenants.Dtos;

namespace Application.Features.Tenants.Queries.GetContentLibraryResources
{
    public record GetContentLibraryResourcesQuery(string? q, string type) : IRequest<ContentLibraryResourceDto> { }
}
