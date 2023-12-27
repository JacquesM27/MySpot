
namespace MySpot.Infrastructure.DAL
{
    internal sealed class PostgresUnitOfWork(MySpotDbContext dbContext)
        : IUnitOfWork
    {
        public async Task ExecuteAsync(Func<Task> action)
        {
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                await action();
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
