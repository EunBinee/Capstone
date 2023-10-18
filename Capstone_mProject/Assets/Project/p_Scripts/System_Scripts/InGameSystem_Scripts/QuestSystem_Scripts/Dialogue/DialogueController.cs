using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{

    [SerializeField] TMP_Text objectText;


    bool startChat = false;
    public bool stopChat = false;


    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startChat)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //EnterŰ�� ������ �ִϸ��̼� �����ϰ�, �ٷ� �۾� �������� �ϱ� ����.
                stopChat = true;
            }
        }
    }



    //Ÿ���� �ִϸ��̼�
    public void Chat_Obect(string sentence)
    {
        startChat = true;
        StartCoroutine(ObjectChat(sentence));
    }


    IEnumerator ObjectChat(string sentence)
    {


        string writerText = "";

        for (int i = 0; i < sentence.Length; i++)
        {
            if (stopChat)
            {
                //EnterŰ�� ������ �ִϸ��̼� �����ϰ�, �ٷ� �۾� ��������.
                writerText = sentence;
                objectText.text = writerText;
                break;
            }
            else
            {
                writerText += sentence[i];
                objectText.text = writerText;


                yield return new WaitForSeconds(0.07f);
            }
        }

        //��� ��簡 Ÿ���� �ƴٴ� ���� �˷�����.
        dialogueManager.endChat_inController = true;
        startChat = false;
        stopChat = false;

    }

}
