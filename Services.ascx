<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Services.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Services" %>
<script type="text/javascript">
    <%= ScriptObjectName %>.init($.ServicesFramework(<%= ModuleId %>), <%= Options %>);
</script>
