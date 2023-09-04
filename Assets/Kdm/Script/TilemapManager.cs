using System;
using System.Collections;
using System.Collections.Generic;
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
        
        //newlevel�� asset���� �����ϱ�, ���ϸ��� ��������, ���ҽ��� �ʼ���
        AssetDatabase.CreateAsset(level, $"Assets/Resources/Levels/{level.name}.asset");
        //asset ���� Ȯ��
        AssetDatabase.SaveAssets();
        //Unity Project�� �ֽ� ���·� ���� �� ������Ʈ ��ü�� ���� ������ üũ
        AssetDatabase.Refresh();
    }

    public static void SaveObjFile(ScriptableMapInfo obj)
    {
        //newlevel�� asset���� �����ϱ�, ���ϸ��� ��������, ���ҽ��� �ʼ���
        AssetDatabase.CreateAsset(obj, $"Assets/Resources/Levels/{obj.name}.asset");
        //asset ���� Ȯ��
        AssetDatabase.SaveAssets();
        //Unity Project�� �ֽ� ���·� ���� �� ������Ʈ ��ü�� ���� ������ üũ
        AssetDatabase.Refresh();
    }

    //List<CreateObjectInfo> _createObjectInfoList;

    //�� �����ϴ� �޼���
    public void SaveMap(ScriptableMapInfo _mapInfo)
    {
        var newObj = ScriptableObject.CreateInstance<ScriptableMapInfo>();

        newObj.name = $"Level Obj {_mapInfo.levelIndex}";

        //�� �ܾ���°� �ʿ�
        newObj.levelIndex = _mapInfo.levelIndex;
        newObj.backgroundNum = _mapInfo.backgroundNum;
        newObj.timerCount = _mapInfo.timerCount;
        newObj.playerLifePoint = _mapInfo.playerLifePoint;
        newObj.playerStartPos = _mapInfo.playerStartPos;
        newObj.mapScaleNum = _mapInfo.mapScaleNum;

        //foreach (var obj in _createObjectInfoList)
        //{
        //    yield return new CreateObjectInfo()
        //    {

        //    };
        //}

        newObj.createObjectInfoList = _mapInfo.createObjectInfoList;



        //�±� �̸� ����
        //GameObject[] obj = GameObject.FindGameObjectsWithTag("Object");

        //IEnumerable<CreateObjectInfo> GetObjFromMap(Tilemap map)
        //{
        //    foreach (var pos in obj)
        //    {
        //        yield return new CreateObjectInfo()
        //        {
        //            objectName = pos.name,
        //            createPos = pos.transform.position,
        //        };
        //    }
        //}

        SaveObjFile(newObj);


        //��ũ��Ʈ ������Ʈȭ �� ScriptableLevel�� newLevel�� ���Խ�Ŵ
        var newLevel = ScriptableObject.CreateInstance<ScriptableLevel>();

        newLevel.levelIndex = _mapInfo.levelIndex;
        newLevel.name = $"Level {_mapInfo.levelIndex}";

        ////GetTilesFromMap���� ������ ����Ʈ�� ������ ���� �ʰ� Ÿ�ϰ��� ��������
        //newLevel.GroundTiles = GetTilesFromMap(_gridMap).ToList();
        //newLevel.UnitTiles = GetTilesFromMap(_tempMap).ToList();

        //jsonȭ ���� �ʿ���
        var json = JsonUtility.ToJson(newLevel);
        //���¿� ����
        SaveLevelFile(newLevel);

        ////foreach�� ����ϱ� ���� IEnumerable ���, ����� ��ȯ
        //IEnumerable<SavedTile> GetTilesFromMap(Tilemap map)
        //{
        //    //cellbounds : Ÿ�ϸ��� ��踦 �� ũ��� ��ȯ
        //    foreach (var pos in map.cellBounds.allPositionsWithin)
        //    {
        //        //pos�� �ش��ϴ� Ÿ���� �����ϴ��� �ƴ��� üũ
        //        if (map.HasTile(pos))
        //        {
        //            //���� Ÿ���� �����Ѵٸ� levelTile�� ��ȯ
        //            var levelTile = map.GetTile<LevelTile>(pos);
        //            //�� ��갪�� ����, SavedTile�� ��ȯ
        //            yield return new SavedTile()
        //            {
        //                Position = pos,
        //                Tile = levelTile
        //            };
        //        }
        //    }
        //}
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
        var obj = Resources.Load<ScriptableMapInfo>($"Levels/Level Obj {_levelIndex}");

        _scriptableMapInfo = obj;

        //���� ������ ������ ���
        if (obj == null)// || level == null)
        {
            //����� �α׸� ��� �� �Լ� ����
            Debug.LogError($"Level {_levelIndex} does now exist.");
            return;
        }

        

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

    }

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

