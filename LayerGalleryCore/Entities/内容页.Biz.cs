using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
 

namespace DNNGo.Modules.LayerGallery
{
	/// <summary>
	/// 内容页
	/// </summary>
	public partial class DNNGo_LayerGallery_Content : Entity<DNNGo_LayerGallery_Content>
	{
		#region 对象操作
		//基类Entity中包含三个对象操作：Insert、Update、Delete
		//你可以重载它们，以改变它们的行为
		//如：
		/*
		/// <summary>
		/// 已重载。把该对象插入到数据库。这里可以做数据插入前的检查
		/// </summary>
		/// <returns>影响的行数</returns>
		public override Int32 Insert()
		{
			return base.Insert();
		}
		 * */
		#endregion
		
		#region 扩展属性
		//TODO: 本类与哪些类有关联，可以在这里放置一个属性，使用延迟加载的方式获取关联对象

		/*
		private Category _Category;
		/// <summary>该商品所对应的类别</summary>
		public Category Category
		{
			get
			{
				if (_Category == null && CategoryID > 0 && !Dirtys.ContainKey("Category"))
				{
					_Category = Category.FindByKey(CategoryID);
					Dirtys.Add("Category", true);
				}
				return _Category;
			}
			set { _Category = value; }
		}
		 * */
		#endregion

		#region 扩展查询


        /// <summary>
        /// 根据方案编号查询该方案下所有的字段
        /// </summary>
        /// <param name="ProjectID">方案编号</param>
        /// <returns></returns>
        public static List<DNNGo_LayerGallery_Content> FindAllByModuleID(object ModuleID)
        {
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Orderfld = _.Sort;
            qp.OrderType = 0;
            qp.Where.Add(new SearchParam(_.ModuleId, ModuleID, SearchType.Equal));
 
            return FindAll(qp, out RecordCount);
        }

		/// <summary>
		/// 根据主键查询一个内容页实体对象用于表单编辑
		/// </summary>
		///<param name="__ID">编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static DNNGo_LayerGallery_Content FindByKeyForEdit(Int32 __ID)
		{
			DNNGo_LayerGallery_Content entity=Find(new String[]{_.ID}, new Object[]{__ID});
			if (entity == null)
			{
				entity = new DNNGo_LayerGallery_Content();
			}
			return entity;
		}     

		/// <summary>
		/// 根据编号查找
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static DNNGo_LayerGallery_Content FindByID(Int32 id)
		{
			return Find(_.ID, id);
			// 实体缓存
			//return Meta.Cache.Entities.Find(_.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
		}
		#endregion

		#region 高级查询
		/// <summary>
		/// 查询满足条件的记录集，分页、排序
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>实体集</returns>
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public static List<DNNGo_LayerGallery_Content> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
		}

		/// <summary>
		/// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
		/// </summary>
		/// <param name="key">关键字</param>
		/// <param name="orderClause">排序，不带Order By</param>
		/// <param name="startRowIndex">开始行，0开始</param>
		/// <param name="maximumRows">最大返回行数</param>
		/// <returns>记录数</returns>
		public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
		{
		    return FindCount(SearchWhere(key), null, null, 0, 0);
		}

		/// <summary>
		/// 构造搜索条件
		/// </summary>
		/// <param name="key">关键字</param>
		/// <returns></returns>
		private static String SearchWhere(String key)
		{
            if (String.IsNullOrEmpty(key)) return null;
            key = key.Replace("'", "''");
            String[] keys = key.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		    StringBuilder sb = new StringBuilder();
		    sb.Append("1=1");

            //if (!String.IsNullOrEmpty(name)) sb.AppendFormat(" And {0} like '%{1}%'", _.Name, name.Replace("'", "''"));

            for (int i = 0; i < keys.Length; i++)
            {
                sb.Append(" And ");

                if (keys.Length > 1) sb.Append("(");
                Int32 n = 0;
                foreach (FieldItem item in Meta.Fields)
                {
                    if (item.Property.PropertyType != typeof(String)) continue;
                    // 只要前五项
                    if (++n > 5) break;

                    if (n > 1) sb.Append(" Or ");
                    sb.AppendFormat("{0} like '%{1}%'", item.Name, keys[i]);
                }
                if (keys.Length > 1) sb.Append(")");
            }

            if (sb.Length == "1=1".Length)
                return null;
            else
                return sb.ToString();
		}




        /// <summary>
        /// 根据状态统计数量
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static Int32 FindCountByStatus(Int32 ModuleId, Int32 Status)
        {
            QueryParam qp = new QueryParam();
            qp.Where.Add(new SearchParam(_.ModuleId, ModuleId, SearchType.Equal));
            if (Status >= 0)
            {
                qp.Where.Add(new SearchParam(_.Status, Status, SearchType.Equal));
            }

            return FindCount(qp);
        }

        /// <summary>
        /// 查找所有的项
        /// </summary>
        /// <param name="basePage"></param>
        /// <returns></returns>
        public static List<DNNGo_LayerGallery_Content> FindAll(BasePage basePage)
        {
            QueryParam qp = new QueryParam();
            int RecordCount = 0;
            qp.Orderfld = _.Sort;
            qp.OrderType = 0;

            qp.Where.Add(new SearchParam(_.ModuleId, basePage.ModuleId, SearchType.Equal));

            return FindAll(qp, out RecordCount);
        }


		#endregion

        #region 扩展操作

        /// <summary>
        /// 移动字段
        /// </summary>
        /// <param name="objTab">待移动的字段</param>
        /// <param name="type">移动类型</param>
        /// <param name="ProjectID"></param>
        public static void MoveField(DNNGo_LayerGallery_Content objTab, EnumMoveType type, object ModuleID)
        {
            List<DNNGo_LayerGallery_Content> siblingTabs = FindAllByModuleID(ModuleID);
            int siblingCount = siblingTabs.Count;
            int tabIndex = 0;
            UpdateTabOrder(siblingTabs, 2);
            switch (type)
            {
                case EnumMoveType.Up:
                    tabIndex = GetIndexOfTab(objTab, siblingTabs);
                    if (tabIndex > 0)
                    {
                        DNNGo_LayerGallery_Content swapTab = siblingTabs[tabIndex - 1];
                        SwapAdjacentTabs(objTab, swapTab);
                    }
                    break;
                case EnumMoveType.Down:
                    tabIndex = GetIndexOfTab(objTab, siblingTabs);
                    if (tabIndex < siblingCount - 1)
                    {
                        DNNGo_LayerGallery_Content swapTab = siblingTabs[tabIndex + 1];
                        SwapAdjacentTabs(swapTab, objTab);
                    }
                    break;
            }

        }

        private static void SwapAdjacentTabs(DNNGo_LayerGallery_Content firstTab, DNNGo_LayerGallery_Content secondTab)
        {
            firstTab.Sort -= 2;
            firstTab.Update();
            secondTab.Sort += 2;
            secondTab.Update();
        }


        private static void UpdateTabOrder(List<DNNGo_LayerGallery_Content> tabs, int increment)
        {
            int tabOrder = 1;
            for (int index = 0; index <= tabs.Count - 1; index++)
            {
                DNNGo_LayerGallery_Content objTab = tabs[index];
                objTab.Sort = tabOrder;
                objTab.Update();
                tabOrder += increment;
            }
        }


        private static void UpdateTabOrder(List<DNNGo_LayerGallery_Content> tabs, int startIndex, int endIndex, int increment)
        {
            for (int index = startIndex; index <= endIndex; index++)
            {
                DNNGo_LayerGallery_Content objTab = tabs[index];
                objTab.Sort += increment;
                objTab.Update();
            }
        }

        private static int GetIndexOfTab(DNNGo_LayerGallery_Content objTab, List<DNNGo_LayerGallery_Content> tabs)
        {
            int tabIndex = -1;// Null.NullInteger;
            for (int index = 0; index <= tabs.Count - 1; index++)
            {
                if (tabs[index].ID == objTab.ID)
                {
                    tabIndex = index;
                    break;
                }
            }
            return tabIndex;
        }
        #endregion

		#region 业务
		#endregion
	}
}