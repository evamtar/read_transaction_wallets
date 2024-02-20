using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;
using SyncronizationBot.Domain.Model.Database.Base;
using SyncronizationBot.Domain.Model.Enum;
using System.Reflection;

namespace SyncronizationBot.Infra.Data.Base.Mapper
{
    public abstract class BaseMapper<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        private readonly EDatabase _database;
        public BaseMapper(EDatabase database)
        {
            this._database = database;
        }

        /// <summary>
        /// For override to person keys
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AddKeys(EntityTypeBuilder<T> builder) 
        {
            switch (_database)
            {
                case EDatabase.SqlServer:
                    builder.Ignore(g => g.CachedId);
                    builder.HasKey(g => g.ID);
                    break;
                case EDatabase.Mongodb:
                    builder.Property(g => g.CachedId);
                    builder.HasKey(g => g.CachedId);
                    break;
                default:
                    break;
            }
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
                    builder.ToTable(typeof(T).Name);
                    break;
                default:
                    break;
            }
        }
        protected virtual void PropertiesWithConversion(EntityTypeBuilder<T> builder) { }
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            this.AddSchema(builder);
            this.PropertiesWithConversion(builder);
            var properties = Activator.CreateInstance<T>().GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties) 
            {
                var propertyType = property.PropertyType;
                if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                    propertyType = Nullable.GetUnderlyingType(property.PropertyType);
                var exists = builder.Metadata.GetProperties().FirstOrDefault(p => p.Name == property.Name);
                if (exists!=null)
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
            this.AddKeys(builder);
        }
    }
}
