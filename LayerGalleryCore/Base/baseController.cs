using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;


using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using DotNetNuke.Common.Utilities;

namespace DNNGo.Modules.LayerGallery
{
    public class baseController :  ISearchable, IPortable
    {
        #region "Optional Interfaces"

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// GetSearchItems implements the ISearchable Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModInfo">The ModuleInfo for the module to be Indexed</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            SearchItemInfoCollection SearchItemCollection = new SearchItemInfoCollection();
            //QueryParam qp = new QueryParam();
            //int RecordCount = 0;

            //qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.ModuleId, ModInfo.ModuleID, SearchType.Equal));
            //qp.Where.Add(new SearchParam(DNNGo_LayerGallery_Content._.Status, (Int32)EnumStatus.Published, SearchType.Equal));


            ////查出所有的文章
            //List<DNNGo_LayerGallery_Content> Articles = DNNGo_LayerGallery_Content.FindAll(qp, out RecordCount);

            //if(Articles!= null && Articles.Count>0)
            //{
            //    foreach (DNNGo_LayerGallery_Content ArticleItem in Articles)
            //    {

                   
            //        //拼接文章的信息
            //        SearchItemInfo SearchItem = new SearchItemInfo(ArticleItem.Title, ArticleItem.Description, ArticleItem.LastUser, ArticleItem.LastTime, ModInfo.ModuleID, ArticleItem.ID.ToString(), Common.LostHTML(ArticleItem.ContentText), "");
            //        SearchItemCollection.Add(SearchItem);
            //    }
            //}

            return SearchItemCollection;

        }




        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ExportModule implements the IPortable ExportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The Id of the module to be exported</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public string ExportModule(int ModuleID)
        {
            string strXML = String.Empty;

            ImportExportHelper ieHelper = new ImportExportHelper();
            ieHelper.ModuleID = ModuleID;
            ieHelper.UserId = 1;

            //查询字段的数据,填充待导出的XML实体
            QueryParam qp = new QueryParam();
            qp.OrderType = 0;
            Int32 RecordCount = 0;
            qp.Where.Add(new SearchParam("ModuleId", ModuleID, SearchType.Equal));
            List<DNNGo_LayerGallery_Content> ArticleList = DNNGo_LayerGallery_Content.FindAll(qp, out RecordCount);

            if (ArticleList != null && ArticleList.Count > 0)
            {
                List<GalleryContentEntity> xmlContentList = new List<GalleryContentEntity>();
                List<GallerySettingsEntity> xmlSettingList = new List<GallerySettingsEntity>();


                //查询出所有的配置项
                if (ieHelper.Settings != null && ieHelper.Settings.Count > 0)
                {
                    List<KeyValueEntity> DefaultSettings = ieHelper.GetDefaultSettings();

                    foreach (KeyValueEntity kv in DefaultSettings)
                    {
                        String key = String.Format("Global_{0}", kv.Key);
                        if (!xmlSettingList.Exists(r1 => r1.SettingName == key) && ieHelper.Settings[key] != null)
                        {
                            xmlSettingList.Add(new GallerySettingsEntity(key, Convert.ToString(ieHelper.Settings[key])));
                        }
                    }
                }

                foreach (DNNGo_LayerGallery_Content ContentItem in ArticleList)
                {
                    xmlContentList.Add(ieHelper.EntityToXml(ContentItem));
                }

    

                XmlFormat xf = new XmlFormat(HttpContext.Current.Server.MapPath(String.Format("{0}Resource/xml/Entity.xml", ieHelper.ModulePath)));
                strXML = xf.ToXml<GalleryContentEntity>(xmlContentList, xmlSettingList);
            }
            else
            {
            }

            return strXML;

        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// ImportModule implements the IPortable ImportModule Interface
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="ModuleID">The ID of the Module being imported</param>
        /// <param name="Content">The Content being imported</param>
        /// <param name="Version">The Version of the Module Content being imported</param>
        /// <param name="UserID">The UserID of the User importing the Content</param>
        /// <history>
        /// </history>
        /// -----------------------------------------------------------------------------

        public void ImportModule(int ModuleID, string Content, string Version, int UserId)
        {
            ImportExportHelper ieHelper = new ImportExportHelper();
            ieHelper.ModuleID = ModuleID;
            ieHelper.UserId = UserId;

            if (!String.IsNullOrEmpty(Content))
            {

                //将XML转换为实体
                XmlFormat xf = new XmlFormat();
                xf.XmlDoc.LoadXml(Content);
                List<GalleryContentEntity> XmlContentList = xf.ToList<GalleryContentEntity>();
                List<GallerySettingsEntity> XmlSettingList = xf.ToList<GallerySettingsEntity>();
      

                //插入成功的数量
                Int32 InsertResult = 0;

 


                //插入内容的记录
                foreach (GalleryContentEntity XmlContentItem in XmlContentList)
                {
                    DNNGo_LayerGallery_Content ContentItem =ieHelper. XmlToEntity(XmlContentItem);

                    if (ContentItem.Insert() > 0)
                    {
                        //导入项
                        ieHelper.ImportItem(ContentItem, XmlContentItem.ItemList);
 
                        InsertResult++; 
                    }

                }

                //插入设置的记录
                foreach (GallerySettingsEntity XmlSettingItem in XmlSettingList)
                {
                    if (XmlSettingItem != null && !String.IsNullOrEmpty(XmlSettingItem.SettingName) && XmlSettingItem.SettingName.IndexOf("Global_") >= 0 && XmlSettingItem.SettingValue != null)
                    {
                        ieHelper.UpdateModuleSetting(ModuleID, XmlSettingItem.SettingName, XmlSettingItem.SettingValue);
                    }
                }

            }
        }

        #endregion



 
    }
}