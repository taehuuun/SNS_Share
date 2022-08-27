using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public void ClickScreenShot()
    {
        ScreenCapture.CaptureScreenshot("TEST!!");
    }
}
