namespace Application.Contracts.Repositories
{
    public interface IFileRepository
    {
        Task CreateAsync(Domain.Entites.File file, CancellationToken cancellationToken);
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
