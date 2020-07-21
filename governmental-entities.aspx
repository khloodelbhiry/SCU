<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="governmental-entities.aspx.cs" Inherits="governmental_entities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="updateProgress" DisplayAfter="0" runat="server" AssociatedUpdatePanelID="upnl">
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
            <div class="page-header">
                <h1>البحث</h1>
            </div>
            <div class="row">
                <div class="col-sm-12">

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
                                <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server" meta:resourcekey="txtDepartmentSrcResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="col-lg-5 control-label">الحالة</label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12 text-left fix">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger" Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" />
                        <asp:Button ID="btnNewSearch" runat="server" CssClass="btn btn-sm btn-grey" Text="بحــــث جديد" CausesValidation="False" OnClick="btnNewSearch_Click" />
                        <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" CausesValidation="False" CssClass="btn btn-sm btn-danger" Text="اضافة جديدة" />
                        <div class="clearfix"></div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="header smaller lighter blue"></div>
                    <div class="clearfix"></div>
                    <div class="page-header">
                        <h1>نتائج البحث
                            <asp:Label ID="lblResult" runat="server" Text="0"></asp:Label></h1>
                    </div>
                    <asp:GridView ID="gdvData" Font-Size="Large" CssClass="table table-hover table-responsive" runat="server" AllowPaging="True" AllowSorting="True" OnSorting="gdvData_Sorting" AutoGenerateColumns="False" PageSize="30" EmptyDataText="لا يوجد بيانات" OnPageIndexChanging="gdvData_PageIndexChanging" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="code" HeaderText="الكود " SortExpression="code" ItemStyle-CssClass="center" />
                            <asp:TemplateField HeaderText="الجهة " SortExpression="name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkUnitStructure" runat="server" PostBackUrl='<%# "unit-structure.aspx?id="+EncryptString.Encrypt(Eval("id").ToString()) %>' CausesValidation="False"><%# Eval("name") %></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="status" HeaderText="الحالة" />
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
            <asp:HiddenField ID="hdfObject" runat="server" />
            <asp:ModalPopupExtender ID="mpeObject" TargetControlID="hdfObject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlObject" runat="server" Enabled="True">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlObject" runat="server">
                <div class="modal-dialog">
                    <div class="modal-content modal-lg">
                        <div class="modal-header">
                            <h3 style="text-align: right; margin: 0px;">
                                <asp:LinkButton ID="lnkCloseModal" CausesValidation="False" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseModal_Click"></asp:LinkButton></h3>
                        </div>
                        <div class="modal-body">
                            <asp:Panel ID="pnlRegisteration" runat="server" DefaultButton="lnkSave">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-lg-4">الكود</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtCode" TextMode="Number" runat="server" CssClass="form-control border-input" onkeypress="return this.value.length<=3"></asp:TextBox>
                                                <asp:CustomValidator ID="cvCode" runat="server" ControlToValidate="txtCode" ValidationGroup="vgSave" ErrorMessage="الكود غير متاح" OnServerValidate="cvCode_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                                                <asp:RegularExpressionValidator ID="revCode" runat="server" ControlToValidate="txtCode" ErrorMessage="الحد الأقصى 4 أرقام" ValidationExpression="^[\s\S]{0,4}$" ValidationGroup="vgSave" ForeColor="Red"></asp:RegularExpressionValidator>
                                                <asp:RequiredFieldValidator ID="rfvCode" runat="server" Display="Dynamic" ControlToValidate="txtCode" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="col-lg-4">الجهة</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control border-input"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <hr>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

