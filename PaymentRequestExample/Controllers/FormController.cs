using System;
using System.Linq;
using System.Web.Mvc;
using PaymentRequestExample.PaymentRequest;
using PaymentRequestExample.Misc;

namespace PaymentRequestExample.Controllers
{
    public class FormController : Controller
    {
        private static readonly PaymentRequestClient client = new PaymentRequestClient();
        private static readonly string controllerName = "form";

        public ActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(long formid)
        {
            PaymentRequest.PaymentRequestClient client = new PaymentRequest.PaymentRequestClient();

            PaymentRequest.getformresponse getFormResponse = client.getform(new PaymentRequest.getformrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                form = new PaymentRequest.form
                {
                    formid = formid
                }
            });

            return View(getFormResponse);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string formname)
        {
            PaymentRequest.inputlist1 inputList = new PaymentRequest.inputlist1();
            inputList.Add(new PaymentRequest.input1
            {
                type = PaymentRequest.type.text,
                name = "Firstname"
            });
            inputList.Add(new PaymentRequest.input1
            {
                type = PaymentRequest.type.text,
                name = "Lastname"
            });

            PaymentRequest.optionlist optionList = new PaymentRequest.optionlist();
            optionList.Add(new PaymentRequest.option
            {
                value = "Male"
            });
            optionList.Add(new PaymentRequest.option
            {
                value = "Female"
            });

            inputList.Add(new PaymentRequest.input1
            {
                type = PaymentRequest.type.select,
                name = "Gender",
                optionlist = optionList
            });

            PaymentRequest.createformresponse createFormResponse = client.createform(new PaymentRequest.createformrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                form = new PaymentRequest.form1
                {
                    name = formname + "-" + DateTime.UtcNow,
                    termsurl = "http://www.epay.dk",
                    inputlist = inputList
                }
            });

            if (createFormResponse.result)
            {
                return RedirectToAction("list");
            }

            return View();
        }

        public ActionResult Delete()
        {
            PaymentRequest.listformresponse listFormResponse = client.listform(new PaymentRequest.listformrequest
            {
                authentication = PaymentRequestWrapper.Authentication
            });

            return View(listFormResponse);
        }

        public ActionResult DeleteForm(long formid)
        {
            client.deleteform(new PaymentRequest.deleteformrequest
            {
                authentication = PaymentRequestWrapper.Authentication,
                form = new PaymentRequest.form2
                {
                    formid = formid
                }
            });

            return RedirectToAction("Delete");
        }

        public ActionResult List(long? startkey, int page = 1)
        {
            string cacheName = Session.SessionID + controllerName + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = CacheHandler.Get(cacheName) as PaymentRequest.listformresponse;

            if (response == null)
            {
                response = client.listform(new PaymentRequest.listformrequest
                {
                    authentication = PaymentRequestWrapper.Authentication
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

        public ActionResult RefreshList()
        {
            CacheHandler.RefreshList(Session.SessionID, controllerName);
            return RedirectToAction("List");
        }
    }
}