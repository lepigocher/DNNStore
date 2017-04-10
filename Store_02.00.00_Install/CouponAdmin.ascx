<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouponAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CouponAdmin" %>
<asp:placeholder id="plhGrid" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tbody>
            <tr>
                <td align="center">
                    <asp:GridView ID="gvCoupons" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" PageSize="10" CellPadding="5" OnSorting="gvCoupons_Sorting" OnRowCreated="gvCoupons_RowCreated" OnPageIndexChanging="gvCoupons_PageIndexChanging" OnDataBinding="gvCoupons_DataBinding">
                        <Columns>
                            <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code" />
                            <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" />
                            <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" DataFormatString="{0:d}" />
                            <asp:HyperLinkField DataNavigateUrlFields="CouponID" Text="Edit" ControlStyle-CssClass="CommandButton" />
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="10" NextPageText=">" LastPageText=">>" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:linkbutton id="linkAddImage" runat="server" cssclass="Normal">
                        <asp:Image id="imageAdd" Runat="server" ImageUrl="~/images/edit.gif" AlternateText="Edit" resourcekey="Edit" />
                    </asp:linkbutton>
                    <asp:linkbutton id="linkAddNew" runat="server" resourcekey="linkAddNew" cssclass="CommandButton">Add Coupon</asp:linkbutton>
                </td>
            </tr>
        </tbody>
    </table>
</asp:placeholder>
<asp:placeholder id="plhForm" runat="server" visible="false">
    <table cellspacing="0" cellpadding="0" border="0" width="100%">
        <tbody align="left">
            <tr>
                <td align="center">
                    <asp:label id="lblEditTitle" runat="server" cssclass="SubHead"></asp:label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:placeholder id="plhEditControl" runat="server"></asp:placeholder>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </tbody>
    </table>
</asp:placeholder>
