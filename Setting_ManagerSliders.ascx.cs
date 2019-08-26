using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.LayerGallery
{
    public partial class Setting_ManagerSliders : basePortalModule
    {
        #region "属性"



        #endregion



        #region "方法"

        /// <summary>
        /// 打印ajax的urls
        /// </summary>
        /// <param name="Method"></param>
        /// <returns></returns>
        public String ViewAjaxUrl(String Method = "get", String JsonType = "Settings")
        {
            return String.Format("{0}Resource_Ajax.aspx?PortalId={1}&ModuleId={2}&TabId={3}&Method={4}&Type={5}", ModulePath, PortalId, ModuleId, TabId, Method, JsonType);
        }

        /// <summary>
        /// 显示框架网址
        /// </summary>
        /// <returns></returns>
        public String ViewIframeUrl()
        {
            return String.Format("{0}Resource_FeaturedImage.aspx?PortalId={1}&ModuleId={2}&TabId={3}&FileType=slider", ModulePath, PortalId, ModuleId, TabId);
        }

        #endregion




        #region "事件"

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime LocalTime = xUserTime.LocalTime();
            liFormatData.Text = String.Format("<div id=\"format_data{0}\" data-starttime=\"{1}\" data-endtime=\"{2}\"></div>", ModuleId, LocalTime.ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)), LocalTime.AddYears(10).ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)));
           


        }

        #endregion



    }
}