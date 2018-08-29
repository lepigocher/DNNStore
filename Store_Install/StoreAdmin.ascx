<%@ Control language="c#" CodeBehind="StoreAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.StoreAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<dnn:Label id="lblParentTitle" ResourceKey="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:Label>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="lblStoreName" runat="server" controlname="txtStoreName"></dnn:Label>
            <asp:textbox id="txtStoreName" runat="server"></asp:textbox>
            <asp:RequiredFieldValidator ID="valReqStoreName" runat="server" ControlToValidate="txtStoreName" ErrorMessage="Store Name is required!" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqStoreName" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblSEOFeature" runat="server" ControlName="chkSEOFeature"></dnn:Label>
            <asp:CheckBox id="chkSEOFeature" runat="server" AutoPostBack="True" OnCheckedChanged="chkSEOFeature_CheckedChanged" />
        </div>
        <div class="dnnFormItem" id="trDescription" runat="server">
            <dnn:Label id="lblDescription" runat="server" controlname="txtDescription"></dnn:Label>
            <asp:textbox id="txtDescription" runat="server" textmode="multiline" rows="4"></asp:textbox>
        </div>
        <div class="dnnFormItem" id="trKeywords" runat="server">
            <dnn:Label id="lblKeywords" runat="server" controlname="txtKeywords"></dnn:Label>
            <asp:textbox id="txtKeywords" runat="server" textmode="multiline" rows="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblEmail" runat="server" controlname="txtEmail"></dnn:Label>
            <asp:textbox id="txtEmail" runat="server"></asp:textbox>
            <asp:RequiredFieldValidator ID="valReqStoreEmail" runat="server" ErrorMessage="Store Email is required!" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True" resourcekey="valReqStoreEmail" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valRegExEmail" runat="server" ErrorMessage="A valid email is required!" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valRegExEmail" CssClass="dnnFormMessage dnnFormError"></asp:RegularExpressionValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblCurrencySymbol" runat="server" controlname="txtCurrencySymbol"></dnn:Label>
            <asp:textbox id="txtCurrencySymbol" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblUsePortalTemplates" runat="server"></dnn:Label>
            <asp:checkbox id="chkUsePortalTemplates" runat="server" AutoPostBack="True" OnCheckedChanged="chkUsePortalTemplates_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblStyleSheet" runat="server" controlname="lstStyleSheet"></dnn:Label>
            <asp:dropdownlist id="lstStyleSheet" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblStorePageID" runat="server" controlname="lstStorePageID"></dnn:Label>
            <asp:dropdownlist id="lstStorePageID" runat="server" autopostback="False"></asp:dropdownlist>
            <asp:RequiredFieldValidator ID="valReqStorePageID" runat="server" ErrorMessage="Select the catalog page!" ControlToValidate="lstStorePageID" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqStorePageID" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblShoppingCartPageID" runat="server" controlname="lstShoppingCartPageID"></dnn:Label>
            <asp:dropdownlist id="lstShoppingCartPageID" runat="server" autopostback="False"></asp:dropdownlist>
            <asp:RequiredFieldValidator ID="valReqShoppingCartPageID" runat="server" ControlToValidate="lstShoppingCartPageID" Display="Dynamic" ErrorMessage="Select the shopping cart page!" SetFocusOnError="True" InitialValue="-1" resourcekey="valReqShoppingCartPageID" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblAuthorizeCancel" runat="server" ControlName="chkAuthorizeCancel"></dnn:Label>
            <asp:checkbox id="chkAuthorizeCancel" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblInventoryManagement" runat="server" ControlName="chkInventoryManagement"></dnn:Label>
            <asp:checkbox id="chkInventoryManagement" runat="server" OnCheckedChanged="chkInventoryManagement_CheckedChanged" AutoPostBack="True"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="trOutOfStock" runat="server">
            <dnn:Label id="lblOutOfStock" runat="server" controlname="lstOutOfStock"></dnn:Label>
            <asp:dropdownlist id="lstOutOfStock" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trProductsBehavior" runat="server">
            <dnn:Label id="lblProductsBehavior" runat="server" controlname="lstProductsBehavior"></dnn:Label>
            <asp:dropdownlist id="lstProductsBehavior" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trAvoidNegativeStock" runat="server">
            <dnn:Label id="lblAvoidNegativeStock" runat="server" ControlName="chkAvoidNegativeStock"></dnn:Label>
            <asp:checkbox id="chkAvoidNegativeStock" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblOrderRole" runat="server" controlname="lstOrderRole"></dnn:Label>
            <asp:dropdownlist id="lstOrderRole" runat="server" autopostback="False"></asp:dropdownlist>
            <asp:RequiredFieldValidator ID="valReqOrderRole" runat="server" ErrorMessage="Select a role!" ControlToValidate="lstOrderRole" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqOrderRole" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblCatalogRole" runat="server" controlname="lstCatalogRole"></dnn:Label>
            <asp:dropdownlist id="lstCatalogRole" runat="server" autopostback="False"></asp:dropdownlist>
            <asp:RequiredFieldValidator ID="valReqCatalogRole" runat="server" ErrorMessage="Select a role!" ControlToValidate="lstCatalogRole" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqCatalogRole" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem" id="rowSecureCookie" runat="server">
            <dnn:Label id="lblSecureCookie" runat="server" ControlName="chkSecureCookie"></dnn:Label>
            <asp:checkbox id="chkSecureCookie" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblCheckoutMode" runat="server" controlname="lstCheckoutMode"></dnn:Label>
            <asp:dropdownlist id="lstCheckoutMode" runat="server" autopostback="True" OnSelectedIndexChanged="lstCheckoutMode_SelectedIndexChanged"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trImpersonatedUser" runat="server" visible="false">
            <dnn:Label id="lblImpersonatedUserID" runat="server" controlname="lstImpersonatedUserID"></dnn:Label>
            <table id="tbImpersonatedUserSelection" runat="server" Visible="False">
                <tr>
                    <td>
                        <asp:dropdownlist id="lstImpersonatedRoleID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstImpersonatedRoleID_SelectedIndexChanged"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqImpersonatedRoleID" runat="server" ControlToValidate="lstImpersonatedRoleID" Display="Dynamic" ErrorMessage="Select a role!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqImpersonatedRoleID" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trImpersonatedUserID" runat="server">
                    <td>
                        <asp:dropdownlist id="lstImpersonatedUserID" runat="server" autopostback="False"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqImpersonatedUserID" runat="server" ControlToValidate="lstImpersonatedUserID" Display="Dynamic" ErrorMessage="Select an account!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqImpersonatedUserID" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="trValidateUser" runat="server">
                    <td>
                        <asp:LinkButton ID="btnValidateUser" runat="server" Text="Validate" 
                            ResourceKey="btnValidateUser" CssClass="dnnPrimaryAction" 
                            onclick="btnValidateUser_Click"/>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblImpersonatedUser" runat="server" Visible="False"></asp:Label>&nbsp;
            <asp:LinkButton ID="btnChangeImpersonatedUser" runat="server" 
                Text="Change User" ResourceKey="btnChangeImpersonatedUser" 
                CssClass="dnnPrimaryAction" onclick="btnChangeImpersonatedUser_Click"/>
            <asp:HiddenField ID="hidImpersonatedUserID" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblNoDelivery" runat="server" ControlName="chkNoDelivery"></dnn:Label>
            <asp:checkbox id="chkNoDelivery" runat="server" AutoPostBack="True" oncheckedchanged="chkNoDelivery_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="trAllowVirtualProducts" runat="server">
            <dnn:Label id="lblAllowVirtualProducts" runat="server" ControlName="chkAllowVirtualProducts"></dnn:Label>
            <asp:checkbox id="chkAllowVirtualProducts" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblAllowCoupons" runat="server" ControlName="chkAllowCoupons"></dnn:Label>
            <asp:checkbox id="chkAllowCoupons" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblAllowFreeShipping" runat="server" ControlName="chkAllowFreeShipping"></dnn:Label>
            <asp:checkbox id="chkAllowFreeShipping" runat="server" AutoPostBack="True" oncheckedchanged="chkAllowFreeShipping_CheckedChanged"></asp:checkbox>
        </div>
        <div id="trFreeShipping" runat="server">
            <div class="dnnFormItem">
                <dnn:Label id="lblMinOrderAmount" runat="server" controlname="txtMinOrderAmount"></dnn:Label>
                <asp:textbox id="txtMinOrderAmount" runat="server" width="100"></asp:textbox>
                <asp:RequiredFieldValidator ID="valReqMinOrderAmount" runat="server" ControlToValidate="txtMinOrderAmount" ErrorMessage="Min order amount is required!" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqMinOrderAmount" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valCompMinOrderAmount" runat="server" ErrorMessage="Error! Please enter a valid amount." resourcekey="valCompMinOrderAmount" Type="Currency" ControlToValidate="txtMinOrderAmount" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
			    <dnn:Label id="lblRestrictToCountries" runat="server" ControlName="chkRestrictToCountries"></dnn:Label>
	            <asp:CheckBox ID="chkRestrictToCountries" runat="server" AutoPostBack="True" oncheckedchanged="chkRestrictToCountries_CheckedChanged" />
            </div>
            <div class="dnnFormItem" id="trAuthorizedCountries" runat="server">
			    <dnn:Label id="lblAuthorizedCountries" runat="server" ControlName="lbAuthorizedCountries"></dnn:Label>
                <asp:ListBox ID="lbAuthorizedCountries" runat="server" DataTextField="Text" 
                    DataValueField="Value" Rows="10" SelectionMode="Multiple"></asp:ListBox>
                <asp:RequiredFieldValidator ID="valReqAuthorizedCountries" runat="server" ControlToValidate="lbAuthorizedCountries" 
                    ErrorMessage="valReqAuthorizedCountries" resourcekey="valReqAuthorizedCountries"
                    Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblOnPaidOrder" runat="server" ControlName="lstOnPaidOrderRole"></dnn:Label>
            <asp:dropdownlist id="lstOnPaidOrderRole" runat="server"></asp:dropdownlist>
        </div>
        <div class="dnnFormMessage dnnFormWarning" id="trProviders" runat="server">
            <asp:Label ID="lblProviders" runat="server" resourcekey="lblProviders"></asp:Label>
        </div>
        <div class="dnnFormItem">
			<dnn:Label id="lblAddressProvider" runat="server" controlname="lstAddressProviders"></dnn:Label>
			<asp:dropdownlist id="lstAddressProviders" runat="server" autopostback="True" onselectedindexchanged="lstAddressProviders_SelectedIndexChanged"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblTaxProvider" runat="server" controlname="lstTaxProviders"></dnn:Label>
            <asp:dropdownlist id="lstTaxProviders" runat="server" autopostback="True" onselectedindexchanged="lstTaxProvider_SelectedIndexChanged"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trShippingProviderSelection" runat="server">
            <dnn:Label id="lblShippingProvider" runat="server" controlname="lstShippingProviders"></dnn:Label>
            <asp:dropdownlist id="lstShippingProviders" runat="server" autopostback="True" onselectedindexchanged="lstShippingProvider_SelectedIndexChanged"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblGateway" runat="server" controlname="lstGateway"></dnn:Label>
            <asp:dropdownlist id="lstGateway" runat="server" autopostback="True" onselectedindexchanged="lstGateway_SelectedIndexChanged"></asp:dropdownlist>
            <asp:RequiredFieldValidator ID="valReqGateway" runat="server" ControlToValidate="lstGateway" Display="Dynamic" ErrorMessage="Select a gateway provider!" SetFocusOnError="True" resourcekey="valReqGateway" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
    </fieldset>
    <h2 id="fshGatewayProvider" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shGatewayProvider" runat="server" ResourceKey="shGatewayProvider">Gateway Provider</asp:Label> </a></h2>
    <fieldset>
        <asp:placeholder id="plhGatewayProvider" runat="server" />
    </fieldset>
   <ul class="dnnActions dnnClear">
        <li>
            <asp:linkbutton id="btnSave" runat="server" cssclass="dnnPrimaryAction" resourcekey="btnSave" onclick="btnSave_Click">Update</asp:linkbutton>
        </li>
    </ul>
</div>
<asp:placeholder id="plhAddressProvider" runat="server" />
<asp:placeholder id="plhTaxProvider" runat="server" />
<asp:placeholder id="plhShippingProvider" runat="server" />
<script type="text/javascript">
    jQuery(function ($) {
        var setupStoreAdmin = function () {
            $('.dnnForm').dnnPanels();
        };

        setupStoreAdmin();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            // note that this will fire when _any_ UpdatePanel is triggered,
            // which may or may not cause an issue
            setupStoreAdmin();
        });
    });
</script>
	