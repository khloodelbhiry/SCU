<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="projects.aspx.cs" Inherits="projects" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                <asp:Panel runat="server" ID="Panel1">
                    <div class="page-header">
                        <h1>البحث</h1>
                    </div>
                    <div class="form-horizontal">
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                المشروع
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server" meta:resourcekey="txtDepartmentSrcResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الجهة
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtMinistrySrc" CssClass="form-control" runat="server" meta:resourcekey="txtDepartmentSrcResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الشركة المنفذة
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtCompanySrc" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                تاريخ البدء من
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtStartDateFrom" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDateFrom" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الى
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtEndDateTo" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDateTo" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                تاريخ التسليم من
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDeliveryStartDate" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDeliveryStartDate" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الى
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtDeliveryEndDate" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtDeliveryEndDate" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                            </div>
                        </div>
                        <div class="form-group col-md-6">
                            <label class="col-lg-5 control-label">الحالة</label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-12 text-left fix">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" meta:resourcekey="btnClearSearchResource1" />
                            <asp:Button ID="btnAdd" runat="server" CssClass=" btn btn-sm btn-danger " Text="إضافة جديدة" OnClick="btnAdd_Click" CausesValidation="False" meta:resourcekey="btnAddResource1" />
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
                        <asp:TemplateField HeaderText="المشروع">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDashboard" runat="server" PostBackUrl='<%#"project-prerequisites.aspx?id="+EncryptString.Encrypt(Eval("id").ToString())+"&g="+EncryptString.Encrypt(Eval("governmentalEntityId").ToString())+"&c="+EncryptString.Encrypt(Eval("companyId").ToString())  %>' Text='<%# Eval("name") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="governmentalEntity" HeaderText="الجهة" />
                        <asp:BoundField DataField="company" HeaderText="الشركة المنفذة" />
                        <asp:BoundField DataField="noOfPages" HeaderText="عدد الورق" />
                        <asp:BoundField DataField="startDate" HeaderText="تاريخ البدء" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="period" HeaderText="مدة المشروع" />
                        <asp:BoundField DataField="deliveryDate" HeaderText="تاريخ التسليم" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="status" HeaderText="الحالة" />
                        <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="تعديل"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Visible='<%# Eval("statusId").ToString()=="1" %>' OnClientClick="return confirm('هل أنت متأكد من الحذف؟');" CommandArgument='<%# Bind("id") %>' OnCommand="btnDelete_Command" CausesValidation="False" ToolTip="حذف"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                <asp:HiddenField ID="hdfProject" runat="server" />
                <asp:ModalPopupExtender ID="mpeProject" TargetControlID="hdfProject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlProject" runat="server" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlProject" runat="server">
                    <div class="modal-dialog">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click"></asp:LinkButton></h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">اسم المشروع</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="txtName" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                الجهة
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlGovernmentalEntity" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="ddlGovernmentalEntity" InitialValue="0" ForeColor="Red" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">الشركة المنفذة</label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCompany" runat="server" Display="Dynamic" ControlToValidate="ddlCompany" InitialValue="0" ForeColor="Red" ValidationGroup="vgSave"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                عدد الورق
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtPagesCount" TextMode="Number" CssClass="col-sm-4 form-control" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtPagesCount" ValidationGroup="vgSave" meta:resourcekey="rfvNameResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">تاريخ البدء</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtStartDateDay" runat="server" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged" Width="28%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtStartDateMonth')"></asp:TextBox>
                                                <asp:TextBox ID="txtStartDateMonth" runat="server" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged" Width="28%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtStartDateYear')"></asp:TextBox>
                                                <asp:TextBox ID="txtStartDateYear" runat="server" AutoPostBack="true" OnTextChanged="txtStartDate_TextChanged" Width="44%" CssClass="form-control inline" placeholder="السنة" MaxLength="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvStartDateDay" runat="server" Display="Dynamic" ControlToValidate="txtStartDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvStartDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtStartDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvStartDateYear" runat="server" Display="Dynamic" ControlToValidate="txtStartDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtStartDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtStartDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">مدة المشروع</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtPeriod" TextMode="Number" OnTextChanged="txtPeriod_TextChanged" Width="90%" AutoPostBack="true" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                                يوم
                                                <asp:RequiredFieldValidator ForeColor="Red" ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="txtPeriod" ValidationGroup="vgSave" meta:resourcekey="rfvAdminResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">تاريخ التسليم</label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtDeliveryDateDay" Enabled="false" runat="server" Width="28%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtDeliveryDateMonth')"></asp:TextBox>
                                                <asp:TextBox ID="txtDeliveryDateMonth" Enabled="false" runat="server" Width="28%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtDeliveryDateYear')"></asp:TextBox>
                                                <asp:TextBox ID="txtDeliveryDateYear" Enabled="false" runat="server" Width="44%" CssClass="form-control inline" placeholder="السنة" MaxLength="4"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txtDeliveryDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txtDeliveryDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="txtDeliveryDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDeliveryDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtDeliveryDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                                                <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtDeliveryDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <div class="clearfix"></div>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                        <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                        <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

