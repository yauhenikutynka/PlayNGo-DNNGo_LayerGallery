using System;
using System.Collections.Generic;
using System.Web;
using System.ComponentModel;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// 效果实体(XML & 序列化)
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("相册")]
    [XmlEntityAttributes("DNNGo_LayerGallery//GalleryItemEntityList//GalleryItemEntityItem")]
    public class GalleryItemEntity
    {
        #region 属性

        ///<summary>
        /// 项标题
        ///</summary>
        public String Title { get; set; }
 

        ///<summary>
        /// 选项集合
        ///</summary>
        public String Options { get; set; }

      
        ///<summary>
        /// 排序
        ///</summary>
        public Int32 Sort { get; set; }


        ///<summary>
        /// 状态
        ///</summary>
        public Int32 Status { get; set; }
 

        #endregion
 
    }
}