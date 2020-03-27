using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Nomina.API.Accessor.Modelo;


namespace Nomina.API.Accessor
{
    public abstract class DataAccessorBase : DataAccessor, IDisposable
    {

        #region Constantes
        private const string action_Insert = "Insert";
        private const string action_Update = "Update";
        private const string action_Delete = "Delete";
        private const string trace_Category = "Query Trace";

        private const string identity_P = "@Identity_P";
        private const string ultimaAct_P = "@UltimaAct_P";

        private const string query_GetUltimaAct = "\nSELECT [UltimaAct] FROM [{0}] WHERE [{1}] = {2}";
        private const string query_SetIdentity = "\nSET @Identity_P = Cast(SCOPE_IDENTITY() AS BIGINT)\n";
        private const string query_SetUltimaAct = "\nSET @UltimaAct_P = Cast((MIN_ACTIVE_ROWVERSION()) as bigint)\n";
        private const string query_SetMaxID = "\nSELECT {0} = ISNULL(MAX([{1}]),0) + 1 FROM [{2}]";
        private const string query_SetGuid = "\nSET NOCOUNT ON;\nDECLARE @NextSequentialId AS TABLE\n([Id] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL PRIMARY KEY CLUSTERED);\nDECLARE @NewSequentialId AS TABLE\n([Id] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL DEFAULT(NEWSEQUENTIALID()) PRIMARY KEY CLUSTERED([Id]),\nXxX CHAR(1) NOT NULL);\nINSERT INTO @NewSequentialId(XxX)\nOUTPUT inserted.Id INTO @NextSequentialId([Id])\nVALUES('X');\nSELECT {0} = ROWGUIDCOL\nFROM @NextSequentialId;\n";
        private const string query_SetNoLock = "\nSET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;\n";
        private const string query_RemoveNoLock = "\nSET TRANSACTION ISOLATION LEVEL READ COMMITTED;\n";
        #endregion

        #region Atributos
        private SqlBulkCopy bulkCopy;
        private static readonly SqlDbType[] tiposVarchar = new SqlDbType[] { SqlDbType.VarChar, SqlDbType.NVarChar, SqlDbType.Text, SqlDbType.NText };
        private static readonly SqlDbType[] tiposFecha = new SqlDbType[] { SqlDbType.Date, SqlDbType.DateTime, SqlDbType.Time, SqlDbType.SmallDateTime };
        #endregion

        #region Metodos Publicos
        public T ExecuteScalar<T>(string query, IEnumerable<IDbDataParameter> parameters)
        {
            return this.executeScalar<T>(query, parameters);
        }
        public void ExecuteNonQuery(string query, IEnumerable<IDbDataParameter> parameters)
        {
            this.executeNonQuery(query, parameters);
        }
        public IDbDataParameter CreateParameter(string nombre, object valor)
        {
            return base.DbManager.Parameter(string.Format("@{0}", nombre), valor);
        }
        public void SetNoLock()
        {
            this.executeNonQuery(query_SetNoLock, new IDbDataParameter[] { });
        }
        public void RemoveNoLock()
        {
            this.executeNonQuery(query_RemoveNoLock, new IDbDataParameter[] { });
        }

        public int CommandTimeout
        {
            get { return base.DbManager.Command.CommandTimeout; }
            set { base.DbManager.Command.CommandTimeout = value; }
        }
        public virtual void Dispose()
        {
            if (base.DbManager.Transaction == null)
            {
                base.DbManager.Connection.Close();
                base.DbManager.Connection.Dispose();
                base.DbManager.Close();
                base.DbManager.Dispose();
                /* LLAMAR EL GARBAGE COLLECTOR PARA DESCONECTAR DEL SERVIDOR LAS CONEXIONES PENDIENTES
                 * Y EVITAR LA EXCEPCION DEL MAXIMO DE CONEXIONES DEL SQL SERVER */
                GC.Collect();

                base.Dispose(base.DbManager);

                if (this.bulkCopy != null)
                {
                    ((IDisposable)this.bulkCopy).Dispose();
                }
            }
        }
        #endregion

        #region Metodos Overriden
        protected override string GetSpName(Type type, string actionName)
        {
            return actionName;
        }
        protected override IDbDataParameter[] PrepareParameters(DbManager db, object[] parameters)
        {
            foreach (object obj in parameters)
            {
                if (obj is IDbDataParameter)
                {
                    prepareParameters((IDbDataParameter)obj);
                }
                else if (obj is IDbDataParameter[])
                {
                    prepareParameters((IDbDataParameter[])obj);
                }
            }

            return base.PrepareParameters(db, parameters);
        }
        #endregion

        #region Metodos Select
        public T Select<T>(object primaryKey)
        {
            SqlQuery<T> query = new SqlQuery<T>(base.DbManager);
            return query.SelectByKey(primaryKey);
        }
        public List<T> Select<T>()
        {
            SqlQuery<T> query = new SqlQuery<T>(base.DbManager);
            return query.SelectAll();
        }
        public IQueryable<T> GetQueryable<T>() where T : class
        {
            return base.DbManager.GetTable<T>();
        }
        public IQueryable<T> NoLock<T>() where T : class
        {
            return new DataAccessorFunctions(base.DbManager).NoLock<T>();
        }

        public DataSet Select(string query, IEnumerable<IDbDataParameter> parameters)
        {
            return this.executeQuery(query, parameters);
        }
        #endregion
        #region Metodos Insert
        public void Insert<T>(T item)
        {
            base.DbManager.Insert<T>(item);
        }
        public void Insert<T>(IEnumerable<T> list)
        {
            base.DbManager.InsertBatch<T>(list);
        }
        public void Insert<T>(T item, out int identity)
        {
            object value = base.DbManager.InsertWithIdentity<T>(item);
            int.TryParse(value.ToString(), out identity);
        }

        public void Insert<T>(T item, out long maxId)
        {
            this.insert<T>(item, out maxId);
        }

        public void Insert<T>(T item, out Guid guid)
        {
            this.insert<T>(item, out guid);
        }

        public void BulkIsert(string tableName, DataTable table)
        {
            SqlConnection connection = (SqlConnection)base.DbManager.Connection;
            this.bulkCopy = new SqlBulkCopy(connection);
            this.bulkCopy.DestinationTableName = tableName;
            this.bulkCopy.WriteToServer(table);
        }

        public void BulkIsert(string tableName, DataTable table, IDbTransaction transaction)
        {
            SqlConnection connection = (SqlConnection)base.DbManager.Connection;
            this.bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, (SqlTransaction)transaction);
            this.bulkCopy.DestinationTableName = tableName;
            this.bulkCopy.WriteToServer(table);
        }

        public void ExecuteMultiInsert(string TableName, List<Dictionary<string, string>> valores, string PK, long PKValueInicial, bool IsIdentity)
        {
            StringBuilder query = new StringBuilder();
            //if (IsIdentity) {
            //    query.AppendFormat("SET IDENTITY_INSERT [dbo].[{0}] ON",TableName);
            //    query.AppendLine();
            //}
            foreach (var item in valores)
            {
                PKValueInicial++;
                query.AppendLine(GetQueryInsert(TableName, item, PK, PKValueInicial.ToString(), IsIdentity));
            }
            //if (IsIdentity)
            //{                
            //    query.AppendFormat("SET IDENTITY_INSERT [dbo].[{0}] OFF", TableName);
            //}
            Trace.Write(query.ToString());
            base.DbManager.SetCommand(query.ToString()).ExecuteNonQuery();

        }

        #endregion

        #region Metodos Update
        public void Update<T>(T item)
        {
            base.DbManager.Update<T>(item);
        }
        public void Update<T>(IList<T> list)
        {
            base.DbManager.Update<T>(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">nombre de la tabla que es el modelo</param>
        /// <param name="list"> 
        /// es un modelo con las 
        /// propiedades
        /// string Key, 
        /// string Value, 
        /// strig TipoDato(Int,Bool,DateTime(long),Long,Decimal,Float,Short,)) 
        /// string Where
        /// con los nombres de las columnas como la entidad
        /// </param>
        /// <param name="transaction"></param>
        public void Update<T>(string tableName, IList<T> data, IDbTransaction transaction)
        {
            SqlConnection connection = (SqlConnection)base.DbManager.Connection;
            SqlCommand SqlCommandAction = connection.CreateCommand();
            SqlCommandAction.Transaction = (SqlTransaction)transaction;
            SqlCommandAction.CommandText = "";

            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            //se genera para recorrer el numero d epropiedades para generar el comando
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //***para no mapear la columna que no corresponda en la tabla asi se valida una propiedad que no esta en la tabla                
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            string CadenaFormada = "";
            object[] values = new object[table.Columns.Count];
            SqlQueryModelo sqlQueryModelos = new SqlQueryModelo();
            foreach (T item in data)
            {
                string Propiedades = "";
                string Where = "";

                Propiedades = props[0].GetValue(item).ToString();
                Where = props[1].GetValue(item).ToString();
                //Int,Bool,DateTime,Long,Decimal,Float,Short                              

                List<PropiedadEntidad> PropiedadesUpdate = JsonConvert.DeserializeObject<List<PropiedadEntidad>>(Propiedades);
                string SetCommand = "";
                foreach (PropiedadEntidad PU in PropiedadesUpdate)
                {
                    string Key = PU.Key;
                    string Value = PU.Value;
                    string TipoDato = PU.TipoDato;

                    switch (TipoDato)
                    {
                        case "Int":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, int.Parse(Value)) : string.Format(", {0} = {1} ", Key, int.Parse(Value));
                            break;
                        case "Bool":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, bool.Parse(Value) ? 1 : 0) : string.Format(", {0} = {1} ", Key, bool.Parse(Value) ? 1 : 0);
                            break;
                        case "DateTime":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = '{1}' ", Key, new DateTime(long.Parse(Value))) : string.Format(", {0} = CONVERT(DATETIME, {1} / 10000.0 / 1000 / 86400-693595) ", Key, long.Parse(Value));
                            break;
                        case "Long":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, long.Parse(Value)) : string.Format(", {0} = {1} ", Key, long.Parse(Value));
                            break;
                        case "Decimal":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, decimal.Parse(Value)) : string.Format(", {0} = {1} ", Key, decimal.Parse(Value));
                            break;
                        case "Float":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, float.Parse(Value)) : string.Format(", {0} = {1} ", Key, float.Parse(Value));
                            break;
                        case "Short":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = {1} ", Key, short.Parse(Value)) : string.Format(", {0} = {1} ", Key, short.Parse(Value));
                            break;
                        case "String":
                            SetCommand += SetCommand == "" ? string.Format(" {0} = '{1}' ", Key, Value) : string.Format(", {0} = '{1}' ", Key, Value);
                            break;
                        default:
                            SetCommand += "";
                            break;
                    }
                }

                sqlQueryModelos.SqlQueryList.Add(string.Format("UPDATE {0} SET {1} WHERE {2}; ", tableName, SetCommand, Where));

            }
            SqlCommandAction.CommandText = string.Join(Environment.NewLine, sqlQueryModelos.SqlQueryList);
            SqlCommandAction.ExecuteNonQuery();
        }
        #endregion

        #region Metodos Delete
        public void Delete<T>(T item)
        {
            base.DbManager.Delete<T>(item);
        }
        public void Delete<T>(IList<T> list)
        {
            base.DbManager.Delete<T>(list);
        }
        #endregion

        #region Metodos Privados

        private string GetQueryInsert(string TableName, Dictionary<string, string> valores, string PK, string PKvalue, bool IsIdentity)
        {
            string format = "INSERT INTO [dbo].[{0}] ({1}) VALUES({2})";
            string columnas = GeneraValores(valores, PK, IsIdentity, true);
            string values = GeneraValores(valores, PKvalue, IsIdentity, false);
            return string.Format(format, TableName, columnas, values);
        }



        private string GeneraValores(Dictionary<string, string> valores, string PKORValue, bool IsIdentity, bool isColumn)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in valores)
            {
                if (isColumn)
                {
                    sb.AppendFormat("{0}, ", key.Key);
                }
                else
                {
                    sb.AppendFormat("{0}, ", key.Value);
                }

            }
            if (IsIdentity)
            {
                var str = sb.ToString();
                str = str.Remove(str.Length - 2, 2);
                return str;
            }
            else
            {
                sb.AppendFormat("{0}", PKORValue);
            }

            return sb.ToString();
        }

        private void insert<T>(T item, out Guid guid)
        {
            SqlQueryInfo info = this.getSqlQueryInfo<T>(action_Insert);
            string guidParameterName = string.Format("@{0}_P", getPrimaryKeyName(info));
            string queryText = string.Format(query_SetGuid, guidParameterName) + info.QueryText;

            List<IDbDataParameter> parameters = new List<IDbDataParameter>(info.GetParameters(base.DbManager, item));
            IDbDataParameter guidParam = parameters.Find(delegate (IDbDataParameter param)
            {
                return param.ParameterName == guidParameterName;
            });

            guidParam.Direction = ParameterDirection.InputOutput;

            this.executeNonQuery(queryText, parameters.ToArray());
            guid = new Guid(guidParam.Value.ToString());
        }

        private void insert<T>(T item, out long maxId)
        {
            SqlQueryInfo info = this.getSqlQueryInfo<T>(action_Insert);

            string primaryKeyName = getPrimaryKeyName(info);
            string maxIDParameterName = string.Format("@{0}_P", primaryKeyName);
            string tableName = base.GetTableName(typeof(T));
            string queryText = string.Format(query_SetMaxID, maxIDParameterName, primaryKeyName, tableName) + info.QueryText;

            List<IDbDataParameter> parameters = new List<IDbDataParameter>(info.GetParameters(base.DbManager, item));
            IDbDataParameter maxIdParam = parameters.Find(delegate (IDbDataParameter param)
            {
                return param.ParameterName == maxIDParameterName;
            });

            maxIdParam.Direction = ParameterDirection.InputOutput;

            this.executeNonQuery(queryText, parameters.ToArray());
            long.TryParse(maxIdParam.Value.ToString(), out maxId);
        }

        private SqlQueryInfo getSqlQueryInfo<T>(string actionName)
        {
            SqlQueryInfo info = new SqlQuery<T>(base.DbManager).GetSqlQueryInfo(base.DbManager, actionName);
            return info;
        }
        private void executeNonQuery(string queryText, IEnumerable<IDbDataParameter> parameters)
        {
            IDbDataParameter[] listaParametros = new List<IDbDataParameter>(parameters).ToArray();
            prepareParameters(listaParametros);
            traceQuery(queryText, parameters);
            base.DbManager.SetCommand(queryText, listaParametros).ExecuteNonQuery();
            traceOutParams(parameters);
        }
        private T executeScalar<T>(string queryText, IEnumerable<IDbDataParameter> parameters)
        {
            IDbDataParameter[] listaParametros = new List<IDbDataParameter>(parameters).ToArray();
            prepareParameters(listaParametros);
            traceQuery(queryText, parameters);
            T value = base.DbManager.SetCommand(queryText, listaParametros).ExecuteScalar<T>();
            traceOutParams(parameters);
            return value;
        }
        private DataSet executeQuery(string queryText, IEnumerable<IDbDataParameter> parameters)
        {
            IDbDataParameter[] listaParametros = new List<IDbDataParameter>(parameters).ToArray();
            prepareParameters(listaParametros);
            DataSet ds = base.DbManager.SetCommand(queryText, listaParametros).ExecuteDataSet();
            traceOutParams(parameters);
            return ds;
        }
        private static void traceQuery(string queryText, IEnumerable<IDbDataParameter> parameters)
        {
            Trace.WriteLine(new string('v', 80));
            traceInputParams(parameters);
            Trace.WriteLine(queryText.Trim(' ', '\n'));
        }
        private static void traceInputParams(IEnumerable<IDbDataParameter> parameters)
        {
            var query = from param in parameters
                        select new
                        {
                            Name = param.ParameterName,
                            Type = getSqlDBType(param.DbType),
                            Size = param.Size,
                            Value = getParamValue(param.Value)
                        };

            foreach (var param in query)
            {
                string format = "DECLARE {0} AS {1} = {3}";
                if (tiposVarchar.Contains(param.Type))
                {
                    format = "DECLARE {0} AS {1}({2}) = '{3}'";
                    if (param.Value is string && param.Value.ToString() == "null")
                    {
                        format = "DECLARE {0} AS {1}({2}) = {3}";
                    }
                }
                if (tiposFecha.Contains(param.Type))
                {
                    format = "DECLARE {0} AS {1} = '{3}'";
                    if (param.Value is string && param.Value.ToString() == "null")
                    {
                        format = "DECLARE {0} AS {1} = {3}";
                    }
                }
                Trace.WriteLine(string.Format(format, param.Name, param.Type, param.Size, param.Value));
            }
            if (query.Count() > 0)
            {
                Trace.WriteLine(string.Empty);
            }
        }
        private static void traceOutParams(IEnumerable<IDbDataParameter> parameters)
        {
            var query = from param in parameters
                        where param.Direction != ParameterDirection.Input
                        select new
                        {
                            Name = param.ParameterName,
                            Type = getSqlDBType(param.DbType),
                            Size = param.Size,
                            Value = getParamValue(param.Value)
                        };
            if (query.Count() > 0)
            {
                Trace.WriteLine(string.Empty);
            }

            foreach (var param in query)
            {
                string format = "{0} = {3}";
                if (tiposVarchar.Contains(param.Type) || tiposFecha.Contains(param.Type))
                {
                    format = "{0} ='{3}'";
                    if (param.Value is string && param.Value.ToString() == "null")
                    {
                        format = "{0} = {3}";
                    }
                }
                Trace.WriteLine(string.Format(format, param.Name, param.Type, param.Size, param.Value));
            }
            Trace.WriteLine(new string('^', 80));
        }
        private static object getParamValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return "null";
            }
            return value;
        }
        private static SqlDbType getSqlDBType(DbType dbType)
        {
            SqlDbType sqlDbType = SqlDbType.VarChar;
            try
            {
                SqlParameter param = new SqlParameter() { DbType = dbType };
                sqlDbType = param.SqlDbType;
            }
            catch (Exception)
            {
                sqlDbType = SqlDbType.VarChar;
            }
            return sqlDbType;
        }
        private static string getPrimaryKeyName(SqlQueryInfo info)
        {
            List<string> properties = (from property in info.ObjectType.GetProperties()
                                       from atributte in property.GetCustomAttributes(true)
                                       where atributte is PrimaryKeyAttribute
                                       select property.Name)
                                   .ToList<string>();

            if (properties == null || properties.Count < 1 || string.IsNullOrEmpty(properties[0]))
            {
                string message = string.Format("Error de Mapeo de clases: La clase [{0}], no tiene definida ninguna propiedad [PrimaryKey].", info.ObjectType.FullName);
                throw new MappingException(message);
            }
            if (properties.Count > 1)
            {
                string message = string.Format("Error de Mapeo de clases: La clase [{0}], tiene definida más de una propiedad [PrimaryKey].", info.ObjectType.FullName);
                throw new MappingException(message);
            }

            string propertyName = properties[0];
            string primaryKeyName = (from member in info.GetMemberMappers()
                                     where member.MemberName == propertyName
                                     select member.Name)
                                      .FirstOrDefault<string>();

            if (string.IsNullOrEmpty(primaryKeyName))
            {
                string message = string.Format("Error de Mapeo de clases: No se encontró la propiedad [{0}], de la clase [{1}] en el Query --> {2}.",
                    propertyName, info.ObjectType.FullName, info.QueryText);
                throw new MappingException(message);
            }
            return primaryKeyName;
        }
        private static void prepareParameters(params IDbDataParameter[] parameters)
        {
            foreach (IDbDataParameter parameter in parameters)
            {
                if (isDateTimeNull(parameter))
                {
                    parameter.Value = DBNull.Value;
                    parameter.DbType = DbType.DateTime;
                    continue;
                }
                if (isInt32Null(parameter))
                {
                    parameter.Value = DBNull.Value;
                    parameter.DbType = DbType.Int32;
                    continue;
                }
                if (isInt64Null(parameter))
                {
                    parameter.Value = DBNull.Value;
                    parameter.DbType = DbType.Int64;
                    continue;
                }
                if (isDecimalNull(parameter))
                {
                    parameter.Value = DBNull.Value;
                    parameter.DbType = DbType.Decimal;
                    continue;
                }
                if (isDoubleNull(parameter))
                {
                    parameter.Value = DBNull.Value;
                    parameter.DbType = DbType.Double;
                    continue;
                }
            }
        }
        private static bool isDateTimeNull(IDbDataParameter parameter)
        {
            DateTime myDateTime = DateTime.MinValue;
            return parameter.DbType == DbType.DateTime &&
                   parameter.Value != null &&
                   DateTime.TryParse(parameter.Value.ToString(), out myDateTime) &&
                   myDateTime == DateTime.MinValue;
        }
        private static bool isInt32Null(IDbDataParameter parameter)
        {
            Int32 myInt32 = Int32.MinValue;
            return parameter.DbType == DbType.Int32 &&
                   parameter.Value != null &&
                   Int32.TryParse(parameter.Value.ToString(), out myInt32) &&
                   myInt32 == Int32.MinValue;
        }
        private static bool isInt64Null(IDbDataParameter parameter)
        {
            Int64 myInt64 = Int64.MinValue;
            return parameter.DbType == DbType.Int64 &&
                   parameter.Value != null &&
                   Int64.TryParse(parameter.Value.ToString(), out myInt64) &&
                   myInt64 == Int64.MinValue;
        }
        private static bool isDecimalNull(IDbDataParameter parameter)
        {
            Decimal myDecimal = Decimal.MinValue;
            return parameter.DbType == DbType.Decimal &&
                   parameter.Value != null &&
                   Decimal.TryParse(parameter.Value.ToString(), out myDecimal) &&
                   myDecimal == Decimal.MinValue;
        }
        private static bool isDoubleNull(IDbDataParameter parameter)
        {
            Double myDouble = Double.MinValue;
            return parameter.DbType == DbType.Double &&
                   parameter.Value != null &&
                   Double.TryParse(parameter.Value.ToString(), out myDouble) &&
                   myDouble == Double.MinValue;
        }
        #endregion
    }
    public abstract class DataAccessorBase<T> : DataAccessorBase where T : class
    {

        #region Metodos Select
        public T Select(object primaryKey)
        {
            return base.Select<T>(primaryKey);
        }
        public IEnumerable<T> Select()
        {
            return base.Select<T>();
        }
        public IQueryable<T> GetQueryable()
        {
            return base.GetQueryable<T>();
        }
        public IQueryable<T> NoLock()
        {
            return base.NoLock<T>();
        }
        #endregion

        #region Metodos Insert
        public void Insert(T item)
        {
            base.Insert<T>(item);
        }
        public void Insert(IList<T> list)
        {
            base.Insert<T>(list);
        }
        public void Insert(T item, out int identity)
        {
            base.Insert<T>(item, out identity);
        }
        public void Insert(T item, out long maxId)
        {
            base.Insert<T>(item, out maxId);
        }
        public void Insert(T item, out Guid guid)
        {
            base.Insert<T>(item, out guid);
        }
        public void ExecuteMultiInsert(string TableName, List<Dictionary<string, string>> valores, string PK, long PKValueInicial, bool IsIdentity)
        {
            base.ExecuteMultiInsert(TableName, valores, PK, PKValueInicial, IsIdentity);
        }
        #endregion

        #region Metodos Update
        public void Update(T item)
        {
            base.Update<T>(item);
        }
        public void Update(List<T> list)
        {
            base.Update<T>(list);
        }

        #endregion

        #region Metodos Delete
        public void Delete(T item)
        {
            base.Delete<T>(item);
        }
        public void Delete(IList<T> list)
        {
            base.Delete<T>(list);
        }
        #endregion
    }

    public class DataAccessorFunctions
    {
        private readonly IDataContext _ctx;

        public DataAccessorFunctions(IDataContext ctx)
        {
            _ctx = ctx;
        }

        [TableExpression("{0} {1} WITH (TABLOCK)")]
        public Table<T> WithTabLock<T>() where T : class
        {
            return _ctx.GetTable<T>(this, ((MethodInfo)(MethodBase.GetCurrentMethod())).MakeGenericMethod(typeof(T)));
        }

        [TableExpression("{0} {1} (NOLOCK)")]
        public Table<T> NoLock<T>()
            where T : class
        {
            return _ctx.GetTable<T>(this, ((MethodInfo)(MethodBase.GetCurrentMethod())).MakeGenericMethod(typeof(T)));
        }

    }
}
