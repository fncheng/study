using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Test.Models;

namespace Test.Helper
{
    public class SqlHelper
    {
        public const string constr = @"Data Source=.;Initial Catalog=UserInfo;User ID=dongcheng;Password=Aa336699";
        SqlConnection conn = new SqlConnection(constr);

        //Add
        public void AddHelper(string test1, string test2, int test3)
        {
            string text = "userinfo_add";
            SqlParameter[] addpara =
            {
                new SqlParameter("@name",test1),
                new SqlParameter("@sex",test2),
                new SqlParameter("@age",test3)
            };
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in addpara)
            {
                if (parameter != null)
                    cmd.Parameters.Add(parameter);
                else
                    cmd.Parameters.Add(0);
            }
            conn.Open();    //Open the sql

            cmd.ExecuteNonQuery();//
            conn.Close();
        }

        //Delete
        public void DelHelper(int id)
        {
            string text = "userinfo_del";
            SqlParameter[] delpara = {
                new SqlParameter("@id",id)
            };
            conn.Open();    //Open the sql
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in delpara)
            {
                cmd.Parameters.Add(parameter);
            }

            cmd.ExecuteNonQuery();
        }

        //Update
        public void UpdateHelper(int test, string test1, string test2, int test3)
        {
            string text = "userinfo_update";
            SqlParameter[] updatepara =
            {
                new SqlParameter("@id",test),
                new SqlParameter("@name",test1),
                new SqlParameter("@sex",test2),
                new SqlParameter("@age",test3)
            };
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in updatepara)
            {
                if (parameter != null)
                    cmd.Parameters.Add(parameter);
                else
                    continue;
            }
            conn.Open();    //Open the sql
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        //Get
        public List<User> SelHelper(string test)
        {
            //string spName = "Select * from people where name like '王'";
            string spName = "userinfo_sel";
            SqlParameter[] selpara =
            {
                new SqlParameter("@name",test)
            };
            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();    //Open the sql
            foreach (SqlParameter parameter in selpara)
            {
                cmd.Parameters.Add(parameter);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> list = new List<User>();
            while (reader.Read())
            {
                User user = new User()
                {
                    Name = reader["Name"].ToString(),
                    Sex = reader["Sex"].ToString(),
                    Age = Convert.ToInt16(reader["Age"])
                };
                list.Add(user);
            }
            return list;
        }
    }
}