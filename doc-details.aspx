<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="doc-details.aspx.cs" Inherits="docs_qa" %>

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
            <asp:HiddenField ID="hdfFile" runat="server" />
            <asp:HiddenField ID="hdfDocsQa" runat="server" />
            <asp:HiddenField ID="hdfNextState" runat="server" />
            <asp:HiddenField ID="hdfUnit" runat="server" />
            <asp:HiddenField ID="hdfDoc" runat="server" />
            <div class="page-content">
                <div class="col-md-5" id="divControls" runat="server">
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">باركود الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtFileCode" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">عنوان الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtFileTitle" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">مرجع الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtFileReference" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الإدارة المالكة</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtUnitStructure" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">باركود الوثيقة</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtDocCode" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">نوع الوثيقة</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlDocType" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvDocType" runat="server" Display="Dynamic" ControlToValidate="ddlDocType" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">العنوان/ الموضوع</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">التاريخ</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtDocumentDateDay" runat="server" Width="25%" CssClass="form-control inline" placeholder="اليوم" MaxLength="2" onkeyup="moveToNext(this,'txtDocumentDateMonth')"></asp:TextBox>
                            <asp:TextBox ID="txtDocumentDateMonth" runat="server" Width="30%" CssClass="form-control inline" placeholder="الشهر" MaxLength="2" onkeyup="moveToNext(this,'txtDocumentDateYear')"></asp:TextBox>
                            <asp:TextBox ID="txtDocumentDateYear" runat="server" Width="45%" CssClass="form-control inline" placeholder="السنة" MaxLength="4" meta:resourcekey="txtDocumentDateYearResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDocumentDateDay" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvDocumentDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvDocumentDateYear" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="rxDay" runat="server" ControlToValidate="txtDocumentDateDay" ErrorMessage="تاكد من اليوم" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|[12]\d|3[01])$"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator27" runat="server" ControlToValidate="txtDocumentDateMonth" ErrorMessage="تاكد من الشهر" ForeColor="#FF3300" ValidationExpression="^(0[1-9]|[1-9]|1[012])$"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator ValidationGroup="vgSave" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDocumentDateYear" ErrorMessage="تاكد من السنه" ForeColor="#FF3300" ValidationExpression="^((1[7-9]|2[0-9])[0-9]{2})$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الرقم</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtReference" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الجهة المرسلة</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlSendingParty" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الجهة المرسل اليها</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlReceivingParty" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div id="divKeywords" runat="server">
                        <div class="page-header">
                            <h4>الكلمات المفتاحية</h4>
                        </div>
                        <asp:GridView ID="gdvKeywords" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <Columns>
                                <asp:BoundField HeaderText="الكلمة المفتاحية" DataField="name" />
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
                    <div class="clearfix"></div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">التصنيف الرئيسي</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvCategory" runat="server" Display="Dynamic" ControlToValidate="ddlCategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">التصنيف الفرعي</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvSubcategory" runat="server" Display="Dynamic" ControlToValidate="ddlSubcategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div id="divReferences" runat="server">
                        <div class="page-header">
                            <h4>المراجع</h4>
                        </div>
                        <asp:GridView ID="gdvReferences" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <Columns>
                                <asp:BoundField HeaderText="المرجع" DataField="type" />
                                <asp:BoundField HeaderText="الوصف" DataField="description" />
                                <asp:BoundField HeaderText="الكود" DataField="code" />
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
                    <div class="clearfix"></div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الملاحظات</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-7">
                    <asp:Literal ID="ltrPDFEmbed" runat="server" meta:resourcekey="ltrPDFEmbedResource1" />
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

