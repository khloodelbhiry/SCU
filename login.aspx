<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Async="true" EnableEventValidation="false" Inherits="login" %>

<%@ Register Namespace="App_Code" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>SCU - تسجيل الدخول</title>
    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link rel="stylesheet" href="App_Themes/css/bootstrap.min.css" />
    <link rel="stylesheet" href="App_Themes/font-awesome/4.5.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="App_Themes/css/fonts.googleapis.com.css" />
    <link rel="stylesheet" href="App_Themes/css/ace.min.css" />
    <link rel="stylesheet" href="App_Themes/css/ace-rtl.min.css" />
    <link rel="stylesheet" href="App_Themes/css/jquery.gritter.min.css" />
    <link rel="stylesheet" href="App_Themes/css/login.css" />
    <link href="App_Themes/css/style.css" rel="stylesheet" />
    <script src="App_Themes/js/jquery-2.1.4.min.js"></script>
    <script src="App_Themes/js/jquery-ui.min.js"></script>
    <script src="App_Themes/js/jquery.gritter.min.js"></script>
</head>
<body class="login-layout light-login">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
            <ProgressTemplate>
                <div class="overlay">
                    <div class="center-overlay">
                        <i class="ace-icon fa fa-spinner fa-spin orange bigger-300"></i>Please Wait...
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <div class="main-container">
                    <div class="main-content">
                        <div class="row">
                            <div class="col-sm-10 col-sm-offset-1">
                                <div class="login-container">
                                <div class="center">
                                        <img src="App_Themes/images/1565272252.png" alt="SCU" style="width:250px;" />
                                            <h1>OneTrack</h1>
                                            <h2>Content Management System</h2>
                                    </div>
                                    <div class="space-6"></div>
                                    <div class="position-relative">
                                        <div id="login-box" class="login-box visible widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main">
                                                    <div class="space-6"></div>
                                                    <form>
                                                        <fieldset>
                                                            <asp:Panel ID="pnlLogin" runat="server" DefaultButton="lnkLogin">
                                                                <label class="block clearfix">
                                                                    <span class="block input-icon input-icon-right">
                                                                        <i class="ace-icon fa fa-envelope"></i>
                                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="البريد الألكترونى"></asp:TextBox>
                                                                    </span>
                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvEmail" runat="server" ForeColor="Red" ControlToValidate="txtEmail" ValidationGroup="vgLogin">*</asp:RequiredFieldValidator>
                                                                </label>
                                                                <label class="block clearfix">
                                                                    <span class="block input-icon input-icon-right">
                                                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="كلمة المرور" TextMode="Password"></asp:TextBox>
                                                                        <i class="ace-icon fa fa-lock"></i>
                                                                    </span>
                                                                    <asp:RequiredFieldValidator SetFocusOnError="true" ID="rfvPassword" runat="server" ForeColor="Red" ControlToValidate="txtPassword" ValidationGroup="vgLogin">*</asp:RequiredFieldValidator>
                                                                </label>
                                                                <div class="space"></div>
                                                                <div class="clearfix">
                                                                    <asp:LinkButton ID="lnkLogin" OnClick="lnkLogin_Click" runat="server" CssClass="width-35 pull-right btn btn-sm btn-primary pull-left" ValidationGroup="vgLogin"><i class="ace-icon fa fa-key"></i><span class="bigger-110">تسجيل دخول</span></asp:LinkButton>
                                                                </div>
                                                                <div class="space-4"></div>
                                                            </asp:Panel>
                                                        </fieldset>
                                                    </form>
                                                </div>
                                                <div class="toolbar clearfix">
                                                     <div>
                                                        <a href="#" data-target="#forgot-box" class="forgot-password-link">
                                                            <i class="ace-icon fa fa-arrow-left"></i>
                                                            نسيت كلمة المرور؟
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- /.login-box -->
                                    </div>
                                    <!-- /.position-relative -->
                                </div>
                            </div>
                            <!-- /.col -->
                        </div>
                        <!-- /.row -->
                    </div>
                    <!-- /.main-content -->
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <!-- inline scripts related to this page -->
    <script type="text/javascript">
        jQuery(function ($) {
            $("#show-option").tooltip({
                show: {
                    effect: "slideDown",
                    delay: 250
                }
            });
            $(document).on('click', '.toolbar a[data-target]', function (e) {
                e.preventDefault();
                var target = $(this).data('target');
                $('.widget-box.visible').removeClass('visible');//hide others
                $(target).addClass('visible');//show target
            });
        });
        function ShowMsg(title, text, cssClass) {
            $.gritter.add({
                title: title,
                text: text,
                class_name: 'gritter-' + cssClass
            });
            return false;
        }
    </script>
</body>
</html>
