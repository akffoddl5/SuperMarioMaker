//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MapBuild : MonoBehaviour
//{
//    TilemapManager tilemapManager = new TilemapManager();

//    ScriptableMapInfo mapInfo;

//    [Header("Background")]
//    [SerializeField] Sprite[] backgroundSprite;
//    [SerializeField] Sprite[] backgroundSkySprite;
//    [SerializeField] SpriteRenderer[] background_ground;
//    [SerializeField] SpriteRenderer[] background_sky;

//    [Header("Object")]
//    [SerializeField] string[] objName;
//    [SerializeField] GameObject[] objPrefab;

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void MakeMap()
//    {
//        tilemapManager.LoadMap(0, out mapInfo);

//        //��� ����
//        for (int i = 0; i < background_ground.Length; i++)
//        {
//            background_ground[i].sprite = backgroundSprite[mapInfo.backgroundNum];
//        }
//        for (int i = 0; i < background_sky.Length; i++)
//        {
//            background_sky[i].sprite = backgroundSkySprite[mapInfo.backgroundNum];
//        }

//        //Ÿ�̸� ����

//        //�÷��̾� ������ġ ����

//        //�� ũ�� ����


//    }
//}
