using Application.Features.Roles.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Infrastructure.Repositories
{
    internal sealed class TenantRoleRepository : ITenantRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TenantRoleRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<TenantRolesDto>> GetTenantRolesAsync(int tenantId, CancellationToken cancellationToken)
        {
            return await _context.TenantRoles
               .AsNoTracking()
               .Where(tr => tr.TenantId == tenantId)
               .ProjectTo<TenantRolesDto>(_mapper.ConfigurationProvider)
               .ToListAsync(cancellationToken);
        }
    }
}
