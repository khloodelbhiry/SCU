<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="App_Themes/js/Chart.min.js"></script>
    <script src="App_Themes/js/utils2.js"></script>
    <link href="App_Themes/css/dashboard.css" rel="stylesheet" />
    <link href="App_Themes/css/jquery.percentageloader-0.2.css" rel="stylesheet" />
  <div id="id_Dashboard" class="tab-pane active">
                            <div class="col-md-12 text-center">
                                <div class="page-header">
                                    <h1>نسبة الإنجاز الكلية </h1>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div style="width: 100%;">
                                    <canvas id="canvas" dir="rtl" height="200"></canvas>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div id="topLoader"></div>
                            </div>
                            <div class="col-sm-5">
                                <canvas id="canvas2" dir="ltr" height="200"></canvas>
                            </div>
                            <div class="clearfix"></div>
                            <div class="row">
                                <div class="col-lg-4 col-xs-4">
                                    <div class="widget-overview bg-primary-1">
                                        <div class="inner">
                                            <h1>
                                                <asp:Literal ID="ltrPages" Text="0" runat="server"></asp:Literal>
                                            </h1>
                                            <h2>عدد الورق</h2>
                                        </div>
                                        <div class="icon">
                                            <i class="fa fa-file"></i>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-xs-4">
                                    <div class="widget-overview bg-primary-6">
                                        <div class="inner">
                                            <h1>
                                                <asp:Literal ID="ltrProjectFiles" Text="0" runat="server"></asp:Literal>
                                            </h1>
                                            <h2>عدد الملفات</h2>
                                        </div>
                                        <div class="icon">
                                            <i class="fa fa-file"></i>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-xs-4">
                                    <div class="widget-overview bg-primary-1">
                                        <div class="inner">
                                            <h1>
                                                <asp:Literal ID="ltrDocs" Text="0" runat="server"></asp:Literal>
                                            </h1>
                                            <h2>عدد الوثائق</h2>
                                        </div>
                                        <div class="icon">
                                            <i class="fa fa-file"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="widget-overview bg-primary-1">
                                    <div class="inner">
                                        <h1>
                                            <asp:Literal ID="ltrMinisters" Text="0" runat="server"></asp:Literal>
                                        </h1>
                                        <h3>التحضير للمشروع</h3>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-cogs"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="widget-overview bg-primary-4">
                                    <div class="inner">
                                        <h1>
                                            <asp:Literal ID="ltrDepartment" Text="0" runat="server"></asp:Literal>
                                        </h1>
                                        <h3>التجهيز الأولي للملفات</h3>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-files"></i>
                                    </div>
                                </div>
                            </div>
                                <div class="col-lg-2 col-xs-2">
                                <div class="widget-overview bg-primary-1">
                                    <div class="inner">
                                        <h1>
                                            <asp:Literal ID="Literal1" Text="0" runat="server"></asp:Literal>
                                        </h1>
                                        <h3>المسح الضوئي</h3>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-file"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="widget-overview bg-primary-5">
                                    <div class="inner">
                                        <h1>
                                            <asp:Literal ID="Literal2" Text="0" runat="server"></asp:Literal>
                                        </h1>
                                        <h3>الفهرسة</h3>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-file"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="widget-overview bg-primary-6">
                                    <div class="inner">
                                        <h1>
                                            <asp:Literal ID="Literal3" Text="0" runat="server"></asp:Literal>
                                        </h1>
                                        <h3>المقبول</h3>
                                    </div>
                                    <div class="icon">
                                        <i class="fa fa-file"></i>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
    <script src="App_Themes/js/jquery.percentageloader-0.1.min.js"></script>
    <script>
        var clock;
        $(document).ready(function () {
            initCount();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            initCount();
        });
        function initCount() {
            var $topLoader = $("#topLoader").percentageLoader({
                width: 256, height: 256, controllable: false, value: noOfExportedPages, progress: projectProgress, onProgressUpdate: function (val) {
                    this.setValue(10 + 'kj');
                }
            });
        }
    </script>
</asp:Content>

