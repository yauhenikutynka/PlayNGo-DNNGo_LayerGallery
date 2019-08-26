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
    [XmlEntityAttributes("DNNGo_LayerGallery//GalleryContentEntityList//GalleryContentEntityItem")]
    public class GalleryContentEntity
    {
        #region 属性
 
        /// <summary>标题</summary>
        public String Title { get; set; }


        /// <summary>排序</summary>
        public Int32 Sort { get; set; }
  
        /// <summary>状态</summary>
        public Int32 Status { get; set; }

        /// <summary>选项集合</summary>
        public String Options { get; set; }


        private DateTime _StartTime = xUserTime.UtcTime();
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }


        private DateTime _EndTime = xUserTime.UtcTime().AddYears(10);
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        /// <summary>
        /// 项的集合
        /// </summary>
        public String ItemList { get; set; }
 
        #endregion
 
    }
}