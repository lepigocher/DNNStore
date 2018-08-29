<%@ Control Language="c#"  AutoEventWireup="True" Codebehind="ProductEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ProductEdit" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>
<asp:Panel ID="pnlCategoriesRequired" runat="server">
    <div class="dnnFormMessage dnnFormWarning"><asp:label ID="lblCategoriesRequired" runat="server" ResourceKey="CategoriesRequired"></asp:label></div>
</asp:Panel>
<div class="dnnForm" id="tblProductForm" runat="server">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblCategory" runat="server" controlname="cmbCategory"></dnn:label>
            <asp:DropDownList id="cmbCategory" Runat="server" DataTextField="CategoryPathName" DataValueField="CategoryID"></asp:DropDownList>
            <asp:RequiredFieldValidator id="valRequireCategory" runat="server" ControlToValidate="cmbCategory" resourcekey="valRequireCategory" ErrorMessage="* Category is required." InitialValue="-1" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblManufacturer" runat="server" controlname="txtManufacturer"></dnn:label>
            <asp:TextBox id="txtManufacturer" Runat="server" MaxLength="50"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblModelNumber" runat="server" controlname="txtModelNumber"></dnn:label>
            <asp:TextBox id="txtModelNumber" Runat="server" MaxLength="50"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblModelName" runat="server" controlname="txtModelName"></dnn:label>
            <asp:TextBox id="txtModelName" Runat="server" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireModelName" runat="server" ControlToValidate="txtModelName" ErrorMessage="* Model name is required!" resourcekey="valRequireModelName" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        </div>
        <div class="dnnFormItem" id="trSEOName" runat="server">
            <dnn:label id="lblSEOName" runat="server" controlname="txtSEOName"></dnn:label>
            <asp:TextBox id="txtSEOName" Runat="server" MaxLength="50"></asp:TextBox>
            <asp:linkbutton id="cmdSuggest" CssClass="dnnSecondaryAction" runat="server" CausesValidation="false" resourcekey="cmdSuggest">Suggest</asp:linkbutton>
            <asp:RegularExpressionValidator ID="valRegExSEOName" runat="server" ErrorMessage="Invalid character(s)!" ControlToValidate="txtSEOName" SetFocusOnError="True" ValidationExpression="[_a-zA-Z0-9-]*" Display="Dynamic" CssClass="dnnFormMessage dnnFormError"></asp:RegularExpressionValidator>
        </div>
        <div class="dnnFormItem" id="trKeywords" runat="server">
            <dnn:label id="lblKeywords" runat="server" controlname="txtKeywords"></dnn:label>
            <asp:TextBox id="txtKeywords" Runat="server" Height="50" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblSummary" runat="server" controlname="txtSummary"></dnn:label>
            <asp:TextBox id="txtSummary" Runat="server" Height="50" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblRegularPrice" runat="server" controlname="txtRegularPrice"></dnn:label>
            <asp:TextBox id="txtRegularPrice" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
            <asp:RequiredFieldValidator id="valRequireRegularPrice" runat="server" ControlToValidate="txtRegularPrice" ErrorMessage="* Regular price is required." resourcekey="valRequireRegularPrice" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:CompareValidator id="valRegularPrice" runat="server" ErrorMessage="Error! Please enter a valid price." resourcekey="valRegularPrice" Type="Currency" ControlToValidate="txtRegularPrice" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblUnitPrice" runat="server" controlname="txtUnitPrice"></dnn:label>
            <asp:TextBox id="txtUnitPrice" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
            <asp:RequiredFieldValidator id="valRequireUnitPrice" runat="server" ControlToValidate="txtUnitPrice" ErrorMessage="* Price is required." resourcekey="valRequireUnitPrice" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:CompareValidator id="valUnitPrice" runat="server" ErrorMessage="Error! Please enter a valid price." resourcekey="valUnitPrice" Type="Currency" ControlToValidate="txtUnitPrice" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem" id="trVirtualProduct" runat="server">
            <dnn:label id="lblVirtualProduct" runat="server" controlname="chkVirtualProduct"></dnn:label>
            <asp:CheckBox id="chkVirtualProduct" Runat="server" AutoPostBack="True" OnCheckedChanged="chkVirtualProduct_CheckedChanged"></asp:CheckBox>
        </div>
    </fieldset>
    <asp:PlaceHolder id="trVirtualProductSection" runat="server">
        <h2 id="fshDownloadInfos" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shDownloadInfos" runat="server" ResourceKey="DownloadInfos">Download Informations</asp:Label></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblProductFile" runat="server" controlname="urlProductFile"></dnn:label>
                <dnn:URL id="urlProductFile" runat="server" EnableViewState="true" width="300" ShowDatabase="false" ShowFiles="true" ShowLog="false" ShowNewWindow="false" ShowNone="true" ShowSecure="false" ShowTabs="false" ShowTrack="false" ShowUpLoad="true" ShowUrls="false" ShowUsers="false" UrlType="N" />
            </div>
            <div class="dnnFormItem" id="trErrorProductFile" runat="server" visible="false">
                <asp:Label ID="lblErrorProductFile" runat="server" resourcekey="lblErrorProductFile"></asp:Label>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblAllowedDownloads" runat="server" ControlName="txtAllowedDownloads"></dnn:label>
                <asp:TextBox id="txtAllowedDownloads" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireAllowedDownloads" runat="server" ControlToValidate="txtAllowedDownloads" ErrorMessage="* Allowed Downloads is required." resourcekey="valRequireAllowedDownloads" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valAllowedDownloads" runat="server" ErrorMessage="Error! Please enter a valid number." Type="Integer" ControlToValidate="txtAllowedDownloads" Operator="DataTypeCheck" resourcekey="valAllowedDownloads" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
        </fieldset>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="trProductDimensions" runat="server">
        <h2 id="fshProductDimensions" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shProductDimensions" runat="server" ResourceKey="shProductDimensions">Product Dimensions</asp:Label></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblUnitWeight" runat="server" ControlName="txtUnitWeight"></dnn:label>
                <asp:TextBox id="txtUnitWeight" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireUnitWeight" runat="server" ControlToValidate="txtUnitWeight" ErrorMessage="* Weight is required." resourcekey="valRequireUnitWeight" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valUnitWeight" runat="server" ErrorMessage="Error! Please enter a valid weight." Type="Double" ControlToValidate="txtUnitWeight" Operator="DataTypeCheck" resourcekey="valUnitWeight" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblUnitHeight" runat="server" ControlName="txtUnitHeight"></dnn:label>
                <asp:TextBox id="txtUnitHeight" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireUnitHeight" runat="server" ControlToValidate="txtUnitHeight" ErrorMessage="* Height is required." resourcekey="valRequireUnitHeight" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valUnitHeight" runat="server" ErrorMessage="Error! Please enter a valid height." Type="Double" ControlToValidate="txtUnitHeight" Operator="DataTypeCheck" resourcekey="valUnitHeight" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblUnitLength" runat="server" ControlName="txtUnitLength"></dnn:label>
                <asp:TextBox id="txtUnitLength" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireUnitLength" runat="server" ControlToValidate="txtUnitLength" ErrorMessage="* Length is required." resourcekey="valRequireUnitLength" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valUnitLength" runat="server" ErrorMessage="Error! Please enter a valid length." Type="Double" ControlToValidate="txtUnitLength" Operator="DataTypeCheck" resourcekey="valUnitLength" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblUnitWidth" runat="server" ControlName="txtUnitWidth"></dnn:label>
                <asp:TextBox id="txtUnitWidth" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireUnitWidth" runat="server" ControlToValidate="txtUnitWidth" ErrorMessage="* Width is required." resourcekey="valRequireUnitWidth" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valUnitWidth" runat="server" ErrorMessage="Error! Please enter a valid width." Type="Double" ControlToValidate="txtUnitWidth" Operator="DataTypeCheck" resourcekey="valUnitWidth" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
        </fieldset>
    </asp:PlaceHolder>
    <asp:PlaceHolder id="trStockManagement" runat="server">
        <h2 id="fshStockManagement" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shStockManagement" runat="server" ResourceKey="shStockManagement">Stock Management</asp:Label></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblStockQuantity" runat="server" ControlName="txtStockQuantity"></dnn:label>
                <asp:TextBox id="txtStockQuantity" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireStockQuantity" runat="server" ControlToValidate="txtStockQuantity" ErrorMessage="* Quantity is required." resourcekey="valRequireStockQuantity" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valStockQuantity" runat="server" ErrorMessage="Error! Please enter a valid quantity." Type="Integer" ControlToValidate="txtStockQuantity" Operator="DataTypeCheck" resourcekey="valStockQuantity" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblLowThreshold" runat="server" controlname="txtLowThreshold"></dnn:label>
                <asp:TextBox id="txtLowThreshold" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireLowThreshold" runat="server" ControlToValidate="txtLowThreshold" ErrorMessage="* Low threshold quantity is required." resourcekey="valRequireLowThreshold" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            	<asp:CompareValidator id="valLowThreshold" runat="server" ErrorMessage="Error! Please enter a valid quantity." Type="Integer" ControlToValidate="txtLowThreshold" Operator="DataTypeCheck" resourcekey="valLowThreshold" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblHighThreshold" runat="server" controlname="txtHighThreshold"></dnn:label>
                <asp:TextBox id="txtHighThreshold" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireHighThreshold" runat="server" ControlToValidate="txtHighThreshold" ErrorMessage="* High threshold quantity is required." resourcekey="valRequireHighThreshold" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            	<asp:CompareValidator id="valHighThreshold" runat="server" ErrorMessage="Error! Please enter a valid quantity." Type="Integer" ControlToValidate="txtHighThreshold" Operator="DataTypeCheck" resourcekey="valHighThreshold" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblDeliveryTime" runat="server" controlname="txtDeliveryTime"></dnn:label>
                <asp:TextBox id="txtDeliveryTime" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequireDeliveryTime" runat="server" ControlToValidate="txtDeliveryTime" ErrorMessage="* Delivery Time is required." resourcekey="valRequireDeliveryTime" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            	<asp:CompareValidator id="valDeliveryTime" runat="server" ErrorMessage="Error! Please enter a valid number of days." Type="Integer" ControlToValidate="txtDeliveryTime" Operator="DataTypeCheck" resourcekey="valDeliveryTime" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblPurchasePrice" runat="server" controlname="txtPurchasePrice"></dnn:label>
                <asp:TextBox id="txtPurchasePrice" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator id="valRequirePurchasePrice" runat="server" ControlToValidate="txtPurchasePrice" ErrorMessage="* Purchase price is required." resourcekey="valRequirePurchasePrice" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valPurchasePrice" runat="server" ErrorMessage="Error! Please enter a valid price." resourcekey="valPurchasePrice" Type="Currency" ControlToValidate="txtPurchasePrice" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
        </fieldset>
    </asp:PlaceHolder>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblArchived" runat="server" controlname="chkArchived"></dnn:label>
            <asp:CheckBox id="chkArchived" Runat="server"></asp:CheckBox>
        </div>
        <div class="dnnFormItem" id="trProductRole" runat="server">
            <dnn:label id="lblRole" runat="server" controlname="lstRole"></dnn:label>
            <asp:dropdownlist id="lstRole" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblFeatured" runat="server" controlname="chkFeatured"></dnn:label>
            <asp:CheckBox id="chkFeatured" Runat="server" OnCheckedChanged="chkFeatured_CheckedChanged" AutoPostBack="True"></asp:CheckBox>
        </div>
    </fieldset>
    <asp:PlaceHolder ID="trFeatured" runat="server">
        <h2 id="fshSpecialOffer" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shSpecialOffer" runat="server" ResourceKey="shSpecialOffer">Special Offer Pricing</asp:Label></a></h2>
        <fieldset>
            <div class="dnnFormItem">
                <dnn:label id="lblSalePrice" runat="server" controlname="txtSalePrice"></dnn:label>
                <asp:TextBox id="txtSalePrice" Runat="server" Width="100" MaxLength="25"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireSalePrice" runat="server" ControlToValidate="txtSalePrice" ErrorMessage="* Sale price is required." resourcekey="valRequireSalePrice" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:CompareValidator id="valSalePrice" runat="server" ErrorMessage="Error! Please enter a valid price." resourcekey="valSalePrice" Type="Currency" ControlToValidate="txtSalePrice" Operator="DataTypeCheck" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblSaleStartDate" runat="server" controlname="txtSaleStartDate"></dnn:label>
                <asp:TextBox ID="txtSaleStartDate" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireSaleStartDate" runat="server" ControlToValidate="txtSaleStartDate" ErrorMessage="* Sale start date is required." resourcekey="valRequireSaleStartDate" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:comparevalidator id="valSaleStartDate" runat="server" resourcekey="valSaleStartDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtSaleStartDate" CssClass="dnnFormMessage dnnFormError"></asp:comparevalidator>
                <asp:HyperLink ID="cmdSaleStartDate" runat="server" CssClass="dnnSecondaryAction"></asp:HyperLink>
            </div>
            <div class="dnnFormItem">
                <dnn:label id="lblSaleEndDate" runat="server" controlname="txtSaleEndDate"></dnn:label>
                <asp:TextBox ID="txtSaleEndDate" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireSaleEndDate" runat="server" ControlToValidate="txtSaleEndDate" ErrorMessage="* Sale end date is required." resourcekey="valRequireSaleEndDate" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
                <asp:comparevalidator id="valSaleEndDate" runat="server" resourcekey="valSaleEndDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtSaleEndDate" CssClass="dnnFormMessage dnnFormError"></asp:comparevalidator>
                <asp:comparevalidator id="valSaleDates" runat="server" resourcekey="valSaleDates" display="Dynamic" type="Date" operator="GreaterThan" errormessage="* Sale end date must be greater than sale start date!" controltovalidate="txtSaleEndDate" controltocompare="txtSaleStartDate" CssClass="dnnFormMessage dnnFormError"></asp:comparevalidator>
                <asp:HyperLink ID="cmdSaleEndDate" runat="server" CssClass="dnnSecondaryAction"></asp:HyperLink>
            </div>
        </fieldset>
    </asp:PlaceHolder>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblImage" runat="server" controlname="imgProduct"></dnn:label>
            <dnn:URL id="imgProduct" runat="server" EnableViewState="true" width="300" ShowDatabase="false" ShowFiles="true" ShowLog="false" ShowNewWindow="false" ShowNone="true" ShowSecure="false" ShowTabs="false" ShowTrack="false" ShowUpLoad="true" ShowUrls="true" ShowUsers="false" UrlType="N" />
            <div class="dnnFormMessage dnnFormWarning" id="trWarningTrustLevel" runat="server" visible="false"><asp:Label ID="lblWarningTrustLevel" runat="server" Text="Warning Trust Level"></asp:Label></div>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblDescription" runat="server" controlname="txtDescription"></dnn:label>
            <dnn:TextEditor id="txtDescription" runat="server" width="100%" height="500"></dnn:TextEditor>
        </div>
    </fieldset>
    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton id="cmdUpdate" CssClass="dnnPrimaryAction" runat="server" resourcekey="cmdUpdate">Update</asp:linkbutton></li>
        <li><asp:linkbutton id="cmdCancel" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" resourcekey="cmdCancel">Cancel</asp:linkbutton></li>
        <li><asp:linkbutton id="cmdDelete" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" Visible="False" resourcekey="cmdDelete">Delete</asp:linkbutton></li>
    </ul>
</div>
<script type="text/javascript">
    jQuery(function ($) {
        var setupProductEdit = function () {
            $('.dnnForm').dnnPanels();
        };

        setupProductEdit();

        Sys.WebForms.PageRequestManager.setupProductEdit().add_endRequest(function () {
            // note that this will fire when _any_ UpdatePanel is triggered,
            // which may or may not cause an issue
            setupProductEdit();
        });
    });
</script>
