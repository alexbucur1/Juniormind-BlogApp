#pragma checksum "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d42648b9f4d55c585922d3a15966a74cd8c93a63"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Components_RepliesSection__RepliesSection), @"mvc.1.0.view", @"/Views/Shared/Components/RepliesSection/_RepliesSection.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d42648b9f4d55c585922d3a15966a74cd8c93a63", @"/Views/Shared/Components/RepliesSection/_RepliesSection.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"614bb461a8b77953305aa9e911ea4594ea8e4e40", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Components_RepliesSection__RepliesSection : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BlogApp.Dotnet.Web.ViewModels.CommentViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div>\r\n");
#nullable restore
#line 4 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
     foreach (var replyViewModel in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"container ml-3\"");
            BeginWriteAttribute("id", " id=\"", 167, "\"", 207, 2);
            WriteAttributeValue("", 172, "comment", 172, 7, true);
#nullable restore
#line 6 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 179, replyViewModel.Comment.ID, 179, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                <div class=\"row\">\r\n                    <div class=\"card card-body bg-light\">\r\n                        <p");
            BeginWriteAttribute("id", " id=\"", 331, "\"", 378, 2);
            WriteAttributeValue("", 336, "reply-content-", 336, 14, true);
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 350, replyViewModel.Comment.ID, 350, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("><b>");
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                                                                         Write(replyViewModel.Comment.UserFullName);

#line default
#line hidden
#nullable disable
            WriteLiteral(": </b><span class=\"text-justify\" style=\"white-space:pre-wrap\">");
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                                                                                                                                                                           Write(Html.DisplayFor(comment => replyViewModel.Comment.Content));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span></p>
                    </div>
                </div>
                <div class=""row row-cols-auto gap-1"">
                    <div class=""col"">
                        <button type=""button"" class=""btn p-0 m-0"" data-bs-toggle=""collapse"" disabled>
                            ");
#nullable restore
#line 15 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                       Write(Html.DisplayFor(comment => replyViewModel.Comment.Date));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </button>\r\n                    </div>\r\n");
#nullable restore
#line 18 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                     if (replyViewModel.IsOwnerOrAdmin)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"col\">\r\n                            <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"collapse\"");
            BeginWriteAttribute("onclick", "\r\n                                    onclick=\"", 1165, "\"", 1261, 3);
            WriteAttributeValue("", 1212, "closeReplyFeature(\'", 1212, 19, true);
#nullable restore
#line 22 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 1231, replyViewModel.Comment.ID, 1231, 28, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1259, "\')", 1259, 2, true);
            EndWriteAttribute();
            BeginWriteAttribute("id", "\r\n                                    id=\"", 1262, "\"", 1342, 2);
            WriteAttributeValue("", 1304, "editButton", 1304, 10, true);
#nullable restore
#line 23 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 1314, replyViewModel.Comment.ID, 1314, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("\r\n                                    data-bs-target=\"#editComment");
#nullable restore
#line 24 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                                                            Write(replyViewModel.Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                                    aria-controls=\"", 1460, "\"", 1552, 2);
            WriteAttributeValue("", 1513, "editComment", 1513, 11, true);
#nullable restore
#line 25 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 1524, replyViewModel.Comment.ID, 1524, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                Edit\r\n                            </button>\r\n                        </div>\r\n");
#nullable restore
#line 29 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 30 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                     if (replyViewModel.IsOwnerOrAdmin)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"col\">\r\n                            <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"modal\"\r\n                                    data-bs-target=\"#deleteComment");
#nullable restore
#line 34 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                                                              Write(replyViewModel.Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                                    aria-controls=\"", 2018, "\"", 2112, 2);
            WriteAttributeValue("", 2071, "deleteComment", 2071, 13, true);
#nullable restore
#line 35 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 2084, replyViewModel.Comment.ID, 2084, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                Delete\r\n                            </button>\r\n                        </div>\r\n");
#nullable restore
#line 39 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"col\">\r\n                            <button type=\"button\" class=\"btn p-0\" data-bs-toggle=\"collapse\"");
            BeginWriteAttribute("onclick", "\r\n                                    onclick=\"", 2384, "\"", 2479, 3);
            WriteAttributeValue("", 2431, "closeEditFeature(\'", 2431, 18, true);
#nullable restore
#line 42 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 2449, replyViewModel.Comment.ID, 2449, 28, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2477, "\')", 2477, 2, true);
            EndWriteAttribute();
            BeginWriteAttribute("id", "\r\n                                    id=\"", 2480, "\"", 2561, 2);
            WriteAttributeValue("", 2522, "replyButton", 2522, 11, true);
#nullable restore
#line 43 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 2533, replyViewModel.Comment.ID, 2533, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("\r\n                                    data-bs-target=\"#replyToComment");
#nullable restore
#line 44 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                                                               Write(replyViewModel.Comment.ID);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" aria-expanded=\"false\"");
            BeginWriteAttribute("aria-controls", "\r\n                                    aria-controls=\"", 2682, "\"", 2777, 2);
            WriteAttributeValue("", 2735, "replyToComment", 2735, 14, true);
#nullable restore
#line 45 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 2749, replyViewModel.Comment.ID, 2749, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                Reply\r\n                            </button>\r\n                        </div>\r\n                    </div>\r\n                <div class=\"collapse container-fluid\"");
            BeginWriteAttribute("id", " id=\"", 2972, "\"", 3016, 2);
            WriteAttributeValue("", 2977, "editComment", 2977, 11, true);
#nullable restore
#line 50 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 2988, replyViewModel.Comment.ID, 2988, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <div>\r\n                        ");
#nullable restore
#line 52 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                    Write(await Component.InvokeAsync("CommentEdit", replyViewModel.Comment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n                <div>\r\n                    ");
#nullable restore
#line 56 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                Write(await Component.InvokeAsync("CommentDelete", replyViewModel.Comment));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n                <div class=\"collapse container-fluid\"");
            BeginWriteAttribute("id", " id=\"", 3387, "\"", 3434, 2);
            WriteAttributeValue("", 3392, "replyToComment", 3392, 14, true);
#nullable restore
#line 58 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
WriteAttributeValue("", 3406, replyViewModel.Comment.ID, 3406, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <div>\r\n                        ");
#nullable restore
#line 60 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
                    Write(await Component.InvokeAsync("CommentReply", new { receiverID = replyViewModel.Comment.ParentID, postID = replyViewModel.Comment.PostID }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n            </div>\r\n            <br />\r\n");
#nullable restore
#line 65 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Shared\Components\RepliesSection\_RepliesSection.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BlogApp.Dotnet.Web.ViewModels.CommentViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
