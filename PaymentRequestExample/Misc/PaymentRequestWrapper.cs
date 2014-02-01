using System;
using System.Linq;
using PaymentRequestExample.PaymentRequest;

namespace PaymentRequestExample.Misc
{
    public static class PaymentRequestWrapper
    {
        private static readonly string merchantNumber = System.Configuration.ConfigurationManager.AppSettings["merchantNumber"];
        private static readonly string password = System.Configuration.ConfigurationManager.AppSettings["password"];

        public static authentication Authentication
        {
            get
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
}