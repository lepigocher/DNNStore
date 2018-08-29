<%@ Control language="c#" CodeBehind="CatalogSettings.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CatalogSettings" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm">
    <h2 id="fshGenSettings" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shGenSettings" runat="server" ResourceKey="shGenSettings">General Settings</asp:Label> </a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCatTemplate" runat="server" ResourceKey="lblCatTemplate" ControlName="lstTemplate" Text="Catalog Template:" />
            <asp:dropdownlist id="lstTemplate" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblUseDefaultCategory" runat="server" ResourceKey="lblUseDefaultCategory" ControlName="chkUseDefaultCategory" Text="Use Default Category:" />
            <asp:checkbox id="chkUseDefaultCategory" runat="server" AutoPostBack="True" OnCheckedChanged="chkUseDefaultCategory_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="trDefaultCategory" runat="server">
            <dnn:Label ID="lblDefaultCategory" runat="server" ResourceKey="lblDefaultCategory" ControlName="lstDefaultCategory" Text="Default Category:" />
            <asp:dropdownlist id="lstDefaultCategory" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trDisplayAllProducts" runat="server">
            <dnn:Label ID="lblDisplayAllProducts" runat="server" ResourceKey="lblDisplayAllProducts" ControlName="chkDisplayAllProducts" Text="Display All Products:" />
            <asp:checkbox id="chkDisplayAllProducts" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowCategoryMsg" runat="server" ResourceKey="lblShowCategoryMsg" ControlName="chkShowMessage" Text="Show Category Message:" />
            <asp:checkbox id="chkShowMessage" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowCategoryProducts" runat="server" ResourceKey="lblShowCategoryProducts" ControlName="chkShowCategory" Text="Show Category Products:" />
            <asp:checkbox id="chkShowCategory" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowCategory_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowProductDetail" runat="server" ResourceKey="lblShowProductDetail" ControlName="chkShowDetail" Text="Show Product Detail:" />
            <asp:checkbox id="chkShowDetail" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowDetail_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem" id="trShowAlsoBoughtProducts" runat="server">
            <dnn:Label ID="lblShowAlsoBoughtProducts" runat="server" ResourceKey="lblShowAlsoBoughtProducts" ControlName="chkShowAlsoBought" Text="Show Also Bought Products:" />
            <asp:checkbox id="chkShowAlsoBought" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowAlsoBought_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowNewProducts" runat="server" ResourceKey="lblShowNewProducts" ControlName="chkShowNew" Text="Show New Products:" />
            <asp:checkbox id="chkShowNew" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowNew_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowFeaturedProducts" runat="server" ResourceKey="lblShowFeaturedProducts" ControlName="chkShowFeatured" Text="Show Featured Products:" />
            <asp:checkbox id="chkShowFeatured" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowFeatured_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblShowPopularProducts" runat="server" ResourceKey="lblShowPopularProducts" ControlName="chkShowPopular" Text="Show Popular Products:" />
            <asp:checkbox id="chkShowPopular" runat="server" AutoPostBack="True" OnCheckedChanged="chkShowPopular_CheckedChanged"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblAllowPrint" runat="server" ResourceKey="lblAllowPrint" ControlName="chkAllowPrint" Text="Allow Print?:" />
            <asp:checkbox id="chkAllowPrint" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEnableContentIndexing" runat="server" ResourceKey="lblEnableContentIndexing" ControlName="chkEnableContentIndexing" Text="Enable Content Indexing:" />
            <asp:checkbox id="chkEnableContentIndexing" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEnableImageCaching" runat="server" ResourceKey="lblEnableImageCaching" ControlName="chkEnableImageCaching" Text="Enable Image Caching:" />
            <asp:checkbox id="chkEnableImageCaching" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCacheDuration" runat="server" ResourceKey="lblCacheDuration" ControlName="txtCacheDuration" Text="Cache Duration:" />
            <asp:textbox id="txtCacheDuration" runat="server" width="80"></asp:textbox>
        </div>
    </fieldset>
    <h2 id="fshCategoryProductList" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shCategoryProductList" runat="server" ResourceKey="shCategoryProductList">Category Product Settings</asp:Label></a></h2>
    <fieldset id="fsCategoryProductList" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSContainerTemplate" runat="server" ResourceKey="lblCSContainerTemplate" ControlName="lstCPLContainerTemplate" Text="Container Template:" />
            <asp:dropdownlist id="lstCPLContainerTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSListTemplate" runat="server" ResourceKey="lblCSListTemplate" ControlName="lstCPLTemplate" Text="List Template:" />
            <asp:dropdownlist id="lstCPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSRows" runat="server" ResourceKey="lblCSRows" ControlName="txtCPLRowCount" Text="Rows:" />
            <asp:textbox id="txtCPLRowCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSColumns" runat="server" ResourceKey="lblCSColumns" ControlName="txtCPLColumnCount" Text="Columns:" />
            <asp:textbox id="txtCPLColumnCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSColumnWidth" runat="server" ResourceKey="lblCSColumnWidth" ControlName="txtCPLColumnWidth" Text="Column Width:" />
            <asp:textbox id="txtCPLColumnWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSRepeatDirection" runat="server" ResourceKey="lblCSRepeatDirection" ControlName="lstCPLRepeatDirection" Text="Repeat Direction:" />
            <asp:dropdownlist id="lstCPLRepeatDirection" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSShowThumbnail" runat="server" ResourceKey="lblCSShowThumbnail" ControlName="chkCPLShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkCPLShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSThumbnailWidth" runat="server" ResourceKey="lblCSThumbnailWidth" ControlName="txtCPLThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtCPLThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCPSGIFBgColor" runat="server" ResourceKey="lblCPSGIFBgColor" ControlName="txtCPLGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtCPLGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSDetailPage" runat="server" ResourceKey="lblCSDetailPage" ControlName="lstCPLDetailPage" Text="Detail Page:" />
            <asp:dropdownlist id="lstCPLDetailPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSSubCategories" runat="server" ResourceKey="lblCSSubCategories" ControlName="chkCPLSubCategories" Text="Sub-Categories:" />
            <asp:checkbox id="chkCPLSubCategories" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSRepositioning" runat="server" ResourceKey="lblCSRepositioning" ControlName="chkCPLRepositioning" Text="Repositioning:" />
            <asp:checkbox id="chkCPLRepositioning" runat="server"></asp:checkbox>
        </div>
    </fieldset>
    <h2 id="fshSearchProduct" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shSearchProduct" runat="server" ResourceKey="shSearchProduct">General Settings</asp:Label></a></h2>
    <fieldset id="fsSearchProduct" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSSearchColumns" runat="server" ResourceKey="lblCSSearchColumns" ControlName="chkSearchManufacturer" Text="Search Columns:" />
            <div>
                <asp:CheckBox ID="chkSearchManufacturer" runat="server" ResourceKey="chkSearchManufacturer" AutoPostBack="True" OnCheckedChanged="chkSearchManufacturer_CheckedChanged" /><br />
                <asp:CheckBox ID="chkSearchModelNumber" runat="server" ResourceKey="chkSearchModelNumber" AutoPostBack="True" OnCheckedChanged="chkSearchModelNumber_CheckedChanged" /><br />
                <asp:CheckBox ID="chkSearchModelName" runat="server" ResourceKey="chkSearchModelName" AutoPostBack="True" OnCheckedChanged="chkSearchModelName_CheckedChanged" /><br />
                <asp:CheckBox ID="chkSearchSummary" runat="server" ResourceKey="chkSearchSummary" AutoPostBack="True" OnCheckedChanged="chkSearchSummary_CheckedChanged" /><br />
                <asp:CheckBox ID="chkSearchDescription" runat="server" ResourceKey="chkSearchDescription" AutoPostBack="True" OnCheckedChanged="chkSearchDescription_CheckedChanged" />
            </div>
        </div>
        <div class="dnnFormItem" id="trSearchColumn" runat="server">
            <dnn:Label ID="lblSSSearchColumn" runat="server" ResourceKey="lblSSSearchColumn" ControlName="lstSPLSearchColumn" Text="Search Column:" />
            <asp:dropdownlist id="lstSPLSearchColumn" runat="server" enableviewstate="True" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trSearchTemplate" runat="server">
            <dnn:Label ID="lblSSListTemplate" runat="server" ResourceKey="lblSSListTemplate" ControlName="lstSPLTemplate" Text="Search Template:" />
            <asp:dropdownlist id="lstSPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshSortProduct" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shSortProduct" runat="server" ResourceKey="shSortProduct">General Settings</asp:Label></a></h2>
    <fieldset id="fsSortProduct" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblCSSortColumns" runat="server" ResourceKey="lblCSSortColumns" ControlName="chkSortManufacturer" Text="Sort Columns:" />
            <div>
            <asp:CheckBox ID="chkSortManufacturer" runat="server" ResourceKey="chkSortManufacturer" AutoPostBack="True" OnCheckedChanged="chkSortManufacturer_CheckedChanged" /><br />
            <asp:CheckBox ID="chkSortModelNumber" runat="server" ResourceKey="chkSortModelNumber" AutoPostBack="True" OnCheckedChanged="chkSortModelNumber_CheckedChanged" /><br />
            <asp:CheckBox ID="chkSortModelName" runat="server" ResourceKey="chkSortModelName" AutoPostBack="True" OnCheckedChanged="chkSortModelName_CheckedChanged" /><br />
            <asp:CheckBox ID="chkSortUnitPrice" runat="server" ResourceKey="chkSortUnitPrice" AutoPostBack="True" OnCheckedChanged="chkSortUnitPrice_CheckedChanged" /><br />
            <asp:CheckBox ID="chkSortCreatedDate" runat="server" ResourceKey="chkSortCreatedDate" AutoPostBack="True" OnCheckedChanged="chkSortCreatedDate_CheckedChanged" />
            </div>
        </div>
        <div class="dnnFormItem" id="trSortBy" runat="server">
            <dnn:Label ID="lblCSSortBy" runat="server" ResourceKey="lblCSSortBy" ControlName="lstCPLSortBy" Text="Sort By:" />
            <asp:dropdownlist id="lstCPLSortBy" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem" id="trSortDir" runat="server">>
            <dnn:Label ID="lblCSSortDir" runat="server" ResourceKey="lblCSSortDir" ControlName="lstCPLSortDir" Text="Direction:" />
            <asp:dropdownlist id="lstCPLSortDir" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshProductDetails" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shProductDetails" runat="server" ResourceKey="shProductDetails">General Settings</asp:Label></a></h2>
    <fieldset id="fsProductDetails" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblDetailTemplate" runat="server" ResourceKey="lblDetailTemplate" ControlName="lstDetailTemplate" Text="Detail Template:" />
            <asp:dropdownlist id="lstDetailTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSCartWarning" runat="server" ResourceKey="lblPDSCartWarning" ControlName="chkDetailCartWarning" Text="Cart Warning:" />
            <asp:checkbox id="chkDetailCartWarning" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSShowThumbnail" runat="server" ResourceKey="lblPDSShowThumbnail" ControlName="chkDetailShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkDetailShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSThumbnailWidth" runat="server" ResourceKey="lblPDSThumbnailWidth" ControlName="txtDetailThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtDetailThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSGIFBgColor" runat="server" ResourceKey="lblPDSGIFBgColor" ControlName="txtDetailGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtDetailGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSShowReviews" runat="server" ResourceKey="lblPDSShowReviews" ControlName="chkDetailShowReviews" Text="Show Reviews:" />
            <asp:CheckBox id="chkDetailShowReviews" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPDSReturnPage" runat="server" ResourceKey="lblPDSReturnPage" ControlName="lstPDSReturnPage" Text="Return To" />
            <asp:dropdownlist id="lstPDSReturnPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshAlsoBoughtProductList" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shAlsoBoughtProductList" runat="server" ResourceKey="shAlsoBoughtProductList">General Settings</asp:Label></a></h2>
    <fieldset id="fsAlsoBoughtProductList" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSContainerTemplate" runat="server" ResourceKey="lblABPSContainerTemplate" ControlName="lstABPLContainerTemplate" Text="Container Template:" />
            <asp:dropdownlist id="lstABPLContainerTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSListTemplate" runat="server" ResourceKey="lblABPSListTemplate" ControlName="lstABPLTemplate" Text="List Template:" />
            <asp:dropdownlist id="lstABPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSRows" runat="server" ResourceKey="lblABPSRows" ControlName="txtABPLRowCount" Text="Rows:" />
            <asp:textbox id="txtABPLRowCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSColumns" runat="server" ResourceKey="lblABPSColumns" ControlName="txtABPLColumnCount" Text="Columns:" />
            <asp:textbox id="txtABPLColumnCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSColumnWidth" runat="server" ResourceKey="lblABPSColumnWidth" ControlName="txtABPLColumnWidth" Text="Column Width:" />
            <asp:textbox id="txtABPLColumnWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSRepeatDirection" runat="server" ResourceKey="lblABPSRepeatDirection" ControlName="lstABPLRepeatDirection" Text="Repeat Direction:" />
            <asp:dropdownlist id="lstABPLRepeatDirection" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSShowThumbnail" runat="server" ResourceKey="lblABPSShowThumbnail" ControlName="chkABPLShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkABPLShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSThumbnailWidth" runat="server" ResourceKey="lblABPSThumbnailWidth" ControlName="txtABPLThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtABPLThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSGIFBgColor" runat="server" ResourceKey="lblABPSGIFBgColor" ControlName="txtABPLGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtABPLGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblABPSDetailPage" runat="server" ResourceKey="lblABPSDetailPage" ControlName="lstABPLDetailPage" Text="Detail Page:" />
            <asp:dropdownlist id="lstABPLDetailPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshNewProductList" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shNewProductList" runat="server" ResourceKey="shNewProductList">General Settings</asp:Label></a></h2>
    <fieldset id="fsNewProductList" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSContainerTemplate" runat="server" ResourceKey="lblNPSContainerTemplate" ControlName="lstNPLContainerTemplate" Text="Container Template:" />
            <asp:dropdownlist id="lstNPLContainerTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSListTemplate" runat="server" ResourceKey="lblNPSListTemplate" ControlName="lstNPLTemplate" Text="List Template:" />
            <asp:dropdownlist id="lstNPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSRows" runat="server" ResourceKey="lblNPSRows" ControlName="txtNPLRowCount" Text="Rows:" />
            <asp:textbox id="txtNPLRowCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSColumns" runat="server" ResourceKey="lblNPSColumns" ControlName="txtNPLColumnCount" Text="Columns:" />
            <asp:textbox id="txtNPLColumnCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSColumnWidth" runat="server" ResourceKey="lblNPSColumnWidth" ControlName="txtNPLColumnWidth" Text="Column Width:" />
            <asp:textbox id="txtNPLColumnWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSRepeatDirection" runat="server" ResourceKey="lblNPSRepeatDirection" ControlName="lstNPLRepeatDirection" Text="Repeat Direction:" />
            <asp:dropdownlist id="lstNPLRepeatDirection" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSDisplayByCategory" runat="server" ResourceKey="lblNPSDisplayByCategory" ControlName="chkNPLDisplayByCategory" Text="Display By Category:" />
            <asp:checkbox id="chkNPLDisplayByCategory" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSShowThumbnail" runat="server" ResourceKey="lblNPSShowThumbnail" ControlName="chkNPLShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkNPLShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSThumbnailWidth" runat="server" ResourceKey="lblNPSThumbnailWidth" ControlName="txtNPLThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtNPLThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSGIFBgColor" runat="server" ResourceKey="lblNPSGIFBgColor" ControlName="txtNPLGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtNPLGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblNPSDetailPage" runat="server" ResourceKey="lblNPSDetailPage" ControlName="lstNPLDetailPage" Text="Detail Page:" />
            <asp:dropdownlist id="lstNPLDetailPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshFeaturedProductList" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shFeaturedProductList" runat="server" ResourceKey="shFeaturedProductList">General Settings</asp:Label></a></h2>
    <fieldset id="fsFeaturedProductList" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSContainerTemplate" runat="server" ResourceKey="lblFPSContainerTemplate" ControlName="lstFPLContainerTemplate" Text="Container Template:" />
            <asp:dropdownlist id="lstFPLContainerTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSListTemplate" runat="server" ResourceKey="lblFPSListTemplate" ControlName="lstFPLTemplate" Text="List Template:" />
            <asp:dropdownlist id="lstFPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSRows" runat="server" ResourceKey="lblFPSRows" ControlName="txtFPLRowCount" Text="Rows:" />
            <asp:textbox id="txtFPLRowCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSColumns" runat="server" ResourceKey="lblFPSColumns" ControlName="txtFPLColumnCount" Text="Columns:" />
            <asp:textbox id="txtFPLColumnCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSColumnWidth" runat="server" ResourceKey="lblFPSColumnWidth" ControlName="txtFPLColumnWidth" Text="Column Width:" />
            <asp:textbox id="txtFPLColumnWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSRepeatDirection" runat="server" ResourceKey="lblFPSRepeatDirection" ControlName="lstFPLRepeatDirection" Text="Repeat Direction:" />
            <asp:dropdownlist id="lstFPLRepeatDirection" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSDisplayByCategory" runat="server" ResourceKey="lblFPSDisplayByCategory" ControlName="chkFPLDisplayByCategory" Text="Display By Category:" />
            <asp:checkbox id="chkFPLDisplayByCategory" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSShowThumbnail" runat="server" ResourceKey="lblFPSShowThumbnail" ControlName="chkFPLShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkFPLShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSThumbnailWidth" runat="server" ResourceKey="lblFPSThumbnailWidth" ControlName="txtFPLThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtFPLThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSGIFBgColor" runat="server" ResourceKey="lblFPSGIFBgColor" ControlName="txtFPLGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtFPLGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblFPSDetailPage" runat="server" ResourceKey="lblFPSDetailPage" ControlName="lstFPLDetailPage" Text="Detail Page:" />
            <asp:dropdownlist id="lstFPLDetailPage" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
    <h2 id="fshPopularProductList" runat="server" class="dnnFormSectionHead"><a href="#"><asp:Label ID="shPopularProductList" runat="server" ResourceKey="shPopularProductList">General Settings</asp:Label></a></h2>
    <fieldset id="fsPopularProductList" runat="server">
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSContainerTemplate" runat="server" ResourceKey="lblPPSContainerTemplate" ControlName="lstPPLContainerTemplate" Text="Container Template:" />
            <asp:dropdownlist id="lstPPLContainerTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSListTemplate" runat="server" ResourceKey="lblPPSListTemplate" ControlName="lstPPLTemplate" Text="List Template:" />
            <asp:dropdownlist id="lstPPLTemplate" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSRows" runat="server" ResourceKey="lblPPSRows" ControlName="txtPPLRowCount" Text="Rows:" />
            <asp:textbox id="txtPPLRowCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSColumns" runat="server" ResourceKey="lblPPSColumns" ControlName="txtPPLColumnCount" Text="Columns:" />
            <asp:textbox id="txtPPLColumnCount" runat="server" width="50" MaxLength="3"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSColumnWidth" runat="server" ResourceKey="lblPPSColumnWidth" ControlName="txtPPLColumnWidth" Text="Repeat Direction:" />
            <asp:textbox id="txtPPLColumnWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSRepeatDirection" runat="server" ResourceKey="lblPPSRepeatDirection" ControlName="lstPPLRepeatDirection" Text="Repeat Direction:" />
            <asp:dropdownlist id="lstPPLRepeatDirection" runat="server" autopostback="False"></asp:dropdownlist>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSShowThumbnail" runat="server" ResourceKey="lblPPSShowThumbnail" ControlName="chkPPLShowThumbnail" Text="Show Thumbnail:" />
            <asp:checkbox id="chkPPLShowThumbnail" runat="server"></asp:checkbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSThumbnailWidth" runat="server" ResourceKey="lblPPSThumbnailWidth" ControlName="txtPPLThumbnailWidth" Text="Thumbnail Width:" />
            <asp:textbox id="txtPPLThumbnailWidth" runat="server" width="50" MaxLength="4"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSGIFBgColor" runat="server" ResourceKey="lblPPSGIFBgColor" ControlName="txtPPLGIFBgColor" Text="GIF Background:" />
            <asp:textbox id="txtPPLGIFBgColor" runat="server" width="80" MaxLength="7"></asp:textbox>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblPPSDetailPage" runat="server" ResourceKey="lblPPSDetailPage" ControlName="lstPPLDetailPage" Text="Detail Page:" />
            <asp:dropdownlist id="lstPPLDetailPage" runat="server" CssClass="NormalTextBox" enableviewstate="True" autopostback="False"></asp:dropdownlist>
        </div>
    </fieldset>
</div>
<script type="text/javascript">
    jQuery(function ($) {
        var setupCatalogSettings = function () {
            $('.dnnForm').dnnPanels();
        };

        setupCatalogSettings();

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            // note that this will fire when _any_ UpdatePanel is triggered,
            // which may or may not cause an issue
            setupCatalogSettings();
        });
    });
</script>
