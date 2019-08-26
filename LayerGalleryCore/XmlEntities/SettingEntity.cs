using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// 设置参数
    /// </summary>
    [XmlEntityAttributes("Settings//Setting")]
    public class GlobalSetting
    {

        private String _Name = String.Empty;
        /// <summary>
        /// 参数名
        /// </summary>
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }


        private String _Value = String.Empty;
        /// <summary>
        /// 默认值
        /// </summary>
        public String Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

    }
}