using PaymentRequestExample.Misc;
using PaymentRequestExample.PaymentRequest;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PaymentRequestExample.Controllers
{
    public class ResponseLogController : Controller
    {
        private static readonly PaymentRequestClient client = new PaymentRequestClient();
        private static readonly string controllerName = "responselog";

        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(long requestresponselogid)
        {
            PaymentRequest.getrequestresponselogresponse response = client.getrequestresponselog(new PaymentRequest.getrequestresponselogrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                requestresponselog = new requestresponselog { requestresponselogid = requestresponselogid }
            });

            return View(response);
        }

        public ActionResult List(long? startkey, int page = 1)
        {
            string cacheName = Session.SessionID + controllerName + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = CacheHandler.Get(cacheName) as PaymentRequest.listrequestresponselogresponse;

            if (response == null)
            {
                var request = new PaymentRequest.listrequestresponselogrequest
                {
                    authentication = PaymentRequestWrapper.Authentication,
                    paging = startkey.HasValue ? new paging1 { exclusivestartkey = startkey.Value } : null
                };
                response = client.listrequestresponselog(request);

                if (response.result)
                {
                    CacheHandler.AddToCache(cacheName, response);
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
            CacheHandler.RefreshList(Session.SessionID, controllerName);
            return RedirectToAction("List");
        }
    }
}