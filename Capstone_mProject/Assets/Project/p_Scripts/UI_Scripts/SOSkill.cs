using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOSkill : ScriptableObject
{
    public float damage;
    public float cool;
    public bool isTwice;

    public string animationName;
    public Sprite icon;

    public string skillName; //스킬 이름
    public string skillDetail; //스킬 설명 
}