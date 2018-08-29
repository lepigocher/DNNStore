<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CartDetail.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CartDetail" %>
<asp:Label id="lblCartEmpty" runat="server" resourcekey="lblCartEmpty" Visible="false" CssClass="NormalRed StoreCartEmpty"></asp:Label>
<asp:datagrid id="grdItems" runat="server" showheader="true" showfooter="true" autogeneratecolumns="false" allowpaging="false" CssClass="StoreCartDetail dnnGrid" GridLines="None" OnItemDataBound="grdItems_ItemDataBound" DataKeyField="ItemID" OnItemCommand="grdItems_ItemCommand">
    <HeaderStyle CssClass="StoreCartDetailHeader dnnGridHeader" />
    <ItemStyle CssClass="StoreCartDetailItem dnnGridItem" />
    <AlternatingItemStyle CssClass="StoreCartDetailAlternatingItem dnnGridAltItem" />
    <FooterStyle CssClass="StoreCartDetailFooter dnnGridFooter" />
    <columns>
        <asp:templatecolumn>
            <itemtemplate>
                <asp:Image id="imgThumbnail" runat="server" />
            </itemtemplate>
            <ItemStyle CssClass="StoreCartDetailThumbnail" />
        </asp:templatecolumn>
        <asp:templatecolumn>
            <headertemplate>
                <asp:Label id="lblProduct" Runat="server" resourcekey="lblProduct">Product</asp:Label>
            </headertemplate>
            <itemtemplate>
                <a id="lnkTitle" runat="server" visible="false"></a>
                <asp:Label ID="lblTitle" runat="server" Visible="false"></asp:Label>
            </itemtemplate>
            <footertemplate>
                <asp:Button ID="btnUpdateCart" runat="server" resourcekey="btnUpdateCart" CssClass="dnnSecondaryAction StoreCartDetailUpdateCart" Text="Update Cart" CommandName="UpdateCart" ValidationGroup="vgCart" />
            </footertemplate>
            <HeaderStyle CssClass="StoreCartDetailProductHeader" />
            <ItemStyle CssClass="StoreCartDetailProduct" />
            <FooterStyle CssClass="StoreCartDetailProductFooter" HorizontalAlign="Right" />
        </asp:templatecolumn>
        <asp:templatecolumn>
            <headertemplate>
                <asp:Label id="lblPriceHeader" Runat="server" resourcekey="lblPriceHeader">Price</asp:Label>
            </headertemplate>
            <itemtemplate>
                <asp:label id="lblPrice" runat="server"></asp:label>
            </itemtemplate>
            <footertemplate>
                <asp:Label id="lblTotals" Runat="server" resourcekey="lblTotals">Basket total:</asp:Label>
            </footertemplate>
            <HeaderStyle CssClass="StoreCartDetailPriceHeader" />
            <ItemStyle CssClass="StoreCartDetailPrice" />
            <FooterStyle CssClass="StoreCartDetailPriceFooter" />
        </asp:templatecolumn>
        <asp:templatecolumn>
            <headertemplate>
                <asp:Label id="lblQty" Runat="server" resourcekey="lblQty">Qty</asp:Label>
            </headertemplate>
            <itemtemplate>
                <asp:TextBox id="txtQuantity" runat="server" CssClass="StoreCartDetailQuantityTextBox"></asp:TextBox>
                <asp:ImageButton ID="ibUpdate" runat="server" CausesValidation="True" ImageUrl="Templates/Images/cart_put.png" resourcekey="ibUpdate" ValidationGroup="vgCart" />
                <asp:RequiredFieldValidator ID="valReqQuantity" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" Text="*" SetFocusOnError="True" CssClass="NormalRed" ValidationGroup="vgCart"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="valCompQuantity" runat="server" ControlToValidate="txtQuantity" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" Text="*" SetFocusOnError="True" CssClass="NormalRed" ValidationGroup="vgCart"></asp:CompareValidator>
                <asp:CustomValidator ID="valCustQuantity" runat="server" ControlToValidate="txtQuantity" Display="Dynamic" Text="*" OnServerValidate="valCustQuantity_ServerValidate" SetFocusOnError="True" CssClass="NormalRed" ValidationGroup="vgCart"></asp:CustomValidator>
            </itemtemplate>
            <footertemplate>
                <asp:label id="lblCount" runat="server"></asp:label>
            </footertemplate>
            <HeaderStyle CssClass="StoreCartDetailQuantityHeader" />
            <ItemStyle CssClass="StoreCartDetailQuantity" />
            <FooterStyle CssClass="StoreCartDetailQuantityFooter" />
        </asp:templatecolumn>
        <asp:templatecolumn>
            <headertemplate>
                <asp:Label id="lblSubtotalHeader" Runat="server" resourcekey="lblSubtotal">Subtotal</asp:Label>
            </headertemplate>
            <itemtemplate>
                <asp:label id="lblSubtotal" runat="server"></asp:label>
            </itemtemplate>
            <footertemplate>
                <asp:label id="lblTotal" runat="server"></asp:label>
            </footertemplate>
            <HeaderStyle CssClass="StoreCartDetailSubtotalHeader"/>
            <ItemStyle CssClass="StoreCartDetailSubtotal" />
            <FooterStyle CssClass="StoreCartDetailSubtotalFooter" />
        </asp:templatecolumn>
        <asp:TemplateColumn>
            <ItemTemplate>
                <asp:ImageButton ID="ibDelete" runat="server" CausesValidation="False" ImageUrl="Templates/Images/cart_delete.png" resourcekey="ibDelete" />
            </ItemTemplate>
            <ItemStyle CssClass="StoreCartDetailDelete" />
        </asp:TemplateColumn>
    </columns>
</asp:datagrid>
<asp:ValidationSummary ID="valSummaryCart" runat="server" DisplayMode="List" CssClass="dnnFormMessage dnnFormValidationSummary" ValidationGroup="vgCart" />
