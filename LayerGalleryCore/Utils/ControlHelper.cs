using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// 控件操作类
    /// </summary>
    public class ControlHelper
    {

        private Int32 _ModuleId = 0;
        /// <summary>
        /// 模块编号
        /// </summary>
        public Int32 ModuleId
        {
            get { return _ModuleId; }
            set { _ModuleId = value; }
        }


        public ControlHelper(Int32 __ModuleId)
        {
            ModuleId = __ModuleId;
        }





        #region "--关于控件格式化--"

        /// <summary>
        /// 显示输入控件内容
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewControl(SettingEntity FieldItem)
        {
            String ControlName = ViewControlName(FieldItem);
            String ControlID = ViewControlID(FieldItem);
            String ControlHtml = String.Empty;//控件的HTML
            if (FieldItem.ControlType == EnumControlType.CheckBox.ToString())
                ControlHtml = ViewCreateCheckBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.CheckBoxList.ToString())
                ControlHtml = ViewCreateCheckBoxList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.DatePicker.ToString())
                ControlHtml = ViewCreateDatePicker(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.DropDownList.ToString())
                ControlHtml = ViewCreateDropDownList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.FileUpload.ToString())
                ControlHtml = ViewCreateFileUpload(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.Label.ToString())
                ControlHtml = ViewCreateLabel(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.ListBox.ToString())
                ControlHtml = ViewCreateListBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.RadioButtonList.ToString())
                ControlHtml = ViewCreateRadioButtonList(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.RichTextBox.ToString())
                ControlHtml = ViewCreateRichTextBox(FieldItem, ControlName, ControlID);
            else if (FieldItem.ControlType == EnumControlType.TextBox.ToString() || FieldItem.ControlType.IndexOf("text") >= 0)
                ControlHtml = ViewCreateTextBox(FieldItem, ControlName, ControlID);

            return ControlHtml;



        }


        #region "创建HTML控件方法集合"
        /// <summary>
        /// 创建TextBox
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewCreateTextBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {

            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            //看行数决定控件的是什么控件
            if (FieldItem.Rows > 1)
            {
                ControlHtml.AppendFormat("<textarea  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"form-control form_default input_text {0}\"", ViewVerification(FieldItem));

                ControlHtml.AppendFormat(" style=\"width:{0}px;height:{1}px;\" rows=\"{2}\"", FieldItem.Width, FieldItem.Rows * 20, FieldItem.Rows);

                ControlHtml.Append(" >");

                //默认值
                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.Append(FieldItem.DefaultValue);

                ControlHtml.Append("</textarea>");
            }
            else
            {
                ControlHtml.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

                //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

                ControlHtml.AppendFormat(" class=\"form-control form_default input_text {0}\"", ViewVerification(FieldItem));

                ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

                if (!String.IsNullOrEmpty(FieldItem.DefaultValue)) ControlHtml.AppendFormat(" value=\"{0}\"", FieldItem.DefaultValue);

                ControlHtml.Append(" />");
            }

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML


            ControlHtml.AppendFormat("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"grey {0}\"", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.DefaultValue))
            {
                Boolean DefaultValue, b; DefaultValue = b = false;
                if (FieldItem.DefaultValue == "1" || FieldItem.DefaultValue.Equals(Boolean.TrueString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = true;
                else if (FieldItem.DefaultValue == "0" || FieldItem.DefaultValue.Equals(Boolean.FalseString, StringComparison.CurrentCultureIgnoreCase))
                    DefaultValue = false;
                else if (Boolean.TryParse(FieldItem.DefaultValue.ToLower(), out b))
                    DefaultValue = b;

                if (DefaultValue) ControlHtml.Append("checked=\"checked\"");
            }

            ControlHtml.Append(" />");

            //提示的关键字用作是后面的描述
            //if (!String.IsNullOrEmpty(FieldItem.ToolTip))
            //{
            //    ControlHtml.AppendFormat("<label for=\"{0}\" title=\"{1}\" style=\"display:inline;\">{1}</label>", ControlID, FieldItem.ToolTip);
            //}
            //if (!String.IsNullOrEmpty(FieldItem.Description))
            //{
            //    ControlHtml.AppendFormat("<label for=\"{0}\" title=\"{1}\" style=\"display:inline;\">{1}</label>", ControlID, FieldItem.Description);
            //}

            //ControlHtml.Append("</div>");

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateCheckBoxList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{1}\" name=\"{0}\" >", ControlName, ControlID);



            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = WebHelper.GetList(FieldItem.ListContent.Replace("\r\n", ",").Replace("\r", ","));
                List<String> DefaultListValue = WebHelper.GetList(FieldItem.DefaultValue);
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(list[i]) ? "checked=\"checked\"" : "";

                        ControlHtml.AppendFormat("<input id=\"{1}_{2}\" type=\"checkbox\" name=\"{0}\"", ControlName, ControlID, i);

                        if (FieldItem.Required) ControlHtml.AppendFormat(" class=\"grey {0}\" ", "validate[minCheckbox[1]]");

                        ControlHtml.AppendFormat("value=\"{0}\" {1} />", list[i], CheckedStr);

                        ControlHtml.AppendFormat("<label for=\"{0}_{1}\" style=\"display:inline;\">{2}</label>", ControlID, i, list[i]);

                        if (FieldItem.Direction == EnumControlDirection.Vertical.ToString()) ControlHtml.Append("<br />");
                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDatePicker(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID));

            ControlHtml.Append("<script type=\"text/javascript\">");
            ControlHtml.Append("jQuery(document).ready(function(){").AppendLine();
            ControlHtml.Append("	    jQuery(function() {").AppendLine();
            ControlHtml.AppendFormat("		    jQuery(\"#{0}\").datepicker({{showButtonPanel: true,changeMonth: true,changeYear: true}});", ControlID).AppendLine();
            ControlHtml.Append("	    });").AppendLine();
            ControlHtml.Append("});").AppendLine();
            ControlHtml.Append("</script>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateDropDownList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

            ControlHtml.AppendFormat(" class=\"form-control form_default input_text {0}\">", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = WebHelper.GetList(FieldItem.ListContent.Replace("\r\n", ",").Replace("\r", ","));
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String CheckedStr = FieldItem.DefaultValue.IndexOf(list[i], StringComparison.CurrentCultureIgnoreCase) >= 0 ? "selected=\"selected\"" : "";
                        ControlHtml.AppendFormat("<option {0} value=\"{1}\">{1}</option>", CheckedStr, list[i]);
                    }
                }
            }
            ControlHtml.Append(" </select>");
            return ControlHtml.ToString();
        }



 
 






        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateFileUpload(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<input type=\"file\" name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(" class=\"file-input validate[{0}ajax[ajaxUpload]]\"", FieldItem.Required ? "required," : "");

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width < 250 ? 250 : FieldItem.Width);

            ControlHtml.Append(" />");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateLabel(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML

            ControlHtml.AppendFormat("<span  name=\"{0}\" id=\"{1}\"", ControlName, ControlID);
            //if (!String.IsNullOrEmpty(FieldItem.ToolTip)) ControlHtml.AppendFormat(" title=\"{0}\"", FieldItem.ToolTip);

            ControlHtml.AppendFormat(">{0}</span>", FieldItem.DefaultValue);
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateListBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<select name=\"{0}\" id=\"{1}\"", ControlName, ControlID);

            ControlHtml.AppendFormat(" class=\"form-control input_text {0}\"", ViewVerification(FieldItem));

            ControlHtml.AppendFormat(" style=\"width:{0}px;\"", FieldItem.Width);

            ControlHtml.AppendFormat(" size=\"{0}\" multiple=\"multiple\">", FieldItem.Rows);

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = WebHelper.GetList(FieldItem.ListContent.Replace("\r\n", ",").Replace("\r", ","));
                List<String> DefaultListValue = WebHelper.GetList(FieldItem.DefaultValue);
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String CheckedStr = DefaultListValue.Count > 0 && DefaultListValue.Contains(list[i]) ? "selected=\"selected\"" : "";
                        ControlHtml.AppendFormat("<option {0} value=\"{1}\">{1}</option>", CheckedStr, list[i]);
                    }
                }
            }
            ControlHtml.Append(" </select>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateRadioButtonList(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.AppendFormat("<span id=\"{0}\" ", ControlID);

            ControlHtml.AppendFormat(" class=\"{0}\" >", ViewVerification(FieldItem));

            if (!String.IsNullOrEmpty(FieldItem.ListContent))
            {
                List<String> list = WebHelper.GetList(FieldItem.ListContent.Replace("\r\n", ",").Replace("\r", ","));
                for (Int32 i = 0; i < list.Count; i++)
                {
                    if (!String.IsNullOrEmpty(list[i]))
                    {
                        String CheckedStr = FieldItem.DefaultValue.IndexOf(list[i], StringComparison.CurrentCultureIgnoreCase) >= 0 ? "checked=\"checked\"" : "";
                        ControlHtml.AppendFormat("<input id=\"{1}_{2}\" class=\"grey\" type=\"radio\" name=\"{0}\" value=\"{3}\" {4} /><label for=\"{1}_{2}\" style=\"display:inline;\">{3}</label>", ControlName, ControlID, i, list[i], CheckedStr);
                        if (FieldItem.Direction == EnumControlDirection.Vertical.ToString()) ControlHtml.Append("<br />");
                    }
                }
            }

            ControlHtml.Append(" </span>");
            return ControlHtml.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        public String ViewCreateRichTextBox(SettingEntity FieldItem, String ControlName, String ControlID)
        {
            StringBuilder ControlHtml = new StringBuilder();//控件的HTML
            ControlHtml.Append(ViewCreateTextBox(FieldItem, ControlName, ControlID));


            ControlHtml.Append("<script type=\"text/javascript\">").AppendLine();
            ControlHtml.Append("jQuery(function ($) {").AppendLine();
            ControlHtml.AppendFormat(" 	$('#{0}').xheditor({{skin:'nostyle',tools:'simple'}});", ControlID).AppendLine();
            ControlHtml.Append(" });").AppendLine();
            ControlHtml.Append("</script>").AppendLine();

            //ControlHtml.Append("<script>");
            //ControlHtml.Append("var editor;").AppendLine();
            //ControlHtml.Append("KindEditor.ready(function (K) {").AppendLine();
            //ControlHtml.AppendFormat(" 	editor = K.create('#{0}', {{", ControlID).AppendLine();
            //ControlHtml.Append("		afterBlur: function(){this.sync();},allowPreviewEmoticons: false,allowImageUpload: false,").AppendLine();
            //ControlHtml.Append("	    items: [ 'source', '|','fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline','removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist','insertunorderedlist', '|', 'emoticons', 'image', 'link']").AppendLine();
            //ControlHtml.Append("        });").AppendLine();
            //ControlHtml.Append(" });").AppendLine();
            //ControlHtml.Append("</script>");

            return ControlHtml.ToString();
        }

        /// <summary>
        /// 验证字符
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewVerification(SettingEntity FieldItem)
        {
            //无验证时退出
            //if (FieldItem.Verification == EnumVerification.optional.ToString() && !FieldItem.Required) return "";

            String custom = String.Empty;
            if (!String.IsNullOrEmpty(FieldItem.Verification) && FieldItem.Verification != EnumVerification.optional.ToString())
            {
                if (!(("DropDownList,ListBox,RadioButtonList,FileUpload,CheckBox,CheckBoxList,Label").IndexOf(FieldItem.ControlType) >= 0))
                {
                    custom = String.Format("custom[{0}]", FieldItem.Verification);
                }
            }
            if (!String.IsNullOrEmpty(custom))
            {
                if (!FieldItem.Required)
                    return String.Format("validate[{0}]", String.IsNullOrEmpty(custom) ? "optional" : custom);
                else
                    return String.Format("validate[required,{0}]", custom);
            }
            else
            {
                if (FieldItem.Required)
                    return "validate[required]";
                else
                    return "";
            }

        }


        #endregion



        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem">显示字段</param>
        /// <param name="Suffix">后缀名</param>
        /// <returns></returns>
        public String ViewLable(SettingEntity FieldItem, String Suffix)
        {
            String ControlName = ViewControlID(FieldItem);
            return String.Format("<label for=\"{0}\">{1}{2}</label>", ControlName, FieldItem.Alias, Suffix);
        }
        /// <summary>
        /// 显示标题控件
        /// </summary>
        /// <param name="FieldItem"></param>
        /// <returns></returns>
        public String ViewLable(SettingEntity FieldItem)
        {
            return ViewLable(FieldItem, ""); ;
        }

        /// <summary>
        /// 显示控件名
        /// </summary>
        /// <param name="FieldItem">字段</param>
        /// <returns></returns>
        public String ViewControlName(SettingEntity FieldItem)
        {
            return String.Format("Ctl${0}${1}", FieldItem.Name, ModuleId);
        }

        public String ViewControlID(SettingEntity FieldItem)
        {
            return String.Format("Ctl_{0}_{1}", FieldItem.Name, ModuleId);
        }



        #endregion


        #region 获取Form传值
        /// <summary>
        /// 获取Form传值
        /// </summary>
        /// <param name="fieldItem"></param>
        /// <returns></returns>
        public static String GetWebFormValue(SettingEntity fieldItem, basePortalModule bpm)
        {
            String WebFormValue = String.Empty;

            //创建控件的Name和ID
            ControlHelper ControlItem = new ControlHelper(bpm.ModuleId);
            String ControlName = ControlItem.ViewControlName(fieldItem);
            String ControlID = ControlItem.ViewControlID(fieldItem);

            if (fieldItem.ControlType == EnumControlType.CheckBox.ToString())
            {
                WebFormValue = WebHelper.GetStringParam(HttpContext.Current.Request, ControlName, "");
                WebFormValue = !String.IsNullOrEmpty(WebFormValue) && WebFormValue == "on" ? "true" : "false";
            }
            else if (fieldItem.ControlType == EnumControlType.FileUpload.ToString())
            {
                HttpPostedFile hpFile = HttpContext.Current.Request.Files[ControlName];
                if (hpFile != null && hpFile.ContentLength > 0) WebFormValue = String.Format("Url://{0}", FileSystemUtils.UploadFile(hpFile, bpm));                //存放到目录中，并返回
            }
            else
            {
                WebFormValue = WebHelper.GetStringParam(HttpContext.Current.Request, ControlName, "");
            }
            return WebFormValue;

        }

        #endregion
    }
}