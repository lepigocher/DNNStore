<%@ Control language="c#" CodeBehind="CountryTaxAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider.CountryTaxAdmin" AutoEventWireup="True" %>
<%@ Import Namespace="DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <h2 class="dnnFormSectionHead"><a href="#"><asp:Label ID="shTaxProvider" runat="server" ResourceKey="shTaxProvider">Tax Provider</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="lblEnableTax" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbEnableTax" runat="server" />
        </div>
        <div class="dnnFormItem">
			<dnn:label id="lblDefaultTaxRate" runat="server" controlname="txtDefaultTaxRate"></dnn:label>
			<asp:textbox id="txtDefaultTaxRate" runat="server" />
            <asp:CustomValidator ID="valCustDefaultTaxRate" runat="server" ControlToValidate="txtDefaultTaxRate" OnServerValidate="valCustTaxRate_ServerValidate" Display="Dynamic" SetFocusOnError="True" ResourceKey="valCustDefaultTaxRate" CssClass="dnnFormMessage dnnFormError"></asp:CustomValidator>
        </div>
        <div class="dnnFormItem">
            <asp:DataGrid ID="grdCountryTaxRates" runat="server" showheader="true" showfooter="true" autogeneratecolumns="false" width="100%" AllowPaging="False" CellPadding="5" HeaderStyle-CssClass="ShippingAndTaxHeaders" FooterStyle-CssClass="ShippingAndTaxHeaders">
                <Columns>
                    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblCountry" runat="server" resourcekey="lblCountry" cssclass="NormalBold">Country</asp:Label>
                        </HeaderTemplate>
					    <ItemTemplate>
						    <asp:Label id="lblCountryName" runat="server" cssclass="Normal" Text='<%# GetCountryName((CountryTaxInfo)Container.DataItem) %>'></asp:Label>
					    </ItemTemplate>
					    <FooterTemplate>
                            <asp:DropDownList ID="ddlCountries" runat="server"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblRegion" runat="server" resourcekey="lblRegion" cssclass="NormalBold">Region</asp:Label>
                        </HeaderTemplate>
					    <ItemTemplate>
						    <asp:Label id="lblRegionName" runat="server" cssclass="Normal" Text='<%# GetRegionName((CountryTaxInfo)Container.DataItem) %>'></asp:Label>
					    </ItemTemplate>
					    <FooterTemplate>
                            <asp:DropDownList ID="ddlRegions" runat="server" Visible="false"></asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblZipCode" runat="server" resourcekey="lblZipCode" cssclass="NormalBold">Zip Code</asp:Label>
                        </HeaderTemplate>
					    <ItemTemplate>
						    <asp:Label id="lblZipPattern" runat="server" cssclass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "ZipCode") %>'></asp:Label>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtZipCode" runat="server" cssclass="NormalTextBox"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblTaxRateTitle" Runat="server" resourcekey="lblTaxRateTitle" cssclass="NormalBold">Tax Rate</asp:Label>
                        </HeaderTemplate>
					    <ItemTemplate>
					        <asp:TextBox id="txtTaxRate" runat="server" cssclass="NormalTextBox" Text='<%# DataBinder.Eval(Container.DataItem, "TaxRate", "{0:N}") %>'></asp:TextBox>
                            <asp:CustomValidator ID="valCustTaxRate" runat="server" ControlToValidate="txtTaxRate" OnServerValidate="valCustTaxRate_ServerValidate" Display="Dynamic" SetFocusOnError="True" ResourceKey="valCustTaxRate" CssClass="dnnFormMessage dnnFormError"></asp:CustomValidator>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtTaxRate" runat="server" cssclass="NormalTextBox"></asp:TextBox>
                            <asp:CustomValidator ID="valCustTaxRate" runat="server" ControlToValidate="txtTaxRate" OnServerValidate="valCustTaxRate_ServerValidate" Display="Dynamic" SetFocusOnError="True" ResourceKey="valCustTaxRate" CssClass="dnnFormMessage dnnFormError"></asp:CustomValidator>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateColumn>
				    <asp:TemplateColumn>
                        <HeaderTemplate>
						    <asp:Label ID="lblDelete" Runat="server" resourcekey="lblDelete" cssclass="NormalBold">Delete</asp:Label>
					    </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox Runat="server" ID="chkDelete" cssclass="Normal"></asp:CheckBox>
                        </ItemTemplate>
                        <FooterTemplate>
					        <asp:LinkButton ID="lnkAddNew" runat="server" resourcekey="lnkAddNew" Text="Add" CssClass="dnnSecondaryAction" CommandName="Add"></asp:LinkButton>
					    </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="btnSaveTaxRates" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnSaveTaxRates">Update Tax Settings</asp:linkbutton></li>
        </ul>
    </fieldset>
</div>
