<%@ Page Title="SCU - المستخدمين" Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true" CodeFile="search.aspx.cs" Inherits="users" EnableEventValidation="false" %>

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
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <asp:Panel runat="server" ID="Panel1">
                    <div class="page-header">
                        <h1>البحث</h1>
                    </div>
                    <div class="form-horizontal">
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                باركود الملف
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtBoxCodeSrc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                عنوان الملف
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtBoxTitleSrc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                مرجع الملف
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtBoxReferenceSrc" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الإدارة المالكة
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlMinister" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                باركود الوثيقة
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDocCode" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                نوع الوثيقة
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlDocType" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                العنوان/ الموضوع
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtSubject" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الرقم
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtNumber" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                التاريخ من
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateFromSrch_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="yyyy-MM-dd"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                التاريخ الى
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="yyyy-MM-dd"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 top">الجهة المرسلة</label>
                            <div class="col-lg-7 topr ">
                                <asp:DropDownList ID="ddlSendingParty" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 top">الجهة المرسل اليها</label>
                            <div class="col-lg-7 topr ">
                                <asp:DropDownList ID="ddlReceivingParty" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 top">التصنيف العام</label>
                            <div class="col-lg-7 topr ">
                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvCategory" runat="server" Display="Dynamic" ControlToValidate="ddlCategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 top">التصنيف الفرعي</label>
                            <div class="col-lg-7 topr ">
                                <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvSubcategory" runat="server" Display="Dynamic" ControlToValidate="ddlSubcategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                حالة الملف
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlOperation" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الكلمات المفتاحية
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtKeywords" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 text-left form-group">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" />
                            <asp:LinkButton ID="lnkArchiving" runat="server" CssClass="btn btn-sm btn-success" PostBackUrl="~/project-files.aspx" Text="الأرشفة اليومية"></asp:LinkButton>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="clearfix"></div>
                <div class="header smaller lighter blue"></div>
                <div class="clearfix"></div>
                <div class="page-header">
                    <h1>نتائج البحث
            <asp:Label ID="lblResult" runat="server" Text="0"></asp:Label></h1>
                </div>
                <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="true" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                    <Columns>
                        <asp:TemplateField HeaderText="كود الوثيقة" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDetails" runat="server" PostBackUrl='<%#"doc-details.aspx?id=" +EncryptString.Encrypt(Eval("id").ToString()) %>'><%#Eval("barcode") %></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BoxBarcode" HeaderText="كود الملف" ItemStyle-CssClass="center" />
                        <asp:BoundField DataField="DocType" HeaderText="نوع الوثيقة" />
                        <asp:BoundField DataField="title" HeaderText="العنوان" />
                        <asp:BoundField DataField="date" HeaderText="التاريخ" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="reference" HeaderText="الرقم" />
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
                <div class="clearfix"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

