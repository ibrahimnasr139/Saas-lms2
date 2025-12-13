using Application.Features.Users.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Infrastructure.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserTenantsDto>> GetTenantsAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.TenantMembers
                .AsNoTracking()
                .Where(tm => tm.UserId == userId && tm.IsActive)
                .ProjectTo<UserTenantsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}