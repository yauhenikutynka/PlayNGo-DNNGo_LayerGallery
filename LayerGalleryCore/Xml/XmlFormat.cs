﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Reflection;
using System.Globalization;
using System.Text;
using System.IO;

namespace DNNGo.Modules.LayerGallery
{
    /// <summary>
    /// XML操作类
    /// </summary>
    public class XmlFormat
    {
        #region "构造"
        /// <summary>
        /// 构造(XML路径)
        /// </summary>
        /// <param name="__XmlUrl">XmlUrl</param>
        public XmlFormat(String __XmlUrl)
        {
            _XmlUrl = __XmlUrl;
            //载入XML数据
            LoadXML();
        }
        /// <summary>
        /// 构造(XML文档)
        /// </summary>
        /// <param name="__xmlDoc"></param>
        public XmlFormat(XmlDocument __xmlDoc)
        {
            _xmlDoc = __xmlDoc;
        }

        public XmlFormat()
        { }

        #endregion

        #region "属性"

        private String _XmlUrl = String.Empty;
        /// <summary>
        /// XML路径
        /// </summary>
        public String XmlUrl
        {
            get { return _XmlUrl; }
            set { _XmlUrl = value; }
        }

        private Type _ThisType;
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type ThisType
        {
            get { return _ThisType; }
            set { _ThisType = value; }
        }



        private XmlDocument _xmlDoc = new XmlDocument();
        /// <summary>
        /// XML文档内容
        /// </summary>
        public XmlDocument XmlDoc
        {
            get { return _xmlDoc; }
            set { _xmlDoc = value; }
        }




        #endregion

        #region "方法"
        /// <summary>
        /// 载入XML数据
        /// </summary>
        private void LoadXML()
        {
            try
            {
                if(!String.IsNullOrEmpty(_XmlUrl) && System.IO.File.Exists(_XmlUrl))
                {
                     _xmlDoc.Load(_XmlUrl);//载入XML字符串
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 找到节点列表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public XmlNodeList ToNodeList(Type t)
        {
            if (_xmlDoc != null)
            {
                XmlEntityAttributes xmlAttributes = XmlEntityAttributes.GetCustomAttribute(t);

                //找出对应路径下的节点，遍历节点
                return _xmlDoc.SelectNodes(xmlAttributes.xPath);

            }
            return null;
        }



        /// <summary>
        /// 返回实体列表信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> ToList<T>()
            where T :new()
        {
 
            List<T> list = new List<T>();
            if (_xmlDoc != null)
            {
                //找出对应路径下的节点，遍历节点
                XmlNodeList Nodelist = ToNodeList(typeof(T));
                if (Nodelist != null && Nodelist.Count > 0)
                {
                    Type t = typeof(T);
                    PropertyInfo[] Propertys = t.GetProperties();

                    //遍历节点
                    foreach (XmlNode node in Nodelist)
                    {
                        T tItem = new T();
                        Boolean isTrue = false;
                        //遍历字段
                        foreach (PropertyInfo Property in Propertys)
                        {
                            String ColumnName = Property.Name;
                            if (node[ColumnName] != null && !String.IsNullOrEmpty(node[ColumnName].InnerText.Trim()))
                            {
                                object o = ConvertTo.FormatValue(node[ColumnName].InnerText.Trim(), Type.GetType(Property.PropertyType.FullName));
                                t.GetProperty(ColumnName).SetValue(tItem, o, null);

                                isTrue = true;
                            }
                        }
                        //增加到列表
                        if (isTrue)
                            list.Add(tItem);

                    }

                }
            }
         
            return list;
        }

        /// <summary>
        /// 返回单个实体信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToItem<T>() 
            where T :new()
        {
            List<T> list = ToList<T>();
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return new T();
        }


 


        /// <summary>
        /// 统计实体列表个数
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public Int32 ToCount<T>()
        {
            if (_xmlDoc != null)
            {
                //找出对应路径下的节点，遍历节点
                XmlNodeList Nodelist = ToNodeList(typeof(T));
                if (Nodelist != null)
                {
                    return Nodelist.Count;
                }
            }
            return 0;
        }



        /// <summary>
        /// 实体列表转XML
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public String ToXml<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            //读取XML实体的模版
            using (StreamReader sr = new StreamReader(XmlUrl))
            {


                String XmlTemplate = sr.ReadToEnd();
                //找出当前T的实体属性
                if (list != null && list.Count > 0)
                {
                    Type t = typeof(T);
                    PropertyInfo[] Propertys = t.GetProperties();

                    sb.AppendFormat("  <{0}List>", t.Name).AppendLine();

                    //先循环数据列表
                    foreach (T ItemInfo in list)
                    {
                        sb.AppendFormat("    <{0}Item>", t.Name).AppendLine();
                        //再循环字段列表
                        foreach (PropertyInfo Property in Propertys)
                        {
                            object o = Property.GetValue(ItemInfo, null);
                            sb.AppendFormat("      <{0}><![CDATA[{1}]]></{0}>", Property.Name, o).AppendLine();
                        }
                        sb.AppendFormat("    </{0}Item>", t.Name).AppendLine();
                    }

                    sb.AppendFormat("  </{0}List>", t.Name).AppendLine();

                }
                sr.Close();

                return string.Format(XmlTemplate, sb.ToString());
            }
        }

        /// <summary>
        /// 实体列表转XML
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public String ToXml<T>(List<T> list, List<GallerySettingsEntity> Settings)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder SettingSB = new StringBuilder();
            StringBuilder GroupSB = new StringBuilder();
            //读取XML实体的模版
            using (StreamReader sr = new StreamReader(XmlUrl))
            {
                String XmlTemplate = sr.ReadToEnd();
                //找出当前T的实体属性
                if (list != null && list.Count > 0)
                {
                    Type t = typeof(T);
                    PropertyInfo[] Propertys = t.GetProperties();

                    sb.AppendFormat("  <{0}List>", t.Name).AppendLine();

                    //先循环数据列表
                    foreach (T ItemInfo in list)
                    {
                        sb.AppendFormat("    <{0}Item>", t.Name).AppendLine();
                        //再循环字段列表
                        foreach (PropertyInfo Property in Propertys)
                        {
                            object o = Property.GetValue(ItemInfo, null);
                            sb.AppendFormat("      <{0}><![CDATA[{1}]]></{0}>", Property.Name, o).AppendLine();
                        }
                        sb.AppendFormat("    </{0}Item>", t.Name).AppendLine();
                    }
                    sb.AppendFormat("  </{0}List>", t.Name).AppendLine();

                }
                if (Settings != null && Settings.Count > 0)
                {
                    Type t = typeof(GallerySettingsEntity);
                    PropertyInfo[] Propertys = t.GetProperties();

                    SettingSB.AppendFormat("  <{0}List>", t.Name).AppendLine();

          
                    //先循环数据列表
                    foreach (GallerySettingsEntity ItemInfo in Settings)
                    {
                        SettingSB.AppendFormat("    <{0}Item>", t.Name).AppendLine();
                        //再循环字段列表
                        foreach (PropertyInfo Property in Propertys)
                        {
                            object o = Property.GetValue(ItemInfo, null);
                            SettingSB.AppendFormat("      <{0}><![CDATA[{1}]]></{0}>", Property.Name, o).AppendLine();
                        }
                        SettingSB.AppendFormat("    </{0}Item>", t.Name).AppendLine();
                    }
                    SettingSB.AppendFormat("  </{0}List>", t.Name).AppendLine();
                }


 


                sr.Close();

                return string.Format(XmlTemplate, sb.ToString(), SettingSB.ToString(), GroupSB.ToString());
            }
        }

        #endregion


        #region "数据转换"

 



        #endregion





    }
}