using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScreenSizeGetter : MonoBehaviour
{
    // import dll
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    const string PLUGIN_DLL = "GetDeviceCapsPlugin.dll";
#endif

    // import dll functions
    [DllImport(PLUGIN_DLL)]
    private static extern int GetScreenHeight();
    [DllImport(PLUGIN_DLL)]
    private static extern int GetScreenWidth();

    void Start()
    {
        int width = GetScreenWidth();
        Debug.Log("Screen Width: " + width);
        int height = GetScreenHeight();
        Debug.Log("Screen Height: " + height);
    }
}
