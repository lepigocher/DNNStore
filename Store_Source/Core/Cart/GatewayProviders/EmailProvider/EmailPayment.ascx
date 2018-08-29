<%@ Control language="c#" CodeBehind="EmailPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Core.Cart.EmailPayment" AutoEventWireup="True" %>
<asp:Label id="lblError" runat="server" CssClass="NormalRed"></asp:Label>
<asp:Panel id="pnlProceedToEmail" runat="server" CssClass="StoreAccountCheckoutEmailProvider">
    <p>
        <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
        <br />
        <asp:Button ID="btnConfirmOrder" runat="server" resourcekey="btnConfirmOrder" Text="Confirm Order" OnClick="btnConfirmOrder_Click" CssClass="dnnPrimaryAction" />
    </p>
</asp:Panel>