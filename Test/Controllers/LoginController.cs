using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using Test.Controllers;

namespace Test.Controllers
{
    public class LoginController : Controller
    {
        public const string constr = @"Data Source=.;Initial Catalog=UserInfo;User ID=dongcheng;Password=Aa336699";
        SqlConnection conn = new SqlConnection(constr);

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login1()
        {
            Session["login0"] = "no";
            var account = Convert.ToString(Request.Form["UserAccount"]);
            var password = Convert.ToString(Request.Form["UserPassword"]);
            string text = "userinfo_check";
            SqlParameter[] selpara =
            {
                new SqlParameter("@account",account)
            };
            SqlCommand cmd = new SqlCommand(text, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            conn.Open();    //Open the sql
            foreach (SqlParameter parameter in selpara)
            {
                cmd.Parameters.Add(parameter);
            }
            SqlDataReader reader = cmd.ExecuteReader();
            //List<Register> rg = new List<Register>();
            //while (reader.Read())
            //{
            //    Register register = new Register()
            //    {
            //        Account = reader["UserAccount"].ToString(),
            //        Password = reader["UserPassword"].ToString(),
            //    };
            //    rg.Add(register);
            //}
            reader.Read();
            Register register = new Register()
            {
                Account = reader["UserAccount"].ToString(),
                Password = reader["UserPassword"].ToString(),
            };
            if (register.Password == password)
                //Session["login0"] = "allow";
                return RedirectToAction("Index","User");
            else
                return View();
        }

        #region Register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register1()
        {
            string text = "userinfo_register";
            var account = Convert.ToString(Request.Form["UserAccount"]);
            var password = Convert.ToString(Request.Form["UserPassword"]);
            SqlParameter[] registerpara =
            {
                new SqlParameter("@account",account),
                new SqlParameter("@password",password)
            };
            SqlCommand cmd = new SqlCommand(text, conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            foreach (SqlParameter parameter in registerpara)
            {
                if (parameter != null)
                    cmd.Parameters.Add(parameter);
                else
                    cmd.Parameters.Add(0);
            }
            conn.Open();    //Open the sql
            cmd.ExecuteNonQuery();//
            conn.Close();
            return View();
        } 
        #endregion
    }
}