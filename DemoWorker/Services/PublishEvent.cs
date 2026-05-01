using Dapper;
using DemoProject.Domain.Interfaces;
using DemoProject.Domain.Models;

namespace DemoWorker.Services
{
    public class PublishEvent(IConnectionFactory connectionFactory)
    {
        private readonly IConnectionFactory connectionFactory = connectionFactory;
        public async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                using var conn = await connectionFactory.CreateAsync();
                var sql = $"SELECT * FROM \"outbox_event\" WHERE \"processed_at\" IS NULL;";
                var events = await conn.QueryAsync<OutboxEvent>(sql);

                foreach (var @event in events)
                {
                    var updateSql = $"UPDATE \"outbox_event\" SET \"processed_at\" = @processedAt WHERE \"id\" = @id;";
                    await conn.ExecuteAsync(updateSql, new { processedAt = DateTime.UtcNow, id = @event.Id });
                }
            }
            catch (Exception)
            {
                await connectionFactory.CloseConnection();
                throw;
            }
        }
    }
}
