using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject obj_marioIdle;
    public GameObject obj_marioJump;
    public GameObject obj_logInPanel;

    public TextMeshProUGUI txt_pressToStart;
    
    bool isLogin = false;

    // Start is called before the first frame update
    void Start()
    {
        //마리오 서 있는 이미지 켜기, 점프 이미지 끄기
        obj_marioIdle.SetActive(true);
        obj_marioJump.SetActive(false);

        //로그인 패널 끄기
		obj_logInPanel.SetActive(false);

	}

    // Update is called once per frame
    void Update()
    {
        // 좌클릭이 되면 로그인 화면 띄우기
        if (Input.GetMouseButtonDown(0) && !isLogin)
        {
			obj_logInPanel.SetActive(true);
		}
    }

    public void LogIn()
    {
        // 로그인 성공
        isLogin = true;

        // 로그인이 되면 로그인 패널을 끄고
		obj_logInPanel.SetActive(false);
        // 마리오 서있는 이미지 끄고
        obj_marioIdle.SetActive(false);
        // 점프 이미지 키고
        obj_marioJump.SetActive(true);

        // 깜빡깜빡 코루틴 종료 (텍스트 투명도 1로 되도록 만들어야 함)

        // 텍스트 로그인 성공으로 변경
        txt_pressToStart.text = "Log in Complete!!";

        // 마리오 점프 이미지가 또용하고 올라가는 것처럼 보이게

	}

	// Press to Start 버튼이 깜빡 거리게 코루틴

	// 점프 
}