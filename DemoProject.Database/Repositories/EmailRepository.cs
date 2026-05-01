
using DemoProject.Domain.Interfaces;
using DemoProject.Domain.Models;
using Dapper;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("DemoProject.DependencyInjection")]
namespace DemoProject.Database.Repositories
{
    internal class EmailRepository(IConnectionFactory connectionFactory) : IEmailRepository
    {
        private readonly IConnectionFactory _connectionFactory = connectionFactory;

        public async Task InsertEmail(EmailNotification email)
        {
            using var conn = await _connectionFactory.CreateAsync();
            using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var parameters = new
                {
                    id = email.Id,
                    email = email.Email,
                    subject = email.Subject,
                    body = email.Body
                };

                var sql1 = $"INSERT INTO \"emailnotification\" (\"id\", \"email\", \"subject\", \"body\") VALUES(@id, @email, @subject, @body);";

                await conn.ExecuteAsync(sql1, parameters);

                var parameters2 = new
                {
                    id = email.Id,
                    type = "EmailNotification",
                    payload = System.Text.Json.JsonSerializer.Serialize(email),
                    createdAt = DateTime.UtcNow,
                    processedAt = (DateTime?)null
                };

                var sql2 = $"INSERT INTO \"outbox_event\" "+
                                        "(\"id\", \"type\", \"payload\", \"created_at\", \"processed_at\")" +
                                         $"VALUES(@id, @type, @payload::jsonb, @createdAt, @processedAt);";

                await conn.ExecuteAsync(sql2, parameters2);


                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                await _connectionFactory.CloseConnection();

                throw;
            }

        }
    }
}
