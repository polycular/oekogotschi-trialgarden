using System;
using System.Collections.Generic;
using System.Text;

using GamAR.Networking;


namespace GamAR.Networking.Responses
{
    public class CheckoutResponse : Response
    {
        public CheckoutResponse(JSONNode json) : base(json)
        {

        }

        protected override bool processJson(JSONNode json)
        {
            return true;
        }
    }

}
