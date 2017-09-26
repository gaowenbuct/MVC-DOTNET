using IBM.Data.DB2.iSeries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Utils.Tests
{
    [TestClass()]
    public class DB2HelperTests
    {
        public static Object DB2Select()
        {
            iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString);
            string sql = "SELECT VALUE FROM SEQ_TABLE WHERE KEY=@KEY FOR UPDATE";

            iDB2Command cmd = new iDB2Command();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new iDB2Parameter("@KEY", "USER"));
            object val = cmd.ExecuteScalar();
            return val;
        }

        public static Object DB2Select1()
        {
            iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString);
            string sql = "SELECT VALUE FROM SEQ_TABLE WHERE KEY=@KEY FOR UPDATE";

            iDB2Command cmd = new iDB2Command();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new iDB2Parameter("@KEY", "USER"));
            object val = cmd.ExecuteScalar();
            return val;
        }
        
        public void GetSequenceTest()
        {
            long id;
            using (iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString))
            {
                id = DB2Helper.GetSequence(conn, "USER");
            }
            Assert.AreNotEqual(id, 0);
        }
        [TestMethod()]
        public void GetSequenceTransTest()
        {
            long id;
            using (iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString))
            {
                conn.Open();
                id = DB2Helper.GetSequenceTrans(conn, "USER");
            }
            Assert.AreNotEqual(id, 0);
        }
    }
}