using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    //    public Dialogue[] Parse(string _CSVFileName)
    //    {
    //        List<Dialogue> dialogueList = new List<Dialogue>(); //��ȭ ����Ʈ ����
    //        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName); //csv���� ������

    //        string[] data = csvData.text.Split(new char[] { '\n' }); //���� �������� �ɰ�.

    //        for (int i = 1; i < data.Length;)
    //        {

    //            string[] row = data[i].Split(new char[] { ',' });

    //            Dialogue dialogue = new Dialogue(); //��� ����Ʈ ����

    //            dialogue.name = row[3]; //ĳ���� �̸�
    //            dialogue.skipnum = row[7]; //��ŵ��ȣ
    //            dialogue.endingNum = row[9]; //������ȣ

    //            List<string> contextList = new List<string>();

    //            if (row[4].ToString() == "1") //������ ���ΰ� 1�̸� ������ ����. 
    //            {
    //                dialogue.isChoice = true;
    //            }
    //            else
    //            {
    //                dialogue.isChoice = false;
    //            }

    //            do
    //            {
    //                Debug.Log(row[4]);
    //                if (row[3] != "") //��簡 ������ �ƴϸ� �߰� 
    //                {
    //                    contextList.Add(row[4]); //���
    //                }

    //                if (++i < data.Length)
    //                {
    //                    row = data[i].Split(new char[] { ',' });
    //                }
    //                else
    //                {
    //                    break;
    //                }
    //                if (row[5].ToString() != "") //���ô�簡 ������ �ƴϸ� �߰�
    //                {
    //                    contextList.Add(row[6]);
    //                }

    //            } while (row[2].ToString() == ""); //�����̸� ���� ��� �߰�

    //            dialogue.contexts = contextList.ToArray();
    //            dialogueList.Add(dialogue);

    //        }

    //        return dialogueList.ToArray();
    //    }
    //}

    
    int eventNum_in = 0;
    int npcNum_in = 0;
    int dialogueNum_in = 0;  //E��
    int endingNum_in = 0;
    int lineNum_in = 0;

    //Line.cs�� ����
    string name_in;
    int choice_OneTwo = 0;
    bool startChoice = false;
    bool finishBreak = false;

    void Initialization()
    {
        eventNum_in = 0;
        npcNum_in = 0;
        dialogueNum_in = 0;
        endingNum_in = 0;
        lineNum_in = 0;

        name_in = "";
        choice_OneTwo = 0;
        startChoice = false;
        finishBreak = false;
    }
    public Dialogue[] DialogueParse(string csvFileName)
    {
        Initialization();

        List<Dialogue> dialogues = new List<Dialogue>();

        TextAsset csvData = Resources.Load<TextAsset>(csvFileName);
        string[] data = csvData.text.Split(new char[] { '\n' });

        for (int i = 1; i < (data.Length);)
        {
            string[] row = data[i].Split(new char[] { ',' });

            Dialogue dialogue = new Dialogue();
            List<List<Line>> lines = new List<List<Line>>();

            if (row[1].ToString() == "")
            {
                dialogue.eventNum = eventNum_in;   //���� ����ǰ��ִ� �̺�Ʈ��ȣ
                dialogue.npcNum = npcNum_in;     //Npc ��ȣ
                dialogue.endingNum = endingNum_in;  //ending��ȣ
                dialogue.lineNum = lineNum_in;
            }
            else
            {

                eventNum_in = int.Parse(row[1].ToString());
                npcNum_in = int.Parse(row[2].ToString());
                if (row[11] == "")
                    endingNum_in = endingNum_in;
                else
                    endingNum_in = int.Parse(row[11].ToString());
                lineNum_in= int.Parse(row[4].ToString());

                dialogue.eventNum = eventNum_in;   //���� ����ǰ��ִ� �̺�Ʈ
                dialogue.npcNum = npcNum_in;     //Npc ��ȣ
                dialogue.endingNum = endingNum_in;  //ending��ȣ
                dialogue.lineNum = lineNum_in;
            }

            if (row[3].ToString() == "")
            {
                dialogue.dialogueNum = dialogueNum_in;
            }
            else
            {
                dialogueNum_in = int.Parse(row[3].ToString());
                dialogue.dialogueNum = dialogueNum_in;
            }
            if (row[4].ToString() == "")
            {
                dialogue.lineNum = lineNum_in;
            }
            else
            {
                lineNum_in = int.Parse(row[4].ToString());
                dialogue.lineNum = lineNum_in;
            }
            if (row[5].ToString() == "")
            {
                dialogue.lineNum = lineNum_in;
            }
            else
            {
                lineNum_in = int.Parse(row[4].ToString());
                dialogue.lineNum = lineNum_in;
            }
            do
            {
                List<Line> LineList = new List<Line>();
                List<string> contextList = new List<string>();  //Line.cs�� context[]�� �ֱ� ����

                do
                {
                    Line line = new Line();

                    //���� �ʱ�ȭ
                    line.isChoice = false;
                    line.isFinishLine = false;  //��ȭ�� �������� ����    
                    line.nextDialogueNum = dialogueNum_in;
                    line.nextLineNum=lineNum_in;

                    do
                    {
                        if (!startChoice) //�������� ���� ���
                        {
                            if (row[5].ToString() != "")
                            {
                                name_in = row[5].ToString(); //ĳ���� �̸� 
                            }

                            line.Name = name_in;
                            contextList.Add(row[6].ToString());

                        }
                        else if (startChoice) //�������� ���� ���
                        {
                            choice_OneTwo++;

                            line.isChoice = true;
                            if (choice_OneTwo == 1)
                            {
                                //ù��° ��������
                                line.choice = new Choice();
                                line.choice.firstOption = row[8].ToString();
                                line.choice.firstOptDialogNum = int.Parse(row[9].ToString());

                            }
                            else if (choice_OneTwo == 2)
                            {
                                //�ι�° ��������
                                line.choice.secondOption = row[8].ToString();
                                line.choice.secondOptDialogNum = int.Parse(row[9].ToString());

                                choice_OneTwo = 0;
                                startChoice = false;
                            }
                        }

                        if (row[7].ToString() != "")
                        {
                            if (int.Parse(row[7].ToString()) == 1)
                            {
                                //�������� �ִ� ���
                                startChoice = true;
                                line.isChoice = true;
                            }
                            else if (int.Parse(row[7].ToString()) == 0)
                            {
                                line.isFinishLine = true;
                                //���� ����� ���� 
                                int nextDialogueNum = 0;
                                bool isNumeric = int.TryParse(row[3].ToString(), out nextDialogueNum);


                                if (isNumeric) //���� ���� ��ȯ�� �����ϴٸ�
                                {
                                    line.nextDialogueNum = nextDialogueNum;
                                    //Debug.Log(nextDialogueNum);
                                }
                                if (!isNumeric && row[3].ToString() == "")
                                {
                                    //���� �ܶ����� 
                                    line.nextDialogueNum = dialogueNum_in+1;

                                }
                         

                                //��簡 ������ Evnet�� ��ȭ�� �ִ����� Ȯ��
                                if (!line.changeEvnetID) //false�϶��� ���� �ѹ� 
                                {
                                    int changeEvnetID = 0;
                                    isNumeric = int.TryParse(row[12].ToString(), out changeEvnetID);

                                    if (isNumeric) //���� ���� ��ȯ�� �����ϴٸ�
                                    {
                                        line.changeEvnetID = true;
                                        line.evnetIDToBeChange = changeEvnetID;
                                        Debug.Log(line.evnetIDToBeChange);
                                    }
                                }
                                //��簡 ������ Ending�� ��ȭ�� �ִ����� Ȯ��
                                if (!line.changeEndingID)
                                {
                                    int changeEndingID = 0;
                                    isNumeric = int.TryParse(row[11].ToString(), out changeEndingID);

                                    if (isNumeric) //���� ���� ��ȯ�� �����ϴٸ�
                                    {
                                        line.changeEndingID = true;
                                        line.endingIDToBeChange = changeEndingID;
                                    }
                                }

                            }
                        }
                                                
                        //-----------------------------------------------------------
                        //���⼭ i�� ++ ����
                        if (++i < (data.Length))
                        {
                            row = data[i].Split(new char[] { ',' });
                        }
                        else
                        {
                            finishBreak = true;
                            break;
                        }
                    } while (row[5].ToString() == "" );    //�̸��� ��������� ��� ��簡 �̾���.

                    line.context = contextList.ToArray();
                    LineList.Add(line);

                    contextList.Clear();


                    if (finishBreak)
                    {
                        break;
                    }


                } while (row[4].ToString() == "");    //csv E��.

                lines.Add(LineList);

                if (finishBreak)
                {
                    break;
                }
            } while (row[3].ToString() == "");    //csv D�� 


            dialogue.lines = lines;
            dialogues.Add(dialogue);
        }
        return dialogues.ToArray();
    }

}
