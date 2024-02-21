using System.Data;
using Dapper;
using Gatekeeper.Rest.Configuration;

namespace Gatekeeper.Rest.Infra.Dapper;

public class JsonTypeHandler<T>(IJsonSerializer serializer) : SqlMapper.TypeHandler<Json<T>>
{
    public override void SetValue(IDbDataParameter parameter, Json<T>? value)
    {
        if (value is not null)
        {
            parameter.Value = serializer.Serialize(value.Value);
        }
    }

    public override Json<T>? Parse(object value)
    {
        if (value is string json)
        {
            return new Json<T>
            {
                Value = serializer.Deserialize<T>(json)!
            };
        }

        return null;
    }
}

public class Json<T>
{
    public required T Value { get; init; }
}
