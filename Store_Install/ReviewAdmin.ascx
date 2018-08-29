<%@ Control language="c#" CodeBehind="ReviewAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ReviewAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:placeholder id="panelList" Visible="true" runat="server">
    <div class="dnnForm">
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblStatus" runat="server" controlname="cmbStatus"></dnn:label>
                <asp:DropDownList id="cmbStatus" runat="server" Width="200" AutoPostBack="true" DataTextField="StatusName" DataValueField="StatusID" onselectedindexchanged="cmbStatus_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblCategory" runat="server" controlname="cmbCategory"></dnn:label>
                <asp:DropDownList id="cmbCategory" runat="server" AutoPostBack="true" DataTextField="CategoryPathName" DataValueField="CategoryID" onselectedindexchanged="cmbCategory_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblProduct" runat="server" controlname="cmbProduct"></dnn:label>
                <asp:DropDownList id="cmbProduct" runat="server" AutoPostBack="true" DataTextField="ProductTitle" DataValueField="ProductID" onselectedindexchanged="cmbProduct_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="dnnFormItem">
                <asp:datagrid id="grdReviews" runat="server" showheader="true" showfooter="false" autogeneratecolumns="false" width="100%" AllowPaging="True" PageSize="20" CssClass="dnnGrid">
                    <headerstyle cssclass="dnnGridHeader"/>
                    <itemstyle cssclass="dnnGridItem" />
                    <alternatingitemstyle cssclass="dnnGridAltItem" />
                    <columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Label id="lblSubmitter" Runat="server" resourcekey="lblSubmitter">Submitter</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:label id="labelUserName" runat="server" cssclass="Normal"><%# DataBinder.Eval(Container.DataItem, "UserName") %></asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Label id="lblProduct" Runat="server" resourcekey="lblProduct">Product</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:label id="lblProduct2" runat="server" cssclass="Normal"><%# DataBinder.Eval(Container.DataItem, "ModelName") %></asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Label id="lblRating" Runat="server" resourcekey="lblRating">Rating</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:PlaceHolder id="phRating" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                                <asp:Label id="lblComments" Runat="server" resourcekey="lblComments">Comments</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:label id="labelComments" runat="server" cssclass="Normal"><%# DataBinder.Eval(Container.DataItem, "Comments") %></asp:label>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:HyperLink id="linkEdit" Text="Edit" runat="server" cssclass="dnnPrimaryAction" resourcekey="linkEdit"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </columns>
                    <PagerStyle Mode="NumericPages" HorizontalAlign="center" cssclass="dnnGridPager"></PagerStyle>
                </asp:datagrid>
            </div>
        </fieldset>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="linkAddNew" runat="server" cssclass="dnnPrimaryAction" Visible="False" resourcekey="linkAddNew">Add Review</asp:linkbutton></li>
        </ul>
    </div>
</asp:placeholder>
<asp:placeholder id="panelEdit" Visible="false" runat="server"></asp:placeholder>
