#pragma checksum "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "148c2b510fca71aea79961d9aa9e1752b8ce0b94"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(BAMS.Pages.Message.Views_Message_CreateMessage), @"mvc.1.0.view", @"/Views/Message/CreateMessage.cshtml")]
namespace BAMS.Pages.Message
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
#line 1 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
using BAMS.Data.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
using EightElements.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"148c2b510fca71aea79961d9aa9e1752b8ce0b94", @"/Views/Message/CreateMessage.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c958eec748d779defd14101a5c949d29ea388c6b", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Message_CreateMessage : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/content/logo8e.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("logo8el"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
  
    var message = (Message)ViewData["Message"];
    var action = (string)ViewData["Action"];
    var toTeacher = (bool)ViewData["ToTeacher"];
    var toAdmin = (bool)ViewData["ToAdmin"];
    var toDistrict = (bool)ViewData["ToDistrict"];
    var toProvince = (bool)ViewData["ToProvince"];


    var textMessage = "";
    var textTitle = "";
    var recipients = "";
    if (message != null)
    {
        textMessage = $"<p>&nbsp;</p><blockquote>{message.Body}</blockquote>";
        textTitle = message.Title;
        if (action == "reply")
        {
            recipients = message.Account.UserName;
        }
        else if (action == "forward")
        {
            recipients = "";
        }
    }

    var clientLanguage = (string)ViewBag.ClientLanguage;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"title-page flex\">\r\n    <h2>");
#nullable restore
#line 34 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
   Write(PageText.GetHtml("Message_txt_send_message", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "148c2b510fca71aea79961d9aa9e1752b8ce0b945368", async() => {
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
            WriteLiteral(@"
</div>

<div class=""panel-message flex"">

    <div class=""col-9 no-pad"">
        <div class=""full-message"">
            <div class=""flex head-message"">
                <div class=""col-9 no-pad"">
                    <div class=""form-group row"">
                        <label class=""col-sm-2 col-form-label"">");
#nullable restore
#line 45 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                          Write(PageText.GetHtml("Message_txt_to", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                        <div class=\"col-sm-10 no-pad\">\r\n");
            WriteLiteral("                            <select id=\"to\" class=\"js-example-basic-multiple form-control\" name=\"states[]\" multiple=\"multiple\">\r\n                            </select>\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 52 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                     if (toAdmin || toProvince || toTeacher || toDistrict)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"form-group row message-option\">\r\n                            <div class=\"col-sm-2\">&nbsp; </div>\r\n                            <div class=\"col-sm-10 no-pad\">\r\n");
#nullable restore
#line 57 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                 if (toAdmin)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <label>\r\n                                        ");
#nullable restore
#line 60 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                   Write(PageText.GetHtml("Message_radiobutton_txt_send_to_bibalala_admin", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" <input type=\"checkbox\" name=\"choice\" id=\"toAdmin\" />\r\n                                    </label>\r\n");
#nullable restore
#line 62 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 63 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                 if (toProvince)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <label>\r\n                                        ");
#nullable restore
#line 66 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                   Write(PageText.GetHtml("Message_radiobutton_txt_send_to_all_province", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" <input type=\"checkbox\" name=\"choice\" id=\"allProvince\" />\r\n                                    </label>\r\n");
#nullable restore
#line 68 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 69 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                 if (toTeacher)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <label>\r\n                                        ");
#nullable restore
#line 72 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                   Write(PageText.GetHtml("Message_radiobutton_txt_send_to_all_teachers", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" <input type=\"checkbox\" name=\"choice\" id=\"allTeacher\" />\r\n                                    </label>\r\n");
#nullable restore
#line 74 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                }

#line default
#line hidden
#nullable disable
#nullable restore
#line 75 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                 if (toDistrict)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <label>\r\n                                        ");
#nullable restore
#line 78 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                   Write(PageText.GetHtml("Message_radiobutton_txt_send_to_all_district", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" <input type=\"checkbox\" name=\"choice\" id=\"allDistrict\" />\r\n                                    </label>\r\n");
#nullable restore
#line 80 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n");
#nullable restore
#line 84 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"form-group row\">\r\n                        <label class=\"col-sm-2 col-form-label\">");
#nullable restore
#line 86 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                          Write(PageText.GetHtml("Message_txt_title_whencreatemessage", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral(" </label>\r\n                        <div class=\"col-sm-10 no-pad\">\r\n                            <input class=\"form-control\" type=\"text\" id=\"title\"");
            BeginWriteAttribute("placeholder", " placeholder=\"", 4109, "\"", 4123, 0);
            EndWriteAttribute();
            BeginWriteAttribute("value", " value=\"", 4124, "\"", 4142, 1);
#nullable restore
#line 88 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
WriteAttributeValue("", 4132, textTitle, 4132, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n                <div class=\"col-3 head-right\">\r\n                    <span class=\"date\">");
#nullable restore
#line 93 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                  Write(DateTime.Now.ToString("dd/MM/yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                </div>\r\n            </div>\r\n            <div class=\"message-inbox\">\r\n                <textarea id=\"bodyMessage\"");
            BeginWriteAttribute("placeholder", " placeholder=\"", 4489, "\"", 4566, 1);
#nullable restore
#line 97 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
WriteAttributeValue("", 4503, PageText.GetHtml("Message_txt_type_text_here", clientLanguage), 4503, 63, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 97 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                                                                                    Write(textMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("</textarea>\r\n            </div>\r\n\r\n            <div class=\"attachment\">\r\n                <span class=\"text-limit\">\r\n                    <span id=\"char-count\">0</span>");
#nullable restore
#line 102 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                             Write(PageText.GetHtml("Message_txt_300_character", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </span>\r\n                <div class=\"attachFile\">\r\n                    <label for=\"attachFileMessage\">\r\n                        <i class=\"fa fa-paperclip\" aria-hidden=\"true\"></i>\r\n                        ");
#nullable restore
#line 107 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                   Write(PageText.GetHtml("Message_txt_attachment", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </label>\r\n                    <input type=\"file\" id=\"attachFileMessage\" hidden accept=\".xlsx,.xls,.jpg,.png,.bmp,.doc,.docx,.pdf\" multiple />\r\n                    <span id=\"file-attach\" class=\"file-attach\">");
#nullable restore
#line 110 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                          Write(PageText.GetHtml("Message_txt_no_file_choosen", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                </div>\r\n                <div class=\"btn-message flex\">\r\n                    <button class=\"btn-orange\" onclick=\"sendMessage()\"> ");
#nullable restore
#line 113 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                                   Write(PageText.GetHtml("Message_btn_send", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n                    <button class=\"btn-grey\" onclick=\"goBack()\"> ");
#nullable restore
#line 114 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                                                            Write(PageText.GetHtml("Message_btn_cancel", clientLanguage));

#line default
#line hidden
#nullable disable
            WriteLiteral("</button>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script src=""https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js""></script>
    <script>
        $(document).ready(function(){
            $('#bodyMessage').summernote();
            $('#bodyMessage').on('summernote.change summernote.keyup', function(we, contents, $editable){
                let count = $(this).summernote('code').length;
                $(""#char-count"").html(count);
            });
            initAutoComplete()
        });

        function initAutoComplete() {
          $('#to').select2({
          ajax: {
              delay: 500,
              url: '/message/SearchName',
              data: function (params) {
                  return {
                      search: params.term
                  }
                  },
                  processResults: function (data) {
                  return {
                      results: data
                  };
                  },
                  }
          });
        }

        function g");
                WriteLiteral(@"oBack(){
            location.href = ""/message"";
        }

        function sendMessage(){
            let to = $(""#to"").val();
            let title = $(""#title"").val();
            let bodyText = $(""#bodyMessage"").summernote('code');
            let allTeacher = $(""#allTeacher"").is("":checked"")
            let toAdmin = $(""#toAdmin"").is("":checked"")
            let allDistrict = $(""#allDistrict"").is("":checked"")
            let allProvince = $(""#allProvince"").is("":checked"")
  

            if(to.length == 0 && !allTeacher && !toAdmin && !allDistrict && !allProvince){
                Swal.fire({
                    title: 'Error',
                    text: """);
#nullable restore
#line 170 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                      Write(PageText.GetHtml("Message_popup_txt_recipient_empty",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("\",\r\n                    icon: \'error\'\r\n                });\r\n                return;\r\n            }\r\n            if(title == \"\"){\r\n                Swal.fire({\r\n                    title: \'Error\',\r\n                    text: \"");
#nullable restore
#line 178 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                      Write(PageText.GetHtml("Message_popup_txt_title_empty",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral("\",\r\n                    icon: \'error\'\r\n                });\r\n                return;\r\n            }\r\n            if(bodyText == \"\"){\r\n                Swal.fire({\r\n                    title: \'Error\',\r\n                    text: \"");
#nullable restore
#line 186 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                      Write(PageText.GetHtml("Message_popup_txt_body_empty",clientLanguage));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                    icon: 'error'
                });
                return;
            }

            let formData = new FormData();
            formData.append('Title',title);
            formData.append('Body',bodyText);
            formData.append('SenderId',0);
            formData.append('AllTeacher',allTeacher);
            formData.append('AllDistrict',allDistrict);
            formData.append('ToAdmin',toAdmin);
            formData.append('AllProvince', allProvince);
   

            to.map(val => {
                formData.append('Recipients[]',val.trim());
            })
            // for(let acc of to.split(',')){
            //     if (acc){
            //      formData.append('Recipients[]',acc.trim());
            //     }
            // }
            for(let fl of $(""#attachFileMessage"")[0].files){
                formData.append('Attachments',fl);
            }

            Swal.showLoading();
            $.ajax({
                type: ""POST"",
           ");
                WriteLiteral(@"     url: '/message/create',
                data: formData,
                cache: false,
				contentType: false,
				processData: false,
                success: function (response) {
                    Swal.close();
                    if (response.status == 0) {
                        Swal.fire({
                            title: 'Success',
                            text: """);
#nullable restore
#line 227 "/Users/sanceaenulyakin/Documents/8 Elements/Projek/Bams/BAMS/Views/Message/CreateMessage.cshtml"
                              Write(PageText.GetHtml("Message_popup_txt_sent_seccessfully","en"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@""",
                            icon: 'success'
                        }).then((result) => {
                            location.href = ""/message"";
                        });

                        return;
                    }
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                },
                error: function(response){
                    Swal.close();
                    Swal.fire({
                        title: 'Error',
                        text: response.responseJSON.message,
                        icon: 'error'
                    });
                }
            });
        }

        const actualBtn = document.getElementById('attachFileMessage');

        const fileChosen = document.getElementById('file-attach');

        actualBtn.addEventListener('change', function () {
            fileChosen.textContent = this.fi");
                WriteLiteral("les[0].name\r\n        })\r\n    </script>\r\n\r\n");
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
