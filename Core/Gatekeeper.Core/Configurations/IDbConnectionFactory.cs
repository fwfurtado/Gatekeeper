using System.Data;

namespace Gatekeeper.Core.Configurations;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}