using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using System.IO;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using System.Collections;
using DotNetNuke.Common.Utilities;
using System.Web.Script.Serialization;

namespace DNNGo.Modules.LayerGallery
{
    public class ImportExportHelper
    {

        #region "==属性=="
        /// <summary>
        /// 导入时的图片列表
        /// </summary>
        private List<KeyValueEntity> ImportPictureList = new List<KeyValueEntity>();

        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleID
        {
            get;
            set;
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public Int32 UserId
        {
            get;
            set;
        }

        

        private ModuleInfo _moduleInfo = new ModuleInfo();
        /// <summary>
        /// 模块信息
        /// </summary>
        public ModuleInfo ModuleInfo
        {
            get {
                if (!(_moduleInfo != null && _moduleInfo.ModuleID > 0) && ModuleID >0)
                {
                    ModuleController mc= new ModuleController();
                    _moduleInfo = mc.GetModule(ModuleID);
                }
                return _moduleInfo; }
        }

        private PortalInfo _portalInfo = new PortalInfo();
        /// <summary>
        /// 站点信息
        /// </summary>
        public PortalInfo portalInfo
        {
            get
            {
                if (!(_portalInfo != null && _portalInfo.PortalID > 0) && ModuleID > 0)
                {
                    PortalController pc = new PortalController();
                    _portalInfo = pc.GetPortal(ModuleInfo.PortalID);
                    
                }
                return _portalInfo;
            }
        }
        private PortalSettings _LayerGallery_PortalSettings = new PortalSettings();
        /// <summary>
        /// 获取站点配置
        /// </summary>
        public PortalSettings LayerGallery_PortalSettings
        {
            get
            {
                if (!(_LayerGallery_PortalSettings != null && _LayerGallery_PortalSettings.PortalId > 0))
                {

                    _LayerGallery_PortalSettings = new PortalSettings(portalInfo.PortalID);

                        DotNetNuke.Entities.Portals.PortalAliasController pac = new PortalAliasController();
                        ArrayList PortalAlias = pac.GetPortalAliasArrayByPortalID(portalInfo.PortalID);
                        if (PortalAlias != null && PortalAlias.Count > 0)
                        {
                            _LayerGallery_PortalSettings.PortalAlias = (PortalAliasInfo)PortalAlias[0];
                        }
                        else
                        {

                            _LayerGallery_PortalSettings.PortalAlias = new PortalAliasInfo();
                            _LayerGallery_PortalSettings.PortalAlias.PortalID = portalInfo.PortalID;
                        }
                }
                return _LayerGallery_PortalSettings;
            }
        }



        private Hashtable _Settings = new Hashtable();
        /// <summary>
        /// 获取模块配置(可以获取其他模块配置)
        /// </summary>
        public Hashtable Settings
        {
            get
            {
                if (!(_Settings != null && _Settings.Count > 0))
                {
                    _Settings = new ModuleController().GetModule(ModuleID).ModuleSettings; 
                }
                return _Settings;
            }
        }


        private String _ModulePath = String.Empty;
        /// <summary>
        /// 模块路径
        /// </summary>
        public String ModulePath
        {
            get {
                if (String.IsNullOrEmpty(_ModulePath))
                {
                    ModuleController mc = new ModuleController();
                    ModuleInfo mInfo = mc.GetModule(ModuleID);
                    _ModulePath = "~/DesktopModules/DNNGo_LayerGallery/";
                    if (mInfo != null && mInfo.ModuleID > 0)
                    {
                        bool propertyNotFound = false;
                        _ModulePath = String.Format("~/DesktopModules/{0}/", mInfo.GetProperty("FolderName", "", null, null, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound));
                    }
                }


                return _ModulePath;
            }
        }
        


        private List<SettingEntity> _Setting_EffectSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定效果设置项
        /// </summary>
        public List<SettingEntity> Setting_EffectSettingDB
        {
            get
            {
                if (!(_Setting_EffectSettingDB != null && _Setting_EffectSettingDB.Count > 0))
                {
                    String EffectSettingDBPath = HttpContext.Current.Server.MapPath(String.Format("{0}Effects/{1}/EffectSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _Setting_EffectSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_EffectSettingDB;
            }
        }


        /// <summary>
        /// 获取绑定的效果名称
        /// </summary>
        public String Settings_EffectName
        {
            get { return Settings["LayerGallery_EffectName"] != null ? Convert.ToString(Settings["LayerGallery_EffectName"]) : "Effect_01_AnythingSlider"; }
        }


        #endregion 


        #region "==公用方法=="

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
                    XmlFormat xf = new XmlFormat(HttpContext.Current.Server.MapPath(String.Format("{0}Resource/xml/ItemEntity.xml", ModulePath)));
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
            ContentItem.ModuleId = ModuleID;
            ContentItem.PortalId = portalInfo.PortalID;

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
            Dictionary<String, object> dicts = (Dictionary<String, object>)jsSerializer.DeserializeObject(System.IO.File.ReadAllText(HttpContext.Current.   Server.MapPath(String.Format("{0}Resource/xml/GlobalSetting.json", ModulePath))));
            return ConvertTo.ToList(dicts);
        }
 



        #endregion

        #region "更新模块设置"


        /// <summary>
        /// 更新当前模块的设置
        /// </summary>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(string SettingName, string SettingValue)
        {
            UpdateModuleSetting(ModuleID, SettingName, SettingValue);
        }


        /// <summary>
        /// 更新模块设置
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="SettingName"></param>
        /// <param name="SettingValue"></param>
        public void UpdateModuleSetting(int ModuleId, string SettingName, string SettingValue)
        {
            ModuleController controller = new ModuleController();

            controller.UpdateModuleSetting(ModuleId, SettingName, SettingValue);
        }

        /// <summary>
        /// 效果参数保存名称格式化
        /// </summary>
        /// <param name="EffectName">效果名</param>
        /// <param name="ThemeName">主题名</param>
        /// <returns></returns>
        public String EffectSettingsFormat(String EffectName, String ThemeName)
        {
            return String.Format("Gallery{0}_{1}", EffectName, ThemeName);
        }

        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            String SettingKey = EffectSettingsFormat(Settings_EffectName, Name);
            return Settings[SettingKey] != null ? ConvertTo.FormatValue(Settings[SettingKey].ToString(), DefaultValue.GetType()) : DefaultValue;
        }

        #endregion

    }
}