<%@ Control language="c#" CodeBehind="StoreAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.StoreAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<dnn:label id="lblParentTitle" ResourceKey="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
<table cellpadding="0" cellspacing="0" style="border: 0; text-align: center;">
    <tr>
        <td>
            <table style="text-align:left;">
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblStoreName" runat="server" controlname="txtStoreName"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:textbox id="txtStoreName" runat="server" width="300" CssClass="NormalTextBox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="valReqStoreName" runat="server" ControlToValidate="txtStoreName" ErrorMessage="* Store Name is required!" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqStoreName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblSEOFeature" runat="server" ControlName="chkSEOFeature"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:CheckBox id="chkSEOFeature" runat="server" CssClass="NormalTextBox" AutoPostBack="True" OnCheckedChanged="chkSEOFeature_CheckedChanged" />
                    </td>
                </tr>
                <tr id="trDescription" runat="server" style="vertical-align:top;">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblDescription" runat="server" controlname="txtDescription"></dnn:label>
                    </td>
                    <td>
                        <asp:textbox id="txtDescription" runat="server" textmode="multiline" rows="4" width="300" CssClass="NormalTextBox"></asp:textbox>
                    </td>
                </tr>
                <tr id="trKeywords" runat="server" style="vertical-align:top;">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblKeywords" runat="server" controlname="txtKeywords"></dnn:label>
                    </td>
                    <td>
                        <asp:textbox id="txtKeywords" runat="server" textmode="multiline" rows="4" width="300" CssClass="NormalTextBox"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblEmail" runat="server" controlname="txtEmail"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:textbox id="txtEmail" runat="server" width="300" CssClass="NormalTextBox"></asp:textbox>
                        <asp:RequiredFieldValidator ID="valReqStoreEmail" runat="server" ErrorMessage="* Store Email is required!" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True" resourcekey="valReqStoreEmail"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valRegExEmail" runat="server" ErrorMessage="* A valid email is required!" ControlToValidate="txtEmail" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valRegExEmail"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblCurrencySymbol" runat="server" controlname="txtCurrencySymbol"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:textbox id="txtCurrencySymbol" runat="server" width="50" MaxLength="3" CssClass="NormalTextBox"></asp:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblUsePortalTemplates" runat="server"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkUsePortalTemplates" runat="server" CssClass="NormalTextBox" AutoPostBack="True" OnCheckedChanged="chkUsePortalTemplates_CheckedChanged"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblStyleSheet" runat="server" controlname="lstStyleSheet"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstStyleSheet" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblStorePageID" runat="server" controlname="lstStorePageID"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstStorePageID" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqStorePageID" runat="server" ErrorMessage="* Select the catalog page!" ControlToValidate="lstStorePageID" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqStorePageID"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblShoppingCartPageID" runat="server" controlname="lstShoppingCartPageID"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstShoppingCartPageID" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqShoppingCartPageID" runat="server" ControlToValidate="lstShoppingCartPageID" Display="Dynamic" ErrorMessage="* Select the shopping cart page!" SetFocusOnError="True" InitialValue="-1" resourcekey="valReqShoppingCartPageID"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblAuthorizeCancel" runat="server" ControlName="chkAuthorizeCancel"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkAuthorizeCancel" runat="server" CssClass="NormalTextBox"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblInventoryManagement" runat="server" ControlName="chkInventoryManagement"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkInventoryManagement" runat="server" CssClass="NormalTextBox" OnCheckedChanged="chkInventoryManagement_CheckedChanged" AutoPostBack="True"></asp:checkbox>
                    </td>
                </tr>
                <tr id="trOutOfStock" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblOutOfStock" runat="server" controlname="lstOutOfStock"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstOutOfStock" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                    </td>
                </tr>
                <tr id="trProductsBehavior" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblProductsBehavior" runat="server" controlname="lstProductsBehavior"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstProductsBehavior" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                    </td>
                </tr>
                <tr id="trAvoidNegativeStock" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblAvoidNegativeStock" runat="server" ControlName="chkAvoidNegativeStock"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkAvoidNegativeStock" runat="server" CssClass="NormalTextBox"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblOrderRole" runat="server" controlname="lstOrderRole"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstOrderRole" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqOrderRole" runat="server" ErrorMessage="* Select a role!" ControlToValidate="lstOrderRole" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqOrderRole"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblCatalogRole" runat="server" controlname="lstCatalogRole"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstCatalogRole" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqCatalogRole" runat="server" ErrorMessage="* Select a role!" ControlToValidate="lstCatalogRole" Display="Dynamic" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqCatalogRole"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="rowSecureCookie" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblSecureCookie" runat="server" ControlName="chkSecureCookie"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkSecureCookie" runat="server" CssClass="NormalTextBox"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblCheckoutMode" runat="server" controlname="lstCheckoutMode"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstCheckoutMode" runat="server" CssClass="NormalTextBox" autopostback="True" OnSelectedIndexChanged="lstCheckoutMode_SelectedIndexChanged"></asp:dropdownlist>
                    </td>
                </tr>
                <tr id="trImpersonatedUser" runat="server" visible="false">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblImpersonatedUserID" runat="server" controlname="lstImpersonatedUserID"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <table id="tbImpersonatedUserSelection" runat="server" Visible="False">
                            <tr>
                                <td>
                                    <asp:dropdownlist id="lstImpersonatedRoleID" runat="server" CssClass="NormalTextBox" AutoPostBack="True" OnSelectedIndexChanged="lstImpersonatedRoleID_SelectedIndexChanged"></asp:dropdownlist>
                                    <asp:RequiredFieldValidator ID="valReqImpersonatedRoleID" runat="server" ControlToValidate="lstImpersonatedRoleID" Display="Dynamic" ErrorMessage="* Select a role!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqImpersonatedRoleID"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trImpersonatedUserID" runat="server">
                                <td>
                                    <asp:dropdownlist id="lstImpersonatedUserID" runat="server" CssClass="NormalTextBox" autopostback="False"></asp:dropdownlist>
                                    <asp:RequiredFieldValidator ID="valReqImpersonatedUserID" runat="server" ControlToValidate="lstImpersonatedUserID" Display="Dynamic" ErrorMessage="* Select an account!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqImpersonatedUserID"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trValidateUser" runat="server">
                                <td>
                                    <asp:LinkButton ID="btnValidateUser" runat="server" Text="Validate" 
                                        ResourceKey="btnValidateUser" CssClass="CommandButton" 
                                        onclick="btnValidateUser_Click"/>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblImpersonatedUser" runat="server" CssClass="Normal" Visible="False"></asp:Label>&nbsp;
                        <asp:LinkButton ID="btnChangeImpersonatedUser" runat="server" 
                            Text="Change User" ResourceKey="btnChangeImpersonatedUser" 
                            CssClass="CommandButton" onclick="btnChangeImpersonatedUser_Click"/>
                        <asp:HiddenField ID="hidImpersonatedUserID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblNoDelivery" runat="server" ControlName="chkNoDelivery"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkNoDelivery" runat="server" CssClass="NormalTextBox" AutoPostBack="True" oncheckedchanged="chkNoDelivery_CheckedChanged"></asp:checkbox>
                    </td>
                </tr>
                <tr id="trAllowVirtualProducts" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblAllowVirtualProducts" runat="server" ControlName="chkAllowVirtualProducts"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkAllowVirtualProducts" runat="server" CssClass="NormalTextBox"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblAllowCoupons" runat="server" ControlName="chkAllowCoupons"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkAllowCoupons" runat="server" CssClass="NormalTextBox"></asp:checkbox>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblAllowFreeShipping" runat="server" ControlName="chkAllowFreeShipping"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:checkbox id="chkAllowFreeShipping" runat="server" CssClass="NormalTextBox" AutoPostBack="True" oncheckedchanged="chkAllowFreeShipping_CheckedChanged"></asp:checkbox>
                    </td>
                </tr>
                <tr id="trFreeShipping" runat="server">
                    <td colspan="2">
			            <table>
                            <tr>
                                <td class="SubHead" style="width:200px;">
                                    <dnn:label id="lblMinOrderAmount" runat="server" controlname="txtMinOrderAmount"></dnn:label>
                                </td>
                                <td style="vertical-align:top;">
                                    <asp:textbox id="txtMinOrderAmount" runat="server" width="100" CssClass="NormalTextBox"></asp:textbox>
                                    <asp:RequiredFieldValidator ID="valReqMinOrderAmount" runat="server" ControlToValidate="txtMinOrderAmount" ErrorMessage="* Min order amount is required!" Display="Dynamic" SetFocusOnError="true" resourcekey="valReqMinOrderAmount"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator id="valCompMinOrderAmount" runat="server" ErrorMessage="Error! Please enter a valid amount." resourcekey="valCompMinOrderAmount" Type="Currency" ControlToValidate="txtMinOrderAmount" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True"></asp:CompareValidator>
                                </td>
                            </tr>
	                        <tr>
	                            <td class="SubHead" style="width:200px;">
			                        <dnn:label id="lblRestrictToCountries" runat="server" ControlName="chkRestrictToCountries"></dnn:label>
		                        </td>
		                        <td>
	                                <asp:CheckBox ID="chkRestrictToCountries" runat="server" CssClass="NormalTextBox" AutoPostBack="True" oncheckedchanged="chkRestrictToCountries_CheckedChanged" />
		                        </td>
	                        </tr>
	                        <tr id="trAuthorizedCountries" runat="server">
	                            <td class="SubHead" style="width:200px;">
			                        <dnn:label id="lblAuthorizedCountries" runat="server" ControlName="lbAuthorizedCountries"></dnn:label>
		                        </td>
		                        <td>
                                    <asp:ListBox ID="lbAuthorizedCountries" runat="server" DataTextField="Text" 
                                        DataValueField="Value" Rows="10" SelectionMode="Multiple" ></asp:ListBox>
                                    <asp:RequiredFieldValidator ID="valReqAuthorizedCountries" runat="server" 
                                        ErrorMessage="RequiredFieldValidator" ControlToValidate="lbAuthorizedCountries" 
                                        Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
		                        </td>
	                        </tr>
			            </table>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblOnPaidOrder" runat="server" ControlName="lstOnPaidOrderRole"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstOnPaidOrderRole" runat="server" CssClass="NormalTextBox"></asp:dropdownlist>
                    </td>
                </tr>
                <tr id="trProviders" runat="server">
                    <td colspan="2">
                        <asp:Label ID="lblProviders" runat="server" CssClass="NormalRed" resourcekey="lblProviders"></asp:Label>
                    </td>
                </tr>
                <tr>
		            <td class="SubHead" style="width:200px;">
			            <dnn:label id="lblAddressProvider" runat="server" controlname="lstAddressProviders"></dnn:label>
		            </td>
		            <td style="vertical-align:top;">
			            <asp:dropdownlist id="lstAddressProviders" runat="server" CssClass="NormalTextBox" autopostback="True" onselectedindexchanged="lstAddressProviders_SelectedIndexChanged"></asp:dropdownlist>
		            </td>
	            </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblTaxProvider" runat="server" controlname="lstTaxProviders"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstTaxProviders" runat="server" CssClass="NormalTextBox" autopostback="True" onselectedindexchanged="lstTaxProvider_SelectedIndexChanged"></asp:dropdownlist>
                    </td>
                </tr>
                <tr id="trShippingProviderSelection" runat="server">
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblShippingProvider" runat="server" controlname="lstShippingProviders"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstShippingProviders" runat="server" CssClass="NormalTextBox" autopostback="True" onselectedindexchanged="lstShippingProvider_SelectedIndexChanged"></asp:dropdownlist>
                    </td>
                </tr>
                <tr>
                    <td class="SubHead" style="width:200px;">
                        <dnn:label id="lblGateway" runat="server" controlname="lstGateway"></dnn:label>
                    </td>
                    <td style="vertical-align:top;">
                        <asp:dropdownlist id="lstGateway" runat="server" CssClass="NormalTextBox" autopostback="True" onselectedindexchanged="lstGateway_SelectedIndexChanged"></asp:dropdownlist>
                        <asp:RequiredFieldValidator ID="valReqGateway" runat="server" ControlToValidate="lstGateway" Display="Dynamic" ErrorMessage="* Select a gateway provider!" SetFocusOnError="True" resourcekey="valReqGateway"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="text-align:left;">
                    <td colspan="2">
                        <dnn:sectionhead id="dshGatewayProvider" ResourceKey="dshGatewayProvider" runat="server" cssclass="Head" text="Payment Gateway Settings" section="tblGatewayProvider" includerule="false"></dnn:sectionhead>
                        <table id="tblGatewayProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
                            <tr>
                                <td>
                                    <asp:placeholder id="plhGatewayProvider" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:linkbutton id="btnSave" runat="server" cssclass="CommandButton" resourcekey="btnSave" onclick="btnSave_Click">Update</asp:linkbutton>
        </td>
    </tr>
    <tr id="trAddressProvider" runat="server" style="text-align:left;">
        <td>
            <dnn:sectionhead id="dshAddressProvider" ResourceKey="dshAddressProvider" runat="server" cssclass="Head" text="Address Administration" section="tblAddressProvider" includerule="false"></dnn:sectionhead>
            <table id="tblAddressProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
                    <td>
                        <asp:placeholder id="plhAddressProvider" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trTaxProvider" runat="server" style="text-align:left;">
        <td>
            <dnn:sectionhead id="dshTaxProvider" ResourceKey="dshTaxProvider" runat="server" cssclass="Head" text="Tax Administration" section="tblTaxProvider" includerule="false"></dnn:sectionhead>
            <table id="tblTaxProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
                    <td>
                        <asp:placeholder id="plhTaxProvider" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trShippingProvider" runat="server" style="text-align:left;">
        <td>
            <dnn:sectionhead id="dshShippingProvider" ResourceKey="dshShippingProvider" runat="server" cssclass="Head" text="Shipping Administration" section="tblShippingProvider" includerule="false"></dnn:sectionhead>
            <table id="tblShippingProvider" runat="server" cellspacing="0" cellpadding="0" border="0" width="100%">
                <tr>
                    <td>
                        <asp:placeholder id="plhShippingProvider" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
