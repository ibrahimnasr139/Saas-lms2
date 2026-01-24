namespace Infrastructure.Repositories
{
    internal class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        public FileRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Domain.Entites.File file, CancellationToken cancellationToken)
        {
            await _context.Files.AddAsync(file, cancellationToken);
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
