using IBM.Data.DB2.iSeries;
using MVC.Models.System;
using MVC.Utils;
using System.Collections.Generic;
using System.Data;

namespace MVC.Daos.Db2Impl
{
    public class UserDaoImpl : BaseDaoImpl<User>, UserDao
    {
        public void create(User user)
        {
            string sql = "INSERT INTO \"USER\"(USER_NO,USERNAME,PASSWORD) VALUES(@USER_NO,@USERNAME,@PASSWORD)";
            using (iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString))
            {
                conn.Open();
                long userNo = DB2Helper.GetSequence(conn, "USER");
                iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@USER_NO", userNo),
                    new iDB2Parameter("@USERNAME", user.username),
                    new iDB2Parameter("@PASSWORD", user.password)};
                DB2Helper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }

        public void delete(string username)
        {
            string sql = "DELETE FROM \"USER\" WHERE USERNAME=@USERNAME";
            using (iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString))
            {
                conn.Open();
                iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@USERNAME", username)};
                DB2Helper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }

        public List<User> findAll()
        {
            string sql = "SELECT USER_NO,USERNAME,PASSWORD FROM \"USER\" ORDER BY USERNAME";
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql,null))
            {
                if (rdr.Read())
                {
                    List<User> list = new List<User>();
                    do
                    {
                        User user = new User();
                        user.userNo = rdr.GetInt32(0);
                        user.username = rdr.GetString(1);
                        user.password = rdr.GetString(2);
                        list.Add(user);
                    } while (rdr.Read());
                    return list;
                }
                else
                {
                    return null;
                }
            }
        }

        public User get(string username)
        {
            string sql = "SELECT USER_NO,USERNAME,PASSWORD FROM \"USER\" WHERE USERNAME=@USERNAME";
            iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@USERNAME", username)};
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, parms))
            {
                if (rdr.Read())
                {
                    User user = new User();
                    user.userNo = rdr.GetInt32(0);
                    user.username = rdr.GetString(1);
                    user.password = rdr.GetString(2);
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public void update(User user)
        {
            string sql = "UPDATE \"USER\" SET PASSWORD=@PASSWORD WHERE USERNAME=@USERNAME";
            using (iDB2Connection conn = new iDB2Connection(DB2Helper.ConnectionString))
            {
                conn.Open();
                iDB2Parameter[] parms = new iDB2Parameter[] {
                    new iDB2Parameter("@PASSWORD", user.password),
                    new iDB2Parameter("@USERNAME", user.username)};
                DB2Helper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }
    }
}