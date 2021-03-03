using AndroidModel.douyin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SqliteWebApp
{
    public partial class _Default : Page
    {
        /// <summary>
        /// Sqlite数据库位置
        /// </summary>
        /// System.Environment.CurrentDirectory
        //static readonly string strCon = System.Environment.CurrentDirectory+ "/data.db"; //; Password=1111  // sqlite数据库连接字符串
        //static readonly string strCon = "G:/WCF/SqliteManagerWcfService/SqliteManagerWcfService/bin/Debug/data.db"; //; Password=1111  // sqlite数据库连接字符串
        //static readonly string strCon = System.Web.Hosting.HostingEnvironment.MapPath($"/bin") + @"/data.db";
        static readonly string strCon = System.Web.Hosting.HostingEnvironment.MapPath($"/Crawler") + @"/data.db";  //正确取路径的方法
        protected void Page_Load(object sender, EventArgs e)
        {
            // 关闭爬虫代理

            // 传输爬取到的数据
            List<CommentDataModel> comDataModels = new List<CommentDataModel>();
            try
            {
                // 初始化数据库链接字符串
                SQLiteHelper.Initial(strCon);
                // SQL参数生成
                StringBuilder sqldata_Tiktok = new StringBuilder();
                List<SQLiteParameter> ps = new List<SQLiteParameter>();

                sqldata_Tiktok.AppendLine("SELECT short_id,nickname,phone,gender,signature,share_url,comment_text,create_time,digg_count,up_time FROM comment_data");

                // 数据取得
                DataTable getComDatas = new DataTable();
                getComDatas = SQLiteHelper.Select(sqldata_Tiktok.ToString());

                foreach (DataRow dataRow in getComDatas.Rows)
                {
                    CommentDataModel comDataModel = new CommentDataModel();
                    comDataModel.Number = (comDataModels.Count + 1).ToString();  // 序号
                    comDataModel.ShortId = dataRow[0].ToString();      // 短ID
                    comDataModel.NickName = dataRow[1].ToString();     // 昵称
                    comDataModel.Phone = dataRow[2].ToString();        // 手机号
                    comDataModel.Gender = dataRow[3].ToString();       // 性别
                    comDataModel.Signature = dataRow[4].ToString();    // 签名
                    comDataModel.ShareUrl = dataRow[5].ToString();     // 用户主页Url
                    comDataModel.CommentText = dataRow[6].ToString();  // 评论内容
                    comDataModel.CreateTime = dataRow[7].ToString();   // 评论时间
                    comDataModel.DiggCount = dataRow[8].ToString();    // 本条点赞数
                    comDataModel.UpTime = dataRow[9].ToString();       // 采集时间
                    comDataModels.Add(comDataModel);
                }
            }
            catch { }
            string Message = JsonConvert.SerializeObject(comDataModels); //序列化
            TextBox1.Text = Message;
        }
    }
}