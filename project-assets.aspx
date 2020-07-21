<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-assets.aspx.cs" Inherits="issues" meta:resourcekey="PageResource1" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
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
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <div class="tabbable">
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_issues" class="tab-pane active">
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
                                    <div class="col-md-12 text-left fix">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                        <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" meta:resourcekey="btnClearSearchResource1" />
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
                                    <asp:BoundField DataField="name" HeaderText="الأسم" SortExpression="name" />
                                    <asp:BoundField DataField="model" HeaderText="الموديل" SortExpression="model" />
                                    <asp:BoundField DataField="serialNo" HeaderText="رقم المسلسل" SortExpression="serialNo" />
                                    <asp:TemplateField HeaderText="الحالة">
                                        <ItemTemplate>
                                            <%# Eval("deliveryDate").ToString()==string.Empty?"قيد الأستلام":"مستلم" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelivered" runat="server" OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" OnCommand="lnkDelivered_Command" CommandArgument='<%# Eval("id") %>' Visible='<%# Eval("deliveryDate").ToString()==string.Empty %>'>تم الاستلام</asp:LinkButton>
                                            <asp:LinkButton ID="lnkDeliveryCert" runat="server" Visible='<%# Eval("deliveryDate").ToString()!=string.Empty %>'>شهادة الاستلام</asp:LinkButton>
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
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

