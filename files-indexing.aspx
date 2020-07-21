<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="files-indexing.aspx.cs" Inherits="files_indexing" %>

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
            <asp:HiddenField ID="hdfBatch" runat="server" />
            <asp:HiddenField ID="hdfFileIndexing" runat="server" />
            <asp:HiddenField ID="hdfNextState" runat="server" />
            <asp:HiddenField ID="hdfFile" runat="server" />
            <div class="page-content">
                <div class="col-md-5" id="divControls" runat="server">
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">كود الملف</label>
                        <div class="col-lg-8 topr calend">
                            <asp:TextBox ID="txtCode" Enabled="false" CssClass="col-md-12 form-control" runat="server" meta:resourcekey="txtCodeResource1"></asp:TextBox>
                            <asp:RequiredFieldValidator ForeColor="Red" ID="rfvCode" runat="server" Display="Dynamic" ControlToValidate="txtCode" ValidationGroup="vgSave" meta:resourcekey="rfvCodeResource1">*</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">عنوان الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtTitleResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">مرجع الملف</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtReference" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtReferenceResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label class="col-lg-4 top">الملاحظات</label>
                        <div class="col-lg-8 topr ">
                            <asp:TextBox ID="txtNotes" TextMode="MultiLine" Rows="3" runat="server" CssClass="col-md-12 form-control" meta:resourcekey="txtNotesResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12 text-left fix">
                        <div class="clearfix"></div>
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-danger" Text="تمت فهرسة الملف" OnClick="btnSave_Click" ValidationGroup="vgSave" />
                      </div>
                </div>
                <div class="col-md-7">
                    <asp:Literal ID="ltrPDFEmbed" runat="server" meta:resourcekey="ltrPDFEmbedResource1" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

