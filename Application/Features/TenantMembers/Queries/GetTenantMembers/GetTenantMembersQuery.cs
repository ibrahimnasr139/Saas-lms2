using Application.Features.TenantMembers.Dtos;

namespace Application.Features.TenantMembers.Queries.GetTenantMembers
{
    public sealed record GetTenantMembersQuery : IRequest<List<TenantMembersDto>> { }
}
