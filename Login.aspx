<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.cs" Inherits="EBillingWeb.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Login</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="card">
    <h3>Admin Login</h3>
    <asp:Label runat="server" Text="Username"></asp:Label>
    <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
    <asp:Label runat="server" Text="Password"></asp:Label>
    <asp:TextBox ID="txtPass" runat="server" TextMode="Password"></asp:TextBox>
    <br />
    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</div>

</asp:Content>
