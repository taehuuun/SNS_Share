using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeShareTest : MonoBehaviour
{
    public void ClickShare()
    {
        new NativeShare().AddFile("Assets/Test.png")
		.SetSubject( "TES TEST" ).SetText( "Hello world!" ).SetUrl( "https://www.naver.com/" )
		.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
		.Share();
    }
}