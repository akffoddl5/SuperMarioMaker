using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject obj_marioIdle;
    public GameObject obj_marioJump;
    public GameObject obj_logInPanel;

    public TextMeshProUGUI txt_pressToStart;
    
    public string lobbySceneName;
    
    bool isTextInvisible = false;

    bool isLogin = false;

    Coroutine blinkTxt;
    Coroutine marioJump;

    public AudioSource main_audioSource;
    public AudioSource jump_audioSource;
    public AudioSource select_audioSource;
    

	void Start()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.LOGIN_BGM,true, 0);
        blinkTxt = StartCoroutine(blinkStart());
        ShowBeforeLogin();
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
        // 파이어베이스에서 사용자인증해서 성공하면
        // 로그인 성공
        isLogin = true;
		ShowAfterLogin();

		// 깜빡깜빡 코루틴 종료 (텍스트 투명도 1로 되도록 만들어야 함)
		StopCoroutine(blinkTxt);
        if (txt_pressToStart.color.a <= 1) txt_pressToStart.color = Color.white;

		txt_pressToStart.text = "Login Complete!!";

        // 마리오 점프 이미지가 또용하고 올라가는 것처럼 보이게
        marioJump = StartCoroutine(JumpMario());
	}

    void ShowBeforeLogin()
    {
		//로그인 패널 끄기
		obj_logInPanel.SetActive(false);
       // AudioManager.instance.PlayerOneShot(MARIO_SOUND.OUTBUTTEM, false, 0);
        //마리오 서 있는 이미지 켜기, 점프 이미지 끄기
        obj_marioIdle.SetActive(true);
		obj_marioJump.SetActive(false);

	}
    void ShowAfterLogin()
    {
		// 로그인이 되면 로그인 패널을 끄고
		obj_logInPanel.SetActive(false);
       // AudioManager.instance.PlayerOneShot(MARIO_SOUND.OUTBUTTEM, false, 0);
        // 마리오 서있는 이미지 끄고, 점프 이미지 키고
        obj_marioIdle.SetActive(false);
		obj_marioJump.SetActive(true);
	}

	// Press to Start 버튼이 깜빡 거리게 코루틴
    IEnumerator blinkStart()
    {
        while (true)
        {
            Color invisibleWhite = new Color(1, 1, 1, 0.2f);
            // 텍스트가 보이는 상태 isTextInvisible = false인 상태
            if (!isTextInvisible)
            {
                txt_pressToStart.color -= invisibleWhite;
                yield return new WaitForSeconds(0.1f);
		        // 텍스트가 안 보이게 되면 isTextInvisible=true로 변경해주기
		        if (txt_pressToStart.color.a <= 0)
                {
                    isTextInvisible = true;
                }
            }
            // 텍스트가 안 보이는 상태라면 
            else
            {
			    txt_pressToStart.color += invisibleWhite;
                yield return new WaitForSeconds(0.1f);
			    // 텍스트가 보이게 되면 isTextInvisible = false 로 변경해주기
			    if (txt_pressToStart.color.a >= 1)
			    {
				    isTextInvisible = false;
			    }
		    }
        }
	}

	// 마리오 점프
	IEnumerator JumpMario()
    {
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.JUMP,false,0);
        
        yield return new WaitForSeconds(0.28f);
		Vector3 a = new Vector3(obj_marioJump.transform.localPosition.x , obj_marioJump.transform.localPosition.y , obj_marioJump.transform.localPosition.z);
		Vector3 b = new Vector3(obj_marioJump.transform.localPosition.x, obj_marioJump.transform.localPosition.y +140f, obj_marioJump.transform.localPosition.z);
        // -160 -> 14까지 증가
        while (obj_marioJump.transform.localPosition.y < b.y - 0.1f)
        {
            obj_marioJump.transform.localPosition = Vector3.Lerp(obj_marioJump.transform.localPosition, b, 0.5f);
            yield return new WaitForSeconds(0.08f);
		}
        while (obj_marioJump.transform.localPosition.y > a.y+3f)
        {
			obj_marioJump.transform.localPosition = Vector3.Lerp(obj_marioJump.transform.localPosition, a, 0.2f);
			yield return new WaitForSeconds(0.08f);
		}

		obj_marioJump.gameObject.SetActive(false);
        obj_marioIdle.gameObject.SetActive(true);


        // 씬 옮기기
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(lobbySceneName);
	}
}