<%@ Control Language="C#" AutoEventWireup="true" CodeFile="companies.ascx.cs" Inherits="UCs_projects" %>
<ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
    
    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.CompanyUsersPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='company-users.aspx?id="+Request.QueryString["id"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">المستخدمين ( <asp:Literal ID="ltrUsers" runat="server"></asp:Literal> )</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.CompanyAssetsPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='company-assets.aspx?id="+Request.QueryString["id"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">الأصول ( <asp:Literal ID="ltrAssets" runat="server"></asp:Literal> )</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.CompanyConsumablesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='company-consumables.aspx?id="+Request.QueryString["id"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">المستهلكات</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.CompanyHrPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='company-hr.aspx?id="+Request.QueryString["id"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="id_Dashboard" aria-expanded="true">الموارد البشرية</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.CompanyFinancialPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='company-financial.aspx?id="+Request.QueryString["id"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الإدارة المالية</a>
    </li>
</ul>
