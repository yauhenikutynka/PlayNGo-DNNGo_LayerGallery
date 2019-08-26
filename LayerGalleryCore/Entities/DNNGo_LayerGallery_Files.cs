using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
 
 
 

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>多媒体</summary>
    [Serializable]
    [DataObject]
    [Description("多媒体")]
    [BindTable("DNNGo_LayerGallery_Files", Description = "多媒体", ConnName = "SiteSqlServer")]
    public partial class DNNGo_LayerGallery_Files : Entity<DNNGo_LayerGallery_Files>
    {
        #region 属性
        private Int32 _ID = 0;
        /// <summary>媒体编号</summary>
        [DisplayName("媒体编号")]
        [Description("媒体编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "ID", "媒体编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChange("ID", value)) { _ID = value;  } }
        }

        private String _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        [DataObjectField(false, false, true, 256)]
        [BindColumn("Name", Description = "名称", DefaultValue = "", Order = 2)]
        public String Name
        {
            get { return _Name; }
            set { if (OnPropertyChange("Name", value)) _Name = value; }
        }

        private String _FileName = String.Empty;
        /// <summary>文件名</summary>
        [DisplayName("文件名")]
        [Description("文件名")]
        [DataObjectField(false, false, false, 128)]
        [BindColumn(3, "FileName", "文件名", null, "nvarchar(128)", 0, 0, true)]
        public virtual String FileName
        {
            get { return _FileName; }
            set { if (OnPropertyChange("FileName", value)) { _FileName = value; } }
        }

        private String _FileMate = String.Empty;
        /// <summary>Mate</summary>
        [DisplayName("Mate")]
        [Description("Mate")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(4, "FileMate", "Mate", null, "nvarchar(32)", 0, 0, true)]
        public virtual String FileMate
        {
            get { return _FileMate; }
            set { if (OnPropertyChange("FileMate", value)) { _FileMate = value;  } }
        }

        private String _FilePath = String.Empty;
        /// <summary>文件路径</summary>
        [DisplayName("文件路径")]
        [Description("文件路径")]
        [DataObjectField(false, false, false, 256)]
        [BindColumn(5, "FilePath", "文件路径", null, "nvarchar(256)", 0, 0, true)]
        public virtual String FilePath
        {
            get { return _FilePath; }
            set { if (OnPropertyChange("FilePath", value)) { _FilePath = value;  } }
        }

        private String _FileExtension = String.Empty;
        /// <summary>后缀名</summary>
        [DisplayName("后缀名")]
        [Description("后缀名")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(6, "FileExtension", "后缀名", null, "nvarchar(32)", 0, 0, true)]
        public virtual String FileExtension
        {
            get { return _FileExtension; }
            set { if (OnPropertyChange("FileExtension", value)) { _FileExtension = value;  } }
        }

        private Int32 _FileSize;
        /// <summary>
        /// 文件大小(KB)
        /// </summary>
        [Description("文件大小(KB)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("FileSize", Description = "文件大小(KB)", DefaultValue = "0", Order = 7)]
        public Int32 FileSize
        {
            get { return _FileSize; }
            set { if (OnPropertyChange("FileSize", value)) _FileSize = value; }
        }

        private Int32 _ImageWidth;
        /// <summary>
        /// 图片的宽度
        /// </summary>
        [Description("图片的宽度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("ImageWidth", Description = "图片的宽度", DefaultValue = "0", Order = 8)]
        public Int32 ImageWidth
        {
            get { return _ImageWidth; }
            set { if (OnPropertyChange("ImageWidth", value)) _ImageWidth = value; }
        }

        private Int32 _ImageHeight;
        /// <summary>
        /// 图片的高度
        /// </summary>
        [Description("图片的高度")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("ImageHeight", Description = "图片的高度", DefaultValue = "0", Order = 9)]
        public Int32 ImageHeight
        {
            get { return _ImageHeight; }
            set { if (OnPropertyChange("ImageHeight", value)) _ImageHeight = value; }
        }

        private String _Exif;
        /// <summary>
        /// 相片的扩展信息
        /// </summary>
        [Description("相片的扩展信息")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn("Exif", Description = "相片的扩展信息", DefaultValue = "", Order = 10)]
        public String Exif
        {
            get { return _Exif; }
            set { if (OnPropertyChange("Exif", value)) _Exif = value; }
        }
 
         private Int32 _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Sort", Description = "排序", DefaultValue = "0", Order = 11)]
        public Int32 Sort
        {
            get { return _Sort; }
            set { if (OnPropertyChange("Sort", value)) _Sort = value; }
        }

        private Int32 _Status = 1;
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Status", Description = "状态", DefaultValue = "1", Order = 12)]
        public Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChange("Status", value)) _Status = value; }
        }


        private DateTime _LastTime = xUserTime.UtcTime();
        /// <summary>更新时间</summary>
        [DisplayName("更新时间")]
        [Description("更新时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(13, "LastTime", "更新时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastTime
        {
            get { return _LastTime; }
            set { if (OnPropertyChange("LastTime", value)) { _LastTime = value; } }
        }

        private Int32 _LastUser = 0;
        /// <summary>更新用户</summary>
        [DisplayName("更新用户")]
        [Description("更新用户")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(14, "LastUser", "更新用户", null, "int", 10, 0, false)]
        public virtual Int32 LastUser
        {
            get { return _LastUser; }
            set { if (OnPropertyChange("LastUser", value)) { _LastUser = value; } }
        }

        private String _LastIP = "127.0.0.1";
        /// <summary>更新IP</summary>
        [DisplayName("更新IP")]
        [Description("更新IP")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(15, "LastIP", "更新IP", null, "varchar(32)", 0, 0, false)]
        public virtual String LastIP
        {
            get { return _LastIP; }
            set { if (OnPropertyChange("LastIP", value)) { _LastIP = value; } }
        }

        private Int32 _ModuleId = 0;
        /// <summary>模块编号</summary>
        [DisplayName("模块编号")]
        [Description("模块编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(16, "ModuleId", "模块编号", null, "int", 10, 0, false)]
        public virtual Int32 ModuleId
        {
            get { return _ModuleId; }
            set { if (OnPropertyChange("ModuleId", value)) { _ModuleId = value; } }
        }

        private Int32 _PortalId = 0;
        /// <summary>站点编号</summary>
        [DisplayName("站点编号")]
        [Description("站点编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(17, "PortalId", "站点编号", null, "int", 10, 0, false)]
        public virtual Int32 PortalId
        {
            get { return _PortalId; }
            set { if (OnPropertyChange("PortalId", value)) { _PortalId = value; } }
        }

        private Int16 _Extension1;
        /// <summary>
        /// 扩展字段1 (tinyint)
        /// </summary>
        [Description("扩展字段1 (tinyint)")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Extension1", Description = "扩展字段1 (tinyint)", DefaultValue = "0", Order = 18)]
        public Int16 Extension1
        {
            get { return _Extension1; }
            set { if (OnPropertyChange("Extension1", value)) _Extension1 = value; }
        }

        private Int32 _Extension2;
        /// <summary>
        /// 扩展字段2 (int)
        /// </summary>
        [Description("扩展字段2 (int)")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Extension2", Description = "扩展字段2 (int)", DefaultValue = "0", Order = 19)]
        public Int32 Extension2
        {
            get { return _Extension2; }
            set { if (OnPropertyChange("Extension2", value)) _Extension2 = value; }
        }

        private String _Extension3;
        /// <summary>
        /// 扩展字段3 (nvarchar512)
        /// </summary>
        [Description("扩展字段3 (nvarchar512)")]
        [DataObjectField(false, false, true, 512)]
        [BindColumn("Extension3", Description = "扩展字段3 (nvarchar512)", DefaultValue = "", Order = 20)]
        public String Extension3
        {
            get { return _Extension3; }
            set { if (OnPropertyChange("Extension3", value)) _Extension3 = value; }
        }

        private String _Extension4;
        /// <summary>
        /// 扩展字段4 (ntext)
        /// </summary>
        [Description("扩展字段4 (ntext)")]
        [DataObjectField(false, false, true, 1073741823)]
        [BindColumn("Extension4", Description = "扩展字段4 (ntext)", DefaultValue = "", Order = 21)]
        public String Extension4
        {
            get { return _Extension4; }
            set { if (OnPropertyChange("Extension4", value)) _Extension4 = value; }
        }
		#endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "ID" : return _ID;
                    case "FileName" : return _FileName;
                    case "FileMate" : return _FileMate;
                    case "FilePath" : return _FilePath;
                    case "FileExtension" : return _FileExtension;
                    case "LastTime" : return _LastTime;
                    case "LastUser" : return _LastUser;
                    case "LastIP" : return _LastIP;
                    case "ModuleId" : return _ModuleId;
                    case "PortalId" : return _PortalId;
                    case "Name": return _Name;
                    case "FileSize": return _FileSize;
                    case "ImageWidth": return _ImageWidth;
                    case "ImageHeight": return _ImageHeight;
                    case "Exif": return _Exif;
                    case "Status": return _Status;
                    case "Sort": return _Sort;
                    case "Extension1": return _Extension1;
                    case "Extension2": return _Extension2;
                    case "Extension3": return _Extension3;
                    case "Extension4": return _Extension4;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "ID" : _ID = Convert.ToInt32(value); break;
                    case "FileName" : _FileName = Convert.ToString(value); break;
                    case "FileMate" : _FileMate = Convert.ToString(value); break;
                    case "FilePath" : _FilePath = Convert.ToString(value); break;
                    case "FileExtension" : _FileExtension = Convert.ToString(value); break;
                    case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
                    case "LastUser" : _LastUser = Convert.ToInt32(value); break;
                    case "LastIP" : _LastIP = Convert.ToString(value); break;
                    case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
                    case "PortalId" : _PortalId = Convert.ToInt32(value); break;
                    case "Name": _Name = Convert.ToString(value); break;
                    case "FileSize": _FileSize = Convert.ToInt32(value); break;
                    case "ImageWidth": _ImageWidth = Convert.ToInt32(value); break;
                    case "ImageHeight": _ImageHeight = Convert.ToInt32(value); break;
                    case "Exif": _Exif = Convert.ToString(value); break;
                    case "Status": _Status = Convert.ToInt32(value); break;
                    case "Sort": _Sort = Convert.ToInt32(value); break;
                    case "Extension1": _Extension1 = Convert.ToInt16(value); break;
                    case "Extension2": _Extension2 = Convert.ToInt32(value); break;
                    case "Extension3": _Extension3 = Convert.ToString(value); break;
                    case "Extension4": _Extension4 = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得多媒体字段信息的快捷方式</summary>
        public class _
        {
            ///<summary>媒体编号</summary>
            public const String ID = ("ID");

            ///<summary>
            /// 名称
            ///</summary>
            public const String Name = "Name";

            ///<summary>文件名</summary>
            public const String FileName = ("FileName");

            ///<summary>Mate</summary>
            public const String FileMate = ("FileMate");

            ///<summary>文件路径</summary>
            public const String FilePath = ("FilePath");

            ///<summary>后缀名</summary>
            public const String FileExtension = ("FileExtension");


            ///<summary>
            /// 文件大小(KB)
            ///</summary>
            public const String FileSize = "FileSize";

            ///<summary>
            /// 图片的宽度
            ///</summary>
            public const String ImageWidth = "ImageWidth";

            ///<summary>
            /// 图片的高度
            ///</summary>
            public const String ImageHeight = "ImageHeight";

            ///<summary>
            /// 相片的扩展信息
            ///</summary>
            public const String Exif = "Exif";

            ///<summary>
            /// 排序
            ///</summary>
            public const String Sort = "Sort";

            ///<summary>
            /// 状态
            ///</summary>
            public const String Status = "Status";



            ///<summary>更新时间</summary>
            public const String LastTime = ("LastTime");

            ///<summary>更新用户</summary>
            public const String LastUser = ("LastUser");

            ///<summary>更新IP</summary>
            public const String LastIP = ("LastIP");

            ///<summary>模块编号</summary>
            public const String ModuleId = ("ModuleId");

            ///<summary>站点编号</summary>
            public const String PortalId = ("PortalId");
 
            ///<summary>
            /// 扩展字段1 (tinyint)
            ///</summary>
            public const String Extension1 = "Extension1";

            ///<summary>
            /// 扩展字段2 (int)
            ///</summary>
            public const String Extension2 = "Extension2";

            ///<summary>
            /// 扩展字段3 (nvarchar512)
            ///</summary>
            public const String Extension3 = "Extension3";

            ///<summary>
            /// 扩展字段4 (ntext)
            ///</summary>
            public const String Extension4 = "Extension4";
            
        }
        #endregion
    }

 
}