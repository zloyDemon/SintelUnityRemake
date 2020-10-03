using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
   [MenuItem("SintelUtils/MakeScreenshot")]
   public static void MakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("sintel_screen.jpg");
    }

    [MenuItem("SintelUtils/Open Trello")]

    public static void OpenTrello()
    {
        string url = "https://trello.com/b/raXrmVSs/sintelunityremake";
        Application.OpenURL(url);
    }
}
