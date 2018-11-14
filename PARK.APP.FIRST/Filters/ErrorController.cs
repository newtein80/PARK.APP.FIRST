using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PARK.APP.FIRST.Filters
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        #region+ https://codedaze.io/global-error-handling-aspnet-core-mvc/
        //[Route("Error/500")]
        //public IActionResult Error500()
        //{
        //    var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        //    if (exceptionFeature != null)
        //    {
        //        ViewBag.ErrorMessage = exceptionFeature.Error.Message;
        //        ViewBag.RouteOfException = exceptionFeature.Path;
        //    }

        //    return View();
        //}

        //[Route("Error/{statusCode}")]
        //public IActionResult HandleErrorCode(int statusCode)
        //{
        //    var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        //    switch (statusCode)
        //    {
        //        case 404:
        //            ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
        //            ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //            break;
        //        case 500:
        //            ViewBag.ErrorMessage = "Sorry something went wrong on the server";
        //            ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //            break;
        //    }

        //    return View();
        //}
        #endregion

        [Route("500")]
        public IActionResult AppError()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                // Write the exception path to debug output
                System.Diagnostics.Debug.WriteLine(routeWhereExceptionOccurred);
            }
            return View();
        }

        [Route("404")]
        public IActionResult PageNotFound()
        {
            // Perform any action before serving the View
            return View();
        }
    }
}