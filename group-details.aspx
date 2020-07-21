<%@ Page Title="SCU - المجموعات" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="group-details.aspx.cs" Inherits="group_details" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeView" TagPrefix="dx" %>
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
                <div class="col-lg-6 form-group">
                    <label class="col-lg-4 top">اسم المجموعة</label>
                    <div class="col-lg-8 topr calend">
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSubmit" ForeColor="Red" meta:resourcekey="rfvFullNameResource1">*</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-12 text-center fix">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSubmit" />
                </div>
                <div class="col-md-12">
                    <h2 class="header smaller lighter green m-t-0">الصلاحيات الأفتراضية</h2>
                    <table class="table  table-bordered table-hover">
                        <thead class="thead-color">
                            <tr class="tx-10 tx-spacing-1 tx-color-03 tx-uppercase height-130">
                                <th></th>
                                <asp:Repeater ID="rpActivities" runat="server">
                                    <ItemTemplate>
                                        <th class="large bolder text-info tx-large">
                                            <div><%#Eval("name") %></div>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpForms" runat="server" OnItemDataBound="rpForms_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td class="large bolder text-info tx-large"><%# Eval("name") %></td>
                                        <asp:HiddenField ID="hdfFormId" runat="server" Value='<%# Eval("id") %>' />
                                        <asp:Repeater ID="rpFormActivities" runat="server">
                                            <ItemTemplate>
                                                <td class="text-center">
                                                    <label class='<%# Eval("visible").ToString()=="True"?"inline":"hidden" %>'>
                                                        <input id="chkActivity" type="checkbox" runat="server" class="ace input-lg" checked='<%# Eval("isChecked").ToString()=="True" %>' />
                                                        <span class="lbl middle"></span>
                                                    </label>
                                                    <asp:HiddenField ID="hdfActivityId" runat="server" Value='<%# Eval("id") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <br />
                    <h2 class="header smaller lighter green">صلاحيات متطلبات التشغيل</h2>
                    <table class="table  table-bordered table-hover">
                        <thead class="thead-color">
                            <tr class="tx-10 tx-spacing-1 tx-color-03 tx-uppercase height-130">
                                <th></th>
                                <th class="large bolder text-info tx-large">تم</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpPrerequisites" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="large bolder text-info tx-large"><%# Eval("name") %>
                                            <asp:HiddenField ID="hdfPrerequisiteId" runat="server" Value='<%# Eval("id") %>' />
                                        </td>
                                        <td class="text-center">
                                            <label class="inline">
                                                <input id="chkPrerequisite" type="checkbox" runat="server" class="ace input-lg" checked='<%# Eval("isChecked").ToString()=="True" %>' />
                                                <span class="lbl middle"></span>
                                            </label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <h2 class="header smaller lighter green">صلاحيات الرقمنة</h2>
                    <table class="table  table-bordered table-hover">
                        <thead class="thead-color">
                            <tr class="tx-10 tx-spacing-1 tx-color-03 tx-uppercase height-130">
                                <th></th>
                                <asp:Repeater ID="rpActions" runat="server">
                                    <ItemTemplate>
                                        <th class="large bolder text-info tx-large">
                                            <div><%#Eval("name") %></div>
                                        </th>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rpStates" runat="server" OnItemDataBound="rpStates_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td class="large bolder text-info tx-large"><%# Eval("name") %></td>
                                        <asp:HiddenField ID="hdfStateId" runat="server" Value='<%# Eval("id") %>' />
                                        <asp:Repeater ID="rpStateActions" runat="server">
                                            <ItemTemplate>
                                                <td class="text-center">
                                                    <label class='<%# Eval("visible").ToString()=="True"?"inline":"hidden" %>'>
                                                        <input id="chkAction" type="checkbox" runat="server" class="ace input-lg" checked='<%# Eval("isChecked").ToString()=="True" %>' />
                                                        <span class="lbl middle"></span>
                                                    </label>
                                                    <asp:HiddenField ID="hdfActionId" runat="server" Value='<%# Eval("id") %>' />
                                                </td>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

