<%@ Control Language="C#" AutoEventWireup="true" CodeFile="project-users.ascx.cs" Inherits="UCs_projects" %>
<ul class="nav nav-tabs" id="myTab3">
    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.UsersPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-users.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">المستخدمين</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectSalaryEffectsPath)&&Request.Url.AbsoluteUri.ToLower().Contains("&t=1"))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-salary-effects.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"&t=1'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الأعطال</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectSalaryEffectsPath)&&Request.Url.AbsoluteUri.ToLower().Contains("&t=2"))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-salary-effects.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"&t=2'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الأذونات</a>
    </li>
</ul>
