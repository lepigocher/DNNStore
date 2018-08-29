<%@ Control language="c#" CodeBehind="ProductAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ProductAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="panelList" runat="server" visible="true">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblCategory" controlname="lblCategory" runat="server"></dnn:label>
            <asp:DropDownList id="cmbCategory" runat="server" AutoPostBack="true" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <asp:datagrid id="grdProducts" runat="server" showheader="true" showfooter="false" autogeneratecolumns="false" AllowPaging="True" cellpadding="5" PageSize="20" CssClass="dnnGrid">
                <headerstyle cssclass="dnnGridHeader"/>
                <itemstyle cssclass="dnnGridItem" />
                <alternatingitemstyle cssclass="dnnGridAltItem" />
                <pagerstyle cssclass="dnnGridPager" />
                <columns>
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label id="lblModelNumber" runat="server" resourcekey="lblModelNumber">Model Number</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="labelModelNumber" runat="server"><%# DataBinder.Eval(Container.DataItem, "ModelNumber") %></asp:label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn ItemStyle-Width="100%">
                        <HeaderTemplate>
                            <asp:Label id="lblProductName" runat="server" resourcekey="lblProductName">Product Name</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="labelProductName" runat="server"><%# DataBinder.Eval(Container.DataItem, "ModelName") %></asp:label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label id="lblQuantity" runat="server" resourcekey="lblQuantity">Quantity</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="labelQuantity" runat="server"><%# DataBinder.Eval(Container.DataItem, "StockQuantity")%></asp:label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label id="lblArchived" runat="server" resourcekey="lblArchived">Archived</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label id="labelArchived" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label id="lblFeatured" runat="server" resourcekey="lblFeatured">Featured</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="labelFeatured" runat="server"></asp:label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <HeaderTemplate>
                            <asp:Label id="lblPrice" runat="server" resourcekey="lblPrice">Price</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="labelPrice" runat="server"></asp:label>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:HyperLink id="linkEdit" resourcekey="linkEdit" runat="server" cssclass="dnnPrimaryAction"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
                        <ItemTemplate>
                            <asp:HyperLink id="linkCopy" resourcekey="linkCopy" runat="server" cssclass="dnnSecondaryAction"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </columns>
                <PagerStyle mode="NumericPages" horizontalalign="center"></PagerStyle>
            </asp:datagrid>
        </div>
        <div class="dnnFormItem">
            <asp:linkbutton id="linkAddNew" runat="server" resourcekey="linkAddNew" cssclass="dnnPrimaryAction">Add Product</asp:linkbutton>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="labelAuthorized" runat="server" controlname="chkAuthorized" Visible="False"></dnn:label>
            <asp:CheckBox ID="chkAuthorized" runat="server" Visible="False" />
        </div>
    </fieldset>
</div>
<div  id="panelEdit" runat="server" visible="false">
    <asp:PlaceHolder id="editControl" runat="server"></asp:PlaceHolder>
</div>
