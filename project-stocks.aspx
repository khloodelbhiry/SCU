<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-stocks.aspx.cs" Inherits="poject_stocks" meta:resourcekey="PageResource1" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="App_Themes/css/map.css" rel="stylesheet" />
    <script>
        var contentString;</script>
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
                        <div id="id_Stock" class="tab-pane active">
                            <asp:Panel runat="server" ID="Panel1">
                                <div class="page-header">
                                    <h1>البحث</h1>
                                </div>
                                <div class="form-horizontal">
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الكود
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtCodeSrc" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الاسم
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server" meta:resourcekey="txtNameSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            المحافظة
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlGovernorateSrc" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGovernorateSrc_SelectedIndexChanged" meta:resourcekey="ddlGovernorateSrcResource1"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            المدينة
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlCitySrc" CssClass="form-control" runat="server" meta:resourcekey="ddlCitySrcResource1"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            المسؤول
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtAdminSrc" CssClass="form-control" runat="server" meta:resourcekey="txtAdminSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الهاتف المحمول
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtMobileSrc" CssClass="form-control" runat="server" meta:resourcekey="txtMobileSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الحالة
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control" meta:resourcekey="ddlStatusSrcResource1"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12 text-left fix">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" meta:resourcekey="btnClearSearchResource1" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass=" btn btn-sm btn-danger " Text="إضافة جديدة" OnClick="btnAdd_Click" CausesValidation="False" meta:resourcekey="btnAddResource1" />
                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div class="clearfix"></div>
                            <div class="header smaller lighter blue"></div>
                            <div class="clearfix"></div>
                            <div class="page-header">
                                <h1>نتائج البحث
                                    <asp:Label ID="lblResult" runat="server" Text="0" meta:resourcekey="lblResultResource1"></asp:Label></h1>
                            </div>
                            <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                                <Columns>
                                    <asp:BoundField DataField="code" HeaderText="كود السمتودع" SortExpression="code" />
                                    <asp:BoundField DataField="name" HeaderText="اسم المستودع" SortExpression="name" />
                                    <asp:BoundField DataField="governorate" HeaderText="المحافظة" SortExpression="governorate" />
                                    <asp:BoundField DataField="governorate" HeaderText="المدينة" SortExpression="city" />
                                    <asp:BoundField DataField="address" HeaderText="العنوان" SortExpression="address" />
                                    <asp:BoundField DataField="administrator" HeaderText="المسئول" SortExpression="administrator" />
                                    <asp:BoundField DataField="mobile" HeaderText="الهاتف المحمول" SortExpression="mobile" />
                                    <asp:BoundField DataField="status" HeaderText="الحالة" SortExpression="status" />
                                    <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="btnEdit_Command" ToolTip="تعديل" CausesValidation="False"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" Visible='<%# Eval("statusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="btnDelete_Command" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
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
                            <asp:HiddenField ID="hdfStock" runat="server" />
                            <asp:ModalPopupExtender ID="mpeStock" TargetControlID="hdfStock" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlStock" runat="server" DynamicServicePath="" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlStock" runat="server" meta:resourcekey="pnlStockResource1">
                                <div class="modal-dialog">
                                    <div class="modal-content modal-lg">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4 top">الكود</label>
                                                        <div class="col-lg-8 topr calend">
                                                            <asp:TextBox ID="txtCode" Enabled="false" CssClass="col-md-12 form-control" runat="server" meta:resourcekey="txtCodeResource1"></asp:TextBox>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">
                                                            الاسم
                                                        </label>
                                                        <div class="col-lg-8 topr">
                                                            <asp:TextBox ID="txtName" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtNameResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave" meta:resourcekey="rfvNameResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">المسؤول</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:TextBox ID="txtAdmin" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtAdminResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvAdmin" runat="server" Display="Dynamic" ControlToValidate="txtAdmin" ValidationGroup="vgSave" meta:resourcekey="rfvAdminResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">الهاتف المحمول</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:TextBox ID="txtMobile" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtMobileResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvMobile" runat="server" Display="Dynamic" ControlToValidate="txtMobile" ValidationGroup="vgSave" meta:resourcekey="rfvMobileResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">المحافظة</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:DropDownList ID="ddlGovernorate" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlGovernorate_SelectedIndexChanged" meta:resourcekey="ddlGovernorateResource1"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvGovernorate" runat="server" Display="Dynamic" ControlToValidate="ddlGovernorate" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="rfvGovernorateResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">المدينة</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:DropDownList ID="ddlCity" runat="server" CssClass="form-control" meta:resourcekey="ddlCityResource1"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvCity" runat="server" Display="Dynamic" ControlToValidate="ddlCity" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="rfvCityResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">العنوان</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtAddressResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvAddress" runat="server" Display="Dynamic" ControlToValidate="txtAddress" ValidationGroup="vgSave" meta:resourcekey="rfvAddressResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="clearfix"></div>
                                                        <label class="col-lg-4 top">ملاحظات</label>
                                                        <div class="col-lg-8 topr ">
                                                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtNotesResource1"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
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
                                            </div>
                                            <div class="modal-footer">
                                                <div class="col-md-12 text-left fix">
                                                    <div class="clearfix"></div>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                                    <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                                    <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfLatLng" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="App_Themes/js/jquery-ui.min.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?libraries=places&key=AIzaSyD3ARneFhy_QSAHNyd5ZbXx2IEaUHUfFEI"></script>
    <script>
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            initiateMap();
        });
        function InitializeRequest() {
        }
        function EndRequest() {
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

