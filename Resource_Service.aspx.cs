
using System;
using System.Web;
using System.Collections.Generic;
using DotNetNuke.Entities.Users;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using System.IO;
using System.Drawing;
using System.Text;
using System.Collections;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Services.FileSystem;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// 资源文件(主要用于一些请求的服务)
    /// 1.文件上传
    /// </summary>
    public partial class Resource_Service : BasePage
    {

        #region "属性"
        /// <summary>
        /// 功能
        /// 文件上传 FileUpload
        /// </summary>
        private String Token = WebHelper.GetStringParam(HttpContext.Current.Request, "Token", "").ToLower();
 
         

        #endregion




        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PushThumbnail();
            }
        }


 



        #region "缩略图的生成"
        /// <summary>
        /// 缩略图的生成
        /// </summary>
        private void PushThumbnail()
        {
            //根据ID查询出缩略的方式
            Int32 ItemID = WebHelper.GetIntParam(Request, "ID", 0);
            Int32 Width = WebHelper.GetIntParam(Request, "width", 200);
            Int32 height = WebHelper.GetIntParam(Request, "height", 200);
            String Mode = WebHelper.GetStringParam(Request, "mode", "AUTO");
            String PhotoType = WebHelper.GetStringParam(Request, "Type", "p").ToLower();

            String ImagePath = MapPath(String.Format("{0}/Resource/images/no_image.png", TemplateSourceDirectory));



            //if (ItemID > 0)
            //{
            //    DNNGo_LayerGallery_Content ContentItem = DNNGo_LayerGallery_Content.FindByID(ItemID);
            //    if (ContentItem != null && ContentItem.ID > 0)
            //    {
                    

            //        if (PhotoType == "p")
            //            ImagePath = ViewLinkUrl(ContentItem.Picture, ContentItem.PortalId);
            //        else
            //            ImagePath = ViewLinkUrl(ContentItem.Thumbnails, ContentItem.PortalId);

            //    }
            //}
            GenerateThumbnail.PushThumbnail(Server.UrlDecode(ImagePath), Width, height, Mode);


        }

        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, int PortalId)
        {
            String DefaultValue = String.Empty;
            if (!String.IsNullOrEmpty(UrlValue))
            {
                if (UrlValue.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    var fi = FileManager.Instance.GetFile(Convert.ToInt32(UrlValue.Replace("FileID=", "")));
                    
                    DefaultValue = string.Format("{0}{1}{2}", PortalSettings.HomeDirectory, fi.Folder, Server.UrlPathEncode(fi.FileName));
                    DefaultValue = MapPath(DefaultValue);
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")));
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = String.Format("{0}Resource/images/no_image.png", ModulePath);

                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_LayerGallery_Files Multimedia = DNNGo_LayerGallery_Files.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            DefaultValue = String.Format("{0}{1}",PortalSettings.HomeDirectory, Multimedia.FilePath);
                        }
                    }
                    DefaultValue = Server.MapPath(DefaultValue);

                }
                else
                {
                    DefaultValue = UrlValue;
                    if (!String.IsNullOrEmpty(DefaultValue) && DefaultValue.IndexOf("http://") < 0 )
                    {
                        DefaultValue = MapPath(DefaultValue);
                    }

                }
            }
            return DefaultValue;

        }

        #endregion


 

    }
}