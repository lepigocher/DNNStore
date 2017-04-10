<%@ Control language="c#" CodeBehind="DefaultAddressAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.DefaultAddressAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table style="text-align:left;">
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblAllowPickup" runat="server"></dnn:label>
	    </td>
	    <td>
	        <asp:CheckBox ID="cbAllowPickup" runat="server" CssClass="NormalTextBox" />
	    </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblFirstName" runat="server"></dnn:label>
	    </td>
	    <td>
            <asp:Label ID="lblFirstNameRowOrder" runat="server" AssociatedControlID="ddlFirstNameRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlFirstNameRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblLastName" runat="server"></dnn:label>
	    </td>
	    <td>
            <asp:Label ID="lblLastNameRowOrder" runat="server" AssociatedControlID="ddlLastNameRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlLastNameRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblShowStreet" runat="server"></dnn:label>
	    </td>
	    <td>
	        <asp:CheckBox ID="cbShowStreet" runat="server" CssClass="NormalTextBox" />
            <asp:Label ID="lblStreetRowOrder" runat="server" AssociatedControlID="ddlStreetRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlStreetRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
        </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowUnit" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowUnit" runat="server" CssClass="NormalTextBox" />
            <asp:CheckBox ID="cbRequireUnit" runat="server" CssClass="NormalTextBox" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblUnitRowOrder" runat="server" AssociatedControlID="ddlUnitRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlUnitRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowPostal" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowPostal" runat="server" CssClass="NormalTextBox" />
            <asp:Label ID="lblPostalRowOrder" runat="server" AssociatedControlID="ddlPostalRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlPostalRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowCity" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowCity" runat="server" CssClass="NormalTextBox" />
            <asp:Label ID="lblCityRowOrder" runat="server" AssociatedControlID="ddlCityRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCityRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowCountry" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowCountry" runat="server" CssClass="NormalTextBox" AutoPostBack="True" oncheckedchanged="cbShowCountry_CheckedChanged" />
            <asp:Label ID="lblCountryRowOrder" runat="server" AssociatedControlID="ddlCountryRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCountryRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr id="trRestrictToCountries" runat="server">
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblRestrictToCountries" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbRestrictToCountry" runat="server" CssClass="NormalTextBox" AutoPostBack="True" oncheckedchanged="cbRestrictToCountry_CheckedChanged" />
		</td>
	</tr>
	<tr id="trAuthorizedCountries" runat="server">
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblAuthorizedCountries" runat="server"></dnn:label>
		</td>
		<td>
            <asp:ListBox ID="lbAuthorizedCountries" runat="server" DataTextField="Text" 
                DataValueField="Value" Rows="10" SelectionMode="Multiple" ></asp:ListBox>
            <asp:RequiredFieldValidator ID="valReqAuthorizedCountries" runat="server" 
                ErrorMessage="RequiredFieldValidator" ControlToValidate="lbAuthorizedCountries" 
                Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
		</td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblShowRegion" runat="server"></dnn:label>
	    </td>
	    <td>
	        <asp:CheckBox ID="cbShowRegion" runat="server" CssClass="NormalTextBox" />
            <asp:CheckBox ID="cbRequireRegion" runat="server" CssClass="NormalTextBox" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblRegionRowOrder" runat="server" AssociatedControlID="ddlRegionRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlRegionRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
	        <dnn:label id="lblEmail" runat="server"></dnn:label>
	    </td>
	    <td>
            <asp:Label ID="lblEmailRowOrder" runat="server" AssociatedControlID="ddlEmailRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlEmailRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
	    </td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowTelephone" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowTelephone" runat="server" CssClass="NormalTextBox" />
            <asp:CheckBox ID="cbRequireTelephone" runat="server" CssClass="NormalTextBox" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblTelephoneRowOrder" runat="server" AssociatedControlID="ddlTelephoneRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlTelephoneRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
	    <td class="SubHead" style="width:200px;">
			<dnn:label id="lblShowCell" runat="server"></dnn:label>
		</td>
		<td>
	        <asp:CheckBox ID="cbShowCell" runat="server" CssClass="NormalTextBox" />
            <asp:CheckBox ID="cbRequireCell" runat="server" CssClass="NormalTextBox" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblCellRowOrder" runat="server" AssociatedControlID="ddlCellRowOrder" Text="Display order:" CssClass="Normal" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCellRowOrder" runat="server" CssClass="NormalTextBox">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
            </asp:DropDownList>
		</td>
	</tr>
	<tr>
		<td colspan="2" class="Normal">
			<asp:linkbutton id="btnSaveSettings" runat="server" CssClass="CommandButton" resourcekey="btnSaveSettings" onclick="btnSaveSettings_Click">Update Settings</asp:linkbutton>
		</td>
	</tr>
</table>