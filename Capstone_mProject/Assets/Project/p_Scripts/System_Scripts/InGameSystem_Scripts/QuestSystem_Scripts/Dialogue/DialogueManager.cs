using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //[SerializeField] GameObject go_DialogueBar;
    //[SerializeField] GameObject go_ChoiceBar;

    //[SerializeField] TextMeshProUGUI Text_Name;
    //[SerializeField] TextMeshProUGUI Text_Dialogue;

    //[SerializeField] Button FirstChoice;
    //[SerializeField] Button SecondChoice;
    //[SerializeField] Text Text_FirstChoice;
    //[SerializeField] Text Text_SecondChoice;

    //[Header("�ؽ�Ʈ ��� ������")]
    //[SerializeField] float textDelay;

    //Dialogue[] dialogues;

    //NpcAreaController theNC;

    //bool isDialogue = false; //��ȭ���� ��� true
    //bool isNext = false; //Ư�� Ű �Է� ���

    //int lineCount = 0; //��ȭ ī��Ʈ 
    //int contextCount = 0; //��� ī��Ʈ 


    //private void Update()
    //{
    //    if (isDialogue)
    //    {
    //        if (isNext)
    //        {
    //            if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                isNext = false;
    //                Text_Dialogue.text = "";
    //                if (++contextCount < dialogues[lineCount].contexts.Length)
    //                {
    //                    StartCoroutine(TypeWriter());
    //                }
    //                else
    //                {
    //                    contextCount = 0;
    //                    if (++lineCount < dialogues.Length)
    //                    {
    //                        StartCoroutine(TypeWriter());
    //                    }
    //                    else
    //                    {
    //                        EndDialogue();
    //                    }
    //                }


    //            }
    //        }
    //    }
    //}
    //public void ShowDialogue(Dialogue[] p_dialogues) //��ȭ ���� ���̰�
    //{
    //    isDialogue = true; //��ȭ����
    //    Text_Dialogue.text = "";
    //    Text_Name.text = "";
    //    //theNC.SettingUI(false);
    //    dialogues = p_dialogues;

    //    StartCoroutine(TypeWriter());
    //}

    //public void ShowChoice(Dialogue[] p_dialogues) //��ȭ ���� ���̰�
    //{

    //    Text_FirstChoice.text = "";
    //    Text_SecondChoice.text = "";
    //    go_ChoiceBar.SetActive(true);


    //}

    //void EndDialogue()
    //{
    //    isDialogue = false; //��ȭ ��
    //    contextCount = 0;
    //    lineCount = 0;
    //    dialogues = null;
    //    isNext = false;
    //    //theNC.SettingUI(true);
    //    SettingUI(false);

    //}
    //void SettingUI(bool p_flag)
    //{
    //    go_DialogueBar.SetActive(p_flag); //��ȭ â ���̰� 

    //}

    //IEnumerator TypeWriter()
    //{
    //    SettingUI(true);

    //    string t_ReplaceText = dialogues[lineCount].contexts[contextCount];
    //    t_ReplaceText = t_ReplaceText.Replace("'", ","); // '�� ,�� �ٲ����Բ�.
    //    t_ReplaceText = t_ReplaceText.Replace("\\n", "\n"); // \n�� �ٹٲ����� �ٲ����Բ�.

    //    Text_Name.text = dialogues[lineCount].name; //�̸� �����ְ�.
    //    for (int i = 0; i < t_ReplaceText.Length; i++)
    //    {
    //        Text_Dialogue.text += t_ReplaceText[i];
    //        yield return new WaitForSeconds(textDelay);
    //    }

    //    isNext = true;
    //    yield return null;
    //}


    //1������========================================================

    //GameInfo gameInfo;

    //[SerializeField] GameObject go_DialogueBar; //��ȭ ui
    //[SerializeField] GameObject go_ChoiceBar; //������ ui

    //[SerializeField] TextMeshProUGUI Text_Name; //�̸� �ؽ�Ʈ
    //[SerializeField] TextMeshProUGUI Text_Dialogue; //��ȭ �ؽ�Ʈ

    //[SerializeField] GameObject FirstChoice;
    //[SerializeField] GameObject SecondChoice;
    //[SerializeField] Text Text_FirstChoice;
    //[SerializeField] Text Text_SecondChoice;

    //private void Start()
    //{
    //    gameInfo = GetComponent<GameInfo>();
    //}
    //public void Action_Npc(int id, Item interaction_Item)
    //{
    //    Debug.Log("Action Script NPC: " + id);  //������
    //    Dialogue dialogue = DatabaseManager.GetInstance().NPC_diaglogues_Dictionary[id];
    //    StartCoroutine(StartObjectTextBox(dialogue, interaction_Item));
    //}

    //IEnumerator StartObjectTextBox(Dialogue dialogue, Item interaction_Item)
    //{
    //    go_DialogueBar.SetActive(true); //��ȭâ ui ���̰� ��.
    //    Text_Name.text = "";
    //    Text_Dialogue.text = "";

    //    bool AllFinish = false; //��� ��簡 ��������

    //    yield return new WaitForSeconds(0.5f);

    //    int curPart = 0; //Dialogue.cs�� lines[curPart][curLine]  B��
    //    int curLine = 0; // ĳ���� �̸� D��
    //    int curContext = 0; //lines[curPart][curLine].context[curContext] ��� E��

    //    bool isFinish = false; //

    //    bool isChoice = false; //������ ����
    //    bool choiceSettingF = false; //������ ������ ������ ������ ��ư���� �ؽ�Ʈ ��ȯ�� ������ ���´���
    //    bool ClickChoiceBtn = true; //�������� ������ ���, �ٷ� EnterŰ ���� �ٷ� ���� ���� �Ѿ���� �����ϴ� bool��


    //    int curlineContextLen;  //���� dialogue.line[curline]�� line���� context�� ����


    //    bool choice_OpenObject = false; //��ȭ�� ����ġ�� ���ο� ���� �������ϴ��� ���� //ex. ����Ʈ
    //    string openObjectName = ""; //� ��, ������Ʈ�� ������ϴ���, string���� ����

    //    //ending�̳� Ending�� ��ȭ�� �Ҵ���
    //    bool changeEvnetID = false;
    //    int eventIDToBeChange = 0; //������Ʈ�� �̺�Ʈ ID
    //    bool changeEndingID = false;
    //    int endingIDToBeChange = 0; //������Ʈ�� ���� ID

    //    string line = ""; //���

    //    while (!AllFinish)
    //    {
    //        curlineContextLen = dialogue.lines[curPart][curLine].context.Length;

    //        if (curContext < curlineContextLen)
    //        {
    //            //���� ���� �ȳ����ٸ�
    //            if (ClickChoiceBtn)
    //            {
    //                //������ ���� ����
    //                Text_Dialogue.text = "";
    //                Text_Name.text = "";

    //                curContext++;
    //                ClickChoiceBtn = false;

    //            }
    //            else if (Input.GetKeyDown(KeyCode.Space))
    //            {
    //                //�������� ���ٸ�
    //                Text_Dialogue.text = "";
    //                Text_Name.text = "";

    //                Text_Name.text = dialogue.lines[curPart][curLine].Name;
    //                line = dialogue.lines[curPart][curLine].context[curContext];

    //                curContext++;
    //            }
    //        }

    //        if (curContext == curlineContextLen) //������ context�� ������ ������ ���� ���
    //        {
    //            isChoice = dialogue.lines[curPart][curLine].isChoice;
    //            isFinish = dialogue.lines[curPart][curLine].isFinishLine;

    //            if (isChoice)
    //            {
    //                //��簡 ������ �������� �ִ� ���
    //                if (!choiceSettingF)
    //                {
    //                    //������ ��ư Ȱ��ȭ
    //                    FirstChoice.SetActive(true);
    //                    SecondChoice.SetActive(true);

    //                    int firstOptSkipLine = dialogue.lines[curPart][curLine].choice.firstSkipDialogNum;
    //                    int secondOptSkipLine = dialogue.lines[curPart][curLine].choice.secondSkipDialogNum;

    //                    bool firstChoice_OpenObject = dialogue.lines[curPart][curLine].choice.firstOpenQuest;
    //                    string firstOpenObjectName = dialogue.lines[curPart][curLine].choice.firstQuestName;
    //                    bool secondChoice_OpenObject = dialogue.lines[curPart][curLine].choice.secondOpenQuest;
    //                    string secondOpenObjectName = dialogue.lines[curPart][curLine].choice.secondQuestName;

    //                    Text_FirstChoice.text = dialogue.lines[curPart][curLine].choice.firstOption;
    //                    Text_SecondChoice.text = dialogue.lines[curPart][curLine].choice.secondOption;

    //                    Button btn01 = FirstChoice.GetComponent<Button>();
    //                    btn01.onClick.RemoveAllListeners();

    //                    btn01.onClick.AddListener(() =>
    //                    {
    //                        if (!Input.GetKeyDown(KeyCode.Space))
    //                        {
    //                            curPart = (firstOptSkipLine - 1);
    //                            curLine = 0;
    //                            curContext = 0;
    //                            FirstChoice.SetActive(false);
    //                            SecondChoice.SetActive(!false);

    //                            if (!choice_OpenObject)
    //                            {
    //                                choice_OpenObject = firstChoice_OpenObject;
    //                                openObjectName = firstOpenObjectName;
    //                            }
    //                            choiceSettingF = false;
    //                            ClickChoiceBtn = true;
    //                        }
    //                    });

    //                    Button btn02 = FirstChoice.GetComponent<Button>();
    //                    btn02.onClick.RemoveAllListeners();

    //                    btn02.onClick.AddListener(() =>
    //                    {
    //                        if (!Input.GetKeyDown(KeyCode.Space))
    //                        {
    //                            curPart = (firstOptSkipLine - 1);
    //                            curLine = 0;
    //                            curContext = 0;
    //                            FirstChoice.SetActive(false);
    //                            SecondChoice.SetActive(!false);

    //                            if (!choice_OpenObject)
    //                            {
    //                                choice_OpenObject = firstChoice_OpenObject;
    //                                openObjectName = firstOpenObjectName;
    //                            }
    //                            choiceSettingF = false;
    //                            ClickChoiceBtn = true;
    //                        }
    //                    });

    //                    choiceSettingF = true;
    //                }
    //            }
    //            else if (!isChoice && isFinish)
    //            {
    //                int nextDialogueNum = dialogue.lines[curPart][curLine].nextDialogueNum;
    //                interaction_Item.eventNum = nextDialogueNum;

    //                bool eventID = dialogue.lines[curPart][curLine].changeEvent;
    //                if (eventID) //������ �̺�Ʈ�� ���� ���
    //                {
    //                    changeEndingID = eventID;
    //                    if (changeEndingID)
    //                    {
    //                        eventIDToBeChange = dialogue.lines[curPart][curLine].changeEventID;
    //                    }
    //                }

    //                bool endingID = dialogue.lines[curPart][curLine].changeEnding;
    //                if (endingID)
    //                {
    //                    changeEndingID = endingID;
    //                    if (changeEndingID)
    //                    {
    //                        endingIDToBeChange = dialogue.lines[curPart][curLine].changeEndingID;
    //                    }
    //                }
    //                if (Input.GetKeyDown(KeyCode.Space))
    //                {
    //                    AllFinish = true;
    //                }
    //            }

    //            else if (!isChoice && !isFinish)
    //            {
    //                curLine++;
    //                curContext = 0;
    //                ClickChoiceBtn = false;
    //            }
    //        }

    //    }
    //    if (choice_OpenObject)
    //    {
    //        //���ÿ� ���ؼ� �� �� �ٸ��� �������ϴ� ���
    //    }
    //    if (changeEndingID)
    //    {
    //        gameInfo.EndingNum = endingIDToBeChange;

    //    }

    //    go_DialogueBar.SetActive(false);
    //    //GameManager.GetInstance().player_InteractingFalse(); //�÷��̾� ��ȣ�ۿ� ���
    //}


    //2������========================================================
    DialogueController dialogueController;
    GameInfo gameInfo; //������ �������� ������ ���� ��ũ��Ʈ
    public GameObject go_DialogueBar; //��� UI
    public TMP_Text Text_Dialogue; //��� �ؽ�Ʈ
    public TMP_Text Text_Name; //�̸� �ؽ�Ʈ
    public GameObject ObjectTextBox_Button01; //��ư 1�� ������Ʈ
    public Text Text_Btn01; //��ư 1�� �ؽ�Ʈ
    public GameObject ObjectTextBox_Button02; //��ư 2�� ������Ʈ
    public Text Text_Btn02; //��ư 2�� �ؽ�Ʈ

    public bool endChat_inController = false;  //ChatController���� Chat �ִϸ��̼��� ��������, Ȯ�ο�.

    void Start()
    {
        dialogueController = GetComponent<DialogueController>();
        gameInfo = GetComponent<GameInfo>();
    }

    public void Action_NPC(int id, Item interaction_Item)
    {
        //NPC�� ��縦 ������ �´�.
        //DBManager.GetInstance().NPC_diaglogues_Dictionary[id]�� ���ؼ� ���� id�� �´� Dialogue�� ������ �´�.
        Dialogue dialogue = DatabaseManager.GetInstance().NPC_diaglogues_Dictionary[id];
        StartCoroutine(StartObjectTextBox(dialogue, interaction_Item));
    }


    IEnumerator StartObjectTextBox(Dialogue dialogue, Item interaction_Item)
    {
        //�ؽ�Ʈ�� �����ִ� �ڷ�ƾ

        go_DialogueBar.SetActive(true); //��� UI Ȱ��ȭ
        Text_Dialogue.text = "";
        Text_Name.text = "";
        bool AllFinish = false; //��� ��簡 �������� Ȯ�ο�

        int curPart = 0; //Dialogue.cs�� lines[curPart][curLine] => lines[curPart]   
        int curLine = 0; //lines[curPart][curPart] 
        int curContext = 0; //lines[curPart][curLine].context[curContext] 

        bool isFinish = false; //��簡 ����. ALLFinish���� �ٸ�
        //�״��� ���� ��� ���� ��������, ��縦 �� ��,���� ���͸� ġ�� �ʾƼ� ���� ������ �������� ���� ����

        bool isChoice = false; // �������� ������ �ִ���
        bool choiceSettingF = false; //������ ������ ������ ������ ��ư���� �ؽ�Ʈ ��ȯ�� ������ ���´���
        bool ClickChoiceBtn = true; //�������� ������ ���, �ٷ� EnterŰ ���� �ٷ� ���� ���� �Ѿ���� �����ϴ� bool��


        int curlineContextLen;  //���� dialogue.line[curline]�� line���� context�� ����


        //ending�̳� Ending�� ��ȭ�� �Ҵ���
        bool changeEvnetID = false;
        int eventIDToBeChange = 0; //������Ʈ�� �̺�Ʈ ID
        bool changeEndingID = false;
        int endingIDToBeChange = 0; //������Ʈ�� ���� ID

        string line = ""; //���

        endChat_inController = true; //ChatController���� Chat �ִϸ��̼��� ��������, Ȯ�ο�.

        while (!AllFinish) 
        {
            curlineContextLen = dialogue.lines[curPart][curLine].context.Length; //���� ����� �迭 ����

            if (curContext < curlineContextLen)
            {
                //���� ������ ������ �ʾҴٸ�..
                if (ClickChoiceBtn)
                {
                    //�������� ���� �� ���ĸ�?
                    Text_Dialogue.text = "";
                    Text_Name.text = "";
                    endChat_inController = false; //chat �ִϸ��̼� Ȯ�ο�.

                    Text_Name.text = dialogue.lines[curPart][curLine].Name;
                    line = dialogue.lines[curPart][curLine].context[curContext];

                    dialogueController.Chat_Obect(line);
                    curContext++;
                    ClickChoiceBtn = false;

                    //Debug.Log("d");

                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    //������ ���� ����, ���� ������ ������ �ʾҴٸ�..
                    Text_Dialogue.text = "";
                    Text_Name.text = "";
                    endChat_inController = false;

                    Text_Name.text = dialogue.lines[curPart][curLine].Name;

                    line = dialogue.lines[curPart][curLine].context[curContext];
                    dialogueController.Chat_Obect(line);
                    curContext++;

                    //Debug.Log("d1");

                }
            }

            yield return new WaitUntil(() => endChat_inController == true);

            //������ context�� ������ ������ ���� ��� Ȯ���ϱ�
            if (curContext == curlineContextLen)
            {
                //������ context�� ������ ������ ���� ��, ��ȭ�� ���� ������, �������� �ִ��� Ȯ��

                isChoice = dialogue.lines[curPart][curLine].isChoice;
                isFinish = dialogue.lines[curPart][curLine].isFinishLine;

                if (isChoice)
                {
                    //���� ��簡 ������ �������� �ִ� ���
                    if (!choiceSettingF)
                    {
                        Debug.Log("ischoice Ʈ��");
                        //1. �������� ��ư���� ��Ȱ��ȭ -> Ȱ��ȭ
                        ObjectTextBox_Button01.SetActive(true);
                        ObjectTextBox_Button02.SetActive(true);

                        //�ɼ��� ������ ���� ���� ����
                        int firstOptDialogPart = dialogue.lines[curPart][curLine].choice.firstOptDialogNum;
                        int secondOptDialogPart = dialogue.lines[curPart][curLine].choice.secondOptDialogNum;


                        //�������� ����.
                        Text_Btn01.text = dialogue.lines[curPart][curLine].choice.firstOption;
                        Text_Btn02.text = dialogue.lines[curPart][curLine].choice.secondOption;

                        //��ư�ȿ� ���빰 �־���.
                        Button btn01 = ObjectTextBox_Button01.GetComponent<Button>();
                        btn01.onClick.RemoveAllListeners();
                        //AddListener�� �Լ��� ����� �־��� �� ������.. �������� ��� curPart�� ���ؾ��ϱ⿡..
                        //���ٸ� �̿��ؼ� �͸��Լ��� ������־���.
                        btn01.onClick.AddListener(() =>
                        {
                            if (!Input.GetKeyDown(KeyCode.Return))// ���� ������ ������ �ʾҴٸ�..
                            {
                                curPart = (firstOptDialogPart - 1); //curPart�� �������� �Ѿ��.
                                curLine = 0;
                                curContext = 0;
                                ObjectTextBox_Button01.SetActive(false);
                                ObjectTextBox_Button02.SetActive(false);

                                choiceSettingF = false;
                                ClickChoiceBtn = true;
                            }

                        });

                        Button btn02 = ObjectTextBox_Button02.GetComponent<Button>();
                        btn02.onClick.RemoveAllListeners();
                        btn02.onClick.AddListener(() =>
                        {
                            if (!Input.GetKeyDown(KeyCode.Return))
                            {
                                curPart = (secondOptDialogPart - 1);
                                curLine = 0;
                                curContext = 0;
                                ObjectTextBox_Button01.SetActive(false);
                                ObjectTextBox_Button02.SetActive(false);

                                choiceSettingF = false;
                                ClickChoiceBtn = true;
                            }
                        });


                        choiceSettingF = true;

                    }

                }


                else if (!isChoice && isFinish)
                {
                    //�������� ���� ������ ������ ���� ���

                    //�� ����� ���� � ��縦 ó������
                    int nextDialogueNum = dialogue.lines[curPart][curLine].nextDialogueNum;
                    interaction_Item.dialogueNum = nextDialogueNum;
 
                    // ���� ��ȣ�ۿ��ϰ� �ִ� ������Ʈ�� Item�� �� ���� ������Ʈ ���ش�.
                    bool eventID = dialogue.lines[curPart][curLine].changeEvnetID; //���� Event�� �����ؾ��ϴ� ��..
                    if (eventID) //������ �̺�Ʈ�� ���� ��쿡�� ����
                    {
                        changeEvnetID = eventID;
                        if (changeEvnetID)
                        {
                            interaction_Item.dialogueNum = 1;
                            eventIDToBeChange = dialogue.lines[curPart][curLine].evnetIDToBeChange;
                        }
                        Debug.Log("�̺�Ʈ ��ȭ");

                    }

                    bool endingID = dialogue.lines[curPart][curLine].changeEndingID;
                    if (endingID)
                    {
                        changeEndingID = endingID;
                        if (changeEndingID)
                        {
                            interaction_Item.dialogueNum = 1;
                            endingIDToBeChange = dialogue.lines[curPart][curLine].endingIDToBeChange;
                        }
                        Debug.Log("���� ��ȭ");
                    }


                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        AllFinish = true;
                        Debug.Log("d3");
                    }
                }

                else if (!isChoice && !isFinish)
                {
                    //�������� ����, ������ ������ ���� ��찡 �ƴ� �ڿ� �ٸ� ����� ��簡 �� �ִ� ���
                    //��� �̾�����.
                    curLine++;
                    curContext = 0;
                    ClickChoiceBtn = false;

                    Debug.Log("��� �̾������ϴµ�..");
                }
            }
        }

        //������ ��ȭ�� �ִ���
        if (changeEndingID)
        {
            gameInfo.EndingNum = endingIDToBeChange;
            int nextEventNum = dialogue.lines[curPart][curLine].evnetIDToBeChange;
            interaction_Item.preEventNum = nextEventNum;

            Debug.Log(nextEventNum);
        }

        //�̺�Ʈ ID�� ��ȭ�� �ִ���
        if (changeEvnetID)
        {
            gameInfo.EventNum = eventIDToBeChange;
        }
        go_DialogueBar.SetActive(false); //��ȭ ������Ʈ�� ��Ȱ��ȭ ��Ų��.
        GameManager.GetInstance().player_InteractingFalse();  //�÷��̾ ������ �� �ֵ��� ��ȣ�ۿ�ٽ� ���
    
    }

}

