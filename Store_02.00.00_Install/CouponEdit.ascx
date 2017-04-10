<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CouponEdit.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CouponEdit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table width="100%" border="0" cellspacing="5">
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblCode" runat="server" controlname="txtCode"></dnn:label>
        </td>
        <td class="Normal">
            <asp:TextBox id="txtCode" Runat="server" Width="140" MaxLength="10" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="valReqCode" runat="server" ControlToValidate="txtCode" Display="Dynamic" ErrorMessage="* Coupon code is required." resourcekey="valReqCode" SetFocusOnError="true"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblDescription" runat="server" controlname="txtDescription"></dnn:label>
        </td>
        <td>
            <asp:TextBox id="txtDescription" Runat="server" Width="400" MaxLength="50" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="valReqDescription" runat="server" ControlToValidate="txtDescription" Display="Dynamic" ErrorMessage="* Description is required." resourcekey="valReqDescription" SetFocusOnError="true"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblRuleType" runat="server" controlname="rblRuleType"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="rblRuleType" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblRuleType_SelectedIndexChanged">
                <asp:ListItem resourcekey="RuleOrderAnything" Value="0" Selected="True">Order anything</asp:ListItem>
                <asp:ListItem resourcekey="RuleSpendsAtLeast" Value="1">Spends at least</asp:ListItem>
                <asp:ListItem resourcekey="RuleOrdersAtLeast" Value="2">Orders at least</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trRuleAmount" runat="server" valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblRuleAmount" runat="server" controlname="txtRuleAmount"></dnn:label>
        </td>
        <td class="Normal">
            <asp:TextBox id="txtRuleAmount" Runat="server" Width="100" MaxLength="20" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="valReqRuleAmount" runat="server" ControlToValidate="txtRuleAmount" Display="Dynamic" ErrorMessage="* Minimum amount is required." resourcekey="valReqRuleAmount" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="valCompRuleAmount" runat="server" 
                ErrorMessage="* Must be greater than 0!" ControlToValidate="txtRuleAmount" 
                Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Currency" 
                ValueToCompare="0" resourcekey="valCompRuleAmount"></asp:CompareValidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblDiscountType" runat="server" controlname="rblDiscountType"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="rblDiscountType" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblDiscountType_SelectedIndexChanged">
                <asp:ListItem resourcekey="DiscountPercentage" Value="0" Selected="True">Percentage</asp:ListItem>
                <asp:ListItem resourcekey="DiscountFixedAmount" Value="1">Fixed amount</asp:ListItem>
                <asp:ListItem resourcekey="DiscountFreeShipping" Value="2">Free shipping</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trDiscountPercentage" runat="server" valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblDiscountPercentage" runat="server" controlname="txtDiscountPercentage"></dnn:label>
        </td>
        <td class="Normal">
            <asp:TextBox id="txtDiscountPercentage" Runat="server" Width="70" MaxLength="3" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="valReqDiscountPercentage" runat="server" ControlToValidate="txtDiscountPercentage" Display="Dynamic" ErrorMessage="* Discount percentage is required." resourcekey="valReqDiscountPercentage" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="valCompDiscountPercentage" runat="server" 
                ErrorMessage="* Must be greater than 0!" ControlToValidate="txtDiscountPercentage" 
                Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Integer" 
                ValueToCompare="0" resourcekey="valCompDiscountPercentage"></asp:CompareValidator>
        </td>
    </tr>
    <tr id="trDiscountAmount" runat="server" valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblDiscountAmount" runat="server" controlname="txtDiscountAmount"></dnn:label>
        </td>
        <td class="Normal">
            <asp:TextBox id="txtDiscountAmount" Runat="server" Width="100" MaxLength="10" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="valReqDiscountAmount" runat="server" ControlToValidate="txtDiscountAmount" Display="Dynamic" ErrorMessage="* Discount amount is required." resourcekey="valReqDiscountAmount" SetFocusOnError="true"></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="valCompDiscountAmount" runat="server" 
                ErrorMessage="* Must be greater than 0!" ControlToValidate="txtDiscountAmount" 
                Display="Dynamic" Operator="GreaterThan" SetFocusOnError="True" Type="Currency" 
                ValueToCompare="0" resourcekey="valCompDiscountAmount"></asp:CompareValidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblApplyTo" runat="server" controlname="rblApplyTo"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="rblApplyTo" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblApplyTo_SelectedIndexChanged">
                <asp:ListItem resourcekey="ApplyToOrder" Value="0" Selected="True">Entire order</asp:ListItem>
                <asp:ListItem resourcekey="ApplyToCategory" Value="1">All items in category</asp:ListItem>
                <asp:ListItem resourcekey="ApplyToProduct" Value="2">Specific product</asp:ListItem>
            </asp:RadioButtonList>
            <asp:CustomValidator ID="valCustValidateItem" runat="server" 
                ControlToValidate="rblApplyTo" Display="Dynamic" EnableClientScript="False" 
                ErrorMessage="* Please validate your choice!" 
                onservervalidate="valCustValidateItem_ServerValidate" SetFocusOnError="True"></asp:CustomValidator>
        </td>
    </tr>
    <tr id="trCategory" runat="server" visible="false">
        <td class="SubHead" style="width:200px;">
            <dnn:label id="lblCategory" runat="server" controlname="lstCategory"></dnn:label>
        </td>
        <td style="vertical-align:top;">
            <asp:DropDownList ID="lstCategory" runat="server" CssClass="NormalTextBox" OnSelectedIndexChanged="lstCategory_SelectedIndexChanged" ></asp:DropDownList>
            <asp:RequiredFieldValidator ID="valReqCategory" runat="server" ControlToValidate="lstCategory" Display="Dynamic" ErrorMessage="* Select a category!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqCategory"></asp:RequiredFieldValidator>
            <asp:LinkButton ID="btnValidateCategory" runat="server" Text="Validate" ResourceKey="btnValidateCategory" CssClass="CommandButton" onclick="btnValidateCategory_Click" CausesValidation="false"/>
        </td>
    </tr>
    <tr id="trProduct" runat="server" visible="false">
        <td class="SubHead" style="width:200px;">
            <dnn:label id="lblProduct" runat="server" controlname="lstProduct"></dnn:label>
        </td>
        <td style="vertical-align:top;">
            <asp:DropDownList ID="lstProduct" runat="server" CssClass="NormalTextBox"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="valReqProduct" runat="server" ControlToValidate="lstProduct" Display="Dynamic" ErrorMessage="* Select a product!" InitialValue="-1" SetFocusOnError="True" resourcekey="valReqProduct"></asp:RequiredFieldValidator>
            <asp:LinkButton ID="btnValidateProduct" runat="server" Text="Validate" ResourceKey="btnValidateProduct" CssClass="CommandButton" onclick="btnValidateProduct_Click" CausesValidation="false"/>
        </td>
    </tr>
    <tr id="trSelectedItem" runat="server" valign="top" visible="false">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblSelectedItem" runat="server" controlname="lblItemName"></dnn:label>
        </td>
        <td class="Normal">
            <asp:HiddenField ID="hfItemID" runat="server" />
            <asp:Label ID="lblItemName" runat="server"></asp:Label>
            <asp:LinkButton ID="btnChangeItem" runat="server" Text="Change Item" ResourceKey="btnChangeItem" CssClass="CommandButton" onclick="btnChangeItem_Click" CausesValidation="false"/>
        </td>
    </tr>
    <tr id="trIncludeSubCategories" runat="server" visible="false">
        <td class="SubHead" style="width:200px;">
            <dnn:label id="lblIncludeSubCategories" runat="server" controlname="chkIncludeSubCategories"></dnn:label>
        </td>
        <td style="vertical-align:top;">
            <asp:CheckBox ID="chkIncludeSubCategories" runat="server" />
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead">
            <dnn:label id="lblStartDate" runat="server" controlname="txtStartDate"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server" CssClass="NormalTextBox" Width="80"></asp:TextBox>
            <asp:HyperLink ID="cmdStartDate" runat="server" CssClass="CommandButton"></asp:HyperLink>
            <asp:RequiredFieldValidator ID="valReqStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="* Start date is required." resourcekey="valReqStartDate" Display="Dynamic" SetFocusOnError="true" cssclass="NormalRed"></asp:RequiredFieldValidator>
            <asp:comparevalidator id="valCompStartDate" cssclass="NormalRed" runat="server" resourcekey="valCompStartDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtStartDate"></asp:comparevalidator>
        </td>
    </tr>
    <tr valign="top">
        <td class="SubHead" style="width:200px">
            <dnn:label id="lblValidity" runat="server" controlname="rblValidity"></dnn:label>
        </td>
        <td>
            <asp:RadioButtonList ID="rblValidity" runat="server" CausesValidation="False" AutoPostBack="True" onselectedindexchanged="rblValidity_SelectedIndexChanged">
                <asp:ListItem resourcekey="ValidityPermanent" Value="0" Selected="True">Permanent</asp:ListItem>
                <asp:ListItem resourcekey="ValiditySingleUse" Value="1">Single use</asp:ListItem>
                <asp:ListItem resourcekey="ValidityUntil" Value="2">Until</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="trEndDate" runat="server" visible="false" valign="top">
        <td class="SubHead">
            <dnn:label id="lblEndDate" runat="server" controlname="txtEndDate"></dnn:label>
        </td>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server" CssClass="NormalTextBox" Width="80"></asp:TextBox>
            <asp:HyperLink ID="cmdEndDate" runat="server" CssClass="CommandButton"></asp:HyperLink>
            <asp:RequiredFieldValidator ID="valReqEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="* End date is required." resourcekey="valReqEndDate" Display="Dynamic" SetFocusOnError="true" cssclass="NormalRed"></asp:RequiredFieldValidator>
            <asp:comparevalidator id="valCompEndDate" cssclass="NormalRed" runat="server" resourcekey="valCompEndDate" display="Dynamic" type="Date" operator="DataTypeCheck" errormessage="Error! Please enter a valid date." controltovalidate="txtEndDate"></asp:comparevalidator>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <asp:linkbutton id="cmdUpdate" CssClass="CommandButton" runat="server" resourcekey="cmdUpdate" onclick="cmdUpdate_Click">Update</asp:linkbutton>
            <asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" CausesValidation="False" resourcekey="cmdCancel" onclick="cmdCancel_Click">Cancel</asp:linkbutton>
            <asp:linkbutton id="cmdDelete" CssClass="CommandButton" runat="server" CausesValidation="False" Visible="False" resourcekey="cmdDelete" onclick="cmdDelete_Click">Delete</asp:linkbutton>
        </td>
    </tr>
</table>
