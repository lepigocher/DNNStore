<%@ Control language="c#" CodeBehind="AccountSettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.AccountSettings" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="lblDefaultView" runat="server" controlname="lstDefaultView"></dnn:Label>
            <asp:DropDownList id="lstDefaultView" runat="server" AutoPostBack="false"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
			<dnn:label id="lblRequireSSL" controlname="chkRequireSSL" runat="server"></dnn:label>
			<asp:CheckBox id="chkRequireSSL" runat="server"></asp:CheckBox>
        </div>
        <div class="dnnFormItem">
			<dnn:label id="lblSSLNote" controlname="lblSSLMessage" runat="server"></dnn:label>
			<div class="dnnFormMessage dnnFormWarning"><asp:Label id="lblSSLMessage" runat="server" resourcekey="SSLMessage"></asp:Label></div>
        </div>
    </fieldset>
    <h2 id="fshMainCartSettings" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shMainCartSettings" runat="server" ResourceKey="shMainCartSettings">Main Cart Settings</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="lblShowThumbnail" runat="server" controlname="chkShowThumbnail"></dnn:Label>
            <asp:CheckBox id="chkShowThumbnail" runat="server" CssClass="NormalTextBox" AutoPostBack="True" OnCheckedChanged="chkShowThumbnail_CheckedChanged" />
        </div>
        <div class="dnnFormItem" id="trThumbnailWidth" runat="server">
            <dnn:Label id="lblThumbnailWidth" runat="server" controlname="txtThumbnailWidth"></dnn:Label>
            <asp:TextBox id="txtThumbnailWidth" runat="server" MaxLength="3" Width="30"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valReqThumbnailWidth" runat="server" ErrorMessage="* Thumbnail Width is required!" resourcekey="valReqThumbnailWidth" ControlToValidate="txtThumbnailWidth" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="valCompThumbnailWidth" runat="server" ErrorMessage="* A numeric value is required!" resourcekey="valCompThumbnailWidth" ControlToValidate="txtThumbnailWidth" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem" id="trGIFBgColor" runat="server">
            <dnn:Label ID="lblGIFBgColor" runat="server" ResourceKey="lblGIFBgColor" ControlName="txtGIFBgColor" Text="Thumbnail Width:" />
            <asp:textbox id="txtGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
            <asp:RequiredFieldValidator ID="valReqGIFBgColor" runat="server" ErrorMessage="* Background color for GIF images is required!" ControlToValidate="txtGIFBgColor" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valRegExGIFBgColor" runat="server" ErrorMessage="* A valid HTML Color is required!" ControlToValidate="txtGIFBgColor" Display="Dynamic" SetFocusOnError="True" ValidationExpression="^#?([a-f]|[A-F]|[0-9]){3}(([a-f]|[A-F]|[0-9]){3})?$" CssClass="dnnFormMessage dnnFormError"></asp:RegularExpressionValidator>
        </div>
        <div class="dnnFormItem" id="trEnableImageCaching" runat="server">
            <dnn:Label ID="lblEnableImageCaching" runat="server" ResourceKey="lblEnableImageCaching" ControlName="chkEnableImageCaching" Text="Enable Image Caching:" />
            <asp:checkbox id="chkEnableImageCaching" runat="server" AutoPostBack="True" OnCheckedChanged="chkEnableImageCaching_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="trCacheDuration" runat="server">
            <dnn:Label ID="lblCacheDuration" runat="server" ResourceKey="lblCacheDuration" ControlName="txtCacheDuration" Text="Cache Duration:" />
            <asp:textbox id="txtCacheDuration" runat="server" width="80"></asp:textbox>
            <asp:RequiredFieldValidator ID="valReqCacheDuration" runat="server" ErrorMessage="* Cache Duration is required!" ControlToValidate="txtCacheDuration" Display="Dynamic" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="valCompCacheDuration" runat="server" ErrorMessage="* A numeric value is required!" ControlToValidate="txtCacheDuration" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblProductColumn" runat="server" controlname="lstProductColumn"></dnn:Label>
            <asp:DropDownList id="lstProductColumn" runat="server" AutoPostBack="false"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblLinkToDetail" runat="server" controlname="chkLinkToDetail"></dnn:Label>
            <asp:CheckBox id="chkLinkToDetail" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label id="lblIncludeVAT" runat="server" controlname="chkIncludeVAT"></dnn:Label>
            <asp:CheckBox id="chkIncludeVAT" runat="server" />
        </div>
    </fieldset>
</div>
<script type="text/javascript">
    jQuery(function ($) {
        var setupAccountSettings = function () {
            $('.dnnForm').dnnPanels();
        };

        setupAccountSettings();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            // note that this will fire when _any_ UpdatePanel is triggered,
            // which may or may not cause an issue
            setupAccountSettings();
        });
    });
</script>
