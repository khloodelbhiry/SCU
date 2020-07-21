<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="file-documents.aspx.cs" Inherits="file_documents" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=13.1.19.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="App_Themes/css/jquery-ui.css" rel="stylesheet" />
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
                <div class="clearfix"></div>
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                        <li class="active">
                            <a data-toggle="tab" href="#home4" aria-expanded="true">الوثائق (<asp:Literal ID="lblResult" runat="server" Text="0" meta:resourcekey="lblResultResource1"></asp:Literal>)</a>
                        </li>
                    </ul>
                    <div class="clearfix"></div>
                    <div class="tab-content">
                        <div id="home4" class="tab-pane active">
                            <div class="alert alert-info" style="padding-bottom:0px !important;">
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5 control-label bigger-150">
                                        باركود الملف
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:Label ID="lblFileBarcode" CssClass="bigger-115" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5 control-label bigger-150">
                                        الإدارة المالكة
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:Label ID="lblUnit" CssClass="bigger-115" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5 control-label bigger-150">
                                        عنوان الملف
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:Label ID="lblFileTitle" CssClass="bigger-115" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-6 form-group">
                                    <label class="col-lg-5 control-label bigger-150">
                                        مرجع الملف
                                    </label>
                                    <div class="col-lg-7">
                                        <asp:Label ID="lblFileReference" CssClass="bigger-115" runat="server"></asp:Label>
                                    </div>
                                </div>
                            <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-actions margin-0 no-margin-bottom">
                                <div class="col-xs-12">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtBarcodeSrc" AutoPostBack="true" OnTextChanged="txtBarcodeSrc_TextChanged" placeholder="ابحث بباركود الوثيقة" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="lnkSearch" runat="server" CssClass="btn btn-sm btn-pink no-radius" OnClick="lnkSearch_Click"><i class="ace-icon fa fa-search"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkClearSearch" runat="server" CssClass="btn btn-sm btn-gray no-radius" OnClick="lnkClearSearch_Click"><i class="ace-icon fa fa-trash"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkBoxFiles" runat="server" OnClick="lnkBoxFiles_Click" CausesValidation="False" CssClass="btn btn-sm btn-purple no-radius"><i class="ace-icon fa fa-plus bigger-110"></i>إضافة وثيقة</asp:LinkButton>
                                            <asp:LinkButton ID="lnkBack" runat="server" CausesValidation="False" CssClass="btn btn-sm btn-warning no-radius" ToolTip="رجوع للملفات"><i class="ace-icon fa fa-backward bigger-110"></i></asp:LinkButton>
                                            <%--  <div class="btn-group" style="margin-right:20px;">
                                                <asp:LinkButton ID="lnkChangeAllStatus" runat="server" CssClass="btn btn-info btn-white" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" OnCommand="lnkChangeAllStatus_Command"><i class="ace-icon fa fa-check-circle bigger-110"></i>تــــــــــــم</asp:LinkButton>
                                                <button data-toggle="dropdown" class="btn btn-info btn-white dropdown-toggle" aria-expanded="false">
                                                    <span class="ace-icon fa fa-caret-down icon-only"></span>
                                                </button>
                                                <ul class="dropdown-menu dropdown-info dropdown-menu-right">
                                                    <li>
                                                        <asp:LinkButton ID="lnkReverseAll" runat="server" CssClass="btn btn-info btn-white" OnCommand="lnkReverseAll_Command">ارجاع لحالة سابقة</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>--%>
                                        </span>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="كود الوثيقة" SortExpression="barcode">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLog" runat="server" CommandArgument='<%# Eval("id")+";"+Eval("fileId") %>' OnCommand="lnkLog_Command"><%# Eval("barcode") %></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="الإدارة" DataField="unit" SortExpression="unit" />
                                    <asp:BoundField HeaderText="كود الباتش" DataField="batch" SortExpression="batch" ItemStyle-CssClass="center" />
                                    <asp:BoundField HeaderText="# الصفحات" DataField="noOfPages" SortExpression="noOfPages" ItemStyle-CssClass="center" />
                                    <asp:TemplateField HeaderText="تغير الحالة" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <div class='<%# ((Eval("nextStateId").ToString()!=string.Empty&&int.Parse(Eval("nextStateId").ToString())==(int)StatesEnum.DocumentsIndexing&&Eval("isIndexed").ToString()=="False")||(Eval("nextStateId").ToString()!=string.Empty&&int.Parse(Eval("nextStateId").ToString())==(int)StatesEnum.DocumentsQa&&Eval("isReviewed").ToString()=="False"))&&(CheckVisible(int.Parse(Eval("nextStateId").ToString()),1)||CheckVisible(int.Parse(Eval("nextStateId").ToString()),2))?"btn-group":"btn-group hidden" %>'>
                                                <asp:LinkButton ID="lnkChangeStatus" Visible='<%# Eval("nextStateId").ToString()!=string.Empty&&CheckVisible(int.Parse(Eval("nextStateId").ToString()),1) %>' runat="server" CssClass="btn btn-info btn-white" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" CommandArgument='<%# Eval("id") +";"+Eval("nextStateId") %>' OnCommand="lnkChangeStatus_Command">تمت </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="حالة الوثيقة" DataField="state" SortExpression="state" />
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
                        <div class="clearfix"></div>
                        <asp:HiddenField ID="hdfCreateDocuments" runat="server" />
                        <asp:ModalPopupExtender ID="mpeCreateDocuments" TargetControlID="hdfCreateDocuments" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlCreateDocuments" runat="server" Enabled="True">
                        </asp:ModalPopupExtender>
                        <asp:Panel ID="pnlCreateDocuments" runat="server">
                            <div class="modal-dialog">
                                <div class="modal-content modal-sm">
                                    <div class="modal-header">
                                        <h3 style="text-align: right; margin: 0px;">
                                            <asp:LinkButton ID="btnCloseDocuments" runat="server" Text="إغلاق" class="pull-left" OnClick="btnCloseDocuments_Click"></asp:LinkButton></h3>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row">
                                            <div class="col-lg-12 form-group">
                                                <label class="col-lg-5">
                                                    عدد الوثائق
                                                </label>
                                                <div class="col-lg-7">
                                                    <asp:TextBox ID="txtDocumentsCount" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtBoxFilesCountResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvDocumentsCount" runat="server" Display="Dynamic" ControlToValidate="txtDocumentsCount" ValidationGroup="vgDocuments" meta:resourcekey="rfvBoxFilesCountResource1">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-lg-12 form-group">
                                                <label class="col-lg-5">
                                                    عدد النسخ
                                                </label>
                                                <div class="col-lg-7">
                                                    <asp:TextBox ID="txtCopies" Text="1" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtBoxFilesCountResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtCopies" ValidationGroup="vgDocuments" meta:resourcekey="rfvBoxFilesCountResource1">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:Button ID="btnSaveDocuments" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveDocuments_Click" ValidationGroup="vgDocuments" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
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
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <telerik:ReportViewer ID="ReportViewer1" EnableAccessibility="false" runat="server" Style="display: none" meta:resourcekey="ReportViewer1Resource1" ReportBookID=""></telerik:ReportViewer>
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
</asp:Content>

