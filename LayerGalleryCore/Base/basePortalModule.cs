using System;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Entities.Modules;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using DotNetNuke.Entities.Host;
using DotNetNuke.Common.Utilities;

using DotNetNuke.Entities.Portals;
using DotNetNuke.UI.Skins;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Controllers;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public class basePortalModule : PortalModuleBase
    {

        #region "模块设置与绑定"
        /// <summary>
        /// 设置绑定的模块编号
        /// </summary>
        public Int32 Settings_ModuleID
        {
            get { return Settings_CopyOfOtherModule ? (Settings["LayerGallery_ModuleID"] != null ? Convert.ToInt32(Settings["LayerGallery_ModuleID"]) : ModuleId) : ModuleId; }
        }


        /// <summary>
        /// 设置绑定的页面编号
        /// </summary>
        public Int32 Settings_TabID
        {
            get { return Settings_CopyOfOtherModule ? (Settings["LayerGallery_TabID"] != null ? Convert.ToInt32(Settings["LayerGallery_TabID"]) : TabId):TabId; }
        }

        /// <summary>
        /// 设置绑定的站点编号
        /// </summary>
        public Int32 Settings_PortalID
        {
            get { return Settings_CopyOfOtherModule ? (Settings["LayerGallery_CopyOfPortal"] != null ? Convert.ToInt32(Settings["LayerGallery_CopyOfPortal"]) : PortalId) : PortalId; }
        }


        /// <summary>
        /// 排序方式
        /// </summary>
        public Int32 Settings_Sortby
        {
            get { return Settings["LayerGallery_Sortby"] != null ? Convert.ToInt32(Settings["LayerGallery_Sortby"]) : 0; }
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
                    if (Settings_CopyOfOtherModule && Settings_ModuleID != ModuleId)
                    {
                        _LayerGallery_PortalSettings = new PortalSettings(Settings_PortalID);

                        DotNetNuke.Entities.Portals.PortalAliasController pac = new PortalAliasController(); 
                        ArrayList PortalAlias = pac.GetPortalAliasArrayByPortalID(Settings_PortalID);
                        if (PortalAlias != null && PortalAlias.Count > 0)
                        {
                            _LayerGallery_PortalSettings.PortalAlias = (PortalAliasInfo)PortalAlias[0];
                        }else
                        {

                            _LayerGallery_PortalSettings.PortalAlias = new PortalAliasInfo();
                            _LayerGallery_PortalSettings.PortalAlias.PortalID = Settings_PortalID;
                        }

                        
                    }
                    else
                    {
                        _LayerGallery_PortalSettings = PortalController.GetCurrentPortalSettings();
                    }
                }
                return _LayerGallery_PortalSettings;
            }
        }


        /// <summary>
        /// 数据是否来自于其他模块
        /// </summary>
        public Boolean Settings_CopyOfOtherModule
        {
            get { return Settings["LayerGallery_CopyOfOtherModule"] != null ? Convert.ToBoolean(Settings["LayerGallery_CopyOfOtherModule"]) : false; }
        }


 



        #endregion



        #region "基础配置属性"


        

        /// <summary>
        /// 获取绑定的效果名称
        /// </summary>
        public String Settings_EffectName
        {
            get { return Settings["LayerGallery_EffectName"] != null ? Convert.ToString(Settings["LayerGallery_EffectName"]) : GetDefaultEffectName(); }
        }


        /// <summary>
        /// 获取绑定的效果主题名称
        /// </summary>
        public String Settings_EffectThemeName
        {
            get { return Settings["LayerGallery_EffectThemeName"] != null ? Convert.ToString(Settings["LayerGallery_EffectThemeName"]) : GetDefaultThemeName(); }
        }

        public  Boolean designMode
        {
            get { return DesignMode; }
        }



        private EffectDBEntity _Setting_EffectDB = new EffectDBEntity();
        /// <summary>
        /// 获取绑定效果内容
        /// </summary>
        public EffectDBEntity Setting_EffectDB
        {
            get {
                if (!(_Setting_EffectDB != null && !String.IsNullOrEmpty(_Setting_EffectDB.Name)))
                {
                      String EffectDBPath = Server.MapPath( String.Format("{0}Effects/{1}/EffectDB.xml", ModulePath, Settings_EffectName));
                      if (File.Exists(EffectDBPath))
                      {
                          XmlFormat xf = new XmlFormat(EffectDBPath);
                          _Setting_EffectDB = xf.ToItem<EffectDBEntity>();
                      }
                }
                return _Setting_EffectDB;
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
                if (!(_Setting_EffectSettingDB != null && _Setting_EffectSettingDB.Count >0))
                {
                    String EffectSettingDBPath = Server.MapPath(String.Format("{0}Effects/{1}/EffectSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(EffectSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(EffectSettingDBPath);
                        _Setting_EffectSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_EffectSettingDB;
            }
        }

        //public List<SettingEntity> GetSetting_EffectSettingDB(String )




        private List<SettingEntity> _Setting_ItemSettingDB = new List<SettingEntity>();
        /// <summary>
        /// 获取绑定数据设置项(非效果)
        /// </summary>
        public List<SettingEntity> Setting_ItemSettingDB
        {
            get
            {
                if (!(_Setting_ItemSettingDB != null && _Setting_ItemSettingDB.Count > 0))
                {
                    String ItemSettingDBPath = Server.MapPath(String.Format("{0}Effects/{1}/ItemSetting.xml", ModulePath, Settings_EffectName));
                    if (File.Exists(ItemSettingDBPath))
                    {
                        XmlFormat xf = new XmlFormat(ItemSettingDBPath);
                        _Setting_ItemSettingDB = xf.ToList<SettingEntity>();
                    }
                }
                return _Setting_ItemSettingDB;
            }
        }



        private string _CrmVersion = String.Empty;
        /// <summary>
        /// 引用文件版本
        /// </summary>
        public string CrmVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_CrmVersion))
                {
                    var ModuleVersion = ModuleProperty("Version");
                    string setting = GetHostSetting("CrmVersion");
                    if (!string.IsNullOrEmpty(setting))
                    {
                        _CrmVersion = String.Format("{0}.{1}", ModuleVersion, setting);
                    }
                }
                return _CrmVersion;
            }
        }

        private string GetHostSetting(string key, string defaultValue = "")
        {
            return HostController.Instance.GetString(key, defaultValue); ;
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return UserInfo.IsSuperUser || UserInfo.IsInRole("Administrators"); }
        }

        /// <summary>
        /// 管理员锁
        /// (检索目录下有无admindisplay.lock)
        /// </summary>
        public Boolean AdministratorLock
        {
            get { return File.Exists(MapPath(String.Format("{0}admindisplay.lock", ModulePath))); }
        }
        /// <summary>
        /// 显示管理员选项
        /// </summary>
        public Boolean DisplayAdminOption
        {
            get {
                Boolean display = true;
                if (AdministratorLock && !IsAdministrator)
                {
                    display = false; 
                }
                return display;
            }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public String language
        {
            get { return WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage); }
        }


        #endregion

        #region "jQuery配置属性"

        /// <summary>
        /// 开始模块jQuery
        /// </summary>
        public Boolean Settings_jQuery_Enable
        {
            get { return Settings["LayerGallery_jQuery_Enable"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_jQuery_Enable"].ToString()) ? Convert.ToBoolean(Settings["LayerGallery_jQuery_Enable"]) : false; }
        }

        /// <summary>
        /// 使用jQuery库
        /// </summary>
        public Boolean Settings_jQuery_UseHosted
        {
            get { return Settings["LayerGallery_jQuery_UseHosted"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_jQuery_UseHosted"].ToString()) ? Convert.ToBoolean(Settings["LayerGallery_jQuery_UseHosted"]) : false; }
        }

        /// <summary>
        /// jQuery库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQuery
        {
            get { return Settings["LayerGallery_jQuery_HostedjQuery"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_jQuery_HostedjQuery"].ToString()) ? Convert.ToString(Settings["LayerGallery_jQuery_HostedjQuery"]) : "https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"; }
        }

        /// <summary>
        /// jQueryUI库的地址
        /// </summary>
        public String Settings_jQuery_HostedjQueryUI
        {
            get { return Settings["LayerGallery_jQuery_HostedjQueryUI"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_jQuery_HostedjQueryUI"].ToString()) ? Convert.ToString(Settings["LayerGallery_jQuery_HostedjQueryUI"]) : "https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.4/jquery-ui.min.js"; }
        }


        #endregion


        #region "加载样式表"

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="Name"></param>
        public void BindStyleFile(String Name, String FileName)
        {
            BindStyleFile(Name, FileName, 50);
        }

        /// <summary>
        /// 绑定样式表文件
        /// </summary>
        /// <param name="Name"></param>
        public void BindStyleFile(String Name, String FileName, int priority)
        {
            if (HttpContext.Current.Items[Name] == null)
            {
                HttpContext.Current.Items.Add(Name, "true");

                ClientResourceManager.RegisterStyleSheet(Page, FileName);
            }
        }

        /// <summary>
        /// 绑定脚本文件
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FileName"></param>
        public void BindJavaScriptFile(String Name, String FileName)
        {
            BindJavaScriptFile(Name, FileName, 50);
        }

        /// <summary>
        /// 绑定脚本文件
        /// </summary>
        /// <param name="ThemeName"></param>
        public void BindJavaScriptFile(String Name, String FileName, int priority)
        {
            if (HttpContext.Current.Items[Name] == null)
            {
                HttpContext.Current.Items.Add(Name, "true");

                ClientResourceManager.RegisterScript(Page, FileName, priority);
            }
        }


        #endregion

        #region "加载XML配置文件中的脚本与样式表"
        /// <summary>
        /// XmlDB
        /// </summary>
        /// <param name="XmlDB">配置文件</param>
        /// <param name="XmlName">效果/皮肤</param>
        public void BindXmlDBToPage(EffectDBEntity XmlDB, String XmlName)
        {

            int priority = 50;

            //绑定全局附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.GlobalScript))
            {
                List<String> GlobalScripts = WebHelper.GetList(XmlDB.GlobalScript);

                foreach (String Script in GlobalScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/css/{1}", ModulePath, Script);
                            BindStyleFile(Script.Replace(".css", ""), FullFileName, priority);
                        }
                        else //if (Script.IndexOf(".js", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}Resource/js/{1}", ModulePath, Script);
                            BindJavaScriptFile(Script.Replace(".js", ""), FullFileName, priority);
                        }
                        priority++;
                    }
                }
            }
            //绑定效果附带的脚本
            if (!String.IsNullOrEmpty(XmlDB.EffectScript))
            {
                List<String> EffectScripts = WebHelper.GetList(XmlDB.EffectScript);

                foreach (String Script in EffectScripts)
                {
                    if (!String.IsNullOrEmpty(Script))
                    {
                        if (Script.IndexOf(".css", StringComparison.CurrentCultureIgnoreCase) > 0)
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/css/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindStyleFile(Script.Replace(".css", ""), FullFileName, priority);
                        }
                        else
                        {
                            String FullFileName = String.Format("{0}{1}s/{2}/js/{3}", ModulePath, XmlName, XmlDB.Name, Script);
                            BindJavaScriptFile(Script.Replace(".js", ""), FullFileName, priority);
                        }
                        priority++;
                    }
                }
            }
        }




        #endregion

        #region "加载界面脚本样式表"





        /// <summary>
        /// 加载系统的jquery
        /// </summary>
        public void LoadSystemJQuery(System.Web.UI.Control objCSS)
        {


            string ContentSrc = ResolveUrl("~/admin/Skins/jQuery.ascx");
            if (File.Exists(Server.MapPath(ContentSrc)))
            {
                SkinObjectBase ManageContent = new SkinObjectBase();
                ManageContent = (SkinObjectBase)LoadControl(ContentSrc);
                ManageContent.ModuleControl = this;
                objCSS.Controls.Add(ManageContent);//具有编辑权限才能看到模块

            }
            else
            {
                LoadScriptForJqueryAndUI(ModulePath);
            }
        }





        /// <summary>
        /// 加载显示界面脚本样式表
        /// </summary>
        public void LoadViewScript()
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null))
            {
                //LoadScriptForJqueryAndUI(ModulePath);


                LoadSystemJQuery(objCSS);



                //if (HttpContext.Current.Items["jquery-ui-CSS"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/jquery-ui-1.7.custom.css\" />";

                //    HttpContext.Current.Items.Add("jquery-ui-CSS", "true");
                //    objCSS.Controls.Add(litLink);
                //}



                //if (HttpContext.Current.Items["DNNGo_LayerGallery_Modules_css"] == null)
                //{
                //    Literal litLink = new Literal();
                //    litLink.Text = "<link  rel=\"stylesheet\" type=\"text/css\" href=\"" + ModulePath + "Resource/css/Modules.css\" />";

                //    HttpContext.Current.Items.Add("DNNGo_LayerGallery_Modules_css", "true");
                //    objCSS.Controls.Add(litLink);
                //}

            }
        }


 


        /// <summary>
        /// 加载脚本
        /// </summary>
        public void LoadScriptForJqueryAndUI(string modulePath)
        {
            System.Web.UI.Control objCSS = this.Page.FindControl("CSS");
            if ((objCSS != null) )
            {
                String jQueryUrl = String.Format("{0}Resource/js/jquery.min.js?cdv={1}", ModulePath,CrmVersion);
                String jQueryUIUrl = String.Format("{0}Resource/js/jquery-ui.min.js?cdv={1}", ModulePath,CrmVersion);
                if (Settings_jQuery_UseHosted)//使用指定的jQuery库的地址
                {
                    jQueryUrl = Settings_jQuery_HostedjQuery;
                    jQueryUIUrl = Settings_jQuery_HostedjQueryUI;
                }





                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jQueryUIRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQueryUI")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUIUrl);
                    //if (!Settings_jQuery_Enable)
                    //{

                    if (!HttpContext.Current.Items.Contains("jQueryUIRequested")) HttpContext.Current.Items.Add("jQueryUIRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQueryUI")) HttpContext.Current.Items.Add("DNNGo_jQueryUI", "true");
                    objCSS.Controls.AddAt(0, litLink);
                }

                if ((Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("jquery_registered") && !HttpContext.Current.Items.Contains("jQueryRequested")) || (Settings_jQuery_Enable && !HttpContext.Current.Items.Contains("DNNGo_jQuery")))
                {
                    Literal litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", jQueryUrl);
                    //if (!Settings_jQuery_Enable)
                    //{
                    if (!HttpContext.Current.Items.Contains("jquery_registered")) HttpContext.Current.Items.Add("jquery_registered", "true");
                    if (!HttpContext.Current.Items.Contains("jQueryRequested")) HttpContext.Current.Items.Add("jQueryRequested", "true");
                    //}
                    if (!HttpContext.Current.Items.Contains("DNNGo_jQuery")) HttpContext.Current.Items.Add("DNNGo_jQuery", "true");

                    objCSS.Controls.AddAt(0, litLink);


                    litLink = new Literal();
                    litLink.Text = String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", String.Format("{0}Resource/js/jquery-migrate.min.js?cdv={1}", ModulePath, CrmVersion));
                    objCSS.Controls.AddAt(1,litLink);
                }
            }
        }

        #endregion

        #region "加载提示语句"

        /// <summary>
        /// 显示未绑定模版的语句
        /// </summary>
        /// <returns></returns>
        public String ViewNoTemplate()
        {
            String NoTemplate = Localization.GetString("NoTemplate.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));


            return NoTemplate + ViewThemeGoUrl();
        }
        /// <summary>
        /// 显示未绑定主题时的跳转链接
        /// </summary>
        /// <returns></returns>
        public String ViewThemeGoUrl()
        {
            String ThemeGoUrl = String.Empty;
            //有编辑权限的时候，显示跳转到模版加载页
            if (IsEditable)
            {
                ThemeGoUrl = Localization.GetString("ThemeGoUrl.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
                ThemeGoUrl = ThemeGoUrl.Replace("[ThemeUrl]", xUrl("Effect_List"));
            }
            return ThemeGoUrl;
        }

        /// <summary>
        /// 未设置模块的绑定
        /// </summary>
        /// <returns></returns>
        public String ViewNoSettingBind()
        {
            return Localization.GetString("NoModuleSetting.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
        }



        /// <summary>
        /// 显示列表无数据的提示
        /// </summary>
        /// <returns></returns>
        public String ViewGridViewEmpty()
        {
            return Localization.GetString("GridViewEmpty.Message", Localization.GetResourceFile(this, "Message.ascx.resx"));
        }

        /// <summary>
        /// 绑定GridView的空信息 
        /// <param name="gvList"></param>
        public void BindGridViewEmpty<T>(GridView gvList, T t)
        {
            String EmptyDataText = ViewGridViewEmpty();
            if (gvList.Rows.Count == 0 || gvList.Rows[0].Cells[0].Text == EmptyDataText)
            {
                List<T> ss = new List<T>();
                ss.Add(t);
                gvList.DataSource = ss;
                gvList.DataBind();

                gvList.Rows[0].Cells.Clear();
                gvList.Rows[0].Cells.Add(new TableCell());
                gvList.Rows[0].Cells[0].ColumnSpan = gvList.Columns.Count;
                gvList.Rows[0].Cells[0].Text = EmptyDataText;
                gvList.Rows[0].Cells[0].Style.Add("text-align", "center");
            }
        }

        #endregion

        #region "新的后台URL"

        /// <summary>
        /// URL转换默认名
        /// </summary>
        /// <returns></returns>
        public String xUrlToken()
        {
            return "ManagerList";
        }


        public string xUrl()
        {
            return xUrl("", "", xUrlToken());
        }
        public string xUrl(string ControlKey)
        {
            return xUrl("", "", ControlKey);
        }
        public string xUrl(string KeyName, string KeyValue)
        {
            return xUrl(KeyName, KeyValue, xUrlToken());
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey)
        {
            string[] parameters = new string[] { };
            return xUrl(KeyName, KeyValue, ControlKey, parameters);
        }
        public string xUrl(string KeyName, string KeyValue, string ControlKey, params string[] AdditionalParameters)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            String language = WebHelper.GetStringParam(Request, "language", PortalSettings.DefaultLanguage);

            sb.AppendFormat("{0}Index_Manager.aspx?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", ModulePath, PortalId, TabId, ModuleId, language);

            string key = ControlKey;
            if (string.IsNullOrEmpty(key))
            {
                sb.AppendFormat("&Token={0}", xUrlToken());
            }
            else
            {
                sb.AppendFormat("&Token={0}", key);
            }
            if (!string.IsNullOrEmpty(KeyName) && !string.IsNullOrEmpty(KeyValue))
            {
                sb.AppendFormat("&{0}={1}", KeyName, KeyValue);
            }

            if (AdditionalParameters != null && AdditionalParameters.Length > 0)
            {
                foreach (String parameter in AdditionalParameters)
                {
                    sb.AppendFormat("&{0}", parameter);
                }
            }
            return sb.ToString();

        }





        #endregion

        #region "错误捕获"

        /// <summary>
        /// 错误捕获
        /// </summary>
        /// <param name="exc">错误</param>
        public void ProcessModuleLoadException(Exception exc)
        {
            if (HttpContext.Current.Session["Exception"] != null)
            {
                HttpContext.Current.Session.Remove("Exception");
            }
            //增加当前序列化的内容到Session
            HttpContext.Current.Session.Add("Exception", exc);

            if (WebHelper.GetStringParam(Request, "Token", "").ToLower() != "error")
            {
                Response.Redirect(xUrl("ReturnUrl", HttpUtility.UrlEncode(WebHelper.GetScriptUrl), "Error"), false);
            }

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
            UpdateModuleSetting(Settings_ModuleID, SettingName, SettingValue);
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

            //refresh cache
            SynchronizeModule();
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

        #endregion


        #region "绑定模版文件"
 

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="ThemeFile"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(EffectDBEntity Theme, String ThemeFile, Hashtable Puts)
        {
            TemplateFormat xf = new TemplateFormat(this);
            return ViewTemplate(Theme, ThemeFile, Puts, xf);
        }

        /// <summary>
        /// 显示模版
        /// </summary>
        /// <param name="Theme"></param>
        /// <param name="xf"></param>
        /// <param name="Puts"></param>
        /// <returns></returns>
        public String ViewTemplate(EffectDBEntity Theme, String ThemeFile, Hashtable Puts, TemplateFormat xf)
        {
            VelocityHelper vltContext = new VelocityHelper(this, Theme);


            vltContext.Put("xf", xf);//模版格式化共用方法
            vltContext.Put("ModuleID", ModuleId);//绑定的主模块编号
            vltContext.Put("TabID", TabId);//绑定的主模块页面编号

            if (Puts != null && Puts.Count > 0)
            {
                foreach (String key in Puts.Keys)
                {
                    vltContext.Put(key, Puts[key]);
                }
            }
            return vltContext.Display(ThemeFile);
        }



 

        #endregion

        #region "绑定页面标题和帮助"

        /// <summary>
        /// 显示控件标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <param name="Suffix"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewControlTitle(String Title, String DefaultValue, String ControlName, String Suffix, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
            if (!String.IsNullOrEmpty(ControlName))
            {
                System.Web.UI.Control c = FindControl(ControlName);
                if (c != null && !String.IsNullOrEmpty(c.ClientID))
                {
                    ControlName = c.ClientID;
                }
                else
                {
                    ControlName = String.Empty;
                }
            }

            return String.Format("<label  {2} {1}>{0}{3}</label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : "",
              Suffix
                );
        }




        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewTitle(Title, DefaultValue, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName)
        {
            return ViewTitle(Title, DefaultValue, ControlName, "");
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewTitle(String Title, String DefaultValue, String ControlName, String ClassName)
        {
            String Content = ViewResourceText(Title, DefaultValue);
            return ViewSpan(Content, ControlName, ClassName);
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue)
        {
            return ViewHelp(Title, DefaultValue, "");
        }

        /// <summary>
        /// 显示帮助
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewHelp(String Title, String DefaultValue, String ControlName)
        {
            String Content = ViewResourceText(Title, DefaultValue, "Help");
            return ViewSpan(Content, ControlName, "span_help");
            // return ViewSpan(String.Format("<i class=\"fa fa-info-circle\"></i>{0}",   Content), ControlName, "span_help");
        }

        /// <summary>
        /// 显示内容框
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ControlName"></param>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public String ViewSpan(String Content, String ControlName, String ClassName)
        {
            if (!String.IsNullOrEmpty(ControlName))
            {
                System.Web.UI.Control c = FindControl(ControlName);
                if (c != null && !String.IsNullOrEmpty(c.ClientID))
                {
                    ControlName = c.ClientID;
                }
                else
                {
                    ControlName = String.Empty;
                }
            }

            return String.Format("<label  {2} {1}><span {1} >{0}</span></label>",
                Content,
                !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
              !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                );
        }




        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title)
        {
            return ViewResourceText(Title, "");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue, "Text");
        }

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="TextType"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue, String TextType)
        {
            String _Title = Localization.GetString(String.Format("{0}.{1}", Title, TextType), this.LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }

        /// <summary>
        /// 显示菜单的文本
        /// </summary>
        /// <param name="MenuItem">菜单项</param>
        /// <returns></returns>
        public String ShowMenuText(TokenItem MenuItem)
        {
            return ViewResourceText(MenuItem.Token, MenuItem.Title, "MenuText");
        }


        /// <summary>
        /// 计算页面执行的时间
        /// </summary>
        /// <param name="TimeStart">开始时间</param>
        public String InitTimeSpan(DateTime TimeStart)
        {
            //查询数据库所花的时间
            System.DateTime endTime = DateTime.Now;
            System.TimeSpan ts = endTime - TimeStart;
            String RunTime = string.Format("{0}秒{1}毫秒", ts.Seconds, ts.Milliseconds);
            TimeStart = endTime = DateTime.Now;
            return RunTime;
        }

        /// <summary>
        /// 显示字段标题
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="ClassName"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewTitleSpan(String Content, String ClassName, String ControlName)
        {

            return String.Format("<label  {2}><span {1} >{0}</span></label>",
                        Content,
                        !String.IsNullOrEmpty(ClassName) ? String.Format("class=\"{0}\"", ClassName) : "",
                      !String.IsNullOrEmpty(ControlName) ? String.Format("for=\"{0}\"", ControlName) : ""
                        );
        }
        #endregion

        #region "名称格式化"
        /// <summary>
        /// 搜索条件格式化
        /// </summary>
        /// <param name="Search">搜索条件</param>
        /// <returns></returns>
        public String SearchFormat(String Search)
        {
            return String.Format("{0}-{1}-{2}-{3}", Search, Settings_ModuleID, ClientID, Settings_TabID);
        }
        #endregion



        #region "获取文件后缀名和路径"

        /// <summary>
        /// 根据后缀名显示图标
        /// </summary>
        /// <param name="FileExtension">文件后缀</param>
        /// <param name="FilePath">文件路径</param>
        /// <returns></returns>
        public String GetPhotoExtension(String FileExtension, String FilePath)
        {
            FileExtension = FileExtension.ToLower();

            //先判断是否是图片格式的
            if (FileExtension == "jpg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "png")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "jpeg")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "gif")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "bmp")
                return GetPhotoPath(FilePath);
            else if (FileExtension == "mp3")
                return GetFileIcon("audio.png");
            else if (FileExtension == "wma")
                return GetFileIcon("audio.png");
            else if (FileExtension == "zip")
                return GetFileIcon("archive.png");
            else if (FileExtension == "rar")
                return GetFileIcon("archive.png");
            else if (FileExtension == "7z")
                return GetFileIcon("archive.png");
            else if (FileExtension == "xls")
                return GetFileIcon("spreadsheet.png");
            else if (FileExtension == "txt")
                return GetFileIcon("text.png");
            else if (FileExtension == "cs")
                return GetFileIcon("code.png");
            else if (FileExtension == "html")
                return GetFileIcon("code.png");
            else if (FileExtension == "doc")
                return GetFileIcon("document.png");
            else if (FileExtension == "docx")
                return GetFileIcon("document.png");
            else
                return GetFileIcon("default.png");
        }

        /// <summary>
        /// 获取图片的路径
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns></returns>
        public String GetPhotoPath(String FilePath)
        {
            return String.Format("{0}{1}", LayerGallery_PortalSettings.HomeDirectory, FilePath);
        }

        /// <summary>
        /// 获取文件图标
        /// </summary>
        /// <param name="IconName">图标文件</param>
        /// <returns></returns>
        public String GetFileIcon(String IconName)
        {
            return String.Format("{0}Resource/images/crystal/{1}", ModulePath, IconName);
        }

        #endregion


        #region "设置默认的效果与主题"

        /// <summary>
        /// 获取默认的效果名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultEffectName()
        {
            //构造效果存放路径
            String EffectDirPath = String.Format("{0}Effects/", Server.MapPath(ModulePath));
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();//不存在就创建
            //获取当前所有的目录
            DirectoryInfo[] EffectDirs = EffectDir.GetDirectories();

            if (EffectDirs != null && EffectDirs.Length > 0)
            {
               UpdateModuleSetting("LayerGallery_EffectName", EffectDirs[0].Name);
               return EffectDirs[0].Name;
            }

            return String.Empty;
            
        }

        /// <summary>
        /// 获取默认的主题名
        /// </summary>
        /// <returns></returns>
        public String GetDefaultThemeName()
        {

            //绑定效果的主题
            String EffectDirPath = String.Format("{0}Effects/{1}/Themes/", Server.MapPath(ModulePath), Settings_EffectName);
            DirectoryInfo EffectDir = new DirectoryInfo(EffectDirPath);
            if (!EffectDir.Exists) EffectDir.Create();
            DirectoryInfo[] ThemeDirs = EffectDir.GetDirectories();
            if (ThemeDirs != null && ThemeDirs.Length > 0)
            {
                UpdateModuleSetting("LayerGallery_EffectThemeName", ThemeDirs[0].Name);
                return ThemeDirs[0].Name;
            }

            return String.Empty;
        }



        #endregion


        #region "页面加载事件-触发生成导航"

        /// <Description>
        /// 页面加载
        /// </Description>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Literal LiBreadcrumb = FindControl("LiBreadcrumb") as Literal;
                //if (LiBreadcrumb != null)
                //{
                //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //    sb.Append("		<ol class=\"breadcrumb\">").AppendLine();
                //    sb.Append("         <li>").AppendLine();
                //    sb.Append("				<i class=\"fa clip-leaf\"></i>").AppendLine();
                //    sb.AppendFormat("				<a href=\"{1}\">{0}</a>",Settings_EffectName,xUrl("Effect_List")).AppendLine();
                //    sb.Append("			</li>").AppendLine();
                //    sb.AppendFormat("         <li class=\"active\"><a href=\"{1}\">{0}</a></li>",Settings_EffectThemeName,xUrl("Effect_Options")).AppendLine();
                //    sb.Append("		</ol>").AppendLine();



                //    LiBreadcrumb.Text = sb.ToString();
                //}

            }
        }
        #endregion

        #region "DNN 920 的支持"

        #region "获取模块信息属性DNN920"

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="m">模块信息</param>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(ModuleInfo m, String Name)
        {
            bool propertyNotFound = false;
            return m.GetProperty(Name, "", System.Globalization.CultureInfo.CurrentCulture, UserInfo, DotNetNuke.Services.Tokens.Scope.DefaultSettings, ref propertyNotFound);
        }

        /// <summary>
        /// 获取模块信息属性DNN920
        /// </summary>
        /// <param name="Name">属性名</param>
        /// <returns></returns>
        public String ModuleProperty(String Name)
        {
            return ModuleProperty(ModuleConfiguration, Name);
        }

        #endregion

        #region "模块路径"
        /// <summary>
        /// 模块路径
        /// </summary>
        public String ModulePath
        {
            get { return ControlPath; }
        }

        #endregion

        #endregion

    }
}