using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public Tile[] currentTile { get; private set; } = new Tile[1];
    //[SerializeField] public TileName currentTileName { get; private set; }
    public string currentTileName { get; private set; } = null;
    GameObject[] currentTileObjectPrefab;


    List<List<object>> objectList = new List<List<object>>();
    List<List<object>> undoList = new List<List<object>>();

    //[Serializable]
    //public class MyTiles
    //{
    //    public Tile[] tiles;
    //}
    //[SerializeField] private MyTiles[] tiles;


    [SerializeField] bool isSetTile = false;
    int tileX = 1;
    int tileY = 1;

    //Pipe ���� ����(���� : 0, ������ : 1, �Ʒ��� : 2, ���� : 3)
    public int pipeDir { get; set; } = 0;
    Vector3Int pipeTopPosition = new Vector3Int(0, 0, -100);
    Vector3Int defaultPipeTopPosition = new Vector3Int(0, 0, -100);

    Vector3 pipeLinkPos = new Vector3(0, 0, -100);
    int dirInfo = 0;
    //List<int> brickItemListInfo = new List<int>();

    //brick


    Vector3Int pastMousePosition;


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

        //Ÿ�� �̸����� Ÿ�ϰ� �������� ã������ ��ųʸ��� �̸��� �ε��� ����
        for (int i = 0; i < tiles.Length; i++)
        {
            tilesDictionary.Add(tiles[i].tileName, i);
        }
        Debug.Log(tiles.Length);

        currentTile[0] = null;

    }

    // Update is called once per frame
    void Update()
    {
        CameraMove();

        isSetTile = ui_Editor.IsSetTile();

        //if (!ui_Editor.pipeLinkMode)
        if (ui_Editor.functionEditMode == UI_Editor.FunctionEditMode.None)
            ClickSetTile();

        //Debug.Log("????????");
        //����Ʈ �׽�Ʈ�� 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate((GameObject)objectList[objectList.Count - 1][4], (Vector3)objectList[objectList.Count - 1][1], Quaternion.identity);
        }

    }

    private void CameraMove()
    {
        //����Ʈ ��ǥ
        Vector2 mousePositionInCamera = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //��ǥ (0 ~ 1) => (-1 ~ 1) ��ȯ
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

        ////ī�޶� �̵�
        Camera.main.transform.Translate(moveX * cameraSpeed * Time.deltaTime,
            moveY * cameraSpeed * Time.deltaTime, 0);

    }

    private void ClickSetTile()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.Log("���콺 ��ġ : " + mousePosition);
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3Int tilemapMousePosition = SetTilemap.WorldToCell(mousePosition);

        //if (pastMousePosition != tilemapMousePosition)

        //���� Ÿ�Ͽ� �°� x,y�� ����
        if (currentTileName == "Pipe")
        {
            if (pipeDir == 0 || pipeDir == 2)
            {
                tileX = 2;
                tileY = 1;
            }
            else if (pipeDir == 1 || pipeDir == 3)
            {
                tileX = 1;
                tileY = 2;
            }

            //�ӽ� Ÿ�ϸʿ� �׷��� ���� ��ġ Ÿ�� ������
            for (int i = 0; i < tileX; i++)
            {
                for (int j = 0; j < tileY; j++)
                {
                    TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
                }
            }
            //�ӽ� Ÿ�ϸʿ� ��ġ�� Ÿ�� ǥ��
            for (int i = 0; i < tileX; i++)
            {
                for (int j = 0; j < tileY; j++)
                {
                    TempTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + pipeDir * 4]);
                }
            }
        }
        else if (currentTileName == "StoneMonster")
        {
            tileX = 2;
            tileY = 2;
        }
        else if (currentTileName == "castle")
        {
            tileX = 5;
            tileY = 5;
        }
        else
        {
            tileX = 1;
            tileY = 1;
        }

        if (currentTileName == "Pipe")
        {

        }
        else
        {
            //�ӽ� Ÿ�ϸʿ� �׷��� ���� ��ġ Ÿ�� ������
            for (int i = 0; i < tileX; i++)
            {
                for (int j = 0; j < tileY; j++)
                {
                    TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
                }
            }
            //�ӽ� Ÿ�ϸʿ� ��ġ�� Ÿ�� ǥ��
            for (int i = 0; i < tileX; i++)
            {
                for (int j = 0; j < tileY; j++)
                {
                    TempTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j * tileX]);
                }
            }
        }


        //���� ���콺 ��ġ ����
        pastMousePosition = tilemapMousePosition;

        //Ÿ�� ��ġ
        if (Input.GetMouseButton(0) && isSetTile)
        {

            if (PossibleSetTile(tilemapMousePosition))
            {
                //������Ʈ�� ������ ������������ ���
                Vector3 tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);

                Vector3 createPos = new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                    tilemapToWorldPoint.y + grid.cellSize.x / 2);

                GameObject createObj;
                int dirInfo = 0;

                //������ ��ġ
                if (currentTileName == "Pipe")
                {
                    int pipePrefabIndex;

                    //ù Ŭ���� ������ �Ա� ����
                    if (pipeTopPosition == defaultPipeTopPosition)
                    {
                        pipeTopPosition = tilemapMousePosition;

                        //Ÿ�ϸʿ� Ÿ�� ����
                        for (int i = 0; i < tileX; i++)
                        {
                            for (int j = 0; j < tileY; j++)
                            {
                                SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + pipeDir * 4]);
                            }
                        }

                        if (pipeDir == 2)
                            createPos += new Vector3(grid.cellSize.x, 0);
                        else if (pipeDir == 3)
                            createPos += new Vector3(0, -grid.cellSize.y);

                        pipePrefabIndex = 0;
                    }
                    else //������ ���� ����
                    {
                        //������ ������ �Ա� �����θ� ����������� ����
                        if (pipeDir == 0 || pipeDir == 2)
                        {
                            tilemapMousePosition = new Vector3Int(pipeTopPosition.x, tilemapMousePosition.y);
                            if (pipeDir == 0 && tilemapMousePosition.y > pipeTopPosition.y)
                                return;
                            else if (pipeDir == 2 && tilemapMousePosition.y < pipeTopPosition.y)
                                return;
                        }
                        else if (pipeDir == 1 || pipeDir == 3)
                        {
                            tilemapMousePosition = new Vector3Int(tilemapMousePosition.x, pipeTopPosition.y);
                            if (pipeDir == 1 && tilemapMousePosition.x > pipeTopPosition.x)
                                return;
                            else if (pipeDir == 3 && tilemapMousePosition.x < pipeTopPosition.x)
                                return;
                        }
                        if (!PossibleSetTile(tilemapMousePosition))
                        {
                            return;
                        }

                        //Ÿ�ϸʿ� Ÿ�� ����
                        for (int i = 0; i < tileX; i++)
                        {
                            for (int j = 0; j < tileY; j++)
                            {
                                SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j + 2 + pipeDir * 4]);
                            }
                        }

                        tilemapToWorldPoint = SetTilemap.CellToWorld(tilemapMousePosition);
                        createPos = new Vector3(tilemapToWorldPoint.x + grid.cellSize.x / 2,
                            tilemapToWorldPoint.y + grid.cellSize.x / 2);

                        if (pipeDir == 2)
                            createPos += new Vector3(grid.cellSize.x, 0);
                        else if (pipeDir == 3)
                            createPos += new Vector3(0, -grid.cellSize.y);

                        pipePrefabIndex = 1;
                    }

                    dirInfo = pipeDir;

                    //���� ������Ʈ ����
                    createObj = Instantiate(currentTileObjectPrefab[pipePrefabIndex], createPos, Quaternion.Euler(0, 0, pipeDir * (-90)));
                }
                else
                {
                    //Ÿ�ϸʿ� Ÿ�� ����
                    //SetTilemap.SetTile(tilemapMousePosition, currentTile[0]);
                    for (int i = 0; i < tileX; i++)
                    {
                        for (int j = 0; j < tileY; j++)
                        {
                            SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), currentTile[i + j * tileX]);
                        }
                    }

                    createObj = Instantiate(currentTileObjectPrefab[0], createPos, Quaternion.identity);
                }

                //����Ʈ�� ���� ���� ����(�̸�, ���� ������ġ, �׸��� ������ġ ����, �׸��� ������ġ ��, ������ ���� ������Ʈ)
                //������Ʈ �����Ҷ��� �����
                //objectList.Add(new List<object> { currentTileName, createPos, tilemapMousePosition,
                //    tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });
                objectList.Add(new List<object> { currentTileName, createPos, pipeLinkPos, dirInfo, new List<int>(),
                    tilemapMousePosition, tilemapMousePosition + new Vector3Int(tileX - 1, -(tileY - 1)), createObj });


            }



        }
        //Ÿ�� ����
        else if (Input.GetMouseButton(1) && isSetTile)
        {
            //SetTilemap.SetTile(tilemapMousePosition, null);
            //tilemap.DeleteCells(gridPosition, tilemapPosition);

            //�� Ÿ������ Ȯ��
            if (SetTilemap.GetTile(tilemapMousePosition) != null)
            {
                //����Ʈ���� �����ؾ��� ������Ʈ �˻�
                for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
                {
                    Vector3Int startSearchPoint = ((Vector3Int)objectList[listIndex][5]);
                    Vector3Int endSearchPoint = ((Vector3Int)objectList[listIndex][6]);
                    //���콺 ��ġ�� �ش��ϴ� ������Ʈ �˻�
                    if (tilemapMousePosition.x >= startSearchPoint.x &&
                        tilemapMousePosition.x <= endSearchPoint.x &&
                        tilemapMousePosition.y <= startSearchPoint.y &&
                        tilemapMousePosition.y >= endSearchPoint.y)
                    {
                        //��ġ�� Ÿ�� ����
                        for (int i = 0; i <= endSearchPoint.x - startSearchPoint.x; i++)
                        {
                            for (int j = 0; j <= -(endSearchPoint.y - startSearchPoint.y); j++)
                            {
                                SetTilemap.SetTile(startSearchPoint + new Vector3Int(i, -j), null);
                            }
                        }
                        //((GameObject)objectList[listIndex][4]).SetActive(false);

                        //������ ������Ʈ ����
                        Destroy((GameObject)objectList[listIndex][7]);
                        //����Ʈ ��ҿ��� ����
                        objectList.RemoveAt(listIndex);
                    }
                }
            }

        }

        //���콺 �� ��
        if (Input.GetMouseButtonUp(0))
        {
            //������ Ÿ�� ��ġ�� ������ pipeTopPosition �ʱ�ȭ
            if (currentTileName == "Pipe")
            {
                pipeTopPosition = defaultPipeTopPosition;



                //(������ �� �κ� HardBrick���� ������(�̿ϼ�))
                ////Ÿ�ϸʿ� Ÿ�� ����
                //for (int i = 0; i < tileX; i++)
                //{
                //    for (int j = 0; j < tileY; j++)
                //    {
                //        SetTilemap.SetTile(tilemapMousePosition + new Vector3Int(i, -j), tiles[tilesDictionary["HardBrick"]].tile[0]);
                //    }
                //}
            }
        }

    }

    //���� Ÿ�� ����
    public void SetCurrentTile(string _tileName)
    {
        PastTempTileClear();

        //���� Ÿ�� ����
        currentTile = tiles[tilesDictionary[_tileName]].tile;
        currentTileName = _tileName;
        currentTileObjectPrefab = tiles[tilesDictionary[_tileName]].objectPrefab;

    }

    //Ÿ�� ���� �� �ӽ� Ÿ�ϸʿ� �׷��� ���� ��ġ Ÿ�� ������
    public void PastTempTileClear()
    {
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                TempTilemap.SetTile(pastMousePosition + new Vector3Int(i, -j), null);
            }
        }
    }

    bool PossibleSetTile(Vector3Int _tilemapMousePosition)
    {
        for (int i = 0; i < tileX; i++)
        {
            for (int j = 0; j < tileY; j++)
            {
                if (SetTilemap.GetTile(_tilemapMousePosition + new Vector3Int(i, -j)) != null)
                    return false;
            }
        }

        return true;
    }

    public void UndoListInput()
    {

    }

    public void Undo()
    {

    }

    public void PipeLinkPos_ObjectListInput(GameObject _pipeLinkObject_0, GameObject _pipeLinkObject_1)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_pipeLinkObject_0 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_1.transform.position;
            }

            if (_pipeLinkObject_1 == (GameObject)objectList[listIndex][7])
            {
                objectList[listIndex][2] = _pipeLinkObject_0.transform.position;
            }
        }
    }

    public int BrickItemSet_ObjectListInput(GameObject _itemSetBrick, int _brickItemNum)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                ((List<int>)objectList[listIndex][4]).Add(_brickItemNum);

                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }

    public int BrickItemSet_ObjectListOutput(GameObject _itemSetBrick, int _brickItemNum)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                ((List<int>)objectList[listIndex][4]).RemoveAt(((List<int>)objectList[listIndex][4]).Count - 1);

                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }

    public int BrickItemSet_ObjectListCount(GameObject _itemSetBrick)
    {
        for (int listIndex = 0; listIndex < objectList.Count; listIndex++)
        {
            if (_itemSetBrick == (GameObject)objectList[listIndex][7])
            {
                return ((List<int>)objectList[listIndex][4]).Count;
            }
        }
        return 0;
    }
}
