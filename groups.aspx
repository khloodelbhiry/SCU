<%@ Page Title="SCU - المجموعات" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="groups.aspx.cs" Inherits="groups" meta:resourcekey="PageResource1" %>

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
                <asp:Panel runat="server" ID="Panel1" meta:resourcekey="Panel1Resource1">
                    <div class="page-header">
                        <h1>البحث</h1>
                    </div>
                    <div class="form-horizontal">
                        <div class="col-lg-6 form-group">
                            <div class="col-lg-3 top">
                                الاسم
                            </div>
                            <div class="col-lg-9 top">
                                <asp:TextBox ID="txtNameSrch" CssClass="form-control border-input" runat="server" meta:resourcekey="txtNameSrchResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12 text-left fix">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger" Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False"/>
                            <asp:Button ID="btnNewSearch" runat="server" CssClass="btn btn-sm btn-grey" Text="بحــــث جديد" CausesValidation="False" OnClick="btnNewSearch_Click" />
                            <asp:Button ID="btn_NewGroup" runat="server" CssClass=" btn btn-sm btn-danger " Text="إضافة جديدة" OnClick="btn_NewGroup_Click" CausesValidation="False" />
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
                <asp:GridView ID="gdvData" runat="server" CssClass="table" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="اسم المجموعة" SortExpression="name" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <%# Eval("name") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnGroupEdit" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkEditGroup_Command" CausesValidation="False" ToolTip="تعديل" meta:resourcekey="btnGroupEditResource1"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                <asp:LinkButton ID="btnGroupDelete" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkDeleteGroup_Command" CausesValidation="False" ToolTip="حذف" meta:resourcekey="btnGroupDeleteResource1"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
                            </ItemTemplate>
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
                <asp:HiddenField ID="hdfGroupAdd" runat="server" />
                <asp:ModalPopupExtender ID="mpeGroupAdd" TargetControlID="hdfGroupAdd" PopupControlID="pnlGroupAdd" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlGroupAdd" runat="server" meta:resourcekey="pnlGroupAddResource1">
                    <div class="modal-dialog">
                        <div class="modal-content" style="width: 900px; max-height: 500px; overflow-y: scroll">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div id="div2" runat="server">
                                            <div class="col-md-12">
                                                <div class="col-lg-3">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass=" border-input" placeholder="اسم المجموعة"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvFullName" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgRegister" ForeColor="Red" meta:resourcekey="rfvFullNameResource1">*</asp:RequiredFieldValidator>

                                                        <div class="clearfix"></div>
                                                    </div>
                                                </div>
                                                <div class="col-md-9">
                                                    <div class="tabbable">
                                                        <ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
                                                            <li id="liDefault" class="active">
                                                                <a data-toggle="tab" href="#id_Default" aria-expanded="true">الصلاحيات الأفتراضية</a>
                                                            </li>
                                                            <li id="liPrerequisites">
                                                                <a data-toggle="tab" href="#id_Prerequisites" aria-expanded="true">صلاحيات متطلبات التشغيل</a>
                                                            </li>
                                                            <li id="liStatesTransition">
                                                                <a data-toggle="tab" href="#id_StatesTransition" aria-expanded="true">صلاحيات الرقمنة</a>
                                                            </li>
                                                        </ul>
                                                        <div class="tab-content">
                                                            <div id="id_Default" class="tab-pane active">
                                                                <dx:ASPxTreeView ID="tvPermissions" CheckNodesRecursive="True" Style="width: 900px;" AllowSelectNode="True" AllowCheckNodes="True" runat="server" meta:resourcekey="ASPXtvPermissions"></dx:ASPxTreeView>
                                                            </div>
                                                            <div id="id_Prerequisites" class="tab-pane">
                                                                <dx:ASPxTreeView ID="tvPrerequisites" CheckNodesRecursive="True" Style="width: 900px;" AllowSelectNode="True" AllowCheckNodes="True" runat="server"></dx:ASPxTreeView>
                                                            </div>
                                                            <div id="id_StatesTransition" class="tab-pane">
                                                                <dx:ASPxTreeView ID="tvStatesTransition" CheckNodesRecursive="True" Style="width: 900px;" AllowSelectNode="True" AllowCheckNodes="True" runat="server"></dx:ASPxTreeView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>

                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <div class="col-md-12 text-left fix">
                                    <div class="clearfix"></div>
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="lnkSubmitEditGroup_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

