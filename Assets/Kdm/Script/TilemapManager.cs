using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
//using static ScriptableMapInfo;
//using static UnityEditor.PlayerSettings;

public class TilemapManager// : MonoBehaviour
{
    ////inspector ����
    [SerializeField] private Tilemap _gridMap, _tempMap, _setTilemap;
    [SerializeField] public int _levelIndex = 0;

    public int _backgroundNum;
    public float _timerCount;
    public int _playerLifePoint;
    public Vector3 _playerStartPos;
    public int _mapScaleNum;

    public static void SaveLevelFile(ScriptableLevel level)
    {

        ////newlevel�� asset���� �����ϱ�, ���ϸ��� ��������, ���ҽ��� �ʼ���
        //AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");
        ////asset ���� Ȯ��
        //AssetDatabase.SaveAssets();
        ////Unity Project�� �ֽ� ���·� ���� �� ������Ʈ ��ü�� ���� ������ üũ
        //AssetDatabase.Refresh();
    }

    public void JsonSave()
    {
        ScriptableMapInfo saveData = new ScriptableMapInfo();



        string path = Path.Combine(Application.dataPath, "database.json");


        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    public void JsonLoad()
    {
        ScriptableMapInfo saveData = new ScriptableMapInfo();

        string path = Path.Combine(Application.dataPath, "database.json");
        if (!File.Exists(path))
        {
            //SaveData();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<ScriptableMapInfo>(loadJson);
            //saveData.
            if (saveData != null)
            {
                for (int i = 0; i < saveData.createObjectInfoList.Count; i++)
                {
                    Debug.Log(saveData.createObjectInfoList[i].objectName);


                }
            }
        }
    }


    //public static void SaveObjFile(ScriptableMapInfo obj)
    //{
    //    //newlevel�� asset���� �����ϱ�, ���ϸ��� ��������, ���ҽ��� �ʼ���
    //    AssetDatabase.CreateAsset(obj, $"Assets/Resources/Levels/{obj.name}.asset");
    //    //asset ���� Ȯ��
    //    AssetDatabase.SaveAssets();
    //    //Unity Project�� �ֽ� ���·� ���� �� ������Ʈ ��ü�� ���� ������ üũ
    //    AssetDatabase.Refresh();
    //}

    //List<CreateObjectInfo> _createObjectInfoList;

    //�� �����ϴ� �޼���
    public void SaveMap(ScriptableMapInfo _mapInfo)
    {
        //ScriptableMapInfo saveData = new ScriptableMapInfo();

        string path = Path.Combine(Application.dataPath, $"MapData {_mapInfo.levelIndex}.json");


        var newObj = new ScriptableMapInfo();

        //newObj.name = $"Level Obj {_mapInfo.levelIndex}";

        //�� �ܾ���°� �ʿ�
        newObj.levelIndex = _mapInfo.levelIndex;
        newObj.backgroundNum = _mapInfo.backgroundNum;
        newObj.timerCount = _mapInfo.timerCount;
        newObj.playerLifePoint = _mapInfo.playerLifePoint;
        newObj.playerStartPos = _mapInfo.playerStartPos;
        newObj.mapScaleNum = _mapInfo.mapScaleNum;


        newObj.createObjectInfoList = _mapInfo.createObjectInfoList;

        string json = JsonUtility.ToJson(newObj, true);

        File.WriteAllText(path, json);


    }

    //public void ClearMap()
    //{
    //    //�������� var maps�� Tilemap ������Ʈ
    //    var maps = FindObjectsOfType<Tilemap>();
    //    var obj = GameObject.FindGameObjectsWithTag("Object");

    //    //maps�� ���� �ؿ� ���� �۾��� ������
    //    foreach (var tilemap in maps)
    //    {
    //        //Ÿ�ϸ� ����
    //        tilemap.ClearAllTiles();
    //    }

    //    foreach (var Obj in obj)
    //    {
    //        DestroyImmediate(Obj);
    //    }
    //}

    public void LoadMap(int _levelIndex, out ScriptableMapInfo _scriptableMapInfo)
    {
        //Resources �������� ��ũ���ͺ� ������Ʈ�� ScriptableLevel��
        //Level *.asset ������ level�� �����
        //var level = Resources.Load<ScriptableLevel>($"Levels/Level {_levelIndex}");
        //var obj = Resources.Load<ScriptableMapInfo>($"Levels/Level Obj {_levelIndex}");

        //_scriptableMapInfo = new ScriptableMapInfo();

        //ScriptableMapInfo saveData = new ScriptableMapInfo();

        string path = Path.Combine(Application.dataPath, $"MapData {_levelIndex}.json");
        if (!File.Exists(path))
        {
            //SaveData();
        }

        string loadJson = File.ReadAllText(path);
        _scriptableMapInfo = JsonUtility.FromJson<ScriptableMapInfo>(loadJson);
        //saveData.
        //if (_scriptableMapInfo != null)
        //{
        //    for (int i = 0; i < _scriptableMapInfo.createObjectInfoList.Count; i++)
        //    {
        //        Debug.Log(_scriptableMapInfo.createObjectInfoList[i].objectName);


        //    }
        //}
        
        //_scriptableMapInfo = obj;

        ////���� ������ ������ ���
        //if (obj == null)// || level == null)
        //{
        //    //����� �α׸� ��� �� �Լ� ����
        //    Debug.LogError($"Level {_levelIndex} does now exist.");
        //    return;
        //}



        ////�ҷ����� �� �� ����
        ////ClearMap();

        //CreateObjectInfo createObjectInfo = new CreateObjectInfo();

        //Vector3 playerStartPos = obj.playerStartPos;

        //Debug.Log(playerStartPos);

        //foreach (var savedObj in obj.createObjectInfoList)
        //{
        //    switch (savedObj)
        //    {

        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }

        //}



        ////�� Ÿ��, �Ѱ��� �ݺ��۾� �� foreach
        //foreach (var savedTile in level.GroundTiles)
        //{
        //    //switch�� ���� ó�� �ٲٱ�, level�� �ִ� enum ����
        //    switch (savedTile.Tile.Type)
        //    {
        //        case TileType.Field:
        //        case TileType.Brick:
        //        case TileType.Ice:
        //        case TileType.Grid:
        //        case TileType.Castle:
        //            //_groundMap�� Ÿ�ϵ� ó�����ֱ�
        //            SetTile(_gridMap, savedTile);
        //            break;
        //        default:
        //            //���� ó�� �� ���� �������� ���� �Ѿ�� ��� ���� �߻�
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        ////����Ÿ��, �� ��Ÿ���ϰ� �Ȱ���
        //foreach (var savedTile in level.UnitTiles)
        //{
        //    switch (savedTile.Tile.Type)
        //    {
        //        case TileType.Field:
        //        case TileType.Brick:
        //        case TileType.Ice:
        //        case TileType.Grid:
        //        case TileType.Castle:
        //            SetTile(_tempMap, savedTile);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}

        //void SetTile(Tilemap map, SavedTile tile)
        //{
        //    //Ÿ���� ������ �� Ÿ�� ��ȣ�� ���� �� ����ִ� �޼���
        //    map.SetTile(tile.Position, tile.Tile);
        //}

        //}

    }




    //level ����ü ����
    public struct Level
    {
        public int LevelIndex;
        public List<SavedTile> GroundTiles;
        public List<SavedTile> UnitTiles;

        //�����͸� ����ȭ�Ͽ� ���ڿ��� ��ȯ
        public string Serialize()
        {
            //string�� �ᵵ ������ stringBuilder�� �� ������ ���ڿ� ������ ��� ������ �����
            var builder = new StringBuilder();
            //���ڿ� ���ۿ� g[ cnrkgka
            builder.Append("g[");

            //foreach �ݺ���
            foreach (var groundTile in GroundTiles)
            {
                //��Ÿ���� type�� ���ڷ� ��ȯ, position�� ���ڿ��� ��ȯ �� stringBuilder�� �߰���
                builder.Append($"{(int)groundTile.Tile.Type}({groundTile.Position.x}," +
                    $" {groundTile.Position.y})");
            }
            //�������� ��
            builder.Append("]");

            //�߰��ߴ� �������� ���ڿ��� ��ȯ
            return builder.ToString();
        }
    }

}

