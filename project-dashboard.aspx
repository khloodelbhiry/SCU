<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="project-dashboard.aspx.cs" Inherits="project_dashboard" %>

<%@ Register Src="UCs/projects.ascx" TagName="WebControl" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="App_Themes/js/Chart.min.js"></script>
    <link href="App_Themes/css/board.css" rel="stylesheet" />
    <link href="App_Themes/css/animate.min.css" rel="stylesheet" />
    <link href="App_Themes/css/jquery.percentageloader-0.2.css" rel="stylesheet" />
    <link href="https://cdn.anychart.com/releases/8.7.1/css/anychart-ui.min.css?hcode=a0c21fc77e1449cc86299c5faa067dc4" rel="stylesheet" />
    <script src="App_Themes/js/moment.min.js"></script>
    <link href="App_Themes/css/daterangepicker-bs3.css" rel="stylesheet" />
    <link href="App_Themes/css/chartist.min.css" rel="stylesheet" />
    <script src="App_Themes/js/spark.js"></script>
    <script src="App_Themes/js/jquery.easypiechart.min.js"></script>
    <script src="//cdn.jsdelivr.net/chartist.js/latest/chartist.min.js"></script>
    <link href="//cdn.jsdelivr.net/chartist.js/latest/chartist.min.css" rel="stylesheet" type="text/css" />

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
            <div class="page-content">
                <div class="tabbable">
                    <asp:WebControl ID="ucProjects" runat="server" />
                    <div class="tab-content">
                        <div id="id_Dashboard" class="tab-pane active">
                            <div class="row d-flex">
                                <div class="col-md-6 col-lg-4 col-xl-3 mg-t-10 mg-lg-t-0">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">التجهيز الأولي</h4>
                                        </div>
                                        <div class="card-body pd-lg-25">
                                            <div class="chart-seven">
                                                <canvas id="chartPrepare"></canvas>
                                            </div>
                                        </div>
                                        <div class="card-footer pd-20">
                                            <div class="row">
                                                <asp:Repeater ID="rpPrepare" runat="server">
                                                    <ItemTemplate>
                                                        <div class='<%# Eval("name").ToString().Length>30?"clearfix":string.Empty %>'></div>
                                                        <div class='<%# Container.ItemIndex >1?"col-xs-6 mg-t-20":"col-xs-6" %>'>
                                                            <p class="tx-10 tx-uppercase tx-medium tx-color-03 tx-spacing-1 tx-nowrap mg-b-5"><%#Eval("name") %></p>
                                                            <div class="d-flex align-items-center">
                                                                <div class='<%#"wd-10 ht-10 rounded-circle "+dtColors.Rows[Container.ItemIndex]["color"]+" mg-l-5" %>'></div>
                                                                <h5 class="tx-normal mg-t-0 tx-rubik mg-b-0"><%#Eval("pages") %> صفحة <strong class="tx-color-04"><%#Math.Round(double.Parse(Eval("Progress").ToString()),6)+"%" %></strong></h5>
                                                            </div>
                                                        </div>
                                                        <div class='<%# Eval("name").ToString().Length>30?"clearfix":string.Empty %>'></div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-lg-4 col-xl-3 mg-t-10 mg-lg-t-0">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">نسبة الإنجاز الكلي</h4>
                                        </div>
                                        <div class="card-body pd-lg-25">
                                            <div class="col-sm-1">
                                                <div style="width: 100%;">
                                                    <canvas id="canvas" dir="rtl" height="100"></canvas>
                                                </div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div id="topLoader"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <br />
                                    <div class="col-xs-6 padding_left_none">
                                        <div class="tiles purple added-margin">
                                            <div class="tiles-body">
                                                <div class="tiles-title animated">عدد موظفين الجهة </div>
                                                <div class="row-fluid">
                                                    <div class="heading">
                                                        <span class="counter">
                                                            <asp:Literal ID="ltrGovernorateEmp" runat="server"></asp:Literal></span>
                                                    </div>
                                                    <div class="progress transparent progress-white progress-small no-radius">
                                                        <div class="progress-bar progress-bar-white animate-progress-bar" data-percentage="100%" style="width: 100%;"></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xs-6 padding_left_none">
                                        <div class="tiles red added-margin">
                                            <div class="tiles-body">
                                                <div class="tiles-title animated">عدد موظفين الشركة </div>
                                                <div class="heading">
                                                    <span class="counter">
                                                        <asp:Literal ID="ltrCompanyEmp" runat="server"></asp:Literal></span>
                                                </div>
                                                <div class="progress transparent progress-white progress-small no-radius">
                                                    <div class="progress-bar progress-bar-white animate-progress-bar" data-percentage="100%" style="width: 100%;"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-lg-4 col-xl-3 mg-t-10 mg-lg-t-0">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">الرقمنة</h4>
                                        </div>
                                        <div class="card-body pd-lg-25">
                                            <div class="chart-seven">
                                                <canvas id="chartDigitization"></canvas>
                                            </div>
                                        </div>
                                        <div class="card-footer pd-20">
                                            <div class="row">
                                                <asp:Repeater ID="rpDigitization" runat="server">
                                                    <ItemTemplate>
                                                        <div class='<%# Eval("name").ToString().Length>30?"clearfix":string.Empty %>'></div>
                                                        <div class='<%# Container.ItemIndex >1?"col-xs-6 mg-t-20":"col-xs-6" %>'>
                                                            <p class="tx-10 tx-uppercase tx-medium tx-color-03 tx-spacing-1 tx-nowrap mg-b-5"><%#Eval("name") %></p>
                                                            <div class="d-flex align-items-center">
                                                                <div class='<%#"wd-10 ht-10 rounded-circle "+dtColors.Rows[Container.ItemIndex]["color"]+" mg-l-5" %>'></div>
                                                                <h5 class="tx-normal mg-t-0 tx-rubik mg-b-0"><%#Eval("pages") %> صفحة <strong class="tx-color-04"><%#Math.Round(double.Parse(Eval("Progress").ToString()),6)+"%" %></strong></h5>
                                                            </div>
                                                        </div>
                                                        <div class='<%# Eval("name").ToString().Length>30?"clearfix":string.Empty %>'></div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-body pd-lg-25 padding-0">
                                            <div class="ct-chart"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row d-flex">
                                <div class="col-sm-4 col-height">
                                    <div class="card col-height">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">متطلبات التشغيل</h4>
                                        </div>
                                        <div class="card-body pd-lg-25">
                                            <div class="col-xs-12 text-center">
                                                <asp:Literal ID="ltrPrerequisites" runat="server"></asp:Literal>
                                            </div>
                                            <div class="clearfix"></div>
                                            <hr />
                                            <asp:Repeater ID="rpPrerequisites" runat="server">
                                                <ItemTemplate>
                                                    <div class="clearfix">
                                                        <span class="pull-right"><%# Eval("name") %></span>
                                                        <span class="pull-left"><%# Eval("progress") %>%</span>
                                                    </div>
                                                    <div class="progress progress-striped active h-15">
                                                        <div <%#"style='width:" +Eval("progress")+"%'" %> class="progress-bar progress-bar-purple"></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-8 col-height">
                                    <div class="card col-height">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">المشكلات</h4>
                                        </div>
                                        <div class="card-body pd-lg-25">
                                            <div class="col-xs-4 text-center">
                                                <asp:Literal ID="ltrIssuesUnderApprove" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-xs-4 text-center">
                                                <asp:Literal ID="ltrOpen" runat="server"></asp:Literal>
                                            </div>
                                            <div class="col-xs-4 text-center">
                                                <asp:Literal ID="ltrClosed" runat="server"></asp:Literal>
                                            </div>
                                            <div class="clearfix"></div>
                                            <hr />
                                            <div class="clearfix"></div>
                                            <div class="timeline">
                                                <asp:Repeater ID="rpIssuesByDate" runat="server" OnItemDataBound="rpIssuesByDate_ItemDataBound">
                                                    <ItemTemplate>
                                                        <h6 class="timeline-head"><%#GetMonthName(int.Parse(Eval("month").ToString())) +", "+Eval("year") %></h6>
                                                        <asp:HiddenField ID="hdfMonth" runat="server" Value='<%# Eval("month") %>' />
                                                        <asp:HiddenField ID="hdfYear" runat="server" Value='<%# Eval("year") %>' />
                                                        <ul class="timeline-list">
                                                            <asp:Repeater ID="rpIssues" runat="server">
                                                                <ItemTemplate>
                                                                    <li class="timeline-item">
                                                                        <div class='<%#"timeline-status mg-l-5 "+dtColors.Rows[Container.ItemIndex]["color"] %>'></div>
                                                                        <div class="timeline-date"><%#Convert.ToDateTime(Eval("Issue_Date")).ToString("MMM dd")%> </div>
                                                                        <div class="timeline-data">
                                                                            <h6 class="timeline-title inline"><%# Eval("name") %></h6>
                                                                            <span class='<%# int.Parse(Eval("statusId").ToString())==(int)IssueStatusEnum.Closed?"tx-success inline":(int.Parse(Eval("statusId").ToString())==(int)IssueStatusEnum.Open?"tx-danger inline":"tx-primary inline") %>'><%# Eval("status") %></span>
                                                                            <div class="timeline-des">
                                                                                <p><%# Eval("issue1") %></p>
                                                                                <i class="fa fa-clock-o"></i><%# Eval("MinutesAndHours") %>
                                                                            </div>
                                                                        </div>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <a class="btn btn-pink btn-block bolder pad-t-1 pad-b-1">الـــــــــمـــــــزيـــــــــد ...</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-header">
                                            <h4 class="mg-b-0 mg-t-0 text-center">إنجاز الأدارات</h4>
                                        </div>
                                        <div class="card-body pd-lg-25 padding-0">
                                            <div class="form-actions mg-t-0 mg-b-0">
                                                <div class="col-xs-3">
                                                    <div class="input-prepend input-group">
                                                        <input type="text" name="daterange" id="daterange" class="form-control" runat="server" />
                                                        <span class="add-on input-group-addon"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-xs-9">
                                                    <div class="input-group">
                                                        <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-control"></asp:DropDownList>
                                                        <span class="input-group-btn">
                                                            <asp:LinkButton ID="lnkSearchUnits" runat="server" CssClass="btn btn-sm btn-info no-radius" OnClick="lnkSearchUnits_Click"><i class="ace-icon fa fa-search"></i></asp:LinkButton>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>
                                            <div class="clearfix"></div>
                                            <div class="apexchart-wrapper">
                                                <div id="chartUnits" class="chart-fit"></div>
                                            </div>
                                            <table class="table table-borderless table-sm tx-13 vrtheader mg-b-0">
                                                <thead class="thead-color">
                                                    <tr class="tx-10 tx-spacing-1 tx-color-03 tx-uppercase height-130">
                                                        <th>الإدارة</th>
                                                        <asp:Repeater ID="rpStates" runat="server">
                                                            <ItemTemplate>
                                                                <th class="verticalTableHeader">
                                                                    <div><%#Eval("name").ToString().Length>24? Eval("name").ToString().Substring(0,24):Eval("name") %></div>
                                                                </th>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Repeater ID="rpUnits" runat="server" OnItemDataBound="rpUnits_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="tx-medium thead-color"><%# Eval("name") %></td>
                                                                <asp:HiddenField ID="hdfUnitId" runat="server" Value='<%# Eval("id") %>' />
                                                                <asp:Repeater ID="rpUnitProgress" runat="server">
                                                                    <ItemTemplate>
                                                                        <td class="text-right"><%# Eval("progress") %>%</td>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="App_Themes/js/jquery.percentageloader-0.1.min.js"></script>
    <script src="App_Themes/js/flot.js"></script>
    <script src="App_Themes/js/flot.stack.js"></script>
    <script src="App_Themes/js/apex.js"></script>
    <script src="App_Themes/js/daterangepicker.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/waypoints/2.0.3/waypoints.min.js"></script>
    <script src="https://cdn.jsdelivr.net/jquery.counterup/1.0/jquery.counterup.min.js"></script>
    <script>
        var optionpie = {
            maintainAspectRatio: false,
            responsive: true,
            legend: {
                display: false,
            },
            animation: {
                animateScale: true,
                animateRotate: true
            }
        };

        var clock;
        $(document).ready(function () {
            initCount();
        });
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            initCount();
        });
        var optionSet1 = {
            startDate: moment(),
            endDate: moment(),
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            opens: 'left',
            buttonClasses: ['btn btn-default'],
            applyClass: 'btn-sm btn-success',
            cancelClass: 'btn-sm btn-danger',
            format: 'YYYY/MM/DD',
            separator: ' : ',
            locale: {
                applyLabel: 'اختيار',
                cancelLabel: 'الغاء',
                fromLabel: 'من',
                toLabel: 'الى',
            }
        };
        function initCount() {

            $('.easy-pie-chart.percentage').each(function () {
                $(this).easyPieChart({
                    barColor: $(this).data('color'),
                    trackColor: '#EEEEEE',
                    scaleColor: false,
                    lineCap: 'butt',
                    lineWidth: 8,
                    animate: ace.vars['old_ie'] ? false : 1000,
                    size: 75
                }).css('color', $(this).data('color'));
            });
            $('.counter').counterUp({
                delay: 10,
                time: 2000
            });
            $('[id$=daterange]').daterangepicker(optionSet1, function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
            });
            $('.counter').addClass('animated fadeInDownBig');
            $('.tiles-title').addClass('animated fadeIn');
            var $topLoader = $("#topLoader").percentageLoader({
                width: 220, height: 220, controllable: false, value: noOfPages, progress: projectProgress, onProgressUpdate: function (val) {
                    this.setValue(10 + 'kj');
                }
            });
        }
    </script>
</asp:Content>

