<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-doc-types.aspx.cs" Inherits="DocumentTypes" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Src="UCs/project-lookups.ascx" TagName="WebControl2" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="updateProgress" DisplayAfter="0" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upnl">
        <ProgressTemplate>
            <div class="loading">
                <div>
                    <img class="" src="App_Themes/img/loading18.gif" alt="loading" width="100px" style="top: 40%; right: 50%; position: absolute;">
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="upnl">
        <ContentTemplate>
            <div class="page-content">
                <div class="tabbable">
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_issues" class="tab-pane active">
                            <div class="col-xs-12">
                                <div class="tabbable tabs-right">
                                    <asp:WebControl2 ID="ucLookup" runat="server" />
                                    <div class="tab-content">
                                        <div id="home3" class="tab-pane active">
                                            <div class="page-header">
                                                <h1>البحث</h1>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="form-horizontal">
                                                        <div class="form-group col-md-6">
                                                            <label class="col-lg-5 control-label">الاسم</label>
                                                            <div class="col-lg-7">
                                                                <asp:TextBox ID="txtNameSrch" CssClass="form-control border-input" runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group col-md-6">
                                                            <label class="col-lg-5 control-label">الحالة</label>
                                                            <div class="col-lg-7">
                                                                <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12 text-left form-group">
                                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger" Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" />
                                                        <asp:Button ID="btnNewSearch" runat="server" CssClass="btn btn-sm btn-grey" Text="بحــــث جديد" CausesValidation="False" OnClick="btnNewSearch_Click" />
                                                        <asp:Button ID="btnNewDocumentType" runat="server" OnClick="btnNewDocType_Click" CausesValidation="false" CssClass="btn btn-sm btn-danger" Text="اضافة جديدة" />
                                                        <div class="clearfix"></div>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="header smaller lighter blue"></div>
                                                    <div class="clearfix"></div>
                                                    <div class="page-header">
                                                        <h1>نتائج البحث
            <asp:Label ID="lblResult" runat="server" Text="0"></asp:Label></h1>
                                                    </div>
                                                    <asp:GridView ID="gdvData" Font-Size="Large" CssClass="table table-hover table-responsive" runat="server" AllowPaging="True" AllowSorting="true" OnSorting="gdvData_Sorting" AutoGenerateColumns="false" PageSize="30" EmptyDataText="لا يوجد بيانات" OnPageIndexChanging="gdvData_PageIndexChanging" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="id" HeaderText="ID " SortExpression="id" />
                                                            <asp:BoundField DataField="name" HeaderText="الاسم " SortExpression="name" />
                                                            <asp:BoundField DataField="status" HeaderText="الحالة " SortExpression="status" />
                                                            <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkEdit_Command" ToolTip="تعديل" CausesValidation="False"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDelete" runat="server" Visible='<%# Eval("statusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="lnkDelete_Command" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCC99" />
                                                        <HeaderStyle BackColor="#333333" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#f3f5f7" ForeColor="Black" HorizontalAlign="Right" />
                                                        <RowStyle BackColor="#f3f5f7" />
                                                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                                        <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                                        <SortedAscendingHeaderStyle BackColor="#848384" />
                                                        <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                                        <SortedDescendingHeaderStyle BackColor="#575357" />
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hdfDocumentType" runat="server" />
                                            <asp:ModalPopupExtender ID="mpeDocumentType" TargetControlID="hdfDocumentType" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlDocumentType" runat="server">
                                            </asp:ModalPopupExtender>
                                            <asp:Panel ID="pnlDocumentType" runat="server">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h3 style="text-align: right; margin: 0px;">
                                                                <asp:LinkButton ID="lnkCloseModal" CausesValidation="false" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseModal_Click"></asp:LinkButton></h3>
                                                        </div>
                                                        <div class="modal-body">
                                                            <asp:Panel ID="pnlRegisteration" runat="server" DefaultButton="lnkSave">
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="form-group">
                                                                            <label class="col-lg-4">الاسم</label>
                                                                            <div class="col-lg-8">
                                                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control border-input" placeholder="الأسم"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                                <asp:CustomValidator ID="cvName" runat="server" OnServerValidate="cvName_ServerValidate" ErrorMessage="موجود من قبل" SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave" ForeColor="Red">موجود من قبل</asp:CustomValidator>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </asp:Panel>
                                                        </div>
                                                        <div class="modal-footer">
                                                            <div class="col-md-12 text-left fix">
                                                                <asp:LinkButton ID="lnkSave" OnClick="lnkSave_Click" runat="server" CssClass="btn btn-sm btn-danger" ValidationObject="vgSave">حفظ</asp:LinkButton>
                                                                <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                                                <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

