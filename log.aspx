<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="DocumentTypes" %>

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
            <div class="page-header">
                <h1>البحث</h1>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <div class="form-horizontal">
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                التاريخ من
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateFrom" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الى
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateTo" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                العملية
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlOperation" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-12 text-left form-group">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger" Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" />
                        <asp:Button ID="btnNewSearch" runat="server" CssClass="btn btn-sm btn-grey" Text="بحــــث جديد" CausesValidation="False" OnClick="btnNewSearch_Click" />
                        <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="btn btn-sm btn-success" OnClick="lnkUpdate_Click" CausesValidation="False"><i class="fa fa-refresh"></i> تحديث سجل الحالات</asp:LinkButton>
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
                            <asp:BoundField DataField="operation" HeaderText="العملية " SortExpression="operation" />
                            <asp:BoundField DataField="occurredAt" HeaderText="التاريخ " SortExpression="occurredAt" />
                            <asp:BoundField DataField="fullName" HeaderText="بواسطة " SortExpression="fullName" />
                            <asp:BoundField DataField="description" HeaderText="الوصف " SortExpression="description" />
                            <asp:BoundField DataField="fieldName" HeaderText="الحقل " SortExpression="fieldName" />
                            <asp:BoundField DataField="oldValue" HeaderText="القيمة القديمة " SortExpression="oldValue" />
                            <asp:BoundField DataField="newValue" HeaderText="القيمة الجديدة " SortExpression="newValue" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

