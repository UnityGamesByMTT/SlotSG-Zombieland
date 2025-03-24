using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLLogger : MonoBehaviour
{
  [DllImport("__Internal")]
  private static extern void SendLogToReactNative(string message);
  void OnEnable()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    Application.logMessageReceived += HandleLog;
#endif
  }

  void OnDisable()
  {
#if UNITY_WEBGL && !UNITY_EDITOR
    Application.logMessageReceived -= HandleLog;
#endif
  }

#if UNITY_WEBGL && !UNITY_EDITOR
  void HandleLog(string logString, string stackTrace, LogType type)
  {
    string formattedMessage = $"[{type}] {logString}";
    // Debug.Log("Handle Log: " + formattedMessage);
    try
    {
      // Application.ExternalEval(@"
      //         if(window.ReactNativeWebView){
      //           window.ReactNativeWebView.postMessage('Trying to send log data.');
      //         }
      //       ");
      SendLogToReactNative(formattedMessage);
    }
    catch (System.Exception e)
    {
      Debug.LogWarning("Could not send log to React Native: " + e.Message);
      // Application.ExternalEval(@"
      //         if(window.ReactNativeWebView){
      //           window.ReactNativeWebView.postMessage('Log Catch Block');
      //         }
      //       ");
    }
  }
#endif
}

