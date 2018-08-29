<%@ Control language="c#" CodeBehind="SystempayPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Core.Cart.SystempayPayment" AutoEventWireup="True" %>
<asp:Label id="lblError" runat="server" CssClass="NormalRed" Visible="false"></asp:Label>
<asp:Panel id="pnlProceedToSystempay" runat="server" Visible="true" CssClass="StoreAccountCheckoutSystempayProvider">
    <p>
        <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
	    <br />
        <asp:Image ID="systempayimage" runat="server" AlternateText="Click here to pay by Systempay using your credit/debit card" /><br />
        <asp:Button ID="btnConfirmOrder" runat="server" resourcekey="btnConfirmOrder" Text="Confirm Order" OnClick="btnConfirmOrder_Click" CssClass="StandardButton" />
    </p>
</asp:Panel>
