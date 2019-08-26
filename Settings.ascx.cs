using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Common;


using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using System.Collections;
using System.Collections.Generic;
using DotNetNuke.Security;
using DotNetNuke.Common.Utilities;
using System.Web.UI.WebControls;
using DotNetNuke.Security.Permissions;

namespace DNNGo.Modules.LayerGallery
{
    public partial class Settings : basePortalModule
    {


        #region "==属性=="

        /// <summary>
        /// 模块操作类
        /// </summary>
        private static ModuleController controller = new ModuleController();





        /// <summary>提示操作类</summary>
        MessageTips mTips = new MessageTips();

        #endregion


        #region "==方法=="

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindDataToPage()
        {
            cbCopyOfOtherModule.Checked = Settings["LayerGallery_CopyOfOtherModule"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_CopyOfOtherModule"].ToString()) ? Convert.ToBoolean(Settings["LayerGallery_CopyOfOtherModule"]) : false;


            //绑定当前站点列表
            DotNetNuke.Entities.Portals.PortalController portalController = new DotNetNuke.Entities.Portals.PortalController();
            WebHelper.BindList<PortalInfo>(ddlPortals,Common.Split<PortalInfo>( portalController.GetPortals(),1,int.MaxValue), "PortalName", "PortalID");
            WebHelper.SelectedListByValue(ddlPortals, Settings["LayerGallery_CopyOfPortal"] != null && !string.IsNullOrEmpty(Settings["LayerGallery_CopyOfPortal"].ToString()) ? Convert.ToInt32(Settings["LayerGallery_CopyOfPortal"]) : PortalId);


            BindModuleList();
            WebHelper.SelectedListByValue(ddlTabModule, String.Format("{0}-{1}", Settings_TabID, Settings_ModuleID));
         
         
        }



        /// <summary>
        /// 设置数据项
        /// </summary>
        private void SetDataItem()
        {



             
            UpdateModuleSetting(ModuleId,  "LayerGallery_CopyOfOtherModule", cbCopyOfOtherModule.Checked.ToString());
            UpdateModuleSetting(ModuleId, "LayerGallery_CopyOfPortal", ddlPortals.Items[ddlPortals.SelectedIndex].Value);

        

            if ((ddlTabModule.Items.Count > 0))
            {
                string[] values = ddlTabModule.SelectedValue.Split(Convert.ToChar("-"));

                if (values.Length == 2)
                {
                    UpdateModuleSetting(ModuleId, "LayerGallery_TabID", values[0]);
                    UpdateModuleSetting(ModuleId, "LayerGallery_ModuleID", values[1]);
                }
            }

 

        }

        /// <summary>
        /// 绑定模块列表
        /// </summary>
        private void BindModuleList()
        {
            DesktopModuleInfo objDesktopModuleInfo = DesktopModuleController.GetDesktopModuleByModuleName("DNNGo.LayerGallery", PortalId);

            ddlTabModule.Items.Clear();
            if ((objDesktopModuleInfo != null))
            {
                int aPortalId = Convert.ToInt32(ddlPortals.Items[ddlPortals.SelectedIndex].Value);
                TabController objTabController = new TabController();
                ArrayList objTabs = objTabController.GetTabsByPortal(aPortalId).ToArrayList();
                foreach (DotNetNuke.Entities.Tabs.TabInfo objTab in objTabs)
                {
                    if ((objTab != null))
                    {
                        if ((objTab.IsDeleted == false))
                        {
                            ModuleController objModules = new ModuleController();
                            foreach (KeyValuePair<int, ModuleInfo> pair in objModules.GetTabModules(objTab.TabID))
                            {
                                ModuleInfo objModule = pair.Value;
                                if ((objModule.IsDeleted == false))
                                {
                                    if ((objModule.DesktopModuleID == objDesktopModuleInfo.DesktopModuleID))
                                    {
                                        if (ModulePermissionController.CanEditModuleContent(objModule) & objModule.IsDeleted == false)
                                        {
                                            string strPath = objTab.TabName;
                                            TabInfo objTabSelected = objTab;
                                            while (objTabSelected.ParentId != Null.NullInteger)
                                            {
                                                objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, false);
                                                if ((objTabSelected == null))
                                                {
                                                    break; // TODO: might not be correct. Was : Exit While
                                                }
                                                strPath = objTabSelected.TabName + " -> " + strPath;
                                            }

                                            ListItem objListItem = new ListItem();

                                            objListItem.Value = objModule.TabID.ToString() + "-" + objModule.ModuleID.ToString();
                                            objListItem.Text = strPath + " -> " + objModule.ModuleTitle;

                                            ddlTabModule.Items.Add(objListItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }








        #endregion


        #region "==事件=="


        /// <summary>
        /// 页面加载事件
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //绑定数据
                    BindDataToPage();
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        /// <summary>
        /// 更新绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // 设置需要绑定的方案项
                SetDataItem();

                mTips.LoadMessage("UpdateSettingsSuccess", EnumTips.Success, this, new String[] { "" });

                //refresh cache
                SynchronizeModule();

                Response.Redirect(xUrl("Setting"), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {

                Response.Redirect(xUrl(), true);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        /// <summary>
        /// 重新选取其他站点时
        /// </summary>
        protected void ddlPortals_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindModuleList();
        }

        #endregion

     









    }
}