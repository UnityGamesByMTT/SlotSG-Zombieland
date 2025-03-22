#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLLogger : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void SendLogToReactNative(string message);

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string formattedMessage = $"[{type}] {logString}";
        try
        {
            SendLogToReactNative(formattedMessage);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Could not send log to React Native: " + e.Message);
        }
    }
}
#endif

