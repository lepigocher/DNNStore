<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouponEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CouponEdit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label id="lblCode" runat="server" controlname="txtCode"></dnn:label>
        <asp:TextBox id="txtCode" Runat="server" Width="140" MaxLength="10"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqCode" runat="server" ControlToValidate="txtCode" Display="Dynamic" ErrorMessage="* Coupon code is required." resourcekey="valReqCode" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblDescription" runat="server" controlname="txtDescription"></dnn:label>
        <asp:TextBox id="txtDescription" Runat="server" MaxLength="50"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" ErrorMessage="* Description is required." resourcekey="valReqDescription" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblRuleType" runat="server" controlname="rblRuleType"></dnn:label>
        <asp:RadioButtonList ID="rblRuleType" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblRuleType_SelectedIndexChanged">
            <asp:ListItem resourcekey="RuleOrderAnything" Value="0" Selected="True">Order anything</asp:ListItem>
            <asp:ListItem resourcekey="RuleSpendsAtLeast" Value="1">Spends at least</asp:ListItem>
            <asp:ListItem resourcekey="RuleOrdersAtLeast" Value="2">Orders at least</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="dnnFormItem" id="trRuleAmount" runat="server">
        <dnn:label id="lblRuleAmount" runat="server" controlname="txtRuleAmount"></dnn:label>
        <asp:TextBox id="txtRuleAmount" Runat="server" MaxLength="20"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqRuleAmount" runat="server" ControlToValidate="txtRuleAmount" Display="Dynamic" ErrorMessage="* Minimum amount is required." resourcekey="valReqRuleAmount" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="valCompRuleAmount" runat="server" 
            ErrorMessage="* Must be greater than 0!" ControlToValidate="txtRuleAmount" 
            Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Currency" 
            ValueToCompare="0" resourcekey="valCompRuleAmount" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblDiscountType" runat="server" controlname="rblDiscountType"></dnn:label>
        <asp:RadioButtonList ID="rblDiscountType" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblDiscountType_SelectedIndexChanged">
            <asp:ListItem resourcekey="DiscountPercentage" Value="0" Selected="True">Percentage</asp:ListItem>
            <asp:ListItem resourcekey="DiscountFixedAmount" Value="1">Fixed amount</asp:ListItem>
            <asp:ListItem resourcekey="DiscountFreeShipping" Value="2">Free shipping</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="dnnFormItem" id="trDiscountPercentage" runat="server">
        <dnn:label id="lblDiscountPercentage" runat="server" controlname="txtDiscountPercentage"></dnn:label>
        <asp:TextBox id="txtDiscountPercentage" Runat="server" Width="70" MaxLength="3"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqDiscountPercentage" runat="server" ControlToValidate="txtDiscountPercentage" Display="Dynamic" ErrorMessage="* Discount percentage is required." resourcekey="valReqDiscountPercentage" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="valCompDiscountPercentage" runat="server" 
            ErrorMessage="* Must be greater than 0!" ControlToValidate="txtDiscountPercentage" 
            Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Integer" 
            ValueToCompare="0" resourcekey="valCompDiscountPercentage" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
    </div>
    <div class="dnnFormItem" id="trDiscountAmount" runat="server">
        <dnn:label id="lblDiscountAmount" runat="server" controlname="txtDiscountAmount"></dnn:label>
        <asp:TextBox id="txtDiscountAmount" Runat="server" Width="100" MaxLength="10"></asp:TextBox>
        <asp:RequiredFieldValidator id="valReqDiscountAmount" runat="server" ControlToValidate="txtDiscountAmount" Display="Dynamic" ErrorMessage="* Discount amount is required." resourcekey="valReqDiscountAmount" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="valCompDiscountAmount" runat="server" 
            ErrorMessage="* Must be greater than 0!" ControlToValidate="txtDiscountAmount" 
            Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Currency" 
            ValueToCompare="0" resourcekey="valCompDiscountAmount" CssClass="dnnFormMessage dnnFormError"></asp:CompareValidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblApplyTo" runat="server" controlname="rblApplyTo"></dnn:label>
        <asp:RadioButtonList ID="rblApplyTo" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblApplyTo_SelectedIndexChanged">
            <asp:ListItem resourcekey="ApplyToOrder" Value="0" Selected="True">Entire order</asp:ListItem>
            <asp:ListItem resourcekey="ApplyToCategory" Value="1">All items in category</asp:ListItem>
            <asp:ListItem resourcekey="ApplyToProduct" Value="2">Specific product</asp:ListItem>
        </asp:RadioButtonList>
        <asp:CustomValidator ID="valCustValidateItem" runat="server" 
            ControlToValidate="rblApplyTo" Display="Dynamic" EnableClientScript="False" 
            ErrorMessage="* Please validate your choice!" 
            onservervalidate="valCustValidateItem_ServerValidate" SetFocusOnError="True" CssClass="dnnFormMessage dnnFormError"></asp:CustomValidator>
    </div>
    <div class="dnnFormItem" id="trCategory" runat="server" visible="false">
        <dnn:label id="lblCategory" runat="server" controlname="lstCategory"></dnn:label>
        <asp:DropDownList ID="lstCategory" runat="server" OnSelectedIndexChanged="lstCategory_SelectedIndexChanged" ></asp:DropDownList>
        <asp:RequiredFieldValidator ID="valReqCategory" runat="server" ControlToValidate="lstCategory" Display="Dynamic" ErrorMessage="* Select a category!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqCategory" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:LinkButton ID="btnValidateCategory" runat="server" Text="Validate" ResourceKey="btnValidateCategory" CssClass="dnnSecondaryAction" onclick="btnValidateCategory_Click" CausesValidation="false"/>
    </div>
    <div class="dnnFormItem" id="trProduct" runat="server" visible="false">
        <dnn:label id="lblProduct" runat="server" controlname="lstProduct"></dnn:label>
        <asp:DropDownList ID="lstProduct" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator ID="valReqProduct" runat="server" ControlToValidate="lstProduct" Display="Dynamic" ErrorMessage="* Select a product!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqProduct" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:LinkButton ID="btnValidateProduct" runat="server" Text="Validate" ResourceKey="btnValidateProduct" CssClass="dnnSecondaryAction" onclick="btnValidateProduct_Click" CausesValidation="false"/>
    </div>
    <div class="dnnFormItem" id="trSelectedItem" runat="server" visible="false">
        <dnn:label id="lblSelectedItem" runat="server" controlname="lblItemName"></dnn:label>
        <asp:HiddenField ID="hfItemID" runat="server" />
        <asp:Label ID="lblItemName" runat="server"></asp:Label>
        <asp:LinkButton ID="btnChangeItem" runat="server" Text="Change Item" ResourceKey="btnChangeItem" CssClass="dnnSecondaryAction" onclick="btnChangeItem_Click" CausesValidation="false"/>
    </div>
    <div class="dnnFormItem" id="trIncludeSubCategories" runat="server" visible="false">
        <dnn:label id="lblIncludeSubCategories" runat="server" controlname="chkIncludeSubCategories"></dnn:label>
        <asp:CheckBox ID="chkIncludeSubCategories" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblStartDate" runat="server" controlname="txtStartDate"></dnn:label>
        <asp:TextBox ID="txtStartDate" runat="server" Width="80"></asp:TextBox>
        <asp:HyperLink ID="cmdStartDate" runat="server" CssClass="dnnSecondaryAction"></asp:HyperLink>
        <asp:RequiredFieldValidator ID="valReqStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="* Start date is required." resourcekey="valReqStartDate" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:comparevalidator id="valCompStartDate" runat="server" resourcekey="valCompStartDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtStartDate" CssClass="dnnFormMessage dnnFormError"></asp:comparevalidator>
    </div>
    <div class="dnnFormItem">
        <dnn:label id="lblValidity" runat="server" controlname="rblValidity"></dnn:label>
        <asp:RadioButtonList ID="rblValidity" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblValidity_SelectedIndexChanged">
            <asp:ListItem resourcekey="ValidityPermanent" Value="0" Selected="True">Permanent</asp:ListItem>
            <asp:ListItem resourcekey="ValiditySingleUse" Value="1">Single use</asp:ListItem>
            <asp:ListItem resourcekey="ValidityUntil" Value="2">Until</asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <div class="dnnFormItem" id="trEndDate" runat="server" visible="false">
        <dnn:label id="lblEndDate" runat="server" controlname="txtEndDate"></dnn:label>
        <asp:TextBox ID="txtEndDate" runat="server" Width="80"></asp:TextBox>
        <asp:HyperLink ID="cmdEndDate" runat="server" CssClass="dnnSecondaryAction"></asp:HyperLink>
        <asp:RequiredFieldValidator ID="valReqEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="* End date is required." resourcekey="valReqEndDate" Display="Dynamic" SetFocusOnError="true" CssClass="dnnFormMessage dnnFormError"></asp:RequiredFieldValidator>
        <asp:comparevalidator id="valCompEndDate" runat="server" resourcekey="valCompEndDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtEndDate" CssClass="dnnFormMessage dnnFormError"></asp:comparevalidator>
    </div>
</fieldset>
<ul class="dnnActions dnnClear">
    <li><asp:linkbutton id="cmdUpdate" CssClass="dnnPrimaryAction" runat="server" resourcekey="cmdUpdate" onclick="cmdUpdate_Click">Update</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdCancel" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" resourcekey="cmdCancel" onclick="cmdCancel_Click">Cancel</asp:linkbutton></li>
    <li><asp:linkbutton id="cmdDelete" CssClass="dnnSecondaryAction" runat="server" CausesValidation="False" Visible="False" resourcekey="cmdDelete" onclick="cmdDelete_Click">Delete</asp:linkbutton></li>
</ul>
