using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildSystem : MonoBehaviour
{

    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tile;
    [SerializeField] private Tile deleteTile;
    int tileNum = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("���콺 ��ġ : " + mousePosition);
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        Vector3Int tilemapPosition = tilemap.WorldToCell(mousePosition);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tileNum++;
            if (tileNum >= tile.Length)
                tileNum = 0;
        }

        
        if (Input.GetMouseButton(0))
        {
            tilemap.SetTile(tilemapPosition, tile[tileNum]);
        }
        else if (Input.GetMouseButton(1))
        {
            tilemap.SetTile(tilemapPosition, deleteTile);
            //tilemap.DeleteCells(gridPosition, tilemapPosition);
        }

        Debug.Log("�׸��� ��ġ : " + gridPosition);
        Debug.Log("Ÿ�ϸ� ��ġ : " + tilemapPosition);


    }
}
