using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WIndowManager : MonoBehaviour
{

    [SerializeField] string screenShotName;
    // Start is called before the first frame update
    void Start()
    {
        RenderTexture renderTexture = GetComponent<Camera>().targetTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        File.WriteAllBytes($"{Application.dataPath}/{screenShotName}.png", texture.EncodeToPNG());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
