<%@ Control Language="C#" AutoEventWireup="true" CodeFile="project-lookups.ascx.cs" Inherits="UCs_projects" %>
<ul class="nav nav-tabs" id="myTab3">
    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectDocTypesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-doc-types.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">أنواع الوثائق</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectCategoriesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-categories.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">التصنيفات</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectPartiesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-parties.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الجهات</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectBusinessReferencesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-references.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">أنواع المراجع</a>
    </li>
</ul>
