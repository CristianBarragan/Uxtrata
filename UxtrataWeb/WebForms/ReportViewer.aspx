<%@ Page Language="C#" CodeBehind="ReportViewer.aspx.cs" Inherits="UxtrataWeb.WebForms.ReportViewer" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Viewer</title>
    <script runat="server">
        void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                int Id = 0;
                List<UxtrataWeb.ModelView.TransactionDTO> trans = new List<UxtrataWeb.ModelView.TransactionDTO>();
                UxtrataWeb.Business.ReportBusiness business = new UxtrataWeb.Business.ReportBusiness();
                if (Request.QueryString["studentID"] != null)
                {
                    Id = Convert.ToInt32(Request.QueryString["studentID"]);
                    trans = business.getReportStudents(Id);
                }
                if (Request.QueryString["courseID"] != null)
                {
                    Id = Convert.ToInt32(Request.QueryString["courseID"]);
                    trans = business.getReportCourses(Id);
                }
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/StudentReport.rdlc");
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rdc = new ReportDataSource("studentReportDataSet", trans);
                ReportViewer1.LocalReport.DataSources.Add(rdc);
                ReportViewer1.LocalReport.Refresh();
            }
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
