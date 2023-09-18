#pragma checksum "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2119219e90e5c33c58c6957f8fa5210f9f5e0518"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.UserAccount.Views_UserAccount_Detail), @"mvc.1.0.view", @"/Views/UserAccount/Detail.cshtml")]
namespace BAMS.Pages.UserAccount
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
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2119219e90e5c33c58c6957f8fa5210f9f5e0518", @"/Views/UserAccount/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_UserAccount_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
  
    var student = (UserAccount)ViewData["Student"];
    var title = student == null ? "Create" : "Update";
    long uid = student?.Uid ?? 0;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<h2>");
#nullable restore
#line 9 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
Write(title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" school</h2>\r\n<input type=\"hidden\" id=\"uid\"");
            BeginWriteAttribute("value", " value=\"", 234, "\"", 246, 1);
#nullable restore
#line 10 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 242, uid, 242, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n\r\n<div class=\"form-school col-3\">\r\n\r\n    <label>Class:</label>\r\n    <input type=\"text\" id=\"class\"");
            BeginWriteAttribute("value", " value=\"", 349, "\"", 372, 1);
#nullable restore
#line 15 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 357, student?.Class, 357, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n    <label>Student name:</label>\r\n    <input type=\"text\" id=\"name\"");
            BeginWriteAttribute("value", " value=\"", 467, "\"", 489, 1);
#nullable restore
#line 18 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 475, student?.Name, 475, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n    <label>Username:</label>\r\n    <input type=\"text\" id=\"username\"");
            BeginWriteAttribute("value", " value=\"", 584, "\"", 610, 1);
#nullable restore
#line 21 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 592, student?.UserName, 592, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n    <label>Phone number:</label>\r\n    <input type=\"text\" id=\"phoneNumber\"");
            BeginWriteAttribute("value", " value=\"", 712, "\"", 741, 1);
#nullable restore
#line 24 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 720, student?.PhoneNumber, 720, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n    <label>Email:</label>\r\n    <input type=\"text\" id=\"email\"");
            BeginWriteAttribute("value", " value=\"", 830, "\"", 853, 1);
#nullable restore
#line 27 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
WriteAttributeValue("", 838, student?.Email, 838, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\" />\r\n\r\n    <div class=\"form-group mt-3\">\r\n        <button id=\"save\" type=\"button\" class=\"btn btn-primary\">Save</button>\r\n        <button type=\"button\" class=\"btn btn-danger\">Cancel</button>\r\n    </div>\r\n\r\n</div>\r\n\r\n\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"

    <script>

        $(document).ready(function () {

            $(""#save"").click(function() {

                let uid = parseInt($(""#uid"").val());

                let student = {
                    uid: uid,
                    class: $(""#class"").val(),
                    name: $(""#name"").val(),
                    username: $(""#username"").val(),
                    phoneNumber: $(""#phoneNumber"").val(),
                    email: $(""#email"").val()
                };

                let validationResult = validateInput(student);
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
#line 69 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
                  Write(Url.Action("Create", "Student"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\'\r\n                    : \'");
#nullable restore
#line 70 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/UserAccount/Detail.cshtml"
                  Write(Url.Action("Update", "Student"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"';

                sendRequest(url,  JSON.stringify(student))
            });
        });

        function validateInput(input) {
            let results = [];
            if(input.name == """"){
                results.push(""Name cannot be empty"");
            }
            if(input.username == """"){
                results.push(""Username cannot be empty"");
            }
            if(input.students == """"){
                results.push(""Contact person cannot empty"");
            }
            return results;
        }

        function sendRequest(url, payload) {
            $.ajax({
                type: 'POST',
                url: url,
                contentType: 'application/json; charset=utf-8',
                data: payload,
                success: function (response) {
                    if (response.status == 0) {
                        Swal.fire({
                            title: 'Success',
                            text: response.message,
                     ");
                WriteLiteral(@"       icon: 'success'
                        }).then((result) => {
                            history.back();
                            location.href = ""/School"";
                        });
                    }
                },
                error: function(response){
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                }
            });
        }

        function testSave() {
            $.ajax({
                type: 'POST',
                url: ""/school/Create"",
                data: {Name:""sance""},
                success: function (respon) {
                    alert(""ok"");
                },
                error: function () { alert(""Err TestSave."") }
            });
        }



    </script>
");
            }
            );
        }
        #pragma warning restore 1998
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
