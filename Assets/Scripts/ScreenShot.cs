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
        Debug.Log("UI 비활성화");

        yield return new WaitForEndOfFrame();

        CaptureAndSave();
        Debug.Log("캡쳐 및 저장");

        yield return new WaitForEndOfFrame();

        blinkObj.SetActive(true);

        Debug.Log("캡쳐 효과 활성화");

        yield return new WaitForEndOfFrame();

        blinkObj.SetActive(false);
        uiObj.SetActive(true);

        Debug.Log("캡쳐 효과 비활성화 및 UI 활성화");

        yield return new WaitForSecondsRealtime(0.3f);

        captureScreen.gameObject.SetActive(true);
        captureScreen.sprite = Sprite.Create(tmpTexture, new Rect(0,0,Screen.width,Screen.height), new Vector2(0.5f, 0.5f));
        Debug.Log("캡쳐 이미지 활성화");

        isCoroutinePlay = false;
    }

    public void ClickShare()
    {
        new NativeShare().AddFile(tmpTexture)
		.SetSubject( "TES TEST" ).SetText( "Hello world!" ).SetUrl( "https://www.naver.com/" )
		.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
		.Share();
    }
    public void ClosePreview()
    {
        tmpTexture = null;
        captureScreen.gameObject.SetActive(false);
    }

    private void CaptureAndSave()
    {
        tmpTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        tmpTexture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
        tmpTexture.Apply();

         Debug.Log("" + NativeGallery.SaveImageToGallery(tmpTexture, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
    }
}
