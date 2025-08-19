
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillPrint.aspx.cs" Inherits="EBillingWeb.BillPrint" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Print Bill</title>
    <link href="App_Themes/Modern/Styles.css" rel="stylesheet" />
</head>
<body class="bg">
<form runat="server">
    <div class="container no-print">
        <asp:Button ID="btnBack" runat="server" Text="Back" OnClientClick="window.history.back(); return false;" CssClass="btn" />
        <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="window.print(); return false;" CssClass="btn" />
    </div>
    <div class="print-area">
        <div class="print-title">Electricity Bill</div>
        <asp:Label ID="lblCno" runat="server" /><br />
        <asp:Label ID="lblCname" runat="server" /><br />
        <asp:Label ID="lblUnits" runat="server" /><br />
        <asp:Label ID="lblAmount" runat="server" /><br />
    </div>
</form>
</body>
</html>
