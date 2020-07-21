<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-salary-effects.aspx.cs" Inherits="project_users" meta:resourcekey="PageResource1" %>

<%@ Register Src="UCs/project-users.ascx" TagName="WebControl2" TagPrefix="asp" %>
<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="App_Themes/css/select2.min.css" rel="stylesheet" />
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
                        <div id="id_users" class="tab-pane active">
                            <div class="col-xs-12">
                                <div class="tabbable tabs-right">
                                    <asp:WebControl2 ID="ucUsers" runat="server" />
                                    <div class="tab-content">
                                        <div id="home3" class="tab-pane active">
                                            <asp:Panel runat="server" ID="Panel1" meta:resourcekey="Panel1Resource1">
                                                <div class="page-header">
                                                    <h1>البحث</h1>
                                                </div>
                                                <div class="form-horizontal">
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            الاسم
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:TextBox ID="txtNameSrc" CssClass="form-control" runat="server" meta:resourcekey="txtNameSrcResource1"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            البريد الألكترونى
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:TextBox ID="txtElecMSrc" CssClass="form-control" runat="server" meta:resourcekey="txtElecMSrcResource1"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            التاريخ من
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:TextBox ID="txtDateFrom" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateFrom" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            الى
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:TextBox ID="txtDateTo" CssClass="form-control" runat="server" meta:resourcekey="txtCodeSrcResource1"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateTo" Format="MM-dd-yyyy" Enabled="True"></asp:CalendarExtender>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            المجموعة
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:DropDownList ID="ddlGroupSrc" runat="server" CssClass="form-control" meta:resourcekey="ddlStatusSrcResource1"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-5 control-label">
                                                            الحالة
                                                        </label>
                                                        <div class="col-lg-7">
                                                            <asp:DropDownList ID="ddlStatusSrc" runat="server" CssClass="form-control" meta:resourcekey="ddlStatusSrcResource1"></asp:DropDownList>
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
                                                    <asp:BoundField DataField="date" HeaderText="التاريخ" SortExpression="date" DataFormatString="{0:dd/MM/yyyy}" />
                                                    <asp:BoundField DataField="timeFrom" HeaderText="من" SortExpression="timeFrom" />
                                                    <asp:BoundField DataField="timeTo" HeaderText="الى" SortExpression="timeTo" />
                                                    <asp:BoundField DataField="fullName" HeaderText="الاسم" SortExpression="fullName" />
                                                    <asp:BoundField DataField="email" HeaderText="البريد الألكترونى" SortExpression="email" />
                                                    <asp:BoundField DataField="Group" HeaderText="المجموعة" SortExpression="Group" />
                                                    <asp:BoundField DataField="Status" HeaderText="الحالة" SortExpression="Status" />
                                                    <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="btnEdit_Command" ToolTip="تعديل" CausesValidation="False"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnDelete" runat="server" Visible='<%# Eval("statusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="btnDelete_Command" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                                            <div class="clearfix"></div>
                                            <asp:HiddenField ID="hdfUser" runat="server" />
                                            <asp:ModalPopupExtender ID="mpeUser" TargetControlID="hdfUser" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlUser" runat="server" DynamicServicePath="" Enabled="True">
                                            </asp:ModalPopupExtender>
                                            <asp:Panel ID="pnlUser" runat="server" meta:resourcekey="pnlUserResource1">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <h3 style="text-align: right; margin: 0px;">
                                                                <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                                        </div>
                                                        <div class="modal-body">
                                                            <div class="row">
                                                                <div class="col-lg-3 form-group"></div>
                                                                <div class="col-lg-3 form-group">
                                                                    <div class="radio pull-right">
                                                                        <label>
                                                                            <input name="rdUsers" value="0" type="radio" id="rdAll" runat="server" class="ace" checked />
                                                                            <span class="lbl">كل الموظفين  </span>
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-1 form-group">
                                                                    <div class="radio">
                                                                        <label>
                                                                            <input name="rdUsers" value="1" type="radio" id="rdByEmp" runat="server" class="ace" />
                                                                            <span class="lbl">
                                                                               
                                                                            </span>
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-5 form-group" style="padding-left:25px;">
                                                                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                                    <div class="col-lg-12 form-group">
                                                                    <label class="col-lg-3">السبب</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:TextBox ID="txtReason" Rows="2" TextMode="MultiLine" runat="server" CssClass="form-control" meta:resourcekey="txtFullNameResource1"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvReason" runat="server" ControlToValidate="txtReason" ValidationGroup="vgSave" ForeColor="Red" meta:resourcekey="rfvFullNameResource1">*</asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-12 form-group">
                                                                    <label class="col-lg-3">التاريخ</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:TextBox ID="txtDateDay" runat="server" Width="25%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtDateMonth')"></asp:TextBox>
                                                                        <asp:TextBox ID="txtDateMonth" runat="server" Width="30%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtDateYear')"></asp:TextBox>
                                                                        <asp:TextBox ID="txtDateYear" runat="server" Width="45%" CssClass="form-control inline" placeholder="السنة" MaxLength="4" meta:resourcekey="txtDateYearResource1"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvDateDay" runat="server" Display="Dynamic" ControlToValidate="txtDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvDateYear" runat="server" Display="Dynamic" ControlToValidate="txtDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="rxDay" runat="server" ControlToValidate="txtDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator27" runat="server" ControlToValidate="txtDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-3 form-group"></div>
                                                                <div class="col-lg-4 form-group">
                                                                    <label class="col-lg-3">من</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:TextBox ID="txtTimeFromH" runat="server" Width="50%" CssClass="form-control inline" placeholder="س" MaxLength="2" onkeyup="moveToNext(this,'txtTimeFromM')"></asp:TextBox>
                                                                        <asp:TextBox ID="txtTimeFromM" runat="server" Width="50%" CssClass="form-control inline" placeholder="د" MaxLength="2"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvTimeFromH" runat="server" Display="Dynamic" ControlToValidate="txtTimeFromH" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvTimeFromM" runat="server" Display="Dynamic" ControlToValidate="txtTimeFromM" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="revTimeFromH" runat="server" ControlToValidate="txtTimeFromH" ErrorMessage="تاكد من الساعة" ForeColor="#FF3300" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3])$"></asp:RegularExpressionValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="revTimeFromM" runat="server" ControlToValidate="txtTimeFromM" ErrorMessage="تاكد من الدقيقة" ForeColor="#FF3300" ValidationExpression="^([0-9]|[0-5][0-9])$"></asp:RegularExpressionValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-4 form-group">
                                                                    <label class="col-lg-3">الى</label>
                                                                    <div class="col-lg-9">
                                                                        <asp:TextBox ID="txtTimeToH" runat="server" Width="50%" CssClass="form-control inline" placeholder="س" MaxLength="2" onkeyup="moveToNext(this,'txtTimeToM')"></asp:TextBox>
                                                                        <asp:TextBox ID="txtTimeToM" runat="server" Width="50%" CssClass="form-control inline" placeholder="د" MaxLength="2"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvTimeToH" runat="server" Display="Dynamic" ControlToValidate="txtTimeToH" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RequiredFieldValidator ID="rfvTimeToM" runat="server" Display="Dynamic" ControlToValidate="txtTimeToM" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="revTimeToH" runat="server" ControlToValidate="txtTimeToH" ErrorMessage="تاكد من الساعة" ForeColor="#FF3300" ValidationExpression="^([0-9]|0[0-9]|1[0-9]|2[0-3])$"></asp:RegularExpressionValidator>
                                                                        <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="revTimeToM" runat="server" ControlToValidate="txtTimeToM" ErrorMessage="تاكد من الدقيقة" ForeColor="#FF3300" ValidationExpression="^([0-9]|[0-5][0-9])$"></asp:RegularExpressionValidator>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="modal-footer">
                                                                <div class="col-md-12 text-left fix">
                                                                    <div class="clearfix"></div>
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" />
                                                                    <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                                                    <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
      <script>
        function moveToNext(field, nextFieldID) {
            if (field.value.length >= field.maxLength) {
                $("input[name*='" + nextFieldID + "']").focus();
            }
          }
    </script>
</asp:Content>

