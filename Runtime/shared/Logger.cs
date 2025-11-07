using System.Collections.Generic;
using LuneiSolei.BetterSplines.Adapters;

namespace LuneiSolei.BetterSplines.Shared
{
    public static class Logger
    {
        private static void Log(
            string message,
            LogLevel level = LogLevel.Debug,
            Dictionary<string, object> data = null,
            UnityEngine.Object context = null)
        {
            // Create the full message
            string msgPrefix = $"[{InternalConfig.PackageName}] [{level}]";
            string msg = $"{msgPrefix} {message}";

            // Attach any relevant data, if any
            if (data is { Count: > 0 })
            {
                msg += "\n    Data: " + FormatData(data);
            }

            switch (level)
            {
                case LogLevel.Debug:
                case LogLevel.Info:
                    UnityEngine.Debug.LogFormat(msg, context);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarningFormat(msg, context);
                    break;
            }
        }
        
        private static string FormatData(Dictionary<string, object> data)
        {
            List<string> dataPairs = new();
            foreach ((string key, object value) in data)
            {
                dataPairs.Add($"{key}={value ?? "null"}");
            }
            
            return string.Join(", ", dataPairs);
        }

        // Convenience Logging Methods
        internal static void Debug(
            string message,
            Dictionary<string, object> data = null,
            UnityEngine.Object context = null)
        {
            Log(message, LogLevel.Debug, data, context);
        }

        internal static void Warning(
            string message,
            Dictionary<string, object> data = null,
            UnityEngine.Object context = null)
        {
            Log(message, LogLevel.Warning, data, context);
        }

        internal static void Error(
            string message,
            Dictionary<string, object> data = null,
            UnityEngine.Object context = null)
        {
            Log(message, LogLevel.Error, data, context);
        }
        
        // Specific Message Methods
        internal static void NodePrefabIsNull(UnityEngine.Object context = null)
        {
            Warning(message: "Spline node prefab is null", context: context);
        }

        internal static void SplineNodeNotFound(UnityEngine.Object context = null)
        {
            Error(
                message: "SplineNode not found",
                data: new Dictionary<string, object>
                {
                    {"expected", typeof(SplineNode)},
                    {"actual", null}
                },
                context: context);
        }
    }
}