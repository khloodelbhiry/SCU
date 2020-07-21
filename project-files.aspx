<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-files.aspx.cs" Inherits="project_files" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=13.1.19.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                        <div id="id_Boxes" class="tab-pane active">
                            <div class="form-actions margin-0 no-margin-bottom">
                                <div class="col-xs-12">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtBarcodeSrc" Width="55%" AutoPostBack="true" OnTextChanged="txtBarcodeSrc_TextChanged" placeholder="ابحث بباركود الملف" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:DropDownList ID="ddlStatus" Width="30%" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:DropDownList ID="ddlPageSize" Width="15%" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="50" Text="عرض 50 صف" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="100" Text="عرض 100 صف"></asp:ListItem>
                                            <asp:ListItem Value="all" Text="عرض الكل"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="lnkSearch" ToolTip="بحث" runat="server" CssClass="btn btn-sm btn-pink no-radius" OnClick="lnkSearch_Click"><i class="ace-icon fa fa-search"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkClearSearch" ToolTip="بحث جديد" runat="server" CssClass="btn btn-sm btn-gray no-radius" OnClick="lnkClearSearch_Click"><i class="ace-icon fa fa-trash"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkBoxFiles" runat="server" OnClick="lnkBoxFiles_Click" CausesValidation="False" CssClass="btn btn-sm btn-purple no-radius"><i class="ace-icon fa fa-plus bigger-110"></i>تسجيل الملفات</asp:LinkButton>
                                            <div class="btn-group" style="margin-right: 20px;" runat="server" id="divBtnChangeStatus">
                                                <asp:LinkButton ID="lnkChangeAllStatus" runat="server" CssClass="btn btn-info btn-white" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" OnCommand="lnkChangeAllStatus_Command"><i class="ace-icon fa fa-check-circle bigger-110"></i>تــــــــــــم</asp:LinkButton>
                                                <button data-toggle="dropdown" class="btn btn-info btn-white dropdown-toggle" aria-expanded="false">
                                                    <span class="ace-icon fa fa-caret-down icon-only"></span>
                                                </button>
                                                <ul class="dropdown-menu dropdown-info dropdown-menu-right">
                                                    <li>
                                                        <asp:LinkButton ID="lnkReverseAll" runat="server" CssClass="btn btn-info btn-white" OnCommand="lnkReverseAll_Command">ارجاع لحالة سابقة</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </span>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                            <div class=" table-responsive">
                                <asp:GridView ID="gdvData" CssClass="table" OnRowDataBound="gdvData_RowDataBound" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <label class="inline">
                                                    <input id="chkAll" type="checkbox" runat="server" class="ace input-lg" onclick="checkAll(this);" />
                                                    <span class="lbl middle"></span>
                                                </label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfId" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:HiddenField ID="hdfState" runat="server" Value='<%# Eval("nextStateId") %>' />
                                                <asp:HiddenField ID="hdfOrder" runat="server" Value='<%# Eval("stateOrder") %>' />
                                                <asp:HiddenField ID="hdfUnit" runat="server" Value='<%# Eval("unitStructureId") %>' />
                                                <label class="inline">
                                                    <input id="chkFile" type="checkbox" runat="server" class="ace input-lg" onclick="Check_Click(this)" />
                                                    <span class="lbl middle"></span>
                                                </label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="كود الملف" SortExpression="barcode">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkLog" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkLog_Command"><%#  Eval("barcode") %></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="الإدارة" DataField="unit" SortExpression="unit" />
                                        <asp:BoundField HeaderText="كود الباتش" DataField="batch" SortExpression="batch" ItemStyle-CssClass="center" />
                                        <asp:BoundField HeaderText="# الوثائق" DataField="docs" SortExpression="docs" ItemStyle-CssClass="center" />
                                        <asp:BoundField HeaderText="# الصفحات" DataField="noOfPages" SortExpression="noOfPages" ItemStyle-CssClass="center" />
                                        <asp:TemplateField HeaderText="تغير الحالة" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <div class='<%#Eval("nextStateId").ToString()!="0"&&(CheckVisible(int.Parse(Eval("nextStateId").ToString()),1)||CheckVisible(int.Parse(Eval("nextStateId").ToString()),2))?"btn-group":"btn-group hidden" %>'>
                                                    <asp:LinkButton ID="lnkChangeStatus" Visible='<%# CheckVisible(int.Parse(Eval("nextStateId").ToString()),1) %>' runat="server" CssClass="btn btn-info btn-white" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" CommandArgument='<%# Eval("id")+";"+Eval("stateOrder")+";"+Eval("nextStateId") %>' OnCommand="lnkChangeStatus_Command">تم </asp:LinkButton>
                                                    <button data-toggle="dropdown" class="btn btn-info btn-white dropdown-toggle" aria-expanded="false">
                                                        <span class="ace-icon fa fa-caret-down icon-only"></span>
                                                    </button>
                                                    <ul class="dropdown-menu dropdown-info dropdown-menu-right">
                                                        <li>
                                                            <asp:LinkButton ID="lnkReverse" runat="server" Visible='<%# CheckVisible(int.Parse(Eval("nextStateId").ToString()),2) %>' CssClass="btn btn-info btn-white" CommandArgument='<%# Eval("id")+";"+Eval("stateOrder") %>' OnCommand="lnkReverse_Command">ارجاع لحالة سابقة</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="حالة الملف" DataField="state" SortExpression="state" />
                                        <asp:TemplateField HeaderText="طباعة باركود الملف">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPrintFileBarcode" runat="server" CommandArgument='<%# Eval("barcode")+";"+Eval("unit") %>' OnCommand="lnkPrintFileBarcode_Command"><i class="fa fa-print"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="الوثائق">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDocs" runat="server" PostBackUrl='<%# "file-documents.aspx?b="+EncryptString.Encrypt(Eval("id").ToString())+"&id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"] %>'><i class="fa fa-eye"></i></asp:LinkButton>
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
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <asp:HiddenField ID="hdfLog" runat="server" />
                        <asp:ModalPopupExtender ID="mpeLog" TargetControlID="hdfLog" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlLog" runat="server" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlLog" runat="server">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                            <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click"></asp:LinkButton>
                                        </h3>
                                    </div>
                                    <div class="modal-body" style="max-height: 500px; overflow-y: auto;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:GridView ID="gdvStatus" OnRowDataBound="gdvStatus_RowDataBound" AllowSorting="True" OnSorting="gdvStatus_Sorting" AllowPaging="True" OnPageIndexChanging="gdvStatus_PageIndexChanging" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="الحالة" SortExpression="name">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdfAction" Value='<%# Eval("actionTypeId") %>' runat="server" />
                                                                <%# Eval("name") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="بواسطة" DataField="fullName" SortExpression="fullName" />
                                                        <asp:BoundField HeaderText="التاريخ" DataField="date" SortExpression="date" />
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <HeaderStyle BackColor="#333333" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#F3F5F7" ForeColor="Black" HorizontalAlign="Right" />
                                                    <RowStyle BackColor="#F3F5F7" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                                    <SortedAscendingHeaderStyle BackColor="#848384" />
                                                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                                    <SortedDescendingHeaderStyle BackColor="#575357" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="hdfCreateBoxfile" runat="server" />
                        <asp:ModalPopupExtender ID="mpeCreateBoxfile" TargetControlID="hdfCreateBoxfile" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlCreateBoxfile" runat="server" DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlCreateBoxfile" runat="server">
                            <div class="modal-dialog">
                                <div class="modal-content modal-md">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                            <asp:LinkButton ID="btnCloseBoxFiles" runat="server" Text="إغلاق" class="pull-left" OnClick="btnCloseBoxFiles_Click" meta:resourcekey="btnCloseBoxFilesResource1"></asp:LinkButton>
                                        </h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        الإدارة
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:DropDownList ID="ddlUnit" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvUnit" InitialValue="0" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="ddlUnit" ValidationGroup="vgBoxFiles">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        عدد الملفات (تقديري)
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:TextBox ID="txtBoxFilesCount" TextMode="Number" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtBoxFilesCountResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvBoxFilesCount" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="txtBoxFilesCount" ValidationGroup="vgBoxFiles" meta:resourcekey="rfvBoxFilesCountResource1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        متوسط عدد الوثائق بكل ملف (تقديري)
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:TextBox ID="txtDocsCount" TextMode="Number" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtBoxFilesCountResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDocsCount" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="txtDocsCount" ValidationGroup="vgBoxFiles" meta:resourcekey="rfvBoxFilesCountResource1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        متوسط عدد الورق بكل ملف (تقديري)
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:TextBox ID="txtPagesCount" TextMode="Number" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtBoxFilesCountResource1"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvPagesCount" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="txtPagesCount" ValidationGroup="vgBoxFiles" meta:resourcekey="rfvBoxFilesCountResource1">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:Button ID="btnSaveBoxFiles" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveBoxFiles_Click" ValidationGroup="vgBoxFiles" meta:resourcekey="btnSaveBoxFilesResource1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="hdfBarcodeDocuments" runat="server" />
                        <asp:ModalPopupExtender ID="mpeBarcodeDocuments" TargetControlID="hdfBarcodeDocuments" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlBarcodeDocuments" runat="server" DynamicServicePath="" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlBarcodeDocuments" runat="server">
                            <div class="modal-dialog">
                                <div class="modal-content modal-md">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                            <asp:LinkButton ID="lnkCloseBarcodeDocuments" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseBarcodeDocuments_Click"></asp:LinkButton>
                                        </h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        من الوثيقة
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:TextBox ID="txtDocFrom" Text="1" TextMode="Number" CssClass="col-sm-4 form-control" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDocFrom" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="txtDocFrom" ValidationGroup="vgBarcodeDocs">*</asp:RequiredFieldValidator>
                                                        <asp:RangeValidator ID="rvDocFrom" runat="server" ControlToValidate="txtDocFrom" ErrorMessage="يجب ان تبدأ لوثائق من الباركود 1" ForeColor="Red" MaximumValue="1" MinimumValue="1" Type="Integer" ValidationGroup="vgBarcodeDocs"></asp:RangeValidator>
                                                    </div>
                                                </div>
                                                <div class="col-lg-12 form-group">
                                                    <label class="col-lg-6">
                                                        الى الوثيقة
                                                    </label>
                                                    <div class="col-lg-6">
                                                        <asp:TextBox ID="txtDocTo" TextMode="Number" CssClass="col-sm-4 form-control" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDocTo" ForeColor="Red" runat="server" Display="Dynamic" ControlToValidate="txtDocTo" ValidationGroup="vgBarcodeDocs">*</asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="cmpMax" runat="server" ForeColor="Red" ControlToValidate="txtDocTo" Operator="GreaterThanEqual" Display="Dynamic" Type="Integer" SetFocusOnError="true" ValueToCompare="1" ErrorMessage="يجب ان يكون الباركود اكبر من او يساوي 1" ValidationGroup="vgBarcodeDocs"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:HiddenField ID="hdfId_Barcode" runat="server" />
                                                <asp:HiddenField ID="hdfOrder_Barcode" runat="server" />
                                                <asp:HiddenField ID="hdfState_Barcode" runat="server" />
                                                <asp:HiddenField ID="hdfCurrentState_Barcode" runat="server" />
                                                <asp:Button ID="btnSaveBarcodeDocuments" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveBarcodeDocuments_Click" ValidationGroup="vgBarcodeDocs" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:HiddenField ID="hdfReverse" runat="server" />
                        <asp:ModalPopupExtender ID="mpeReverse" TargetControlID="hdfReverse" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlReverse" runat="server" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlReverse" runat="server">
                            <div class="modal-dialog">
                                <div class="modal-content modal-md">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                            <asp:LinkButton ID="lnkCloseReverse" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseReverse_Click"></asp:LinkButton>
                                        </h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <div class="col-lg-12 form-group">
                                                    <div class="page-header text-right">
                                                        <h1>اختر الحالة :
                                                        </h1>
                                                    </div>
                                                    <div class="col-lg-12 text-right">
                                                        <asp:Repeater ID="rpStates" runat="server">
                                                            <ItemTemplate>
                                                                <div class="radio">
                                                                    <label>
                                                                        <input name="rdStates" value='<%#Eval("id") %>' type="radio" id="rdStates" runat="server" class="ace input-lg" />
                                                                        <span class="lbl bigger-120"><%#Eval("name") %></span>
                                                                    </label>
                                                                </div>
                                                                <div class="clearfix"></div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:Button ID="btnSaveReverse" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveReverse_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:reportviewer id="ReportViewer1" enableaccessibility="false" runat="server" style="display: none"></telerik:reportviewer>
    <script>
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }
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
</asp:Content>

