<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="company-assets.aspx.cs" Inherits="company_assets" %>

<%@ Register Src="UCs/companies.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                    <asp:WebControl ID="ucCompaanies" runat="server" />
                    <div class="tab-content">
                        <div id="id_Stock" class="tab-pane active">
                            <asp:Panel runat="server" ID="Panel1">
                                <div class="page-header">
                                    <h1>البحث</h1>
                                </div>
                                <div class="form-horizontal">
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الاسم
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server" meta:resourcekey="txtDepartmentSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الموديل
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtModelSrc" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            رقم المسلسل
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtSerNoSrc" CssClass="form-control" runat="server" meta:resourcekey="txtNameSrcResource1"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            حالة الأصل</label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlStatusSrc" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            المشروع</label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlProjectSrc" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-12 text-left fix">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" meta:resourcekey="btnClearSearchResource1" />
                                        <asp:Button ID="btnAdd" runat="server" CssClass=" btn btn-sm btn-danger " Text="إضافة جديدة" OnClick="btnAdd_Click" CausesValidation="False" meta:resourcekey="btnAddResource1" />
                                        <asp:Button ID="btnAssign" runat="server" CssClass="btn btn-sm btn-yellow " Text="تخصيص لمشروع" OnClick="btnAssign_Click" CausesValidation="False" />
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
                                    <asp:TemplateField HeaderText="تخصيص لمشروع" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <label class="inline">
                                                <input id="chkProjectRow" type="checkbox" runat="server" class="ace input-lg" />
                                                <span class="lbl middle"></span>
                                            </label>
                                            <asp:HiddenField ID="hdfID" runat="server" Value='<%# Bind("id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="الأسم" SortExpression="name">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkLog" runat="server" OnCommand="lnkLog_Command" CommandArgument='<%# Eval("id") %>'><%# Eval("name") %></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="الموديل" DataField="model" SortExpression="model" />
                                    <asp:BoundField HeaderText="رقم المسلسل" DataField="serialNo" SortExpression="serialNo" />
                                    <asp:TemplateField HeaderText="حالة الأصل" SortExpression="status">
                                        <ItemTemplate>
                                            <%# Eval("status") %>
                                            <asp:LinkButton ID="lnkReceived" runat="server" Visible='<%# Eval("assetsStatusId").ToString()=="1" %>' OnCommand="lnkReceived_Command" CommandArgument='<%# Eval("id") %>' OnClientClick="return confirm('هل أنت متأكد من تغير الحالة ؟');">( تم التوريد )</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="المشروع" DataField="project" SortExpression="project" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="#">شهادة الأستلام</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Visible='<%# Eval("assetsStatusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="lnkEdit_Command" ToolTip="تعديل" CausesValidation="False"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" Visible='<%# Eval("assetsStatusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="lnkDelete_Command" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                            <asp:HiddenField ID="hdfObject" runat="server" />
                            <asp:ModalPopupExtender ID="mpeObject" TargetControlID="hdfObject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlObject" runat="server" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlObject" runat="server">
                                <div class="modal-dialog">
                                    <div class="modal-content modal-lg">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">
                                                            اسم الأصل
                                                        </label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtName" CssClass="col-sm-4 form-control" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave" meta:resourcekey="rfvNameResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4 top">الموديل</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtModel" CssClass="col-md-12 form-control" runat="server" meta:resourcekey="txtCodeResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtModel" ValidationGroup="vgSave" meta:resourcekey="rfvNameResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">
                                                            رقم المسلسل
                                                        </label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtSerNo" CssClass="col-sm-4 form-control" runat="server" meta:resourcekey="txtNameResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtSerNo" ValidationGroup="vgSave" meta:resourcekey="rfvNameResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="col-md-12 text-left fix">
                                                    <div class="clearfix"></div>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-sm btn-grey" Text="جديد" OnClick="btnClear_Click" CausesValidation="False" meta:resourcekey="btnClearResource1" />
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
                                    <div class="modal-content modal-lg">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="LinkButton1" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click"></asp:LinkButton>
                                            </h3>
                                        </div>
                                        <div class="modal-body" style="max-height: 500px; overflow-y: auto;">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <asp:GridView ID="gdvStatus" AllowSorting="True" OnSorting="gdvStatus_Sorting" AllowPaging="True" OnPageIndexChanging="gdvStatus_PageIndexChanging" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                                                        <Columns>
                                                            <asp:BoundField HeaderText="المشروع" DataField="project" SortExpression="project" />
                                                            <asp:BoundField HeaderText="تم التخصيص بواسطة" DataField="assignedBy" SortExpression="assignedBy" />
                                                            <asp:BoundField HeaderText="تم التخصيص بتاريخ" DataField="assignmentDate" SortExpression="assignmentDate" />
                                                            <asp:BoundField HeaderText="تم الأستلام بواسطة" DataField="deliveredBy" SortExpression="deliveredBy" />
                                                            <asp:BoundField HeaderText="تم الأستلام بتاريخ" DataField="deliveryDate" SortExpression="deliveryDate" />
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
                            <asp:HiddenField ID="hdfProject" runat="server" />
                            <asp:ModalPopupExtender ID="mpeProject" TargetControlID="hdfProject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlProject" runat="server" DynamicServicePath="" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlProject" runat="server">
                                <div class="modal-dialog">
                                    <div class="modal-content modal-sm">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkClosePoject" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkClosePoject_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">
                                                            المشروع
                                                        </label>
                                                        <div class="col-lg-8">
                                                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="col-md-12 text-left fix">
                                                    <div class="clearfix"></div>
                                                    <asp:Button ID="btnSaveProject" runat="server" CssClass="btn btn-sm btn-danger" Text="حفظ" OnClick="btnSaveProject_Click" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

