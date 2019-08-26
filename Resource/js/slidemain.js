var imgurl=function(){};
$(document).ready(function() {
	
	Date.prototype.Format = function (fmt) { 
		var o = {
			"M+": this.getMonth() + 1,
			"d+": this.getDate(), 
			"h+": this.getHours(),
			"m+": this.getMinutes(), 
			"s+": this.getSeconds(), 
			"q+": Math.floor((this.getMonth() + 3) / 3), 
			"S": this.getMilliseconds() 
		};
		if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
		for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
		return fmt;
	}

	var SlidersHtml,
		layersHtml,
		receive_data,
		Sliders_length=0,
		current=0,
		slideTab = $("#slide-tab-title"),
		slidecontent = $("#slide-tab-content");

	$.ajax({
	url :$("#ajaxParameters").attr("TemplateUrl"), 
	async:false,
	success: function(data){
		 SlidersHtml=data;	
	}
	})
	$.ajax({
		url :$("#ajaxParameters").attr("LayersUrl"), 
		async:false,
		success: function(data){
			layersHtml=data;
					
		}
	})
	var SlideEvent=function(id){
		slideTab.children("#slide-title" + id).find(".ico-close").on("click",function() {delSlide($(this).attr("id"))});
	}
	$.ajax({
		url :$("#ajaxParameters").attr("GetDataUrl_Sliders")+"?d="+(new Date()).Format("hhmmss"), 
		async:false,
		success: function(data){
			receive_data=JSON.parse(data);
		}
	})
	for (var Slider in receive_data) {
		Sliders_length++;
		var sle = eval("(" + "receive_data." + Slider + ")");
		var id= sle.id; 
		var ls = sle.layers;
		slideTab.append("<li id=\"slide-title" + id + "\" class=\"slide-tab-nav\"  data-id=\"" + id + "\" data-sort=\"" + parseInt(slideTab.find('li').length + 1) + "\"> <a  data-toggle=\"tab\" href=\"#slide_tab1_example" + id + "\" >Slide<span>" + parseInt(slideTab.find('li').length + 1) + "</span><i class=\"clip-cancel-circle-2 ico-close\" id=\"" + id + "\" ></i><i class=\"drag clip-arrow-3\"></i></a></li>");
		SlideEvent(id);
	}
	var acquisition_data=function(id){
		var slidedata = "";
		slideoption(id);
		imageoption(id);
		mediaoption(id);
		typeoption(id);
		var layersdata = "{";
		for (x = 0; x < $("#layers-accordion" + id).children(".panel").length; x++) {
			var layerid = $("#layers-accordion" + id).children(".panel").eq(x).attr("data-new");
			contentoption(id, layerid);
			transitionoption(id, layerid);
			linkoption(id, layerid);
			styleoption(id, layerid);
			attributesoption(id, layerid);
			datasettings(id, layerid);
			othersettings(id, layerid);
			var layerids = $("#panel" + id + "_" + layerid).attr("data-id"),
				layersorts = $("#panel" + id + "_" + layerid).attr("data-sort"),
				layercid = parseInt($("#panel" + id + "_" + layerid).attr("data-new"));
			   layersdata += "\"layers_" + id + "_" + layerid + "\":{\"id\":\"" + layerids + "\",\"sort\":\"" + layersorts + "\",\"currentid\":\"" + layercid + "\", \"contentoption\":" + JSON.stringify(cop) + ",\"transitionoption\":" + JSON.stringify(tnop) + ",\"linkoption\":" + JSON.stringify(linkop) + ",\"styleoption\":" + JSON.stringify(styleop) + ",\"attributesoption\":" + JSON.stringify(aop) + ",\"datasettings\":" + JSON.stringify(dsgop) + ",\"Othersettings\":" + JSON.stringify(othop) + "},";
		}
		if ($("#layers-accordion" + id).children(".panel").length > 0) {
			layersdata = layersdata.substring(0, layersdata.length - 1) + "}";
			layersinfo = ",\"layers\":" + layersdata;
		} else {
			layersinfo = "";
		}
		slidedata += "{" + JSON.stringify(sop).replace(/{/, "").replace(/}+$/g, '') + JSON.stringify(iop).replace(/{/, ",").replace(/}+$/g, '') + JSON.stringify(mop).replace(/{/, ",").replace(/}+$/g, '') + JSON.stringify(typeop).replace(/{/, ",").replace(/}+$/g, '') + layersinfo + "}";
		return slidedata;
	}			
			
	var loadslider=function(id,op){
		$("#Loading2").show();
		slideTab.css("pointer-events","none")
		$(this).addClass("active");
			var sle = eval("(" + "receive_data.slide_" +id + ")");
			addSlide(id, sle);
			var ls = sle.layers ;
			
			$.ajax({success:function(){
				for (var pros in ls) {
				  var slle = eval("(" + "ls." + pros + ")");
				  addLayer(id,slle.currentid?slle.currentid:slle.id,slle);
				}
				slideTab.css("pointer-events","inherit")
				$("#Loading2").hide();
				if($("#slide_tab1_example"+id).length!=0){refreshscript(id);}
				
			}})
			
			if(current!=0 && current!=id){
				receive_data["slide_" + current]=JSON.parse(acquisition_data(current));
				$("#slide_tab1_example"+current).remove();
			}
			$("#slide_tab1_example"+id).addClass("active");
			current=id;
	}
			
	slideTab.find("li").on("click",function(){
		if($(this).hasClass("active")) return false;
		loadslider($(this).find(".ico-close").attr("id"));
	})
	
	$("#addnewslide").on("click",function() {
		var id = 1;
		for (i = 0; i < slideTab.children("li").length; i++) {
			id = Math.max(slideTab.find("li").eq(i).find(".ico-close").attr("id"), id);
		}
		id++;
		slideTab.append("<li id=\"slide-title" + id + "\" class=\"slide-tab-nav\"  data-id=\"" + 0 + "\" data-sort=\"" + parseInt(slideTab.find('li').length + 1) + "\"> <a  data-toggle=\"tab\" href=\"#slide_tab1_example" + id + "\" >Slide<span>" + parseInt(slideTab.find('li').length + 1) + "</span><i class=\"clip-cancel-circle-2 ico-close\" id=\"" + id + "\" ></i><i class=\"drag clip-arrow-3\"></i></a></li>");
		SlideEvent(id);
		var newslide= $("#slide-title" + id);
		newslide.addClass("active").siblings().removeClass("active");
		addSlide(id);
		$("#slide_tab1_example"+id).addClass("active");
		receive_data["slide_" + id]=JSON.parse(acquisition_data(id));
		$("#slide_tab1_example"+current).remove();
		refreshscript(id);
		current=id;
		newslide.on("click",function(){
			if($(this).hasClass("active")) return false;
			loadslider($(this).find(".ico-close").attr("id"));
		})
		$("#Loading2").hide();
	});
	var delSlide = function(id) {
		if (confirm("Sure to delete ?")) {
			var it = "#slide-title" + id,
				ic = "#slide_tab1_example" + id,
				index =slideTab.children(it).index();
				slideTab.children(it).remove();
				slidecontent.children(ic).remove();
				if(slideTab.children(it).last().length==0){
					index=index-1;
				}
				current=slideTab.find("li").eq(index).find(".ico-close").attr("id");
				slideTab.find("li").eq(index).click();
				delete receive_data["slide_" + id];
				slideName();
				contentnot();
			if ($(it).attr("data-id") != "0")  {$.get($("#ajaxParameters").attr("DeleteDataUrl_Slider")+"?d="+(new Date()).Format("hhmmss"), {DeleteID: id });}
		}
	}
	$("#submit").on("click",function() {
		$("#Loading").show();
		var nindex=slideTab.find("li.active").index();
		receive_data["slide_" + current]=JSON.parse(acquisition_data(current));
		for (var Slider in receive_data) {
			receive_data[Slider]["sort"]= $("#slide-title"+Slider.split("_")[1]).data("sort");
		}
		settings();

		$.post($("#ajaxParameters").attr("PostDataUrl")+"?d="+(new Date()).Format("hhmmss"), {Content: JSON.stringify(receive_data),Settings: JSON.stringify(stg)},function() {
			$.get($("#ajaxParameters").attr("GetDataUrl_Sliders")+"?d="+(new Date()).Format("hhmmss"),function(data) {
				receive_data=JSON.parse(data);
				slideTab.find("li").remove();
		
				for (var Slider in receive_data) {
					Sliders_length++;
					var sle = eval("(" + "receive_data." + Slider + ")");
					var id= sle.id; 
					var ls = sle.layers;
					slideTab.append("<li id=\"slide-title" + id + "\" class=\"slide-tab-nav\"  data-id=\"" + id + "\" data-sort=\"" + parseInt(slideTab.find('li').length + 1) + "\"> <a  data-toggle=\"tab\" href=\"#slide_tab1_example" + id + "\" >Slide<span>" + parseInt(slideTab.find('li').length + 1) + "</span><i class=\"clip-cancel-circle-2 ico-close\" id=\"" + id + "\" ></i><i class=\"drag clip-arrow-3\"></i></a></li>");
					SlideEvent(id);
				}
				slideTab.find("li").on("click",function(){
					if($(this).hasClass("active")) return false;
					loadslider($(this).find(".ico-close").attr("id"));
				})
				
				$("#slide_tab1_example"+current).remove();
				current=slideTab.find("li").eq(nindex).find(".ico-close").attr("id");
				slideTab.find("li").eq(nindex).click();				
				$("#Loading").hide();
				
			});
			
			$("#alertbox").html("<div class=\"alert alert-success\"><button data-dismiss=\"alert\" class=\"close\"> × </button><i class=\"fa fa-check-circle\"></i> Update successful </div>");
			$("html,body").animate({
				scrollTop: 0
			},
			200)
		});
		return false;
	})
	$.get($("#ajaxParameters").attr("GetDataUrl_Settings")+"?d="+(new Date()).Format("hhmmss"),function(data) {
		recomposesettings(eval("(" + data + ")"));
		$("#SliderSettings").find("input[type=checkbox]").each(function() {
			if ($(this).val() == "off") {
				$(this).removeAttr("checked")
			} else {
				$(this).attr("checked", "checked")
			}
			checkboxval($(this));
		});
		$("#SliderSettings").find(".make-switch")['bootstrapSwitch']();
		Previewbox();
		var dynamic=function() {
		  $("#Loading").hide();
		  clearTimeout(intervals);
		}
	  var intervals = setInterval(dynamic,2000); 
	});
		
	var contentnot = function() {
		if (slideTab.find("li").length == 0) {
			$("#slide-tab-content").append("<div id=\"contentnot\">Please add a Slide ...</div>")
		} else {
			$("#contentnot").remove()
		}
	}
	var recomposesettings = function(se) {
		$("#delay").val(se.delay);
		$("#startheight").val(se.startheight);
		$("#startwidth").val(se.startwidth);
		$("#fullScreenAlignForce").val(se.fullScreenAlignForce);
		$("#autoHeight").val(se.autoHeight);
		$("#hideTimerBar").val(se.hideTimerBar);
		$("#hideThumbs").val(se.hideThumbs);
		$("#hideNavDelayOnMobile").val(se.hideNavDelayOnMobile);
		$("#thumbWidth").val(se.thumbWidth);
		$("#thumbHeight").val(se.thumbHeight);
		$("#thumbAmount").val(se.thumbAmount);
		$("#navigationType").val(se.navigationType);
		$("#navigationArrows").val(se.navigationArrows);
		$("#navigationInGrid").val(se.navigationInGrid);
		$("#hideThumbsOnMobile").val(se.hideThumbsOnMobile);
		$("#hideBulletsOnMobile").val(se.hideBulletsOnMobile);
		$("#hideArrowsOnMobile").val(se.hideArrowsOnMobile);
		$("#hideThumbsUnderResoluition").val(se.hideThumbsUnderResoluition);
		$("#navigationStyle").val(se.navigationStyle);
		$("#navigationHAlign").val(se.navigationHAlign);
		$("#navigationVAlign").val(se.navigationVAlign);
		$("#navigationHOffset").val(se.navigationHOffset);
		$("#navigationVOffset").val(se.navigationVOffset);
		$("#soloArrowLeftHalign").val(se.soloArrowLeftHalign);
		$("#soloArrowLeftValign").val(se.soloArrowLeftValign);
		$("#soloArrowLeftHOffset").val(se.soloArrowLeftHOffset);
		$("#soloArrowLeftVOffset").val(se.soloArrowLeftVOffset);
		$("#soloArrowRightHalign").val(se.soloArrowRightHalign);
		$("#soloArrowRightValign").val(se.soloArrowRightValign);
		$("#soloArrowRightHOffset").val(se.soloArrowRightHOffset);
		$("#soloArrowRightVOffset").val(se.soloArrowRightVOffset);
		$("#keyboardNavigation").val(se.keyboardNavigation);
		$("#touchenabled").val(se.touchenabled);
		$("#onHoverStop").val(se.onHoverStop);
		$("#startWithSlide").val(se.startWithSlide);
		$("#stopAtSlide").val(se.stopAtSlide);
		$("#stopAfterLoops").val(se.stopAfterLoops);
		$("#hideCaptionAtLimit").val(se.hideCaptionAtLimit);
		$("#hideAllCaptionAtLimit").val(se.hideAllCaptionAtLimit);
		$("#hideSliderAtLimit").val(se.hideSliderAtLimit);
		$("#shadow").val(se.shadow);
		$("#fullWidth").val(se.fullWidth);
		$("#fullScreen").val(se.fullScreen);
		$("#minFullScreenHeight").val(se.minFullScreenHeight);
		$("#fullScreenOffsetContainer").val(se.fullScreenOffsetContainer);
		$("#fullScreenOffset").val(se.fullScreenOffset);
		$("#dottedOverlay").val(se.dottedOverlay);
		$("#DisplayRandom").val(se.DisplayRandom);
		$("#forceFullWidth").val(se.forceFullWidth);
		$("#spinner").val(se.spinner);
		$("#swipe_treshold").val(se.swipe_treshold);
		$("#swipe_min_touches").val(se.swipe_min_touches);
		$("#drag_block_vertical").val(se.drag_block_vertical == "true" ? "on": "off")
		$("#Parallax").val(se.parallax);
		$("#parallaxLevels").val(se.parallaxLevels);
		$("#parallaxBgFreeze").val(se.parallaxBgFreeze);
		$("#parallaxOpacity").val(se.parallaxOpacity);
		$("#parallaxDisableOnMobile").val(se.parallaxDisableOnMobile);
		$("#panZoomDisableOnMobile").val(se.panZoomDisableOnMobile);
		$("#simplifyAll").val(se.simplifyAll);
		$("#minHeight").val(se.minHeight);
	}
	var recomposeslide = function(id, sle) {
		$("#video" + id).val(sle.videotype);
		$("#Transition" + id).val(sle.transition.split(","));
		$("#RandomTransition" + id).val(sle.randomtransition);
		$("#SlotAmount" + id).val(sle.slotamount);
		$("#MasterSpeed" + id).val(sle.masterspeed);
		$("#SlideDelay" + id).val(sle.delay);
		$("#LinkURL" + id).val(sle.link);
		$("#Target" + id).val(sle.target);
		$("#SlideIndex" + id).val(sle.slideindex);
		$("#Thumbnail" + id).val(sle.thumb);
		$("#Title" + id).val(sle.title);
		$("#bgcolor" + id).val(sle.bgcolor);
		$("#lazyload" + id).val(sle.lazyload);
		$("#bgrepeat" + id).val(sle.bgrepeat);
		$("#bgfit" + id).val(sle.bgfit);
		$("#bgfitother" + id).val(sle.bgfitother);
		$("#bgfitend" + id).val(sle.bgfitend);
		$("#bgposition" + id).val(sle.bgposition);
		$("#bgpositionend" + id).val(sle.bgpositionend);
		$("#imgalt" + id).val(sle.imgalt);
		$("#kenburns" + id).val(sle.kenburns);
		$("#duration" + id).val(sle.duration);
		$("#ease" + id).val(sle.ease);
		$("#bgimg" + id).val(sle.bgimg);
		$("#autoplay" + id).val(sle.autoplay == "true" ? "on": "off");
		$("#autoplayonlyfirsttime" + id).val(sle.autoplayonlyfirsttime == "true" ? "on": "off");
		$("#nextslideatend" + id).val(sle.nextslideatend == "true" ? "on": "off");
		$("#videoposter" + id).val(sle.videoposter);
		$("#forcecover" + id).val(sle.forcecover == "1" ? "on": "off");
		$("#ForceRewind" + id).val(sle.forcerewind);

		$("#volume" + id).val(sle.volume == "mute" ? "on": "off");
		$("#videowidth" + id).val(sle.videowidth);
		$("#videoheight" + id).val(sle.videoheight);
		$("#aspectratio" + id).val(sle.aspectratio);
		$("#videopreload" + id).val(sle.videopreload);
		$("#videomp4" + id).val(sle.videomp4);
		$("#videowebm" + id).val(sle.videowebm);
		$("#videoogv" + id).val(sle.videoogv);
		$("#ytid" + id).val(sle.ytid);
		$("#vimeoid" + id).val(sle.vimeoid);
		$("#videocontrols" + id).val(sle.videocontrols == "controls" ? "on": "off");
		$("#videoattributes" + id).val(sle.videoattributes);
		$("#videoloop" + id).val(sle.videoloop);
		$("#sbgspeed" + id).val(sle.speed);
		$("#sbgstart" + id).val(sle.start);
		$("#sbgease" + id).val(sle.easing);
		$("#sbgendspeed" + id).val(sle.endspeed);
		$("#sbgendeasing" + id).val(sle.endeasing);
		$("#incoming" + id).val(sle.incoming);
		$("#outgoing" + id).val(sle.outgoing);
		$("#slide-title" + id).attr("data-id", sle.id);
		$("#slide-title" + id).attr("data-sort", sle.sort);
		
		sle.display_desktop == "off" ? $("#display_desktop" + id).val("off").attr("checked",false):$("#display_desktop" + id).val("on").attr("checked","checked");
		sle.display_tablet == "off" ? $("#display_tablet" + id).val("off").attr("checked",false):$("#display_tablet" + id).val("on").attr("checked","checked");
		sle.display_phone == "off" ? $("#display_phone" + id).val("off").attr("checked",false):$("#display_phone" + id).val("on").attr("checked","checked");



		$("#slidetype_nav" + id).find("li[data-type^='" + sle.type + "']").addClass("active").siblings().removeClass("active");
		var index = $("#slidetype_nav" + id).find("li[data-type^='" + sle.type + "']").index() + 1;
		$("#slidetype_tab" + id + "_example" + index).addClass("active").siblings(".tab-pane").removeClass("active");
		$("#slide-title" + id).addClass("active").siblings("li").removeClass("active");
		$("#slide-tab-content").children(".tab-pane").eq($("#slide-title" + id).index()).addClass("active").siblings(".tab-pane").removeClass("active");
	}
	var recomposeLayers = function(s, l, slle) {
		 $("#panel" + s + "_" + l).attr("data-id", slle.id)
		 $("#panel" + s + "_" + l).attr("data-sort", slle.sort)
		 $("#Item" + s + "_" + l).val(slle.contentoption[0].itemname)

		
		 slle.contentoption[0].displaydesktop == "off" ? $("#display_desktop_" + s + "_" + l).val("off").attr("checked",false):$("#display_desktop_" + s + "_" + l).val("on").attr("checked","checked");
		 slle.contentoption[0].displaytablet == "off" ? $("#display_tablet_" + s + "_" + l).val("off").attr("checked",false):$("#display_tablet_" + s + "_" + l).val("on").attr("checked","checked");
		 slle.contentoption[0].displayphone == "off" ? $("#display_phone_" + s + "_" + l).val("off").attr("checked",false):$("#display_phone_" + s + "_" + l).val("on").attr("checked","checked");
		 

		 $("#contentimg" + s + "_" + l).val(slle.contentoption[1].contentimg)
		 $("#contenttext" + s + "_" + l).val(slle.contentoption[2].contenttext)
		 $("#ytb" + s + "_" + l).val(slle.contentoption[3].youtube)
		 $("#ytbautoplay" + s + "_" + l).val(slle.contentoption[3].autoplay == "true" ? "on": "off")
		 $("#ytbautoplayoft" + s + "_" + l).val(slle.contentoption[3].autoplayonlyfirsttime == "true" ? "on": "off")
		 $("#ytbnext" + s + "_" + l).val(slle.contentoption[3].nextslideatend == "true" ? "on": "off")
		 $("#ytbattributes" + s + "_" + l).val(slle.contentoption[3].videoattributes)
		 $("#ytbcontrols" + s + "_" + l).val(slle.contentoption[3].videocontrols == "controls" ? "on": "off")
		 $("#ytbwidth" + s + "_" + l).val(slle.contentoption[3].videowidth)
		 $("#ytbheight" + s + "_" + l).val(slle.contentoption[3].videoheight)
		 $("#vimeo" + s + "_" + l).val(slle.contentoption[4].vimeo)
		 $("#vimeoautoplay" + s + "_" + l).val(slle.contentoption[4].autoplay == "true" ? "on": "off")
		 $("#vimeoautoplayoft" + s + "_" + l).val(slle.contentoption[4].autoplayonlyfirsttime == "true" ? "on": "off")
		 $("#vimeonfae" + s + "_" + l).val(slle.contentoption[4].vimeonfae == "true" ? "on": "off")
		 $("#vimeoattributes" + s + "_" + l).val(slle.contentoption[4].videoattributes)
		 $("#vimeocontrols" + s + "_" + l).val(slle.contentoption[4].videocontrols == "true" ? "on": "off")
		 $("#vimeowidth" + s + "_" + l).val(slle.contentoption[4].videowidth)
		 $("#vimeoheight" + s + "_" + l).val(slle.contentoption[4].videoheight)
		 $("#html5mp4" + s + "_" + l).val(slle.contentoption[5].mp4)
		 $("#html5webm" + s + "_" + l).val(slle.contentoption[5].webm)
		 $("#html5ogg" + s + "_" + l).val(slle.contentoption[5].ogv)
		 $("#html5autoplay" + s + "_" + l).val(slle.contentoption[5].autoplay == "true" ? "on": "off")
		 $("#html5poster" + s + "_" + l).val(slle.contentoption[5].videoposter)
		 $("#html5autoplayft" + s + "_" + l).val(slle.contentoption[5].autoplayonlyfirsttime == "true" ? "on": "off")
		 $("#html5nsae" + s + "_" + l).val(slle.contentoption[5].nextslideatend == "true" ? "on": "off")
		 $("#html5controls" + s + "_" + l).val(slle.contentoption[5].videocontrols == "true" ? "on": "off")
		 $("#html5forcecover" + s + "_" + l).val(slle.contentoption[5].forcecover)
		 $("#html5forcerewind" + s + "_" + l).val(slle.contentoption[5].forcerewind)
		 $("#html5aspectratio" + s + "_" + l).val(slle.contentoption[5].aspectratio)
		 $("#html5preload" + s + "_" + l).val(slle.contentoption[5].videopreload)
		 $("#html5volume" + s + "_" + l).val(slle.contentoption[5].volume)
		 $("#html5width" + s + "_" + l).val(slle.contentoption[5].videowidth)
		 $("#html5height" + s + "_" + l).val(slle.contentoption[5].videoheight)
		 $("#datax" + s + "_" + l).val(slle.datasettings.x)
		 $("#datay" + s + "_" + l).val(slle.datasettings.y)
		 $("#datahoffset" + s + "_" + l).val(slle.datasettings.hoffset)
		 $("#datavoffset" + s + "_" + l).val(slle.datasettings.voffset)
		 $("#dataspeed" + s + "_" + l).val(slle.datasettings.speed)
		 $("#datasplitin" + s + "_" + l).val(slle.datasettings.splitin)
		 $("#dataelementdelay" + s + "_" + l).val(slle.datasettings.elementdelay)
		 $("#datasplitout" + s + "_" + l).val(slle.datasettings.splitout)
		 $("#dataendelementdelay" + s + "_" + l).val(slle.datasettings.endelementdelay)
		 $("#datastart" + s + "_" + l).val(slle.datasettings.start)
		 $("#dataeasing" + s + "_" + l).val(slle.datasettings.easing)
		 $("#dataendspeed" + s + "_" + l).val(slle.datasettings.endspeed)
		 $("#dataend" + s + "_" + l).val(slle.datasettings.end)
		 $("#dataendeasing" + s + "_" + l).val(slle.datasettings.endeasing)
		 $("#stylingcaptions" + s + "_" + l).val(slle.Othersettings.stylingcaptions)
		 $("#parallaxsettings" + s + "_" + l).val(slle.Othersettings.parallaxsettings)
		 $("#scrollbelowslider" + s + "_" + l).val(slle.Othersettings.scrollbelowslider)
		 $("#scrolloffset" + s + "_" + l).val(slle.Othersettings.scrolloffset)
		 $("#resizeme" + s + "_" + l).val(slle.Othersettings.resizeme)
		 $("#withoutcorner" + s + "_" + l).val(slle.Othersettings.withoutcorner)
		 $("#loopanimations" + s + "_" + l).val(slle.Othersettings.loopanimations)
		 $("#TransitionX" + s + "_" + l).val(slle.transitionoption[0].x)
		 $("#TransitionY" + s + "_" + l).val(slle.transitionoption[0].y)
		 $("#TransitionZ" + s + "_" + l).val(slle.transitionoption[0].z)
		 $("#RotationX" + s + "_" + l).val(slle.transitionoption[0].rotationX)
		 $("#RotationY" + s + "_" + l).val(slle.transitionoption[0].rotationY)
		 $("#RotationZ" + s + "_" + l).val(slle.transitionoption[0].rotationZ)
		 $("#ScaleX" + s + "_" + l).val(slle.transitionoption[0].scaleX * 100)
		 $("#ScaleY" + s + "_" + l).val(slle.transitionoption[0].scaleY * 100)
		 $("#SkewX" + s + "_" + l).val(slle.transitionoption[0].skewX)
		 $("#SkewY" + s + "_" + l).val(slle.transitionoption[0].skewY)
		 $("#Opacity" + s + "_" + l).val(slle.transitionoption[0].opacity * 100)
		 $("#Perspective" + s + "_" + l).val(slle.transitionoption[0].perspective)
		 $("#OriginX" + s + "_" + l).val(slle.transitionoption[0].originX.replace(/%/g, ""))
		 $("#OriginY" + s + "_" + l).val(slle.transitionoption[0].originY.replace(/%/g, ""))
		 $("#Classes" + s + "_" + l).val(slle.transitionoption[0].Classes)
		 $("#OutTransitionX" + s + "_" + l).val(slle.transitionoption[1].x)
		 $("#OutTransitionY" + s + "_" + l).val(slle.transitionoption[1].y)
		 $("#OutTransitionZ" + s + "_" + l).val(slle.transitionoption[1].z)
		 $("#OutRotationX" + s + "_" + l).val(slle.transitionoption[1].rotationX)
		 $("#OutRotationY" + s + "_" + l).val(slle.transitionoption[1].rotationY)
		 $("#OutRotationZ" + s + "_" + l).val(slle.transitionoption[1].rotationZ)
		 $("#OutScaleX" + s + "_" + l).val(slle.transitionoption[1].scaleX * 100)
		 $("#OutScaleY" + s + "_" + l).val(slle.transitionoption[1].scaleY * 100)
		 $("#OutSkewX" + s + "_" + l).val(slle.transitionoption[1].skewX)
		 $("#OutSkewY" + s + "_" + l).val(slle.transitionoption[1].skewY)
		 $("#OutOpacity" + s + "_" + l).val(slle.transitionoption[1].opacity * 100)
		 $("#OutPerspective" + s + "_" + l).val(slle.transitionoption[1].perspective)
		 $("#OutOriginX" + s + "_" + l).val(slle.transitionoption[1].originX.replace(/%/g, ""))
		 $("#OutOriginY" + s + "_" + l).val(slle.transitionoption[1].originY.replace(/%/g, ""))
		 $("#OutClasses" + s + "_" + l).val(slle.transitionoption[1].Classes)
		 $("#Url" + s + "_" + l).val(slle.linkoption.Url)
		 $("#Target" + s + "_" + l).val(slle.linkoption.Target)
		 $("#Width" + s + "_" + l).val(slle.styleoption.width)
		 $("#Height" + s + "_" + l).val(slle.styleoption.height)
		 $("#Top" + s + "_" + l).val(slle.styleoption.top)
		 $("#Left" + s + "_" + l).val(slle.styleoption.left)
		 $("#PaddingTop" + s + "_" + l).val(slle.styleoption.padding_top)
		 $("#PaddingRight" + s + "_" + l).val(slle.styleoption.padding_right)
		 $("#PaddingBottom" + s + "_" + l).val(slle.styleoption.padding_bottom)
		 $("#PaddingLeft" + s + "_" + l).val(slle.styleoption.padding_left)
		 $("#BorderTop" + s + "_" + l).val(slle.styleoption.border_top)
		 $("#BorderRight" + s + "_" + l).val(slle.styleoption.border_right)
		 $("#BorderBottom" + s + "_" + l).val(slle.styleoption.border_bottom)
		 $("#BorderLeft" + s + "_" + l).val(slle.styleoption.border_left)
		 $("#FontFamily" + s + "_" + l).val(slle.styleoption.font_family)
		 $("#FontSize" + s + "_" + l).val(slle.styleoption.font_size)
		 $("#Line-height" + s + "_" + l).val(slle.styleoption.line_height)
		 $("#Color" + s + "_" + l).val(slle.styleoption.color)
		 $("#BackgroundColor" + s + "_" + l).val(slle.styleoption.background_color)
		 $("#Roundedcorners" + s + "_" + l).val(slle.styleoption.border_radius)
		 $("#Word-wrap" + s + "_" + l).val(slle.styleoption.word_wrap)
		 $("#CustomCSS" + s + "_" + l).val(slle.styleoption.CustomCSS)
		 $("#Id" + s + "_" + l).val(slle.attributesoption.id)
		 $("#Class" + s + "_" + l).val(slle.attributesoption.class)
		 $("#Title" + s + "_" + l).val(slle.attributesoption.title)
		 $("#Alt" + s + "_" + l).val(slle.attributesoption.alt)
		 $("#Rel" + s + "_" + l).val(slle.attributesoption.rel)
		 $("#layercontent_tab" + s + "_" + l).find("li[data-type^='" + slle.contentoption[0].type + "']").addClass("active").siblings().removeClass("active");
		var index = $("#layercontent_tab" + s + "_" + l).find("li[data-type^='" + slle.contentoption[0].type + "']").index() + 1;
		$("#layercontent_tab" + s + "_" + l + "_example" + index).addClass("active").siblings(".tab-pane").removeClass("active");
		$("#layervideo" + s + "_" + l).find("li[data-type^='" + slle.contentoption[0].vtype + "']").addClass("active").siblings().removeClass("active");
		var index2 = $("#layervideo" + s + "_" + l).find("li[data-type^='" + slle.contentoption[0].vtype + "']").index() + 1;
		$("#layervideo_tab" + s + "_" + l + "_example" + index2).addClass("active").siblings(".tab-pane").removeClass("active");
		$("#contentcode" + s + "_" + l).find("span[data-code^='" + slle.contentoption[2].contentcode + "']").addClass("active").siblings("span").removeClass("active");
	}
	
	var ShowroomStyling="#1_1";
	var previewlayers = function(s, l) {
		contentoption(s, l);
		othersettings(s, l); 
		styleoption(s, l);
		var layersContent = function() {
			var lc = " ";
			if (cop[0].type == "image") {
				if(styleop.width!=0 && styleop.width.length>1 || styleop.height!=0 && styleop.height.length>1){
				var	maxw="max-width:100%;max-height:100%"
			    }else{
					var	maxw=""
				}
				if(cop[1].contentimg.length<=1){
					cop[1].contentimg="Resource/images/transparent.png";
				}
				lc = " <img  src=\"Resource/images/dummy.png\"  data-lazyload=\"" + cop[1].contentimg + "\" style=\""+maxw+"\" />"
				
			}
			if (cop[0].type == "text") {
			//	if(cop[2].contentcode!="div"){
			//		lc = "<" + cop[2].contentcode + "> " +cop[2].contenttext+ "</" + cop[2].contentcode + ">"
			//	}else{
					lc = cop[2].contenttext
			//	}
			}
			return lc;
		}
		
		var layersVideo = function() {
			var vc = " ";
			if (cop[0].type == "video") {
				if (cop[0].vtype == "YouTube") {
					
					var attributes=cop[3].videoattributes;
					if(cop[3].autoplay== "true"){
					   attributes +="&autoplay=1"	
					}
					if(cop[3].autoplayonlyfirsttime!= "true"){
					   attributes +="&loop=1&playlist="+cop[3].youtube
					}
					if(cop[3].videocontrols != "true"){
					   attributes +="&controls=0&showinfo=0&rel=0"
					}
					
					vc = " data-autoplay=\"" + cop[3].autoplay + "\" data-autoplayonlyfirsttime=\"" + cop[3].autoplayonlyfirsttime + "\" data-nextslideatend=\"" + cop[3].nextslideatend + "\" data-ytid=\"" + cop[3].youtube + "\" data-videoattributes=\"" + attributes + "\" data-videocontrols=\"" + cop[3].videocontrols + "\" data-videowidth=\"" + cop[3].videowidth + "\" data-videoheight=\"" + cop[3].videoheight + "\""
				}
				if (cop[0].vtype == "Vimeo") {
					
					var attributes=cop[4].videoattributes;
					if(cop[4].autoplay== "true"){
					   attributes +="&autoplay=1"	
					}
					if(cop[4].autoplayonlyfirsttime!= "true"){
					   attributes +="&loop=1"
					}
					if(cop[4].videocontrols != "true"){
					   attributes +="&title=0&byline=0&portrait=0"
					}
					
					vc = " data-autoplay=\"" + cop[4].autoplay + "\" data-autoplayonlyfirsttime=\"" + cop[4].autoplayonlyfirsttime + "\" data-nextslideatend=\"" + cop[4].nextslideatend + "\" data-vimeoid=\"" + cop[4].vimeo + "\" data-videoattributes=\"" + attributes + "\" data-videocontrols=\"" + cop[4].videocontrols + "\" data-videowidth=\"" + cop[4].videowidth + "\" data-videoheight=\"" + cop[4].videoheight + "\""
				}
				if (cop[0].vtype == "HTML5") {
					vc = " data-autoplay=\"" + cop[5].autoplay + "\" data-autoplayonlyfirsttime=\"" + cop[5].autoplayonlyfirsttime + "\" data-nextslideatend=\"" + cop[5].nextslideatend + "\" data-videowidth=\"" + cop[5].videowidth + "\" data-videoheight=\"" + cop[5].videoheight + "\" data-videpreload=\"" + cop[5].videopreload + "\" data-videomp4=\"" + cop[5].mp4 + "\" data-videowebm=\"" + cop[5].webm + "\" data-videoogv=\"" + cop[5].ogv + "\" data-videocontrols=\"" + cop[5].videocontrols + "\" data-forcecover=\"" + cop[5].forcecover + "\" data-forcerewind=\"" + cop[5].forcerewind + "\" data-aspectratio=\"" + cop[5].aspectratio + "\" data-volume=\"" + cop[5].volume + "\" data-videoposter=\"" + cop[5].videoposter + "\""
				}
			}
			return vc;
			
		}
		
		var layersTransition = function() {
			transitionoption(s, l);
			var customin = "",
			customout = "";
			if (tnop[0].Classes == "customin") {
				customin = " data-customin=\"x:" + tnop[0].x + ";y:" + tnop[0].y + ";z:" + tnop[0].z + ";rotationX:" + tnop[0].rotationX + ";rotationY:" + tnop[0].rotationY + ";rotationZ:" + tnop[0].rotationZ + ";scaleX:" + tnop[0].scaleX + ";scaleY:" + tnop[0].scaleY + ";skewX:" + tnop[0].skewX + ";skewY:" + tnop[0].skewY + ";opacity:" + tnop[0].opacity + ";transformPerspective:" + tnop[0].perspective + ";transformOrigin:" + tnop[0].originX + " " + tnop[0].originY + "\""
			}
			if (tnop[1].Classes == "customout") {
				customout = " data-customout=\"x:" + tnop[1].x + ";y:" + tnop[1].y + ";z:" + tnop[1].z + ";rotationX:" + tnop[1].rotationX + ";rotationY:" + tnop[1].rotationY + ";rotationZ:" + tnop[1].rotationZ + ";scaleX:" + tnop[1].scaleX + ";scaleY:" + tnop[1].scaleY + ";skewX:" + tnop[1].skewX + ";skewY:" + tnop[1].skewY + ";opacity:" + tnop[1].opacity + ";transformPerspective:" + tnop[1].perspective + ";transformOrigin:" + tnop[1].originX + " " + tnop[1].originY + "\"";
			}
			var tc = customin + customout
			return tc;
		}
		var layersTransitionClass = function() {
			var tcc = " ";
			if (tnop[0].Classes != "customin") {
				tcc += tnop[0].Classes + " "
			} else {
				tcc += " customin "
			}
			if (tnop[1].Classes != "customout") {
				tcc += tnop[1].Classes + " "
			} else {
				tcc += " customout "
			}
			return tcc;
		}
		var layerslink = function() {
			linkoption(s, l);
			if (linkop.Url && linkop.Url != "http://" && linkop.Url.length > 1) {
				
			//	lc = "<a href=\"" + linkop.Url + "\" target=\"" + linkop.Target + "\">" + layersContent() + "</a>"
			} else {
				
				if(othop.loopanimations != "none"){
					if (othop.loopanimations == "rs-wave") {
						lc = "<div class=\"rs-wave\"  data-speed=\"2\" data-angle=\"0\" data-radius=\"5\" data-origin=\"50% 50%\">"+layersContent()+"</div>";
					}
					if(othop.loopanimations == "rs-pulse") {
						lc = "<div class=\"rs-pulse\"  data-easing=\"Power4.easeInOut\" data-speed=\"2\" data-zoomstart=\"1\" data-zoomend=\"0.95\">"+layersContent()+"</div>";
					}
					if(othop.loopanimations == "rs-pendulum") {
						lc = "<div class=\"rs-pendulum\"  data-easing=\"Power1.easeInOut\" data-startdeg=\"-6\" data-enddeg=\"6\" data-speed=\"2\" data-origin=\"50% 75%\">"+layersContent()+"</div>";
					}
					if(othop.loopanimations == "rs-slideloop") {
						lc ="<div class=\" rs-slideloop\" data-easing=\"Power3.easeInOut\" data-speed=\"0.5\" data-xs=\"-5\" data-xe=\"5\" data-ys=\"0\" data-ye=\"0\">"+layersContent()+"</div>";
					}
					if(othop.loopanimations == "rs-rotate") {
						lc ="<div class=\" rs-rotate\"  data-easing=\"Power1.easeInOut\" data-startdeg=\"-6\" data-enddeg=\"6\" data-speed=\"2\" data-origin=\"50% 75%\">"+layersContent()+"</div>";
					}
				
				}else{
					lc = layersContent();
				}
			}
			return lc;
		}
		var layersstyle = function() {
			var sc = " ";
			for (var pro in styleop) {
				if (styleop[pro] && styleop[pro] != "undefined") {
					if (pro != "CustomCSS") {
						sc += pro.replace(/_/g, "-") + ":" + styleop[pro] + ";"
					} else {
						sc += styleop[pro] + ";"
					}
				}
			}
			return sc;
		}
		var layerothersettings = function() {
			var othc = " ";
			if (othop.stylingcaptions != "none") {
				othc += othop.stylingcaptions + " "
			}
			if (othop.parallaxsettings != "none") {
				othc += othop.parallaxsettings + " "
			}
			if (othop.scrollbelowslider != "off") {
				othc += "tp-scrollbelowslider "
			}
			if (othop.resizeme != "off") {
				othc += "tp-resizeme "
			}
			if (othop.withoutcorner != "none") {
				othc += othop.withoutcorner + " "
			}
			return othc;
		}
		var layersattributes = function() {
			attributesoption(s, l);
			var ac = " ";
			for (var pro in aop) {
				if (aop[pro] && aop[pro] != "undefined") {
					if (pro != "class") {
						ac += pro + "=\"" + aop[pro] + "\" "
					} else {
						var vclass = cop[0].type == 'video' ? ' tp-videolayer': ' ';
						ac += pro + "=\"tp-caption " + aop[pro] + layersTransitionClass() + layerothersettings() + vclass + "\" "
					}
				} else if (pro == "class") {
					var vclass = cop[0].type == 'video' ? ' tp-videolayer ': ' ';
					ac += pro + "=\"tp-caption " + layersTransitionClass() + layerothersettings() + vclass + " \" "
				}
			}
			return ac;
		}
		var layersdatasettings = function() {
			datasettings(s, l);
			var dc = " ";
			for (var pro in dsgop) {
				if (dsgop[pro] && dsgop[pro] != "undefined") {
					dc += " data-" + pro + "=\"" + dsgop[pro] + "\" "
				}
			}
			return dc;
		}
		linkoption(s, l);
		if (linkop.Url && linkop.Url != "http://" && linkop.Url.length >= 1) {
		$(".page" + s).append("<a href=\"" + linkop.Url + "\" target=\"" + linkop.Target + "\" style=\""+  layersstyle() + "\" " + layersTransition() + layersattributes() + layersdatasettings() + layersVideo() + "data-scrolloffset=\"" + othop.scrolloffset + "\" >" + layerslink() + "</a>");
		}else{
		$(".page" + s).append("<"+cop[2].contentcode+" style=\"" + layersstyle() + "\" " + layersTransition() + layersattributes() + layersdatasettings() + layersVideo() + "data-scrolloffset=\"" + othop.scrolloffset + "\" >" + layerslink() + "</"+cop[2].contentcode+">");
		}
	}
var copyslide=function(copyid){
	
		receive_data["slide_" + copyid]= JSON.parse(acquisition_data(copyid));
		
		var id = 1;
		for (i = 0; i < slideTab.children("li").length; i++) {
			id = Math.max(slideTab.find("li").eq(i).find(".ico-close").attr("id"), id);
		}
		id++;
		slideTab.append("<li id=\"slide-title" + id + "\" class=\"slide-tab-nav\"  data-id=\"" + 0 + "\" data-sort=\"" + parseInt(slideTab.find('li').length + 1) + "\"> <a  data-toggle=\"tab\" href=\"#slide_tab1_example" + id + "\" >Slide<span>" + parseInt(slideTab.find('li').length + 1) + "</span><i class=\"clip-cancel-circle-2 ico-close\" id=\"" + id + "\" ></i><i class=\"drag clip-arrow-3\"></i></a></li>");
		
	var copydata=eval("(" + "receive_data.slide_" +copyid + ")");
		console.log(copydata)
		copydata.id=0;	
		copydata.sort=slideTab.children("li").length+1
		receive_data["slide_"+id]= copydata;
		SlideEvent(id);
		for(i in copydata.layers){
			copydata.layers[i].id=0
		}
		var newslide= $("#slide-title" + id);
			newslide.on("click",function(){
				if($(this).hasClass("active")) return false;
				loadslider($(this).find(".ico-close").attr("id"));
			})
			newslide.click()
			refreshscript(id);
	}
	
	
	var copylayer=function(copyid){
		var n=copyid.split(",");
		
		var layerdate = receive_data["slide_" + n[0]].layers
		
			if(!layerdate){
				receive_data["slide_" + n[0]]["layers"]={};
				layerdate = receive_data["slide_" + n[0]].layers;
			}
		var copylayerdata=layerdate["layer_"+n[0]+"_"+n[1]];
			if(!copylayerdata){
				layerdate["layer_"+n[0]+"_"+n[1]]={};
				copylayerdata=layerdate["layer_"+n[0]+"_"+n[1]]
				copylayerdata.id=$("#panel" + n[0] + "_" + n[1]).attr("data-id")
				copylayerdata["currentid"]= parseInt($("#panel" + n[0] + "_" + n[1]).attr("data-new"))
				copylayerdata.sort=$("#panel" + n[0] + "_" + n[1]).attr("data-sort")
			}
			contentoption(n[0], n[1]);
			transitionoption(n[0], n[1]);
			linkoption(n[0], n[1]);
			styleoption(n[0], n[1]);
			attributesoption(n[0], n[1]);
			datasettings(n[0], n[1]);
			othersettings(n[0], n[1]);
		
			copylayerdata["contentoption"]=cop;
			copylayerdata["transitionoption"]=tnop;
			copylayerdata["linkoption"]=linkop;
			copylayerdata["styleoption"]=styleop;
			copylayerdata["attributesoption"]=aop;
			copylayerdata["datasettings"]=dsgop;
			copylayerdata["Othersettings"]=othop;
			
		var Layernumber = 0,length=1;
		for (i = 0; i < $("#layers-accordion" + n[0]).children(".panel").length; i++) {
			Layernumber = Math.max($("#layers-accordion" + n[0]).children(".panel").eq(i).attr("data-new"), Layernumber);
			length++
		}
			layerdate["layer_"+n[0]+"_"+(Layernumber+1)]= jQuery.extend(true, {},copylayerdata);
			
			
		var newlayer=layerdate["layer_"+n[0]+"_"+(Layernumber+1)];
			newlayer.contentoption[0].itemname=newlayer.contentoption[0].itemname+"(copy)"
			newlayer.id=0
			newlayer["currentid"]=Layernumber
			newlayer.sort=length
		addLayer(n[0], Layernumber + 1, newlayer);
		
		if($("#slide_tab1_example" + n[0]).find(".panel").length>1){
			$("#panel" +n[0]+"_"+(Layernumber + 1)).find(".panel-title a").click();
		}
	}
	
	var previewslide = function(id, op) {
		var slidebg = function() {
			var c = " ";
			typeoption(id);
			if (typeop.type == "image") {
				imageoption(id);
				var src = "",
				style = "",
				bgimg = "",
				data = "";
				bf = "";
				if(iop.bgimg.length <4){
					iop.bgimg="Resource/images/transparent.png";
				}
				src = "src=\"" + iop.bgimg + "\"";
				if (iop.bgcolor.length > 1) {
					src = "src=\"Resource/images/transparent.png\"";
					style = "style=\" background-color:" + iop.bgcolor + "\"";
				}
				if (iop.lazyload == "on" && iop.bgcolor.length <= 1) {
					bgimg = "data-lazyload=\"" + iop.bgimg + "\"";
					src = "src=\"Resource/images/dummy.png\"";
				}
				
				if (iop.kenburns == "on") {
					bf = "data-bgfit=\"" + iop.bgfitother + "\"";
				} else {
					bf = "data-bgfit=\"" + iop.bgfit + "\"";
				}
				for (var pro in iop) {
					if (iop[pro] && iop[pro] != "undefined" && pro != "bgcolor" && pro != "lazyload" && pro != "bgimg" && pro != "bgfitother" && pro != "bgfit") {
						data += "data-" + pro + "=\"" + iop[pro] + "\" "
					}
				}
				c = "<img " + src + " " + style + " " + bgimg + "class=\"slidebg" + id + "\" " + data + bf + " alt=\""+iop.imgalt+"\" />";
			} else {
				mediaoption(id);
				c +="<img src=\""+mop.videoposter?mop.videoposter:"Resource/images/transparent.png"+"\"  alt=\"video_forest\"  data-bgposition=\"center center\" data-bgfit=\"cover\" data-bgrepeat=\"no-repeat\">"
				
				if ($("#video" + id).val() == "HTML5 Video") {
					c += "<div class=\"tp-caption " + mop.incoming + " " + mop.outgoing + " fullscreenvideo tp-videolayer\" data-x=\"0\" data-y=\"0\" data-speed=\"" + mop.speed + "\" data-start=\"" + mop.start + "\" data-easing=\"" + mop.easing + "\" data-endspeed=\"" + mop.endspeed + "\" data-endeasing=\"" + mop.endeasing + "\" data-autoplay=\"" + mop.autoplay + "\" data-autoplayonlyfirsttime=\"" + mop.autoplayonlyfirsttime + "\" data-nextslideatend=\"" + mop.nextslideatend + "\" data-videowidth=\"100%\" data-videoheight=\"100%\" data-videpreload=\"" + mop.videopreload + "\" data-videomp4=\"" + mop.videomp4 + "\"  data-videoogv=\"" + mop.videoogv + "\" data-videowebm=\"" + mop.videowebm + "\" data-videocontrols=\"" + mop.videocontrols + "\" data-forcecover=\"" + mop.forcecover + "\" data-forcerewind=\"" + mop.forcerewind + "\" data-aspectratio=\"" + mop.aspectratio + "\" data-volume=\"" + mop.volume + "\" data-videoposter=\"" + mop.videoposter + "\"> </div>"
				}
				if ($("#video" + id).val() == "YouTube Video") {
					
					var attributes=mop.videoattributes;
					if(mop.autoplay== "true"){
					   attributes +="&autoplay=1"	
					}
					if(mop.autoplayonlyfirsttime!= "true"){
					   attributes +="&loop=1&playlist="+mop.youtube
					}
					if(mop.videocontrols != "true"){
					   attributes +="&controls=0&showinfo=0&rel=0"
					}
					
					c += "<div style=\"zindex:0\" class=\"tp-caption " + mop.incoming + " " + mop.outgoing + " fullscreenvideo tp-videolayer\" data-x=\"0\" data-y=\"0\" data-speed=\"" + mop.speed + "\" data-start=\"" + mop.start + "\" data-easing=\"" + mop.easing + "\" data-endspeed=\"" + mop.endspeed + "\" data-endeasing=\"" + mop.endeasing + "\" data-autoplay=\"" + mop.autoplay + "\" data-autoplayonlyfirsttime=\"" + mop.autoplayonlyfirsttime + "\" data-nextslideatend=\"" + mop.nextslideatend + "\" style=\"z-index: 8\" data-ytid=\"" + mop.ytid + "\" data-videoattributes=\"" + attributes + "\" data-videocontrols=\"" + mop.videocontrols + "\" data-videowidth=\"100%\" data-videoheight=\"100%\" data-videoposter=\"" + mop.videoposter + "\"> </div>"
				}
				if ($("#video" + id).val() == "Vimeo Video") {
					
					var attributes=mop.videoattributes;
					if(mop.autoplay== "true"){
					   attributes +="&autoplay=1"	
					}
					if(mop.autoplayonlyfirsttime != "true"){
					   attributes +="&loop=1"
					}
					if(mop.videocontrols != "true"){
					   attributes +="&title=0&byline=0&portrait=0"
					}
					
					c += "<div style=\"zindex:0\" class=\"tp-caption " + mop.incoming + " " + mop.outgoing + " fullscreenvideo tp-videolayer\" data-x=\"0\" data-y=\"0\" data-speed=\"" + mop.speed + "\" data-start=\"" + mop.start + "\" data-easing=\"" + mop.easing + "\" data-endspeed=\"" + mop.endspeed + "\" data-endeasing=\"" + mop.endeasing + "\" data-autoplay=\"" + mop.autoplay + "\" data-autoplayonlyfirsttime=\"" + mop.autoplayonlyfirsttime + "\" data-nextslideatend=\"" + mop.nextslideatend + "\" style=\"z-index: 8\" data-vimeoid=\"" + mop.vimeoid + "\" data-videoattributes=\"" + attributes + "\" data-videocontrols=\"" + mop.videocontrols + "\" data-videowidth=\"100%\" data-videoheight=\"100%\" data-videoposter=\"" + mop.videoposter + "\"> </div>"
				}
			}
			
			return c
		}
		var slidedata = function() {
			slideoption(id);
			var d = "";
			for (var pro in sop) {
				if (sop[pro] && pro != "Url" && pro != "randomtransition" && pro != "id" && pro != "sort" && pro != "temporary" && sop[pro]!="http://")(d += " data-" + pro + "=\"" + sop[pro] + "\"")
			}
			if (sop.randomtransition == "on") {
				d += " data-randomtransition=\"" + sop.randomtransition + "\""
			} else {
				var ra=sop.transition.split(",");
				d += " data-transition=\"" +ra[Math.floor(Math.random()*ra.length)] + "\""
			}
			return d
		}
		$(".slidepreview").find(".box").append("<li " + slidedata() + " class=page" + id + "  data-saveperformance=\"off\" >" + slidebg() + "</li>");
	}
	
	
	
	var sortlayer = function(id) {
		$("#layers-accordion" + id).children(".panel-default").each(function() {
			$(this).attr("data-sort", $(this).index() + 1)
		});
	}
	var dellayer = function(d) {
		if (confirm("Sure to delete ?")) {
			if ($("#panel" + d).attr("data-id") == "0") {
				$("#panel" + d).remove();
			} else {
				new_d = $("#panel" + d).attr("data-id");
				$.get($("#ajaxParameters").attr("DeleteDataUrl_Layer"), {
					DeleteID: new_d
				},
				function(data) {
					if (data > 0)
					 {
						$("#panel" + d).remove();
						sortlayer(d.split("_")[0]);
					}
				});
			}
			sortlayer(d.split("_")[0]);
		}
	}
	var refreshscript = function(id) {
		if ($("#previewcontent").css("display") != "none" && slideTab.children("li").length!==0) {
			$("#previewcontent .tp-banner-container").remove();
			$("#previewcontent").html("<div class=\"tp-banner-container\"><div  class=\"tp-banner slidepreview\"><ul class=\"box\"></ul></div></div>");
			previewslide(id);
			for (x = 0; x < $("#layers-accordion" + id).children(".panel").length; x++) {
				nb = $("#layers-accordion" + id).children(".panel").eq(x).data("new");
				previewlayers(id, nb);
			}
			slidesettings();
		}
	}
	var slideName = function() {
		slideTab.find("li").each(function() {
			var nu = $(this).index() + 1;
			$(this).find("span").html(nu);
			$(this).data("sort",nu);
		});
	}
	var addLayer = function(SlideID, LayersID, op) {
		$("#layers-accordion" + SlideID).append(layersHtml.replace(/<SlideID>/g, SlideID).replace(/<LayersID>/g, LayersID));
		if (op == "new") {
			$("#panel" + SlideID + "_" + LayersID).attr("data-id", "0");
			$("#panel" + SlideID + "_" + LayersID).find("input[type=checkbox]").each(function() {
				checkboxval($(this));
			});
		} else if (op) {
			recomposeLayers(SlideID, LayersID, op);
			$("#panel" + SlideID + "_" + LayersID).find("input[type=checkbox]").each(function() {
				if ($(this).val() == "off") {
					$(this).removeAttr("checked")
				} else {
					$(this).attr("checked", "checked")
				}
				checkboxval($(this));
			});
		}
		if (LayersID <= 1) {
			$("#layers-accordion" + SlideID).children(".panel-default").eq(0).find(".accordion-toggle").removeClass("collapsed")
			$("#layers-accordion" + SlideID).children(".panel-default").eq(0).find(".panel-collapse").removeClass("collapse").addClass("in")
		}
		$("#dellayer" + SlideID + "_" + LayersID).on("click",
		function() {
			dellayer($(this).attr("delid"))
		});
		$("#contentcode" + SlideID + "_" + LayersID).find("span").on("click",
		function() {
			$(this).addClass("active").siblings().removeClass("active")
		})
		$("#slidecopy" + SlideID + "_" + LayersID).on("click",function() {
			copylayer($(this).data("layerscopy"));
		})
		sortlayer(SlideID);
		modalImg(SlideID, LayersID);
		$("#panel" + SlideID + "_" + LayersID).attr("data-sort", $("#panel" + SlideID + "_" + LayersID).index() + 1);
		$("#panel" + SlideID + "_" + LayersID).find('.make-switch')['bootstrapSwitch']();
		$("#panel" + SlideID + "_" + LayersID).find('.tooltips').tooltip();
		$("#panel" + SlideID + "_" + LayersID).find(".form-validation").each(function() {
			$(this).newValidator()
		});
		$("#panel" + SlideID + "_" + LayersID).find('.color-picker').colorpicker();
		$("#panel" + SlideID + "_" + LayersID).find(".ShowroomStyling").on("click",function(){ ShowroomStyling="#stylingcaptions"+SlideID + "_" + LayersID });
	}
	var addSlide = function(SlideID, op) {
		slidecontent.append(SlidersHtml.replace(/<SlideID>/g, SlideID));
		contentnot();
		if (op && op != "newSlideID") {
			recomposeslide(SlideID, op);
			$("#slide_tab1_example" + SlideID).find("input[type=checkbox]").each(function() {
				if ($(this).val() == "off") {
					$(this).removeAttr("checked")
				} else{
					$(this).attr("checked", "checked")
				}
				checkboxval($(this));
			});
		}else{
			$("#slide_tab1_example" + SlideID).find("input[type=checkbox]").each(function() {
				checkboxval($(this));
			});
		}
		$("#add-layer" + SlideID).on("click",function() {
			var Layernumber = 0;
			for (i = 0; i < $("#layers-accordion" + SlideID).children(".panel").length; i++) {
				Layernumber = Math.max($("#layers-accordion" + SlideID).children(".panel").eq(i).data("new"), Layernumber);
			}
			addLayer(SlideID, Layernumber + 1, "new");
			if($("#slide_tab1_example" + SlideID).find(".panel").length>1){
				$("#panel" + SlideID+"_"+(Layernumber + 1)).find(".panel-title a").click();
			}
		});
		$("#slidecopy"+SlideID).on("click",function() {
			copyslide($(this).data("slidecopy"));
		})
		modalImg(SlideID);
		$("#layers-accordion" + SlideID).dragsort({
			dragSelector: ".panel-heading .drag",
			placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
			dragEnd: function() {
				sortlayer(SlideID);
			}
		});
		$("#slide_tab1_example" + SlideID).find('.make-switch')['bootstrapSwitch']();
		$("#slide_tab1_example" + SlideID).find('.tooltips').tooltip();
		$("#slide_tab1_example" + SlideID).find(".form-validation").each(function() {
			$(this).newValidator()
		});
		$("#slide_tab1_example" + SlideID).find('.color-picker').colorpicker();
	}
	$("#slide-tab-title").dragsort({
		dragSelector: "li .drag",
		placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
		dragEnd: slideName
	});
	var checkboxval = function(e, op) {
		if (e.attr("checked")) {
			e.attr("checked", true);
			e.val("on")
		} else {
			e.attr("checked", false);
			e.val("off")
		}
		e.on("click",
		function() {
			if (e.attr("checked")) {
				e.attr("checked", false);
				e.val("off");
			} else {
				e.attr("checked", true);
				e.val("on");
			}
		})
		 e.parent(".make-switch").on("click",
		function() {
			if ($(this).children(".switch-animate").length > 0) {
				if (e.attr("checked")) {
					$(this).children(".switch-animate").addClass("switch-off").removeClass("switch-on");
					e.attr("checked", false);
					e.val("off");
				} else {
					$(this).children(".switch-animate").addClass("switch-on").removeClass("switch-off");
					e.attr("checked", true);
					e.val("on");
				}
			}
		})
	}
	var settings = function() {
		stg = {
			delay: parseInt($("#delay").val()),
			startheight: parseInt($("#startheight").val()),
			startwidth: parseInt($("#startwidth").val()),
			fullScreenAlignForce: $("#fullScreenAlignForce").val(),
			autoHeight: $("#autoHeight").val(),
			hideTimerBar: $("#hideTimerBar").val(),
			hideThumbs: parseInt($("#hideThumbs").val()),
			hideNavDelayOnMobile: parseInt($("#hideNavDelayOnMobile").val()),
			thumbWidth: parseInt($("#thumbWidth").val()),
			thumbHeight: parseInt($("#thumbHeight").val()),
			thumbAmount: parseInt($("#thumbAmount").val()),
			navigationType: $("#navigationType").val(),
			navigationArrows: $("#navigationArrows").val(),
			navigationInGrid: $("#navigationInGrid").val(),
			hideThumbsOnMobile: $("#hideThumbsOnMobile").val(),
			hideBulletsOnMobile: $("#hideBulletsOnMobile").val(),
			hideArrowsOnMobile: $("#hideArrowsOnMobile").val(),
			hideThumbsUnderResoluition: parseInt($("#hideThumbsUnderResoluition").val()),
			navigationStyle: $("#navigationStyle").val(),
			navigationHAlign: $("#navigationHAlign").val(),
			navigationVAlign: $("#navigationVAlign").val(),
			navigationHOffset: parseInt($("#navigationHOffset").val()),
			navigationVOffset: parseInt($("#navigationVOffset").val()),
			soloArrowLeftHalign: $("#soloArrowLeftHalign").val(),
			soloArrowLeftValign: $("#soloArrowLeftValign").val(),
			soloArrowLeftHOffset: parseInt($("#soloArrowLeftHOffset").val()),
			soloArrowLeftVOffset: parseInt($("#soloArrowLeftVOffset").val()),
			soloArrowRightHalign: $("#soloArrowRightHalign").val(),
			soloArrowRightValign: $("#soloArrowRightValign").val(),
			soloArrowRightHOffset: parseInt($("#soloArrowRightHOffset").val()),
			soloArrowRightVOffset: parseInt($("#soloArrowRightVOffset").val()),
			keyboardNavigation: $("#keyboardNavigation").val(),
			touchenabled: $("#touchenabled").val(),
			onHoverStop: $("#onHoverStop").val(),
			startWithSlide: parseInt($("#startWithSlide").val()),
			stopAtSlide: parseInt($("#stopAtSlide").val()),
			stopAfterLoops: parseInt($("#stopAfterLoops").val()),
			hideCaptionAtLimit: parseInt($("#hideCaptionAtLimit").val()),
			hideAllCaptionAtLimit: parseInt($("#hideAllCaptionAtLimit").val()),
			hideSliderAtLimit: parseInt($("#hideSliderAtLimit").val()),
			shadow: parseInt($("#shadow").val()),
			fullWidth: $("#fullWidth").val(),
			fullScreen: $("#fullScreen").val(),
			minFullScreenHeight: parseInt($("#minFullScreenHeight").val()),
			fullScreenOffsetContainer: $("#fullScreenOffsetContainer").val(),
			fullScreenOffset: parseInt($("#fullScreenOffset").val()),
			dottedOverlay: $("#dottedOverlay").val(),
			DisplayRandom: $("#DisplayRandom").val(),
			forceFullWidth: $("#forceFullWidth").val(),
			spinner: $("#spinner").val(),
			swipe_treshold: parseInt($("#swipe_treshold").val()),
			swipe_min_touches: parseInt($("#swipe_min_touches").val()),
			drag_block_vertical: $("#drag_block_vertical").val() == "on" ? "true": "false",
			parallax: $("#Parallax").val(),
			parallaxLevels: $("#parallaxLevels").val(),
			parallaxBgFreeze: $("#parallaxBgFreeze").val(),
			parallaxOpacity: $("#parallaxOpacity").val(),
			parallaxDisableOnMobile: $("#parallaxDisableOnMobile").val(),
			panZoomDisableOnMobile: $("#panZoomDisableOnMobile").val(),
			simplifyAll: $("#simplifyAll").val(),
			minHeight: parseInt($("#minHeight").val())
		}
	}
	var slideoption = function(id) {
		sop = {
			id: $("#slide-title" + id).attr("data-id"),
			sort: $("#slide-title" + id).attr("data-sort"),

			display_desktop:$("#display_desktop" + id).val(),
			display_tablet:$("#display_tablet" + id).val(),
			display_phone:$("#display_phone" + id).val(),

			temporary: id,
			transition: String($("#Transition" + id).val()),
			randomtransition: $("#RandomTransition" + id).val(),
			slotamount: $("#SlotAmount" + id).val(),
			masterspeed: $("#MasterSpeed" + id).val(),
			delay: $("#SlideDelay" + id).val(),
			link: $("#LinkURL" + id).val(),
			target: $("#Target" + id).val(),
			slideindex: $("#SlideIndex" + id).val(),
			thumb: $("#Thumbnail" + id).val(),
			title: $("#Title" + id).val()
		}
	}
	var imageoption = function(id) {
		iop = {
			bgcolor: $("#bgcolor" + id).val(),
			lazyload: $("#lazyload" + id).val(),
			bgrepeat: $("#bgrepeat" + id).val(),
			bgfit: $("#bgfit" + id).val(),
			bgfitother: $("#bgfitother" + id).val(),
			bgfitend: $("#bgfitend" + id).val(),
			bgposition: $("#bgposition" + id).val(),
			bgpositionend: $("#bgpositionend" + id).val(),
			imgalt: $("#imgalt" + id).val(),
			kenburns: $("#kenburns" + id).val(),
			duration: $("#duration" + id).val(),
			ease: $("#ease" + id).val(),
			bgimg: $("#bgimg" + id).val()
		}
	}
	var mediaoption = function(id) {
		mop = {
			videotype: $("#video" + id).val(),
			autoplay: $("#autoplay" + id).val() == "on" ? "true": "false",
			autoplayonlyfirsttime: $("#autoplayonlyfirsttime" + id).val() == "on" ? "true": "false",
			nextslideatend: $("#nextslideatend" + id).val() == "on" ? "true": "false",
			videoposter: $("#videoposter" + id).val(),
			forcecover: $("#forcecover" + id).val() == "on" ? "1": "0",
			forcerewind: $("#ForceRewind" + id).val(),
			volume: $("#volume" + id).val() == "on" ? "mute": "none",
			videowidth: $("#videowidth" + id).val(),
			videoheight: $("#videoheight" + id).val(),
			aspectratio: $("#aspectratio" + id).val(),
			videopreload: $("#videopreload" + id).val(),
			videomp4: $("#videomp4" + id).val(),
			videowebm: $("#videowebm" + id).val(),
			videoogv: $("#videoogv" + id).val(),
			ytid: $("#ytid" + id).val(),
			vimeoid: $("#vimeoid" + id).val(),
			videocontrols: $("#videocontrols" + id).val() == "on" ? "controls": " ",
			videoattributes: $("#videoattributes" + id).val(),
			videoloop: $("#videoloop" + id).val(),
			speed: $("#sbgspeed" + id).val(),
			start: $("#sbgstart" + id).val(),
			easing: $("#sbgease" + id).val(),
			endspeed: $("#sbgendspeed" + id).val(),
			endeasing: $("#sbgendeasing" + id).val(),
			incoming: $("#incoming" + id).val(),
			outgoing: $("#outgoing" + id).val()
		}
	}
	var typeoption = function(id) {
		typeop = {
			type: $("#slidetype_nav" + id).find("li.active").data("type")
		}
	}
	var contentoption = function(s, l) {
		cop = [{
			id: $("#panel" + s + "_" + l).find("li").attr("data-id"),
			currentid: $("#panel" + s + "_" + l).find("li").attr("data-new"),
			sort: $("#panel" + s + "_" + l).find("li").data("sort"),
			temporary: s + "_" + l,
			type: $("#layercontent_tab" + s + "_" + l).find("li.active").data("type"),
			vtype: $("#layervideo" + s + "_" + l).find("li.active").data("type"),
			itemname: $("#Item" + s + "_" + l).val(),
			displaydesktop: $("#display_desktop_" + s + "_" + l).val(),
			displaytablet: $("#display_tablet_" + s + "_" + l).val(),
			displayphone: $("#display_phone_" + s + "_" + l).val(),
	
		},
		{
			contentimg: $("#contentimg" + s + "_" + l).val()
		},
		{
			contenttext: $("#contenttext" + s + "_" + l).val(),
			contentcode: $("#contentcode" + s + "_" + l).find("span.active").data("code")
		},
		{
			youtube: $("#ytb" + s + "_" + l).val(),
			autoplay: $("#ytbautoplay" + s + "_" + l).val() == "on" ? "true": "false",
			autoplayonlyfirsttime: $("#ytbautoplayoft" + s + "_" + l).val() == "on" ? "true": "false",
			nextslideatend: $("#ytbnext" + s + "_" + l).val() == "on" ? "true": "false",
			videoattributes: $("#ytbattributes" + s + "_" + l).val(),
			videocontrols: $("#ytbcontrols" + s + "_" + l).val() == "on" ? "controls": " ",
			videowidth: $("#ytbwidth" + s + "_" + l).val(),
			videoheight: $("#ytbheight" + s + "_" + l).val()
		},
		{
			vimeo: $("#vimeo" + s + "_" + l).val(),
			autoplay: $("#vimeoautoplay" + s + "_" + l).val() == "on" ? "true": "false",
			autoplayonlyfirsttime: $("#vimeoautoplayoft" + s + "_" + l).val() == "on" ? "true": "false",
			nextslideatend: $("#vimeonfae" + s + "_" + l).val() == "on" ? "true": "false",
			videoattributes: $("#vimeoattributes" + s + "_" + l).val(),
			videocontrols: $("#vimeocontrols" + s + "_" + l).val() == "on" ? "controls": " ",
			videowidth: $("#vimeowidth" + s + "_" + l).val(),
			videoheight: $("#vimeoheight" + s + "_" + l).val()
		},
		{
			mp4: $("#html5mp4" + s + "_" + l).val(),
			webm: $("#html5webm" + s + "_" + l).val(),
			ogv: $("#html5ogg" + s + "_" + l).val(),
			autoplay: $("#html5autoplay" + s + "_" + l).val() == "on" ? "true": "false",
			videoposter: $("#html5poster" + s + "_" + l).val(),
			autoplayonlyfirsttime: $("#html5autoplayft" + s + "_" + l).val() == "on" ? "true": "false",
			nextslideatend: $("#html5nsae" + s + "_" + l).val() == "on" ? "true": "false",
			videocontrols: $("#html5controls" + s + "_" + l).val() == "on" ? "controls": " ",
			forcecover: $("#html5forcecover" + s + "_" + l).val(),
			forcerewind: $("#html5forcerewind" + s + "_" + l).val(),
			aspectratio: $("#html5aspectratio" + s + "_" + l).val(),
			videopreload: $("#html5preload" + s + "_" + l).val(),
			volume: $("#html5volume" + s + "_" + l).val(),
			videowidth: $("#html5width" + s + "_" + l).val(),
			videoheight: $("#html5height" + s + "_" + l).val()
		}]
	}
	var datasettings = function(s, l) {
		dsgop = {
			x: $("#datax" + s + "_" + l).val(),
			y: $("#datay" + s + "_" + l).val(),
			hoffset: $("#datahoffset" + s + "_" + l).val(),
			voffset: $("#datavoffset" + s + "_" + l).val(),
			speed: $("#dataspeed" + s + "_" + l).val(),
			splitin: $("#datasplitin" + s + "_" + l).val(),
			elementdelay: $("#dataelementdelay" + s + "_" + l).val(),
			splitout: $("#datasplitout" + s + "_" + l).val(),
			endelementdelay: $("#dataendelementdelay" + s + "_" + l).val(),
			start: $("#datastart" + s + "_" + l).val(),
			easing: $("#dataeasing" + s + "_" + l).val(),
			endspeed: $("#dataendspeed" + s + "_" + l).val(),
			end: $("#dataend" + s + "_" + l).val(),
			endeasing: $("#dataendeasing" + s + "_" + l).val(),
		}
	}
	var othersettings = function(s, l) {
		othop = {
			stylingcaptions: $("#stylingcaptions" + s + "_" + l).val(),
			parallaxsettings: $("#parallaxsettings" + s + "_" + l).val(),
			scrollbelowslider: $("#scrollbelowslider" + s + "_" + l).val(),
			scrolloffset: $("#scrolloffset" + s + "_" + l).val(),
			resizeme: $("#resizeme" + s + "_" + l).val(),
			withoutcorner: $("#withoutcorner" + s + "_" + l).val(),
			loopanimations: $("#loopanimations" + s + "_" + l).val(),
		}
	}
	var transitionoption = function(s, l) {
		tnop = [{
			x: $("#TransitionX" + s + "_" + l).val(),
			y: $("#TransitionY" + s + "_" + l).val(),
			z: $("#TransitionZ" + s + "_" + l).val(),
			rotationX: $("#RotationX" + s + "_" + l).val(),
			rotationY: $("#RotationY" + s + "_" + l).val(),
			rotationZ: $("#RotationZ" + s + "_" + l).val(),
			scaleX: $("#ScaleX" + s + "_" + l).val() * 0.01,
			scaleY: $("#ScaleY" + s + "_" + l).val() * 0.01,
			skewX: $("#SkewX" + s + "_" + l).val(),
			skewY: $("#SkewY" + s + "_" + l).val(),
			opacity: $("#Opacity" + s + "_" + l).val() * 0.01,
			perspective: $("#Perspective" + s + "_" + l).val(),
			originX: $("#OriginX" + s + "_" + l).val() + "%",
			originY: $("#OriginY" + s + "_" + l).val() + "%",
			Classes: $("#Classes" + s + "_" + l).val(),
		},
		{
			x: $("#OutTransitionX" + s + "_" + l).val(),
			y: $("#OutTransitionY" + s + "_" + l).val(),
			z: $("#OutTransitionZ" + s + "_" + l).val(),
			rotationX: $("#OutRotationX" + s + "_" + l).val(),
			rotationY: $("#OutRotationY" + s + "_" + l).val(),
			rotationZ: $("#OutRotationZ" + s + "_" + l).val(),
			scaleX: $("#OutScaleX" + s + "_" + l).val() * 0.01,
			scaleY: $("#OutScaleY" + s + "_" + l).val() * 0.01,
			skewX: $("#OutSkewX" + s + "_" + l).val(),
			skewY: $("#OutSkewY" + s + "_" + l).val(),
			opacity: $("#OutOpacity" + s + "_" + l).val() * 0.01,
			perspective: $("#OutPerspective" + s + "_" + l).val(),
			originX: $("#OutOriginX" + s + "_" + l).val() + "%",
			originY: $("#OutOriginY" + s + "_" + l).val() + "%",
			Classes: $("#OutClasses" + s + "_" + l).val()
		}]
	}
	var linkoption = function(s, l) {
		linkop = {
			Url: $("#Url" + s + "_" + l).val(),
			Target: $("#Target" + s + "_" + l).val(),
		}
	}
	var styleoption = function(s, l) {
		styleop = {
			width: $("#Width" + s + "_" + l).val(),
			height: $("#Height" + s + "_" + l).val(),
			top: $("#Top" + s + "_" + l).val(),
			left: $("#Left" + s + "_" + l).val(),
			padding_top: $("#PaddingTop" + s + "_" + l).val(),
			padding_right: $("#PaddingRight" + s + "_" + l).val(),
			padding_bottom: $("#PaddingBottom" + s + "_" + l).val(),
			padding_left: $("#PaddingLeft" + s + "_" + l).val(),
			border_top: $("#BorderTop" + s + "_" + l).val(),
			border_right: $("#BorderRight" + s + "_" + l).val(),
			border_bottom: $("#BorderBottom" + s + "_" + l).val(),
			border_left: $("#BorderLeft" + s + "_" + l).val(),
			font_family: $("#FontFamily" + s + "_" + l).val(),
			font_size: $("#FontSize" + s + "_" + l).val(),
			line_height: $("#Line-height" + s + "_" + l).val(),
			color: $("#Color" + s + "_" + l).val(),
			background_color: $("#BackgroundColor" + s + "_" + l).val(),
			border_radius: $("#Roundedcorners" + s + "_" + l).val(),
			word_wrap: $("#Word-wrap" + s + "_" + l).val(),
			CustomCSS: $("#CustomCSS" + s + "_" + l).val()
		}
	}
	var attributesoption = function(s, l) {
		aop = {
			id: $("#Id" + s + "_" + l).val(),
			class: $("#Class" + s + "_" + l).val(),
			title: $("#Title" + s + "_" + l).val(),
			alt: $("#Alt" + s + "_" + l).val(),
			rel: $("#Rel" + s + "_" + l).val(),
		}
	}
	var slidesettings = function() {
		settings();
		$('.slidepreview').revolution(eval(stg));
	}
	var Previewbox = function() {
		var btn = $("#previewfolding"),
		con = $("#previewcontent"),
		hig = $("#previewhig"),
		tie = $("#Preview > .slide-title");
		btn.on("click",
		function() {
			con.toggle(500);
			hig.toggle(500);
			tie.toggleClass("drag");
			btn.toggleClass("fa-arrow-down");
			if ($(this).hasClass("fa-arrow-down")) {
				$.cookie('cookiepreviewfolding', "show");
				refreshscript(slideTab.find("li.active .ico-close").attr("id"));
			} else {
				$("#previewcontent .tp-banner-container").remove();
				$.cookie('cookiepreviewfolding', "hide")
			}
		})
		 if ($.cookie('cookiepreviewfolding') && $.cookie('cookiepreviewfolding') == "show") {
			con.show();
			hig.show();
			tie.toggleClass("drag");
			btn.addClass("fa-arrow-down");
			
		} else {
			con.hide();
			hig.hide();
			btn.removeClass("fa-arrow-down");
			
		}
		if ($.cookie('cookiepreviewcontent') && parseInt($.cookie('cookiepreviewcontent')) < $(window).height()) {
			con.css({
				"height": $.cookie('cookiepreviewcontent')
			})
			 hig.css({
				"height": $.cookie('cookiepreviewcontent')
			})
		} else {
			con.css({
				"height": $(window).height() / 2
			})
			 hig.css({
				"height": $(window).height() / 2
			})
		}
		tie.on("mousedown",
		function(event) {
			var e = $(this);
			if (con.css("display") == "block") {
				e.css("cursor", "row-resize");
				e.addClass("selected");
 				Y = event.clientY;
				var h = con.height();
				e.on("mousemove",
				function(event) {
					con.css("height", h + (Y - event.clientY));
				})
			}
		}).on("mouseup",
		function() {
			var e = $(this);
			e.off("mousemove");
			$.cookie('cookiepreviewcontent', con.css("height"));
			hig.css("height", con.css("height"));
			e.css("cursor", "inherit");
			e.removeClass("selected");
		})
	}
	$("#previewrefresh").on("click",function() {
		refreshscript(slideTab.find("li.active .ico-close").attr("id"));
	})
	var mark;
	var modalImg = function(id, op) { 
		if (op) {
			var box = $("#panel" + id + "_" + op).find(".fileupimg")
		} else {
			var box = $("#slide_tab1_example" + id).find(".fileupimg")
		}
		box.each(function() {
			var box = $(this),
			text = box.find(".imgtext"),
			timg = box.find(".slideimgs img");
			remove = box.find(".removeimg");
			box.find(".imgdemo").click(function() {
                $("#AddMedia_Iframe").attr("src", $("#AddMedia_Iframe").attr("data-src"));
				mark=$(this);
			});
			var thumbnail = function() {
				timg.hide().attr("src", text.val()?text.val():" ");
				 timg.error(function(e) {
					timg.attr("src","Resource/images/thumbnailerror.jpg")
					 if (text.val().replace(/[ ]/g, "").length == 0) {
						timg.attr("src", "Resource/images/thumbnail.jpg")
					}
				})
				timg.show();
			}
			thumbnail();
			if (box.hasClass("fileupimg2")) {
				text.on("focus",
				function() {
					text.css("opacity", "1");
				})
				 text.on("blur",
				function() {
					text.attr("style", "");
				})
			}
			text.on("blur",
			function() {
				thumbnail()
			})
			 remove.on("click",
			function() {
				text.val("");
				thumbnail();
				return false;
			})
			imgurl = function(i){
				mark.parents(".fileupimg").find(".imgtext").val(i);
				mark.parents(".fileupimg").find(".slideimgs img").attr("src", i);
				$(".modal-header .close").click();
			
			}
		});
	}
	
	$("#ajax-modal .modal-body .tp-caption").on("click",function(){
		$(ShowroomStyling).val($(this).text())
		$(".modal-header .close").click();
	})
	
	if(slideTab.find("li").length!=0){
		loadslider($(this).find(".ico-close").attr("id"));
	}else{
		contentnot();
	}

});


