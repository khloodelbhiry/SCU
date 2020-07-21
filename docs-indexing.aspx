<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="docs-indexing.aspx.cs" Inherits="docs_indexing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=13.1.19.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

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
            <asp:HiddenField ID="hdfFile" runat="server" />
            <asp:HiddenField ID="hdfDocsIndexing" runat="server" />
            <asp:HiddenField ID="hdfNextState" runat="server" />
            <asp:HiddenField ID="hdfDoc" runat="server" />
            <div class="page-content">
                <div class="col-md-5" id="divControls" runat="server">
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">عنوان الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtFileTitle" Enabled="false" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtTitleResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">مرجع الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtFileReference" Enabled="false" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtReferenceResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">باركود الوثيقة</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtDocCode" Enabled="false" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
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
                        <label class="col-lg-4 top bolder">العنوان/الموضوع</label>
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
                            <asp:RequiredFieldValidator Enabled="false" ID="rfvDocumentDateDay" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateDay" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator Enabled="false" ID="rfvDocumentDateMonth" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateMonth" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator Enabled="false" ID="rfvDocumentDateYear" runat="server" Display="Dynamic" ControlToValidate="txtDocumentDateYear" ValidationGroup="vgSave" ForeColor="Red">*</asp:RequiredFieldValidator>
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
                            <asp:DropDownList ID="ddlSendingParty" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الجهة المرسل اليها</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlReceivingParty" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                   <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">الكلمة المفتاحية</label>
                        <div class="col-lg-7 topr ">
                            <asp:DropDownList ID="ddlKeywords" AutoPostBack="true" OnSelectedIndexChanged="ddlKeywords_SelectedIndexChanged" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <div class="col-lg-1 topr ">
                            <asp:LinkButton ID="lnkAddKeyword" runat="server" CssClass="" OnClick="lnkAddKeyword_Click"><i class="fa fa-plus"></i></asp:LinkButton>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top"></label>
                        <div class="col-lg-8 topr ">
                            <asp:GridView ID="gdvKeywords" CssClass="table" runat="server" Font-Size="Small" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                                <Columns>
                                    <asp:BoundField HeaderText="الكلمة المفتاحية" DataField="name" />
                                    <asp:TemplateField HeaderText="" ItemStyle-Width="10">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDeleteKeyword" runat="server" OnClientClick="return confirm('هل أنت متأكد من الحذف؟');" CommandArgument='<%# Bind("id") %>' OnCommand="btnDeleteKeyword_Command" CausesValidation="False" ToolTip="حذف"><img src="App_Themes/images/delete.png" width="15" height="15" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
                    <div class="clearfix"></div>
                    <hr />
                    <div class="clearfix"></div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">التصنيف الرئيسي</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator Enabled="false" ForeColor="Red" ID="rfvCategory" runat="server" Display="Dynamic" ControlToValidate="ddlCategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">التصنيف الفرعي</label>
                        <div class="col-lg-8 topr ">
                            <asp:DropDownList ID="ddlSubcategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
                            <asp:RequiredFieldValidator Enabled="false" ForeColor="Red" ID="rfvSubcategory" runat="server" Display="Dynamic" ControlToValidate="ddlSubcategory" InitialValue="0" ValidationGroup="vgSave">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div runat="server" id="divRef" visible="false">
                        <div class="page-header">
                            <h4>المراجع</h4>
                        </div>
                        <div class="col-lg-12 form-group">
                            <label class="col-lg-4 top">نوع المرجع</label>
                            <div class="col-lg-8 topr ">
                                <asp:DropDownList ID="ddlReference" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvBusinessReference" runat="server" InitialValue="0" Display="Dynamic" ControlToValidate="ddlReference" ValidationGroup="vgReference">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <label class="col-lg-4 top">الوصف</label>
                            <div class="col-lg-8 topr ">
                                <asp:TextBox ID="txtDescription" Rows="3" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvDescription" runat="server" Display="Dynamic" ControlToValidate="txtDescription" ValidationGroup="vgReference">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <label class="col-lg-4 top">الكود</label>
                            <div class="col-lg-6 topr ">
                                <asp:TextBox ID="txtReferenceCode" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ForeColor="Red" ID="rfvReferenceCode" runat="server" Display="Dynamic" ControlToValidate="txtReferenceCode" ValidationGroup="vgReference">*</asp:RequiredFieldValidator>
                            </div>
                            <div class="col-lg-2">
                                <asp:Button ID="btnAddReference" runat="server" ValidationGroup="vgReference" CssClass="btn btn-sm btn-yellow" Text="اضافة" OnClick="btnAddReference_Click" />
                            </div>
                        </div>
                        <asp:GridView ID="gdvReferences" CssClass="table" runat="server" Font-Size="Medium" Width="100%" AllowPaging="True" AutoGenerateColumns="False" SkinID="Professional" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                            <Columns>
                                <asp:BoundField HeaderText="المرجع" DataField="type" />
                                <asp:BoundField HeaderText="الوصف" DataField="description" />
                                <asp:BoundField HeaderText="الكود" DataField="code" />
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDeleteReference" runat="server" OnClientClick="return confirm('هل أنت متأكد من الحذف؟');" CommandArgument='<%# Bind("id") %>' OnCommand="btnDeleteReference_Command" CausesValidation="False" ToolTip="حذف"><img src="App_Themes/images/delete.png" width="25" height="25" /></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
                    <hr style="margin-bottom: 0px; margin-top: 0px;" />
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top bolder">الملاحظات</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-md-12 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12 text-left fix">
                        <div class="clearfix"></div>
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="تمت فهرسة الوثيقة" OnClick="btnSave_Click" ValidationGroup="vgSave" />
                    </div>
                    <asp:HiddenField ID="hdfKeyword" runat="server" />
                    <asp:ModalPopupExtender ID="mpeKeyword" TargetControlID="hdfKeyword" BackgroundCssClass="modal modal-st-erorr" PopupControlID="pnlKeyword" runat="server">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlKeyword" runat="server">
                        <div class="modal-dialog modal-sm">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h3 style="text-align: right; margin: 0px;">
                                        <asp:LinkButton ID="lnkCloseKeywordModal" CausesValidation="false" runat="server" Text="إغلاق" class="pull-left" OnClick="lnkCloseKeywordModal_Click"></asp:LinkButton></h3>
                                </div>
                                <div class="modal-body">
                                    <asp:Panel ID="Panel2" runat="server" DefaultButton="lnkSaveKeyword">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label class="col-lg-4">الكلمة</label>
                                                    <div class="col-lg-8">
                                                        <asp:TextBox ID="txtKeyword" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ForeColor="Red" ID="rfvKeyword" runat="server" Display="Dynamic" ControlToValidate="txtKeyword" ValidationGroup="vgKeyword">*</asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="modal-footer">
                                    <div class="col-md-12 text-left fix">
                                        <asp:LinkButton ID="lnkSaveKeyword" OnClick="lnkSaveKeyword_Click" runat="server" CssClass="btn btn-sm btn-danger" ValidationGroup="vgKeyword">حفظ</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col-md-7">
                    <asp:LinkButton ID="lnkPrintBarcode" CssClass="btn btn-purple btn-block btn-lg" runat="server" OnClick="lnkPrintBarcode_Click" Visible="false"><i class="fa fa-print"></i> طباعة الباركود</asp:LinkButton>
                    <asp:LinkButton ID="lnkScan" CssClass="btn btn-grey btn-block btn-lg" runat="server" OnClick="lnkScan_Click" Visible="false"><i class="fa fa-barcode"></i> المسح الضوئي</asp:LinkButton>
                    <asp:LinkButton ID="lnkUpload" CssClass="btn btn-pink btn-block btn-lg" OnClick="lnkUpload_Click" runat="server" Visible="false"><i class="fa fa-upload"></i> تحميل الوثيقة</asp:LinkButton>
                    <asp:LinkButton ID="lnkSubmitUpload" runat="server" CssClass="hidden" OnClick="lnkSubmitUpload_Click"></asp:LinkButton>
                    <asp:FileUpload ID="fuDoc" runat="server" CssClass="ace-file-input" />
                    <asp:Literal ID="ltrPDFEmbed" runat="server" meta:resourcekey="ltrPDFEmbedResource1" />
                </div>
            </div>
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkUpload" />
            <asp:PostBackTrigger ControlID="lnkSubmitUpload" />
        </Triggers>
    </asp:UpdatePanel>
    <script src="App_Themes/js/select2.min.js"></script>
    <telerik:ReportViewer ID="ReportViewer1" enableaccessibility="false" runat="server" Style="display: none"></telerik:ReportViewer>
    <script>
        $(document).ready(function () {
            document.getElementById("<%=fuDoc.ClientID %>").setAttribute('accept', 'application/pdf');
        
        });
        function initFileUpload() {
            $('.ace-file-input').ace_file_input({
                no_file: 'لم يتم اختير ملف بعد',
                btn_choose: 'اختر',
                btn_change: 'غير',
                droppable: false,
                thumbnail: false //| true | large
                //whitelist:'gif|png|jpg|jpeg'
                //blacklist:'exe|php'
                //onchange:''
                //
            });
        }
        function showBrowseDialog() {
            document.getElementById('<%=fuDoc.ClientID%>').click();
        }
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=lnkSubmitUpload.ClientID %>").click();
            }
        }
        ReportViewer.prototype.PrintReport = function () {
            switch (this.defaultPrintFormat) {
                case "Default":
                    this.DefaultPrint();
                    break;
                case "PDF":
                    this.PrintAs("PDF");
                    previewFrame = document.getElementById(this.previewFrameID);
                    previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); }
                    break;
            }
        };
        ReportViewer.OnReportLoadedOld = ReportViewer.OnReportLoaded;
        ReportViewer.prototype.OnReportLoaded = function () {
            this.OnReportLoadedOld();
        }
        function fnPrintReport() {
    <%=ReportViewer1.ClientID %>.PrintReport();
        }
    </script>
    <script>
        function moveToNext(field, nextFieldID) {
            if (field.value.length >= field.maxLength) {
                $("input[name*='" + nextFieldID + "']").focus();
            }
        }
        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            $('.select2').select2();
        });

        function InitializeRequest() {
        }

        function EndRequest() {
            $('.select2').select2();
        }
    </script>
    </asp:Content>