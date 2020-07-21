<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="unit-structure.aspx.cs" Inherits="unit_structure" %>

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
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="page-content">
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                        <li class="active" id="liStructure">
                            <a data-toggle="tab" href="#id_structure" aria-expanded="true">الهيكل التنظيمي </a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div id="id_structure" class="tab-pane active">
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
                                                        <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvName" runat="server" ControlToValidate="txtName" ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvName" runat="server" ControlToValidate="txtName" ValidationGroup="vgSubmit" ErrorMessage="الاسم غير متاح" OnServerValidate="cvName_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-right">الكود </label>
                                                    <div class="col-sm-8 ">
                                                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" onkeypress="return this.value.length<=4"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revCode" runat="server" ValidationExpression="^[A-Z0-9]+$" ErrorMessage="مسموح فقط بأحرف انجليزية كبيره او ارقام" ForeColor="Red" ControlToValidate="txtCode"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvCode" runat="server" ControlToValidate="txtCode" ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="cvCode" runat="server" ControlToValidate="txtCode" ValidationGroup="vgSubmit" ErrorMessage="الكود غير متاح" OnServerValidate="cvCode_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-right">متفرع من </label>
                                                    <div class="col-sm-8 ">
                                                        <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-right">النوع </label>
                                                    <div class="col-sm-8 ">
                                                        <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvType" runat="server" ControlToValidate="ddlType" InitialValue="0" ValidationGroup="vgSubmit" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label no-padding-right">المستوى </label>
                                                    <div class="col-sm-8 ">
                                                        <asp:TextBox ID="txtLvl" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revLvl" ControlToValidate="txtLvl" runat="server" ErrorMessage="مسوح بأرقام فقط" ValidationExpression="^[0-9]+$" ValidationGroup="vgSubmit" ForeColor="Red"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 text-left fix">
                                                    <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="lnkSubmit_Click" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" ValidationGroup="vgSubmit"></asp:LinkButton>
                                                    <asp:Button ID="btnClear" runat="server" CssClass="btn btn-sm btn-grey" Text="جديد" OnClick="btnClear_Click" CausesValidation="False" />
                                                    <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" OnClientClick="return confirm('هل أنت متأكد من الحذف ؟');" Visible="False" CausesValidation="False" CssClass="btn btn-sm btn-yellow"><i class="ace-icon fa fa-trash bigger-110"></i>حذف</asp:LinkButton>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>
                                <div class="col-sm-7 icon-caret ace-icon tree-minus pull-left">
                                    <asp:TreeView ID="tvMenu" runat="server" Font-Size="Large" OnSelectedNodeChanged="tvMenu_SelectedNodeChanged" ExpandDepth="0" ShowLines="True" OnTreeNodePopulate="tvMenu_TreeNodePopulate" />
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
