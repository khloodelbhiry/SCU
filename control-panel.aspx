<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="control-panel.aspx.cs" Inherits="control_panel" %>

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
                <div class="col-xs-6 col-xs-offset-3">
                    <div class="widget-box no-padding-top">
                        <div class="widget-header widget-header-flat">
                            <h4 class="widget-title">الأعدادت</h4>
                        </div>
                        <div class="widget-body">
                            <div class="widget-main">
                                <asp:Repeater ID="rpData" runat="server">
                                    <ItemTemplate>
                                        <div class="col-lg-12 form-group">
                                            <asp:HiddenField ID="hdfId" runat="server" Value='<%# Eval("id") %>' />
                                            <label class="col-lg-5">
                                                <span class="bolder"><%# Eval("name") %></span>
                                            </label>
                                            <div class="col-lg-5">
                                                <asp:TextBox ID="txtValue" Text='<%# Eval("value") %>' CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="revInt" runat="server"
                                                    ControlToValidate="txtValue"
                                                    ErrorMessage="أرقام صحيحه فقط"
                                                    Text="أرقام صحيحه فقط"
                                                    ForeColor="Red" SetFocusOnError="true"
                                                    CssClass="validator"
                                                    ValidationExpression="^[0-9]*$"
                                                    Display="Dynamic"
                                                    ValidationGroup="vgSave" Enabled='<%# Eval("dataType").ToString()=="int" %>' />
                                            </div>
                                            <label class="col-lg-2">
                                                <span class="bolder blue"><%# Eval("unit") %></span>
                                            </label>
                                        </div>
                                        <div class="clearfix"></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div class="clearfix"></div>
                                <div class="header smaller lighter blue"></div>
                                <div class="clearfix"></div>
                                <div class="text-center">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

