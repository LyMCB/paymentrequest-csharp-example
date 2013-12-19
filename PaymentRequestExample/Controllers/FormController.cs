using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaymentRequestExample.PaymentRequest;
using PaymentRequestExample.Misc;

namespace PaymentRequestExample.Controllers
{
    public class FormController : Controller
    {
        private static PaymentRequestClient client = new PaymentRequestClient();
        private static string controllername = "paymentrequest";

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
                authentication = PaymentRequestWrapper.GetAuthentication(),
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
                authentication = PaymentRequestWrapper.GetAuthentication(),
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
                authentication = PaymentRequestWrapper.GetAuthentication()
            });

            return View(listFormResponse);
        }

        public ActionResult DeleteForm(long formid)
        {
            PaymentRequest.deleteformresponse createFormResponse = client.deleteform(new PaymentRequest.deleteformrequest
            {
                authentication = PaymentRequestWrapper.GetAuthentication(),
                form = new PaymentRequest.form2
                {
                    formid = formid
                }
            });

            return RedirectToAction("Delete");
        }

        public ActionResult List(long? startkey, int page = 1)
        {
            string cachename = Request.UserHostAddress + controllername + page;
            ViewBag.ShowBackKey = false;
            ViewBag.CurrentPage = page;

            var response = (PaymentRequest.listformresponse)CacheHandler.Get(cachename);

            if (response == null)
            {
                response = client.listform(new PaymentRequest.listformrequest
                {
                    authentication = PaymentRequestWrapper.GetAuthentication()
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

        public ActionResult RefreshList()
        {
            CacheHandler.RefreshList(Request.UserHostAddress, controllername);
            return RedirectToAction("List");
        }

    }
}
