using Domain.Abstractions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        internal DbSet<RefreshToken> RefreshTokens { get; set; }
        internal DbSet<Plan> Plans { get; set; }
        internal DbSet<PlanPricing> PlanPricings { get; set; }
        internal DbSet<PlanFeature> PlanFeatures { get; set; }
        internal DbSet<Feature> Features { get; set; }
        internal DbSet<Tenant> Tenants { get; set; }
        internal DbSet<TenantRole> TenantRoles { get; set; }
        internal DbSet<TenantMember> TenantMembers { get; set; }
        internal DbSet<Grade> Grades { get; set; }
        internal DbSet<TeachingLevel> TeachingLevels { get; set; }
        internal DbSet<Subject> Subjects { get; set; }
        internal DbSet<Permission> Permissions { get; set; }
        internal DbSet<Domain.Entites.File> Files { get; set; }
        internal DbSet<RolePermission> RolePermissions { get; set; }
        internal DbSet<Subscription> Subscriptions { get; set; }
        internal DbSet<Course> Courses { get; set; }
        internal DbSet<Student> Students { get; set; }
        internal DbSet<Enrollment> Enrollments { get; set; }
        internal DbSet<CourseProgress> CourseProgresses { get; set; }
        internal DbSet<TenantUsage> TenantUsage { get; set; }
        internal DbSet<TenantInvite> TenantInvites { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        override public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditable && e.State == EntityState.Modified);
            foreach (var entityEntry in entries)
            {
                ((IAuditable)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
