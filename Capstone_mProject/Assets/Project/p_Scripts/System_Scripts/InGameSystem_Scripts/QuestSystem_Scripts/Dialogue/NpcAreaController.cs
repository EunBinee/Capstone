using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAreaController : MonoBehaviour
{
    DialogueManager theDM; 
    GameInfo gameInfo;

    GameObject interObject;
    Item interaction_Item; 
    private void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        gameInfo=GetComponent<GameInfo>();
        interObject = gameObject;
        interaction_Item = gameObject.GetComponent<Item>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") //�÷��̾ ���� ��ȭâ Ȱ��ȭ
        {
            //theDM.ShowDialogue(gameObject.transform.GetComponent<InterectionEvent>().GetDialogue
            
        }
    }

    public void SettingUI(bool p_flag)
    {
        //��ȭâ ��Ȱ��ȭ false => �ٸ� ui, Ŀ���� ��Ȱ��ȭ
        //��ȭâ Ȱ��ȭ true => �ٸ� ui, Ŀ���� Ȱ��ȭ

        //���߿� �ڵ� �߰��ؾ���. 
    }

   
}
