#pragma checksum "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c7bc2f7a0c15502ba5cc905355cdf6206c5a3774"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Posts_Details), @"mvc.1.0.view", @"/Views/Posts/Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7bc2f7a0c15502ba5cc905355cdf6206c5a3774", @"/Views/Posts/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"614bb461a8b77953305aa9e911ea4594ea8e4e40", @"/Views/_ViewImports.cshtml")]
    public class Views_Posts_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<BlogApp.Dotnet.Web.ViewModels.BlogPostViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("deleteButton"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
  
    ViewData["Title"] = @Html.DisplayFor(model => model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<br>\r\n<h1 class=\"post-title\">");
#nullable restore
#line 8 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                  Write(Html.DisplayFor(model => model.Title));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n<p class=\"text-muted mb-2 post-misc-data\">Written by: ");
#nullable restore
#line 9 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                 Write(Html.DisplayFor(model => model.Owner));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n\r\n<div class=\"row\">\r\n    <div class=\"col-sm\">\r\n        <pre class=\"full-post fs-6\">\r\n");
#nullable restore
#line 14 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
             if (Model.ShowPostImage) 
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("<img align=\"left\" class=\"post-img img-responsive img-thumbnail mt-1 mb-1\"");
            BeginWriteAttribute("src", " src=", 515, "", 535, 1);
#nullable restore
#line 15 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
WriteAttributeValue("", 520, Model.ImageURL, 520, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 15 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                                                                           }

#line default
#line hidden
#nullable disable
#nullable restore
#line 15 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                                                                       Write(Html.DisplayFor(post => Model.Content));

#line default
#line hidden
#nullable disable
            WriteLiteral("</pre>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"mb-4\">\r\n    <hr />\r\n    <div class=\"row row-cols-sm-auto\">\r\n        <p class=\"col-auto col-md-5 text-muted mb-0 post-misc-data\">Created:  ");
#nullable restore
#line 22 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                                         Write(Html.DisplayFor(model => model.CreatedAt));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n    </div>\r\n    <div class=\"row row-cols-sm-auto\">\r\n");
#nullable restore
#line 25 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
         if (@Model.ShowModifiedDate)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <p class=\"col-auto col-md-5 text-muted mb-0 post-misc-data\">Modified:  ");
#nullable restore
#line 27 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                                          Write(Html.DisplayFor(model => model.ModifiedAt));

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n");
#nullable restore
#line 28 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n<div>\r\n    <input type=\"button\" id=\"btn-back\" value=\"Back to Posts\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1132, "\"", 1186, 3);
            WriteAttributeValue("", 1142, "location.href=\'", 1142, 15, true);
#nullable restore
#line 32 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
WriteAttributeValue("", 1157, Url.Action("Index","Posts"), 1157, 28, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1185, "\'", 1185, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-outline-dark\" />\r\n");
#nullable restore
#line 33 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
     if (Model.IsOwnerOrAdmin)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <input type=\"button\" value=\"Edit\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1301, "\"", 1378, 3);
            WriteAttributeValue("", 1311, "location.href=\'", 1311, 15, true);
#nullable restore
#line 35 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
WriteAttributeValue("", 1326, Url.Action("Edit", "Posts", new { id = Model.ID }), 1326, 51, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1377, "\'", 1377, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-outline-dark\" />\r\n        <button type=\"button\" class=\"btn btn-outline-dark\" data-bs-toggle=\"modal\" data-bs-target=\"#deleteModal\">\r\n            Delete\r\n        </button>\r\n");
#nullable restore
#line 39 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <br />
    <hr />
    <button type=""button"" class=""btn btn-outline-dark"" data-bs-toggle=""collapse"" data-bs-target=""#commentsSection"" aria-expanded=""false"" aria-controls=""commentsSection"">
        Comments
    </button>
</div>

    <div class=""modal fade"" id=""deleteModal"" tabindex=""-1"" aria-labelledby=""deleteModalLabel"" aria-hidden=""true"">
        <div class=""modal-dialog"">
            <div class=""modal-content"">
                <div class=""modal-header"">
                    <h5 class=""modal-title"" id=""deleteModalLabel"">Confirm Deletion</h5>
                    <button type=""button"" class=""btn-close"" data-bs-dismiss=""modal"" aria-label=""Close""></button>
                </div>
                <div class=""modal-body"">
                    <h5>Are you sure you want to delete this post?</h5>
                </div>
                <div class=""modal-footer"">
                    <button type=""button"" class=""btn btn-outline-dark"" data-bs-dismiss=""modal"">Close</button>
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c7bc2f7a0c15502ba5cc905355cdf6206c5a377411789", async() => {
                WriteLiteral("\r\n                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c7bc2f7a0c15502ba5cc905355cdf6206c5a377412072", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 60 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.ID);

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                        <input id=\"deleteButtonSubmit\" type=\"submit\" value=\"Delete\" class=\"btn btn-danger\" />\r\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"collapse show\" id=\"commentsSection\">\r\n\r\n        <div class=\"d-flex flex-row-reverse bd-highlight\">\r\n\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c7bc2f7a0c15502ba5cc905355cdf6206c5a377415413", async() => {
                WriteLiteral("\r\n\r\n                <input type=\"text\" class=\"mt-1 align-middle\" name=\"SearchString\"");
                BeginWriteAttribute("value", " value=\"", 3189, "\"", 3216, 1);
#nullable restore
#line 74 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
WriteAttributeValue("", 3197, TempData["Search"], 3197, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                <input type=\"submit\" value=\"Search\" class=\"btn btn-outline-dark  mt-1\" />\r\n\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 72 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                         WriteLiteral(Model.ID);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n\r\n\r\n        <div class=\"collapse show\" id=\"commentsSection\">\r\n            <br />\r\n            ");
#nullable restore
#line 83 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
        Write(await Component.InvokeAsync("CommentsSection", new { PostID = Model.ID, searchString = @TempData["Search"], commsPageNumber = @TempData["CurrentCommsPage"] }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            <div>\r\n                ");
#nullable restore
#line 85 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
            Write(await Component.InvokeAsync("CommentsEditor", new { PostID = Model.ID}));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n\r\n        <br />\r\n        <script>\r\n    window.onload = function scrollToComment() {\r\n        var elmnt = document.getElementById(\"comment\" + ");
#nullable restore
#line 92 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                   Write(TempData["CommID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(");\r\n\r\n        if (");
#nullable restore
#line 94 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
       Write(TempData["ParentID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" != \"0\") {\r\n            var repliesSection = document.getElementById(\"showRepliesForComment\" + ");
#nullable restore
#line 95 "C:\Users\Gabriela\Documents\GitHub\blog-app\BlogApp\BlogApp.Dotnet.Web\Views\Posts\Details.cshtml"
                                                                              Write(TempData["ParentID"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(");\r\n            repliesSection.className += \" \" + \"show\";\r\n        }\r\n\r\n        elmnt.scrollIntoView(false);\r\n    }\r\n        </script>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<BlogApp.Dotnet.Web.ViewModels.BlogPostViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
