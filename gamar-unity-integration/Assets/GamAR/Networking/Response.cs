using System;
using System.Collections.Generic;
using System.Text;

namespace GamAR.Networking
{
    public abstract class Response
    {
        public string ErrorText = "";
        public int ErrorId = 0;
        public bool Ready { get; private set; }

        public Response(JSONNode json)
        {
            Ready = json != null && json["success"].AsBool && processJson(json);
        }

        protected abstract bool processJson(JSONNode json);
    }
}
