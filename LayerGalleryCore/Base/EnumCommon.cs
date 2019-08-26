using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.LayerGallery
{


    /// <summary>
    /// 提示类型
    /// </summary>
    public enum EnumTips
    {
        /// <summary>
        /// Success
        /// </summary>
        [Text("Success")]
        Success = 0,
        /// <summary>
        /// Warning
        /// </summary>
        [Text("Warning")]
        Warning = 1,
        /// <summary>
        /// Error
        /// </summary>
        [Text("Error")]
        Error = 2,
        /// <summary>
        /// Alert
        /// </summary>
        [Text("Alert")]
        Alert = 3
    }

    /// <summary>
    /// 显示类型(正常/模块)
    /// </summary>
    public enum EnumViewType
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Text("Normal")]
        Normal = 0,
        /// <summary>
        /// 模块
        /// </summary>
        [Text("Module")]
        Module = 1
    }

    /// <summary>
    /// 状态
    /// </summary>
    public enum EnumStatus
    {
        /// <summary>
        /// 通过激活
        /// </summary>
        [Text("Published")]
        Published = 1,
        /// <summary>
        /// 未审核
        /// </summary>
        [Text("Pending")]
        Pending = 0,
        /// <summary>
        /// 删除
        /// </summary>
        [Text("recycle bin")]
        RecycleBin = 2
    }


    /// <summary>
    /// 移动类型
    /// </summary>
    public enum EnumMoveType
    {
        /// <summary>
        /// Up
        /// </summary>
        [Text("Up")]
        Up,
        /// <summary>
        /// Down
        /// </summary>
        [Text("Down")]
        Down,
        /// <summary>
        /// Top
        /// </summary>
        [Text("Top")]
        Top,
        /// <summary>
        /// Bottom
        /// </summary>
        [Text("Bottom")]
        Bottom,
        /// <summary>
        /// Promote
        /// </summary>
        [Text("Promote")]
        Promote,
        /// <summary>
        /// Demote
        /// </summary>
        [Text("Demote")]
        Demote
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumLinkTarget
    {
        /// <summary>
        /// _self
        /// </summary>
        [Text("_self")]
        self = 0,
        /// <summary>
        /// _blank
        /// </summary>
        [Text("_blank")]
        blank = 1
    }

    /// <summary>
    /// 链接文件的属性
    /// </summary>
    public enum EnumLinkMeta
    {
        /// <summary>
        /// Link
        /// </summary>
        [Text("Link")]
        Link = 0,

        /// <summary>
        /// Picture
        /// </summary>
        [Text("Picture")]
        Picture = 1,
        /// <summary>
        /// iFrame
        /// </summary>
        [Text("iFrame")]
        iFrame = 2,
        /// <summary>
        /// Video
        /// </summary>
        [Text("Video")]
        Video = 3,
        /// <summary>
        /// Flash
        /// </summary>
        [Text("Flash")]
        Flash = 4,
        /// <summary>
        /// YouTube
        /// </summary>
        [Text("YouTube")]
        YouTube = 5,
        /// <summary>
        /// Vimeo
        /// </summary>
        [Text("Vimeo")]
        Vimeo = 6,
        /// <summary>
        /// QuickTime
        /// </summary>
        [Text("QuickTime")]
        QuickTime = 7
    }



        /// <summary>
    /// 链接文件的属性
    /// </summary>
    public enum EnumLinkMeta2
    {
        /// <summary>
        /// Link
        /// </summary>
        [Text("Link")]
        Link = 0,

        /// <summary>
        /// Lightbox
        /// </summary>
        [Text("Lightbox")]
        Lightbox = 1,
        /// <summary>
        /// iFrame
        /// </summary>
        [Text("iFrame")]
        iFrame = 2
    }


    /// <summary>
    /// 控件类型
    /// </summary>
    public enum EnumControlType
    {
        /// <summary>
        /// TextBox
        /// </summary>
        [Text("TextBox")]
        TextBox = 1,
        /// <summary>
        /// RichTextBox
        /// </summary>
        [Text("RichTextBox")]
        RichTextBox = 2,
        /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList")]
        DropDownList = 3,
          /// <summary>
        /// DropDownList
        /// </summary>
        [Text("DropDownList Group")]
        DropDownList_Group = 30,
        /// <summary>
        /// ListBox
        /// </summary>
        [Text("ListBox")]
        ListBox = 4,
        /// <summary>
        /// RadioButtonList
        /// </summary>
        [Text("RadioButtonList")]
        RadioButtonList = 5,
        /// <summary>
        /// FileUpload
        /// </summary>
        [Text("FileUpload")]
        FileUpload = 6,
        /// <summary>
        /// CheckBox
        /// </summary>
        [Text("CheckBox")]
        CheckBox = 7,
        /// <summary>
        /// CheckBoxList
        /// </summary>
        [Text("CheckBoxList")]
        CheckBoxList = 8,
        /// <summary>
        /// DatePicker
        /// </summary>
        [Text("DatePicker")]
        DatePicker = 9,
        /// <summary>
        /// Label
        /// </summary>
        [Text("Label")]
        Label = 10


    }

    /// <summary>
    /// 列表控件方向
    /// </summary>
    public enum EnumControlDirection
    {

        /// <summary>
        /// 横向
        /// </summary>
        [Text("Horizontal")]
        Horizontal = 1,

        /// <summary>
        /// 垂直
        /// </summary>
        [Text("Vertical")]
        Vertical = 0
    }



    /// <summary>
    /// 验证类型
    /// </summary>
    public enum EnumVerification
    {
        /// <summary>
        /// 表示可选项。若不输入，不要求必填，若有输入，则验证其是否符合要求。
        /// </summary>
        [Text("optional")]
        optional = 0,
        /// <summary>
        /// 验证整数
        /// </summary>
        [Text("integer")]
        integer = 1,
        /// <summary>
        /// 验证数字
        /// </summary>
        [Text("number")]
        number = 2,
        /// <summary>
        /// 验证日期，格式为 YYYY/MM/DD、YYYY/M/D、YYYY-MM-DD、YYYY-M-D
        /// </summary>
        [Text("dateFormat")]
        date = 3,
        /// <summary>
        /// 验证 Email 地址
        /// </summary>
        [Text("email")]
        email = 4,
        /// <summary>
        /// 验证电话号码
        /// </summary>
        [Text("phone")]
        phone = 5,
        /// <summary>
        /// 验证 ipv4 地址
        /// </summary>
        [Text("ipv4")]
        ipv4 = 6,
        /// <summary>
        /// 验证 url 地址，需以 http://、https:// 或 ftp:// 开头
        /// </summary>
        [Text("url")]
        url = 7,
        /// <summary>
        /// 只接受填数字和空格
        /// </summary>
        [Text("onlyNumberSp")]
        onlyNumberSp = 8,
        /// <summary>
        /// 只接受填英文字母（大小写）和单引号(')
        /// </summary>
        [Text("onlyLetterSp")]
        onlyLetterSp = 9,
        /// <summary>
        /// 只接受数字和英文字母
        /// </summary>
        [Text("onlyLetterNumber")]
        onlyLetterNumber = 10



    }


    /// <summary>
    /// 排序字段
    /// </summary>
    public enum EnumSortby
    {
        /// <summary>
        ///  默认
        /// </summary>
        [Text("Default")]
        Default = 0,
        /// <summary>
        ///  随机
        /// </summary>
        [Text("Random")]
        Random = 1,
        /// <summary>
        ///  名称(正序)
        /// </summary>
        [Text("Title:A to Z")]
        Name = 2,
        /// <summary>
        ///  名称(倒序)
        /// </summary>
        [Text("Title:Z to A")]
        Name_Desc = 4,
        /// <summary>
        ///  时间(正序)
        /// </summary>
        [Text("Start Time ASC")]
        Time = 3 ,
  
        /// <summary>
        ///  时间(倒序)
        /// </summary>
        [Text("Start Time DESC")]
        Time_Desc = 5


    }





    /// <summary>
    /// 文件类型
    /// </summary>
    public enum EnumFileMate
    {
        /// <summary>
        /// Image
        /// </summary>
        [Text("Image")]
        Image = 0,
        /// <summary>
        /// Video
        /// </summary>
        [Text("Video")]
        Video = 1,
        /// <summary>
        /// Audio
        /// </summary>
        [Text("Audio")]
        Audio = 2,
        /// <summary>
        /// Zip
        /// </summary>
        [Text("Zip")]
        Zip = 3,
        /// <summary>
        /// Doc
        /// </summary>
        [Text("Doc")]
        Doc = 4,
        /// <summary>
        /// Other
        /// </summary>
        [Text("Other")]
        Other = 9,



    }

    /// <summary>
    /// 多媒体文件状态(未审核、正常、删除/回收站)
    /// </summary>
    public enum EnumFileStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Text("Pending")]
        Pending = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Text("Approved")]
        Approved = 1,
        /// <summary>
        /// 回收站
        /// </summary>
        [Text("Recycle")]
        Recycle = 2
    }


    /// <summary>
    /// 链接控件枚举
    /// </summary>
    public enum EnumUrlControls
    {
        /// <summary>
        /// URL
        /// </summary>
        [Text("URL ( A Link To An External Resource )")]
        Url = 1,
        /// <summary>
        /// 页面
        /// </summary>
        [Text("Page ( A Page On Your Site )")]
        Page = 2,
        /// <summary>
        /// 页面
        /// </summary>
        [Text("Picture ( From the media library )")]
        Picture = 3

    }

    /// <summary>
    /// 编辑器类型枚举
    /// </summary>
    public enum EnumEditor
    {
        /// <summary>
        /// Tinymce
        /// </summary>
        [Text("Tinymce Editor")]
        Tinymce = 0,
        /// <summary>
        /// Tinymce
        /// </summary>
        [Text("Telerik ( DNN Default )")]
        Telerik = 1

    }



}