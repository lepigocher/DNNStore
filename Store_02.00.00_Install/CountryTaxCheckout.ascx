<%@ Control language="c#" CodeBehind="CountryTaxCheckout.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Tax.CountryTaxProvider.CountryTaxCheckout" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table>
	<tr>
		<td>
		    <dnn:label id="lblTaxTotal" runat="server" controlname="lblOrderTaxTotal"></dnn:label>
		</td>
		<td class="StoreAccountCheckoutTaxTotal">
		    <asp:Label ID="lblOrderTaxTotal" runat="server"></asp:Label>
		</td>
	</tr>
</table>
