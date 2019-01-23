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

        public const string constr = @"Data Source=.;Initial Catalog=UserInfo;User ID=dongcheng;Password=Aa336699";
        public static List<User> regionUserInfoList { get; set; }

        SqlHelper sqlhelp = new SqlHelper();

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
                string text = "userinfo_get"; //name of Stored procedure
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
            var test1 = Convert.ToString(Request.Form["UserName"]);
            var test2 = Convert.ToString(Request.Form["UserSex"]);
            var test3 = Convert.ToInt16(Request.Form["UserAge"]);
            sqlhelp.AddHelper(test1, test2, test3);
            return View();
        }
        #endregion

        #region DelInfo
        public ActionResult DelInfo1(int id)
        {
            var test = id;
            sqlhelp.DelHelper(test);
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
            var test0 = id;
            var test1 = Convert.ToString(Request.Form["UserName"]);
            var test2 = Convert.ToString(Request.Form["UserSex"]);
            var test3 = Convert.ToInt16(Request.Form["UserAge"]);
            sqlhelp.UpdateHelper(test0, test1, test2, test3);
            return View();
        }
        #endregion

        #region SelInfo
        [HttpPost]
        public ActionResult SelInfo()
        {
            var test = Request.Form["SelInfo"].ToString();
            var sellist=sqlhelp.SelHelper(test);
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
            return View(sellist);
        }
        #endregion
        public  static void ToXml(DataTable dt)
        {
            dt.WriteXml(@"D:\abk.xml");//以xml形式写入DataTable中的内容并保存在ck.xml文件中
        }

        public static void ReadXmlByDataSet()
        {
            DataSet ds = new DataSet();
        }
    }

    
}