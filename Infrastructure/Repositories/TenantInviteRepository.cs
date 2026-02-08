namespace Infrastructure.Repositories
{
    internal class TenantInviteRepository : ITenantInviteRepository
    {
        private readonly AppDbContext _context;

        public TenantInviteRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateTenantInviteAsync(TenantInvite tenantInvite, CancellationToken cancellationToken)
        {
            await _context.TenantInvites.AddAsync(tenantInvite, cancellationToken);
        }
        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
