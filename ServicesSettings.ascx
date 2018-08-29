<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServicesSettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.ServicesSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label id="lblScriptObjectName" runat="server" controlname="txtScriptObjectName"></dnn:Label>
            <asp:TextBox id="txtScriptObjectName" runat="server"></asp:TextBox>
        </div>
        <div class="dnnFormItem">
			<dnn:label id="lblOptions" runat="server" controlname="txtOptions"></dnn:label>
            <asp:TextBox id="txtOptions" runat="server"></asp:TextBox>
        </div>
    </fieldset>
</div>
