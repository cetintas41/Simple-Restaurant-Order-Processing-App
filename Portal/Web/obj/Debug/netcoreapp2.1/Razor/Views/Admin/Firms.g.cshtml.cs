#pragma checksum "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "93ebe8d898617b51fc00218c288cbef3a38d2cf8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Firms), @"mvc.1.0.view", @"/Views/Admin/Firms.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Admin/Firms.cshtml", typeof(AspNetCore.Views_Admin_Firms))]
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
#line 1 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\_ViewImports.cshtml"
using Web;

#line default
#line hidden
#line 2 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\_ViewImports.cshtml"
using Web.Models;

#line default
#line hidden
#line 2 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"93ebe8d898617b51fc00218c288cbef3a38d2cf8", @"/Views/Admin/Firms.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"74b0619e1a302f0598271da1847e697c39d57b88", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Firms : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Models.AdminFirmsViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("demo-form2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-horizontal form-label-left"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("novalidate", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(119, 25, true);
            WriteLiteral("<div class=\"row\">\r\n\r\n    ");
            EndContext();
            BeginContext(145, 169, false);
#line 6 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
Write(Html.Partial("~/Views/Shared/_PaginationFormPartial.cshtml", new Web.Models.PaginationPartialModel { Pagination = Model.Pagination, Url = Url.Action("Firms", "Admin") }));

#line default
#line hidden
            EndContext();
            BeginContext(314, 181, true);
            WriteLiteral("\r\n\r\n    <div class=\"col-lg-12 col-md-12 col-sm-12 col-xs-12\">\r\n        <div class=\"x_panel\">\r\n            <div class=\"x_title\">\r\n                <h2><i class=\"fa fa-briefcase\"></i> ");
            EndContext();
            BeginContext(496, 22, false);
#line 11 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                               Write(Localizer["FirmUsers"]);

#line default
#line hidden
            EndContext();
            BeginContext(518, 8, true);
            WriteLiteral(" <small>");
            EndContext();
            BeginContext(527, 31, false);
#line 11 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                                              Write(DateTime.Now.ToLongDateString());

#line default
#line hidden
            EndContext();
            BeginContext(558, 307, true);
            WriteLiteral(@"</small></h2>
                <div class=""nav navbar-right panel_toolbox"">
                    <button data-toggle=""modal""
                            data-target=""#modal-create-firm""
                            class=""btn btn-sm btn-primary"">
                        <i class=""fa fa-plus-circle""></i> ");
            EndContext();
            BeginContext(866, 19, false);
#line 16 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                     Write(Localizer["Insert"]);

#line default
#line hidden
            EndContext();
            BeginContext(885, 494, true);
            WriteLiteral(@"
                    </button>
                </div>

                <div class=""clearfix""></div>
            </div>
            <div class=""x_content"">
                <div id=""datatable-responsive_wrapper"" class=""dataTables_wrapper form-inline dt-bootstrap no-footer"">
                    <div class=""row"">
                        <div class=""col-sm-6"">
                            <div class=""dataTables_length"" id=""datatable-responsive_length"">
                                ");
            EndContext();
            BeginContext(1380, 82, false);
#line 27 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                           Write(Html.Partial("~/Views/Shared/_ShownDataCountPartial.cshtml", Model.Pagination.Top));

#line default
#line hidden
            EndContext();
            BeginContext(1462, 300, true);
            WriteLiteral(@"
                            </div>
                        </div>
                        <div class=""mb-3"">
                            <div class=""col-sm-6"">
                                <div id=""datatable-responsive_filter"" class=""dataTables_filter"">
                                    ");
            EndContext();
            BeginContext(1763, 85, false);
#line 33 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                               Write(Html.Partial("~/Views/Shared/_SearchAreaPartial.cshtml", Localizer["FirmName"].Value));

#line default
#line hidden
            EndContext();
            BeginContext(1848, 625, true);
            WriteLiteral(@"
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class=""row"">
                        <div class=""col-sm-12"">
                            <table id=""datatable-responsive"" class=""table table-striped table-bordered dt-responsive nowrap dataTable no-footer dtr-inline"" cellspacing=""0"" width=""100%"" role=""grid"" aria-describedby=""datatable-responsive_info"" style=""width: 100%;"">
                                <thead>
                                    <tr role=""row"">
                                        ");
            EndContext();
            BeginContext(2474, 200, false);
#line 43 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                   Write(Html.PartialAsync("~/Views/Shared/_SortPartial.cshtml", new Web.Models.SortPartialModel { Action = "Firms", Controller = "Admin", ColumnName = Localizer["Firm"].Value, Pagination = Model.Pagination }));

#line default
#line hidden
            EndContext();
            BeginContext(2674, 66, true);
            WriteLiteral("\r\n                                        <th class=\"text-center\">");
            EndContext();
            BeginContext(2741, 18, false);
#line 44 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                           Write(Localizer["Email"]);

#line default
#line hidden
            EndContext();
            BeginContext(2759, 71, true);
            WriteLiteral("</th>\r\n                                        <th class=\"text-center\">");
            EndContext();
            BeginContext(2831, 18, false);
#line 45 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                           Write(Localizer["Phone"]);

#line default
#line hidden
            EndContext();
            BeginContext(2849, 71, true);
            WriteLiteral("</th>\r\n                                        <th class=\"text-center\">");
            EndContext();
            BeginContext(2921, 19, false);
#line 46 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                           Write(Localizer["Access"]);

#line default
#line hidden
            EndContext();
            BeginContext(2940, 71, true);
            WriteLiteral("</th>\r\n                                        <th class=\"text-center\">");
            EndContext();
            BeginContext(3012, 19, false);
#line 47 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                           Write(Localizer["Delete"]);

#line default
#line hidden
            EndContext();
            BeginContext(3031, 171, true);
            WriteLiteral("</th>\r\n\r\n                                    </tr>\r\n                                </thead>\r\n                                <tbody>\r\n                                    ");
            EndContext();
            BeginContext(3203, 97, false);
#line 52 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                               Write(Html.Partial("~/Views/Shared/_NoDataFoundPartial.cshtml", new int[] { Model.FirmUsers.Count, 5 }));

#line default
#line hidden
            EndContext();
            BeginContext(3300, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
#line 54 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                       int k = 0;

#line default
#line hidden
            BeginContext(3356, 36, true);
            WriteLiteral("                                    ");
            EndContext();
#line 55 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                     foreach (var firm in Model.FirmUsers)
                                    {

#line default
#line hidden
            BeginContext(3471, 43, true);
            WriteLiteral("                                        <tr");
            EndContext();
            BeginWriteAttribute("id", " id=\"", 3514, "\"", 3525, 2);
            WriteAttributeValue("", 3519, "row_", 3519, 4, true);
#line 57 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
WriteAttributeValue("", 3523, k, 3523, 2, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3526, 11, true);
            WriteLiteral(" role=\"row\"");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 3537, "\"", 3587, 2);
#line 57 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
WriteAttributeValue("", 3545, k % 2 == 0 ? "even" : "odd", 3545, 30, false);

#line default
#line hidden
            WriteAttributeValue(" ", 3575, "text-center", 3576, 12, true);
            EndWriteAttribute();
            BeginContext(3588, 53, true);
            WriteLiteral(">\r\n\r\n                                            <td>");
            EndContext();
            BeginContext(3642, 9, false);
#line 59 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                           Write(firm.Name);

#line default
#line hidden
            EndContext();
            BeginContext(3651, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(3707, 10, false);
#line 60 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                           Write(firm.Email);

#line default
#line hidden
            EndContext();
            BeginContext(3717, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(3773, 10, false);
#line 61 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                           Write(firm.Phone);

#line default
#line hidden
            EndContext();
            BeginContext(3783, 259, true);
            WriteLiteral(@"</td>
                                            <td>
                                                <div class=""checkbox"">
                                                    <label class="""">
                                                        <div");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 4042, "\"", 4076, 3);
            WriteAttributeValue("", 4052, "changeAccess(\'", 4052, 14, true);
#line 65 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
WriteAttributeValue("", 4066, firm.Id, 4066, 8, false);

#line default
#line hidden
            WriteAttributeValue("", 4074, "\')", 4074, 2, true);
            EndWriteAttribute();
            BeginContext(4077, 464, true);
            WriteLiteral(@"
                                                             class=""icheckbox_flat-green checked""
                                                             style=""position: relative;"">
                                                            <input type=""checkbox""
                                                                   class=""flat""
                                                                   style=""position: absolute; opacity: 0;"" ");
            EndContext();
            BeginContext(4543, 30, false);
#line 70 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                                                                       Write(firm.IsActive ? "checked" : "");

#line default
#line hidden
            EndContext();
            BeginContext(4574, 921, true);
            WriteLiteral(@">
                                                            <ins class=""iCheck-helper""
                                                                 style=""position: absolute; top: 0%; left: 0%; display: block;
                                                            width: 100%; height: 100%; margin: 0px;
                                                            padding: 0px; background: rgb(255, 255, 255);
                                                            border: 0px; opacity: 0;"">
                                                            </ins>
                                                        </div>
                                                    </label>
                                                </div>
                                            </td>
                                            <td>
                                                <button");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 5495, "\"", 5560, 6);
            WriteAttributeValue("", 5505, "$(\'#delete-id\').val(\'", 5505, 21, true);
#line 82 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
WriteAttributeValue("", 5526, firm.Id, 5526, 8, false);

#line default
#line hidden
            WriteAttributeValue("", 5534, "\');", 5534, 3, true);
            WriteAttributeValue(" ", 5537, "$(\'#row-id\').val(\'", 5538, 19, true);
#line 82 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
WriteAttributeValue("", 5556, k, 5556, 2, false);

#line default
#line hidden
            WriteAttributeValue("", 5558, "\')", 5558, 2, true);
            EndWriteAttribute();
            BeginContext(5561, 297, true);
            WriteLiteral(@"
                                                        data-toggle=""modal"" data-target=""#modal-delete-confirm""
                                                        type=""button"" class=""btn btn-danger btn-xs"">
                                                    <i class=""fa fa-trash""></i> ");
            EndContext();
            BeginContext(5859, 19, false);
#line 85 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                                           Write(Localizer["Delete"]);

#line default
#line hidden
            EndContext();
            BeginContext(5878, 159, true);
            WriteLiteral("\r\n                                                </button>\r\n                                            </td>\r\n                                        </tr>\r\n");
            EndContext();
#line 89 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                        k++;
                                    }

#line default
#line hidden
            BeginContext(6122, 215, true);
            WriteLiteral("                                </tbody>\r\n                            </table>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"row text-center\">\r\n                        ");
            EndContext();
            BeginContext(6338, 74, false);
#line 96 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                   Write(Html.Partial("~/Views/Shared/_PaginationPartial.cshtml", Model.Pagination));

#line default
#line hidden
            EndContext();
            BeginContext(6412, 566, true);
            WriteLiteral(@"
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


<!-- Modals -->
<div class=""modal fade"" id=""modal-create-firm"" role=""dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-body"">
                <div class=""row"">
                    <div class=""col-md-12 col-sm-12 col-xs-12"">
                        <div class=""x_panel"">
                            <div class=""x_title"">
                                <h2><i class=""fa fa-briefcase""></i>");
            EndContext();
            BeginContext(6979, 23, false);
#line 114 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                              Write(Localizer["CreateFirm"]);

#line default
#line hidden
            EndContext();
            BeginContext(7002, 228, true);
            WriteLiteral("</h2>\r\n                                <div class=\"clearfix\"></div>\r\n                            </div>\r\n                            <div class=\"x_content\">\r\n                                <br>\r\n                                ");
            EndContext();
            BeginContext(7230, 4915, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ef6c3e939db846a8a203403071917430", async() => {
                BeginContext(7331, 218, true);
                WriteLiteral("\r\n                                    <div class=\"form-group\">\r\n                                        <label class=\"control-label col-md-3 col-sm-3 col-xs-12\" for=\"logo\">\r\n                                            ");
                EndContext();
                BeginContext(7550, 21, false);
#line 122 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                       Write(Localizer["FirmLogo"]);

#line default
#line hidden
                EndContext();
                BeginContext(7571, 296, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <button type=""button"" id=""btn-logo"" class=""btn btn-sm btn-primary"">
                                                ");
                EndContext();
                BeginContext(7868, 23, false);
#line 126 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                           Write(Localizer["SelectLogo"]);

#line default
#line hidden
                EndContext();
                BeginContext(7891, 477, true);
                WriteLiteral(@"
                                            </button>
                                        </div>
                                        <input type=""file"" style=""display:none;"" name=""logo"" id=""logo"" />
                                    </div>
                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"" for=""firm-name"">
                                            ");
                EndContext();
                BeginContext(8369, 21, false);
#line 133 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                       Write(Localizer["FirmName"]);

#line default
#line hidden
                EndContext();
                BeginContext(8390, 632, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <input type=""text"" id=""firm-name"" required=""required"" class=""form-control col-md-7 col-xs-12"">
                                        </div>
                                    </div>
                                    <hr />
                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"" for=""firm-email"">
                                           ");
                EndContext();
                BeginContext(9023, 22, false);
#line 142 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                      Write(Localizer["FirmEmail"]);

#line default
#line hidden
                EndContext();
                BeginContext(9045, 589, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <input type=""email"" id=""firm-email"" required=""required"" class=""form-control col-md-7 col-xs-12"">
                                        </div>
                                    </div>
                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"" for=""firm-tel"">
                                            ");
                EndContext();
                BeginContext(9635, 22, false);
#line 150 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                       Write(Localizer["FirmPhone"]);

#line default
#line hidden
                EndContext();
                BeginContext(9657, 633, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <input type=""tel"" id=""firm-tel"" required=""required"" class=""form-control col-md-7 col-xs-12"" data-inputmask=""'mask' : '(999) 999-99-99'"">
                                        </div>
                                    </div>
                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"" for=""firm-password"">
                                           ");
                EndContext();
                BeginContext(10291, 25, false);
#line 158 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                      Write(Localizer["FirmPassword"]);

#line default
#line hidden
                EndContext();
                BeginContext(10316, 608, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <input type=""password"" id=""firm-password"" required=""required"" class=""form-control col-md-7 col-xs-12"">
                                        </div>
                                    </div>

                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"" for=""firm-password-confirm"">
                                          ");
                EndContext();
                BeginContext(10925, 30, false);
#line 167 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                     Write(Localizer["FirmPasswordAgain"]);

#line default
#line hidden
                EndContext();
                BeginContext(10955, 860, true);
                WriteLiteral(@"
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <input type=""password"" id=""firm-password-confirm"" required=""required"" class=""form-control col-md-7 col-xs-12"">
                                        </div>
                                    </div>
                                    <br />
                                    <div class=""form-group"">
                                        <label class=""control-label col-md-3 col-sm-3 col-xs-12"">
                                        </label>
                                        <div class=""col-md-6 col-sm-6 col-xs-12"">
                                            <button type=""button"" class=""btn btn-default"" data-dismiss=""modal""><i class=""fa fa-close""></i> ");
                EndContext();
                BeginContext(11816, 18, false);
#line 178 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                                                                                                      Write(Localizer["Close"]);

#line default
#line hidden
                EndContext();
                BeginContext(11834, 151, true);
                WriteLiteral("</button>\r\n                                            <button onclick=\"createFirm()\" type=\"button\" class=\"btn btn-primary\"><i class=\"fa fa-save\"></i> ");
                EndContext();
                BeginContext(11986, 17, false);
#line 179 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
                                                                                                                                       Write(Localizer["Save"]);

#line default
#line hidden
                EndContext();
                BeginContext(12003, 135, true);
                WriteLiteral("</button>\r\n                                        </div>\r\n                                    </div>\r\n                                ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("data-parsley-validate", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(12145, 180, true);
            WriteLiteral("\r\n                            </div>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
            EndContext();
            BeginContext(12326, 98, false);
#line 192 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
Write(Html.Partial("~/Views/Shared/_DeleteConfirmPartial.cshtml", Url.Action("DeleteFirmUser", "Admin")));

#line default
#line hidden
            EndContext();
            BeginContext(12424, 166, true);
            WriteLiteral("\r\n\r\n<script src=\"https://code.jquery.com/jquery-3.3.1.slim.min.js\" integrity=\"sha256-3edrmyuQ0w65f8gfBsqowzjJe2iM6n0nKciPUp8y+7E=\" crossorigin=\"anonymous\"></script>\r\n");
            EndContext();
            BeginContext(12590, 55, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "95b4a74a67964c36b3b7f6892dcb33cf", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 12603, "~/js/admin/", 12603, 11, true);
#line 195 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Admin\Firms.cshtml"
AddHtmlAttributeValue("", 12614, Localizer["JsPath"], 12614, 20, false);

#line default
#line hidden
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(12645, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IViewLocalizer Localizer { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Models.AdminFirmsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591