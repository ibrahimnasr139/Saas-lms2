using Application.Features.Tenants.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using Infrastructure.Constants;
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

        public async Task<(int ownerRoleId, int assistantRoleId)> AddTenantRoles(int tenantId, CancellationToken cancellationToken)
        {
            var roles = new List<TenantRole>
            {
                new TenantRole { Name = RolesConstants.Owner, TenantId = tenantId, HasAllPermissions = true },
                new TenantRole { Name = RolesConstants.Assistant, TenantId = tenantId }
            };
            var ownerRole = await _dbContext.TenantRoles.AddAsync(roles[0], cancellationToken);
            var assistantRole = await _dbContext.TenantRoles.AddAsync(roles[1], cancellationToken);
            await SaveAsync(cancellationToken);
            return (ownerRole.Entity.Id, assistantRole.Entity.Id);
        }


        public async Task<int> CreateTenantAsync(Tenant tenant, CancellationToken cancellationToken)
        {
            await _dbContext.Tenants.AddAsync(tenant, cancellationToken);
            await SaveAsync(cancellationToken);
            return tenant.Id;
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

        public async Task<LastTenantDto?> GetLastTenantAsync(string? subDomain, CancellationToken cancellationToken)
        {
            return await _dbContext.Tenants
                .AsNoTracking()
                .ProjectTo<LastTenantDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.SubDomain == subDomain, cancellationToken);
        }

        public async Task AssignAssistantPermissions(int assistantRoleId, CancellationToken cancellationToken)
        {
            var permissions = GetAssistantPermissions().Select(permission => new Permission
            {
                Id = permission
            }).ToList();
            _dbContext.AttachRange(permissions);
            var rolePermissions = permissions.Select(permission => new RolePermission
            {
                TenantRoleId = assistantRoleId,
                PermissionId = permission.Id
            });
            await _dbContext.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);
        }
        private List<string> GetAssistantPermissions()
        {
            return new List<string>
            {
                PermissionConstants.CREATE_ASSIGNMENTS,
                PermissionConstants.VIEW_ASSIGNMENTS,
                PermissionConstants.MANAGE_ASSIGNMENTS,
                PermissionConstants.GRADE_ASSIGNMENTS,
                PermissionConstants.VIEW_COURSES,
                PermissionConstants.EDIT_COURSES,
                PermissionConstants.MANAGE_LESSONS,
                PermissionConstants.MANAGE_VIDEOS,
                PermissionConstants.MANAGE_MODULE_ITEMS,
                PermissionConstants.VIEW_MEMBERS,
                PermissionConstants.VIEW_MEMBER_PROFILE,
                PermissionConstants.VIEW_DASHBOARD,
                PermissionConstants.VIEW_ANALYTICS,
                PermissionConstants.VIEW_PERFORMANCE_CHART,
                PermissionConstants.VIEW_RECORDINGS,
                PermissionConstants.INVITE_STUDENTS,
                PermissionConstants.MANAGE_QUIZZES,
                PermissionConstants.CREATE_QUIZZES,
                PermissionConstants.VIEW_QUIZZES,
                PermissionConstants.VIEW_QUESTION_BANK,
                PermissionConstants.GRADE_QUIZZES,
            };
        }
    }
}
