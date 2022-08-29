using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using NativeGalleryNamespace;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] private GameObject uiObj;
    [SerializeField] private GameObject blinkObj;
    [SerializeField] private Image captureScreen;
    [SerializeField] private bool isCoroutinePlay;
    [SerializeField] private string albumName = "";
    [SerializeField] private Texture2D tmpTexture = null;

    #if UNITY_ANDROID
    private static AndroidJavaClass ajc = new AndroidJavaClass("com.yasirkula.unity.NativeGallery");
    #endif
    
    public void ClickScreenShot()
    {
        if(!isCoroutinePlay)
        {
            StartCoroutine(CaptureScreen());
        }
    }

    private IEnumerator CaptureScreen()
    {
        isCoroutinePlay = true;

        uiObj.SetActive(false);
        Debug.Log("UI ��Ȱ��ȭ");

        yield return new WaitForEndOfFrame();

        CaptureAndSave();
        Debug.Log("ĸ�� �� ����");

        yield return new WaitForEndOfFrame();

        blinkObj.SetActive(true);

        Debug.Log("ĸ�� ȿ�� Ȱ��ȭ");

        yield return new WaitForEndOfFrame();

        blinkObj.SetActive(false);
        uiObj.SetActive(true);

        Debug.Log("ĸ�� ȿ�� ��Ȱ��ȭ �� UI Ȱ��ȭ");

        yield return new WaitForSecondsRealtime(0.3f);

        captureScreen.gameObject.SetActive(true);
        captureScreen.sprite = Sprite.Create(tmpTexture, new Rect(0,0,Screen.width,Screen.height), new Vector2(0.5f, 0.5f));
        Debug.Log("ĸ�� �̹��� Ȱ��ȭ");

        isCoroutinePlay = false;
    }

    private void CaptureAndSave()
    {
        tmpTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        

        tmpTexture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
        tmpTexture.Apply();

         Debug.Log("" + NativeGallery.SaveImageToGallery(tmpTexture, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
    }

    public void ClosePreview()
    {
        tmpTexture = null;
        captureScreen.gameObject.SetActive(false);
    }
}
