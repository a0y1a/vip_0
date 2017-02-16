using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using KeKeSoftPlatform.Common;
using NPOI.XSSF.UserModel;

namespace KeKeSoftPlatform.Core
{
    [Right]
    public class BaseController : Controller
    {
        #region 文件下载
        /// <summary>
        /// 下载文件流
        /// </summary>
        /// <param name="data">文件流</param>
        /// <param name="fileName">包含后缀的文件名称</param>
        /// <returns></returns>
        public FileResult Download(byte[] data, string fileName)
        {
            return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, Url.Encode(fileName));
        }

        /// <summary>
        /// 下载指定文件
        /// </summary>
        /// <param name="fileFullName">包含路径、后缀的完整文件名称</param>
        /// <returns></returns>
        public FileResult Download(string fileFullName)
        {
            using (var fileStream = new FileStream(fileFullName, FileMode.Open))
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);

                string outputFileName = Path.GetFileName(fileFullName);
                string browser = Request.UserAgent.ToUpper();
                if (browser.Contains("MS") == true && browser.Contains("IE") == true)
                {
                    outputFileName = Url.Encode(outputFileName);
                }
                else if (browser.Contains("FIREFOX") == true)
                {
                }
                else
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, outputFileName);
            }
        }

        /// <summary>
        /// 下载指定文件
        /// </summary>
        /// <param name="fileFullName">包含路径、后缀的完整文件名称</param>
        /// <returns></returns>
        public FileResult Download(string filePath,string fileName)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);

                string outputFileName = fileName;
                string browser = Request.UserAgent.ToUpper();
                if (browser.Contains("MS") == true && browser.Contains("IE") == true)
                {
                    outputFileName = Url.Encode(outputFileName);
                }
                else if (browser.Contains("FIREFOX") == true)
                {
                }
                else
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                return File(data, System.Net.Mime.MediaTypeNames.Application.Octet, outputFileName);// HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString()
            }
        }

        /// <summary>
        /// 下载制定mime格式的文件
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <param name="contentType">MIME类型</param>
        /// <param name="fileName">文件名称</param>
        /// <returns></returns>
        public FileResult Download(string filePath, string contentType, string fileName)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] data = new byte[fileStream.Length];
                fileStream.Read(data, 0, (int)fileStream.Length);

                string outputFileName = fileName;
                string browser = Request.UserAgent.ToUpper();
                if (browser.Contains("MS") == true && browser.Contains("IE") == true)
                {
                    outputFileName = Url.Encode(outputFileName);
                }
                else if (browser.Contains("FIREFOX") == true)
                {
                }
                else
                {
                    outputFileName = Url.Encode(outputFileName);
                }

                return File(data, contentType, outputFileName);// HttpUtility.UrlEncode(fileName, Encoding.UTF8).ToString()
            }
        }
        #endregion

        #region 覆盖重载默认的 JsonResult 序列化json 逻辑 改用json.net 引擎序列化对象，解决 日期格式等问题
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        #endregion

        #region   导出Excel

        public ActionResult ExportToExcel(string fileName, DataTable data)
        {
            //Excel功能
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(); //新建一个xls文件
            ISheet sheet = hssfworkbook.CreateSheet(fileName); //创建一个sheet


            if (data != null)
            {
                var firstRow = sheet.CreateRow(0);
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    firstRow.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);
                }
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        string value = (data.Rows[i][j] == DBNull.Value) ? "" : data.Rows[i][j].ToString();
                        row.CreateCell(j).SetCellValue(value);
                    }
                }
            }
            MemoryStream filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
            hssfworkbook.Write(filestream); //把文件读到内存流里面
            return File(filestream.GetBuffer(), "application/vnd.ms-excel", string.Format("{0}.xls", fileName));
        }

        #endregion

    }
}
