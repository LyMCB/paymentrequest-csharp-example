using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaymentRequestExample.PaymentRequest
{
    public static class PaymentRequestWrapper
    {
        private static string merchantNumber = System.Configuration.ConfigurationManager.AppSettings["merchantNumber"];
        private static string password = System.Configuration.ConfigurationManager.AppSettings["password"];

        public static authentication GetAuthentication()
        {
            authentication auth = new PaymentRequest.authentication
                {
                    merchantnumber = merchantNumber,
                    password = password
                };

            return auth;
        }

    }
}