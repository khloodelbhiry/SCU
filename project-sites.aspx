<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-sites.aspx.cs" Inherits="project_sites" meta:resourcekey="PageResource1" %>
<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=13.1.19.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="App_Themes/css/map.css" rel="stylesheet" />
    <script>
        var contentString;</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="overlay">
                <div class="center-overlay">
                    <i class="ace-icon fa fa-spinner fa-spin orange bigger-300"></i>انتظر من فضلك
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <div class="tabbable"> 
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_Sites" class="tab-pane active">
                            <div class="clearfix"></div>
                            <div class="col-md-4">
                                <asp:GridView ID="gdvSites" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvSitesResource1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="م">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfID" runat="server" Value='<%# Eval("id") %>' />
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="العنوان" DataField="address" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkImages" runat="server" ToolTip="الصور" CommandArgument='<%# Eval("id") %>' OnCommand="lnkImages_Command" meta:resourcekey="lnkImagesResource1"><i class="fa fa-image purple"></i></asp:LinkButton>

                                                <asp:LinkButton ID="lnkDetails" runat="server" ToolTip="التفاصيل" CommandArgument='<%# Eval("id") %>' OnCommand="lnkDetails_Command" meta:resourcekey="lnkDetailsResource1"><i class="fa fa-list green"></i></asp:LinkButton>

                                                <a href='<%# "https://maps.google.com?q="+Eval("lat")+","+Eval("lng") %>' title="الموقع" target="_blank"><i class="fa fa-map-marker yellow"></i></a>

                                                <asp:LinkButton ID="lnkPrint" runat="server" ToolTip="طباعة" CommandArgument='<%# Eval("id") %>' OnCommand="lnkPrint_Command"><i class="fa fa-print red"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPrepared" runat="server" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" CommandArgument='<%# Eval("id") %>' OnCommand="lnkPrepared_Command" Visible='<%# bool.Parse(Eval("IsPreparedVisible").ToString()) %>' CssClass="btn btn-sm btn-yellow btn-bold">تم التجهيز</asp:LinkButton>
                                                <asp:LinkButton ID="lnkReceived" runat="server" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" CommandArgument='<%# Eval("id") %>' OnCommand="lnkReceived_Command" Visible='<%# bool.Parse(Eval("IsReceivedVisible").ToString()) %>' CssClass="btn btn-sm btn-success btn-bold">تم الأستلام</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#333333" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#F3F5F7" ForeColor="Black" HorizontalAlign="Right" />
                                    <RowStyle BackColor="#F3F5F7" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                    <SortedAscendingHeaderStyle BackColor="#848384" />
                                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                    <SortedDescendingHeaderStyle BackColor="#575357" />
                                </asp:GridView>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                        <div class="col-md-8" style="border-style: dashed; border-color: yellowgreen; padding-top: 5px;">
                            <div class="col-lg-12">
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       العنوان
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="5" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtAddressResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                        المساحة المطلوبة
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtRequiredArea" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtRequiredAreaResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       المساحة الفعلية
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtActualArea" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtActualAreaResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       عدد المكاتب
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtOffices" TextMode="Number" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtOfficesResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                        مصدر كهرباء دائم
                                    </label>
                                    <div class="col-lg-7">
                                        <label class="pull-right inline">
                                            <input id="chkElecSource" runat="server" type="checkbox" class="ace ace-switch ace-switch-7" />
                                            &nbsp;<span class="lbl middle"></span></label>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                        عدد الطاولات
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtTables" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtTablesResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                    قريب من المرافق الصحية
                                    </label>
                                    <div class="col-lg-7">
                                        <label class="pull-right inline">
                                            <input id="chkCloseToBathroom" runat="server" type="checkbox" class="ace ace-switch ace-switch-7" />
                                            &nbsp;<span class="lbl middle"></span></label>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                      عدد الكراسي
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtChairs" TextMode="Number" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtChairsResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       مكيف الهواء
                                    </label>
                                    <div class="col-lg-7">
                                        <label class="pull-right inline">
                                            <input id="chkAirConditio" runat="server" type="checkbox" class="ace ace-switch ace-switch-7">
                                            <span class="lbl middle"></span>
                                        </label>
                                        &nbsp;
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                        عدد دواليب الحفظ
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtWheels" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtWheelsResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                      الشخص المسؤول عن الصيانة
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtMaintenancePerson" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtMaintenancePersonResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                     هاتف الشخص المسؤول عن الصيانة
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtMaintenancePersonPhone" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtMaintenancePersonPhoneResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                    الشخص المسؤول عن أمن الموقع
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtSecurityPerson" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtSecurityPersonResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       هاتف الشخص المسؤول عن أمن الموقع
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtSecurityPersonPhone" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtSecurityPersonPhoneResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       رقم التليفون الأرضي
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtPhone" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtPhoneResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5">
                                       سرعة خط الأنترنت
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:TextBox ID="txtInternetLineSpeed" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtInternetLineSpeedResource1"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-lg-12 form-group">
                                    <label class="col-lg-2">
                                      خريطة الموقع
                                    </label>
                                    <div class="col-lg-10">
                                        <div class="pac-card" id="pac-card">
                                            <div id="pac-container">
                                                <input id="pac-input" type="text"
                                                    placeholder="ابحث بالعنوان" style="direction: rtl;">
                                            </div>
                                        </div>
                                        <div style="width: 100%; height: 350px;">
                                            <div id="map"></div>
                                        </div>
                                        <div id="infowindow-content">
                                            <img src="" width="16" height="16" id="place-icon">
                                            <span id="place-name" class="title"></span>
                                            <br>
                                            <span id="place-address"></span>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hdfLatitude" Value="30.044281" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hdfLongitude" Value="31.340002" runat="server"></asp:HiddenField>
                                </div>
                                <div class="col-lg-12 form-group">
                                    <label class="col-lg-2">
                                       ملاحظات أخرى
                                    </label>
                                    <div class="col-lg-10">
                                        <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="5" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtNotesResource1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 text-left fix">
                                <div class="clearfix"></div>
                                <hr />
                                <asp:HiddenField ID="hdfQuestionID" runat="server" />
                                <asp:HiddenField ID="hdfQuestionnaireID" runat="server" />
                                <asp:HiddenField ID="hdfDepartmentAppStatusID" runat="server" />
                                <asp:HiddenField ID="hdfMinisterAppStatusID" runat="server" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                <asp:Button ID="btnNew" runat="server" CssClass="btn btn-sm btn-default" Text="جـــديــد" OnClick="btnNew_Click" meta:resourcekey="btnNewResource1" />
                            </div>
                        </div>
                        <asp:HiddenField ID="hdfLatLng" runat="server" />
                        <asp:HiddenField ID="hdfImages" runat="server" />
                        <asp:ModalPopupExtender ID="mpeImages" TargetControlID="hdfImages" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlImages" runat="server" DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlImages" runat="server" meta:resourcekey="pnlImagesResource1">
                            <div class="modal-dialog">
                                <div class="modal-content modal-lg">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                        <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="col-xs-8 col-xs-offset-2">
                                                    <asp:FileUpload ID="fuImages" runat="server" AllowMultiple="True" meta:resourcekey="fuImagesResource1" />
                                                </div>
                                                <div class="col-xs-12">
                                                    <ul class="ace-thumbnails clearfix">
                                                        <asp:Repeater ID="rpData" runat="server">
                                                            <ItemTemplate>
                                                                <li>
                                                                    <input type="hidden" name="LocationId" value='<%# Eval("SiteLocationImage_ID") %>' />
                                                                    <a href='<%# Eval("SiteLocationImage_FileName") %>' data-rel="colorbox">
                                                                        <img width="150" height="150" alt="150x150" src='<%# Eval("SiteLocationImage_FileName") %>' onerror="this.src='/App_Themes/Admin/images/noImg.png'">
                                                                    </a>
                                                                    &nbsp;&nbsp;<div class="tags">
                                                                        <span class="label-holder">
                                                                            <span class="label label-success">بواسطة: <%# Eval("User_FullName") %></span>
                                                                        </span>
                                                                        <span class="label-holder">
                                                                            <span class="label label-warning">التاريخ: <%# string.Format("{0:dd/MM/yyyy}",Eval("SiteLocationImage_StampDate")) %></span>
                                                                        </span>
                                                                    </div>
                                                                    <div class="tools tools-right in">
                                                                        <asp:LinkButton ID="lnkDeleteImage" OnClientClick="return confirm('هل أنت متأكد من الحذف')" runat="server" OnCommand="lnkDeleteImage_Command" CommandArgument='<%# Eval("SiteLocationImage_FileName") %>' meta:resourcekey="lnkDeleteImageResource1"><i class="ace-icon fa fa-times red"></i></asp:LinkButton>
                                                                        <a href='<%# Eval("SiteLocationImage_FileName") %>' data-rel="colorbox">
                                                                            <i class="ace-icon fa fa-search-plus"></i>
                                                                        </a>
                                                                    </div>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                    <div class="space-10"></div>
                                                    <div class="alert alert-warning text-center" id="divNoImages" runat="server" visible="False">
                                                      لم يتم رفع صور بعد
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:Button ID="btnSaveImages" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveImages_Click" meta:resourcekey="btnSaveImagesResource1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveImages" />
        </Triggers>
    </asp:UpdatePanel>
    <telerik:ReportViewer ID="ReportViewer1" EnableAccessibility="false" runat="server" Style="display: none"></telerik:ReportViewer>
    <script>
        ReportViewer.prototype.PrintReport = function () {
            switch (this.defaultPrintFormat) {
                case "Default":
                    this.DefaultPrint();
                    break;
                case "PDF":
                    this.PrintAs("PDF");
                    previewFrame = document.getElementById(this.previewFrameID);
                    previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); }
                    break;
            }
        };
        ReportViewer.OnReportLoadedOld = ReportViewer.OnReportLoaded;
        ReportViewer.prototype.OnReportLoaded = function () {
            this.OnReportLoadedOld();
        }
        function fnPrintReport() {
    <%=ReportViewer1.ClientID %>.PrintReport();
        }
    </script>
 <script src="App_Themes/js/jquery-ui.min.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?libraries=places&key=AIzaSyD3ARneFhy_QSAHNyd5ZbXx2IEaUHUfFEI"></script>
    <script>
        function initiateUploader() {
            $("[id$=fuImages]").ace_file_input({
                style: 'well',
                btn_choose: 'Click to choose images',
                btn_change: null,
                no_icon: 'ace-icon fa fa-picture-o',
                droppable: true,
                thumbnail: 'small',
                allowExt: ["jpeg", "jpg", "png", "gif", "bmp"],
                allowMime: ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp"],
                preview_error: function () {
                }

            });
        }
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            initiateUploader();
            initiateMap();
        });
        function InitializeRequest() {
        }
        function EndRequest() {
            initiateUploader();
            initiateMap();
        }
        var marker;
        function initiateMap() {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: new google.maps.LatLng($('#<%=hdfLatitude.ClientID %>').val(), $('#<%=hdfLongitude.ClientID %>').val()),
                zoom: 8
            });
            var card = document.getElementById('pac-card');
            var input = document.getElementById('pac-input');
            var types = document.getElementById('type-selector');
            var strictBounds = document.getElementById('strict-bounds-selector');

            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(card);

            var autocomplete = new google.maps.places.Autocomplete(input);

            // Bind the map's bounds (viewport) property to the autocomplete object,
            // so that the autocomplete requests use the current map bounds for the
            // bounds option in the request.
            autocomplete.bindTo('bounds', map);

            var infowindow = new google.maps.InfoWindow();
            var infowindowContent = contentString;
            infowindow.setContent(infowindowContent);
            marker = new google.maps.Marker({
                map: map,
                draggable: true,
                animation: google.maps.Animation.DROP,
                anchorPoint: new google.maps.Point(0, -29),
                position: new google.maps.LatLng($('#<%=hdfLatitude.ClientID %>').val(), $('#<%=hdfLongitude.ClientID %>').val())
            });
            marker.addListener('click', toggleBounce);
            autocomplete.addListener('place_changed', function () {
                infowindow.close();
                marker.setVisible(false);
                var place = autocomplete.getPlace();
                if (!place.geometry) {
                    // User entered the name of a Place that was not suggested and
                    // pressed the Enter key, or the Place Details request failed.
                    window.alert("No details available for input: '" + place.name + "'");
                    return;
                }

                // If the place has a geometry, then present it on a map.
                if (place.geometry.viewport) {
                    map.fitBounds(place.geometry.viewport);
                } else {
                    map.setCenter(place.geometry.location);
                    map.setZoom(17);  // Why 17? Because it looks good.
                }
                marker.setPosition(place.geometry.location);
                marker.setVisible(true);

                var address = '';
                if (place.address_components) {
                    address = [
                        (place.address_components[0] && place.address_components[0].short_name || ''),
                        (place.address_components[1] && place.address_components[1].short_name || ''),
                        (place.address_components[2] && place.address_components[2].short_name || '')
                    ].join(' ');
                }
                infowindow.open(map, marker)
                var lat_lng = place.geometry.location.toString().substring(1, place.geometry.location.toString().length - 1);
                $('#<%=hdfLatLng.ClientID %>').val(lat_lng);
                $('#<%=hdfLatitude.ClientID %>').val(lat_lng.split(',')[0]);
                $('#<%=hdfLongitude.ClientID %>').val(lat_lng.split(',')[1]);
            });

            marker.addListener('click', function () {
                infowindow.open(map, marker);
            });
            google.maps.event.addListener(map, 'click', function (evt) {
                marker.setPosition(evt.latLng);
                $('#<%=hdfLatitude.ClientID %>').val(evt.latLng.lat().toFixed(3));
                $('#<%=hdfLongitude.ClientID %>').val(evt.latLng.lng().toFixed(3));
            });
            google.maps.event.addListener(marker, 'dragend', function (evt) {
                $('#<%=hdfLatitude.ClientID %>').val(evt.latLng.lat().toFixed(3));
                $('#<%=hdfLongitude.ClientID %>').val(evt.latLng.lng().toFixed(3));
            });
            // Sets a listener on a radio button to change the filter type on Places
            // Autocomplete.
            function setupClickListener(id, types) {
                var radioButton = document.getElementById(id);
                radioButton.addEventListener('click', function () {
                    autocomplete.setTypes(types);
                });
            }
            google.maps.event.trigger(map, 'resize');
            map.setZoom(map.getZoom());

        }
        function toggleBounce() {
            if (marker.getAnimation() !== null) {
                marker.setAnimation(null);
            } else {
                marker.setAnimation(google.maps.Animation.BOUNCE);
            }
        }
    </script>
</asp:Content>

