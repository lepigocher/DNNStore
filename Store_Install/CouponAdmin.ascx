<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouponAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CouponAdmin" %>
<asp:placeholder id="plhGrid" runat="server">
    <div class="dnnForm">
        <fieldset>
            <div class="dnnFormItem">
                <asp:GridView ID="gvCoupons" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" PageSize="10" CellPadding="5" OnSorting="gvCoupons_Sorting" OnRowCreated="gvCoupons_RowCreated" OnPageIndexChanging="gvCoupons_PageIndexChanging" OnDataBinding="gvCoupons_DataBinding" CssClass="dnnGrid" HeaderStyle-CssClass="dnnGridHeader" RowStyle-CssClass="dnnGridItem" SelectedRowStyle-CssClass="dnnFormError" FooterStyle-CssClass="dnnGridFooter" PagerStyle-CssClass="dnnGridPager">
                    <Columns>
                        <asp:BoundField DataField="Code" SortExpression="Code" HeaderText="Code" />
                        <asp:BoundField DataField="Description" SortExpression="Description" HeaderText="Description" />
                        <asp:BoundField DataField="StartDate" SortExpression="StartDate" HeaderText="StartDate" DataFormatString="{0:d}" />
                        <asp:HyperLinkField DataNavigateUrlFields="CouponID" Text="Edit" ControlStyle-CssClass="dnnSecondaryAction" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="10" NextPageText=">" LastPageText=">>" />
                </asp:GridView>
            </div>
        </fieldset>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="linkAddNew" runat="server" resourcekey="linkAddNew" cssclass="dnnPrimaryAction">Add Coupon</asp:linkbutton></li>
        </ul>
    </div>
</asp:placeholder>
<asp:placeholder id="plhForm" runat="server" visible="false">
    <div class="dnnForm">
        <h2 class="dnnFormSectionHead"><asp:label id="lblEditTitle" runat="server"></asp:label></h2>
        <asp:placeholder id="plhEditControl" runat="server"></asp:placeholder>
    </div>
</asp:placeholder>
