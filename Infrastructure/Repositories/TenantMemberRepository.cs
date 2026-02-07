using Application.Features.TenantMembers.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Infrastructure.Repositories
{
    internal sealed class TenantMemberRepository : ITenantMemberRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TenantMemberRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<string>?> GetAllPermissions(int tenantRoleId, CancellationToken cancellationToken)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Where(trp => trp.TenantRoleId == tenantRoleId)
                .Select(trp => trp.PermissionId)
                .ToListAsync(cancellationToken);
        }
        public async Task<CurrentTenantMemberDto?> GetCurrentTenantMemberAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.TenantMembers
                 .AsNoTracking()
                 .ProjectTo<CurrentTenantMemberDto>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(tm => tm.UserId == userId, cancellationToken);
        }
        public Task<List<TenantMembersDto>> GetTenantMembersAsync(int tenantId, CancellationToken cancellationToken)
        {
            return _context.TenantMembers
                .AsNoTracking()
                .Where(tm => tm.TenantId == tenantId)
                .ProjectTo<TenantMembersDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
