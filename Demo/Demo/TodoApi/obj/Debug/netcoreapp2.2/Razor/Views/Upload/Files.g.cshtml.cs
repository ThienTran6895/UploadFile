#pragma checksum "C:\Users\thientc\Desktop\Kendo UI\Demo\Demo\TodoApi\Views\Upload\Files.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "024ce5d888459a88cad9449074d7271775307161"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Upload_Files), @"mvc.1.0.view", @"/Views/Upload/Files.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Upload/Files.cshtml", typeof(AspNetCore.Views_Upload_Files))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"024ce5d888459a88cad9449074d7271775307161", @"/Views/Upload/Files.cshtml")]
    public class Views_Upload_Files : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WebAPI.Models.Upload.FileViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(43, 42, true);
            WriteLiteral("\r\n\r\n    <p>List of Files</p>\r\n\r\n    <ul>\r\n");
            EndContext();
#line 7 "C:\Users\thientc\Desktop\Kendo UI\Demo\Demo\TodoApi\Views\Upload\Files.cshtml"
         foreach (var item in Model.Files)
        {

#line default
#line hidden
            BeginContext(140, 58, true);
            WriteLiteral("            <li>\r\n                <a asp-action=\"Download\"");
            EndContext();
            BeginWriteAttribute("asp-route-filename", "\r\n                   asp-route-filename=\"", 198, "\"", 249, 1);
#line 11 "C:\Users\thientc\Desktop\Kendo UI\Demo\Demo\TodoApi\Views\Upload\Files.cshtml"
WriteAttributeValue("", 239, item.Name, 239, 10, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(250, 23, true);
            WriteLiteral(">\r\n                    ");
            EndContext();
            BeginContext(274, 9, false);
#line 12 "C:\Users\thientc\Desktop\Kendo UI\Demo\Demo\TodoApi\Views\Upload\Files.cshtml"
               Write(item.Name);

#line default
#line hidden
            EndContext();
            BeginContext(283, 43, true);
            WriteLiteral("\r\n                </a>\r\n            </li>\r\n");
            EndContext();
#line 15 "C:\Users\thientc\Desktop\Kendo UI\Demo\Demo\TodoApi\Views\Upload\Files.cshtml"
        }

#line default
#line hidden
            BeginContext(337, 21, true);
            WriteLiteral("    </ul>\r\n\r\n\r\n\r\n\r\n\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebAPI.Models.Upload.FileViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
