#pragma checksum "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6873367357995ba7f569ee6631007c4f0684e059"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.Menu.Views_Menu_CreateOrUpdate), @"mvc.1.0.view", @"/Views/Menu/CreateOrUpdate.cshtml")]
namespace BAMS.Pages.Menu
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
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
using EightElements.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6873367357995ba7f569ee6631007c4f0684e059", @"/Views/Menu/CreateOrUpdate.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Menu_CreateOrUpdate : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("create-form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/core/menu.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 5 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
  
    var id = ViewBag.Id;
    var data = ViewBag.Menu as AccessPermission;
    var clientLanguage = (string) ViewBag.ClientLanguage;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"col-4\">\r\n    <div class=\"card card-primary\">\r\n        <div class=\"card-header\">\r\n            <h3 class=\"card-title\">");
#nullable restore
#line 15 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                               Write(id > 0 ? PageText.GetHtml("Update", clientLanguage) : PageText.GetHtml("Create", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 15 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                                                                                                                                  Write(PageText.GetHtml("Menu", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n        </div>\r\n        <div class=\"card-body\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6873367357995ba7f569ee6631007c4f0684e0596600", async() => {
                WriteLiteral("\r\n                <input type=\"hidden\" name=\"id\" id=\"id\"");
                BeginWriteAttribute("value", " value=\"", 676, "\"", 687, 1);
#nullable restore
#line 19 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
WriteAttributeValue("", 684, id, 684, 3, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"id\">\r\n                <div class=\"form-group\">\r\n                    <label>");
#nullable restore
#line 21 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                      Write(PageText.GetHtml("Name", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral(" </label>\r\n                    <input class=\"form-control\" type=\"text\" name=\"name\" id=\"name\"");
                BeginWriteAttribute("value", " value=\"", 910, "\"", 929, 1);
#nullable restore
#line 22 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
WriteAttributeValue("", 918, data?.Name, 918, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"name\">\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <label>");
#nullable restore
#line 25 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                      Write(PageText.GetHtml("Group", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("  </label>\r\n                    <input class=\"form-control\" type=\"text\" name=\"group\" id=\"group\"");
                BeginWriteAttribute("value", " value=\"", 1182, "\"", 1202, 1);
#nullable restore
#line 26 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
WriteAttributeValue("", 1190, data?.Group, 1190, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"group\">\r\n                </div>\r\n                <div class=\"form-group\">\r\n                    <label>");
#nullable restore
#line 29 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                      Write(PageText.GetHtml("Permission", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral(" </label>\r\n                    <input class=\"form-control\" type=\"number\" name=\"permission\" id=\"permission\"");
                BeginWriteAttribute("value", " value=\"", 1472, "\"", 1497, 1);
#nullable restore
#line 30 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
WriteAttributeValue("", 1480, data?.Permission, 1480, 17, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"Permission\">\r\n                </div>\r\n                ");
#nullable restore
#line 32 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
           Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                <div class=\"form-group mt-2\">\r\n                    <button type=\"submit\" class=\" btn btn-primary\">");
#nullable restore
#line 34 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Menu/CreateOrUpdate.cshtml"
                                                               Write(id > 0 ? PageText.GetHtml("Update", clientLanguage) : PageText.GetHtml("Create", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                </div>\r\n            ");
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
            WriteLiteral("\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6873367357995ba7f569ee6631007c4f0684e05912261", async() => {
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
                WriteLiteral("\r\n");
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
