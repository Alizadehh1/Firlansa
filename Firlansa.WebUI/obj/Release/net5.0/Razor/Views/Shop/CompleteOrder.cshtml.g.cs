#pragma checksum "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Shop\CompleteOrder.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1d7304960be60f171d0bf1e62bdc5fafcf8cea7a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shop_CompleteOrder), @"mvc.1.0.view", @"/Views/Shop/CompleteOrder.cshtml")]
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
#nullable restore
#line 8 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using System.Globalization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\_ViewImports.cshtml"
using Resources;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1d7304960be60f171d0bf1e62bdc5fafcf8cea7a", @"/Views/Shop/CompleteOrder.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e69c718b0497370eb6f2339725ec33eec2cb2871", @"/Views/_ViewImports.cshtml")]
    public class Views_Shop_CompleteOrder : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Shop\CompleteOrder.cshtml"
  
    ViewData["Title"] = "CompleteOrder";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""page-content pt-7 pb-10 mb-10"">
    <div class=""step-by pr-4 pl-4"">
        <h3 class=""title title-simple title-step""><a href=""cart.html"">1. Shopping Cart</a></h3>
        <h3 class=""title title-simple title-step""><a href=""checkout.html"">2. Checkout</a></h3>
        <h3 class=""title title-simple title-step active""><a href=""order.html"">");
#nullable restore
#line 10 "C:\Users\alide\OneDrive\Desktop\Firlansa\Firlansa.WebUI\Views\Shop\CompleteOrder.cshtml"
                                                                         Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</a></h3>
    </div>
    <div class=""container mt-8"">
        <div class=""order-message mr-auto ml-auto"">
            <div class=""icon-box d-inline-flex align-items-center"">
                <div class=""icon-box-icon mb-0"">
                    <svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" viewBox=""0 0 50 50"" enable-background=""new 0 0 50 50"" xml:space=""preserve"">
                    <g>
                    <path fill=""none"" stroke-width=""3"" stroke-linecap=""round"" stroke-linejoin=""bevel"" stroke-miterlimit=""10"" d=""
											M33.3,3.9c-2.7-1.1-5.6-1.8-8.7-1.8c-12.3,0-22.4,10-22.4,22.4c0,12.3,10,22.4,22.4,22.4c12.3,0,22.4-10,22.4-22.4
											c0-0.7,0-1.4-0.1-2.1""></path>
                    <polyline fill=""none"" stroke-width=""4"" stroke-linecap=""round"" stroke-linejoin=""bevel"" stroke-miterlimit=""10"" points=""
											48,6.9 24.4,29.8 17.2,22.3 	""></polyline>
									</g>
								</svg>
                </div>
    ");
            WriteLiteral(@"            <div class=""icon-box-content text-left"">
                    <h5 class=""icon-box-title font-weight-bold lh-1 mb-1"">Thank You!</h5>
                    <p class=""lh-1 ls-m"">Your order has been received</p>
                </div>
            </div>
        </div>
    </div>
</div>

");
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