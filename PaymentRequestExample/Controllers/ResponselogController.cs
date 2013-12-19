using PaymentRequestExample.Misc;
using PaymentRequestExample.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PaymentRequestExample.Controllers
{
    public class ResponselogController : Controller
    {
        private static PaymentRequestClient client = new PaymentRequestClient();
        private static string controllername = "responselog";

        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(long requestresponselogid)
        {
            PaymentRequest.getrequestresponselogresponse response = client.getrequestresponselog(new PaymentRequest.getrequestresponselogrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                requestresponselog = new requestresponselog { requestresponselogid = requestresponselogid }
            });

            return View(response);
        }

        public ActionResult List(long? startkey, int page = 1)
        {
            string cachename = Request.UserHostAddress + controllername + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = (PaymentRequest.listrequestresponselogresponse)CacheHandler.Get(cachename);

            if (response == null)
            {
                var request = new PaymentRequest.listrequestresponselogrequest
                {
                    authentication = PaymentRequestWrapper.GetAuthentication(),
                    paging = startkey.HasValue ? new paging { exclusivestartkey = startkey.Value } : null
                };
                response = client.listrequestresponselog(request);

                if (response.result)
                {
                    CacheHandler.AddToCache(cachename, response);
                }
            }

            if (page > 1)
            {
                ViewBag.ShowBackKey = true;
            }

            return View(response);
        }

        public ActionResult RefreshList()
        {
            CacheHandler.RefreshList(Request.UserHostAddress, controllername);
            return RedirectToAction("List");
        }

    }
}
