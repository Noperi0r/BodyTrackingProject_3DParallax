using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class ScreenSizeGetter
{
    // import dll
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    //const string PLUGIN_DLL = "GetDeviceCapsPlugin.dll";

    // import dll functions
    [DllImport("GetDeviceCapsPlugin.dll")]
    public static extern int GetScreenHeight();
    [DllImport("GetDeviceCapsPlugin.dll")]
    public static extern int GetScreenWidth();

#endif


    /*    void Start()
        {
            int width = GetScreenWidth();
            Debug.Log("Screen Width: " + width);
            int height = GetScreenHeight();
            Debug.Log("Screen Height: " + height);
        }*/
}
