<%@ Control Language="C#" AutoEventWireup="true" CodeFile="projects.ascx.cs" Inherits="UCs_projects" %>
<ul class="nav nav-tabs padding-12 tab-color-blue background-blue" id="myTab4">
       <% if (UserDetails.DeSerializeUserDetails(Session["User"].ToString()).CompanyId!=0)
        { %>
    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectPrerequisitesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-prerequisites.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">خطة التشغيل</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectDashboardPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-dashboard.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">لوحة متابعة الإنجاز</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectStructurePath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-structure.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="id_Dashboard" aria-expanded="true">الهيكل التنظيمي</a>
    </li>
    
    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectBusinessReferencesPath)||Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectDocTypesPath)||Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectPersonsPath)||Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectPartiesPath)||Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectCategoriesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-doc-types.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">القوائم</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectFilesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-files.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الملفات (
                                <asp:Literal ID="ltrFiles" runat="server"></asp:Literal>
            )</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectTargetPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-target.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">العمليات</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectIssuesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-issues.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">المشكلات (
                                <asp:Literal ID="ltrIssues" runat="server" Text="0"></asp:Literal>
            )</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectSitesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-sites.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="project-sites.aspx" aria-expanded="true">مواقع العمل (<asp:Literal ID="ltrSites" runat="server" Text="0"></asp:Literal>)</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectStocksPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-stocks.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="project-stocks.aspx" aria-expanded="true">المستودعات (<asp:Literal ID="ltrStocks" runat="server" Text="0"></asp:Literal>)</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectAssetsPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-assets.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">الأصول (<asp:Literal ID="ltrAssets" runat="server" Text="0"></asp:Literal>)</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectConsumablesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-consumables.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">المستهلكات</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectUsersPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-users.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">المستخدمين (
                                <asp:Literal ID="ltrUsers" runat="server"></asp:Literal>
            )</a>
    </li>
      <% }
            else
            { %>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectFilesPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-files.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="stocks.aspx" aria-expanded="true">ملفاتى (
                                <asp:Literal ID="ltrFiles2" runat="server"></asp:Literal>
            )</a>
    </li>

    <% if (Request.Url.AbsoluteUri.ToLower().Contains(Common.ProjectDashboardPath))
        { %>
    <li class="active">
        <% }
            else
            { %>
    <li onclick="<%= "window.location='project-dashboard.aspx?id="+Request.QueryString["id"]+"&g="+Request.QueryString["g"]+"&c="+Request.QueryString["c"]+"'"%>">
        <% }%>
        <a data-toggle="tab" href="" aria-expanded="true">لوحة متابعة الإنجاز</a>
    </li>
      <% } %>
</ul>
