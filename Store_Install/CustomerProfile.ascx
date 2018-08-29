<%@ Control language="c#" CodeBehind="CustomerProfile.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerProfile" AutoEventWireup="True" %>
<asp:Panel id="pnlLoginMessage" Runat="server">
    <div class="dnnFormMessage dnnFormWarning"><asp:Label id="lblLoginMessage" Runat="server" CssClass="StoreAccountAddressesLogin">Please login to view profile settings.</asp:Label></div>
</asp:Panel>
<asp:Panel id="pnlAddressProvider" Runat="server" Visible="False" CssClass="StoreAccountAddressesWrapper">
    <asp:placeholder id="plhAddressProvider" runat="server"></asp:placeholder>
</asp:Panel>
