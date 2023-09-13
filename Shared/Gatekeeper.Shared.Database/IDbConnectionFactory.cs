using System.Data;

namespace Gatekeeper.Shared.Database;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}