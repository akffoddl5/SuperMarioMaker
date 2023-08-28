using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    UI_Editor ui_Editor;

    [Serializable]
    class Tiles
    {
        [SerializeField] Tile[] Tile;// { get; private set; }
        public Tile[] tile { get { return Tile; } }

        [SerializeField] string TileName;
        public string tileName { get { return TileName; } }

        [SerializeField] GameObject[] ObjectPrefab;
        public GameObject[] objectPrefab { get { return ObjectPrefab; } }
    }

    //public enum TileName
    //{
    //    ground,
    //    brick,
    //    castle,
    //    coin,
    //    flag,
    //    flower,
    //    hardBrick,
    //    iceBrick,
    //    iceCoin,
    //    MushroomRed,
    //    MushroomGreen,
    //    nomalBrick,
    //    pipe,
    //    questionBrick0,
    //    questionBrick1,
    //    star,
    //    stoneMonster,
    //    thornsBrick,
    //    goomba,
    //    turtle,
    //    boo
    //}

    [SerializeField] public int AAA { get; private set; }

    [SerializeField]
    Tiles[] tiles;

    //TileName tileName;

    [SerializeField] private Grid grid;

    [SerializeField] private Tilemap TempTilemap;
    [SerializeField] private Tilemap SetTilemap;

    int tileNum = 0;
    //[SerializeField] private Tile deleteTile;
    [SerializeField] private List<Tile> tileList;
    //Dictionary<TileName, Tile> tiles = new Dictionary<TileName, Tile>();
    Dictionary<string, int> tilesDictionary = new Dictionary<string, int>();

    private Tile[] currentTile = new Tile[1];
    //[SerializeField] public TileName currentTileName { get; private set; }
    private string currentTileName;
    GameObject[] currentTileObjectPrefab;

    //[Serializable]
    //public class MyTiles
    //{
    //    public Tile[] tiles;
    //}
    //[SerializeField] private MyTiles[] tiles;


    [SerializeField] bool isSetTile = false;


    Vector3Int pastMousePosition;

    //public List<int[]> asd = new List<int[]>();


    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraMoveTriggerPos;





    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ui_Editor = UI_Editor.instance;

        //for (int i = 0; i < tileList.Count; i++)
        //{
        //    tiles.Add((TileName)i, tileList[i]);
        //}

        for (int i = 0; i < tiles.Length; i++)
        {
            tilesDictionary.Add(tiles[i].tileName, i);
        }

        currentTile[0] = null;

    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();

        //SetCurrentTile(currentTileName);

        ClickSetTile();

    }

    private void CameraMove()
    {
        //뷰포트 좌표
        Vector2 mousePositionInCamera = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //좌표 (0 ~ 1) => (-1 ~ 1) 변환
        mousePositionInCamera = new Vector2(mousePositionInCamera.x * 2 - 1,
            mousePositionInCamera.y * 2 - 1);

        float moveX = 0;
        float moveY = 0;
        if (mousePositionInCamera.x >= cameraMoveTriggerPos)
            moveX = 1;
        else if (mousePositionInCamera.x <= -cameraMoveTriggerPos)
            moveX = -1;

        if (mousePositionInCamera.y >= cameraMoveTriggerPos)
            moveY = 1;
        else if (mousePositionInCamera.y <= -cameraMoveTriggerPos)
            moveY = -1;

        //카메라 이동
        Camera.main.transform.Translate(moveX * cameraSpeed * Time.deltaTime,
            moveY * cameraSpeed * Time.deltaTime, 0);
    }

    private void ClickSetTile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log("마우스 위치 : " + mousePosition);
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3Int tilemapMousePosition = SetTilemap.WorldToCell(mousePosition);

        //if (pastMousePosition != tilemapMousePosition)
        TempTilemap.SetTile(pastMousePosition, null);
        TempTilemap.SetTile(tilemapMousePosition, currentTile[0]);

        pastMousePosition = tilemapMousePosition;

        if (Input.GetMouseButton(0) && isSetTile)
        {
            if (SetTilemap.GetTile(tilemapMousePosition) == null)
            {
                //타일맵에 타일 생성
                SetTilemap.SetTile(tilemapMousePosition, currentTile[0]);

                //실제 오브젝트 생성
                //(생성할 오브젝트, 생성위치 리스트에 저장해놓고 한번에 생성할듯?)
                Vector3 tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);
                Instantiate(currentTileObjectPrefab[0],
                    new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                    tilemapToWorldPoint.y + grid.cellSize.x / 2), Quaternion.identity);
            }



        }
        else if (Input.GetMouseButton(1) && isSetTile)
        {
            SetTilemap.SetTile(tilemapMousePosition, null);
            //tilemap.DeleteCells(gridPosition, tilemapPosition);
        }
    }

    public void SetCurrentTile(string _tileName)
    {
        currentTile = tiles[tilesDictionary[_tileName]].tile;
        currentTileName = _tileName;
        currentTileObjectPrefab = tiles[tilesDictionary[_tileName]].objectPrefab;

        isSetTile = true;
    }
}
