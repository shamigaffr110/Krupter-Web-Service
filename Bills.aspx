<%@ Page Title="Bills" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bills.cs" Inherits="EBillingWeb.Bills" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Bills</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="title"><b>Add Customer Bills</b></div>
<div class="card">
    <label>Number of bills to be added</label>
    <asp:TextBox ID="txtCount" runat="server"></asp:TextBox>
    <asp:Button ID="btnPrepare" runat="server" Text="Prepare" OnClick="btnPrepare_Click" />
    <asp:Label ID="lblPrepMsg" runat="server" ForeColor="Red"></asp:Label>
</div>

<asp:PlaceHolder ID="phRows" runat="server"></asp:PlaceHolder>
<asp:Button ID="btnSaveAll" runat="server" Text="Calculate + Save All" OnClick="btnSaveAll_Click" Visible="false" />
<asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>

<hr />
<div class="title"><b>Retrieve last N bills</b></div>
<div class="card">
    <label>Enter Last 'N' Number of Bills To Generate</label>
    <asp:TextBox ID="txtLastN" runat="server"></asp:TextBox>
    <asp:Button ID="btnFetch" runat="server" Text="Fetch" OnClick="btnFetch_Click" />
</div>
<asp:GridView ID="grdBills" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="ConsumerNumber" HeaderText="Consumer No" />
        <asp:BoundField DataField="ConsumerName" HeaderText="Name" />
        <asp:BoundField DataField="UnitsConsumed" HeaderText="Units" />
        <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" DataFormatString="{0:N2}" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <a class="btn" href='<%# "BillPrint.aspx?cno=" + Eval("ConsumerNumber") %>' target="_blank">Print</a>
                <a class="btn" href='<%# "BillDownload.aspx?cno=" + Eval("ConsumerNumber") %>'>Download</a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>
