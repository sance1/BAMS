#pragma checksum "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "514cc781071eedce43812bddd0d0acfe4b2367a3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.District.Views_District_CreateUpdate), @"mvc.1.0.view", @"/Views/District/CreateUpdate.cshtml")]
namespace BAMS.Pages.District
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
#line 1 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
using BAMS.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"514cc781071eedce43812bddd0d0acfe4b2367a3", @"/Views/District/CreateUpdate.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    public class Views_District_CreateUpdate : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
  
    var accounts = (List<ReadAccountDTO>)ViewData["accounts"];    
    var district = (District)ViewData["District"];
    var title = district == null ? "Create" : "Update";
    long uid = district?.Uid ?? 0;

    long projectUid = (long)ViewData["ProjectUid"];
    var projectList = ViewData["ProjectList"] as List<Project>;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    <h2>");
#nullable restore
#line 14 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
   Write(title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" District</h2>\r\n    <input type=\"hidden\" id=\"uid\"");
            BeginWriteAttribute("value", " value=\"", 470, "\"", 482, 1);
#nullable restore
#line 15 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
WriteAttributeValue("", 478, uid, 478, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n\r\n    <div class=\"form-district col-3\" >\r\n        <label>Project :</label>\r\n        <select id=\"projectUid\" class=\"form-control\">    \r\n");
#nullable restore
#line 20 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
             foreach(var project in projectList)
            {
                if (project.Uid == projectUid)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "514cc781071eedce43812bddd0d0acfe4b2367a34831", async() => {
#nullable restore
#line 24 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                                                     Write(project.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 24 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                                WriteLiteral(project.Uid);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 25 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "514cc781071eedce43812bddd0d0acfe4b2367a37200", async() => {
#nullable restore
#line 28 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                                            Write(project.Name);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 28 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                       WriteLiteral(project.Uid);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("        \r\n");
#nullable restore
#line 29 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
                }
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </select>\r\n\r\n        <label>Nama :</label>\r\n        <input type=\"text\" id=\"name\"");
            BeginWriteAttribute("value", " value=\"", 1100, "\"", 1123, 1);
#nullable restore
#line 34 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
WriteAttributeValue("", 1108, district?.Name, 1108, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n        <label>PIC :</label>\r\n        <input type=\"text\" id=\"pic\"");
            BeginWriteAttribute("value", " value=\"", 1217, "\"", 1239, 1);
#nullable restore
#line 37 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
WriteAttributeValue("", 1225, district?.PIC, 1225, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />       \r\n                            \r\n        <label>Remark :</label>\r\n        <input type=\"text\" id=\"remarks\"");
            BeginWriteAttribute("value", " value=\"", 1375, "\"", 1401, 1);
#nullable restore
#line 40 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
WriteAttributeValue("", 1383, district?.Remarks, 1383, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" class=""form-control"" /> 
        
        <div class=""form-group mt-3"">
            <button id=""save"" type=""button"" class=""btn btn-primary"">Save</button>
            <button type=""button"" class=""btn btn-danger"" onclick=""goBack()"">Cancel</button>
        </div>
        
    </div>
    

");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"

    <script>

        $(document).ready(function () {

            $(""#save"").click(save);
        });

        function save() {
            let uid = parseInt($(""#uid"").val());

            let district = {
                uid: uid,
                projectUid: parseInt($(""#projectUid"").val()),
                name: $(""#name"").val(),
                pic: $(""#pic"").val(),
                students: parseInt($(""#students"").val()),
                remarks: $(""#remarks"").val(),
            };

            let validationResult = validateInput(district);
            if (validationResult.length > 0) {
                Swal.fire({
                    title: 'Error',
                    html: validationResult.join('<br>'),
                    icon: 'error'
                });
                return;
            }

            let url = uid == '0'
                ? '");
#nullable restore
#line 82 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
              Write(Url.Action("Create", "district"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\'\r\n                : \'");
#nullable restore
#line 83 "D:\8 Elements New\Projek\BAMS\BAMS\Views\District\CreateUpdate.cshtml"
              Write(Url.Action("Update", "district"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';

            sendRequest(url, JSON.stringify(district));
        }

        function validateInput(input) {
            let err = [];
            if(input.name == """"){
                err.push(""District name cannot be empty"");
            }
            return err;
        }

        function sendRequest(url, payload) {
            $.ajax({
                type: ""POST"",
                url: url,
                contentType: 'application/json; charset=utf-8',
                data: payload,
                success: function (response) {
                    if (response.status == 0) {
                        Swal.fire({
                            title: 'Success',
                            text: response.message,
                            icon: 'success'
                        }).then((result) => {
                            history.back();
                            location.href = ""/District"";
                        });
                    }
                },
        ");
                WriteLiteral(@"        error: function(response){
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                }
            });
        }
        
        function goBack(){
            history.back();
        }

    </script>
");
            }
            );
        }
        #pragma warning restore 1998
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
