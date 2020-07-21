<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=13.1.19.618, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
    <telerik:reportviewer id="ReportViewer1" enableaccessibility="false" runat="server" style="display: none"></telerik:reportviewer>
    <script>
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }
        ReportViewer.prototype.PrintReport = function () {
            switch (this.defaultPrintFormat) {
                case "Default":
                    this.DefaultPrint();
                    break;
                case "PDF":
                    this.PrintAs("PDF");
                    previewFrame = document.getElementById(this.previewFrameID);
                    previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); }
                    break;
            }
        };
        ReportViewer.OnReportLoadedOld = ReportViewer.OnReportLoaded;
        ReportViewer.prototype.OnReportLoaded = function () {
            this.OnReportLoadedOld();
        }
        function fnPrintReport() {
    <%=ReportViewer1.ClientID %>.PrintReport();
        }
    </script>
        </div>
    </form>
</body>
</html>
