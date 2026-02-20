namespace Application.Common.Interfaces
{
    // Интерфейс контекста содержит только операции сохранения.
    // Доступ к данным осуществляется через репозитории.
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
