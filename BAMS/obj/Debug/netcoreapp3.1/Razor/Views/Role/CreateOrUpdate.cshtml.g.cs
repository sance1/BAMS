#pragma checksum "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "de1b2acf153b6afd52898e6b0f09238e9d4c9772"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.Role.Views_Role_CreateOrUpdate), @"mvc.1.0.view", @"/Views/Role/CreateOrUpdate.cshtml")]
namespace BAMS.Pages.Role
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/_ViewImports.cshtml"
using BAMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
using EightElements.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de1b2acf153b6afd52898e6b0f09238e9d4c9772", @"/Views/Role/CreateOrUpdate.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Role_CreateOrUpdate : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("create-form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/core/role.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("application/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
  
    var id = ViewBag.Id;
    var data = ViewBag.Role as Role;
    var clientLanguage = (string) ViewBag.ClientLanguage;
    var updateRole = @PageText.GetHtml("Role_title_update_role", clientLanguage);
    var createRole = PageText.GetHtml("Role_title_create_role", clientLanguage);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"col-6\">\r\n    <div class=\"modal-content\" style=\"box-shadow:none;\">\r\n        <div class=\"modal-header\">\r\n            <h4 class=\"modal-title\">");
#nullable restore
#line 16 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
                                Write(id > 0 ? updateRole : createRole);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n        </div>\r\n        <div class=\"modal-body\">\r\n            <div class=\"form-horizontal\">\r\n                <div class=\"card-body\">\r\n\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "de1b2acf153b6afd52898e6b0f09238e9d4c97726437", async() => {
                WriteLiteral("\r\n                        <div class=\"form-group row\">\r\n                            <input type=\"hidden\" name=\"id\" id=\"id\"");
                BeginWriteAttribute("value", " value=\"", 906, "\"", 917, 1);
#nullable restore
#line 24 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
WriteAttributeValue("", 914, id, 914, 3, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"id\">\r\n                            <label class=\"col-sm-2 col-form-label\">");
#nullable restore
#line 25 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
                                                              Write(PageText.GetHtml("Role_txt_name", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("  </label>\r\n                            <div class=\"col-sm-9\">\r\n                                <input autocomplete=\"off\" class=\"form-control\" type=\"text\" name=\"name\" id=\"create-name\"");
                BeginWriteAttribute("value", " value=\"", 1238, "\"", 1257, 1);
#nullable restore
#line 27 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
WriteAttributeValue("", 1246, data?.Name, 1246, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"name\">\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"form-group row\">\r\n                            <label class=\"col-sm-2 col-form-label\">");
#nullable restore
#line 31 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
                                                              Write(PageText.GetHtml("Role_txt_access", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("  </label>\r\n                            <div class=\"col-sm-9\">\r\n                                <input autocomplete=\"off\" class=\"form-control\" type=\"number\" name=\"access\" id=\"create-access\"");
                BeginWriteAttribute("value", " value=\"", 1710, "\"", 1736, 1);
#nullable restore
#line 33 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
WriteAttributeValue("", 1718, data?.AccessLevel, 1718, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"access\">\r\n                            </div>\r\n                        </div>\r\n                        <div class=\"modal-footer\">\r\n                            ");
#nullable restore
#line 37 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
                       Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            <button class=\"btn-orange\" type=\"submit\">");
#nullable restore
#line 38 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
                                                                 Write(id > 0 ? PageText.GetHtml("Role_txt_update", clientLanguage) : PageText.GetHtml("Role_txt_create", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                        </div>\r\n\r\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n\r\n<div class=\"pg-create-role\">\r\n    <h2>");
#nullable restore
#line 51 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
   Write(PageText.GetHtml("Role_txt_list_menu", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    <table id=\"tbl-create-role\" class=\"display\">\r\n        <thead>\r\n            <tr>\r\n                <td>");
#nullable restore
#line 55 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
               Write(PageText.GetHtml("Role_tbl_txt_name", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 56 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
               Write(PageText.GetHtml("Role_tbl_txt_group", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                <td>");
#nullable restore
#line 57 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Role/CreateOrUpdate.cshtml"
               Write(PageText.GetHtml("Role_tbl_txt_access", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            </tr>\r\n        </thead>\r\n    </table>\r\n</div>\r\n\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "de1b2acf153b6afd52898e6b0f09238e9d4c977213239", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    <script>\r\n        $(document).ready(function () {\r\n            getMenu();\r\n        })\r\n    </script>\r\n");
            }
            );
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ITextService PageText { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
