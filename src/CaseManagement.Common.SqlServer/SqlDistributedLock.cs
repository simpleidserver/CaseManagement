using CaseManagement.Common.Lock;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace CaseManagement.Common.SqlServer
{
    public class SqlDistributedLock : IDistributedLock
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public SqlDistributedLock(IOptions<SqlDistributedLockOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<bool> TryAcquireLock(string id, CancellationToken token)
        {
            _connection = new SqlConnection(_connectionString);
            try
            {
                await _connection.OpenAsync();
            }
            catch
            {
                _connection.Dispose();
                return false;
            }
            using (var command = CreateAcquireCommand(_connection, id, 10, out var returnValue))
            {
                try
                {
                    command.ExecuteNonQuery();
                    var b = ParseExitCode((int)returnValue.Value);
                    if (!b)
                    {
                        _connection.Dispose();
                        return false;
                    }

                    return true;
                }
                catch
                {
                    _connection.Dispose();
                    return false;
                }
            }
        }

        public Task ReleaseLock(string id, CancellationToken token)
        {
            using (var command = CreateReleaseCommand(_connection, id, out var returnValue))
            {
                command.ExecuteNonQuery();
            }

            _connection.Dispose();
            return Task.CompletedTask;
        }

        private static IDbCommand CreateAcquireCommand(SqlConnection connection, string lockName, int timeoutMillis, out IDbDataParameter returnValue)
        {
            var command = connection.CreateCommand();
            command.CommandText = "dbo.sp_getapplock";
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = timeoutMillis >= 0 ? (timeoutMillis / 1000): 0;
            command.Parameters.Add(new SqlParameter("Resource", lockName));
            command.Parameters.Add(new SqlParameter("LockMode", "Exclusive"));
            command.Parameters.Add(new SqlParameter("LockOwner", "Session"));
            command.Parameters.Add(new SqlParameter("LockTimeout", 1));
            returnValue = command.CreateParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);
            return command;
        }

        private static IDbCommand CreateReleaseCommand(SqlConnection connection, string lockName, out IDbDataParameter returnValue)
        {
            var command = connection.CreateCommand();
            command.CommandText = "dbo.sp_releaseapplock";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("Resource", lockName));
            command.Parameters.Add(new SqlParameter("LockOwner", "Session"));
            returnValue = command.CreateParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);
            return command;
        }

        public static bool ParseExitCode(int exitCode)
        {
            switch (exitCode)
            {
                case 0:
                case 1:
                    return true;

                case -1:
                    return false;

                case -2:
                    return false;
                case -3:
                    return false;
                case -999:
                    return false;
                default:
                    if (exitCode <= 0) return false;
                    return true;
            }
        }
    }
}