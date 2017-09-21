using System;
using System.Configuration;
using System.Data;

using System.Collections;
using IBM.Data.DB2.iSeries;
using System.Text;

namespace MVC.Utils
{

    /// <summary>
    /// A helper class used to execute queries against an iDB2 database
    /// </summary>
    public abstract class DB2Helper
    {

        // Read the connection strings from the configuration file
        private static log4net.ILog log = log = log4net.LogManager.GetLogger(typeof(DB2Helper));
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Db2ConnString"].ConnectionString;

        //Create a hashtable for the parameter cached
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a database query which does not include a select
        /// </summary>
        /// <param name="connString">Connection string to database</param>
        /// <param name="cmdType">Command type either stored procedure or SQL</param>
        /// <param name="cmdText">Acutall SQL Command</param>
        /// <param name="commandParameters">Parameters to bind to the command</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {
            // Create a new iDB2 command
            iDB2Command cmd = new iDB2Command();

            //Create a connection
            using (iDB2Connection connection = new iDB2Connection(connectionString))
            {

                //Prepare the command
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                //Execute the command
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute an iDB2Command (that returns no resultset) against an existing database transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new iDB2Parameter(":prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of iDB2Paramters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(iDB2Transaction trans, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {
            iDB2Command cmd = new iDB2Command();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute an iDB2Command (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new iDB2Parameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of iDB2Paramters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(iDB2Connection connection, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {

            iDB2Command cmd = new iDB2Command();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a select query that will return a result set
        /// </summary>
        /// <param name="connString">Connection string</param>
        //// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of iDB2Paramters used to execute the command</param>
        /// <returns></returns>
        public static iDB2DataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {

            //Create the command and connection
            iDB2Command cmd = new iDB2Command();
            iDB2Connection conn = new iDB2Connection(connectionString);

            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                //Execute the query, stating that the connection should close when the resulting datareader has been read
                iDB2DataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;

            }
            catch(Exception ex)
            {

                //If an error occurs close the connection as the reader will not be used and we expect it to close the connection
                conn.Close();
                throw ex;
            }
        }

        /// <summary>
        /// Execute an iDB2Command that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new iDB2Parameter(":prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of iDB2Paramters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {
            iDB2Command cmd = new iDB2Command();

            using (iDB2Connection conn = new iDB2Connection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        ///	<summary>
        ///	Execute	a iDB2Command (that returns a 1x1 resultset)	against	the	specified SqlTransaction
        ///	using the provided parameters.
        ///	</summary>
        ///	<param name="transaction">A	valid SqlTransaction</param>
        ///	<param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        ///	<param name="commandText">The stored procedure name	or PL/SQL command</param>
        ///	<param name="commandParameters">An array of	iDB2Paramters used to execute the command</param>
        ///	<returns>An	object containing the value	in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(iDB2Transaction transaction, CommandType commandType, string commandText, params iDB2Parameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            // Create a	command	and	prepare	it for execution
            iDB2Command cmd = new iDB2Command();

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // Execute the command & return	the	results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters	from the command object, so	they can be	used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// Execute an iDB2Command that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new iDB2Parameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of iDB2Paramters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(iDB2Connection connectionString, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {
            iDB2Command cmd = new iDB2Command();

            PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Add a set of parameters to the cached
        /// </summary>
        /// <param name="cacheKey">Key value to look up the parameters</param>
        /// <param name="commandParameters">Actual parameters to cached</param>
        public static void CacheParameters(string cacheKey, params iDB2Parameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Fetch parameters from the cache
        /// </summary>
        /// <param name="cacheKey">Key to look up the parameters</param>
        /// <returns></returns>
        public static iDB2Parameter[] GetCachedParameters(string cacheKey)
        {
            iDB2Parameter[] cachedParms = (iDB2Parameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            // If the parameters are in the cache
            iDB2Parameter[] clonedParms = new iDB2Parameter[cachedParms.Length];

            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (iDB2Parameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="commandParameters">Parameters for the command</param>
        private static void PrepareCommand(iDB2Command cmd, iDB2Connection conn, iDB2Transaction trans, CommandType cmdType, string cmdText, params iDB2Parameter[] commandParameters)
        {

            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;

            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (iDB2Parameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
            /*if (commandParameters != null)
            {
                cmd.DeriveParameters();
                //cmd.Parameters["@myKEY"].Value = "USER_NO";
                foreach (var parm in commandParameters)
                    cmd.Parameters["@"+ parm.Key].Value = parm.Value;
            }*/
        }

        /// <summary>
        /// Converter to use boolean data type with iDB2
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string OraBit(bool value)
        {
            if (value)
                return "Y";
            else
                return "N";
        }

        /// <summary>
        /// Converter to use boolean data type with iDB2
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static bool OraBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            else
                return false;
        }

        public static long GetSequence(iDB2Connection conn, string tableName)
        {
            long id;
            string sql = "SELECT VALUE FROM SEQ_TABLE WHERE KEY=@KEY FOR UPDATE WITH RS";
            iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@KEY", "USER") };
            Object obj = ExecuteScalar(conn, CommandType.Text, sql, parms);
            if (obj != null)
            {
                id = Convert.ToInt32(obj);
                string sqlUpdate = "UPDATE SEQ_TABLE SET VALUE=@NEW_ID WHERE VALUE=@OLD_ID AND KEY=@KEY";
                iDB2Parameter[] parmsUpdate = new iDB2Parameter[] {
                    new iDB2Parameter("@NEW_ID", id + 1),
                    new iDB2Parameter("@OLD_ID", id),
                    new iDB2Parameter("@KEY", tableName)};
                ExecuteNonQuery(conn, CommandType.Text, sqlUpdate, parmsUpdate);
            }
            else
            {
                id = 1;
                string sqlInsert = "INSERT INTO SEQ_TABLE(KEY,VALUE) VALUES(@KEY,@VALUE)";
                iDB2Parameter[] parmsInsert = new iDB2Parameter[] {
                    new iDB2Parameter("@KEY", tableName),
                    new iDB2Parameter("@VALUE", id + 1) };
                ExecuteNonQuery(conn, CommandType.Text, sqlInsert, parmsInsert);
            }
            return id;
        }

        public static long GetSequenceTrans(iDB2Connection conn, string tableName)
        {
            //iDB2Transaction trans = conn.BeginTransaction(IsolationLevel.RepeatableRead);
            iDB2Transaction trans = conn.BeginTransaction();
            long id=0;
            try { 
            
            string sql = "SELECT VALUE FROM SEQ_TABLE WHERE KEY=@KEY";
            iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@KEY", "USER") };
            Object obj = ExecuteScalar(trans, CommandType.Text, sql, parms);
            if (obj != null)
            {
                id = Convert.ToInt32(obj);
                string sqlUpdate = "UPDATE SEQ_TABLE SET VALUE=@NEW_ID WHERE VALUE=@OLD_ID AND KEY=@KEY";
                iDB2Parameter[] parmsUpdate = new iDB2Parameter[] {
                    new iDB2Parameter("@NEW_ID", id + 1),
                    new iDB2Parameter("@OLD_ID", id),
                    new iDB2Parameter("@KEY", tableName)};
                ExecuteNonQuery(trans, CommandType.Text, sqlUpdate, parmsUpdate);
            }
            else
            {
                id = 1;
                string sqlInsert = "INSERT INTO SEQ_TABLE(KEY,VALUE) VALUES(@KEY,@VALUE)";
                iDB2Parameter[] parmsInsert = new iDB2Parameter[] {
                    new iDB2Parameter("@KEY", tableName),
                    new iDB2Parameter("@VALUE", id + 1) };
                ExecuteNonQuery(trans, CommandType.Text, sqlInsert, parmsInsert);
            }
            trans.Commit();
                return id;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                trans.Rollback();
                return 0;
            }
        }
        public static string GetPageSql(string selectFields,string orderField, string tableName,string condition,int startIndex,int pageSize)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM(SELECT ");
            sb.Append(selectFields);
            sb.Append(",ROW_NUMBER() OVER(ORDER BY ");
            sb.Append(orderField);
            sb.Append(" ASC) AS ROWNUM FROM ");
            sb.Append(tableName);
            sb.Append(" WHERE 1=1 ");
            sb.Append(condition);
            sb.Append(" FETCH FIRST ");
            sb.Append((startIndex+ pageSize).ToString());
            sb.Append(" ROWS ONLY) A WHERE ROWNUM>");
            sb.Append(startIndex.ToString());
            sb.Append(" AND ROWNUM<= ");
            sb.Append((startIndex + pageSize).ToString());
            return sb.ToString();
        }
        public static string GetCountSql(string tableName, string condition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(1) FROM ");
            sb.Append(tableName);
            sb.Append(" WHERE 1=1 ");
            sb.Append(condition);
            return sb.ToString();
        }
    }
}
