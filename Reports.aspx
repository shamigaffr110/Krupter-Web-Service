<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.cs" Inherits="EBillingWeb.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Reports</h2>
<label>Enter Last 'N' Number of Bills To Generate</label>
<asp:TextBox ID="txtN" runat="server"></asp:TextBox>
<asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
<asp:GridView ID="grid" runat="server" AutoGenerateColumns="false">
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
