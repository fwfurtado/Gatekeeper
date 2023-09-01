using System.Data.Common;

namespace Gatekeeper.Core.Infra;

public interface IConnectionFactory
{
    DbConnection CreateConnection();
}