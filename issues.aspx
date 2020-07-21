<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="issues.aspx.cs" Inherits="issues" meta:resourcekey="PageResource1" %>

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
                <asp:Panel runat="server" ID="Panel1">
                    <div class="page-header">
                        <h1>البحث</h1>
                    </div>
                    <div class="form-horizontal">
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                درجة الخطورة
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlSeveritySrc" CssClass="form-control" runat="server" meta:resourcekey="ddlSeveritySrcResource1"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                بواسطة
                            </label>
                            <div class="col-lg-7">
                                <asp:TextBox ID="txtBySrc" runat="server" CssClass="form-control" meta:resourcekey="txtBySrcResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                عمر المشكلة
                            </label>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="ddlOperator" CssClass="form-control" runat="server" meta:resourcekey="ddlOperatorResource1">
                                    <asp:ListItem Value="-1" Text="اختر" Selected="True" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                    <asp:ListItem Value="equal" Text="=" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                    <asp:ListItem Value="larger" Text=">" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                    <asp:ListItem Value="smaller" Text="<" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-4">
                                <asp:TextBox ID="txtAge" TextMode="Number" runat="server" CssClass="form-control" meta:resourcekey="txtAgeResource1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-6 form-group">
                            <label class="col-lg-5 control-label">
                                الحالة
                            </label>
                            <div class="col-lg-7">
                                <asp:DropDownList ID="ddlStatusSrc" CssClass="form-control" runat="server" meta:resourcekey="ddlStatusSrcResource1"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-12 text-left fix">
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-danger " Text="بحــــث" OnClick="btnSearch_Click" CausesValidation="False" />
                            <asp:Button ID="btnClearSearch" runat="server" CssClass=" btn btn-sm btn-grey" Text="بحــــث جديد" OnClick="btnClearSearch_Click" CausesValidation="False" />
                            <asp:Button ID="btnAdd" runat="server" CssClass=" btn btn-sm btn-danger " Text="إضافة جديدة" OnClick="btnAdd_Click" CausesValidation="False" />
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
                <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" />
                        <asp:BoundField DataField="issue1" HeaderText="نص المشكلة" SortExpression="issue1" />
                        <asp:BoundField DataField="IssueSeverity" HeaderText="درجة الخطورة" SortExpression="IssueSeverity" />
                        <asp:BoundField DataField="IssueStatus" HeaderText="الحالة" SortExpression="IssueStatus" />
                        <asp:BoundField DataField="date" HeaderText="التاريخ" SortExpression="date" />
                        <asp:BoundField DataField="fullName" HeaderText="بواسطة" SortExpression="fullName" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkNotes" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="lnkNotes_Command"><%# "( "+Eval("NotesCount")+" ) ملاحظة" %></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkIssueAttachments" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="lnkIssueAttachments_Command"><%# "( "+Eval("AttachmentCount")+" ) المرفقات" %></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnEdit_Command" CausesValidation="False" ToolTip="تعديل"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Bind("id") %>' Visible='<%# Eval("statusId").ToString()=="1" %>' OnCommand="btnDelete_Command" CausesValidation="False" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                <asp:HiddenField ID="hdfIssue" runat="server" />
                <asp:ModalPopupExtender ID="mpeIssue" TargetControlID="hdfIssue" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlIssue" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlIssue" runat="server" meta:resourcekey="pnlIssueResource1">
                    <div class="modal-dialog">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="btnClose" runat="server" Text="إغلاق" class="pull-left" OnClick="btnClose_Click" meta:resourcekey="btnCloseResource1"></asp:LinkButton>
                                </h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                المشكلة
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtIssue" CssClass="form-control" TextMode="MultiLine" Rows="5" runat="server" meta:resourcekey="txtIssueResource1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvIssue" runat="server" Display="Dynamic" ControlToValidate="txtIssue" ValidationGroup="vgSave" meta:resourcekey="rfvIssueResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                درجة الخطورة
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="form-control" meta:resourcekey="ddlSeverityResource1"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSeverity" runat="server" Display="Dynamic" ControlToValidate="ddlSeverity" InitialValue="0" ValidationGroup="vgSave" meta:resourcekey="rfvSeverityResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                            <div id="divAge" runat="server" visible="False">
                                                <label class="col-lg-4">
                                                    عمر المشكلة
                                                </label>
                                                <div class="col-lg-8 text-right">
                                                    <asp:Label ID="lblAge" runat="server" meta:resourcekey="lblAgeResource1"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <div class="clearfix"></div>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" ValidationGroup="vgSave" meta:resourcekey="btnSaveResource1" />
                                        <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" meta:resourcekey="btnApproveResource1" />
                                        <asp:Button ID="btnCloseIssue" runat="server" CssClass="btn btn-sm btn-danger" Text="اغلاق" OnClick="btnCloseIssue_Click" Visible="False" meta:resourcekey="btnCloseIssueResource1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:HiddenField ID="hdfIssueAttachment" runat="server" />
                <asp:ModalPopupExtender ID="mpeIssueAttachment" TargetControlID="hdfIssueAttachment" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlIssueAttachment" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlIssueAttachment" runat="server" meta:resourcekey="pnlIssueAttachmentResource1">
                    <div class="modal-dialog">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="lnkCloseIssueAttachment" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseIssueAttachment_Click" meta:resourcekey="lnkCloseIssueAttachmentResource1"></asp:LinkButton>
                                </h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <asp:GridView ID="gdvIssueAttachment" CssClass="table" runat="server" AllowSorting="True" OnSorting="gdvIssueAttachment_Sorting" OnPageIndexChanging="gdvIssueAttachment_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvIssueAttachmentResource1">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" />
                                            <asp:TemplateField HeaderText="الملف" SortExpression="fileName">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkIssueFile" runat="server" PostBackUrl='<%# Eval("fileName") %>'><%# Eval("fileName") %></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="date" HeaderText="التاريخ" SortExpression="date" />
                                            <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDeleteIssueAttachment" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnDeleteIssueAttachment_Command" CausesValidation="False" ToolTip="حذف"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                الملف
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:FileUpload ID="fuIssueAttachment" runat="server" meta:resourcekey="fuIssueAttachmentResource1" />
                                                <asp:RequiredFieldValidator ID="rfvIssueNote" runat="server" Display="Dynamic" ControlToValidate="fuIssueAttachment" ValidationGroup="vgIssueAttachment" meta:resourcekey="rfvIssueNoteResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <div class="clearfix"></div>
                                        <asp:Button ID="btnSaveIssueAttachment" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveIssueAttachment_Click" ValidationGroup="vgIssueAttachment" meta:resourcekey="btnSaveIssueAttachmentResource1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:HiddenField ID="hdfNotes" runat="server" />
                <asp:ModalPopupExtender ID="mpeNotes" TargetControlID="hdfNotes" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlNotes" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlNotes" runat="server" meta:resourcekey="pnlNotesResource1">
                    <div class="modal-dialog">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="lnkCloseNotes" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseNotes_Click" meta:resourcekey="lnkCloseNotesResource1"></asp:LinkButton>
                                </h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <asp:GridView ID="gdvNotes" CssClass="table" runat="server" AllowSorting="True" OnSorting="gdvNotes_Sorting" OnPageIndexChanging="gdvNotes_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvNotesResource1">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" />
                                            <asp:BoundField DataField="note" HeaderText="الملاحظة" SortExpression="note" />
                                            <asp:BoundField DataField="status" HeaderText="الحالة" SortExpression="status" />
                                            <asp:BoundField DataField="date" HeaderText="التاريخ" SortExpression="date" />
                                            <asp:BoundField DataField="fullName" HeaderText="بواسطة" SortExpression="fullName" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNotesAttachments" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="lnkNotesAttachments_Command" meta:resourcekey="lnkNotesAttachmentsResource1"><%# "( "+Eval("AttachmentCount")+" ) المرفقات" %></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnEditNote" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnEditNote_Command" CausesValidation="False" ToolTip="تعديل" meta:resourcekey="btnEditNoteResource1"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
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
                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                الملاحظة
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txtNotes" CssClass="form-control" TextMode="MultiLine" Rows="5" runat="server" meta:resourcekey="txtNotesResource1"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvNotes" runat="server" Display="Dynamic" ControlToValidate="txtNotes" ValidationGroup="vgNotes" meta:resourcekey="rfvNotesResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <div class="clearfix"></div>
                                        <asp:Button ID="btnSaveNotes" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveNotes_Click" ValidationGroup="vgNotes" meta:resourcekey="btnSaveNotesResource1" />
                                        <asp:Button ID="btnApproveNotes" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApproveNotes_Click" CausesValidation="False" Visible="False" meta:resourcekey="btnApproveNotesResource1" />
                                        <asp:Button ID="btnFreezeNotes" runat="server" CssClass="btn btn-sm btn-danger" Text="تجميد" OnClick="btnFreezeNotes_Click" CausesValidation="False" Visible="False" meta:resourcekey="btnFreezeNotesResource1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:HiddenField ID="hdfNotesAttachments" runat="server" />
                <asp:ModalPopupExtender ID="mpeNotesAttachments" TargetControlID="hdfNotesAttachments" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlNotesAttachments" runat="server" DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlNotesAttachments" runat="server" meta:resourcekey="pnlNotesAttachmentsResource1">
                    <div class="modal-dialog">
                        <div class="modal-content modal-lg">
                            <div class="modal-header">
                                <h3 style="text-align: right; margin: 0px;">
                                    <asp:LinkButton ID="lnkCloseNotesAttachments" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseNotesAttachments_Click" meta:resourcekey="lnkCloseNotesAttachmentsResource1"></asp:LinkButton>
                                </h3>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <asp:GridView ID="gdvNotesAttachments" CssClass="table" runat="server" AllowSorting="True" OnSorting="gdvNotesAttachments_Sorting" OnPageIndexChanging="gdvNotesAttachments_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvNotesAttachmentsResource1">
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" />
                                            <asp:TemplateField HeaderText="الملف" SortExpression="fileName" meta:resourcekey="TemplateFieldResource22">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNoteFile" runat="server" PostBackUrl='<%# Eval("fileName") %>' meta:resourcekey="lnkNoteFileResource1"><%# Eval("fileName") %></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="date" HeaderText="التاريخ" SortExpression="date" />
                                            <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDeleteNoteAttachment" runat="server" CommandArgument='<%# Bind("id") %>' OnCommand="btnDeleteNoteAttachment_Command" CausesValidation="False" ToolTip="حذف" meta:resourcekey="btnDeleteNoteAttachmentResource1"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                                </div>
                                <div class="clearfix"></div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">
                                        <div class="col-lg-6 form-group">
                                            <label class="col-lg-4">
                                                الملف
                                            </label>
                                            <div class="col-lg-8">
                                                <asp:FileUpload ID="fuNotesAttachments" runat="server" meta:resourcekey="fuNotesAttachmentsResource1" />
                                                <asp:RequiredFieldValidator ID="rfvNotesAttachments" runat="server" Display="Dynamic" ControlToValidate="fuNotesAttachments" ValidationGroup="vgNotesAttachments" meta:resourcekey="rfvNotesAttachmentsResource1">*</asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <div class="clearfix"></div>
                                        <asp:Button ID="btnSaveNotesAttachments" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveNotesAttachments_Click" ValidationGroup="vgNotesAttachments" meta:resourcekey="btnSaveNotesAttachmentsResource1" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveNotesAttachments" />
            <asp:PostBackTrigger ControlID="btnSaveIssueAttachment" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

