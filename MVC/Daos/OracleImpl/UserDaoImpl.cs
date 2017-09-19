using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.System;
using MVC.Utils;
using System.Data;
//using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Client;

namespace MVC.Daos.OracleImpl
{
    public class UserDaoImpl : BaseDaoImpl<User>, UserDao
    {
        public void create(User user)
        {
            string sql = "INSERT INTO \"USER\"(USER_NO,USERNAME,PASSWORD) VALUES(:USER_NO,:USERNAME,:PASSWORD)";
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionString))
            {
                conn.Open();
                long userNo = OracleHelper.GetSequence(conn, "USER");
                OracleParameter[] parms = new OracleParameter[] {
                    new OracleParameter("USER_NO", userNo),
                    new OracleParameter("USERNAME", user.username),
                    new OracleParameter("PASSWORD", user.password)
                };
                OracleHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }

        public void delete(string username)
        {
            string sql = "DELETE FROM \"USER\" WHERE USERNAME=:USERNAME";
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionString))
            {
                conn.Open();
                OracleParameter[] parms = new OracleParameter[] {
                    new OracleParameter("USERNAME", username)
                };
                OracleHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }

        public List<User> findAll()
        {
            string sql = "SELECT USER_NO,USERNAME,PASSWORD FROM \"USER\" ORDER BY USERNAME";
            using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, sql))
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
            string sql = "SELECT USER_NO,USERNAME,PASSWORD FROM \"USER\" WHERE USERNAME=:USERNAME";
            OracleParameter[] parms = new OracleParameter[] {
                    new OracleParameter("USERNAME", username)
                };
            using (OracleDataReader rdr = OracleHelper.ExecuteReader(OracleHelper.ConnectionString, CommandType.Text, sql, parms))
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
            string sql = "UPDATE \"USER\" SET PASSWORD=:PASSWORD WHERE USERNAME=:USERNAME";
            using (OracleConnection conn = new OracleConnection(OracleHelper.ConnectionString))
            {
                conn.Open();
                OracleParameter[] parms = new OracleParameter[] {
                    new OracleParameter("PASSWORD", user.password),
                    new OracleParameter("USERNAME", user.username)
                };
                OracleHelper.ExecuteNonQuery(conn, CommandType.Text, sql, parms);
            }
        }
    }
}