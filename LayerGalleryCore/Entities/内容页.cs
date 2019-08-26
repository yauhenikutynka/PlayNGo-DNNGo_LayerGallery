using System;
using System.Collections.Generic;
using System.ComponentModel;
 

namespace DNNGo.Modules.LayerGallery
{
	/// <summary>
	/// 内容页
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("内容页")]
	[BindTable("DNNGo_LayerGallery_Content", Description = "内容页", ConnName = "SiteSqlServer")]
	public partial class DNNGo_LayerGallery_Content : Entity<DNNGo_LayerGallery_Content>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 编号
		/// </summary>
		[Description("编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private String _Title;
		/// <summary>
		/// 标题
		/// </summary>
		[Description("标题")]
		[DataObjectField(false, false, false, 200)]
		[BindColumn("Title", Description = "标题", DefaultValue = "", Order = 2)]
		public String Title
		{
			get { return _Title; }
			set { if (OnPropertyChange("Title", value)) _Title = value; }
		}

		private String _Options;
		/// <summary>
		/// 选项集合
		/// </summary>
		[Description("选项集合")]
		[DataObjectField(false, false, true, 1073741823)]
		[BindColumn("Options", Description = "选项集合", DefaultValue = "", Order = 3)]
		public String Options
		{
			get { return _Options; }
			set { if (OnPropertyChange("Options", value)) _Options = value; }
		}

		private Int32 _Sort;
		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("Sort", Description = "排序", DefaultValue = "", Order = 4)]
		public Int32 Sort
		{
			get { return _Sort; }
			set { if (OnPropertyChange("Sort", value)) _Sort = value; }
		}

		private Int32 _ModuleId;
		/// <summary>
		/// 模块编号
		/// </summary>
		[Description("模块编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 5)]
		public Int32 ModuleId
		{
			get { return _ModuleId; }
			set { if (OnPropertyChange("ModuleId", value)) _ModuleId = value; }
		}

		private Int32 _PortalId;
		/// <summary>
		/// 站点编号
		/// </summary>
		[Description("站点编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 6)]
		public Int32 PortalId
		{
			get { return _PortalId; }
			set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
		}

		private Int32 _Status;
		/// <summary>
		/// 状态
		/// </summary>
		[Description("状态")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Status", Description = "状态", DefaultValue = "", Order = 7)]
		public Int32 Status
		{
			get { return _Status; }
			set { if (OnPropertyChange("Status", value)) _Status = value; }
		}

        private DateTime _StartTime = xUserTime.UtcTime();
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("StartTime", Description = "开始时间", DefaultValue = "getdate()", Order = 8)]
		public DateTime StartTime
		{
			get { return xUserTime.LocalTime(_StartTime); }
			set { if (OnPropertyChange("StartTime", value)) _StartTime = xUserTime.ServerTime(value); }
		}

        private DateTime _EndTime = xUserTime.UtcTime().AddYears(10);
		/// <summary>
		/// 结束时间
		/// </summary>
		[Description("结束时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("EndTime", Description = "结束时间", DefaultValue = "dateadd(year,(10),getdate())", Order = 9)]
		public DateTime EndTime
		{
			get { return xUserTime.LocalTime(_EndTime); }
			set { if (OnPropertyChange("EndTime", value)) _EndTime = xUserTime.ServerTime(value); }
		}

		private Int32 _LastUser;
		/// <summary>
		/// 更新用户
		/// </summary>
		[Description("更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 10)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

		private String _LastIP;
		/// <summary>
		/// 更新IP
		/// </summary>
		[Description("更新IP")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 11)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
		}

		private DateTime _LastTime;
		/// <summary>
		/// 更新时间
		/// </summary>
		[Description("更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 12)]
		public DateTime LastTime
		{
			get { return _LastTime; }
			set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
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
					case "Title" : return _Title;
					case "Options" : return _Options;
					case "Sort" : return _Sort;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					case "Status" : return _Status;
					case "StartTime" : return _StartTime;
					case "EndTime" : return _EndTime;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					case "LastTime" : return _LastTime;
					default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "Title" : _Title = Convert.ToString(value); break;
					case "Options" : _Options = Convert.ToString(value); break;
					case "Sort" : _Sort = Convert.ToInt32(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					case "Status" : _Status = Convert.ToInt32(value); break;
					case "StartTime" : _StartTime = Convert.ToDateTime(value); break;
					case "EndTime" : _EndTime = Convert.ToDateTime(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
					default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得内容页字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 标题
			///</summary>
			public const String Title = "Title";

			///<summary>
			/// 选项集合
			///</summary>
			public const String Options = "Options";

			///<summary>
			/// 排序
			///</summary>
			public const String Sort = "Sort";

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 站点编号
			///</summary>
			public const String PortalId = "PortalId";

			///<summary>
			/// 状态
			///</summary>
			public const String Status = "Status";

			///<summary>
			/// 开始时间
			///</summary>
			public const String StartTime = "StartTime";

			///<summary>
			/// 结束时间
			///</summary>
			public const String EndTime = "EndTime";

			///<summary>
			/// 更新用户
			///</summary>
			public const String LastUser = "LastUser";

			///<summary>
			/// 更新IP
			///</summary>
			public const String LastIP = "LastIP";

			///<summary>
			/// 更新时间
			///</summary>
			public const String LastTime = "LastTime";
		}
		#endregion
	}
}