using System.Data;
using System.Globalization;
using Dapper;

namespace Gatekeeper.Core.Configurations;

public static class DapperConfiguration {
    public static void Configure()
    {
        SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
    }
}


internal class SqlTimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override void SetValue(IDbDataParameter parameter, TimeOnly time)
    {
        parameter.DbType = DbType.Time;
        parameter.Value = time;
    }

    public override TimeOnly Parse(object value)
    {
        return TimeOnly.FromDateTime((DateTime)value);
    }
}

internal class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = date;
    }

    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }
}   