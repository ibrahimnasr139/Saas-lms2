using Application.Features.Tenants.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories
{
    internal sealed class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private IDbContextTransaction? _transaction;

        public TenantRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddTenantMemberAsync(TenantMember tenantMember, CancellationToken cancellationToken)
        {
            await _dbContext.TenantMembers.AddAsync(tenantMember, cancellationToken);
        }

        public async Task AddTenantRoles(int tenantId, CancellationToken cancellationToken)
        {
            var roles = new List<TenantRole>
            {
                new TenantRole { Name = RolesConstants.Owner, TenantId = tenantId },
                new TenantRole { Name = RolesConstants.Assistant, TenantId = tenantId }
            };
            await _dbContext.TenantRoles.AddRangeAsync(roles, cancellationToken);
        }

        public async Task<int> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken)
        {
           await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
           await SaveAsync(cancellationToken);
            return tenant.Id;
        }

        public async Task<TenantRole?> FindTenantRoleByTenantId(int tenantId, string Name, CancellationToken cancellationToken)
        {
           return await _dbContext.TenantRoles
                .FirstOrDefaultAsync(tr => tr.TenantId == tenantId && tr.Name == Name, cancellationToken);
        }

        public async Task<bool> IsSubDomainExistsAsync(string subDomain, CancellationToken cancellationToken)
        {
            return await _dbContext.Tenants
                .AnyAsync(t => t.SubDomain == subDomain, cancellationToken);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction is null)
            {
                _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
            }
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction is null) return;

            await _dbContext.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            if (_transaction is null) return;

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<LastTenantDto?> GetLastTenantAsync(string? subDomain ,CancellationToken cancellationToken)
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .ProjectTo<LastTenantDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.SubDomain == subDomain ,cancellationToken);
        }
    }
}
