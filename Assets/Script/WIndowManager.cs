using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WIndowManager : MonoBehaviour
{

    public static WIndowManager instance;


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

    string screenShotName = "test88";
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


        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);


        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        Debug.Log(Application.dataPath);
        //File.WriteAllBytes($"C:\\Users\\kim\\Documents\\red/{screenShotName}.png", tex.EncodeToPNG());
        //File.WriteAllBytes($"C:\\Users\\kim\\Documents\\red/{screenShotName}.png", tex.EncodeToPNG());
        File.WriteAllBytes($"{Application.dataPath}/../{screenShotName}.png", tex.EncodeToPNG());

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
