using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Item : MonoBehaviour
{
    //public string Name; //Npc �̸�
    //public bool isNpc;
    //public int eventNum; //�̺�Ʈ id

    ////ending�̳� Event�� ���� ��츦 üũ�ϱ� ����
    //public int preEventNum;
    //public int preEndingNum;


    public string Name; //Npc �̸�
    public int id; //Npc ID

    public bool isNpc;

    //��� ������ ��ȣ
    public int dialogueNum; //��ȭ�ܶ�, ó���� �׻� 1

   //public int lineNum;
    //ending�̳� Event�� ���� ��츦 üũ�ϱ� ����
    //--> �����̳� �̺�Ʈ�� ���� ���, DialogueNum�� �ٽ� 1�� �Ǿ�� �Ѵ�.
    // �װ��� üũ�ϱ� ����.
    public int preEventNum;
    public int preEndingNum;

}
