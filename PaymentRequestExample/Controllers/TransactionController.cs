using PaymentRequestExample.Misc;
using PaymentRequestExample.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentRequestExample.Controllers
{
    public class TransactionController : Controller
    {
        private static PaymentRequestClient client = new PaymentRequestClient();
        private static string controllername = "transaction";

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(long paymentRequestId, long transactionid)
        {
            PaymentRequest.addtransactionresponse addPaymentRequestTransactionResponse = client.addtransaction(new PaymentRequest.addtransactionrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                paymentrequest = new PaymentRequest.paymentrequest2
                {
                    paymentrequestid = paymentRequestId
                },
                transaction = new PaymentRequest.transaction
                {
                    transactionid = transactionid
                }
            });

            if (!addPaymentRequestTransactionResponse.result)
            {
                ViewBag.Message = addPaymentRequestTransactionResponse.message;
            }

            return View();
        }


        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult List(long paymentRequestId, long? startkey, int page = 1)
        {
            string cachename = Request.UserHostAddress + controllername + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = (PaymentRequest.listtransactionresponse)CacheHandler.Get(cachename);

            if (response == null)
            {
                response = client.listtransaction(new PaymentRequest.listtransactionrequest
                {
                    authentication = PaymentRequestWrapper.GetAuthentication(),
                    paging = startkey.HasValue ? new paging2 { exclusivestartkey = startkey.Value } : null,

                    paymentrequest = new PaymentRequest.paymentrequest4
                    {
                        paymentrequestid = paymentRequestId

                    }
                });

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


        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(long paymentRequestId, long transactionid)
        {
            PaymentRequest.gettransactionresponse getPaymentRequestTransactionResponse = client.gettransaction(new PaymentRequest.gettransactionrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),

                paymentrequest = new PaymentRequest.paymentrequest7
                {
                    paymentrequestid = paymentRequestId
                },
                transaction = new PaymentRequest.transaction1
                {
                    transactionid = transactionid
                }
            });

            if (!getPaymentRequestTransactionResponse.result)
            {
                ViewBag.Message = getPaymentRequestTransactionResponse.message;
            }


            return View(getPaymentRequestTransactionResponse);
        }



    }
}
