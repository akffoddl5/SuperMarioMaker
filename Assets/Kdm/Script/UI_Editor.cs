using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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


    int currentOpenButtonPanelNumber = -1;

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


    }

    // Update is called once per frame
    void Update()
    {

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

    //타일 선택 패널 버튼 클릭
    public void TileSetPanelButtonClick(int _buttonNum)
    {
        //if (currentOpenButtonPanelNumber < tileSetButtonEdge.Length)
        //    tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
        ButtonPanel_OnOff(_buttonNum);
        //tileSetButtonEdge[currentOpenButtonPanelNumber].SetActive(false);
    }

    //맵 버튼 클릭
    public void MapButtonClick()
    {
        ButtonPanel_OnOff(buttonPanel.Length - 1);
    }

    //버튼 패널 On, Off
    void ButtonPanel_OnOff(int _buttonNum)
    {
        if (_buttonNum < 0 || _buttonNum >= buttonPanel.Length)
        {
            Debug.Log("배열 범위를 벗어남");
            return;
        }

        //클릭한 버튼과 다른 패널이 켜져있거나 켜져있는 패널이 없는 경우 
        if (currentOpenButtonPanelNumber != _buttonNum)
        {
            //켜져있는 패널이 있을 경우 끔
            if (currentOpenButtonPanelNumber != -1)
                ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);

            //클릭한 버튼 패널을 켠다
            currentOpenButtonPanelNumber = _buttonNum;
            ButtonPanelSetActive_TRUE(currentOpenButtonPanelNumber);
        }
        else //현재 켜져있는 패널의 버튼을 누르면 패널을 끔
        {
            ButtonPanelSetActive_FALSE(currentOpenButtonPanelNumber);
            currentOpenButtonPanelNumber = -1;
        }
    }

    //버튼 패널 On
    void ButtonPanelSetActive_TRUE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(true);
        buttonEdge[_buttonNum].SetActive(true);
    }

    //버튼 패널 Off
    void ButtonPanelSetActive_FALSE(int _buttonNum)
    {
        buttonPanel[_buttonNum].SetActive(false);
        buttonEdge[_buttonNum].SetActive(false);
    }


    //타일 선택
    public void TileButtonClick(string _tileName)
    {
        buildSystem.SetCurrentTile(_tileName);

        //타일 선택 이미지 변경
        tileSetButtonImage[currentOpenButtonPanelNumber].sprite =
            buildSystem.currentTile[buildSystem.currentTile.Length - 1].sprite;

        ButtonPanel_OnOff(currentOpenButtonPanelNumber);
    }


    #endregion
}
