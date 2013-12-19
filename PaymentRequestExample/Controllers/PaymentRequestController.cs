using PaymentRequestExample.Misc;
using PaymentRequestExample.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaymentRequestExample.Controllers
{
    public class PaymentRequestController : Controller
    {
        private static PaymentRequestClient client = new PaymentRequestClient();
        private static string controllername = "paymentrequest";

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string reference, string amount, string currency)
        {
            PaymentRequest.createpaymentrequestresponse createPaymentRequestResponse = client.createpaymentrequest(new PaymentRequest.createpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
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
                authentication = PaymentRequestWrapper.GetAuthentication(),
                paymentrequest = new PaymentRequest.paymentrequest5
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
                authentication = PaymentRequestWrapper.GetAuthentication()
            });

            return View(listPaymentRequestResponse);
        }

        public ActionResult DeletePaymentRequest(long paymentRequestId)
        {
            PaymentRequest.deletepaymentrequestresponse deletePaymentRequestResponse = client.deletepaymentrequest(new PaymentRequest.deletepaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                paymentrequest = new PaymentRequest.paymentrequest3
                {
                    paymentrequestid = paymentRequestId
                }
            });

            return RedirectToAction("Delete");
        }


        public ActionResult List(long? startkey, int page = 1)
        {
            string cachename = Request.UserHostAddress + controllername + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = (PaymentRequest.listpaymentrequestresponse)CacheHandler.Get(cachename);

            if (response == null)
            {
                response = client.listpaymentrequest(new PaymentRequest.listpaymentrequestrequest
                {
                    authentication = PaymentRequestWrapper.GetAuthentication(),
                    paging = startkey.HasValue ? new paging1 { exclusivestartkey = startkey.Value } : null
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


        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(long paymentRequestId, string email)
        {
            PaymentRequest.sendpaymentrequestresponse sendPaymentRequestResponse = client.sendpaymentrequest(new PaymentRequest.sendpaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                paymentrequest = new PaymentRequest.paymentrequest6
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
                authentication = PaymentRequestWrapper.GetAuthentication()
            });

            return View(listPaymentRequestResponse);
        }

        public ActionResult ClosePaymentRequest(long paymentRequestId)
        {
            PaymentRequest.closepaymentrequestresponse closePaymentRequestResponse = client.closepaymentrequest(new PaymentRequest.closepaymentrequestrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                paymentrequest = new PaymentRequest.paymentrequest1
                {
                    paymentrequestid = paymentRequestId
                }
            });

            return RedirectToAction("Close");
        }

        public ActionResult RefreshList()
        {
            CacheHandler.RefreshList(Request.UserHostAddress, controllername);
            return RedirectToAction("List");
        }

    }
}
