using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    //[SerializeField]
    //private int endingNum; //���� ��ȣ

    //[SerializeField]
    //private int eventNum; //�̺�Ʈ ��ȣ

    //public int EndingNum
    //{
    //    get { return endingNum; }
    //    set { endingNum = value; }
    //}
    //public int EventNum
    //{
    //    get { return eventNum; }
    //    set { eventNum = value; }
    //}

    //2������========================================================
    //���� ����
    [SerializeField]
    private int endingNum;

    //�̺�Ʈ ��ȣ
    [SerializeField]
    private int eventNum;


    public int EndingNum
    {
        get { return endingNum; }
        set { endingNum = value; }
    }

    public int EventNum
    {
        get { return eventNum; }
        set { eventNum = value; }
    }
    //public int LineNum
    //{
    //    get { return eventNum; }
    //    set { eventNum = value; }
    //}
}
