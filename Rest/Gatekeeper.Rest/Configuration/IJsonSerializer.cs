using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Gatekeeper.Rest.Configuration;

public interface IJsonSerializer
{
    string Serialize<T>(T value);
}

public class DefaultJsonSerializer(
    IOptions<JsonOptions> jsonOptions
) : IJsonSerializer
{
    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, jsonOptions.Value.SerializerOptions);
    }
}