#pragma checksum "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9b8ad7dfd6086308764f0407d7a4d964cead0274"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.Contract.Views_Contract_CreateUpdateACReq), @"mvc.1.0.view", @"/Views/Contract/CreateUpdateACReq.cshtml")]
namespace BAMS.Pages.Contract
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
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9b8ad7dfd6086308764f0407d7a4d964cead0274", @"/Views/Contract/CreateUpdateACReq.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Contract_CreateUpdateACReq : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
  
    var projectData = (Project)ViewData["project"];
    var contractData = (Contract)ViewData["contract"];
    var acReqData = (ActivationCodeRequest)ViewData["acReq"];
    var projectId = (int)ViewData["projectId"];
    var contractId = (int)ViewData["contractId"];
    var title = "Update";
    if(acReqData == null)
    {
        title = "Create";
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div>\r\n    <h2>");
#nullable restore
#line 18 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
   Write(title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Activation Code Request</h2>\r\n    <input type=\"hidden\" id=\"acReqId\"");
            BeginWriteAttribute("value", " value=\"", 495, "\"", 517, 1);
#nullable restore
#line 19 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 503, acReqData?.Id, 503, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <input type=\"hidden\" id=\"projectId\"");
            BeginWriteAttribute("value", " value=\"", 561, "\"", 579, 1);
#nullable restore
#line 20 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 569, projectId, 569, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <input type=\"hidden\" id=\"contractId\"");
            BeginWriteAttribute("value", " value=\"", 624, "\"", 643, 1);
#nullable restore
#line 21 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 632, contractId, 632, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <input type=\"text\" id=\"projectName\" readonly placeholder=\"Project\"");
            BeginWriteAttribute("value", " value=\"", 718, "\"", 744, 1);
#nullable restore
#line 22 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 726, projectData?.Name, 726, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <input type=\"text\" id=\"contractName\" readonly placeholder=\"Contract\"");
            BeginWriteAttribute("value", " value=\"", 821, "\"", 848, 1);
#nullable restore
#line 23 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 829, contractData?.Name, 829, 19, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <input type=\"text\" id=\"remarks\" placeholder=\"Remarks\"");
            BeginWriteAttribute("value", " value=\"", 910, "\"", 937, 1);
#nullable restore
#line 24 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Contract/CreateUpdateACReq.cshtml"
WriteAttributeValue("", 918, acReqData?.Remarks, 918, 19, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\r\n    <button type=\"submit\" onclick=\"save()\">Save</button>\r\n</div>\r\n\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>
        $(document).ready(function () {
            
            
        });

        function save(){
            let data ={
                ProjectId: parseInt($(""#projectId"").val()) || 0,
                ContractId: parseInt($(""#contractId"").val()) || 0,
                Remarks: $(""#remarks"").val(),
            };

            let err = [];
            if(data.ProjectId == """" || data.ProjectId <= 0){
                err.push(""Project cannot empty"");
            }
            if(data.ContractId == """" || data.ContractId <= 0){
                err.push(""Contract cannot empty"");
            }
            if(err.length > 0){
                Swal.fire({
                    title: 'Error',
                    text: err.join(""<br/>""),
                    icon: 'error'
                });
                return;
            }

            let url = ""/contract/createacrequest"";
            let id = $(""#acReqId"").val();
            if(id && id != """"){
                d");
                WriteLiteral(@"ata.Id = parseInt(id);
                url = ""/contract/updateacrequest""
            }

            $.ajax({
                type: ""POST"",
                url: url,
                dataType: ""json"",
                contentType: ""application/json; charset=utf-8"",
                data: JSON.stringify(data),
                success: function (response) {
                    if (response.status == 0) {
                        Swal.fire({
                            title: 'Success',
                            text: response.message,
                            icon: 'success'
                        }).then((result) => {
                            history.back();
                        });
                        
                        return;
                    }
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                },
                er");
                WriteLiteral(@"ror: function(response){
                    Swal.fire({
                        title: 'Error',
                        text: response.responseJSON.message,
                        icon: 'error'
                    });
                }
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
