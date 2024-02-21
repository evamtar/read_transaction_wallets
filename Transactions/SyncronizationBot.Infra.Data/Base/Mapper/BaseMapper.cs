using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.CustomAttributes;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using System.Reflection;

namespace SyncronizationBot.Infra.Data.Base.Mapper
{
    public class BaseMapper<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        private readonly EDatabase _database;
        public BaseMapper(EDatabase database)
        {
            this._database = database;
        }

        /// <summary>
        /// To table, to view, to collectionList
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AddSchema(EntityTypeBuilder<T> builder)
        {
            switch (_database)
            {
                case EDatabase.SqlServer:
                    builder.ToTable(typeof(T).Name);
                    break;
                case EDatabase.Mongodb:
                    builder.ToCollection(typeof(T).Name);
                    break;
                default:
                    break;
            }
        }

        protected virtual void RelationsShips(EntityTypeBuilder<T> builder) { }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            this.AddSchema(builder);
            var properties = Activator.CreateInstance<T>().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties) 
            {
                var customAttribute = (DbMapperAttribute?) property.GetCustomAttribute(typeof(DbMapperAttribute));
                if (customAttribute != null)
                {
                    if (_database == EDatabase.Mongodb)
                        this.PropertyActionDo(builder, property, customAttribute.GetMongoTarget());
                    else if (_database == EDatabase.SqlServer)
                        this.PropertyActionDo(builder, property, customAttribute.GetSqlServerTarget(), customAttribute.GetTypeConvertion(), customAttribute.GetPrecision(), customAttribute.GetScale());
                }
                else 
                {
                    var propertyType = property.PropertyType;
                    if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        propertyType = Nullable.GetUnderlyingType(property.PropertyType);
                    var exists = builder.Metadata.GetProperties().FirstOrDefault(p => p.Name == property.Name);
                    if (exists != null)
                    {
                        switch (Type.GetTypeCode(propertyType))
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Char:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                            case TypeCode.DateTime:
                            case TypeCode.String:
                                builder.Property(property.Name);
                                break;
                            case TypeCode.Empty:
                            case TypeCode.Object:
                            case TypeCode.DBNull:
                            default:
                                if (property.PropertyType.IsPrimitive == true ||
                                    (property.PropertyType.IsGenericType && property.PropertyType == typeof(Guid?))
                                    || (property.PropertyType.IsGenericType && property.PropertyType == typeof(Guid)))
                                    builder.Property(property.Name);
                                break;
                        }
                    }
                }
                
            }
            this.RelationsShips(builder);
        }

        private void PropertyActionDo(EntityTypeBuilder<T> builder, PropertyInfo property, MongoTarget? target)
        {
            if (target != null) 
            {
                switch (target)
                {
                    case MongoTarget.Ignore:
                        builder.Ignore(property.Name);
                        break;
                    case MongoTarget.Key:
                        builder.Property(property.Name);
                        builder.HasKey(property.Name);
                        break;
                    default:
                        throw new ArgumentException("Not implemented MongoTarget");
                }
            }
        }

        private void PropertyActionDo(EntityTypeBuilder<T> builder, PropertyInfo property, SqlServerTarget? target, Type? convertionType, int precision = 0, int scale = 0) 
        {
            if (target != null)
            {
                switch (target)
                {
                    case SqlServerTarget.Ignore:
                        builder.Ignore(property.Name);
                        break;
                    case SqlServerTarget.Key:
                        builder.Property(property.Name);
                        builder.HasKey(property.Name);
                        break;
                    case SqlServerTarget.HasConvertion:
                        if (convertionType == typeof(string)) 
                        {
                            if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                                builder.Property(property.Name).HasConversion<string?>();
                            else
                                builder.Property(property.Name).HasConversion<string>();
                        }
                        break;
                    case SqlServerTarget.HasPrecision:
                        builder.Property(property.Name).HasPrecision(precision, scale);
                        break;
                    default:
                        throw new ArgumentException("Not implemented MongoTarget");
                }
            }
        }
    }
}
