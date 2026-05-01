using Npgsql;

namespace DemoProject.Domain.Interfaces
{
    public interface IConnectionFactory
    {
        Task<NpgsqlConnection> CreateAsync();

        Task CloseConnection();
    }
}
