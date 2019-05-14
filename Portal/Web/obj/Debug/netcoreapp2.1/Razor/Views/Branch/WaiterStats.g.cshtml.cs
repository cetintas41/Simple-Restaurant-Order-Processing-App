#pragma checksum "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "de10d26fa78c5215d4207ba48b17307ba4c77bb0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Branch_WaiterStats), @"mvc.1.0.view", @"/Views/Branch/WaiterStats.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Branch/WaiterStats.cshtml", typeof(AspNetCore.Views_Branch_WaiterStats))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de10d26fa78c5215d4207ba48b17307ba4c77bb0", @"/Views/Branch/WaiterStats.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"74b0619e1a302f0598271da1847e697c39d57b88", @"/Views/_ViewImports.cshtml")]
    public class Views_Branch_WaiterStats : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Web.Models.WaiterStatsViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(40, 336, true);
            WriteLiteral(@"<!-- Chart JS CDN-->
<script src=""https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.3/Chart.bundle.min.js""></script>

<canvas id=""myChart"" width=""400"" height=""175""></canvas>
<script>
    var ctx = document.getElementById(""myChart"");
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ");
            EndContext();
            BeginContext(377, 48, false);
#line 11 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
               Write(Html.Raw(Json.Serialize(@Model.TableStats.Keys)));

#line default
#line hidden
            EndContext();
            BeginContext(425, 164, true);
            WriteLiteral(",\r\n            datasets: [{\r\n                showLine: false, // disable for a single dataset\r\n                label: \' Masa sayısı (adet)\',\r\n                data: ");
            EndContext();
            BeginContext(590, 50, false);
#line 15 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                 Write(Html.Raw(Json.Serialize(@Model.TableStats.Values)));

#line default
#line hidden
            EndContext();
            BeginContext(640, 36, true);
            WriteLiteral(",\r\n                backgroundColor: ");
            EndContext();
            BeginContext(677, 44, false);
#line 16 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                            Write(Html.Raw(Json.Serialize(@Model.Colors.Keys)));

#line default
#line hidden
            EndContext();
            BeginContext(721, 32, true);
            WriteLiteral(",\r\n                borderColor: ");
            EndContext();
            BeginContext(754, 46, false);
#line 17 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                        Write(Html.Raw(Json.Serialize(@Model.Colors.Values)));

#line default
#line hidden
            EndContext();
            BeginContext(800, 381, true);
            WriteLiteral(@",
                borderWidth: 1
            }]
        },
        options: {
            //elements: {
              //  line: {
                //    tension: 0, // hatlar arasındaki geçişi keskinleştirir. - line chart için
                //}
            //},
            title: {
                display: true,
                text: 'Bakılan Masa Verileri (adet) - ");
            EndContext();
            BeginContext(1182, 16, false);
#line 29 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                                                 Write(Model.WaiterName);

#line default
#line hidden
            EndContext();
            BeginContext(1198, 641, true);
            WriteLiteral(@"'
            },
            tooltips: {
                mode: 'index',
                axis: 'y'
            },
            scales: {
                yAxes: [{
                    stacked: true,
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
</script>

<br /><hr />

<canvas id=""line-chart"" width=""400"" height=""175""></canvas>
<script>
    //http://tobiasahlin.com/blog/chartjs-charts-to-get-you-started/
    new Chart(document.getElementById(""line-chart""), {
        type: 'line',
        data: {
            labels: ");
            EndContext();
            BeginContext(1840, 49, false);
#line 55 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
               Write(Html.Raw(Json.Serialize(@Model.IncomeStats.Keys)));

#line default
#line hidden
            EndContext();
            BeginContext(1889, 51, true);
            WriteLiteral(",\r\n            datasets: [{\r\n                data: ");
            EndContext();
            BeginContext(1941, 51, false);
#line 57 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                 Write(Html.Raw(Json.Serialize(@Model.IncomeStats.Values)));

#line default
#line hidden
            EndContext();
            BeginContext(1992, 72, true);
            WriteLiteral(",\r\n                label: \" Kazanç (TL)\",\r\n                borderColor: ");
            EndContext();
            BeginContext(2065, 59, false);
#line 59 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                        Write(Html.Raw(Json.Serialize(@Model.Colors.Values.ElementAt(0))));

#line default
#line hidden
            EndContext();
            BeginContext(2124, 182, true);
            WriteLiteral(",\r\n                fill: true\r\n            }]\r\n        },\r\n        options: {\r\n            title: {\r\n                display: true,\r\n                text: \'Kazandırılan Gelir (TL) - ");
            EndContext();
            BeginContext(2307, 16, false);
#line 66 "C:\Users\OCETINTAS\Desktop\Portal\Web\Views\Branch\WaiterStats.cshtml"
                                            Write(Model.WaiterName);

#line default
#line hidden
            EndContext();
            BeginContext(2323, 47, true);
            WriteLiteral("\'\r\n            }\r\n        }\r\n    });\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Web.Models.WaiterStatsViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
