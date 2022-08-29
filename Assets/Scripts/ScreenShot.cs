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
    [SerializeField] private GameObject captureScreen;
    [SerializeField] private bool isCoroutinePlay;
    [SerializeField] private string albumName = "";

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

        yield return new WaitForEndOfFrame();

        CaptureAndSave();

        yield return new WaitForEndOfFrame();

        blinkObj.SetActive(true);

        yield return new WaitForEndOfFrame();

        uiObj.SetActive(true);

        yield return new WaitForSecondsRealtime(0.3f);

        ShowCaptureImg();
        isCoroutinePlay = false;
    }

    private void CaptureAndSave()
    {
        Texture2D capture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        capture.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
        capture.Apply();

         Debug.Log("" + NativeGallery.SaveImageToGallery(capture, albumName,
            "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
 
        // To avoid memory leaks.
        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(capture);
    }

    private void ShowCaptureImg()
    {
        string path = GetLastPicurePath();

        if(path == null)
            return;
        
        Texture2D texture = GetScreenShot(path);
        Sprite sp = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0.5f));
        captureScreen.SetActive(true);
        captureScreen.GetComponent<Image>().sprite = sp;
    }

    private Texture2D GetScreenShot(string path)
    {
        Texture2D texture = null;

        byte[] fileByte;
        if (File.Exists(path))
        {
            fileByte = File.ReadAllBytes(path);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(fileByte);
        }

        return texture;
    }

    private string GetLastPicurePath()
    {
        string path;
        #if !UNITY_EDITOR && UNITY_ANDROID
        path = ajc.CallStatic<string>("GetMediaPath", "Shine Bright");
        #else
        path = Application.persistentDataPath;
        #endif

        string[] files = Directory.GetFiles(path,"*.png");

        if(files.Length >0)
            return files[files.Length-1];
        else
            return null;
    }
}
