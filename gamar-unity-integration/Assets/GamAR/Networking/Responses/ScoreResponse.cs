using System;
using System.Collections.Generic;
using System.Text;

using GamAR.Networking;


namespace GamAR.Networking.Responses
{
    public class ScoreResponse : Response
    {
        public string Rank { get; private set; }
        public string Time { get; private set; }

        public ScoreResponse(JSONNode json) : base(json)
        {

        }

        protected override bool processJson(JSONNode json)
        {
            Rank = json["rank"];
            Time = json["time"];

            return true;
        }
    }

}
