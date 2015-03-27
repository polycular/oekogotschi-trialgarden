using UnityEngine;
using System.Collections;

namespace ToolbAR.Math
{
    public class Time
    {
        static public string convertSecondsToHHMM(float seconds)
        {
            return ((int)(seconds / 60)).ToString("D2") + ":" + ((int)(seconds % 60)).ToString("D2");
        }
    }
}