<%@ Control language="c#" CodeBehind="CategoryAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CategoryAdmin" AutoEventWireup="True" %>
<asp:placeholder id="plhGrid" runat="server">
    <div class="dnnForm">
        <fieldset>
            <div class="dnnFormItem">
                <asp:GridView ID="gvCategories" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false" PageSize="10" OnSorting="gvCategories_Sorting" OnRowCreated="gvCategories_RowCreated" OnPageIndexChanging="gvCategories_PageIndexChanging" OnDataBinding="gvCategories_DataBinding" CssClass="dnnGrid" HeaderStyle-CssClass="dnnGridHeader" RowStyle-CssClass="dnnGridItem" SelectedRowStyle-CssClass="dnnFormError" FooterStyle-CssClass="dnnGridFooter" PagerStyle-CssClass="dnnGridPager" BorderStyle="NotSet" GridLines="Both">
                    <Columns>
                        <asp:BoundField DataField="Name" SortExpression="CategoryName" HeaderText="CategoryName" />
                        <asp:BoundField DataField="ParentCategoryName" SortExpression="ParentCategoryName" HeaderText="ParentCategoryName" />
                        <asp:BoundField DataField="OrderID" SortExpression="OrderID" HeaderText="OrderID" />
                        <asp:BoundField DataField="CreatedDate" SortExpression="CreatedDate" HeaderText="CreatedDate" DataFormatString="{0:g}" />
                        <asp:HyperLinkField DataNavigateUrlFields="CategoryID" Text="Edit" ControlStyle-CssClass="dnnPrimaryAction" />
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" PreviousPageText="<" PageButtonCount="10" NextPageText=">" LastPageText=">>" />
                </asp:GridView>
            </div>
        </fieldset>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="linkAddNew" runat="server" resourcekey="linkAddNew" cssclass="dnnPrimaryAction">Add Category</asp:linkbutton></li>
        </ul>
    </div>
</asp:placeholder>
<asp:placeholder id="plhForm" runat="server" visible="false">
    <div class="dnnForm">
        <h2 class="dnnFormSectionHead"><asp:label id="lblEditTitle" runat="server"></asp:label></h2>
        <asp:placeholder id="plhEditControl" runat="server"></asp:placeholder>
    </div>
</asp:placeholder>
