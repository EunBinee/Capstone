using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPattern_Boss : MonsterPattern
{
    // * 보스 몬스터 : 패턴
    public enum BossMonsterPhase
    {
        Phase1,
        Phase2,
        Phase3,
        Death
    }

    public enum BossMonsterAttackAnimation
    {
        ResetAttackAnim,
        Skill01,
        Skill02,
        Skill03,
        Skill04
    }
    public enum BossMonsterMotion
    {
        Skill01,
        Skill02,
        Skill03,
        Skill04,
        GetHit,
        Death
    }

    protected BossMonsterPhase curBossPhase;
    protected float Phase1_BossHP = 0;
    protected float Phase2_BossHP = 0;
    protected float Phase3_BossHP = 0;


    //* 약점--------------------------------------------------------------//
    protected bool enableBossWeakness = false;
    public int curRemainWeaknessesNum = 0; // 남은 약점 수
    //*-------------------------------------------------------------------//
    public override void useUpdate()
    {
        base.useUpdate();

        BossWeaknessUpdate();
    }

    public virtual void SetBossAttackAnimation(BossMonsterAttackAnimation bossMonsterAttackAnimation, int animIndex = 0)
    {
        switch (bossMonsterAttackAnimation)
        {
            case BossMonsterAttackAnimation.ResetAttackAnim:
                break;
            case BossMonsterAttackAnimation.Skill01:
                switch (animIndex)
                {
                    case 0:
                        //점프
                        break;
                    case 1:
                        //착지
                        break;
                    default:
                        break;
                }
                break;
            default:

                break;
        }
    }

    public virtual void Monster_Motion(BossMonsterMotion monsterMotion)
    {
        switch (monsterMotion)
        {
            case BossMonsterMotion.Skill01:
                break;
            case BossMonsterMotion.Skill02:
                break;
            case BossMonsterMotion.Skill03:
                break;
            case BossMonsterMotion.Skill04:
                break;
            case BossMonsterMotion.GetHit:
                break;
            case BossMonsterMotion.Death:
                break;
            default:
                break;
        }
    }

    public virtual void ChangeBossPhase(BossMonsterPhase bossMonsterPhase, bool production = true)
    {
        curBossPhase = bossMonsterPhase;

        switch (curBossPhase)
        {
            case BossMonsterPhase.Phase1:
                break;
            case BossMonsterPhase.Phase2:
                break;
            case BossMonsterPhase.Phase3:
                break;
        }
    }
    public BossMonsterPhase GetBossMonsterPhase()
    {
        return curBossPhase;
    }

    protected void CheckBossHP()
    {
        Phase1_BossHP = (float)m_monster.monsterData.MaxHP;
        Phase2_BossHP = (float)(m_monster.monsterData.MaxHP * 0.3f);
        //Phase3_BossHP = (float)(m_monster.monsterData.MaxHP * 0.2f);
    }

    public virtual void Base_Phase_HP(bool production = true)
    {
        //HP로 나누는 페이즈

    }

    //*-----------------------------------------------------------------------------//
    //* 보스 약점
    bool isLastWeakness = false;
    bool showWeaknessEffect = false; //이펙트 활성화 여부
    //bool finishEffect = false;


    bool OnEnableWeakness = false;

    public virtual void BossWeaknessUpdate()
    {
        if (curRemainWeaknessesNum != 0)
        {
            if (m_monster.monsterData.useWeakness && curBossPhase != BossMonsterPhase.Phase1)
            {
                if (!OnEnableWeakness)
                {
                    OnEnableWeakness = true;
                    EnableBossWeakness(true);
                }
                if ((playerController._currentState.isAim && m_monster.monsterData.useWeakness) && !showWeaknessEffect)
                {
                    EnableBossWeaknessEffect(true);
                }
                else if ((!playerController._currentState.isAim && m_monster.monsterData.useWeakness) && showWeaknessEffect)
                {
                    EnableBossWeaknessEffect(false);
                }
            }
        }
    }

    //* 보스 약점 활성화
    protected void EnableBossWeakness(bool enableWeakness)
    {
        if (!isLastWeakness)
        {
            for (int i = 0; i < m_monster.monsterData.weaknessList.Count; i++)
            {
                BossWeakness bossWeakness = m_monster.monsterData.weaknessList[i].GetComponent<BossWeakness>();
                if (bossWeakness.m_monster == null)
                    bossWeakness.SetMonster(m_monster);

                if (!bossWeakness.destroy_BossWeakness)
                {
                    //* 아직 공격당하지않은 약점이라면? => 활성화
                    bossWeakness.gameObject.SetActive(enableWeakness);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_monster.monsterData.lastWeaknessList.Count; i++)
            {
                BossWeakness bossWeakness = m_monster.monsterData.lastWeaknessList[i].GetComponent<BossWeakness>();

                if (bossWeakness.m_monster == null)
                    bossWeakness.SetMonster(m_monster);

                if (!bossWeakness.destroy_BossWeakness)
                {
                    //* 아직 공격당하지않은 약점이라면? => 활성화
                    bossWeakness.gameObject.SetActive(enableWeakness);
                }
            }
        }
    }

    //* 보스 약점 이펙트 ( 플레이어 화살이 살아있으면, 조준 끝나도 이펙트만 사라지고 콜라이더는 그대로 있도록. )
    protected void EnableBossWeaknessEffect(bool enableWeaknessEffect)
    {
        showWeaknessEffect = enableWeaknessEffect;
        if (!isLastWeakness)
        {
            for (int i = 0; i < m_monster.monsterData.weaknessList.Count; i++)
            {
                BossWeakness bossWeakness = m_monster.monsterData.weaknessList[i].GetComponent<BossWeakness>();

                if (!bossWeakness.destroy_BossWeakness)
                {
                    //* 아직 공격당하지않은 약점이라면? => 활성화
                    bossWeakness.bossWeaknessEffect.gameObject.SetActive(enableWeaknessEffect);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_monster.monsterData.lastWeaknessList.Count; i++)
            {
                BossWeakness bossWeakness = m_monster.monsterData.lastWeaknessList[i].GetComponent<BossWeakness>();

                if (!bossWeakness.destroy_BossWeakness)
                {
                    //* 아직 공격당하지않은 약점이라면? => 활성화
                    bossWeakness.bossWeaknessEffect.gameObject.SetActive(enableWeaknessEffect);
                }
            }
        }
    }

    //* 남은 약점 없애기
    public void ReduceRemainWeaknessesNum(BossWeakness bossWeakness)
    {
        curRemainWeaknessesNum--;
        bossWeakness.gameObject.SetActive(false);
        if (curRemainWeaknessesNum == 0)
        {
            if (m_monster.monsterData.haveLastWeakness && !isLastWeakness)
            {
                //! 보스 연출
                isLastWeakness = true;
                OnEnableWeakness = false;
                //DirectTheBossLastWeakness();
                ChangeBossPhase(BossMonsterPhase.Phase3);
                
            }
            else
            {
            }
        }
    }

    //* 보스 마지막 약점 연출
    public virtual void DirectTheBossLastWeakness()
    {

    }
    //* 보스 약점 연출
    public virtual void DirectTheBossWeakness()
    {

    }



}