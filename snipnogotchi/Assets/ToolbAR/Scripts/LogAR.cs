using UnityEngine;
using System.Collections;

namespace ToolbAR
{
    static class LogAR
    {
        private static string mSeparator = " | Logged by: ";
        public static void log(object message, object classContext)
        {
            UnityEngine.Debug.Log(message.ToString() + mSeparator + classContext.ToString());
        }

        public static void log(object message, object classContext, Object context)
        {
            UnityEngine.Debug.Log(message.ToString() + mSeparator + classContext.ToString(), context);
        }

        public static void logError(object message, object classContext)
        {
            UnityEngine.Debug.LogError(message.ToString() + mSeparator + classContext.ToString());
        }

        public static void logError(object message, object classContext, Object context)
        {
            UnityEngine.Debug.LogError(message.ToString() + mSeparator + classContext.ToString(), context);
        }

        public static void logWarning(object message, object classContext)
        {
            UnityEngine.Debug.LogWarning(message.ToString() + mSeparator + classContext.ToString());
        }

        public static void logWarning(object message, object classContext, Object context)
        {
            UnityEngine.Debug.LogWarning(message.ToString() + mSeparator + classContext.ToString(), context);
        }
    }
}
