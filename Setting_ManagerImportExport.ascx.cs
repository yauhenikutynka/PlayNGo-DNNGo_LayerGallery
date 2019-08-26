using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;
using System.Web.Script.Serialization;
 


namespace DNNGo.Modules.LayerGallery
{
    public partial class Setting_ManagerImportExport : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();


        private List<KeyValueEntity> ImportPictureList = new List<KeyValueEntity>();


        #endregion




        #region "==方法=="







        #endregion





        #region "==事件=="

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //如果查询到当前有字段时，需要给用户提示会清除掉原有的字段
                    cmdImportFormXml.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("ImportContent", this.LocalResourceFile) + "');");
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        /// <summary>
        /// 导出数据到XML
        /// </summary>
        protected void cmdExportToXml_Click(object sender, EventArgs e)
        {
            try
            {
                //查询字段的数据,填充待导出的XML实体
                QueryParam qp = new QueryParam();
                qp.OrderType = 0;
                Int32 RecordCount = 0;
                qp.Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));
                List<DNNGo_LayerGallery_Content> ArticleList = DNNGo_LayerGallery_Content.FindAll(qp, out RecordCount);

                if (ArticleList != null && ArticleList.Count > 0)
                {
                    List<GalleryContentEntity> xmlContentList = new List<GalleryContentEntity>();
                    List<GallerySettingsEntity> xmlSettingList = new List<GallerySettingsEntity>();

                    ImportExportHelper ieHelper = new ImportExportHelper();
                    ieHelper.ModuleID = ModuleId;
                    ieHelper.UserId = UserId;

                    //查询出所有的配置项
                    if (Settings != null && Settings.Count > 0)
                    {
                        List<KeyValueEntity> DefaultSettings = GetDefaultSettings();

                        foreach (KeyValueEntity kv in DefaultSettings)
                        {
                            String key = String.Format("Global_{0}", kv.Key);
                            if (!xmlSettingList.Exists(r1 => r1.SettingName == key) && Settings[key] != null)
                            {
                                xmlSettingList.Add(new GallerySettingsEntity(key, Convert.ToString(Settings[key])));
                            }
                        }
                    }


                    foreach (DNNGo_LayerGallery_Content ContentItem in ArticleList)
                    {
                        xmlContentList.Add(EntityToXml(ContentItem));
                    }

                   

                    XmlFormat xf = new XmlFormat(MapPath(String.Format("{0}Resource/xml/Entity.xml", ModulePath)));
                    //将字段列表转换成XML的实体
                    String XmlContent = xf.ToXml<GalleryContentEntity>(xmlContentList, xmlSettingList);
                    String XmlFilePath = FileSystemUtils.SaveXmlToFile(String.Format("ContentListEntity_{0}_{1}.xml", ModuleId, xUserTime.UtcTime().ToString("yyyyMMddHHmmssffff")), XmlContent, this);
                    FileSystemUtils.DownloadFile(XmlFilePath, "ContentListEntity.xml");
                }
                else
                {
                    //没有可导出的文章条目
                    mTips.IsPostBack = true;
                    mTips.LoadMessage("ExportContentError", EnumTips.Success, this, new String[] { "" });
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
            
        }

        /// <summary>
        /// 从XML导入数据
        /// </summary>
        protected void cmdImportFormXml_Click(object sender, EventArgs e)
        {
            
            try
            {
           
                HttpPostedFile hpfile = fuImportFormXml.PostedFile;

                if (hpfile.ContentLength > 0)
                {

                    if (Path.GetExtension(hpfile.FileName).IndexOf(".xml", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {

                        ImportExportHelper ieHelper = new ImportExportHelper();
                        ieHelper.ModuleID = ModuleId;
                        ieHelper.UserId = UserId;

                        //构造需要上传的路径
                        String XmlFilePath = String.Format("{0}LayerGallery\\Import\\{1}_{2}", PortalSettings.HomeDirectoryMapPath, xUserTime.UtcTime().ToString("yyyyMMddHHmmssffff"), fuImportFormXml.FileName);
                        FileInfo XmlFile = new FileInfo(XmlFilePath);
                        //判断文件夹是否存在
                        if (!XmlFile.Directory.Exists) XmlFile.Directory.Create();
                        //保存文件
                        fuImportFormXml.SaveAs(XmlFilePath);
                        //将XML转换为实体
                        XmlFormat xf = new XmlFormat(XmlFilePath);
                        List<GalleryContentEntity> XmlContentList = xf.ToList<GalleryContentEntity>();
                        List<GallerySettingsEntity> XmlSettingList = xf.ToList<GallerySettingsEntity>();


                        //插入成功的数量
                        Int32 InsertResult = 0;
                        //插入内容的记录
                        foreach (GalleryContentEntity XmlContentItem in XmlContentList)
                        {
                            DNNGo_LayerGallery_Content ContentItem = ieHelper.XmlToEntity(XmlContentItem);

                            if (ContentItem.Insert() > 0)
                            {
                                InsertResult++;

                                //导入项
                                ieHelper.ImportItem(ContentItem, XmlContentItem.ItemList);

                                //删除临时上传的文件
                                if (XmlFile.Exists) XmlFile.Delete();
                            }

                        }
 
                        //插入设置的记录
                        foreach (GallerySettingsEntity XmlSettingItem in XmlSettingList)
                        {
                            if (XmlSettingItem != null && !String.IsNullOrEmpty(XmlSettingItem.SettingName) && XmlSettingItem.SettingName.IndexOf("Global_") >= 0 && XmlSettingItem.SettingValue!= null)
                            {
                                ieHelper.UpdateModuleSetting(ModuleId, XmlSettingItem.SettingName, XmlSettingItem.SettingValue);
                            }
                        }

                        //删除临时上传的文件
                        if (XmlFile.Exists) XmlFile.Delete();
 
                        //提示
                        mTips.LoadMessage("ImportContentSuccess", EnumTips.Success, this, new String[] { InsertResult.ToString() });

                        //跳转
                        Response.Redirect(xUrl("Sliders"));
                    }
                    else
                    {
                        //上传文件的后缀名错误
                        mTips.IsPostBack = true;
                        mTips.LoadMessage("ImportContentExtensionError", EnumTips.Success, this, new String[] { "" });
                    }
                }
                else
                {
                    //为上传任何数据
                    mTips.IsPostBack = true;
                    mTips.LoadMessage("ImportContentNullError", EnumTips.Success, this, new String[] { "" });
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
    
        

 

        }

        #endregion




        #region "数据转换XML & Entity"

        /// <summary>
        /// Gallery数据转XML实体
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public GalleryContentEntity EntityToXml(DNNGo_LayerGallery_Content ContentItem)
        {
            GalleryContentEntity ContentXml = new GalleryContentEntity();
 
            ContentXml.EndTime = ContentItem.EndTime;
            ContentXml.StartTime = ContentItem.StartTime;
            ContentXml.Options = ContentItem.Options;
            ContentXml.Sort = ContentItem.Sort;
            ContentXml.Status = ContentItem.Status;
            ContentXml.Title = ContentItem.Title;


            ContentXml.ItemList = Common.XmlEncode(ItemEntityToString(ContentItem));

            return ContentXml;
        }




        /// <summary>
        /// 项的集合转字符串
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public String ItemEntityToString(DNNGo_LayerGallery_Content ContentItem)
        {
            String ItemXml = String.Empty;
            if (ContentItem != null && ContentItem.ID > 0)
            {
                QueryParam qp = new QueryParam();
                int RecordCount = 0;
                qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Item._.ContentID, ContentItem.ID, SearchType.Equal));
                List<DNNGo_LayerGallery_Item> list = DNNGo_LayerGallery_Item.FindAll(qp, out RecordCount);
                if (list != null && list.Count > 0)
                {
                    List<GalleryItemEntity> ItemList = new List<GalleryItemEntity>();
                    foreach (DNNGo_LayerGallery_Item item in list)
                    {
                        ItemList.Add(EntityToXml(item));
                    }
                    XmlFormat xf = new XmlFormat(MapPath(String.Format("{0}Resource/xml/ItemEntity.xml", ModulePath)));
                    ItemXml = xf.ToXml<GalleryItemEntity>(ItemList);
                }
            }
            return ItemXml;
        }

        /// <summary>
        /// Gallery数据转XML实体
        /// </summary>
        /// <param name="ContentItem"></param>
        /// <returns></returns>
        public GalleryItemEntity EntityToXml(DNNGo_LayerGallery_Item ContentItem)
        {
            GalleryItemEntity ContentXml = new GalleryItemEntity();

            ContentXml.Sort = ContentItem.Sort;
            ContentXml.Status = ContentItem.Status;
            ContentXml.Options = ContentItem.Options;

            ContentXml.Title = ContentItem.Title;

            return ContentXml;
        }


        /// <summary>
        /// 相册XML转数据实体
        /// </summary>
        /// <returns></returns>
        public DNNGo_LayerGallery_Content XmlToEntity(GalleryContentEntity ContentXml)
        {
            DNNGo_LayerGallery_Content ContentItem = new DNNGo_LayerGallery_Content();

       
            ContentItem.Sort = ContentXml.Sort;
            ContentItem.Status = ContentXml.Status;
            ContentItem.Title = ContentXml.Title;
            ContentItem.Options = ContentXml.Options;

            ContentItem.StartTime = ContentXml.StartTime;
            ContentItem.EndTime = ContentXml.EndTime;
            ContentItem.ModuleId = ModuleId;
            ContentItem.PortalId = PortalId;

            ContentItem.LastIP = WebHelper.UserHost;
            ContentItem.LastTime = xUserTime.UtcTime();
            ContentItem.LastUser = UserId;
            return ContentItem;
        }


        /// <summary>
        /// 导入项
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ContentEntity"></param>
        /// <returns></returns>
        public Int32 ImportItem(DNNGo_LayerGallery_Content Content, String ItemList)
        {
            Int32 Result = 0;
            if (Content != null && Content.ID > 0 && !String.IsNullOrEmpty(ItemList))
            {
                //还原出项的列表
                XmlFormat xf = new XmlFormat();
                xf.XmlDoc.LoadXml(Common.XmlDecode(ItemList));

                List<GalleryItemEntity> list = xf.ToList<GalleryItemEntity>();

                foreach (GalleryItemEntity itemEntity in list)
                {
                    DNNGo_LayerGallery_Item item = new DNNGo_LayerGallery_Item();

                    item.Options = Common.XmlDecode(itemEntity.Options);
                    item.Status = itemEntity.Status;
                    item.Title = itemEntity.Title;
                    item.Sort = itemEntity.Sort;



                    item.ModuleId = Content.ModuleId;
                    item.PortalId = Content.PortalId;
                    item.LastTime = Content.LastTime;
                    item.LastUser = Content.LastUser;
                    item.LastIP = Content.LastIP;
                    item.ContentID = Content.ID;

                    //添加项
                    if (item.Insert() > 0)
                    {
                        Result++;
                    }

                }
            }
            return Result;
        }

 




        /// <summary>
        /// 获取默认的设置
        /// </summary>
        /// <returns></returns>
        public List<KeyValueEntity> GetDefaultSettings()
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            Dictionary<String, object> dicts = (Dictionary<String, object>)jsSerializer.DeserializeObject(System.IO.File.ReadAllText(Server.MapPath(String.Format("{0}Resource/xml/GlobalSetting.json", ModulePath))));
            return ConvertTo.ToList(dicts);
        }
 

 
        #endregion



    }
}