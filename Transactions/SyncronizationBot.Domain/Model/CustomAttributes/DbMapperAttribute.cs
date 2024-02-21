

namespace SyncronizationBot.Domain.Model.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbMapperAttribute : Attribute
    {
        private readonly MongoTarget _mongoTarget;
        private readonly SqlServerTarget _sqlServerTarget;
        private readonly Type? _convertionType;
        private readonly int _precision;
        private readonly int _scale;


        public DbMapperAttribute(MongoTarget mongoTarget, Type? convertionType = null, int precision = 0, int scale = 0)
        {
            _mongoTarget = mongoTarget;
            _convertionType = convertionType;
            _precision = precision;
            _scale = scale;
        }

        public DbMapperAttribute(SqlServerTarget sqlServerTarget, Type? convertionType = null, int precision = 0, int scale = 0)
        {
            _sqlServerTarget = sqlServerTarget;
            _convertionType = convertionType;
            _precision = precision;
            _scale = scale;
        }

        public DbMapperAttribute(MongoTarget mongoTarget, SqlServerTarget sqlServerTarget, Type? convertionType = null, int precision = 0, int scale = 0)
        {
            _mongoTarget = mongoTarget;
            _sqlServerTarget = sqlServerTarget;
            _convertionType = convertionType;
            _precision = precision;
            _scale = scale;
        }

        public MongoTarget? GetMongoTarget() => _mongoTarget;
        public SqlServerTarget? GetSqlServerTarget() => _sqlServerTarget;
        public Type? GetTypeConvertion() => _convertionType;
        public int GetPrecision() => _precision;
        public int GetScale() => _scale;
    }

    public enum MongoTarget 
    { 
        Ignore,
        Key
    }
    public enum SqlServerTarget
    {
        Ignore,
        Key,
        HasConvertion,
        HasPrecision
    }
}
