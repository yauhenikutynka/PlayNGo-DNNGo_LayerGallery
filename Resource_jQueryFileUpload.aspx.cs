using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;

namespace DNNGo.Modules.LayerGallery
{
    public partial class Resource_jQueryFileUpload : BasePage
    {

  


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //验证作者状态
                VerificationAuthor();

                if (!(Request.UrlReferrer != null && !String.IsNullOrEmpty(Request.UrlReferrer.ToString()) && Request.UrlReferrer.ToString() != String.Format("http://{0}{1}", WebHelper.GetScriptName, WebHelper.GetScriptUrl)))
                {
                    Response.Redirect(Globals.NavigateURL(TabId));
                }

            }
        }



        protected override void Page_Init(System.Object sender, System.EventArgs e)
        {
            //调用基类Page_Init，主要用于权限验证
            base.Page_Init(sender, e);
        }
    }
}