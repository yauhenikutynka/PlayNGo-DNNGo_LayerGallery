using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using System.Collections;
using System.Collections.Specialized;

namespace DNNGo.Modules.LayerGallery
{
    public partial class View_Info : basePortalModule 
    {


        #region "扩展属性"

        /// <summary>
        /// 页码
        /// </summary>
        public Int32 PageIndex
        {
            get { return WebHelper.GetIntParam(HttpContext.Current.Request, String.Format("PageIndex{0}", ModuleId), 1); }
        }


        #endregion


        #region "方法"

        /// <summary>
        /// 绑定数据项到前台
        /// </summary>
        public void BindDataItem(EffectDBEntity EffectDB)
        {
            List<DNNGo_LayerGallery_Content> DataList = new List<DNNGo_LayerGallery_Content>();
            Hashtable Puts = new Hashtable();
            TemplateFormat xf = new TemplateFormat(this);
            xf.PhContent = PhContent;

            //读取需要载入的参数
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.ModuleId, Settings_ModuleID, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.Status, (Int32)EnumStatus.Published, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.StartTime, xUserTime.UtcTime(), SearchType.LtEqual));
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.EndTime,xUserTime.UtcTime(), SearchType.GtEqual));
            qp.OrderType = 0;


            if (EffectDB.Pager)//传入分页的数量
            {
                qp.PageSize = (Int32)xf.ViewXmlSetting("PageSize", 9999);
                qp.PageSize = qp.PageSize <= 0 ? 9999 : qp.PageSize;
                qp.PageIndex = PageIndex;
            }



            if (Settings_Sortby == (Int32)EnumSortby.Name)
            {
                qp.Orderfld = DNNGo_LayerGallery_Content._.Title;
                DataList = DNNGo_LayerGallery_Content.FindAll(qp, out  RecordCount);
            }
            else if (Settings_Sortby == (Int32)EnumSortby.Name_Desc)
            {
                qp.OrderType = 1;
                qp.Orderfld = DNNGo_LayerGallery_Content._.Title;
                DataList = DNNGo_LayerGallery_Content.FindAll(qp, out  RecordCount);
            }
            else if (Settings_Sortby == (Int32)EnumSortby.Time)
            {
                qp.Orderfld = DNNGo_LayerGallery_Content._.StartTime;
                DataList = DNNGo_LayerGallery_Content.FindAll(qp, out  RecordCount);
            }
            else if (Settings_Sortby == (Int32)EnumSortby.Time_Desc)
            {
                qp.OrderType = 1;
                qp.Orderfld = DNNGo_LayerGallery_Content._.StartTime;
                DataList = DNNGo_LayerGallery_Content.FindAll(qp, out  RecordCount);
            }
            else if (Settings_Sortby == (Int32)EnumSortby.Random)
            {
                qp.Orderfld = " newid() ";
                DataList = DNNGo_LayerGallery_Content.FindRandomAll(qp, out  RecordCount);
            }
            else
            {
                qp.Orderfld = DNNGo_LayerGallery_Content._.Sort;
                DataList = DNNGo_LayerGallery_Content.FindAll(qp, out  RecordCount);
            }

            List<DNNGo_LayerGallery_Content> tList = new List<DNNGo_LayerGallery_Content>();
            //查看需要过滤掉非激活的选项
            foreach (var content in DataList)
            {

                String Activate = Convert.ToString(xf.GetValue(xf.ViewDicts(content), "activate", "on"));
                if (String.IsNullOrEmpty(Activate) || Activate.ToLower() == "on")
                {
                    tList.Add(content);
                }
            }



            Puts.Add("DataList", tList);
            Puts.Add("EffectName", Settings_EffectName);
            Puts.Add("ThemeName", Settings_EffectThemeName);


            //加载LayerList
            Puts.Add("LayerList", GetLayerList());




            if (EffectDB.Pager && RecordCount > qp.PageSize)
            {
                Puts.Add("Pager", new Pager(qp.PageIndex, qp.PageSize, ModuleId, RecordCount, EnumPageType.DnnURL).CreateHtml());//分页
            }
            else
            {
                Puts.Add("Pager", "");
            }

            String HtmlContent = ViewTemplate(EffectDB, "Effect.html", Puts, xf);

            
            //#if RELEASE  //Release状态下需要格式化HTML代码
                HtmlContent = CodeDeal.DealCode(HtmlContent);//格式化HTML
            //#endif
            liContent.Text = HtmlContent;


        }

        /// <summary>
        /// 获取所有的层列表
        /// </summary>
        /// <returns></returns>
        public List<DNNGo_LayerGallery_Item> GetLayerList()
        {
            QueryParam qp = new QueryParam();
            qp.Orderfld = DNNGo_LayerGallery_Item._.Sort;
            qp.OrderType = 0;

            int RecordCount = 0;
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Item._.ModuleId, Settings_ModuleID, SearchType.Equal));
            qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Item._.Status, (Int32)EnumStatus.Published, SearchType.Equal));
            return DNNGo_LayerGallery_Item.FindAll(qp, out  RecordCount);
        }



        #endregion



        #region "事件"
 
        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
               


                if (!String.IsNullOrEmpty(Settings_EffectName))
                {

                    if (!String.IsNullOrEmpty(Settings_EffectThemeName))
                    {
                         EffectDBEntity EffectDB = Setting_EffectDB;
                        if (!IsPostBack)
                        {
                            //绑定数据项到前台
                            BindDataItem(EffectDB);

                        }

                        //需要载入当前设置效果的主题CSS文件
                        String ThemeName = String.Format("{0}_{1}", Settings_EffectName, Settings_EffectThemeName);
                        String ThemePath = String.Format("{0}Effects/{1}/Themes/{2}/Style.css", ModulePath, Settings_EffectName, Settings_EffectThemeName);
                        BindStyleFile(ThemeName, ThemePath);


                        BindXmlDBToPage(EffectDB, "Effect");
 

                    }
                    else
                    {
                        //未定义效果对应的主题
                        liContent.Text = "";
                    }
                }
                else
                {
                    //未绑定效果
                    liContent.Text = "";
                }
 
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

 

        #endregion

 
    }
}