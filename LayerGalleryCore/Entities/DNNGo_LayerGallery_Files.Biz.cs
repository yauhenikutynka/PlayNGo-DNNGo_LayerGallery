﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

 
 

namespace DNNGo.Modules.LayerGallery
{
 
    
    /// <summary>多媒体</summary>
    public partial class DNNGo_LayerGallery_Files: Entity<DNNGo_LayerGallery_Files> 
    {
        #region 对象操作
        static DNNGo_LayerGallery_Files()
        {
            // 用于引发基类的静态构造函数，所有层次的泛型实体类都应该有一个
            DNNGo_LayerGallery_Files entity = new DNNGo_LayerGallery_Files();
        }
 
        #endregion

        #region 扩展属性
        #endregion

        #region 扩展查询
        /// <summary>根据媒体编号查找</summary>
        /// <param name="id">媒体编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static DNNGo_LayerGallery_Files FindByID(Int32 id)
        {
            return Find(_.ID, id);
        }




        /// <summary>
        /// 根据状态统计数量
        /// </summary>
        /// <param name="PortalId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static Int32 FindCountByType(Int32 PortalId, Int32 type)
        {
            QueryParam qp = new QueryParam();

            qp.Where.Add(new SearchParam(_.PortalId, PortalId, SearchType.Equal));

            qp.Where = ByType(qp.Where, type);
           
            return FindCount(qp);
        }
        /// <summary>
        /// 构造搜索条件文件类型
        /// </summary>
        /// <param name="Where"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<SearchParam> ByType(List<SearchParam> Where, Int32 type)
        {
            if (type == (Int32)EnumFileMate.Image)
            {
                Where.Add(new SearchParam(_.FileExtension, "'jpg','png','gif','bmp'", SearchType.In));
            }
            else if (type == (Int32)EnumFileMate.Audio)
            {
                Where.Add(new SearchParam(_.FileExtension, "'mp3'", SearchType.In));
            }
            else if (type == (Int32)EnumFileMate.Video)
            {
                Where.Add(new SearchParam(_.FileExtension, "'mp4','flv'", SearchType.In));
            }
            else if (type == (Int32)EnumFileMate.Doc)
            {
                Where.Add(new SearchParam(_.FileExtension, "'doc','xls','ppt'", SearchType.In));
            }
            else if (type == (Int32)EnumFileMate.Zip)
            {
                Where.Add(new SearchParam(_.FileExtension, "'zip','rar'", SearchType.In));
            }
            return Where;
        }


        #endregion

   

        #region 扩展操作
        #endregion

        #region 业务




 


        #endregion
    }
}