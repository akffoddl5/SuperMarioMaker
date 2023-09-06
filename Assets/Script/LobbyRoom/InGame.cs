using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviourPunCallbacks 
{
    [HideInInspector] public string winnerName;
    Text winTxt;
    object myFrefab;
    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private void Start()
    {
        winTxt = GameObject.Find("Win_text").GetComponent<Text>();
        if(winTxt != null )
        {
            winTxt.text = WIndowManager.instance.nickName + " win";
        }

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("characterName", out object myFrefab);
        PhotonNetwork.Instantiate((string)myFrefab, Vector3.zero, Quaternion.identity);
        AudioManager.instance.PlayerOneShot(MARIO_SOUND.INGAME_BGM, true, 0);

    }



 
}
