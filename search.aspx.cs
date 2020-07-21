using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class users : System.Web.UI.Page
{
    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {
                ViewState["dirState"] = SortDirection.Ascending;
            }
            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;
        }
    }
    private DataTable dtData
    {
        get
        {
            return ((DataTable)ViewState["_dtData"]);
        }
        set
        {
            if (value == null)
            {
                ViewState.Remove("_dtData");
            }
            else
            {
                ViewState["_dtData"] = value;
            }
        }
    }
    public List<UserPermissions> UserPermissions
    {
        get
        {
            if (Session["UserPermissions"] != null && Session["UserPermissions"].ToString() != string.Empty)
                return global::UserPermissions.DeSerializePermissionsList(Session["UserPermissions"].ToString());
            else
            {
                return new List<UserPermissions>();
            }
        }
        set { Session["UserPermissions"] = global::UserPermissions.SerializePermissionsList(value); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            HtmlAnchor lnkHistory = (HtmlAnchor)Master.FindControl("lnkHistory");
            lnkHistory.Visible = false;
            if (Session["User"] != null && Session["User"].ToString() != string.Empty)
            {
                if (UserPermissions.Any(
                    p =>
                        p.PageUrl.ToLower().Equals(Common.SearchPath) && p.Show.Equals(true)))
                {
                    var per = UserPermissions.FirstOrDefault(p => p.PageUrl.ToLower().Equals(Common.SearchPath));
                    ((HtmlGenericControl)Page.Master.FindControl("ulBreadcrumb")).InnerHtml = "<li><i class='ace-icon fa fa-home home-icon'></i><a href ='Default.aspx'> الرئيسية </a></li><li>" + per.ModuleName + "</li><li><a class='active'>" + per.PageName + "</a></li>";
                    Page.Title = per.PageName;
                    using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
                    {
                        Project p = db.Projects.FirstOrDefault();
                        if (p != null)
                        {
                            lnkArchiving.PostBackUrl = "project-files.aspx?id=" + EncryptString.Encrypt(p.id.ToString()) + "&g=" + EncryptString.Encrypt(p.governmentalEntityId.ToString()) + "&c=" + EncryptString.Encrypt(p.companyId.ToString());
                        }
                    }
                }
                else
                    Response.Redirect("no-permission.aspx");
            }
            else
                Response.Redirect("Login.aspx?ReturnURL=" + Request.Url.AbsolutePath);

            BindDDL();
            BindData();
        }
    }
    private void BindDDL()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.UnitStructures
                         where q.parentId != 0
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlMinister.DataSource = query1;
            ddlMinister.DataTextField = "name";
            ddlMinister.DataValueField = "id";
            ddlMinister.DataBind();
            ddlMinister.Items.Insert(0, new ListItem("-- اختر --", "0"));


            var query3 = from q in db.Categories
                         where q.parentId == 0
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlCategory.DataSource = query3;
            ddlCategory.DataTextField = "name";
            ddlCategory.DataValueField = "id";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- اختر --", "0"));
            ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());

            var query2 = from q in db.DocTypes
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlDocType.DataSource = query2;
            ddlDocType.DataTextField = "name";
            ddlDocType.DataValueField = "id";
            ddlDocType.DataBind();
            ddlDocType.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query4 = from q in db.StatesTransitions
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlOperation.DataSource = query4;
            ddlOperation.DataTextField = "name";
            ddlOperation.DataValueField = "id";
            ddlOperation.DataBind();
            ddlOperation.Items.Insert(0, new ListItem("-- اختر --", "0"));

            var query5 = from q in db.Parties
                         where q.statusId == (int)StatusEnum.Approved
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlSendingParty.DataSource = query5;
            ddlSendingParty.DataTextField = "name";
            ddlSendingParty.DataValueField = "id";
            ddlSendingParty.DataBind();
            ddlSendingParty.Items.Insert(0, new ListItem("-- اختر --", "0"));

            ddlReceivingParty.DataSource = query5;
            ddlReceivingParty.DataTextField = "name";
            ddlReceivingParty.DataValueField = "id";
            ddlReceivingParty.DataBind();
            ddlReceivingParty.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void gdvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvData.PageIndex = e.NewPageIndex;
        BindData();
    }
    protected void gdvData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string SortDir = string.Empty;
        if (dir == SortDirection.Ascending)
        {
            dir = SortDirection.Descending;
            SortDir = "Desc";
        }
        else
        {
            dir = SortDirection.Ascending;
            SortDir = "Asc";
        }
        DataView sortedView = new DataView(dtData);
        sortedView.Sort = e.SortExpression + " " + SortDir;
        gdvData.DataSource = sortedView;
        gdvData.DataBind();
    }
    private void BindData()
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query = (from b in db.FileDocuments
                         select new
                         {
                             BoxBarcode=b.BoxFile.barcode,
                             b.id,
                             b.barcode,
                             b.reference,
                             b.date,
                             b.fileId,
                             UnitStructure=b.BoxFile.UnitStructure.name,
                             StatesTransition=b.BoxFile.StatesTransition.name,
                             b.BoxFile.nextStateId,
                             b.BoxFile.code,
                             b.BoxFile.unitStructureId,
                             b.title,
                             b.docTypeId,
                             DocType=b.DocType.name,
                             Category_Name =b.categoryId != null ?db.Categories.FirstOrDefault(x=>x.id.Equals(b.Category.parentId)).name : null,
                             SubCategory_Name =b.categoryId != null ? db.Categories.FirstOrDefault(x => x.id.Equals(b.categoryId)).name : null,
                             SubCategoryID = b.categoryId,
                             CategoryID=b.categoryId!=null?(int?) b.Category.parentId:null,
                             b.isIndexed,
                             b.isReviewed,
                             BoxTitle=b.BoxFile.title,
                             BoxReference = b.BoxFile.reference,
                             b.sendingPartyId,
                             b.receivingPartyId,
                             keywords =from x in b.FileDocumentKeywords
                                      select new
                                      {
                                          x.keyword
                                      }
                         });
            if (ddlMinister.SelectedValue != "0")
                query = query.Where(x => x.unitStructureId.Equals(int.Parse(ddlMinister.SelectedValue)));
            if (ddlDocType.SelectedValue != "0")
                query = query.Where(x => x.docTypeId.Equals(int.Parse(ddlDocType.SelectedValue)));
            if (ddlCategory.SelectedValue != "0")
                query = query.Where(x => x.CategoryID.Equals(int.Parse(ddlCategory.SelectedValue)));
            if (ddlSubcategory.SelectedValue != "0")
                query = query.Where(x => x.SubCategoryID.Equals(int.Parse(ddlSubcategory.SelectedValue)));
            if (txtBoxCodeSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.BoxBarcode.Equals(txtBoxCodeSrc.Text.Trim()));
            if (txtBoxTitleSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.BoxTitle.Contains(txtBoxTitleSrc.Text.Trim()));
            if (txtBoxReferenceSrc.Text.Trim() != string.Empty)
                query = query.Where(x => x.BoxReference.Equals(txtBoxReferenceSrc.Text.Trim()));
            if (ddlOperation.SelectedValue != "0")
                query = query.Where(x => x.nextStateId.Equals(int.Parse(ddlOperation.SelectedValue)));
            if (ddlSendingParty.SelectedValue != "0")
                query = query.Where(x => x.sendingPartyId.Equals(int.Parse(ddlSendingParty.SelectedValue)));
            if (ddlReceivingParty.SelectedValue != "0")
                query = query.Where(x => x.receivingPartyId.Equals(int.Parse(ddlReceivingParty.SelectedValue)));
            if (txtDocCode.Text.Trim() != string.Empty)
                query = query.Where(x => x.barcode.Equals(txtDocCode.Text.Trim()));
            if (txtNumber.Text.Trim() != string.Empty)
                query = query.Where(x => x.reference.Equals(txtNumber.Text.Trim()));
            if (txtSubject.Text.Trim() != string.Empty)
                query = query.Where(x => x.title.Equals(txtSubject.Text.Trim()));
            if (txtKeywords.Text.Trim() != string.Empty)
                query = query.Where(x => x.keywords.Any(y=>y.keyword.Equals(txtKeywords.Text.Trim())));
            if (txtDateFrom.Text.Trim() != string.Empty)
                query = query.Where(x => x.date.Value.Date >= Convert.ToDateTime(txtDateFrom.Text));
            if (txtDateTo.Text.Trim() != string.Empty)
                query = query.Where(x => x.date.Value.Date >= Convert.ToDateTime(txtDateTo.Text));
          
            gdvData.DataSource = query;
            gdvData.DataBind();
            lblResult.Text = query.Count().ToString();
        }
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SCU_OneTrackDataContext db = new SCU_OneTrackDataContext())
        {
            var query1 = from q in db.Categories
                         where q.parentId.Equals(int.Parse(ddlCategory.SelectedValue))
                         select new
                         {
                             q.id,
                             q.name
                         };
            ddlSubcategory.DataSource = query1;
            ddlSubcategory.DataTextField = "name";
            ddlSubcategory.DataValueField = "id";
            ddlSubcategory.DataBind();
            ddlSubcategory.Items.Insert(0, new ListItem("-- اختر --", "0"));
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void btnClearSearch_Click(object sender, EventArgs e)
    {
        txtBoxReferenceSrc.Text = txtBoxTitleSrc.Text = txtBoxCodeSrc.Text = txtDateFrom.Text = txtDateTo.Text = txtDocCode.Text = txtNumber.Text = txtSubject.Text = string.Empty;
        ddlSendingParty.SelectedValue = ddlReceivingParty.SelectedValue = ddlDocType.SelectedValue = ddlCategory.SelectedValue = ddlMinister.SelectedValue = ddlOperation.SelectedValue = "0";
        ddlCategory_SelectedIndexChanged(ddlCategory, new EventArgs());
        BindData();
    }
}