using SiteFuel.Exchange.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace SiteFuel.Exchange.Utilities
{
    public class SqlHelperMethods
    {
        private static Dictionary<Type, SqlDbType> TypeMapDToS = new Dictionary<Type, SqlDbType>
        {
            { typeof(string), SqlDbType.NVarChar },
            { typeof(char[]), SqlDbType.NVarChar },
            { typeof(byte), SqlDbType.TinyInt },
            { typeof(short), SqlDbType.SmallInt },
            { typeof(int), SqlDbType.Int },
            { typeof(long), SqlDbType.BigInt },
            { typeof(byte[]), SqlDbType.VarBinary },
            { typeof(bool), SqlDbType.Bit },
            { typeof(DateTime), SqlDbType.DateTime },
            { typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
            { typeof(decimal), SqlDbType.Money },
            { typeof(float), SqlDbType.Real },
            { typeof(double), SqlDbType.Float },
            { typeof(TimeSpan), SqlDbType.Time },
        };

        private static SqlDbType GetDbType(Type giveType)
        {
            // Allow nullable types to be handled
            giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

            if (TypeMapDToS.ContainsKey(giveType))
            {
                return TypeMapDToS[giveType];
            }

            throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
        }

        public static StoredProcedure GetStoredProcedure<TParam>(string spName, TParam param)
        {
            StoredProcedure result = new StoredProcedure();
            if (!string.IsNullOrEmpty(spName) && param != null)
            {
                Type paramType = param.GetType();
                PropertyInfo[] properties = paramType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(DataTableSearchModel))
                    {
                        var value = property.GetValue(param, null);

                        var dataTableSearchModel = value as DataTableSearchModel;
                        if (dataTableSearchModel != null)
                        {
                            result.Params.Add(new SqlParameter($"GlobalSearchText", GetDbType(typeof(string)))
                            {
                                Value = dataTableSearchModel.GlobalSearchText == null ? (object)DBNull.Value : dataTableSearchModel.GlobalSearchText
                            });
                            result.Params.Add(new SqlParameter($"SortId", GetDbType(typeof(int)))
                            {
                                Value = dataTableSearchModel.SortId
                            });
                            result.Params.Add(new SqlParameter($"SortDirection", GetDbType(typeof(string)))
                            {
                                Value = dataTableSearchModel.SortDirection
                            });

                            result.Params.Add(new SqlParameter($"PageSize", GetDbType(typeof(int)))
                            {
                                Value = dataTableSearchModel.PageSize
                            });
                            result.Params.Add(new SqlParameter($"PageNumber", GetDbType(typeof(int)))
                            {
                                Value = dataTableSearchModel.PageNumber
                            });
                            dataTableSearchModel.DataTableSearchValues.ForEach(x =>
                            result.Params.Add(new SqlParameter($"{x.Name}SearchTypes", SqlDbType.Structured)
                            {
                                Value = x.GetDataTable(),
                                TypeName = "dbo.SearchTypes"
                            }));
                        }
                    }
                    else
                        result.Params.Add(new SqlParameter($"{property.Name}", GetDbType(property.PropertyType))
                        {
                            Value = property.GetValue(param, null) ?? DBNull.Value
                        });
                }
                result.Query = $"{spName} {string.Join(",", result.Params.ToList().Select(s => $" @{s.ParameterName}"))}";
            }

            return result;
        }

        public static StoredProcedure AddOutParameters<TParam>(StoredProcedure input, TParam outParams)
        {
            if (outParams != null)
            {
                Type paramType = outParams.GetType();
                PropertyInfo[] properties = paramType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    input.Params.Add(new SqlParameter($"{property.Name}", GetDbType(property.PropertyType))
                    {
                        Value = property.GetValue(outParams, null),
                        Direction = ParameterDirection.Output
                    });
                }
            }

            return input;
        }
    }
}
