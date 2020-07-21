<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-users.aspx.cs" Inherits="project_users" meta:resourcekey="PageResource1" %>

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
                                            الإدارة 
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:DropDownList ID="ddlUnitSrc" CssClass="form-control" runat="server" meta:resourcekey="ddlUnitSrcResource1"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 form-group">
                                        <label class="col-lg-5 control-label">
                                            الهاتف المحمول
                                        </label>
                                        <div class="col-lg-7">
                                            <asp:TextBox ID="txtMobileSrc" CssClass="form-control" runat="server" meta:resourcekey="txtMobileSrcResource1"></asp:TextBox>
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
                                        <asp:Button ID="btnAssign" runat="server" CssClass="btn btn-sm btn-yellow " Text="تخصيص لادارة" OnClick="btnAssign_Click" CausesValidation="False" />
                                        <asp:Button ID="lnkWorkLicense" runat="server" CssClass="btn btn-sm btn-success " OnClientClick="return confirm('هل أنت متأكد من تغير الحالة؟');" Text="تم اصدار ترخيص عمل" OnClick="lnkWorkLicense_Click" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
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
                                    <asp:TemplateField HeaderText="تخصيص ادارة" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <label class="inline">
                                                <input id="chkUnitRow" type="checkbox" runat="server" class="ace input-lg" />
                                                <span class="lbl middle"></span>
                                            </label>
                                            <asp:HiddenField ID="hdfID" runat="server" Value='<%# Bind("id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ترخيص العمل" ItemStyle-CssClass="center">
                                        <ItemTemplate>
                                            <label class="inline">
                                                <input id="chkRow" visible='<%# bool.Parse(Eval("IsCheckBoxVisible").ToString())?true:false%>' type="checkbox" runat="server" class="ace input-lg" />
                                                <span class="lbl middle"></span>
                                            </label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="fullName" HeaderText="الاسم" SortExpression="fullName" />
                                    <asp:BoundField DataField="email" HeaderText="البريد الألكتورنى" SortExpression="email" />
                                    <asp:BoundField DataField="mobile" HeaderText="الهاتف المحمول" SortExpression="mobile" />
                                    <asp:BoundField DataField="Group" HeaderText="المجموعة" SortExpression="Group" />
                                    <asp:BoundField DataField="UnitStructure" HeaderText="الإدارة" SortExpression="UnitStructure" />
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
                                    <div class="modal-content modal-lg">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">الاسم بالكامل</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" meta:resourcekey="txtFullNameResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ValidationGroup="vgSubmit" ForeColor="Red" meta:resourcekey="rfvFullNameResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">الإدارة</label>
                                                        <div class="col-lg-8">
                                                            <asp:DropDownList ID="ddlUnit" CssClass="form-control" runat="server" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="clearfix"></div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">الهاتف المحمول</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtMobileReg" runat="server" CssClass="form-control" meta:resourcekey="txtMobileRegResource1"></asp:TextBox>
                                                            <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvMobileReg" runat="server" ControlToValidate="txtMobileReg" ValidationGroup="vgSubmit" ForeColor="Red" meta:resourcekey="rfvMobileRegResource1">*</asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cvMobileReg" runat="server" OnServerValidate="cvMobileReg_ServerValidate" ErrorMessage="موجود من قبل" SetFocusOnError="True" Display="Dynamic" ControlToValidate="txtMobileReg" ValidationGroup="vgSubmit" ForeColor="Red" meta:resourcekey="cvMobileRegResource1">موجود من قبل</asp:CustomValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">البريد الألكترونى</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtElecM" runat="server" CssClass="form-control" meta:resourcekey="txtEmailRegResource1"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="revElecM" SetFocusOnError="True" runat="server" ControlToValidate="txtElecM" ErrorMessage="تأكد من ادخال البريد الألكترونى بشكل صحيح" ValidationGroup="vgSubmit" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" meta:resourcekey="revEmailResource1">تأكد من ادخال البريد الألكترونى بشكل صحيح</asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator SetFocusOnError="True" ID="rfvEmailReg" runat="server" ControlToValidate="txtElecM" ValidationGroup="vgSubmit" ForeColor="Red" meta:resourcekey="rfvEmailRegResource1">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-6 form-group">
                                                        <label class="col-lg-4">المجموعة</label>
                                                        <div class="col-lg-8">
                                                            <asp:DropDownList ID="ddlGroup" CssClass="form-control" runat="server" meta:resourcekey="ddlGroupResource1"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="col-md-12 text-left fix">
                                                    <div class="clearfix"></div>
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSubmit" />
                                                    <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                                    <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <style>
                                .index {
                                    z-index: 101 !important;
                                }
                            </style>
                            <asp:HiddenField ID="hdfUnit" runat="server" />
                            <asp:ModalPopupExtender ID="mpeUnit" TargetControlID="hdfUnit" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlUnit" runat="server" DynamicServicePath="" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlUnit" runat="server" CssClass="index">
                                <div class="modal-dialog  modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkCloseUnit" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseUnit_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body" style="max-height: 500px; overflow-y: auto;">
                                            <div class="row">
                                                <div class="col-sm-7 icon-caret ace-icon tree-minus ">
                                                    <asp:TreeView ID="tvMenu" runat="server" OnSelectedNodeChanged="tvMenu_SelectedNodeChanged" ExpandDepth="0" ShowLines="True" OnTreeNodePopulate="tvMenu_TreeNodePopulate" meta:resourcekey="tvMenuResource1" />
                                                </div>
                                                <div class="col-lg-5">
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">
                                                            الأدارة
                                                        </label>
                                                        <div class="col-lg-12">
                                                            <asp:Label ID="lblUserUnit" runat="server" Text="-"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <div class="clearfix"></div>
                                                <asp:Button ID="btnAssigned" runat="server" CssClass="btn btn-sm btn-danger" Text="حفظ" OnClick="btnAssigned_Click" />
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
    <script src="App_Themes/js/select2.min.js"></script>
    <script>
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            $('.select2').select2();
        });
        function InitializeRequest(sender, args) {
        }

        function EndRequest(sender, args) {
            $('.select2').select2();
        }
    </script>
</asp:Content>

