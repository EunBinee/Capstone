using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
