using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Dialogue
//{
//    [Tooltip("��� ġ�� ĳ���� �̸�")]
//    public string name;

//    [Tooltip("��� ����")]
//    public string[] contexts;

//    public bool isChoice;
//    public string skipnum;
//    public string endingNum;
//    public string firstChoice; //ù��° ������
//    public string secondChoice; //�ι�° ������
//}

//[System.Serializable]
//public class DialogueEvent
//{
//    public string name; //�̺�Ʈ �̸�
//    public bool isChoice; //������ ����
//    public string endingNum; //������ȣ

//    public Vector2 line;
//    public Dialogue[] dialogues;
//}

//1������========================================================
//[System.Serializable]
//public class Line
//{
//    //��縦 ġ�� �ι�
//    public string Name;
//    //��� ����
//    public string[] context;

//    //�������� �ִ��� ����
//    public bool isChoice;
//    public Choice choice;

//    //��ȭ�� �������� ����    
//    public bool isFinishLine;
//    public int nextDialogueNum; //�������� ���� ���

//    //��ȭ ������ �̺�Ʈ ��ȣ ���� 
//    public bool changeEvent;
//    public int changeEventID; //����� �̺�Ʈ ID


//    //��ȭ�� ������ ����Ʈ ����
//    //public bool isQuest;
//    //public int QuestId;

//    //���� ����
//    public bool changeEnding;
//    public int changeEndingID; //����� ���� ID

//}


//[System.Serializable]
//public class Dialogue
//{
//    //csv F���� ��������.. ��������.   
//    public List<List<Line>> lines;
//    //lines[i][j] => lines[i]    F�� ����
//    //lines[i][j] => lines[i][g] G�� ����

//    //�̺�Ʈ ��ȣ
//    public int eventNum;
//    //NPC ID = ��� ��ȣ
//    public int dialogueNum;
//    //����Ʈ ��ȣ
//    //public string questNum;
//    //���� ��ȣ
//    public int endingNum;
//}

//[System.Serializable]
//public class Choice
//{
//    public string firstOption; //1��° �������� Text
//    public string secondOption; //2��° �������� Text

//    public int firstSkipDialogNum; //1��° �������� �������� ��� �� ���� ��� g��
//    public int secondSkipDialogNum;//2��° �������� �������� ��� �� ���� ���

//    public bool firstOpenQuest; //������ ��ȭ�� ��ġ�� ������Ʈ�� ������ϴ��� 
//    public string firstQuestName; //������ϴ� ������Ʈ�� �̸�. ���⿡ ���߿� ����Ʈ �߰��ҵ�?

//    public bool secondOpenQuest; //������ ��ȭ�� ��ġ�� ���̵��� �̷�������ϴ��� 
//    public string secondQuestName; //������ϴ� ������Ʈ�� �̸�. 
//}


//2������========================================================
[System.Serializable]
public class Line
{
    public string Name; //npc �̸�
    public string[] context; //����

    public bool isChoice;//������ ����
    public Choice choice;

    public bool isFinishLine;  //��ȭ�� �������� ����    
    public int nextDialogueNum; //�����ٸ�, �� ��� ���� ���(���ܶ�).
    
    public bool changeEvnetID; //��ȭ�� ������ �̺�Ʈ��ȣ�� ������ �ִ��� ����
    public int evnetIDToBeChange; //����� �̺�Ʈ ID

    public bool changeEndingID; //��ȭ�� ������ ending ������ �ִ��� ����
    public int endingIDToBeChange; //����� ���� ID

    public int nextLineNum;
}


[System.Serializable]
public class Dialogue
{
    public List<List<Line>> lines;

    //���� 
    public int endingNum; //L��
    //NPC��ȣ
    public int npcNum; //C��
    //�̺�Ʈ ��ȣ
    public int eventNum; //B��
    //��� ������ ��ȣ
    public int dialogueNum; //D��

    public int lineNum;

}

[System.Serializable]
public class Choice
{
    public string firstOption; //1��° �������� Text
    public string secondOption; //2��° �������� Text

    public int firstOptDialogNum; //1��° �������� �������� ���, �״��� ��� ��ȣ
    public int secondOptDialogNum;//2��° �������� �������� ���, �״��� ��� ��ȣ

}