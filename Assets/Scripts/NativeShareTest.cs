using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeShareTest : MonoBehaviour
{
    public Texture2D shareImg;
    public void ClickShare()
    {
        new NativeShare().AddFile(shareImg)
		.SetSubject( "TES TEST" ).SetText( "Hello world!" ).SetUrl( "https://www.naver.com/" )
		.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
		.Share();
    }
}