<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="prerequisites.aspx.cs" Inherits="prerequisites" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeView" TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="overlay">
                <div class="center-overlay">
                    <i class="ace-icon fa fa-spinner fa-spin orange bigger-300"></i>من فضلك انتظر
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <asp:Panel runat="server" ID="Panel1" Visible="false">
                    <div class="form-horizontal">
                        <div class="text-left">
                            <asp:LinkButton ID="btnAddNew" runat="server" CssClass=" btn btn-sm btn-purple " OnClick="btnAddNew_Click" CausesValidation="False" meta:resourcekey="btnAddResource1"><i class="fa fa-plus"></i> مهمة جديدة</asp:LinkButton>
                            <a href="#" class="btn btn-sm btn-success"><i class="fa fa-dashboard"></i>لوحة متابعةالإنجاز</a>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                    <Columns>
                        <asp:TemplateField HeaderText="المهمة" ItemStyle-VerticalAlign="Middle">
                            <ItemTemplate>
                                <i class='<%# Eval("type").ToString()=="p"?"fa fa-check-circle green":"fa fa-refresh pink" %>'></i> <%# Eval("name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PrerequisiteCategory" HeaderText="الفئة" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="ImplementerType" HeaderText="مسئولية التنفيذ" />
                        <asp:TemplateField HeaderText="الوزن النسبي" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <%# decimal.Parse(Eval("relativeWeight").ToString()).ToString("G29") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ProgressCalculationType" HeaderText="طريقة احتساب الإنجاز" ItemStyle-CssClass="center" />
                        <asp:TemplateField HeaderText="نسبة الإنجاز">
                            <ItemTemplate>
                                <%#  GetProgress(int.Parse(Eval("id").ToString()),Eval("type").ToString())+" %" %>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="متكررة" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <label class="inline">
                                    <input id="chkLoop" type="checkbox" class="ace input-lg" <%# Eval("forEveryProject").ToString()=="True"?"checked":string.Empty %> />
                                    <span class="lbl middle"></span>
                                </label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="تعديل" Visible="false" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="تعديل" meta:resourcekey="btnEditResource1"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="حذف" Visible="false" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('هل أنت متأكد من الحذف؟');" CommandArgument='<%# Bind("id") %>' OnCommand="btnDelete_Command" CausesValidation="False" ToolTip="حذف" meta:resourcekey="btnDeleteResource1"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                <asp:ModalPopupExtender ID="mpeObject" TargetControlID="hdfObject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlObject" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlObject" runat="server">
                    <div class="modal-dialog" id="modal">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-12 form-group">
                                            <label class="col-lg-2">المهمة</label>
                                            <div class="col-lg-10">
                                                <asp:TextBox ID="txtTitle" CssClass="col-md-12 form-control" runat="server" meta:resourcekey="txtTitleResource1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtTitle" ValidationGroup="vgSave" meta:resourcekey="rfvTitleResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                تنفذ بعد المهمة
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlStartAfter" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">الفئة</label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" meta:resourcekey="ddlCategoryResource1"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="ddlCategory" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                مسئولية التنفيذ
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlImplementerType" runat="server" CssClass="form-control" meta:resourcekey="ddlImplementerTypeResource1"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="ddlImplementerType" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="RequiredFieldValidator4Resource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">الوزن النسبي</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtWeight" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">طريقةاحتساب نسبة الإنجاز</label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlCalculationTypes" runat="server" CssClass="form-control" meta:resourcekey="ddlCategoryResource1"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="ddlCalculationTypes" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="RequiredFieldValidator3Resource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">متكررة</label>
                                            <div class="col-lg-8">
                                                <label class="pull-right inline">
                                                    <input id="chkForEveryProject" runat="server" type="checkbox" class="ace ace-switch ace-switch-7">
                                                    <span class="lbl middle"></span>
                                                </label>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

