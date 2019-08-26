<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Setting_ManagerSliders.ascx.cs" Inherits="DNNGo.Modules.LayerGallery.Setting_ManagerSliders" %>
 
<asp:Literal ID="liFormatData" runat="server"></asp:Literal>

<div class="row">
  <div class="col-sm-12"> 
    <!-- start: PAGE TITLE & BREADCRUMB -->
    
    <div class="page-header">
      <h1><i class="fa clip-list-4"></i> <%=ViewResourceText("Header_Title", "3D Sliders")%> </h1>
      <div id="alertbox"></div>
    </div>
    <!-- end: PAGE TITLE & BREADCRUMB --> 
  </div>
</div>
<!-- end: PAGE HEADER -->

<div class="row">
  <div class="col-sm-12">
    <ul class="nav nav-tabs tab-padding tab-space-3 tab-blue">
      <li> <a href="#panel_tab1_example1" data-toggle="tab">Slider Settings </a> </li>
      <li class="active"> <a href="#panel_tab1_example2" id="slidelist" data-toggle="tab">Slides</a>
      </li>
    </ul>
    <div class="tab-content">
      <div id="Loading"></div>
      <div class="tab-pane in " id="panel_tab1_example1">
        <div class="panel panel-default small-bottom" id="SliderSettings">
          <div class="panel-heading"> <i class="fa fa-external-link-square"></i> Slider Settings </div>
          <div class="panel-collapse collapse in">
            <div class="panel-body">
              <div class="tabbable tabs-left tabs-left2">
                <ul class="nav nav-tabs tab-green">
                  <li class="active"> <a href="#panel_tab2_example1" data-toggle="tab"><i class="clip-cogs"></i>&nbsp;  Global Settings </a> </li>
                  <li> <a href="#panel_tab2_example2" data-toggle="tab"><i class="clip-grid-2"></i>&nbsp;  Navigation Settings </a> </li>
                  <li> <a href="#panel_tab2_example3" data-toggle="tab"><i class="clip-loop"></i>&nbsp;  Loops </a> </li>
                  <li> <a href="#panel_tab2_example4" data-toggle="tab"><i class="clip-mobile"></i>&nbsp;  Mobile Visibility </a> </li>
                  <li> <a href="#panel_tab2_example5" data-toggle="tab"><i class="clip-screen"></i>&nbsp;  Layout Styles </a> </li>
                  <li> <a href="#panel_tab2_example6" data-toggle="tab"><i class="clip-stack-2"></i>&nbsp;  Parallax Settings </a> </li>
                  <li> <a href="#panel_tab2_example7" data-toggle="tab"><i class="clip-target"></i>&nbsp;  Pan Zoom Settings </a> </li>
                  <li> <a href="#panel_tab2_example8" data-toggle="tab"><i class="clip-upload-3"></i>&nbsp;  Further Options </a> </li>
                </ul>
                <div class="tab-content">
                  <div class="tab-pane active" id="panel_tab2_example1">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Global Settings</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Delay</td>
                          <td width="15%" class=""><input type="text" class="form-control form-validation" data-validation="plusint" value="9000" id="delay"></td>
                          <td class="tips">The time one slide stays on the screen in Milliseconds. Global Setting. You can set per Slide extra local delay time via the data-delay in the &lt;li&gt; element (Default: 9000)</td>
                        </tr>
                        <tr>
                          <td align="right">Start Width</td>
                          <td><input type="text" class="form-control form-validation" value="960" id="startwidth" data-validation="plusint"></td>
                          <td class="tips">This Height of the Grid where the Captions are displayed in Pixel. This Height is the Max height of Slider in Fullwidth Layout and in Responsive Layout.  In Fullscreen Layout the Gird will be centered Vertically in case the Slider is higher then this value.</td>
                        </tr>
                        <tr>
                          <td align="right">Start Height</td>
                          <td><input type="text" class="form-control form-validation" value="500" id="startheight" data-validation="plusint"></td>
                          <td class="tips">This Height of the Grid where the Captions are displayed in Pixel. This Width is the Max Width of Slider in Responsive Layout.  In Fullscreen and in FullWidth Layout the Gird will be centered Horizontally in case the Slider is wider then this value.</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example2">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Navigation Settings</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Keyboard Navigation</td>
                          <td width="15%"><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked id="keyboardNavigation">
                            </div></td>
                          <td class="tips">Allows to use the Left/Right Arrow for Keyboard navigation when Slider is in Focus.</td>
                        </tr>
                        <tr>
                          <td align="right">On Hover Stop</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked  id="onHoverStop">
                            </div></td>
                          <td class="tips">Stop the Timer if mouse is hovering the Slider.  Caption animations are not stopped !! They will just play to the end.</td>
                        </tr>
                        <tr>
                          <td align="right">Thumbs Width</td>
                          <td><input type="text" class="form-control form-validation" value="100" id="thumbWidth" data-validation="plusint"></td>
                          <td class="tips">The width and height of the thumbs in pixel. Thumbs are not responsive from basic. For Responsive Thumbs you will need to create media qury based thumb sizes. </td>
                        </tr>
                        <tr>
                          <td align="right">Thumbs Height</td>
                          <td><input type="text" class="form-control form-validation" value="50" id="thumbHeight" data-validation="plusint"></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">Thumbs Amount</td>
                          <td><input type="text" class="form-control form-validation" value="3" id="thumbAmount" data-validation="plusint"></td>
                          <td class="tips">The Amount of visible Thumbs in the same time.  The rest of the thumbs are only visible on hover, or at slide change.</td>
                        </tr>
                        <tr>
                          <td align="right">Hide Thumbs</td>
                          <td><input type="text" class="form-control form-validation" value="200" id="hideThumbs" data-validation="plusint"></td>
                          <td class="tips">0 - Never hide Thumbs.  1000- 100000 (ms) hide thumbs and navigation arrows, bullets after the predefined ms  (1000ms == 1 sec,  1500 == 1,5 sec etc..)</td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Type</td>
                          <td><select class="form-control" id="navigationType">
                              <option selected>bullet</option>
                              <option>thumb</option>
                              <option>none</option>
                            </select></td>
                          <td class="tips"> Display type of the "bullet/thumbnail" bar </td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Arrows</td>
                          <td><select class="form-control" id="navigationArrows">
                              <option selected>solo</option>
                              <option>nextto</option>
                              <option>none</option>
                            </select></td>
                          <td class="tips">Display position of the Navigation Arrows </td>
                        </tr>
                        <tr>
                          <td align="right">Navigation In Grid</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="navigationInGrid">
                            </div></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Style</td>
                          <td><select class="form-control"  id="navigationStyle">
                              <option >round</option>
                              <option>square</option>
                              <option>navbar</option>
                              <option>round-old</option>
                              <option>square-old</option>
                              <option>preview1</option>
                              <option>preview2</option>
                              <option>preview3</option>
                              <option>preview4</option>
                            </select></td>
                          <td class="tips"> Look of the navigation bullets if navigationType "bullet" selected. </td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Align(H)</td>
                          <td><select class="form-control"  id="navigationHAlign">
                              <option>left</option>
                              <option selected>center</option>
                              <option>right</option>
                            </select></td>
                          <td class="tips">Vertical and Horizontal Align of the Navigation bullets / thumbs (depending on which Style has been selected). </td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Align(V)</td>
                          <td><select class="form-control" id="navigationVAlign">
                              <option>top</option>
                              <option >center</option>
                              <option selected>bottom</option>
                            </select></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Offset(H)</td>
                          <td><input type="text" class="form-control form-validation" value="0"  id="navigationHOffset" data-validation="int"></td>
                          <td class="tips">The Offset position of the navigation depending on the aligned position.  from -1000 to +1000 any value in px.   i.e. -30 </td>
                        </tr>
                        <tr>
                          <td align="right">Navigation Offset(V)</td>
                          <td><input type="text" class="form-control form-validation" value="20"  id="navigationVOffset" data-validation="int"></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowLeft Align(H)</td>
                          <td><select class="form-control" id="soloArrowLeftHalign">
                              <option selected>left</option>
                              <option>center</option>
                              <option>right</option>
                            </select></td>
                          <td class="tips">Vertical and Horizontal Align of the Navigation Arrows (left and right independent!) Possible values navigationHAlign: "left","center","right"  and naigationVAlign: "top","center","bottom"</td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowLeft Align(V)</td>
                          <td><select class="form-control" id="soloArrowLeftValign">
                              <option>top</option>
                              <option selected>center</option>
                              <option =>bottom</option>
                            </select></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowLeft Offset(H)</td>
                          <td><input type="text" class="form-control form-validation" value="20" id="soloArrowLeftHOffset" data-validation="int"></td>
                          <td class="tips">The Offset position of the navigation depending on the aligned position.  from -1000 to +1000 any value in px.   i.e. -30   Each Arrow independent</td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowLeft Offset(V)</td>
                          <td><input type="text" class="form-control form-validation" value="0" id="soloArrowLeftVOffset" data-validation="int"></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowRight Align(H)</td>
                          <td><select class="form-control" id="soloArrowRightHalign">
                              <option>left</option>
                              <option >center</option>
                              <option selected>right</option>
                            </select></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowRight Align(V)</td>
                          <td><select class="form-control" id="soloArrowRightValign">
                              <option>top</option>
                              <option selected>center</option>
                              <option >bottom</option>
                            </select></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowRight Offset(H)</td>
                          <td><input type="text" class="form-control form-validation" value="20"  id="soloArrowRightHOffset" data-validation="int"></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">SoloArrowRight Offset(V)</td>
                          <td><input type="text" class="form-control form-validation" value="0"  id="soloArrowRightVOffset" data-validation="int"></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">Touch Enabled</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked  id="touchenabled">
                            </div></td>
                          <td class="tips">Enable Swipe Function on touch devices</td>
                        </tr>
                        <tr>
                          <td align="right">Swipe Treshold</td>
                          <td><input type="text" class="form-control form-validation" value="75" id="swipe_treshold" data-validation="plusint"></td>
                          <td class="tips">The number of pixels that the user must move their finger by before it is considered a swipe.</td>
                        </tr>
                        <tr>
                          <td align="right">Swipe Min Touches</td>
                          <td><input type="text" class="form-control form-validation" value="1" id="swipe_min_touches" data-validation="plusint"></td>
                          <td class="tips">Min Finger (touch) used for swipe</td>
                        </tr>
                        <tr>
                          <td align="right">Drag Block Vertical</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked  id="drag_block_vertical">
                            </div></td>
                          <td class="tips">Prevent Vertical Scroll during Swipe</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example3">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Loops</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Start With Slide</td>
                          <td width="15%"><input type="text" class="form-control form-validation" value="0" id="startWithSlide" data-validation="plusint"></td>
                          <td class="tips">Start with a Predefined Slide (index of the slide) </td>
                        </tr>
                        <tr>
                          <td align="right">Stop At Slide</td>
                          <td><input type="text" class="form-control form-validation" value="-1" id="stopAtSlide" data-validation="int"></td>
                          <td class="tips"> Stop Timer if Slide "x" has been Reached. If stopAfterLoops set to 0, then it stops already in the first Loop at slide X which Defined. -1 means do not stop at any slide. stopAfterLoops has no sinn in this case. </td>
                        </tr>
                        <tr>
                          <td align="right">Stop After Loops</td>
                          <td><input type="text" class="form-control form-validation" value="-1" id="stopAfterLoops" data-validation="int"></td>
                          <td class="tips">Stop Timer if All slides has been played "x" times. IT will stop at THe slide which is defined via stopAtSlide:x, if set to -1 Slide never stop automatic</td>
                        </tr>
                        <tr>
                          <td align="right">shuffle</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="shuffle">
                            </div></td>
                          <td class="tips"></td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example4">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Mobile Visibility </th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Hide Caption At Limit</td>
                          <td width="15%"><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="hideCaptionAtLimit"></td>
                          <td class="tips">It Defines if a caption should be shown under a Screen Resolution ( Basod on The Width of Browser)</td>
                        </tr>
                        <tr>
                          <td align="right">Hide All Caption At Llmit</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="hideAllCaptionAtLimit"></td>
                          <td class="tips">Hide all The Captions if Width of Browser is less then this value</td>
                        </tr>
                        <tr>
                          <td align="right">Hide Slider At Limit</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="hideSliderAtLimit"></td>
                          <td class="tips">Hide the whole slider, and stop also functions if Width of Browser is less than this value</td>
                        </tr>
                        <tr>
                          <td align="right">Hide Nav Delay On Mobile</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="1500" id="hideNavDelayOnMobile"></td>
                          <td class="tips">Hide all navigation after a while on Mobile (touch and release events based)</td>
                        </tr>
                        <tr>
                          <td align="right">Hide Thumbs On Mobile</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="hideThumbsOnMobile">
                            </div></td>
                          <td class="tips">If set to "on", Thumbs are not shown on Mobile Devices</td>
                        </tr>
                        
                        <!--<tr>
                          <td align="right">Hide Bullets On Mobile</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="hideBulletsOnMobile">
                            </div></td>
                          <td class="tips">If set to "on", Bullets are not shown on Mobile Devices</td>
                        </tr>
                        <tr>
                          <td align="right">Hide Arrows On Mobile</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox"  id="hideArrowsOnMobile">
                            </div></td>
                          <td class="tips">If set to "on", Arrows are not shown on Mobile Devices</td>
                        </tr>-->
                        
                        <tr>
                          <td align="right">Hide Thumbs UnderResoluition</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="hideThumbsUnderResoluition"></td>
                          <td class="tips">Possible Values: 0 - 1900 - defines under which resolution the Thumbs should be hidden. (only if hideThumbonMobile set to off)</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example5">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Layout Styles</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Spinner</td>
                          <td width="15%"><select class="form-control" id="spinner">
                              <option>spinner0</option>
                              <option>spinner1</option>
                              <option>spinner2</option>
                              <option>spinner3</option>
                              <option>spinner4</option>
                              <option>spinner5</option>
                            </select></td>
                          <td class="tips">Possible Values: "spinner1" , "spinner2", "spinner3" , "spinner4", "spinner5" - The Layout of Loader. If not defined, it will use the basic spinner.</td>
                        </tr>
                        <tr>
                          <td align="right">Hide TimerBar</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="hideTimerBar">
                            </div></td>
                          <td class="tips">It will hide or show the banner timer</td>
                        </tr>
                        <tr>
                          <td align="right">Full Width</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="fullWidth">
                            </div></td>
                          <td class="tips">Defines if the fullwidth/autoresponsive mode is activated</td>
                        </tr>
                        <tr>
                          <td align="right">Auto Height</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="autoHeight">
                            </div></td>
                          <td class="tips">Defines if in fullwidth mode the height of the Slider proportional always can grow. If it is set to "off" the max height is == startheight</td>
                        </tr>
                        <tr>
                          <td align="right">Min Height</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="minHeight"></td>
                          <td class="tips">Possible Values: 0 - 9999 - defines the min height of the Slider. The Slider container height will never be smaller than this value. The Content container is still shrinks linear to the browser width and original width of Container, and will be centered vertically in the Slider</td>
                        </tr>
                        <tr>
                          <td align="right">Full Screen Align Force</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="fullScreenAlignForce">
                            </div></td>
                          <td class="tips">Allowed only in FullScreen Mode. It lets the Caption Grid to be the full screen, means all position should happen with aligns and offsets. This allow you to use the faresst corner of the slider to present any caption there.</td>
                        </tr>
                        <tr>
                          <td align="right">Force Full Width</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="forceFullWidth">
                            </div></td>
                          <td class="tips">Force the FullWidth Size even if the slider embeded in a boxed container. It can provide higher Performance usage on CPU. Try first set it to "off" and use fullwidth container instead of using this option</td>
                        </tr>
                        <tr>
                          <td align="right">Full Screen</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="fullScreen">
                            </div></td>
                          <td class="tips">defines if the fullscreen mode is activated</td>
                        </tr>
                        <tr>
                          <td align="right">Min Full Screen Height</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="minFullScreenHeight"></td>
                          <td class="tips">The Minimum FullScreen Height</td>
                        </tr>
                        <tr>
                          <td align="right">Full Screen OffsetContainer</td>
                          <td><input type="text" class="form-control form-validation" data-validation="string" value="" id="fullScreenOffsetContainer"></td>
                          <td class="tips">The value is a Container ID or Class i.e. "#topheader"  - The Height of Fullheight will be increased with this Container height !</td>
                        </tr>
                        <tr>
                          <td align="right">Full Screen Offset</td>
                          <td><input type="text" class="form-control form-validation" data-validation="unitint" value="0" id="fullScreenOffset"></td>
                          <td class="tips">A px or % value which will be added/reduced of the Full Height Container calculation.</td>
                        </tr>
                        <tr>
                          <td align="right">Shadow</td>
                          <td><input type="text" class="form-control form-validation" data-validation="plusint" value="0" id="shadow"></td>
                          <td class="tips">Possible values: 0,1,2,3  (0 == no Shadow, 1,2,3 - Different Shadow Types)</td>
                        </tr>
                          </tr>
                        
                        <tr>
                          <td align="right">Dotted Overlay</td>
                          <td><select class="form-control" id="dottedOverlay">
                              <option selected>none</option>
                              <option>twoxtwo</option>
                              <option>threexthree</option>
                              <option>twoxtwowhite</option>
                              <option>threexthreewhite</option>
                            </select></td>
                          <td class="tips">Creates a Dotted Overlay for the Background images extra. Best use for FullScreen / fullwidth sliders, where images are too pixaleted.</td>
                        </tr>
                        
                        <tr>
                          <td align="right">Display In Random</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="DisplayRandom">
                            </div></td>
                          <td class="tips"></td>
                        </tr>
                      
                        
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example6">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Parallax Settings</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">parallax</td>
                          <td width="15%"><select class="form-control"  id="Parallax">
                              <option selected>off</option>
                              <option>mouse</option>
                              <option>scroll</option>
                            </select></td>
                          <td class="tips"> How the Parallax should act. On Mouse Hover movements and Tilts on Mobile Devices, or on Scroll (scroll is disabled on Mobiles !)</td>
                        </tr>
                        <tr>
                          <td align="right">parallaxBgFreeze</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" id="parallaxBgFreeze">
                            </div></td>
                          <td class="tips">If it is off, the Main slide images will also move with Parallax Level 1 on Scroll.</td>
                        </tr>
                        <tr>
                          <td align="right">parallaxOpacity</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked  id="parallaxOpacity">
                            </div></td>
                          <td class="tips"></td>
                        </tr>
                        <tr>
                          <td align="right">Parallax Levels</td>
                          <td><input type="text" class="form-control form-validation" data-validation="string"  value="[10,7,4,3,2,5,4,3,2,1]" id="parallaxLevels"></td>
                          <td class="tips">An array which defines the Parallax depths (0 - 10). Depending on the value it will define the Strength of the Parallax offsets on mousemove or scroll. In Layers you can use the class like rs-parallaxlevel-1 for the different levels. If one tp-caption layer has rs-parallaxlevel-X (x 1-10) then it activates the Parallax movements on that layer.  
                            Available values: "none", "twoxtwo", "threexthree", "twoxtwowhite", "threexthreewhite" - Creates a Dotted Overlay for the Background images extra. Best use for FullScreen / fullwidth sliders, where images are too pixaleted.</td>
                        </tr>
                        <tr>
                          <td align="right">Parallax Disable On Mobile</td>
                          <td><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox"  id="parallaxDisableOnMobile">
                            </div></td>
                          <td class="tips">Turn on/ off Parallax Effect on Mobile Devices</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example7">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Pan Zoom Settings</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Pan Zoom Disable On Mobile</td>
                          <td width="15%"><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox"  id="panZoomDisableOnMobile">
                            </div></td>
                          <td class="tips">Turn on/ off Pan Zoom Effects on Mobile Devices </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                  <div class="tab-pane" id="panel_tab2_example8">
                    <table class="table table-title">
                      <thead>
                        <tr>
                          <th colspan="3" >Further Options</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr>
                          <td width="15%" align="right">Simplify All</td>
                          <td width="15%"><div class="make-switch" data-on="primary" data-off="info">
                              <input type="checkbox" checked id="simplifyAll">
                            </div></td>
                          <td class="tips">Set all Animation on older Browsers like IE8 and iOS4 Safari to Fade, without splitting letters to save performance</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="tab-pane active tab-style1" id="panel_tab1_example2">
        <div class="tabbable">
          <div  class="tabbable-title">
            <ul class="nav nav-tabs tab-bricky slide-nav-ico" id="slide-tab-title">
            </ul>
            <i class="clip-plus-circle-2 ico-add" id="addnewslide"></i> </div>
          <div class="tab-content" id="slide-tab-content" > </div>
        </div>
      </div>
    </div>
  </div>
</div>
<div class="row">
  <div class="col-sm-12">
    <div class="form-group">
      <p>&nbsp;</p>
      <input type="submit" lang="Submit" class="btn btn-primary" value="Update" id="submit" >
      &nbsp;
      <input type="submit" class="btn btn-default" value="Cancel">
      &nbsp; </div>
    <br/>
    <br/>
  </div>
  <div id="Preview">
    <div class="slide-title">
      <i class="clip-eye "></i> Preview </div>
      <div class="previewico">
     <!-- <i class="clip-pictures tooltips" data-original-title="show to a single Sliders or all of the Sliders" id="previewpage" data-page="Single"></i>   -->
      <i class="fa fa-refresh tooltips" data-original-title="refresh Sliders" id="previewrefresh"></i> 
      <i class="fa fa-arrow-up" id="previewfolding"></i> 
     </div>

    <div class="slide-content" id="previewcontent">
      <div class="tp-banner-container">
        <div  class="tp-banner slidepreview">
          <ul class='box'>
          </ul>
        </div>
      </div>
    </div>
  </div>
</div>
<!-- end: PAGE CONTENT-->
<div id="previewhig"> </div>
<div id="ajaxParameters" 
PostDataUrl="<% =ViewAjaxUrl("post","")%>" 
GetDataUrl_Settings="<% =ViewAjaxUrl("get","Settings")%>" 
GetDataUrl_Sliders="<% =ViewAjaxUrl("get","Sliders")%>" 
DeleteDataUrl_Slider="<% =ViewAjaxUrl("delete_slider","")%>" 
DeleteDataUrl_Layer="<% =ViewAjaxUrl("delete_layer","")%>" 
LayersUrl="<%=ModulePath %>Resource/templates/Layers.html"
TemplateUrl="<%=ModulePath %>Resource/templates/template.html"
></div>

<%--<div id="ajaxParameters" 
PostDataUrl="<% =ViewAjaxUrl("post")%>" 
GetDataUrl="<%=ModulePath %>Resource/templates/data.html" 
LayersUrl="<%=ModulePath %>Resource/templates/Layers.html"
TemplateUrl="<%=ModulePath %>Resource/templates/template.html"
></div>--%>
<%--$("#ajaxParameters").attr("LayersUrl");--%>
<div id="AddMedia_Modal" class="modal fade" tabindex="-1" data-width="760" data-height="400" style="display: none;">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title"> <i class='fa fa-folder-open'></i> Add New Media</h4>
  </div>
  <div class="modal-body">
    <iframe id="AddMedia_Iframe" width="100%" height="100%" style="border-width: 0px;" data-src="<% =ViewIframeUrl()%>"></iframe>
  </div>
</div>

<div id="ajax-modal"  class="modal fade" tabindex="-1" data-width="760" data-height="400" style="display: none;" >
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title"> <i class='fa fa-folder-open'></i> Styling Captions </h4>
  </div>
  <div class="modal-body">
    <div class="tp-caption medium_grey">medium_grey</div>
    <div class="tp-caption small_text">small_text</div>
    <div class="tp-caption medium_text">medium_text</div>
    <div class="tp-caption large_text">large_text</div>
    <div class="tp-caption very_large_text">very_large_text</div>
    <div class="tp-caption very_big_white">very_big_white</div>
    <div class="tp-caption very_big_black">very_big_black</div>
    <div class="tp-caption modern_medium_fat">modern_medium_fat</div>
    <div class="tp-caption modern_medium_fat_white">modern_medium_fat_white</div>
    <div class="tp-caption modern_medium_light">modern_medium_light</div>
    <div class="tp-caption modern_big_bluebg">modern_big_bluebg</div>
    <div class="tp-caption modern_big_redbg">modern_big_redbg</div>
    <div class="tp-caption modern_small_text_dark">modern_small_text_dark</div>
    <div class="tp-caption boxshadow">boxshadow</div>
    <div class="tp-caption black">black</div>
    <div class="tp-caption thinheadline_dark">thinheadline_dark</div>
    <div class="tp-caption thintext_dark">thintext_dark</div>
    <div class="tp-caption largeblackbg">largeblackbg</div>
    <div class="tp-caption largepinkbg">largepinkbg</div>
    <div class="tp-caption largewhitebg">largewhitebg</div>
    <div class="tp-caption largegreenbg">largegreenbg</div>
    <div class="tp-caption excerpt">excerpt</div>
    <div class="tp-caption large_bold_grey">large_bold_grey</div>
    <div class="tp-caption medium_thin_grey">medium_thin_grey</div>
    <div class="tp-caption small_thin_grey">small_thin_grey</div>
    <div class="tp-caption lightgrey_divider">lightgrey_divider</div>
    <div class="tp-caption large_bold_darkblue">large_bold_darkblue</div>
    <div class="tp-caption medium_bg_darkblue">medium_bg_darkblue</div>
    <div class="tp-caption medium_bold_red">medium_bold_red</div>
    <div class="tp-caption medium_light_red">medium_light_red</div>
    <div class="tp-caption medium_bg_red">medium_bg_red</div>
    <div class="tp-caption medium_bold_orange">medium_bold_orange</div>
    <div class="tp-caption medium_bg_orange">medium_bg_orange</div>
    <div class="tp-caption grassfloor">grassfloor</div>
    <div class="tp-caption large_bold_white">large_bold_white</div>
    <div class="tp-caption medium_light_white">medium_light_white</div>
    <div class="tp-caption mediumlarge_light_white">mediumlarge_light_white</div>
    <div class="tp-caption mediumlarge_light_white_center">mediumlarge_light_white_center</div>
    <div class="tp-caption medium_bg_asbestos">medium_bg_asbestos</div>
    <div class="tp-caption medium_light_black">medium_light_black</div>
    <div class="tp-caption large_bold_black">large_bold_black</div>
    <div class="tp-caption mediumlarge_light_darkblue">mediumlarge_light_darkblue</div>
    <div class="tp-caption small_light_white">small_light_white</div>
    <div class="tp-caption roundedimage">roundedimage</div>
    <div class="tp-caption large_bg_black">large_bg_black</div>
    <div class="tp-caption mediumwhitebg">mediumwhitebg</div>
    <div class="tp-caption large_bold_white_25">large_bold_white_25</div>
    <div class="tp-caption medium_text_shadow">medium_text_shadow</div>
    <div class="tp-caption black_heavy_60">black_heavy_60</div>
    <div class="tp-caption white_heavy_40">white_heavy_40</div>
    <div class="tp-caption grey_heavy_72">grey_heavy_72</div>
    <div class="tp-caption grey_regular_18">grey_regular_18</div>
    <div class="tp-caption black_thin_34">black_thin_34</div>
    <div class="tp-caption arrowicon">arrowicon</div>
    <div class="tp-caption light_heavy_60">light_heavy_60</div>
    <div class="tp-caption black_bold_40">black_bold_40</div>
    <div class="tp-caption light_heavy_70">light_heavy_70</div>
    <div class="tp-caption black_heavy_70">black_heavy_70</div>
    <div class="tp-caption black_bold_bg_20">black_bold_bg_20</div>
    <div class="tp-caption greenbox30">greenbox30</div>
    <div class="tp-caption blue_heavy_60">blue_heavy_60</div>
    <div class="tp-caption green_bold_bg_20">green_bold_bg_20</div>
    <div class="tp-caption whitecircle_600px">whitecircle_600px</div>
    <div class="tp-caption fullrounded">fullrounded</div>
    <div class="tp-caption light_heavy_40">light_heavy_40</div>
    <div class="tp-caption white_thin_34">white_thin_34</div>
    <div class="tp-caption fullbg_gradient">fullbg_gradient</div>
    <div class="tp-caption light_medium_30">light_medium_30</div>
    <div class="tp-caption red_bold_bg_20">red_bold_bg_20</div>
    <div class="tp-caption blue_bold_bg_20">blue_bold_bg_20</div>
    <div class="tp-caption white_bold_bg_20">white_bold_bg_20</div>
    <div class="tp-caption white_heavy_70">white_heavy_70</div>
    <div class="tp-caption light_heavy_70_shadowed">light_heavy_70_shadowed</div>
    <div class="tp-caption light_medium_30_shadowed">light_medium_30_shadowed</div>
    <div class="tp-caption blackboxed_heavy">blackboxed_heavy</div>
    <div class="tp-caption bignumbers_white">bignumbers_white</div>
    <div class="tp-caption whiteline_long">whiteline_long</div>
    <div class="tp-caption light_medium_20_shadowed">light_medium_20_shadowed</div>
    <div class="tp-caption fullgradient_overlay">fullgradient_overlay</div>
    <div class="tp-caption light_medium_20">light_medium_20</div>
    <div class="tp-caption reddishbg_heavy_70">reddishbg_heavy_70</div>
    <div class="tp-caption borderbox_725x130">borderbox_725x130</div>
    <div class="tp-caption light_heavy_34">light_heavy_34</div>
    <div class="tp-caption black_thin_30">black_thin_30</div>
    <div class="tp-caption black_thin_whitebg_30">black_thin_whitebg_30</div>
    <div class="tp-caption white_heavy_60">white_heavy_60</div>
    <div class="tp-caption black_thin_blackbg_30">black_thin_blackbg_30</div>
    <div class="tp-caption light_thin_60">light_thin_60</div>
    <div class="tp-caption greenbgfull">greenbgfull</div>
    <div class="tp-caption bluebgfull">bluebgfull</div>
    <div class="tp-caption blackbgfull">blackbgfull</div>
    <div class="tp-caption wave_repeat1">wave_repeat1</div>
    <div class="tp-caption wavebg1">wavebg1</div>
    <div class="tp-caption wavebg2">wavebg2</div>
    <div class="tp-caption wavebg3">wavebg3</div>
    <div class="tp-caption wavebg4">wavebg4</div>
    <div class="tp-caption wavebg5">wavebg5</div>
    <div class="tp-caption greenishbg_heavy_70">greenishbg_heavy_70</div>
    <div class="tp-caption deepblue_sea">deepblue_sea</div>
  </div>
</div>