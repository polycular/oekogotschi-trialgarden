using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GamAR.Networking
{
    public class Request
    {
        private List<string> entries;
        public string Path { get; private set; }

        public Request(string path)
        {
            entries = new List<string>();
            Path = path;
        }

        public void AddString(string name, string value)
        {
            entries.Add("\"" + name + "\":" + "\"" + value + "\"");
        }

        public string ToJSONString()
        {
            string json = "{";
            json += string.Join(",", entries.ToArray());
            json += "}";
            return json;
        }
    }

}
