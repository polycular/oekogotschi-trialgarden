using System;
using System.Collections.Generic;
using System.Text;

using GamAR.Networking;


namespace GamAR.Networking.Responses
{
    public class CheckinResponse : Response
    {
        public string GameName { get; private set; }
        public int Tries { get; private set; }
        public string AssetsUrl { get; private set; }

        public CheckinResponse(JSONNode json) : base(json)
        {

        }

        protected override bool processJson(JSONNode json)
        {
            GameName = json["game"];
            Tries = int.Parse(json["tries"]);
            AssetsUrl = json["url"];

            return true;
        }
    }

}
