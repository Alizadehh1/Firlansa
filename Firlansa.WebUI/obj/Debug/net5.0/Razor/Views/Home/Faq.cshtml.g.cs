#pragma checksum "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ca7250749b5b99d70a8417fc48c2c0357351719f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Faq), @"mvc.1.0.view", @"/Views/Home/Faq.cshtml")]
namespace AspNetCore
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
#line 2 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI.Models.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI.AppCode.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI.Models.Entities;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Firlansa.WebUI.Models.FormModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ca7250749b5b99d70a8417fc48c2c0357351719f", @"/Views/Home/Faq.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09d3743615352b0f04d0f140fb7ec118dd40d209", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Faq : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<Faq>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/css/demo23.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
  
    ViewData["Title"] = "Faq";

#line default
#line hidden
#nullable disable
            WriteLiteral("<nav class=\"breadcrumb-nav\">\r\n    <div class=\"container\">\r\n        <ul class=\"breadcrumb\">\r\n            <li>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ca7250749b5b99d70a8417fc48c2c0357351719f5367", async() => {
                WriteLiteral("<i class=\"d-icon-home\"></i>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"</li>
            <li>FAQs</li>
        </ul>
    </div>
</nav>
<div class=""page-header"" style=""background-image: url(/assets/images/page-header/faq.jpg)"">
    <h3 class=""page-subtitle lh-1"">??n ??ox </h3>
    <h1 class=""page-title font-weight-bold text-capitalize lh-1"">Verilmi?? suallar</h1>
</div>
<div class=""page-content mb-10 pb-8"">
    <section>
        <div class=""container"">
            <div class=""row"">
");
#nullable restore
#line 21 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
                 foreach (var faq in Model)
                {
                    int id = faq.Id;

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    <div class=""col-md-6 mt-10"">
                        <div class=""accordion accordion-border accordion-boxed accordion-plus"">
                            <div class=""card"">
                                <div class=""card-header"">
                                    <a");
            BeginWriteAttribute("href", " href=\"", 1044, "\"", 1065, 2);
            WriteAttributeValue("", 1051, "#collapse", 1051, 9, true);
#nullable restore
#line 28 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
WriteAttributeValue("", 1060, id, 1060, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"collapse\">\r\n                                        ");
#nullable restore
#line 29 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
                                   Write(faq.Question);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                    </a>\r\n                                </div>\r\n                                <div");
            BeginWriteAttribute("id", " id=\"", 1259, "\"", 1277, 2);
            WriteAttributeValue("", 1264, "collapse", 1264, 8, true);
#nullable restore
#line 32 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
WriteAttributeValue("", 1272, id, 1272, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"expanded\">\r\n                                    <div class=\"card-body\">\r\n                                        <p>\r\n                                            ");
#nullable restore
#line 35 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
                                       Write(faq.Answer);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                        </p>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n");
#nullable restore
#line 42 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Home\Faq.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n        </div>\r\n    </section>\r\n</div>\r\n");
            DefineSection("css", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "ca7250749b5b99d70a8417fc48c2c0357351719f9894", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
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
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<Faq>> Html { get; private set; }
    }
}
#pragma warning restore 1591
