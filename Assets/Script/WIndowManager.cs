using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WIndowManager : MonoBehaviour
{

    public static WIndowManager instance;

    public int mapNum = 0;
    public string nickName;
    public string winnerNickName;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        //RenderTexture renderTexture = GetComponent<Camera>().targetTexture;
        //Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        //RenderTexture.active = renderTexture;
        //texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        //texture.Apply();

        //File.WriteAllBytes($"{Application.dataPath}/{screenShotName}.png", texture.EncodeToPNG());
    }

    

    IEnumerator TakeScreenShot()
    {
        Debug.Log("shot");
        yield return new WaitForEndOfFrame();
        string screenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);


        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        Debug.Log(Application.dataPath);


        RenderTexture rt;
        
        //pc

        //Application.dataPath는 해당 프로젝트 Assets 폴더.

        //해당 경로에 NewDirectory라는 이름을 가진 폴더 생성

        Directory.CreateDirectory(Application.dataPath + "/../ScreenShot");


        File.WriteAllBytes($"{Application.dataPath}/../ScreenShot/{screenShotName}.png", tex.EncodeToPNG());

        Debug.Log("shot end");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            StartCoroutine(TakeScreenShot());
        }
    }
}
