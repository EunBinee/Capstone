using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    
    static public DatabaseManager instance;

    public string csvFileName_NPC;

    //npc�� ����� ���°����
    public Dictionary<int, Dialogue> NPC_diaglogues_Dictionary = new Dictionary<int, Dialogue>();

    //List<int> idList01 = new List<int>(); //Start()���� Debug�Ҷ� ���
    //List<int> idList02 = new List<int>();
    int eventID;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //NPC Dialogue
            DialogueParser(csvFileName_NPC, true);

        }
    }
    //void Start()
    //{
    //    //������ �Ľ��� �ߵƴ���  Debug.Log��
    //    for (int i = 0; i < idList01.Count; i++)
    //    {
    //        int id = idList01[i];
    //        string name = "";
    //        Dialogue dialogue = NPC_diaglogues_Dictionary[id];
    //        Debug.Log(id.ToString());

    //        List<List<Line>> lines = dialogue.lines;


    //        for (int j = 0; j < lines.Count; j++)
    //        {
    //            for (int k = 0; k < lines[j].Count; k++)
    //            {

    //                for (int l = 0; l < lines[j][k].context.Length; l++)
    //                {
    //                    Debug.Log("[" + j + "] [" + k + "] Context" + l);
    //                    if (name != lines[j][k].Name)
    //                    {
    //                        name = lines[j][k].Name;

    //                        Debug.Log(name);
    //                    }

    //                    Debug.Log(lines[j][k].context[l]);

    //                    //���� �������� ������ �ִٸ�..
    //                    if (l == (lines[j][k].context.Length - 1))
    //                    {
    //                        if (lines[j][k].isChoice)
    //                        {
    //                            Debug.Log("������");
    //                            Debug.Log(lines[j][k].choice.firstOption);
    //                            Debug.Log(lines[j][k].choice.firstOptDialogNum.ToString());
    //                            Debug.Log(lines[j][k].choice.secondOption);
    //                            Debug.Log(lines[j][k].choice.secondOptDialogNum.ToString());
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    static public DatabaseManager GetInstance()
    {
        return instance;
    }


    public void DialogueParser(string csvFileName, bool isNPC)
    {

        //NPC
        DialogueParser dialogueParser = GetComponent<DialogueParser>();
        Dialogue[] dialogues = dialogueParser.DialogueParse(csvFileName);


        for (int i = 0; i < dialogues.Length; i++)
        {
            //1 01 0001 01 
            //����, npc id, �̺�Ʈid, ���ܶ���ȣ 
            int id = 0;
            string id_String = "";

            id_String += dialogues[i].endingNum.ToString();
            id_String += dialogues[i].lineNum.ToString();

            if (dialogues[i].npcNum.ToString().Length == 1)
                id_String += "0" + dialogues[i].npcNum.ToString();
            else
                id_String += dialogues[i].npcNum.ToString();


            if (dialogues[i].eventNum.ToString().Length == 1)
                id_String += "000" + dialogues[i].eventNum.ToString();
            else if (dialogues[i].eventNum.ToString().Length == 2)
                id_String += "00" + dialogues[i].eventNum.ToString();
            else if (dialogues[i].eventNum.ToString().Length == 3)
                id_String += "0" + dialogues[i].eventNum.ToString();
            else
                id_String += dialogues[i].eventNum.ToString();


            if (dialogues[i].dialogueNum.ToString().Length == 1)
                id_String += "0" + dialogues[i].dialogueNum.ToString();
            else
                id_String += dialogues[i].dialogueNum.ToString();

            id = int.Parse(id_String);


            if (isNPC)
            {
                //idList01.Add(id);   //Start()���� Debug�Ҷ� ���
                NPC_diaglogues_Dictionary[id] = dialogues[i];
            }

        }
        Debug.Log(csvFileName + "�Ϸ�!");
    }



}