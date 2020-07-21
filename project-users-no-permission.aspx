<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-users-no-permission.aspx.cs" Inherits="no_permission" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<%@ Register Src="UCs/project-users.ascx" TagName="WebControl2" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="tabbable">
            <asp:WebControl ID="ucProjects" runat="server" />
            <div class="tab-content">
                <div id="id_issues" class="tab-pane active">
                    <div class="col-xs-12">
                        <div class="tabbable tabs-right">
                            <asp:WebControl2 ID="ucUsers" runat="server" />
                            <div class="tab-content">
                                <div id="home3" class="tab-pane active">
                                    <div class="error-container">
                                        <div class="well">
                                            <h1 class="grey lighter smaller">
                                                <span class="blue bigger-125">
                                                    <i class="ace-icon fa fa-lock"></i>
                                                    عفوا،
                                                </span>
                                                ليس لديك صلاحية
                                            </h1>
                                            <hr>
                                            <div class="space"></div>

                                            <div class="center">
                                                <a href="default.aspx" class="btn btn-primary">
                                                    <i class="ace-icon fa fa-tachometer"></i>
                                                    الرئيسية
                                                </a>
                                                <a href="javascript:history.back()" class="btn btn-grey">
                                                    <i class="ace-icon fa fa-arrow-left"></i>
                                                    رجوع
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>

