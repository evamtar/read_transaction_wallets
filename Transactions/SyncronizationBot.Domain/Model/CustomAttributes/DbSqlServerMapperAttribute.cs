

namespace SyncronizationBot.Domain.Model.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbSqlServerMapperAttribute : Attribute
    {
        private readonly SqlServerTarget _sqlServerTarget;
        private readonly Type? _convertionType;
        private readonly int _precision;
        private readonly int _scale;

        
        public DbSqlServerMapperAttribute(SqlServerTarget sqlServerTarget, Type? convertionType = null, int precision = 0, int scale = 0)
        {
            _sqlServerTarget = sqlServerTarget;
            _convertionType = convertionType;
            _precision = precision;
            _scale = scale;
        }

        public SqlServerTarget GetSqlServerTarget() => _sqlServerTarget;
        public Type? GetTypeConvertion() => _convertionType;
        public int GetPrecision() => _precision;
        public int GetScale() => _scale;
    }

    
    public enum SqlServerTarget
    {
        Ignore,
        Key,
        HasConvertion,
        HasPrecision
    }
}
