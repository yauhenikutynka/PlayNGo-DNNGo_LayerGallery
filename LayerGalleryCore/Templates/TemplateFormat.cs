using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using System.Web.UI.WebControls;
using System.Collections;

using DotNetNuke.Entities.Modules;
using System.IO;
using System.Web.UI;
using System.Threading;
using DotNetNuke.Common.Utilities;
using System.Web.Script.Serialization;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.LayerGallery
{
    public class TemplateFormat
    {


        #region "属性"
        /// <summary>
        /// 模块基类
        /// </summary>
        private basePortalModule bpm = new basePortalModule();

     

        private Button _ViewButton;
        /// <summary>
        /// 触发按钮
        /// </summary>
        public Button ViewButton
        {
            get { return _ViewButton; }
            set { _ViewButton = value; }
        }



        private String _ThemeXmlName = String.Empty;
        /// <summary>
        /// 主题XML名称
        /// </summary>
        public String ThemeXmlName
        {
            get { return _ThemeXmlName; }
            set { _ThemeXmlName = value; }
        }

        private PlaceHolder _PhContent = new PlaceHolder();

        public PlaceHolder PhContent
        {
            get { return _PhContent; }
            set { _PhContent = value; }
        }


        #endregion



        #region "方法"

        #region "--关于内容与标题--"

        /// <summary>
        /// 显示标题(通过资源文件)
        /// </summary>
        /// <param name="Title">标题</param>
        /// <param name="DefaultValue">资源文件未定义时默认值</param>
        /// <returns>返回值</returns>
        public String ViewTitle(String Title, String DefaultValue)
        {
            return ViewResourceText(Title, DefaultValue);
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        public String ViewContent(String FieldName, DNNGo_LayerGallery_Content DataItem)
        {
            if (DataItem != null && DataItem.ID > 0)
            {
                if ( DataItem[FieldName] != null)
                {
                    return Convert.ToString(DataItem[FieldName]);//找出一般属性
                }
            }
            return string.Empty;
        }

      

        /// <summary>
        /// 显示内容并截取数据
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="Lenght">显示长度</param>
        /// <returns></returns>
        public String ViewContent(String FieldName, DNNGo_LayerGallery_Content DataItem, Int32 Lenght)
        {
            return ViewContent(FieldName, DataItem, Lenght, "...");
        }


        /// <summary>
        /// 显示内容并截取数据
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="Lenght">显示长度</param>
        /// <param name="Suffix">终止符号</param>
        /// <returns></returns>
        public String ViewContent(String FieldName, DNNGo_LayerGallery_Content DataItem, Int32 Lenght, String Suffix)
        {
            String Content = ViewContent(FieldName, DataItem);//先取内容
            return WebHelper.leftx(Content, Lenght, Suffix);
        }

        /// <summary>
        ///  显示时间
        /// </summary>
        /// <param name="FieldName">字段名</param>
        /// <param name="DataItem">数据项</param>
        /// <param name="TimeFormat">时间格式</param>
        /// <returns></returns>
        public String ViewDateTime(String FieldName, DNNGo_LayerGallery_Content DataItem, String TimeFormat)
        { 
            String Content = ViewContent(FieldName, DataItem);//先取内容
            DateTime Temp = xUserTime.UtcTime();
            if (DateTime.TryParse(Content, out Temp))
                return Temp.ToString(TimeFormat);
            else
                return String.Empty;

        }







        /// <summary>
        /// 显示JSON格式的标题
        /// </summary>
        /// <param name="DataList"></param>
        /// <returns></returns>
        public String ViewTitleToJson(List<DNNGo_LayerGallery_Content> DataList)
        {
            StringBuilder Titles = new StringBuilder();

            foreach (DNNGo_LayerGallery_Content Content in DataList)
            {
                if (!String.IsNullOrEmpty(Content.Title))
                {

                    if (Titles.Length > 0)
                    {
                        Titles.Append(",");
                    }
                    Titles.AppendFormat("'{0}'", Content.Title.Replace("'", "\\'").Replace("\"", "\\\""));
                }
            }
            return Titles.ToString();
        }



 

        /// <summary>
        /// 显示URL控件存放的值
        /// </summary>
        /// <param name="UrlValue"></param>
        /// <returns></returns>
        public String ViewLinkUrl(String UrlValue, int PortalId)
        {
            String DefaultValue = String.Empty;
            if (!String.IsNullOrEmpty(UrlValue) && UrlValue != "0")
            {
                if (UrlValue.IndexOf("FileID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    int FileID = 0;
                    if (int.TryParse(UrlValue.Replace("FileID=", ""), out FileID) && FileID > 0)
                    {
                        var fi = FileManager.Instance.GetFile(FileID);
                        if (fi != null && fi.FileId > 0)
                        {
                            DefaultValue = string.Format("{0}{1}{2}", bpm.LayerGallery_PortalSettings.HomeDirectory, fi.Folder, bpm.Server.UrlPathEncode(fi.FileName));
                        }
                    }
                }
                else if (UrlValue.IndexOf("MediaID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    DefaultValue = String.Format("{0}Resource/images/no_image.png", bpm.ModulePath);

                    int MediaID = 0;
                    if (int.TryParse(UrlValue.Replace("MediaID=", ""), out MediaID) && MediaID > 0)
                    {
                        DNNGo_LayerGallery_Files Multimedia = DNNGo_LayerGallery_Files.FindByID(MediaID);
                        if (Multimedia != null && Multimedia.ID > 0)
                        {
                            DefaultValue = bpm.Server.UrlPathEncode(bpm.GetPhotoPath(Multimedia.FilePath));// String.Format("{0}{1}", bpm.LayerGallery_PortalSettings.HomeDirectory, Multimedia.FilePath);
                        }
                    }
                }
                else if (UrlValue.IndexOf("TabID=", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {

                    DefaultValue = Globals.NavigateURL(Convert.ToInt32(UrlValue.Replace("TabID=", "")), false, bpm.LayerGallery_PortalSettings, Null.NullString, "", "");

                }
                else
                {
                    DefaultValue = UrlValue;
                }
            }
            return DefaultValue;
        
        }


        public String ViewLinkUrl(String UrlValue)
        {
            return ViewLinkUrl(UrlValue, bpm.Settings_PortalID);
        }


       
        #endregion


        #region "--关于内容与标题的字典值--"

        /// <summary>
        /// 读取设置的字典信息
        /// </summary>
        /// <param name="DataItem">Slider对象</param>
        /// <returns></returns>
        public Dictionary<String, Object> ViewDicts(DNNGo_LayerGallery_Content DataItem)
        {
            Dictionary<String, Object> Slider_Options = new Dictionary<string, Object>();
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                Slider_Options = jsSerializer.DeserializeObject(DataItem.Options) as Dictionary<String, Object>;
            }
            return Slider_Options;
        }

        /// <summary>
        /// 读取设置的字典信息
        /// </summary>
        /// <param name="DataItem">Layer对象</param>
        /// <returns></returns>
        public Dictionary<String, Object> ViewDicts(DNNGo_LayerGallery_Item DataItem)
        {
            Dictionary<String, Object> Layer_Options = new Dictionary<string, Object>();
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                Layer_Options = jsSerializer.DeserializeObject(DataItem.Options) as Dictionary<String, Object>;
            }
            return Layer_Options;
        }


        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="DictName"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public Object GetValue(Dictionary<String, Object> Dicts, String DictName, Object DefaultValue)
        {
            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                if (Dicts[DictName].GetType() == typeof(Object[]))
                {
                    var dicts = Dicts[DictName] as Object[];
                    if (dicts != null && dicts.Length > 0)
                    {
                        return Common.GetStringByList(dicts);
                    }
                }
 
                return Dicts[DictName];
 
            }
            return DefaultValue;
        }

        public Object GetValueByItem0(Dictionary<String, Object> Dicts, String DictName, Object DefaultValue)
        {
            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                if (Dicts[DictName].GetType() == typeof(Object[]))
                {
                    var dicts = Dicts[DictName] as Object[];
                    if (dicts != null && dicts.Length > 0)
                    {
                        return dicts[0];
                    }
                }

                return Dicts[DictName];

            }
            return DefaultValue;
        }


        public Object GetValueByStringItems(Dictionary<String, Object> Dicts, String DictName, Object DefaultValue)
        {
            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                if (Dicts[DictName].GetType() == typeof(Object[]))
                {
                    var dicts = Dicts[DictName] as Object[];
                    if (dicts != null && dicts.Length > 0)
                    {
                        List<String> Items = new List<string>();
                        foreach (var dict in dicts)
                        {
                            Items.Add(String.Format("'{0}'", dict));
                        }
                        return Common.GetStringByList(Items);
                    }
                }

                return "";

            }
            return DefaultValue;
        }



        /// <summary>
        /// 格式化值
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="DictName"></param>
        /// <param name="DefaultValue"></param>
        /// <param name="_FormatValue"></param>
        /// <returns></returns>
        public String FormatValue(Dictionary<String, Object> Dicts, String DictName, Object DefaultValue, String _FormatValue)
        {
            String ReturnValue = String.Empty;
            Object o = GetValue(Dicts, DictName, DefaultValue);
            if (o != null)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(o)) && !String.IsNullOrEmpty(_FormatValue) && _FormatValue.IndexOf("{0}")>=0)
                {
                    ReturnValue = String.Format(_FormatValue.Replace("\\", ""), o);
                }
            }
            return ReturnValue;
        }



        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="DictName"></param>
        /// <param name="ChildDictName"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public Object GetValue(Dictionary<String, Object> Dicts, String DictName, String ChildDictName, Object DefaultValue)
        {

            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                Dictionary<String, Object> ChildDicts = Dicts[DictName] as Dictionary<String, Object>;
                if (ChildDicts != null && ChildDicts.Count > 0 && ChildDicts.ContainsKey(ChildDictName))
                {
                    return ChildDicts[ChildDictName];
                }
            }
            return DefaultValue;
        }



        public String FormatValue(Dictionary<String, Object> Dicts, String DictName, String ChildDictName, Object DefaultValue, String _FormatValue)
        {
            String ReturnValue = String.Empty;
            Object o = GetValue(Dicts, DictName,ChildDictName, DefaultValue);
            if (o != null)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(o)) && !String.IsNullOrEmpty(_FormatValue) && _FormatValue.IndexOf("{0}") > 0)
                {
                    ReturnValue = String.Format(_FormatValue.Replace("\\", ""), o);
                }
            }
            return ReturnValue;
        }



        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="DictName"></param>
        /// <param name="ChildDictName"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public Object GetValue(Dictionary<String, Object> Dicts,Int32 DictIndex, String DictName, String ChildDictName, Object DefaultValue)
        {

            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                object[] ChildDicts = Dicts[DictName] as object[];
                if (ChildDicts != null && ChildDicts.Length > 0 && DictIndex < ChildDicts.Length)
                {
                    Dictionary<String, Object> ChildDictList = ChildDicts[DictIndex] as Dictionary<String, Object>;
                    if (ChildDictList != null && ChildDictList.Count > 0 && ChildDictList.ContainsKey(ChildDictName))
                    {
                       
                        return ChildDictList[ChildDictName];
                    }
                }
            }
            return DefaultValue;
        }


        public String FormatValue(Dictionary<String, Object> Dicts, Int32 DictIndex, String DictName, String ChildDictName, Object DefaultValue, String _FormatValue)
        {
            String ReturnValue = String.Empty;
            Object o = GetValue(Dicts,DictIndex, DictName, ChildDictName, DefaultValue);
            if (o != null)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(o)) && !String.IsNullOrEmpty(_FormatValue) && _FormatValue.IndexOf("{0}") > 0)
                {
                    ReturnValue = String.Format(_FormatValue.Replace("\\",""), o);
                }
            }
            return ReturnValue;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="DictIndex"></param>
        /// <param name="DictName"></param>
        /// <param name="ChildDictName"></param>
        /// <returns></returns>
        public String GetListToString(Dictionary<String, Object> Dicts, Int32 DictIndex, String DictName)
        {
            StringBuilder ReturnValue = new StringBuilder();

            if (Dicts != null && Dicts.Count > 0 && Dicts.ContainsKey(DictName))
            {
                object[] ChildDicts = Dicts[DictName] as object[];
                if (ChildDicts != null && ChildDicts.Length > 0 && DictIndex < ChildDicts.Length)
                {
                    Dictionary<String, Object> ChildDictList = ChildDicts[DictIndex] as Dictionary<String, Object>;
                    if (ChildDictList != null && ChildDictList.Count > 0)
                    {
                        foreach (var ChildDict in ChildDictList)
                        {
                            if (!String.IsNullOrEmpty(ChildDict.Key) && !(ChildDict.Key.IndexOf("Classes") >= 0))
                            {
                                ReturnValue.AppendFormat("{0}:{1};", ChildDict.Key.Trim(),Convert.ToString( ChildDict.Value).Trim());
                            }
                        }
                     
                    }
                }
            }
            return ReturnValue.ToString();
        }

        /// <summary>
        /// 移除掉需要移除的字符
        /// </summary>
        /// <param name="_Value"></param>
        /// <param name="RemoveString"></param>
        /// <returns></returns>
        public String Rt(String _Value, String RemoveString)
        {
            if (!String.IsNullOrEmpty(_Value) && _Value.IndexOf(RemoveString, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
               _Value = Common.ReplaceNoCase(_Value, RemoveString, "");
            }

           return _Value;
        }

        /// <summary>
        /// 处理URL
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public String Link(Object o)
        {
            String s = String.Empty;
            if (o != null)
            {
                s = Convert.ToString(o);

                if (!String.IsNullOrEmpty(s))
                {
                    s = s.Replace(" ", "%20");
                }
            }
            return s;
        }


        /// <summary>
        /// 随机输出（传入逗号相隔的列表）
        /// </summary>
        /// <param name="StringList"></param>
        /// <returns></returns>
        public String RandomTo(String StringList)
        {
            string result = StringList;
            if (!String.IsNullOrEmpty(StringList) && StringList.IndexOf(",") >=0)
            {
                List<String> list = WebHelper.GetList(StringList);
                result = RandomTo(list);
            }
            return result;
        }


        /// <summary>
        /// 随机输出
        /// </summary>
        /// <param name="StringList"></param>
        /// <returns></returns>
        public String RandomTo(List<String> StringList)
        {
            string result = String.Empty;
            if(StringList!= null && StringList.Count >0)
            {
                result = StringList[new Random().Next(0,StringList.Count - 1)];
            }
            return result;
            
        }

        /// <summary>
        /// 是否输出视频标签
        /// </summary>
        /// <returns></returns>
        public String ViewVideolayer(Dictionary<String, Object> Dicts)
        {
            String Result = String.Empty;

            String Contentoption_Type = Convert.ToString( GetValue(Dicts, 0, "contentoption", "type", "image"));
            if (!String.IsNullOrEmpty(Contentoption_Type) && Contentoption_Type == "video")
            {
                Result = "tp-videolayer";
            }

            return Result;
            //return "tp-videolayer";
        }


        #endregion


        #region "--关于链接跳转--"

        /// <summary>
        /// 返回到列表
        /// </summary>
        /// <returns></returns>
        public String GoUrl()
        {
            return Globals.NavigateURL(bpm.Settings_TabID);
        }


 


 

        /// <summary>
        /// 跳转到登录页面
        /// </summary>
        /// <returns></returns>
        public String GoLogin()
        {
            return  Globals.NavigateURL(bpm.PortalSettings.LoginTabId, "Login", "returnurl=" +  HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
        }
 

        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl(String goUrl)
        {
            return String.Format("http://{0}{1}",WebHelper.GetHomeUrl(), goUrl);
        }
        /// <summary>
        /// 填充为完整的URL
        /// </summary>
        public String GoFullUrl()
        {
            return String.Format("http://{0}", WebHelper.GetHomeUrl());
        }





        #endregion


     









        #region "--关于图片--"
 

        /// <summary>
        /// 显示相片的缩略图片
        /// </summary>
        /// <param name="DataItem"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public String ThumbnailUrl(DNNGo_LayerGallery_Content DataItem, object width, object height, object mode, string phototype = "p")
        {
            return String.Format("{0}Resource_Service.aspx?PortalId={1}&TabId={2}&ID={3}&width={4}&height={5}&mode={6}&type={7}", bpm.ModulePath, bpm.Settings_PortalID, bpm.Settings_TabID, DataItem.ID, width, height, mode, phototype);
        }
        /// <summary>
        /// 显示相片的缩略图片
        /// </summary>
        /// <param name="DataItem"></param>
        /// <returns></returns>
        public String ThumbnailUrl(DNNGo_LayerGallery_Content DataItem)
        {
            return ThumbnailUrl(DataItem, 200, 200, "AUTO");
        }

 
        #endregion












        #region "--关于用户--"

        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(Int32 UserID, String FieldName)
        {
            return ViewUser(UserID, FieldName, String.Empty);
        }

        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(Int32 UserID, String FieldName, String DefaultValue)
        {
            UserInfo uInfo = new UserController().GetUser(bpm.PortalId, UserID);
            return ViewUser(uInfo, FieldName, DefaultValue);
        }


        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="uInfo"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(DotNetNuke.Entities.Users.UserInfo uInfo, String FieldName)
        {
            return ViewUser(uInfo, FieldName, String.Empty);
        }


        /// <summary>
        /// 显示用户信息
        /// </summary>
        /// <param name="uInfo"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public String ViewUser(DotNetNuke.Entities.Users.UserInfo uInfo, String FieldName, String DefaultValue)
        {
            String FieldValue = DefaultValue;
            if (uInfo != null && uInfo.UserID > 0 && !String.IsNullOrEmpty(FieldName))
            {

                switch (FieldName.ToLower())
                {
                    case "username": FieldValue = uInfo.Username; break;
                    case "email": FieldValue = uInfo.Email; break;
                    case "firstName": FieldValue = uInfo.FirstName; break;
                    case "lastname": FieldValue = uInfo.LastName; break;
                    case "displayname": FieldValue = uInfo.DisplayName; break;
                    default: FieldValue = DefaultValue; break;
                }
            }
            return FieldValue;
        }


 


 


 

 



        #endregion

 

        #region "--XML参数读取--"

 


        /// <summary>
        /// 读取XML参数
        /// </summary>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewXmlSetting(String Name, object DefaultValue)
        {
            String SettingKey = String.Format("Global_{0}", Name);// bpm.EffectSettingsFormat(bpm.Settings_EffectName, Name);
            return bpm.Settings[SettingKey] != null ? ConvertTo.FormatValue(bpm.Settings[SettingKey].ToString(), DefaultValue.GetType()) : DefaultValue;
        }


        /// <summary>
        /// 读取数据项参数
        /// </summary>
        /// <param name="DataItem">数据项</param>
        /// <param name="Name">参数名</param>
        /// <param name="DefaultValue">默认值</param>
        /// <returns></returns>
        public object ViewItemSetting(DNNGo_LayerGallery_Content DataItem, String Name, object DefaultValue)
        {
            object o = DefaultValue;
            if (DataItem != null && DataItem.ID > 0 && !String.IsNullOrEmpty(DataItem.Options))
            {
                try
                {
                    List<KeyValueEntity> ItemSettings = ConvertTo.Deserialize<List<KeyValueEntity>>(DataItem.Options);
                    KeyValueEntity KeyValue = ItemSettings.Find(r1 => r1.Key.ToLower() == Name.ToLower());
                    if (KeyValue != null && !String.IsNullOrEmpty(KeyValue.Key))
                    {
                       o = KeyValue.Value;
                    }

                }
                catch
                {
                     
                }
            }
            return o;
        }

        /// <summary>
        /// 将字符串转化为列表,逗号为分隔符
        /// </summary>
        /// <param name="Items"></param>
        /// <returns></returns>
        public List<String> ToList(String Items)
        {
            List<String> list = new List<String>();
            if(!String.IsNullOrEmpty(Items))
            {
                list = WebHelper.GetList(Items);
            }
            return list;
        }

        /// <summary>
        /// 过滤掉空格
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public String ToTrim(String _Value)
        {
            if (!String.IsNullOrEmpty(_Value))
            {
                return _Value.Replace(" ", "");
            }
            return String.Empty;
        }


        /// <summary>
        /// 过滤掉空格
        /// </summary>
        /// <param name="_Value"></param>
        /// <returns></returns>
        public String ToTrim19(String _Value)
        {
            if (!String.IsNullOrEmpty(_Value))
            {
                return _Value.Replace(" ", "").Replace(",", " ");
            }
            return String.Empty;
        }

 


        #endregion


        #region "--关于模版--"
        /// <summary>
        /// 引用脚本文件
        /// </summary>
        /// <param name="Name">JS名称</param>
        /// <param name="FileName">JS文件(已包含主题路径)</param>
        public void IncludeScript(String Name, String FileName)
        {
            String FullFileName = String.Format("{0}{1}", ThemePath, FileName);
            bpm.BindJavaScriptFile(Name, FullFileName);
        }

        /// <summary>
        /// 引用脚本文件
        /// </summary>
        /// <param name="Name">JS名称</param>
        /// <param name="FileName">JS文件(已包含主题路径)</param>
        public void IncludeResourceScript(String Name, String FileName)
        {
            String FullFileName = String.Format("{0}Resource/{1}", bpm.ModulePath, FileName);
            bpm.BindJavaScriptFile(Name, FullFileName);
        }


        #endregion


        #region "--关于模版--"

        private String _ThemePath = String.Empty;
        /// <summary>
        /// 当前模版路径
        /// </summary>
        public String ThemePath
        {
            get {
                if (String.IsNullOrEmpty(_ThemePath))
                {


                    _ThemePath = String.Format("{0}Effects/{1}/", bpm.ModulePath, bpm.Settings_EffectName);

                }
                return _ThemePath;
            }
        }


        #endregion



        #endregion



        #region "构造"

        /// <summary>
        /// 显示资源文件内容
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public String ViewResourceText(String Title, String DefaultValue)
        {
            String _Title = Localization.GetString(String.Format("{0}.Text", Title), bpm.LocalResourceFile);
            if (String.IsNullOrEmpty(_Title))
            {
                _Title = DefaultValue;
            }
            return _Title;
        }


        public TemplateFormat(basePortalModule _bpm)
        {
            bpm = _bpm;
        }

        public TemplateFormat()
        {
            
        }

        #endregion

    }
}