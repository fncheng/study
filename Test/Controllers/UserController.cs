using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using Test.Helper;
using System.Xml;

namespace Test.Controllers
{
    public class UserController : Controller
    {

        public const string constr = @"Data Source=SANDBOXV2DEV142;Initial Catalog=UserInfo;Integrated Security=True";
        public static List<User> regionUserInfoList { get; set; }
        // GET: User
        #region Index
        public ActionResult Index()//Index
        {
            List<User> userInfoList = new List<User>();//new a list to storing data
            var userInfoListCacheKey = "cache_2018-12-28_userinfoList";

            if (CacheHelper.GetCache(userInfoListCacheKey) != null)
            {
                userInfoList = (List<User>)CacheHelper.GetCache(userInfoListCacheKey);
            }
            else
            {
                string text = "userinfo_Get"; //name of Stored procedure
                SqlConnection conn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(text, conn);
                cmd.CommandType = CommandType.StoredProcedure;//Declaring that you want to execute is a stored procedure
                conn.Open();    //Open the sql
                SqlDataAdapter da = new SqlDataAdapter(text, conn);
                DataSet ds = new DataSet("Students");//Dataset contains a lot of table
                da.Fill(ds);
                //ds.WriteXml("write.xml", XmlWriteMode.WriteSchema);

                DataTable dt = ds.Tables[0];
                
                ToXml(dt);
                string xmlString = ToString();
                xmlString = xmlString.Replace("<DocumentElement>", "<Table>").Replace("</DocumentElement>", "</Table>");  //替换
                foreach (DataRow row in dt.Rows)
                {
                    User user = new Models.User();
                    user.Id = Convert.ToInt16(row["Id"]);
                    user.Name = Convert.ToString(row["Name"]);
                    user.Sex = Convert.ToString(row["Sex"]);
                    user.Age = Convert.ToInt16(row["Age"]);

                    userInfoList.Add(user);
                }

                conn.Close();
            }


            regionUserInfoList = userInfoList;

            return View(userInfoList);
        } 
        #endregion

        #region AddInfo
        public ActionResult AddInfo()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddInfo1()
        {
            string text = "userinfo_add";
            var test = Request.Form["UserName"].ToString();
            SqlParameter[] addpara =
            {
                new SqlParameter("@name",Request.Form["UserName"].ToString()),
                new SqlParameter("@sex",Request.Form["UserSex"].ToString()),
                new SqlParameter("@age",Convert.ToInt16(Request.Form["UserAge"]))
            };

            SqlConnection conn = new SqlConnection(constr);
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
            return View();
        }
        #endregion

        //public ActionResult DelInfo(int id)
        //{
        //    //ViewBag.UserId = id;
        //    return View();
        //}
        #region DelInfo
        public ActionResult DelInfo1(int id)
        {
            string text = "userinfo_del";
            var test = id;
            SqlParameter[] delpara = {
                new SqlParameter("@id",id)
            };
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(text, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in delpara)
            {
                cmd.Parameters.Add(parameter);
            }
            conn.Open();    //Open the sql
            cmd.ExecuteNonQuery();
            conn.Close();

            return View();
        }
        #endregion

        #region UpdateInfo
        public ActionResult UpdateInfo(int id)
        {
            User user = new User();

            if (regionUserInfoList.Count > 0)
            {
                user = regionUserInfoList.Where(x => x.Id == id).FirstOrDefault();
            }
            else {
                //if region user information list is null, then get the user from database
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult UpdateInfo1(int id)
        {
            string text = "userinfo_update";
            var test = Convert.ToString(Request.Form["UserName"]);
            SqlParameter[] updatepara =
            {
                new SqlParameter("@id",id),
                new SqlParameter("@name",Request.Form["UserName"].ToString()),
                new SqlParameter("@sex",Request.Form["UserSex"].ToString()),
                new SqlParameter("@age",Convert.ToInt16(Request.Form["UserAge"]))
            };
            //updatepara[0].Value=

            SqlConnection conn = new SqlConnection(constr);
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

            return View();
        }
        #endregion

        #region SelInfo
        [HttpPost]
        public ActionResult SelInfo()
        {
            string spName = "userinfo_sel";
            var test = Request.Form["SelInfo"].ToString();
            SqlParameter[] selpara =
            {
                new SqlParameter("@name",Request.Form["SelInfo"].ToString())
            };
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(spName, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            conn.Open();    //Open the sql
            foreach (SqlParameter parameter in selpara)
            {
                cmd.Parameters.Add(parameter);
            }
            ////
            //XmlReader reader = new XmlReader();
            //reader.Read();
            //while(!reader.EOF)
            //{
            //    a += reader.ReadOuterXml();
            //}
            //reader.Close();
            //return a;
            ////
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
            return View(list);
        }
        #endregion
        public  static void ToXml(DataTable dt)
        {
            dt.WriteXml(@"D:\abk.xml");//以xml形式写入DataTable中的内容并保存在ck.xml文件中
        }

        public static void ReadXmlByDataSet()
        {
            DataSet ds = new DataSet();
            ds.GetXml(@"D:\abk.xml");
        }
    }

}