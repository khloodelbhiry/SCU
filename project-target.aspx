<%@ Page Title="" Language="C#" MasterPageFile="MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="project-target.aspx.cs" Inherits="issues" meta:resourcekey="PageResource1" %>

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
                            <asp:GridView ID="gdvData" CssClass="table" runat="server" PageSize="50" AllowSorting="True" OnSorting="gdvData_Sorting" OnPageIndexChanging="gdvData_PageIndexChanging" Font-Size="Large" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" meta:resourcekey="gdvDataResource1">
                                <Columns>
                                    <asp:BoundField DataField="name" HeaderText="المهمة" SortExpression="name" />
                                    <asp:BoundField DataField="startDate" HeaderText="تاريخ التارجت" DataFormatString="{0:d}" SortExpression="startDate" />
                                    <asp:TemplateField HeaderText="التارجت">
                                        <ItemTemplate>
                                            <%# Eval("Target") %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="مكافأة الوصول للتارجت">
                                        <ItemTemplate>
                                            <%# Eval("reachTarget").ToString()!=string.Empty?decimal.Parse(Eval("reachTarget").ToString()).ToString("G29"):string.Empty %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="مكافأة الزيادة على التارجت">
                                        <ItemTemplate>
                                            <%# Eval("overTarget").ToString()!=string.Empty?decimal.Parse(Eval("overTarget").ToString()).ToString("G29"):string.Empty %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="سعر الخطأ">
                                        <ItemTemplate>
                                            <%# Eval("error").ToString()!=string.Empty?decimal.Parse(Eval("error").ToString()).ToString("G29"):string.Empty %>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAddTarget" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkAddTarget_Command" CausesValidation="False" ToolTip="اضافة تارجت"><i class="fa fa-plus fa-2x green"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkTarget" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkTarget_Command" CausesValidation="False" ToolTip="التارجت"><i class="fa fa-calculator fa-2x purple"></i></asp:LinkButton>
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
                            <asp:HiddenField ID="hdfObject" runat="server" />
                            <asp:ModalPopupExtender ID="mpeObject" TargetControlID="hdfObject" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlObject" runat="server" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlObject" runat="server">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkCloseModal" CausesValidation="False" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseModal_Click"></asp:LinkButton></h3>
                                        </div>
                                        <div class="modal-body">
                                            <asp:Panel ID="pnl" runat="server" DefaultButton="lnkSave">
                                                <div class="row">
                                                    <div class="col-lg-8 form-group">
                                                        <label class="col-lg-6">التارجت</label>
                                                        <div class="col-lg-6">
                                                            <asp:TextBox ID="txtCount" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvCount" runat="server" Display="Dynamic" ControlToValidate="txtCount" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-4 form-group">
                                                        <div class="radio">
                                                            <label>
                                                                <input name="rdTime" value="d" type="radio" id="rdDay" runat="server" class="ace" checked />
                                                                <span class="lbl">  اليوم  </span>
                                                            </label>
                                                        </div>
                                                        <div class="radio">
                                                            <label>
                                                                <input name="rdTime" value="h" type="radio" id="rdHour" runat="server" class="ace" />
                                                                <span class="lbl">  الساعة </span>
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">تاريخ التارجت</label>
                                                        <div class="col-lg-8">
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
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">مكافأة الوصول للتارجت</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtReachTarget" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvReachTarget" runat="server" Display="Dynamic" ControlToValidate="txtReachTarget" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">مكافأة الزيادة على التارجت</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtOverTarget" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvOverTarget" runat="server" Display="Dynamic" ControlToValidate="txtOverTarget" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="col-lg-12 form-group">
                                                        <label class="col-lg-4">سعر الخطأ</label>
                                                        <div class="col-lg-8">
                                                            <asp:TextBox ID="txtError" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvError" runat="server" Display="Dynamic" ControlToValidate="txtError" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="modal-footer">
                                            <div class="col-md-12 text-left fix">
                                                <asp:LinkButton ID="lnkSave" OnClick="btnSave_Click" runat="server" CssClass="btn btn-sm btn-danger" ValidationGroup="vgSave">حفظ</asp:LinkButton>
                                                <asp:Button ID="btnApprove" runat="server" CssClass="btn btn-sm btn-success" Text="اعتماد" OnClick="btnApprove_Click" Visible="False" />
                                                <asp:Button ID="btnFreeze" runat="server" CssClass="btn btn-sm btn-default" Text="تجميد" OnClick="btnFreeze_Click" Visible="False" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:HiddenField ID="hdfTarget" runat="server" />
                            <asp:ModalPopupExtender ID="mpeTarget" TargetControlID="hdfTarget" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlTarget" runat="server" Enabled="True">
                            </asp:ModalPopupExtender>
                            <asp:Panel ID="pnlTarget" runat="server">
                                <div class="modal-dialog">
                                    <div class="modal-content modal-lg">
                                        <div class="modal-header">
                                            <h3 style="text-align: right; margin: 0px;">
                                                <asp:LinkButton ID="lnkCloseTarget" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseTarget_Click"></asp:LinkButton>
                                            </h3>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row">
                                                <asp:GridView ID="gdvTarget" CssClass="table" runat="server" OnPageIndexChanging="gdvTarget_PageIndexChanging" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                                                    <Columns>
                                                        <asp:BoundField DataField="startDate" HeaderText="تاريخ التارجت" DataFormatString="{0:d}" />
                                                        <asp:TemplateField HeaderText="التارجت">
                                                            <ItemTemplate>
                                                                <%# Eval("Target") %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="مكافأة الوصول للتارجت">
                                                            <ItemTemplate>
                                                                <%# decimal.Parse(Eval("reachTarget").ToString()).ToString("G29") %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="مكافأة الزيادة على التارجت">
                                                            <ItemTemplate>
                                                                <%# decimal.Parse(Eval("overTarget").ToString()).ToString("G29") %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="سعر الخطأ">
                                                            <ItemTemplate>
                                                                <%# decimal.Parse(Eval("error").ToString()).ToString("G29")  %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="status" HeaderText="الحالة" />
                                                        <asp:TemplateField HeaderText="نشط؟">
                                                            <ItemTemplate>
                                                                <label class="inline">
                                                                    <input id="chkRow" onclick='<%# "fnActivation("+Eval("id")+")" %>' checked='<%# Eval("isActive")%>' visible='<%# bool.Parse(Eval("IsCheckBoxVisible").ToString())?true:false%>' type="checkbox" runat="server" class="ace input-lg" />
                                                                    <span class="lbl middle"></span>
                                                                </label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="تعديل" ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' OnCommand="lnkEdit_Command" ToolTip="تعديل" CausesValidation="False"><img src="App_Themes/images/edit.png" width="25" height="25" /></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="حذف" ItemStyle-Width="10">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" Visible='<%# Eval("statusId").ToString()=="1" %>' CommandArgument='<%# Eval("id") %>' OnCommand="lnkDelete_Command" ToolTip="حذف" OnClientClick="return confirm('هل أنت متأكد من اتمام الحذف؟');"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
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
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfId" runat="server" />
            <asp:Button ID="btnActivation" runat="server" CssClass="hidden" OnClick="btnActivation_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
      <script>
        function moveToNext(field, nextFieldID) {
            if (field.value.length >= field.maxLength) {
                $("input[name*='" + nextFieldID + "']").focus();
            }
          }
          function fnActivation(id) {
              if (confirm('هل انت متاكد من تغير الحالة؟')) {
                  $('#<%= hdfId.ClientID %>').val(id);
                  var clickButton = document.getElementById("<%= btnActivation.ClientID %>");
                  clickButton.click();
              }
          }
    </script>
</asp:Content>

