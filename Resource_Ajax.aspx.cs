using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Globalization;

namespace DNNGo.Modules.LayerGallery
{
    public partial class Resource_Ajax1 : BasePage
    {





        #region "属性"

        /// <summary>
        /// 请求方法
        /// </summary>
        public String Method = WebHelper.GetStringParam(HttpContext.Current.Request, "Method", "get");
        /// <summary>
        /// 数据类型
        /// </summary>
        public String JsonType = WebHelper.GetStringParam(HttpContext.Current.Request, "Type", "Settings");

        /// <summary>
        /// 请求的JSON字符串
        /// </summary>
        public String JsonContent = WebHelper.GetStringParam(HttpContext.Current.Request, "Content", "");

        /// <summary>
        /// 请求的JSON字符串
        /// </summary>
        public String JsonSettings = WebHelper.GetStringParam(HttpContext.Current.Request, "Settings", "");

        /// <summary>
        /// 删除编号
        /// </summary>
        public Int32 DeleteID = WebHelper.GetIntParam(HttpContext.Current.Request, "DeleteID", 0);
        

        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        #endregion



        #region "方法"

        /// <summary>
        /// 接收参数
        /// </summary>
        public void PostJSON()
        {

            //获取设置并保存
            Dictionary<String, object> EffectSettings = jsSerializer.DeserializeObject(JsonSettings) as Dictionary<String, object>;
            SetSettings(EffectSettings);

            //获取项的集合并保存
            Dictionary<String, object> Sliders = jsSerializer.DeserializeObject(JsonContent) as Dictionary<String, object>;
            //循环遍历Sliders
            int Slider_Index = 0;
            foreach (var Slider in Sliders)
            {
                //解析出单个Slider中的定义
                Dictionary<String, object> Slider_Items = Slider.Value as Dictionary<String, object>;
                //解析出Layers
                Dictionary<String, object> Layers = new Dictionary<string, object>();
                if (Slider_Items.ContainsKey("layers"))
                {
                    Layers = Slider_Items["layers"] as Dictionary<String, object>;
                }

                //更新Slider
                Int32 SliderID =  UpdateSliderItem(Slider_Items, Slider_Index);

                if (Layers != null && Layers.Count > 0)
                {
                    //更新Layers
                    int Layer_Index = 0;
                    foreach (var Layer in Layers)
                    {
                        //解析出单个Layer中的定义
                        Dictionary<String, object> Layer_Items = Layer.Value as Dictionary<String, object>;
                        //更新Layer
                        Int32 LayerID = UpdateLayerItem(Layer_Items, SliderID, Layer_Index);
                        Layer_Index++;
                    }
                }

                Slider_Index++;
            }



        
        }



        public void GetJSON()
        {

            if (JsonType.IndexOf("Settings", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                List<KeyValueEntity> DefaultSettings = GetDefaultSettings();
                Dictionary<String, object> DictsSettings = new Dictionary<string, object>();


                foreach (KeyValueEntity kvEntity in DefaultSettings)
                {
                    String SettingName = String.Format("Global_{0}", kvEntity.Key);
                    if (Settings.ContainsKey(SettingName))
                    {
                        DictsSettings.Add(kvEntity.Key, Settings[SettingName]);
                    }
                    else
                    {
                        DictsSettings.Add(kvEntity.Key, kvEntity.Value);
                    }
                }

                Response.Write(jsSerializer.Serialize(DictsSettings));



            }
            else
            {
                List<DNNGo_LayerGallery_Content> Sliders = DNNGo_LayerGallery_Content.FindAll(this);

                Dictionary<String, object> DictsSliders = new Dictionary<string, object>();


                for (int i = 0; i < Sliders.Count; i++)
                {
                    DNNGo_LayerGallery_Content Slider = Sliders[i];

                    Dictionary<String, object> Slider_Options = jsSerializer.DeserializeObject(Slider.Options) as Dictionary<String, object>;

                    SetDicts(ref Slider_Options, "id", Slider.ID);
                    SetDicts(ref Slider_Options, "sort", i + 1);
                    SetDicts(ref Slider_Options, "StartTime", Slider.StartTime.ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)));
                    SetDicts(ref Slider_Options, "EndTime", Slider.EndTime.ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)));
                    //SetDicts(ref Slider_Options, "StartTime", Slider.StartTime);
                    //SetDicts(ref Slider_Options, "EndTime", Slider.EndTime);

                    //增加layers的定义
                    Dictionary<String, object> Layer_Options = GetLayerItems(Slider);
                    if (Layer_Options != null && Layer_Options.Count > 0)
                    {
                        Slider_Options.Add("layers", Layer_Options);
                    }

                    DictsSliders.Add(String.Format("slide_{0}", Slider.ID), Slider_Options);
                }

                Response.Write(jsSerializer.Serialize(DictsSliders));

                //Response.Write(System.IO.File.ReadAllText(Server.MapPath(String.Format("{0}Resource/templates/data.html", ModulePath))));
            }




        }

        /// <summary>
        /// 获取参数
        /// </summary>
        public String Add_Slider_JSON()
        {

            Dictionary<String, object> Slider_Items = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(JsonContent))
            {
                try
                {
                    Slider_Items = jsSerializer.DeserializeObject(JsonContent) as Dictionary<String, object>;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                
                

                if (Slider_Items != null && Slider_Items.Count > 0)
                {
                    return UpdateSliderItem(Slider_Items, 0).ToString();
                }
                else
                {
                    return "-1";
                }
            }
            return "0";
        }


        public Int32 Add_Layer_JSON()
        {
            Dictionary<String, object> Layer_Items = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(JsonContent))
            {
                Layer_Items = jsSerializer.DeserializeObject(JsonContent) as Dictionary<String, object>;

                if (Layer_Items != null && Layer_Items.Count > 0)
                {
                    return UpdateLayerItem(Layer_Items, 0, 0);
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <summary>
        /// 设置字典的值
        /// </summary>
        /// <param name="Dicts"></param>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public void SetDicts(ref Dictionary<String, object> Dicts,  String Name, object Value)
        {
            if (Dicts!= null && Dicts.Count >0   )
            {
                if (Dicts.ContainsKey(Name))
                {
                    Dicts[Name] = Value;
                }
                else
                {
                    Dicts.Add(Name, Value);
                }
            }
        }





        #region "--Slider--"

        /// <summary>
        /// 更新单个Slider设置
        /// </summary>
        /// <param name="Slider_Items"></param>
        /// <returns></returns>
        public Int32 UpdateSliderItem(Dictionary<String, object> Slider_Items, int Slider_Index)
        {

            List<KeyValueEntity> tempSlider = ConvertTo.ToList(Slider_Items);


            DNNGo_LayerGallery_Content Slider = new DNNGo_LayerGallery_Content();

            //取出编号
            Slider.ID = ConvertTo.GetValue<Int32>(tempSlider, "id", 0);

            if (Slider.ID > 0)
            {
                Slider = DNNGo_LayerGallery_Content.FindByKeyForEdit(Slider.ID);
            }

            //移除层的定义，将Slider项的定义序列化后存储
            Slider_Items.Remove("layers");
            Slider.Options = jsSerializer.Serialize(Slider_Items);

            //取出排序编号
            Slider.Sort = ConvertTo.GetValue<Int32>(tempSlider, "sort", Slider_Index+1);

            //取出标题
            Slider.Title = ConvertTo.GetValue<String>(tempSlider, "title","");

            //开始时间
            DateTime oTime = xUserTime.LocalTime();
            string[] expectedFormats = { "G", "g", "f", "F" };
            string StartTime = ConvertTo.GetValue<String>(tempSlider, "StartTime", oTime.ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)));
            if (DateTime.TryParseExact(StartTime, "MM/dd/yyyy HH:mm", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out oTime))
            {
                if (oTime.Second == 0) oTime = oTime.AddSeconds(xUserTime.LocalTime().Second);//补秒
                Slider.StartTime = oTime;
            }


            //结束时间
            DateTime EndTime = xUserTime.LocalTime().AddYears(10);
            string sEndTime = ConvertTo.GetValue<String>(tempSlider, "EndTime", EndTime.ToString("MM/dd/yyyy HH:mm", new CultureInfo("en-US", false)));
            if (DateTime.TryParseExact(sEndTime, "MM/dd/yyyy HH:mm", new CultureInfo("en-US", false), DateTimeStyles.AllowWhiteSpaces, out EndTime))
            {
                if (EndTime.Second == 0) EndTime = EndTime.AddSeconds(xUserTime.LocalTime().Second);//补秒
                Slider.EndTime = EndTime;
            }

             
            Slider.LastTime = xUserTime.UtcTime();
            Slider.LastIP = WebHelper.UserHost;
            Slider.LastUser = UserId;

            if (Slider.ID > 0)
            {
                //更新
                Slider.Update();
            }
            else
            {
                //新增
                Slider.ModuleId = ModuleId;
                Slider.PortalId = PortalId;
                Slider.Status = (Int32)EnumStatus.Published;
                

                Slider.ID = Slider.Insert();


            }



            return Slider.ID;
                 
        }


        /// <summary>
        /// 删除Slider
        /// </summary>
        /// <param name="SliderID"></param>
        /// <returns></returns>
        public Int32 DeleteSliderItem(Int32 SliderID)
        {
            Int32 Result = 0;
            if (SliderID > 0)
            {
                DNNGo_LayerGallery_Content SliderItem = DNNGo_LayerGallery_Content.FindByKeyForEdit(SliderID);

                if (SliderItem != null && SliderItem.ID > 0)
                {
                    Result = SliderItem.Delete();
                    if (Result > 0)
                    {
                        DNNGo_LayerGallery_Item.Delete(new String[] { DNNGo_LayerGallery_Item._.ContentID },new object[]{ SliderID});
                    }
                }
            }


            return Result;
        }

        #endregion

        #region "--Layers--"

        public Int32 UpdateLayerItem(Dictionary<String, object> Layer_Items, Int32 SliderID, int Layer_Index)
        {
            List<KeyValueEntity> tempLayer = ConvertTo.ToList(Layer_Items);


            DNNGo_LayerGallery_Item Layer = new DNNGo_LayerGallery_Item();


            //取出编号
            Layer.ID = ConvertTo.GetValue<Int32>(tempLayer, "id", 0);
            //读取数据库中的数据
            if (Layer.ID > 0)
            {
                Layer = DNNGo_LayerGallery_Item.FindByKeyForEdit(Layer.ID);
            }

            //将Layer项的定义序列化后存储
            Layer.Options = jsSerializer.Serialize(Layer_Items);

            //取出排序编号
            Layer.Sort = ConvertTo.GetValue<Int32>(tempLayer, "sort", Layer_Index + 1);

            //取出标题
            Layer.Title = ConvertTo.GetValue<String>(tempLayer, "title", "");

            Layer.LastTime = DateTime.Now;
            Layer.LastIP = WebHelper.UserHost;
            Layer.LastUser = UserId;


            if (Layer.ID > 0)
            {
                //更新
                 Layer.Update();
            }
            else
            {
                //新增
                Layer.ModuleId = ModuleId;
                Layer.PortalId = PortalId;

                Layer.ContentID = ConvertTo.GetValue<Int32>(tempLayer, "SliderID", SliderID);
                Layer.Status = (Int32)EnumStatus.Published;

                Layer.ID = Layer.Insert();


            }

            return Layer.ID;
        }

        /// <summary>
        /// 获取Layers
        /// </summary>
        /// <param name="Slider"></param>
        /// <returns></returns>
        public Dictionary<String, object> GetLayerItems(DNNGo_LayerGallery_Content Slider)
        {
            //查询出所有的Layers
            List<DNNGo_LayerGallery_Item> Layers = DNNGo_LayerGallery_Item.FindAll(Slider.ID);

            Dictionary<string, object> LayerItems = new Dictionary<string, object>();


            for(int i =0;i< Layers.Count;i++)
            {

                DNNGo_LayerGallery_Item Layer = Layers[i];

                Dictionary<String, object> LayerItems_Options = jsSerializer.DeserializeObject(Layer.Options) as Dictionary<String, object>;

                if (LayerItems_Options != null && LayerItems_Options.Count > 0)
                {

                    SetDicts(ref LayerItems_Options, "id", Layer.ID);
                    SetDicts(ref LayerItems_Options, "sort", i + 1);

                    LayerItems.Add(String.Format("layer_{0}_{1}", Slider.ID, Layer.ID), LayerItems_Options);
                }
            }
            return LayerItems;
        }

        /// <summary>
        /// 删除Layer
        /// </summary>
        /// <param name="LayerID"></param>
        /// <returns></returns>
        public Int32 DeleteLayerItem(Int32 LayerID)
        {
            Int32 Result  = 0;
            if (LayerID > 0)
            {
                DNNGo_LayerGallery_Item LayerItem = DNNGo_LayerGallery_Item.FindByKeyForEdit(LayerID);

                if (LayerItem != null && LayerItem.ID > 0)
                {
                    Result = LayerItem.Delete();
                }
            }


            return Result;
        }


        #endregion

        #region "--效果通用设置--"

        /// <summary>
        /// 设置接收的全局设置
        /// </summary>
        /// <param name="PostSettings"></param>
        public void SetSettings(Dictionary<String, object> PostSettings)
        {

            // List<KeyValueEntity> DefaultSettings = GetDefaultSettings();
            List<KeyValueEntity> ThisSettings = ConvertTo.ToList(PostSettings);

            foreach (KeyValueEntity k in ThisSettings)
            {
                UpdateModuleSetting(String.Format("Global_{0}", k.Key), k.Value.ToString());
            }
        }

        /// <summary>
        /// 获取默认的设置
        /// </summary>
        /// <returns></returns>
        public List<KeyValueEntity> GetDefaultSettings()
        {
            Dictionary<String, object> dicts = (Dictionary<String, object>)jsSerializer.DeserializeObject(System.IO.File.ReadAllText(Server.MapPath(String.Format("{0}Resource/xml/GlobalSetting.json", ModulePath))));
            return ConvertTo.ToList(dicts);
        }


        #endregion


        #endregion




        #region "事件"

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Method.IndexOf("post", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        //接收参数
                        PostJSON();
                    }else if (Method.IndexOf("add_slider", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        //添加背景并返回json
                        Response.Write(Add_Slider_JSON());
                    }
                    else if (Method.IndexOf("add_layer", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        //添加层并返回json
                        Response.Write(Add_Layer_JSON());
                    }
                    else if (Method.IndexOf("delete_slider", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        //删除滑动项
                        Response.Write(DeleteSliderItem(DeleteID));
                    }
                    else if (Method.IndexOf("delete_layer", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        //删除层
                        Response.Write(DeleteLayerItem(DeleteID));
                    }
                    else
                    {
                        //发送参数
                        GetJSON();
                    }
                }
            }
            catch
            {
 
            }
        }

        #endregion
    }
}