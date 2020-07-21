<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-structure.aspx.cs" Inherits="structure" %>
<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp"%>
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
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <div class="page-content">
                <div class="tabbable">
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_structure" class="tab-pane active">
                            <div class="clearfix"></div>
                            <div class="row" id="divMenu" runat="server">
                                <div class="col-sm-7 icon-caret ace-icon tree-minus ">
                                    <asp:TreeView ID="tvMenu" Font-Size="Large" runat="server" ExpandDepth="0" ShowLines="True" OnTreeNodePopulate="tvMenu_TreeNodePopulate" meta:resourcekey="tvMenuResource1" />
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
