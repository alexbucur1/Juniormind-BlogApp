#pragma checksum "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "01e7c63f6c5e068bfa3a10f24b0f6247a05d6c37"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_ShowCommentReplies__ShowCommentReplies), @"mvc.1.0.view", @"/Views/Shared/Components/ShowCommentReplies/_ShowCommentReplies.cshtml")]
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
#line 1 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\_ViewImports.cshtml"
using BlogApp.Dotnet.ApplicationCore.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"01e7c63f6c5e068bfa3a10f24b0f6247a05d6c37", @"/Views/Shared/Components/ShowCommentReplies/_ShowCommentReplies.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"614bb461a8b77953305aa9e911ea4594ea8e4e40", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_ShowCommentReplies__ShowCommentReplies : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BlogApp.Dotnet.ApplicationCore.DTOs.CommentsDTO>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div>\r\n");
#nullable restore
#line 4 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
     foreach (var Comment in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"container-fluid\"");
            BeginWriteAttribute("id", " id=\"", 158, "\"", 183, 2);
            WriteAttributeValue("", 163, "comment", 163, 7, true);
#nullable restore
#line 6 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 170, Comment.ID, 170, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n            <div class=\"row\">\r\n                <div class=\"card card-body bg-light\">\r\n                    <p> ");
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                    Write(await Component.InvokeAsync("UserFullName", Comment.UserID));

#line default
#line hidden
#nullable disable
            WriteLiteral("<span class=\"text-justify\" style=\"white-space:pre-wrap\">");
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                                                                                                                                         Write(Html.DisplayFor(comment => Comment.Content));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></p>\r\n                </div>\r\n            </div>\r\n            <div class=\"row row-cols-auto gap-3\">\r\n                <div class=\"col\">  ");
#nullable restore
#line 13 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                              Write(Html.DisplayFor(comment => Comment.Date));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                <div class=\"col\">\r\n                    <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"collapse\"\r\n                            data-bs-target=\"#editComment");
#nullable restore
#line 16 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                                                    Write(Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                            aria-controls=\"", 863, "\"", 932, 2);
            WriteAttributeValue("", 908, "editComment", 908, 11, true);
#nullable restore
#line 17 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 919, Comment.ID, 919, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        Edit\r\n                    </button>\r\n                    <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"modal\"\r\n                            data-bs-target=\"#deleteComment");
#nullable restore
#line 21 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                                                      Write(Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                            aria-controls=\"", 1173, "\"", 1244, 2);
            WriteAttributeValue("", 1218, "deleteComment", 1218, 13, true);
#nullable restore
#line 22 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 1231, Comment.ID, 1231, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        Delete\r\n                    </button>\r\n                    <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"collapse\"\r\n                            data-bs-target=\"#replyToComment");
#nullable restore
#line 26 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                                                       Write(Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                            aria-controls=\"", 1491, "\"", 1563, 2);
            WriteAttributeValue("", 1536, "replyToComment", 1536, 14, true);
#nullable restore
#line 27 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 1550, Comment.ID, 1550, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        Reply\r\n                    </button>\r\n                    <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"collapse\"\r\n                            data-bs-target=\"#showRepliesForComment");
#nullable restore
#line 31 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
                                                              Write(Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                            aria-controls=\"", 1816, "\"", 1895, 2);
            WriteAttributeValue("", 1861, "showRepliesForComment", 1861, 21, true);
#nullable restore
#line 32 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 1882, Comment.ID, 1882, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        Show Repplies\r\n                    </button>\r\n\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <div class=\"collapse container\"");
            BeginWriteAttribute("id", " id=\"", 2070, "\"", 2099, 2);
            WriteAttributeValue("", 2075, "editComment", 2075, 11, true);
#nullable restore
#line 39 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 2086, Comment.ID, 2086, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"width:100%;\">\r\n            <div>\r\n                ");
#nullable restore
#line 41 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
            Write(await Component.InvokeAsync("CommentEdit", Comment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div>\r\n            ");
#nullable restore
#line 45 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
        Write(await Component.InvokeAsync("CommentDelete", Comment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n        <div class=\"collapse container\"");
            BeginWriteAttribute("id", " id=\"", 2390, "\"", 2422, 2);
            WriteAttributeValue("", 2395, "replyToComment", 2395, 14, true);
#nullable restore
#line 47 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
WriteAttributeValue("", 2409, Comment.ID, 2409, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"width:100%;\">\r\n            <div>\r\n                ");
#nullable restore
#line 49 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
            Write(await Component.InvokeAsync("CommentReply", Comment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <br />\r\n");
#nullable restore
#line 53 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\ShowCommentReplies\_ShowCommentReplies.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BlogApp.Dotnet.ApplicationCore.DTOs.CommentsDTO>> Html { get; private set; }
    }
}
#pragma warning restore 1591
