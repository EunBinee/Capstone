using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    //* 메인 씬 이름
    public string mainSceneName = "StartMainScene";

    public GameObject player;
    public Transform playerTargetPos;
    public Transform playerHeadPos;
    public Transform playerBackPos;
    public Transform GetPlayerTransform()
    {
        return player.transform;
    }
    public PlayerController GetPlayerController()
    {
        return player.gameObject.GetComponent<PlayerController>();
    }
    public PlayerMovement GetPlayerMovement()
    {
        return player.gameObject.GetComponent<PlayerMovement>();
    }
    public GameObject playerCamera;      //카메라 오브젝트
    public GameObject playerCameraPivot; //카메라 피봇
    public Camera cameraObj;             //카메라.


    [Header("데미지 기준")]
    public float smallDamage = 100;
    public float midDamage = 300;
    public float bigDamage = 600;
    public float criticalAdditionalMultiples = 2f;
    public Material hitMaterial;


}
