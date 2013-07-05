using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;

namespace AHome.Controllers
{
    /// <summary>
    /// 用于上传图片、文件等
    /// </summary>
    public class FileManageController : Controller
    {
        public static string ServerPic = ConfigurationManager.AppSettings["PicServerPath"];

        /// <summary>
        /// MasterRegister上传头像图片
        /// </summary>
        /// <returns></returns>
        public string UpLoadMasterPic()
        {
            string returnJson = "";
            //上传图片到大师的主目录下
            string filePath = Server.MapPath("../Pictures/Master/MainMasterPic");
            //返回的文件名
            string outFileName;
            //返回的json格式
            string state = Common.FileUpload.FileUploadSingle(Request.Files, filePath, out outFileName) ? "success":"error";

            //135*135规格小图片保存
            string small_pic_path = filePath + "/small/";
            //调用缩略图算法
            Common.pic_zip.getzip_pic(135, 135, filePath + "/", small_pic_path, outFileName);
            //308*305规格中等图片
            // Common.pic_zip.getzip_pic(308, 305, filePath, middle_pic_path,outFileName );


            string VirtualPath = ServerPic + "Master/MainMasterPic/";//文件虚拟路径
            string PicTypeDirectory = GetPicType("small");
            if (PicTypeDirectory != null)
                VirtualPath += GetPicType("small");
            else
                VirtualPath += ServerPic + "NoPic.jpg";
            string oututPath = VirtualPath + outFileName;

            returnJson = "{status:\"" + state + "\", fileName:\"" + outFileName + "\" ,filePath:\"" + oututPath + "\"}";
            return returnJson;
        }



        #region ==GetImg==

        /// <summary>
        /// 获得大师的图片
        /// </summary>
        /// <param name="context"></param>
        public string GetMasterPic()
        {
            string type = Request["type"];//要访问的图片尺寸
            string fileName = Request["fileName"];//文件名称

            string VirtualPath = ServerPic + "Master/MainMasterPic/";//文件虚拟路径
            if (!Tools.IsValidInput(ref fileName, true))
            {
                return string.Empty;
            }
            //真实路径
            string PicTypeDirectory = GetPicType(type);
            if (PicTypeDirectory != null)
                VirtualPath += GetPicType(type);
            else
                VirtualPath += ServerPic + "NoPic.jpg";

            //WriteImg(VirtualPath + fileName);//输出图片
            string path = Server.MapPath(VirtualPath + fileName);
            return path;
        }

        /// <summary>
        /// 获得图片的尺寸类别(small,middle,Big)
        /// </summary>
        /// <param name="Type">类别</param>
        /// <returns></returns>
        public string GetPicType(string Type)
        {
            string Directory = "";
            switch (Type.ToLower())
            {
                case "small":
                    Directory = "small/";
                    break;
                case "medium":
                    Directory = "";
                    break;
                case "big":
                    Directory = "big/";
                    break;
                default:
                    break;
            }
            return Directory;
        }

        /// <summary>
        /// 输出图片信息
        /// </summary>
        /// <param name="context"></param>
        public void WriteImg(string FileName)
        {
            string path = Server.MapPath(FileName);
            //获取图片文件的二进制数据。
            Real(path);
        }

        /// <summary>
        /// 按照文件类型输出
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileName"></param>
        private void Real(string fileName)
        {

            FileInfo file = new FileInfo(fileName);
            if (file.Exists == false)
            {
                file = new FileInfo(Server.MapPath(ServerPic + "NoPic.jpg"));
            }
            Response.Clear();

            Response.AddHeader("Content-Disposition", "filename=" + file.Name);

            Response.AddHeader("Content-Length", file.Length.ToString());

            string fileExtension = file.Extension.ToLower();

            //这里选择输出的文件格式

            //可以参考http://ewebapp.cnblogs.com/articles/234756.html增加对文件格式的支持.

            switch (fileExtension)
            {

                case ".mp3":

                    Response.ContentType = "audio/mpeg3";

                    break;

                case ".mpeg":

                    Response.ContentType = "video/mpeg";

                    break;

                case ".jpg":

                    Response.ContentType = "image/jpeg";

                    break;

                case ".bmp":

                    Response.ContentType = "image/bmp";

                    break;

                case ".gif":

                    Response.ContentType = "image/gif";

                    break;

                case ".doc":

                    Response.ContentType = "application/msword";

                    break;

                case ".css":

                    Response.ContentType = "text/css";

                    break;

                default:

                    Response.ContentType = "application/octet-stream";

                    break;

            }
            byte[] datas = System.IO.File.ReadAllBytes(file.FullName);
            Response.ClearContent(); //需要输出图象信息 要修改HTTP头 
            //context.Response.ContentType = "image/Jpeg";
            //将二进制数据写入到输出流中。
            // context.Response.OutputStream.Write(datas, 0, datas.Length);
            Response.BinaryWrite(datas);
            //context.Response.BinaryWrite(file.FullName);

            Response.End();

        }

        #endregion


    }
}
