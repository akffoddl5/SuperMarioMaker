using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
//using static UI_Editor;

public class UI_Editor : MonoBehaviour
{
    public static UI_Editor instance;

    BuildSystem buildSystem;

    [Header("Panel")]
    [SerializeField] RectTransform upPanel;
    [SerializeField] RectTransform leftPanel;
    [SerializeField] RectTransform rightPanel;

    [Header("UpPanel")]
    [SerializeField] GameObject[] buttonEdge;
    [SerializeField] Image[] tileSetButtonImage;

    [Header("LeftPanel")]
    [SerializeField] Image backgroundSetImage;
    [SerializeField] Sprite[] backgroundSprite;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider timerSlider;
    [SerializeField] TextMeshProUGUI lifeCountText;

    [Header("RightPanel")]
    [SerializeField] GameObject characterSetEdge;

    [Header("ButtonPanel")]
    [SerializeField] GameObject[] buttonPanel;

    [SerializeField] GameObject brickSetPanel;

    int currentOpenButtonPanelNumber = -1;


    public enum FunctionEditMode
    {
        None,
        PipeLinkMode,
        BrickItemSetMode
    }
    public FunctionEditMode functionEditMode { get; private set; } = FunctionEditMode.None;
    //public bool pipeLinkMode { get; private set; } = false;
    GameObject[] pipeLinkObject = new GameObject[2];
    List<GameObject> alreadyPipeLinkObject = new List<GameObject>();


    [SerializeField] GameObject[] itemPrefab;
    GameObject currentSetBrick = null;
    [SerializeField] TextMeshProUGUI itemCountText;
    int itemCount = 0;


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
        buildSystem = BuildSystem.instance;
        Debug.Log(pipeLinkObject[0]);

    }

    // Update is called once per frame
    void Update()
    {
        if (functionEditMode != FunctionEditMode.BrickItemSetMode && brickSetPanel.activeSelf)
        {
            BrickSetPanel_Off();
        }
        FunctionEditMode_On();
        Debug.Log(currentSetBrick);
    }

    private void FunctionEditMode_On()
    {
        //������ ���� ���
        if (functionEditMode == FunctionEditMode.PipeLinkMode)
        {
            //���콺 Ŭ��
            if (Input.GetMouseButtonDown(0))
            {
                //Ŭ���� ��� Ȯ��
                GameObject tempObject = RaycastHitObject();
                if (tempObject != null)
                {
                    PipeLink(tempObject);
                }

            }
        }
        else if (functionEditMode == FunctionEditMode.BrickItemSetMode)
        {
            //���콺 Ŭ��
            if (Input.GetMouseButtonDown(0))
            {
                //Ŭ���� ��� Ȯ��
                GameObject tempObject = RaycastHitObject();
                if (tempObject != null)
                {
                    BrickSet(tempObject);
                }
            }
            //Debug.Log(currentSetBrick);

        }
    }

    void BrickSet(GameObject _tempObject)
    {
        //Ŭ���� ����� ���� ��
        if (_tempObject.GetComponent<Box>() != null || _tempObject.GetComponentInParent<Box>() != null)
        {
            currentSetBrick = _tempObject;
        }
    }


    void PipeLink(GameObject _tempObject)
    {
        //Ŭ���� ����� �������� ��
        if (_tempObject.GetComponent<Pipe_top>() != null)
        {
            //�̹� ����� ���������� Ȯ��
            for (int i = 0; i < alreadyPipeLinkObject.Count; i++)
            {
                if (alreadyPipeLinkObject[i] == _tempObject)
                {
                    //Debug.Log("�̹� ����� ������");
                    return;
                }
            }
            //ù��°�� ���� ���������� Ȯ��(�ڱ� �ڽſ� ���� X)
            if (_tempObject == pipeLinkObject[0])
            {
                //Debug.Log("ù��°�� ���� ������");
                return;
            }

            int pipeLinkPosIndex;
            if (pipeLinkObject[0] == null)
            {
                pipeLinkPosIndex = 0;
            }
            else if (pipeLinkObject[1] == null)
            {
                pipeLinkPosIndex = 1;
            }
            else
            {
                return;
            }


            pipeLinkObject[pipeLinkPosIndex] = _tempObject;

            if (pipeLinkPosIndex == 0)
            {
                pipeLinkObject[pipeLinkPosIndex].GetComponent<Pipe_top>().lineActive = true;
            }
            else if (pipeLinkPosIndex == 1)
            {
                buildSystem.PipeLinkPos_ObjectListInput(pipeLinkObject[0], pipeLinkObject[1]);

                pipeLinkObject[0].GetComponent<Pipe_top>().linkObjectTransform =
                    pipeLinkObject[1].GetComponent<Pipe_top>().myTransform;
                pipeLinkObject[1].GetComponent<Pipe_top>().linkObjectTransform =
                    pipeLinkObject[0].GetComponent<Pipe_top>().myTransform;

                alreadyPipeLinkObject.Add(pipeLinkObject[0]);
                alreadyPipeLinkObject.Add(pipeLinkObject[1]);

                pipeLinkObject[0] = null;
                pipeLinkObject[1] = null;
            }
        }
    }

    GameObject RaycastHitObject()
    {
        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(ray, transform.forward, 15);

        if (hit)
        {
            return hit.collider.gameObject;
        }

        return null;
    }


    public bool IsSetTile()
    {
        Vector2 mousePos = Input.mousePosition;
        if ((mousePos.x >= upPanel.position.x && mousePos.x <= upPanel.position.x + upPanel.rect.width &&
            mousePos.y >= upPanel.position.y && mousePos.y <= upPanel.position.y + upPanel.rect.height) ||
            (mousePos.x >= leftPanel.position.x && mousePos.x <= leftPanel.position.x + leftPanel.rect.width &&
            mousePos.y >= leftPanel.position.y && mousePos.y <= leftPanel.position.y + leftPanel.rect.height) ||
            (mousePos.x >= rightPanel.position.x && mousePos.x <= rightPanel.position.x + rightPanel.rect.width &&
            mousePos.y >= rightPanel.position.y && mousePos.y <= rightPanel.position.y + rightPanel.rect.height) ||
            buildSystem.currentTileName == null || currentOpenButtonPanelNumber != -1)
            return false;

        return true;
    }


    #region Button

    //Ÿ�� ���� �г� ��ư Ŭ��
    public void TileSetPanelButtonClick(int _buttonNum)
    {
        //if (currentOpenButtonPanelNumber < tileSetButtonEdge.Length)
        //    tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
        ButtonPanel_OnOff(_buttonNum);
        //tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
    }

    //�� ��ư Ŭ��
    public void MapButtonClick()
    {
        ButtonPanel_OnOff(buttonPanel.Length - 1);
    }

    //��ư �г� On, Off
    void ButtonPanel_OnOff(int _buttonNum)
    {
        if (_buttonNum < 0 || _buttonNum >= buttonPanel.Length)
        {
            Debug.Log("�迭 ������ ���");
            return;
        }

        //Ŭ���� ��ư�� �ٸ� �г��� �����ְų� �����ִ� �г��� ���� ��� 
        if (currentOpenButtonPanelNumber != _buttonNum)
        {
            //�����ִ� �г��� ���� ��� ��
            if (currentOpenButtonPanelNumber != -1)
                ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);

            //Ŭ���� ��ư �г��� �Ҵ�
            currentOpenButtonPanelNumber = _buttonNum;
            ButtonPanelSetActive_TRUE(currentOpenButtonPanelNumber);
        }
        else //���� �����ִ� �г��� ��ư�� ������ �г��� ��
        {
            ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);
            currentOpenButtonPanelNumber = -1;
        }

        if (functionEditMode == FunctionEditMode.BrickItemSetMode)
        {
            functionEditMode = FunctionEditMode.None;
        }
    }

    //��ư �г� On
    void ButtonPanelSetActive_TRUE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(true);
        buttonEdge[_buttonNum].SetActive(true);
    }

    //��ư �г� Off
    void ButtonPanelSetActive_FALSE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(false);
        buttonEdge[_buttonNum].SetActive(false);
    }


    //Ÿ�� ����
    public void TileButtonClick(string _tileName)
    {
        buildSystem.SetCurrentTile(_tileName);

        //Ÿ�� ���� �̹��� ����
        tileSetButtonImage[currentOpenButtonPanelNumber].sprite =
            buildSystem.currentTile[buildSystem.currentTile.Length - 1].sprite;

        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        //pipeLinkMode = false;
        functionEditMode = FunctionEditMode.None;
    }


    //������ ���� ��ư(����)
    public void PipeTileButtonClick(int _pipeDir)
    {
        buildSystem.pipeDir = _pipeDir;
        TileButtonClick("Pipe");
    }

    //������ ���� ��ư
    public void PipeLinkButtonClick()
    {
        //pipeLinkMode = true;
        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        functionEditMode = FunctionEditMode.PipeLinkMode;

        buildSystem.PastTempTileClear();


    }


    public void BrickItemSetButtonClick()
    {
        ButtonPanel_OnOff(currentOpenButtonPanelNumber);

        functionEditMode = FunctionEditMode.BrickItemSetMode;

        //buildSystem.PastTempTileClear();

        brickSetPanel.SetActive(true);

    }

    public void BrickSetPanel_Off()
    {
        functionEditMode = FunctionEditMode.None;

        brickSetPanel.SetActive(false);
    }

    public void BrickItemButtonClick(int _brickItemNum)
    {
        int itemCount = buildSystem.BrickItemSet_ObjectListInput(currentSetBrick, _brickItemNum);
        //Debug.Log(itemCount);
        itemCountText.text = itemCount.ToString();
    }

    #endregion
}
