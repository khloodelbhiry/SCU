<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="categories.aspx.cs" Inherits="ministers_departments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="overlay">
                <div class="center-overlay">
                    <i class="ace-icon fa fa-spinner fa-spin orange bigger-300"></i>انتظر من فضلك ...
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="clearfix"></div>
            <div class="row" id="divMenu" runat="server">
                <div class="col-sm-5">
                    <div class="col-xs-12">
                        <asp:Panel ID="pnlSubmit" runat="server" DefaultButton="lnkSubmit">
                            <div class="form-horizontal widget-box">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right">الاسم </label>
                                    <div class="col-sm-8 ">
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" onkeypress="return this.value.length<=250"></asp:TextBox>
                                        <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvName" runat="server" ControlToValidate="txtName" ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ValidationGroup="vgSubmit" ErrorMessage="الاسم غير متاح" OnServerValidate="cvName_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right">متفرع من </label>
                                    <div class="col-sm-8 ">
                                        <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-12 text-left fix">
                                    <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="lnkSubmit_Click" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" ValidationGroup="vgSubmit"></asp:LinkButton>
                                    <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                    <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('هل أنت متأكد من الحذف ؟');" Visible="false" CausesValidation="false" CssClass="btn btn-sm btn-yellow"><i class="ace-icon fa fa-trash bigger-110"></i>حذف</asp:LinkButton>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-sm-7 icon-caret ace-icon tree-minus pull-left">
                    <asp:TreeView ID="tvMenu" Font-Size="Large" runat="server" OnSelectedNodeChanged="tvMenu_SelectedNodeChanged" ExpandDepth="0" PopulateNodesFromClient="true" ShowLines="true" ShowExpandCollapse="true" OnTreeNodePopulate="tvMenu_TreeNodePopulate" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
