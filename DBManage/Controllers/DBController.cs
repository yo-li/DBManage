using DBManage.Models;
using Newtonsoft.Json;
using Novacode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBManage.Controllers
{
    public class DBController : Controller
    {
        //
        // GET: /DB/

        /// <summary>
        /// 表列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            DataTable dt = DBManage.SqlDbHelper.ExecuteDataTable("select * from db_talbe_list order by tableName");
            string Json = DataTableConvertJson.DataTable2Json(dt);
            ViewBag.PageData = Json;
            return View();
        }


        public ActionResult Detail(string TableName)
        {
            DataTable dt = DBManage.SqlDbHelper.ExecuteDataTable(string.Format("select * from db_talbe_columns where tableName='{0}'", TableName.Replace("'","''")));
           
          //  string Json =DataTableConvertJson.DataTable2Json(dt);
            ViewBag.PageData = JsonConvert.SerializeObject(dt); ;// Json;
            ViewBag.Title = TableName + "详情";
            ViewBag.TableName = TableName;
            return View();
        }


        public JsonResult AddTableDatil(string TableName, string Detail)
        {
            ReturnResult result = new ReturnResult();
            string conn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlconn;
            cmd.CommandText = "db_detail_add";
            cmd.CommandType = CommandType.StoredProcedure;

            // 创建参数
            IDataParameter[] parameters = {
                new SqlParameter("@Result", SqlDbType.Int,4) ,
                new SqlParameter("@TableName", SqlDbType.NVarChar,50) ,
                new SqlParameter("@ColumnName", SqlDbType.NVarChar,50) ,
                new SqlParameter("@DaTail", SqlDbType.NVarChar,300) ,
                new SqlParameter("@TypeId", SqlDbType.Int,4) 
            };
            // 设置参数类型
            parameters[0].Direction = ParameterDirection.Output;  // 设置为输出参数
            parameters[1].Value = TableName;
            parameters[2].Value = "";
            parameters[3].Value = Detail;
            parameters[4].Value =1;




            // 添加参数
            cmd.Parameters.AddRange(parameters);

            sqlconn.Open();
            // 执行存储过程并返回影响的行数
            string count = cmd.ExecuteNonQuery().ToString();
            sqlconn.Close();
            // 显示影响的行数和输出参数
            string primar1 = parameters[0].Value.ToString();
            if (primar1 == "0")
            {
                result.code = 0;
                result.msg = "操作成功";
                result.data = Detail;
            }
            else {
                result.code = 320;
                result.msg = "操作失败";
                result.data = "";
            }

            return Json(result);

        }

        public JsonResult AddColumnDetail(string TableName, string ColumnName, string Detail)
        {
            ReturnResult result = new ReturnResult();
            string conn = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(conn);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlconn;
            cmd.CommandText = "db_detail_add";
            cmd.CommandType = CommandType.StoredProcedure;

            // 创建参数
            IDataParameter[] parameters = {
                new SqlParameter("@Result", SqlDbType.Int,4) ,
                new SqlParameter("@TableName", SqlDbType.NVarChar,50) ,
                new SqlParameter("@ColumnName", SqlDbType.NVarChar,50) ,
                new SqlParameter("@DaTail", SqlDbType.NVarChar,300) ,
                new SqlParameter("@TypeId", SqlDbType.Int,4)
            };
            // 设置参数类型
            parameters[0].Direction = ParameterDirection.Output;  // 设置为输出参数
            parameters[1].Value = TableName;
            parameters[2].Value = ColumnName;
            parameters[3].Value = Detail;
            parameters[4].Value = 2;




            // 添加参数
            cmd.Parameters.AddRange(parameters);

            sqlconn.Open();
            // 执行存储过程并返回影响的行数
            string count = cmd.ExecuteNonQuery().ToString();
            sqlconn.Close();
            // 显示影响的行数和输出参数
            string primar1 = parameters[0].Value.ToString();
            if (primar1 == "0")
            {
                result.code = 0;
                result.msg = "操作成功";
                result.data = Detail;
            }
            else
            {
                result.code = 320;
                result.msg = "操作失败";
                result.data = "";
            }

            return Json(result);
        }

        /// <summary>
        /// 生成文档
        /// </summary>
        /// <returns></returns>
        public ActionResult Document()
        {
            try
            {
                string WordPath = Server.MapPath("~/Content/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".docx");
                string DbName = "temp";
                string[] str_Title = new string[] { "字段", "描述", "标识", "主键", "类型", "长度", "允许空", "默认" };
                Border bor = new Border();
                bor.Tcbs = Novacode.BorderStyle.Tcbs_single;

                using (DocX document = DocX.Create(WordPath))
                {

                    DbName = "";
                    DataTable Dbdt = DBManage.SqlDbHelper.ExecuteDataTable("select db_name() as DbName");
                    if (Dbdt.Rows.Count > 0)
                        DbName = Dbdt.Rows[0]["DbName"].ToString();

                    Paragraph p = document.InsertParagraph(DbName); //插入段落
                                                                    // p.Color(System.Drawing.Color.Blue);
                    p.Alignment = Alignment.center;
                    p.FontSize(18);
                    p.Bold();

                    document.InsertParagraph(new string(' ', 80));
                    document.InsertParagraph(new string(' ', 80));

                    DataTable dt = DBManage.SqlDbHelper.ExecuteDataTable("select * from db_talbe_list order by tableName");
                    foreach (DataRow item in dt.Rows)
                    {

                        //表名
                        Paragraph ph = document.InsertParagraph(item["TableName"].ToString() + "(" + item["ColumnsDetail"] + ")"); //插入段落
                                                                                                                                   // ph.Color(System.Drawing.Color.Blue);
                        ph.Alignment = Alignment.left;
                        ph.FontSize(12);
                        ph.Bold();
                        document.InsertParagraph(new string(' ', 80));

                        DataTable Coldt = DBManage.SqlDbHelper.ExecuteDataTable(string.Format("select * from db_talbe_columns where tableName='{0}'", item["TableName"].ToString()));
                        int CouIndex = 0;

                        Table t = document.AddTable(Coldt.Rows.Count + 1, 8);
                        t.Alignment = Alignment.center;
                        t.Rows[0].Height = 20d;
                        Row row = t.Rows[0];

                        for (int i = 0; i < str_Title.Length; i++)
                        {


                            Cell cell = t.Rows[0].Cells[i];
                            //设置列宽度
                            switch (i)
                            {
                                case 0:
                                    cell.Width = 350;
                                    break;
                                case 1:
                                    cell.Width = 400;
                                    break;
                                case 2:
                                    cell.Width = 60;
                                    break;
                                case 3:
                                    cell.Width = 60;
                                    break;
                                case 4:
                                    cell.Width = 280;
                                    break;
                                case 5:
                                    cell.Width = 60;
                                    break;
                                case 6:
                                    cell.Width = 60;
                                    break;
                                case 7:
                                    cell.Width = 260;
                                    break;

                            }
                            //填充表格颜色及绘制边框
                            cell.FillColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                            cell.Paragraphs[0].Append(str_Title[i]).Bold().Alignment = Alignment.center;
                            cell.Paragraphs[0].FontSize(10);
                            cell.SetBorder(TableCellBorderType.Left, bor);
                            cell.SetBorder(TableCellBorderType.Right, bor);
                            cell.SetBorder(TableCellBorderType.Top, bor);
                            cell.SetBorder(TableCellBorderType.Bottom, bor);
                        }




                        foreach (DataRow Colitem in Coldt.Rows)
                        {


                            //列名信息

                            // Specify some properties for this Table.



                            t.Rows[CouIndex + 1].Cells[0].Paragraphs.First().FontSize(8).Append(Colitem["FieldName"].ToString());
                            t.Rows[CouIndex + 1].Cells[1].Paragraphs.First().FontSize(8).Append(Colitem["FieldDescription"].ToString());
                            t.Rows[CouIndex + 1].Cells[2].Paragraphs.First().FontSize(8).Append(Colitem["IsIdenttity"].ToString() == "1" ? "是" : "否").Alignment = Alignment.center;
                            t.Rows[CouIndex + 1].Cells[3].Paragraphs.First().FontSize(8).Append(Colitem["PrimarKey"].ToString() == "1" ? "是" : "否").Alignment = Alignment.center;
                            t.Rows[CouIndex + 1].Cells[4].Paragraphs.First().FontSize(8).Append(Colitem["FieldType"].ToString());
                            t.Rows[CouIndex + 1].Cells[5].Paragraphs.First().FontSize(8).Append(Colitem["FieldLength"].ToString()).Alignment = Alignment.center;
                            t.Rows[CouIndex + 1].Cells[6].Paragraphs.First().FontSize(8).Append(Colitem["IsNull"].ToString() == "1" ? "是" : "否").Alignment = Alignment.center;
                            t.Rows[CouIndex + 1].Cells[7].Paragraphs.First().FontSize(8).Append(Colitem["DefaultValue"].ToString()).Alignment = Alignment.center;




                            CouIndex++;
                        }
                        document.InsertTable(t);
                        document.InsertParagraph(new string(' ', 80));
                        document.InsertParagraph(new string(' ', 80));



                    }

                    document.Save();




                }

                if (System.IO.File.Exists(WordPath))
                {
                    byte[] fileContents = System.IO.File.ReadAllBytes(WordPath);
                    System.IO.File.Delete(WordPath);  //删除服务器端文件
                    var fileStream = new MemoryStream(fileContents);
                    return File(fileStream, "application/ms-word", DbName + DateTime.Now.ToString("yyMMddHHmmss") + ".docx");
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
            return View();

        }

    }
}
