using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueInfo : MonoBehaviour
{
    // DialogueManager dialogueManager;
    // GameInfo gameInfo;

    // private void Start()
    // {

    //     dialogueManager = GetComponent<DialogueManager>(); //대사 시스템을 위한 스크립트
    //     //게임에 대한 전반적인 정보를 가지고 있는 스크립트. ex. 현재 게임의 엔딩 번호, 이벤트 번호
    //     gameInfo = GetComponent<GameInfo>();
    // }
    // public void StartInteraction(GameObject gameObject)
    // {


    //     Item interaction_Item = gameObject.GetComponent<Item>();

    //     if (interaction_Item != null)
    //     {
    //         Debug.Log(interaction_Item.preEndingNum);
    //         Debug.Log(gameInfo.EndingNum);



    //         if ((interaction_Item.preEndingNum != gameInfo.EndingNum) || (interaction_Item.preEventNum != gameInfo.EventNum))
    //         {
    //             interaction_Item.preEndingNum = gameInfo.EndingNum;
    //             interaction_Item.preEventNum = gameInfo.EventNum;
    //             interaction_Item.dialogueNum = 1;
    //         }

    //         player_InteractingTrue(); //플레이어 캐릭터가 상호작용 못하도록 제한.
    //         Debug.Log(interaction_Item.Name);


    //         //1 01 0001 01 
    //         //엔딩, npc id, 이벤트id, 대사단락번호 
    //         int id = 0;
    //         string id_String = "";

    //         id_String += gameInfo.EndingNum.ToString();
    //         id_String += gameInfo.LineNum.ToString();

    //         if (interaction_Item.id.ToString().Length == 1)
    //             id_String += "0" + interaction_Item.id.ToString();
    //         else
    //             id_String += interaction_Item.id.ToString();


    //         if (gameInfo.EventNum.ToString().Length == 1)
    //             id_String += "000" + gameInfo.EventNum.ToString();
    //         else if (gameInfo.EventNum.ToString().Length == 2)
    //             id_String += "00" + gameInfo.EventNum.ToString();
    //         else if (gameInfo.EventNum.ToString().Length == 3)
    //             id_String += "0" + gameInfo.EventNum.ToString();
    //         else
    //             id_String += gameInfo.EventNum.ToString();


    //         if (interaction_Item.dialogueNum.ToString().Length == 1)
    //             id_String += "0" + interaction_Item.dialogueNum.ToString();
    //         else
    //             id_String += interaction_Item.dialogueNum.ToString();

    //         id = int.Parse(id_String);


    //         Debug.Log(id.ToString());

    //         if (interaction_Item.isNpc)
    //         {
    //             //상조작용이 가능한 NPC인 경우
    //             dialogueManager.Action_NPC(id, interaction_Item);
    //         }

    //     }

    // }

    // public void player_InteractingTrue()
    // {
    //     //플레이어의 상호작용을 막는다.
    //     //playerController.interacting = true;

    // }
    // public void player_InteractingFalse()
    // {
    //     //플레이어가 다시 상호작용할 수 있도록 풀어준다.
    //     //playerController.interacting = false;

    // }



    public void StartInteraction(GameObject gameObject)
    {
        Item interaction_Item = gameObject.GetComponent<Item>();

        if (interaction_Item != null)
        {
            Debug.Log(interaction_Item.preEndingNum);
            Debug.Log(GameManager.Instance.gameInfo.EndingNum);



            if ((interaction_Item.preEndingNum != GameManager.Instance.gameInfo.EndingNum) || (interaction_Item.preEventNum != GameManager.Instance.gameInfo.EventNum))
            {
                interaction_Item.preEndingNum = GameManager.Instance.gameInfo.EndingNum;
                interaction_Item.preEventNum = GameManager.Instance.gameInfo.EventNum;
                interaction_Item.dialogueNum = 1;
            }

            //player_InteractingTrue(); //플레이어 캐릭터가 상호작용 못하도록 제한.
            Debug.Log(interaction_Item.Name);


            //1 01 0001 01 
            //엔딩, npc id, 이벤트id, 대사단락번호 
            int id = 0;
            string id_String = "";

            id_String += GameManager.Instance.gameInfo.EndingNum.ToString();
            id_String += GameManager.Instance.gameInfo.LineNum.ToString();

            if (interaction_Item.id.ToString().Length == 1)
                id_String += "0" + interaction_Item.id.ToString();
            else
                id_String += interaction_Item.id.ToString();


            if (GameManager.Instance.gameInfo.EventNum.ToString().Length == 1)
                id_String += "000" + GameManager.Instance.gameInfo.EventNum.ToString();
            else if (GameManager.Instance.gameInfo.EventNum.ToString().Length == 2)
                id_String += "00" + GameManager.Instance.gameInfo.EventNum.ToString();
            else if (GameManager.Instance.gameInfo.EventNum.ToString().Length == 3)
                id_String += "0" + GameManager.Instance.gameInfo.EventNum.ToString();
            else
                id_String += GameManager.Instance.gameInfo.EventNum.ToString();


            if (interaction_Item.dialogueNum.ToString().Length == 1)
                id_String += "0" + interaction_Item.dialogueNum.ToString();
            else
                id_String += interaction_Item.dialogueNum.ToString();

            id = int.Parse(id_String);


            Debug.Log(id.ToString());

            if (interaction_Item.isNpc)
            {
                //상조작용이 가능한 NPC인 경우
                GameManager.Instance.dialogueManager.Action_NPC(id, interaction_Item);
                //GameManager.GetInstance().dialogueManager.Action_NPC(id, interaction_Item);
            }

        }

    }
}