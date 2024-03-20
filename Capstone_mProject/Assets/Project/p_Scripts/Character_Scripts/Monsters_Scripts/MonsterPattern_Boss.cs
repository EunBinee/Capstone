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

    protected bool enableBossWeakness = false;

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

    protected void CheckBossHP()
    {
        Phase1_BossHP = (float)m_monster.monsterData.MaxHP;
        Phase2_BossHP = (float)(m_monster.monsterData.MaxHP * 0.7f);
        Phase3_BossHP = (float)(m_monster.monsterData.MaxHP * 0.2f);
    }

    public virtual void Base_Phase_HP(bool production = true)
    {
        //HP로 나누는 페이즈

    }

    //*-----------------------------------------------------------------------------//
    //* 보스 약점
    bool showWeaknessEffect = true; //이펙트 활성화 여부
    bool finishEffect = false;
    public virtual void BossWeaknessUpdate()
    {
        if (m_monster.monsterData.useWeakness && curBossPhase != BossMonsterPhase.Phase1)
        {
            if ((playerController._currentState.isAim && m_monster.monsterData.useWeakness) && !enableBossWeakness)
            {
                //* 플레이어가 화살을 조준, 그리고 몬스터가 약점을 가지고 있을 경우.
                //* 보스의 약점이 활성화 되어있지않다면?
                EnableBossWeakness(true); //약점 보여주기
            }
            else if (!playerController._currentState.isAim && finishEffect)
            {
                //* 플레이어 조준이 꺼졌는데, 보스 몬스터의 약점이 켜져있는 경우.
                if (playerMovement.playerArrowList.Count == 0)
                    EnableBossWeakness(false); // 약점 끄기
                else if (showWeaknessEffect)
                {
                    //* 조준은 끝났지만, 아직 날아가는 회살이 있을 경우
                    //* 이펙트만 꺼줌
                    EnableBossWeaknessEffect(false);
                }
            }
        }
    }

    //* 보스 약점 활성화
    protected void EnableBossWeakness(bool enableWeakness)
    {
        finishEffect = enableWeakness;
        enableBossWeakness = enableWeakness;

        for (int i = 0; i < m_monster.monsterData.weakness.Count; i++)
        {
            BossWeakness bossWeakness = m_monster.monsterData.weakness[i].GetComponent<BossWeakness>();

            if (bossWeakness.m_monster == null)
                bossWeakness.SetMonster(m_monster);

            if (!bossWeakness.destroy_BossWeakness)
            {
                //* 아직 공격당하지않은 약점이라면? => 활성화
                bossWeakness.gameObject.SetActive(enableWeakness);
            }
        }

        if (enableWeakness)
        {
            if (!showWeaknessEffect)
                EnableBossWeaknessEffect(true);
        }

    }

    protected void EnableBossWeaknessEffect(bool enableWeaknessEffect)
    {
        if (!enableWeaknessEffect)
            enableBossWeakness = enableWeaknessEffect;

        showWeaknessEffect = enableWeaknessEffect;

        for (int i = 0; i < m_monster.monsterData.weakness.Count; i++)
        {
            BossWeakness bossWeakness = m_monster.monsterData.weakness[i].GetComponent<BossWeakness>();

            if (!bossWeakness.destroy_BossWeakness)
            {
                //* 아직 공격당하지않은 약점이라면? => 활성화

                bossWeakness.bossWeaknessEffect.gameObject.SetActive(enableWeaknessEffect);
            }
        }
    }

}
