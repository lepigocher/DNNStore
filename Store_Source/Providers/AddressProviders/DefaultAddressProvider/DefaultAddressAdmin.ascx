<%@ Control language="c#" CodeBehind="DefaultAddressAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.DefaultAddressAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <h2 class="dnnFormSectionHead"><a href="#"><asp:Label ID="shAddressProvider" runat="server" ResourceKey="shAddressProvider">Address Provider</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
	        <dnn:label id="lblAllowPickup" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbAllowPickup" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblFirstName" runat="server"></dnn:label>
            <asp:Label ID="lblFirstNameRowOrder" runat="server" AssociatedControlID="ddlFirstNameRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlFirstNameRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblLastName" runat="server"></dnn:label>
            <asp:Label ID="lblLastNameRowOrder" runat="server" AssociatedControlID="ddlLastNameRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlLastNameRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowStreet" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowStreet" runat="server" />
            <asp:Label ID="lblStreetRowOrder" runat="server" AssociatedControlID="ddlStreetRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlStreetRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowUnit" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowUnit" runat="server" />
            <asp:CheckBox ID="cbRequireUnit" runat="server" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblUnitRowOrder" runat="server" AssociatedControlID="ddlUnitRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlUnitRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowPostal" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowPostal" runat="server" />
            <asp:Label ID="lblPostalRowOrder" runat="server" AssociatedControlID="ddlPostalRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlPostalRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowCity" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowCity" runat="server" />
            <asp:Label ID="lblCityRowOrder" runat="server" AssociatedControlID="ddlCityRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCityRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowCountry" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowCountry" runat="server" AutoPostBack="True" oncheckedchanged="cbShowCountry_CheckedChanged" />
            <asp:Label ID="lblCountryRowOrder" runat="server" AssociatedControlID="ddlCountryRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCountryRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem" id="trRestrictToCountries" runat="server">
	        <dnn:label id="lblRestrictToCountries" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbRestrictToCountry" runat="server" AutoPostBack="True" oncheckedchanged="cbRestrictToCountry_CheckedChanged" />
        </div>
        <div class="dnnFormItem" id="trAuthorizedCountries" runat="server">
	        <dnn:label id="lblAuthorizedCountries" runat="server"></dnn:label>
            <asp:ListBox ID="lbAuthorizedCountries" runat="server" DataTextField="Text" 
                DataValueField="Value" Rows="10" SelectionMode="Multiple" ></asp:ListBox>
            <asp:RequiredFieldValidator ID="valReqAuthorizedCountries" runat="server" 
                ErrorMessage="RequiredFieldValidator" ControlToValidate="lbAuthorizedCountries" 
                Display="Dynamic" EnableClientScript="False"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowRegion" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowRegion" runat="server" />
            <asp:CheckBox ID="cbRequireRegion" runat="server" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblRegionRowOrder" runat="server" AssociatedControlID="ddlRegionRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlRegionRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblEmail" runat="server"></dnn:label>
            <asp:Label ID="lblEmailRowOrder" runat="server" AssociatedControlID="ddlEmailRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlEmailRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowTelephone" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowTelephone" runat="server" />
            <asp:CheckBox ID="cbRequireTelephone" runat="server" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblTelephoneRowOrder" runat="server" AssociatedControlID="ddlTelephoneRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlTelephoneRowOrder" runat="server" Width="70">
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
        </div>
        <div class="dnnFormItem">
	        <dnn:label id="lblShowCell" runat="server"></dnn:label>
	        <asp:CheckBox ID="cbShowCell" runat="server" />
            <asp:CheckBox ID="cbRequireCell" runat="server" resourcekey="Required" Text="Required" />
            <asp:Label ID="lblCellRowOrder" runat="server" AssociatedControlID="ddlCellRowOrder" Text="Display order:" resourcekey="RowOrder"></asp:Label>
            <asp:DropDownList ID="ddlCellRowOrder" runat="server" Width="70">
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
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton id="btnSaveSettings" runat="server" CssClass="dnnPrimaryAction" resourcekey="btnSaveSettings" onclick="btnSaveSettings_Click">Update Settings</asp:linkbutton></li>
        </ul>
    </fieldset>
</div>
