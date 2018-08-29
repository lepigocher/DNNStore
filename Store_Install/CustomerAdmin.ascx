<%@ Control language="c#" CodeBehind="CustomerAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblOrderNumber" runat="server" controlname="tbOrderNumber"></dnn:label>
            <asp:TextBox id="tbOrderNumber" Width="100" runat="server"></asp:TextBox>
            <asp:Button id="btnSearch" CssClass="dnnSecondaryAction" resourcekey="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblCustomers" runat="server" controlname="lstCustomers"></dnn:label>
            <asp:DropDownList id="lstCustomers" Runat="server" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblOrderStatus" runat="server" controlname="lstOrderStatus"></dnn:label>
            <asp:DropDownList id="lstOrderStatus" runat="server" AutoPostBack="true"></asp:DropDownList>
        </div>
        <div class="dnnFormMessage dnnFormWarning" id="divNoOrderFound" runat="server" Visible="false">
            <asp:Label id="noOrdersFound" resourcekey="noOrdersFound" Text="No Order found for this criteria" runat="server"></asp:Label>
        </div>
    </fieldset>
</div>
<asp:placeholder id="plhOrders" runat="server" Visible="False" />
