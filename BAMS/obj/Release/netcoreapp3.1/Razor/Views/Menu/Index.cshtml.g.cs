#pragma checksum "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ef3ce58b06aaa2f8080512be2512562f2b033ecb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.Menu.Views_Menu_Index), @"mvc.1.0.view", @"/Views/Menu/Index.cshtml")]
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
#line 1 "D:\8 Elements New\Projek\BAMS\BAMS\Views\_ViewImports.cshtml"
using BAMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
using EightElements.Services;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ef3ce58b06aaa2f8080512be2512562f2b033ecb", @"/Views/Menu/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    public class Views_Menu_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/content/logo8e.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("logo8el"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/icon/create-project.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("create-form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("application/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 7 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
  
    var id = ViewBag.Id;
    var data = ViewBag.Menu as AccessPermission;
    var clientLanguage = (string) ViewBag.ClientLanguage;
    var createMenu = PageText.GetHtml("Access_permissions_form_title_create_menu", clientLanguage);
    var updateMenu = PageText.GetHtml("Access_permissions_form_title_update_menu", clientLanguage);
    var update = PageText.GetHtml("Access_permissions_form_btn_update",clientLanguage);
    var create = PageText.GetHtml("Access_permissions_form_btn_create",clientLanguage);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"title-page flex\">\r\n    <h2>");
#nullable restore
#line 18 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
   Write(PageText.GetHtml("Access_permissions_txt_access_permissions", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ef3ce58b06aaa2f8080512be2512562f2b033ecb7096", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n<a class=\"btn-orange btn-icon\" href=\"javascript:void(0)\" onclick=\"openModalMenu()\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ef3ce58b06aaa2f8080512be2512562f2b033ecb8312", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(" ");
#nullable restore
#line 22 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                                                                          Write(PageText.GetHtml("Access_permissions_btn_create_access_permission", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n<div class=\"tbl-general tbl-spc\">\r\n    <table id=\"menu-list\" class=\"display tbl-padding\">\r\n        <thead>\r\n            <tr>\r\n                <th class=\"id\" style=\"width: 10px\">");
#nullable restore
#line 27 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                              Write(PageText.GetHtml("Access_permissions_tbl_txt_id", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 28 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_name", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 29 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_group", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 30 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_permission", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 31 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_actions", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n            </tr>\r\n        </thead>\r\n        <tfoot>\r\n            <tr>\r\n                <th>");
#nullable restore
#line 36 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_id", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 37 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_name", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 38 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_group", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 39 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_permission", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</th>\r\n                <th>");
#nullable restore
#line 40 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
               Write(PageText.GetHtml("Access_permissions_tbl_txt_actions", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</th>
            </tr>
        </tfoot>
    </table>
</div>

<div class=""modal fade"" tabindex=""-1"" role=""dialog"" id=""modal-menu"">
    <div class=""modal-dialog"" role=""document"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <h4 class=""modal-title"">");
#nullable restore
#line 50 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                    Write(id > 0 ? updateMenu : createMenu);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h4>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
            <div class=""modal-body"">
                <div class=""form-horizontal"">
                    <div class=""card-body"">
                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ef3ce58b06aaa2f8080512be2512562f2b033ecb13897", async() => {
                WriteLiteral("\r\n                            <input type=\"hidden\" name=\"id\" id=\"id\"");
                BeginWriteAttribute("value", " value=\"", 3104, "\"", 3115, 1);
#nullable restore
#line 59 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 3112, id, 3112, 3, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" placeholder=\"id\">\r\n                            <div class=\"form-group row\">\r\n                                <label for=\"name\" class=\"col-sm-3 col-form-label\">");
#nullable restore
#line 61 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                             Write(PageText.GetHtml("Access_permissions_form_txt_name", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <div class=\"col-sm-9\">\r\n                                    <input autocomplete=\"off\" class=\"form-control\" type=\"text\" name=\"name\" id=\"name\"");
                BeginWriteAttribute("value", " value=\"", 3527, "\"", 3546, 1);
#nullable restore
#line 63 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 3535, data?.Name, 3535, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 3547, "\"", 3641, 1);
#nullable restore
#line 63 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 3561, PageText.GetHtml("Access_permissions_form_txt_name_placeholder",clientLanguage), 3561, 80, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"form-group row\">\r\n                                <label for=\"group\" class=\"col-sm-3 col-form-label\">");
#nullable restore
#line 67 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                              Write(PageText.GetHtml("Access_permissions_form_txt_group",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <div class=\"col-sm-9\">\r\n                                    <input autocomplete=\"off\" class=\"form-control\" type=\"text\" name=\"group\" id=\"group\"");
                BeginWriteAttribute("value", " value=\"", 4115, "\"", 4135, 1);
#nullable restore
#line 69 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 4123, data?.Group, 4123, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 4136, "\"", 4221, 1);
#nullable restore
#line 69 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 4150, PageText.GetHtml("Access_permissions_form_txt_group_placeholder","en"), 4150, 71, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"form-group row\">\r\n                                <label for=\"permission\" class=\"col-sm-3 col-form-label\">");
#nullable restore
#line 73 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                                   Write(PageText.GetHtml("Access_permissions_form_txt_permission", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <div class=\"col-sm-9\">\r\n                                    <input autocomplete=\"off\" class=\"form-control\" type=\"number\" name=\"permission\" id=\"permission\"");
                BeginWriteAttribute("value", " value=\"", 4718, "\"", 4743, 1);
#nullable restore
#line 75 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 4726, data?.Permission, 4726, 17, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 4744, "\"", 4844, 1);
#nullable restore
#line 75 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 4758, PageText.GetHtml("Access_permissions_form_txt_permission_placeholder",clientLanguage), 4758, 86, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"form-group row\">\r\n                                <label for=\"permission\" class=\"col-sm-3 col-form-label\">");
#nullable restore
#line 79 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                                   Write(PageText.GetHtml("Access_permissions_form_txt_url",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <div class=\"col-sm-9\">\r\n                                    <input  autocomplete=\"off\" class=\"form-control\" type=\"text\" name=\"menuUrl\" id=\"menuUrl\"");
                BeginWriteAttribute("value", " value=\"", 5326, "\"", 5348, 1);
#nullable restore
#line 81 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 5334, data?.MenuUrl, 5334, 14, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 5349, "\"", 5442, 1);
#nullable restore
#line 81 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 5363, PageText.GetHtml("Access_permissions_form_txt_url_placeholder",clientLanguage), 5363, 79, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" disabled>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"form-group row\">\r\n                                <label for=\"permission\" class=\"col-sm-3 col-form-label\">");
#nullable restore
#line 85 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                                   Write(PageText.GetHtml("Access_permissions_form_txt_menu_order",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</label>\r\n                                <div class=\"col-sm-9\">\r\n                                    <input autocomplete=\"off\" class=\"form-control\" type=\"text\" name=\"menuOrder\" id=\"menuOrder\"");
                BeginWriteAttribute("value", " value=\"", 5943, "\"", 5967, 1);
#nullable restore
#line 87 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 5951, data?.MenuOrder, 5951, 16, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("placeholder", " placeholder=\"", 5968, "\"", 6068, 1);
#nullable restore
#line 87 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
WriteAttributeValue("", 5982, PageText.GetHtml("Access_permissions_form_txt_menu_order_placeholder",clientLanguage), 5982, 86, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" disabled>\r\n                                </div>\r\n                            </div>\r\n\r\n                            ");
#nullable restore
#line 91 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                       Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                            <div class=\"modal-footer\">\r\n                                <button type=\"submit\" class=\"btn-green\">");
#nullable restore
#line 94 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                    Write(id > 0 ? update : create);

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                                <button type=\"button\" class=\"btn-grey\" data-dismiss=\"modal\">");
#nullable restore
#line 95 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
                                                                                       Write(PageText.GetHtml("Access_permissions_form_btn_cancel", clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("</button>\r\n                            </div>\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ef3ce58b06aaa2f8080512be2512562f2b033ecb25071", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 6770, "~/js/core/menu.js?version=", 6770, 26, true);
#nullable restore
#line 106 "D:\8 Elements New\Projek\BAMS\BAMS\Views\Menu\Index.cshtml"
AddHtmlAttributeValue("", 6796, _config["Resources:Version"], 6796, 29, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <script>
        $(document).ready(function () {
            const params = new Proxy(new URLSearchParams(window.location.search), {
                get: (searchParams, prop) => searchParams.get(prop),
            });
            let id = params.id;
            if (id) openModalMenu()
            getData();
        });
        
    </script>
");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IConfiguration _config { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ITextService PageText { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
