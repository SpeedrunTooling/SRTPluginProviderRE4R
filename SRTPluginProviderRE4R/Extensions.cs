using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace SRTPluginProducerRE4R
{
    public static class Extensions
    {
   //     public static async Task<string> RenderViewToStringAsync<T>(this Controller controller, string viewName, T model, bool isPartial = false)
   //     {
			//IServiceProvider serviceProvider = controller.ControllerContext.HttpContext.RequestServices;
   //         IRazorViewEngine? razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
   //         ITempDataProvider? tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

			//var view1 = razorViewEngine.GetView(null, $"~/Pages/Shared/{viewName}.cshtml", !isPartial); // WORKING!
   //         //var page1 = razorViewEngine.GetPage(null, $"~/Pages/Shared/{viewName}.cshtml"); // WORKING!
   //         IView? view = view1.View;

   //         if (view is null)
   //             throw new ArgumentNullException(nameof(viewName), $"{viewName} was not found");

   //         using (StringWriter sw = new StringWriter())
   //         {
			//	ViewDataDictionary viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
   //             ViewContext viewContext = new ViewContext(controller.ControllerContext, view!, viewDictionary, new TempDataDictionary(controller.ControllerContext.HttpContext, tempDataProvider), sw, new HtmlHelperOptions());
   //             await view!.RenderAsync(viewContext);
   //             return sw.ToString();
   //         }
   //     }
    }
}
