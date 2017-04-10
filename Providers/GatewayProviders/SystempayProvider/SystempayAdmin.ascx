<%@ Control language="c#" CodeBehind="SystempayAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.SystempayAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table style="text-align:left;">
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblSiteID" runat="server" controlname="txtSiteID"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtSiteID" runat="server" Width="200px" MaxLength="50" cssclass="NormalTextBox"></asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblContracts" runat="server" controlname="txtContracts"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtContracts" runat="server" Width="200px" cssclass="NormalTextBox"></asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblUseTestCertificate" runat="server" controlname="chkUseTestCertificate"></dnn:label>
		</td>
		<td>
			<asp:checkbox id="chkUseTestCertificate" runat="server" cssclass="NormalTextBox" Checked="false"></asp:checkbox>
	    </td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblCertificate" runat="server" controlname="txtCertificate"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtCertificate" runat="server" Width="200px" MaxLength="50" cssclass="NormalTextBox"></asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblSystempayPaymentURL" runat="server" controlname="txtSystempayPaymentURL"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtSystempayPaymentURL" runat="server" Width="300px" MaxLength="255" cssclass="NormalTextBox"></asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblSystempayLanguage" runat="server" controlname="txtSystempayLanguage"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtSystempayLanguage" runat="server" Width="30px" MaxLength="2" cssclass="NormalTextBox">fr</asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblCurrency" runat="server" controlname="txtSystempayCurrency"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtSystempayCurrency" runat="server" Width="50px" MaxLength="3" cssclass="NormalTextBox">978</asp:textbox>
		</td>
	</tr>
	<tr>
		<td class="SubHead" style="width:200px">
			<dnn:label id="lblButtonURL" runat="server" controlname="txtButtonURL"></dnn:label>
		</td>
		<td>
			<asp:textbox id="txtButtonURL" runat="server" Width="50px" cssclass="NormalTextBox"></asp:textbox>
		</td>
	</tr>
</table>
