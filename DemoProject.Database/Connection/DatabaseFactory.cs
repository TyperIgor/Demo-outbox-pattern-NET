
using DemoProject.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DemoProject.Database.Connection
{
    internal class DatabaseFactory(IOptions<DatabaseSettings> settings) : IConnectionFactory
    {

        readonly NpgsqlConnection _connection = new(settings.Value.Host);

        public async Task CloseConnection()
        {

            try
            {
                await _connection.CloseAsync();

            }
            catch (Exception)
            {
                await _connection.DisposeAsync();

                throw;
            }
        }

        public async Task<NpgsqlConnection> CreateAsync()
        {

            try
            {
                await _connection.OpenAsync();

                return _connection;

            }
            catch (Exception)
            {
                NpgsqlConnection.ClearPool(_connection);

                await _connection.DisposeAsync();

                throw;
            }
        }
    }
}
