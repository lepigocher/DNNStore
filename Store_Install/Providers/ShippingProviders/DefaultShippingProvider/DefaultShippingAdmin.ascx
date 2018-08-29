<%@ Control language="c#" CodeBehind="DefaultShippingAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider.DefaultShippingAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <h2 class="dnnFormSectionHead"><a href="#"><asp:Label ID="shShippingProvider" runat="server" ResourceKey="shShippingProvider">Shipping Provider</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="lblApplyTaxRate" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbApplyTaxRate" runat="server" />
        </div>
        <div class="dnnFormItem">
	        <asp:datagrid id="grdShippingRates" runat="server" showheader="true" showfooter="true" autogeneratecolumns="false" width="100%" AllowPaging="False" CellPadding="5" HeaderStyle-CssClass="ShippingAndTaxHeaders" FooterStyle-CssClass="ShippingAndTaxHeaders" DataKeyField="ID" >
			    <columns>
				    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblShippingRateDescriptionTitle" Runat="server" resourcekey="lblShippingRateDescriptionTitle" cssclass="NormalBold">Description</asp:Label>
                        </HeaderTemplate>
					    <ItemTemplate>
						    <asp:TextBox id="txtDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' ValidationGroup="ShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqDescription" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:RequiredFieldValidator>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtNewDescription" runat="server" ValidationGroup="NewShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqNewDescription" runat="server" ControlToValidate="txtNewDescription" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqDescription" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:RequiredFieldValidator>
                        </FooterTemplate>
				    </asp:TemplateColumn>
				    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblMinWeightTitle" Runat="server" resourcekey="lblMinWeightTitle" cssclass="NormalBold">Min. Weight</asp:Label>
					    </HeaderTemplate>
					    <ItemTemplate>
						    <asp:TextBox id="txtMinWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MinWeight", "{0:F}")%>' ValidationGroup="ShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqMinWeight" runat="server" ControlToValidate="txtMinWeight" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqMinWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompMinWeight" runat="server" ControlToValidate="txtMinWeight" Type="Double" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" ResourceKey="valCompMinWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:CompareValidator>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtNewMinWeight" runat="server" ValidationGroup="NewShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqNewMinWeight" runat="server" ControlToValidate="txtNewMinWeight" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqMinWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompNewMinWeight" runat="server" ControlToValidate="txtNewMinWeight" Type="Double" Operator="DataTypeCheck" ResourceKey="valCompMinWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:CompareValidator>
					    </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
				    </asp:TemplateColumn>
				    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblMaxWeightTitle" Runat="server" resourcekey="lblMaxWeightTitle" cssclass="NormalBold">Max. Weight</asp:Label>
					    </HeaderTemplate>
					    <ItemTemplate>
						    <asp:TextBox id="txtMaxWeight" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaxWeight", "{0:F}")%>' ValidationGroup="ShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqMaxWeight" runat="server" ControlToValidate="txtMaxWeight" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqMaxWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompMaxWeight" runat="server" ControlToValidate="txtMaxWeight" Type="Double" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" ResourceKey="valCompMaxWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:CompareValidator>
                            <asp:CompareValidator ID="valCompMinMaxWeight" runat="server" ControlToValidate="txtMaxWeight" ControlToCompare="txtMinWeight" Operator="GreaterThan" ResourceKey="valCompMaxGTMin" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:CompareValidator>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtNewMaxWeight" runat="server" ValidationGroup="NewShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqNewMaxWeight" runat="server" ControlToValidate="txtNewMaxWeight" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqMaxWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompNewMaxWeight" runat="server" ControlToValidate="txtNewMaxWeight" Type="Double" Operator="DataTypeCheck" ResourceKey="valCompMaxWeight" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate" EnableClientScript="False"></asp:CompareValidator>
                            <asp:CompareValidator ID="valCompNewMinMaxWeight" runat="server" ControlToValidate="txtNewMaxWeight" ControlToCompare="txtNewMinWeight" Operator="GreaterThan" ResourceKey="valCompMaxGTMin" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:CompareValidator>
					    </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
				    </asp:TemplateColumn>
				    <asp:TemplateColumn>
					    <HeaderTemplate>
						    <asp:Label ID="lblCostTitle" Runat="server" resourcekey="lblCostTitle" cssclass="NormalBold">Cost</asp:Label>
					    </HeaderTemplate>
					    <ItemTemplate>
						    <asp:TextBox id="txtCost" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Cost", "{0:F}") %>' ValidationGroup="ShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqCost" runat="server" ControlToValidate="txtCost" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqCost" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompCost" runat="server" ControlToValidate="txtCost" Type="Double" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" ResourceKey="valCompCost" CssClass="dnnFormMessage dnnFormError" ValidationGroup="ShippingRate"></asp:CompareValidator>
					    </ItemTemplate>
					    <FooterTemplate>
					        <asp:TextBox id="txtNewCost" runat="server" ValidationGroup="NewShippingRate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valReqNewCost" runat="server" ControlToValidate="txtNewCost" Display="Dynamic" SetFocusOnError="true" ResourceKey="valReqCost" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="valCompNewCost" runat="server" ControlToValidate="txtNewCost" Type="Double" Operator="DataTypeCheck" ResourceKey="valCompCost" CssClass="dnnFormMessage dnnFormError" ValidationGroup="NewShippingRate"></asp:CompareValidator>
					    </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
				    </asp:TemplateColumn>
				    <asp:TemplateColumn>
                        <HeaderTemplate>
						    <asp:Label ID="lblDelete" Runat="server" resourcekey="lblDelete" cssclass="NormalBold">Delete</asp:Label>
					    </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox Runat="server" ID="chkDelete" cssclass="Normal"></asp:CheckBox>
                        </ItemTemplate>
                        <FooterTemplate>
					        <asp:LinkButton ID="lnkAddNew" runat="server" resourcekey="lnkAddNew" Text="Add" CssClass="dnnSecondaryAction" CommandName="Add" ValidationGroup="NewShippingRate"></asp:LinkButton>
					    </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateColumn>
			    </columns>
				<PagerStyle Mode="NumericPages" HorizontalAlign="Center" CssClass="NormalBold"></PagerStyle>
                <FooterStyle CssClass="ShippingAndTaxHeaders" />
                <HeaderStyle CssClass="ShippingAndTaxHeaders" />
			</asp:datagrid>
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="btnSaveShippingFee" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnSaveShippingFee" ValidationGroup="ShippingRate">Update Shipping Rates</asp:linkbutton></li>
        </ul>
    </fieldset>
</div>
