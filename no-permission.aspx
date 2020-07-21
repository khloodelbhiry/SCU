<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="no-permission.aspx.cs" Inherits="no_permission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
</asp:Content>

