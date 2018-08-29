<%@ Control language="c#" CodeBehind="ProductDetail.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ProductDetail" AutoEventWireup="True" %>
<asp:placeholder id="plhDetails" runat="server"></asp:placeholder>
<div class="dnnFormMessage dnnFormWarning" ID="divError" runat="server" Visible="false"><asp:Label ID="lblError" runat="server" resourcekey="NotEnoughProducts" CssClass="StoreDetailError"></asp:Label></div>
<asp:Panel ID="pnlReturn" runat="server" CssClass="StoreDetailReturnWrapper">
    <asp:HyperLink ID="lnkReturn" runat="server" CssClass="dnnSecondaryAction StoreDetailReturnButton">Return To Category</asp:hyperlink>
</asp:Panel>
<asp:Panel id="pnlReviews" runat="server" CssClass="StoreDetailReviewsWrapper">
    <p class="StoreReviews-Title"><asp:Label id="labelReviews" runat="server" resourcekey="labelReviews">Reviews</asp:Label></p>
    <asp:PlaceHolder id="plhReviews" runat="server"></asp:PlaceHolder>
</asp:Panel>
