using PaymentRequestExample.Misc;
using PaymentRequestExample.PaymentRequest;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PaymentRequestExample.Controllers
{
    public class PaymentRequestController : Controller
    {
        private static readonly PaymentRequestClient client = new PaymentRequestClient();
        private static readonly string controllerName = "paymentrequest";

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string reference, string amount, string currency)
        {
            client.createpaymentrequest(new PaymentRequest.createpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                paymentrequest = new PaymentRequest.paymentrequest
                {
                    reference = reference,
                    exactclosedate = null,
                    closeafterxpayments = null,
                    parameters = new PaymentRequest.parameters
                    {
                        //possible parameters - http://tech.epay.dk/en/payment-window-parameters
                        amount = Convert.ToInt32(amount),
                        currency = currency,
                    }
                }
            });

            return RedirectToAction("List");
        }

        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(long paymentRequestId)
        {
            PaymentRequest.getpaymentrequestresponse getPaymentRequestResponse = client.getpaymentrequest(new PaymentRequest.getpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                paymentrequest = new PaymentRequest.paymentrequest4
                {
                    paymentrequestid = paymentRequestId
                }
            });

            return View(getPaymentRequestResponse);
        }

        public ActionResult Delete()
        {
            PaymentRequest.listpaymentrequestresponse listPaymentRequestResponse = client.listpaymentrequest(new PaymentRequest.listpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication
            });

            return View(listPaymentRequestResponse);
        }

        public ActionResult DeletePaymentRequest(long paymentRequestId)
        {
            client.deletepaymentrequest(new PaymentRequest.deletepaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                paymentrequest = new PaymentRequest.paymentrequest3
                {
                    paymentrequestid = paymentRequestId
                }
            });

            return RedirectToAction("Delete");
        }

        public ActionResult List(long? startkey, int page = 1)
        {
            string cacheName = Session.SessionID + controllerName + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = CacheHandler.Get(cacheName) as PaymentRequest.listpaymentrequestresponse;

            if (response == null)
            {
                response = client.listpaymentrequest(new PaymentRequest.listpaymentrequestrequest
                {
                    authentication = PaymentRequestWrapper.Authentication,
                    paging = startkey.HasValue ? new paging2 { exclusivestartkey = startkey.Value } : null
                });

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

        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(long paymentRequestId, string email)
        {
            client.sendpaymentrequest(new PaymentRequest.sendpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                paymentrequest = new PaymentRequest.paymentrequest5
                {
                    paymentrequestid = paymentRequestId
                },
                email = new email
                {
                    recipient = new recipient
                    {
                        emailaddress = email
                    }
                }
            });

            return View();
        }

        public ActionResult Close()
        {
            PaymentRequest.listpaymentrequestresponse listPaymentRequestResponse = client.listpaymentrequest(new PaymentRequest.listpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication
            });

            return View(listPaymentRequestResponse);
        }

        public ActionResult ClosePaymentRequest(long paymentRequestId)
        {
            client.closepaymentrequest(new PaymentRequest.closepaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                paymentrequest = new PaymentRequest.paymentrequest1
                {
                    paymentrequestid = paymentRequestId
                }
            });

            return RedirectToAction("Close");
        }

        public ActionResult RefreshList()
        {
            CacheHandler.RefreshList(Session.SessionID, controllerName);
            return RedirectToAction("List");
        }
    }
}