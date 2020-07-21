<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-prerequisites.aspx.cs" Inherits="project_prerequisites" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeView" TagPrefix="dx" %>
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
                <div class="clearfix"></div>
                <div class="tabbable">
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_pre" class="tab-pane active">
                            <div class="clearfix"></div>
                            <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                                <Columns>
                                    <asp:TemplateField HeaderText="المهمة">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltrDone" runat="server" Visible=' <%# Eval("type").ToString()=="p"&& decimal.Parse(Eval("Progress").ToString())==decimal.Parse("100")&&Eval("DoneDate").ToString()!=string.Empty %>'><i class="fa fa-check-circle green"></i></asp:Literal><i class='<%# Eval("type").ToString()=="s"?"fa fa-refresh pink":string.Empty %>'></i> <%# Eval("name") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ImplementerType" HeaderText="مسئولية التنفيذ" SortExpression="ImplementerType" />
                                    <asp:BoundField DataField="StartDate" HeaderText="تاريخ البدء" DataFormatString="{0:d}" SortExpression="StartDate" />
                                    <asp:BoundField DataField="EndDate" HeaderText="تاريخ الإنتهاء" DataFormatString="{0:d}" SortExpression="EndDate" />
                                    <asp:BoundField DataField="ProgressCalculationType" HeaderText="طريقة احتساب الإنجاز" ItemStyle-CssClass="center" />
                                    <asp:TemplateField HeaderText="الوزن النسبي">
                                        <ItemTemplate>
                                            <%# decimal.Parse(Eval("relativeWeight").ToString()).ToString("G29")+" %" %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="نسبة الإنجاز">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkProgress" runat="server" Text='<%# Eval("Progress").ToString() +" %"%>' CommandArgument='<%# Eval("id").ToString()+";"+Eval("Progress") %>' Enabled='<%#Eval("progressCalculationTypeId").ToString()=="2" &&Eval("DoneDate").ToString()==string.Empty %>' Visible='<%#Eval("StartDate").ToString()!=string.Empty %>' OnCommand="lnkProgress_Command"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <span class='<%# Eval("DoneDate").ToString()!=string.Empty?string.Empty:"hidden" %>'><%# ((double.Parse(Eval("Progress").ToString())/double.Parse("100"))*double.Parse(Eval("relativeWeight").ToString())).ToString("N15").TrimEnd(new char[] { '0' }).TrimEnd(new char[] { '.' }) +" %"%></span>
                                            <asp:LinkButton ID="lnkDone" runat="server" Visible=' <%# CheckVisible(int.Parse(Eval("id").ToString())) && Eval("Progress").ToString() == "100" && Eval("DoneDate").ToString()==string.Empty &&Eval("progressCalculationTypeId").ToString()=="2" %>' CommandArgument='<%# Eval("id").ToString() %>' OnCommand="lnkDone_Command"><i class="fa fa-check-circle green"></i> تم</asp:LinkButton>
                                            <asp:LinkButton ID="btnApprove" Visible='<%# Eval("StartDate").ToString() == string.Empty %>' runat="server" CommandArgument='<%# Eval("id")+";"+Eval("type") %>' OnCommand="btnApprove_Command" CausesValidation="False" ToolTip="اعتماد">اعتماد</asp:LinkButton>
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
                            <asp:HiddenField ID="hdfObject" runat="server" />
                            <asp:ModalPopupExtender ID="mpeObject" TargetControlID="hdfObject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlObject" runat="server" DynamicServicePath="" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlObject" runat="server">
                                <div class="modal-dialog" id="modalObject">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkCloseModal" runat="server" CausesValidation="false" Text="إغلاق" class="pull-left" OnClick="lnkCloseModal_Click"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="col-lg-12 form-group">
                                                <label class="col-lg-4" for="">تاريخ البدء</label>
                                                <div class="col-lg-8">
                                                    <asp:TextBox ID="txtStartDateDay" runat="server" Width="28%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtStartDateMonth')"></asp:TextBox>
                                                    <asp:TextBox ID="txtStartDateMonth" runat="server" Width="28%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtStartDateYear')"></asp:TextBox>
                                                    <asp:TextBox ID="txtStartDateYear" runat="server" Width="40%" CssClass="form-control inline" placeholder="السنة" MaxLength="4"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvStartDateDay" runat="server" Display="Dynamic" ControlToValidate="txtStartDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvStartDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtStartDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvStartDateYear" runat="server" Display="Dynamic" ControlToValidate="txtStartDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtStartDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtStartDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtStartDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <div class="col-lg-12 form-group">
                                                <label class="col-lg-4" for="">تاريخ الإنتهاء</label>
                                                <div class="col-lg-8">
                                                    <asp:TextBox ID="txtEndDateDay" runat="server" Width="28%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtEndDateMonth')"></asp:TextBox>
                                                    <asp:TextBox ID="txtEndDateMonth" runat="server" Width="28%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtEndDateYear')"></asp:TextBox>
                                                    <asp:TextBox ID="txtEndDateYear" runat="server" Width="40%" CssClass="form-control inline" placeholder="السنة" MaxLength="4"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvEndDateDay" runat="server" Display="Dynamic" ControlToValidate="txtEndDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEndDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtEndDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rfvEndDateYear" runat="server" Display="Dynamic" ControlToValidate="txtEndDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEndDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtEndDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                                                    <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtEndDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="clearfix"></div>
                                            <div class="col-md-12 text-left fix">
                                                <asp:Button ID="btnSave" ValidationGroup="vgSave" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:HiddenField ID="hdfProgress" runat="server" />
                            <asp:ModalPopupExtender ID="mpeProgress" TargetControlID="hdfProgress" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlProgress" runat="server" DynamicServicePath="" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlProgress" runat="server">
                                <div class="modal-dialog" id="modalProgress">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkCloseProgress" CausesValidation="false" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseProgress_Click" meta:resourcekey="lnkCloseImplementersResource1"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="col-lg-12 form-group">
                                                <label class="col-lg-5" for="">نسبة الإنجاز</label>
                                                <div class="col-lg-6">
                                                    <asp:TextBox ID="txtProgress" runat="server" CssClass="form-control" TextMode="Number" onkeypress="return this.value.length<=99"></asp:TextBox>
                                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtProgress"
                                                        ErrorMessage="يجب ان يكون اكبر من 0 و اقل من او يساى 100" ForeColor="Red" MaximumValue="100" MinimumValue="0"
                                                        SetFocusOnError="True" Type="Double"></asp:RangeValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtProgress" ValidationGroup="vgSaveProgress" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="clearfix"></div>
                                            <div class="col-md-12 text-left fix">
                                                <asp:Button ID="btnSaveProgress" ValidationGroup="vgSaveProgress" runat="server" CssClass="btn btn-sm btn-danger" Text="حفــــــظ" OnClick="btnSaveProgress_Click" meta:resourcekey="btnSaveImplementersResource1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
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

