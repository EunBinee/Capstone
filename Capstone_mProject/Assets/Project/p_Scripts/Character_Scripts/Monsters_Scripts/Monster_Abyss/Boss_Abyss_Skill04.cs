using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Abyss_Skill04 : MonoBehaviour
{
    //! 스킬 04  번개
    MonsterPattern_Boss_Abyss monsterPattern_Abyss;
    Monster m_monster;
    PlayerController playerController;
    PlayerMovement playerMovement;
    Transform playerTrans;

    [Header("스킬 04")]
    public bool skill04_ing = false;

    public GameObject targetMarker_Prefabs;
    public List<GameObject> targetMarkerList = new List<GameObject>();
    public GameObject targetMarker_Pattern05_Prefabs;
    GameObject targetMarker_Pattern05_obj;
    public List<Skill_Indicator> targetMarker_Pattern05_List = new List<Skill_Indicator>();

    int createTargetMarker = 0;
    float angle = 0;
    bool skillOver = false;
    int curRandomSkillPattern_num = 0;

    bool stopBoss = false;
    List<Skill_Indicator> curTargetMarker;
    Coroutine moveMonster_co = null;
    Coroutine skill04_Co = null;
    Coroutine skill04_Pattern04_Co = null;
    Coroutine CheckPlayerInMarker_co = null;

    bool stopSkillPattern = false;


    public void Init(MonsterPattern_Boss_Abyss _monsterPattern_Boss_Abyss)
    {
        monsterPattern_Abyss = _monsterPattern_Boss_Abyss;
        m_monster = monsterPattern_Abyss.m_monster;
        playerController = GameManager.instance.gameData.GetPlayerController();
        playerMovement = GameManager.instance.gameData.GetPlayerMovement();
        playerTrans = GameManager.instance.gameData.GetPlayerTransform();
    }

    public void Skill04()
    {
        skill04_ing = true;
        curTargetMarker = new List<Skill_Indicator>();
        skill04_Co = StartCoroutine(BossAbyss_Skill04());
    }

    public void SettingSkill04Pattern(int patternNum)
    {
        //* 패턴세팅
        switch (patternNum)
        {
            case 1:
                createTargetMarker = 8;
                angle = 360 / createTargetMarker;
                break;
            case 2:
                createTargetMarker = 9;
                angle = 360 / createTargetMarker;
                break;
            case 3:
                createTargetMarker = 10;
                angle = 360 / createTargetMarker;

                break;
            case 4:
                createTargetMarker = 19;
                angle = 20;
                break;
            case 5:
                break;
            default:
                break;
        }
    }
    IEnumerator BossMove()
    {
        //Vector3 randomPos = monsterPattern_Abyss.GetRandomPos(10f, monsterPattern_Abyss.GetGroundPos(transform));
        Vector3 originPos = monsterPattern_Abyss.GetOriginPosition();
        NavMeshHit hit;

        float time = 0;
        bool moveMonster = false;
        while (time < 1)
        {
            time += Time.deltaTime;
            if (NavMesh.SamplePosition(originPos, out hit, 20f, NavMesh.AllAreas))
            {
                if (originPos != hit.position)
                {
                    originPos = hit.position;
                }
                moveMonster = true;
                break;
            }
            yield return null;
        }

        time = 0;

        if (moveMonster)
        {
            monsterPattern_Abyss.SetMove_AI(true);
            monsterPattern_Abyss.navMeshAgent.SetDestination(originPos);
            monsterPattern_Abyss.SetAnimation(MonsterPattern.MonsterAnimation.Move);
            while (time < 5) //* 3초간 이동하거나 이미 도착한 경우 끝
            {
                time += Time.deltaTime;
                if (Vector3.Distance(originPos, transform.position) <= 2f)
                    break;
                yield return null;
            }

        }

        time = 0;
        monsterPattern_Abyss.SetMove_AI(false);
        monsterPattern_Abyss.SetAnimation(MonsterPattern.MonsterAnimation.Move);

        //몬스터 방향 플레이어 쪽으로 돌리기
        Vector3 direction = monsterPattern_Abyss.bossForward;
        Quaternion targetAngle = Quaternion.LookRotation(direction);

        while (time < 4)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, Time.deltaTime * 1f);

            if (Quaternion.Angle(transform.rotation, targetAngle) < 1.5f)
                break;

            yield return null;
        }
        transform.rotation = targetAngle;
        monsterPattern_Abyss.SetAnimation(MonsterPattern.MonsterAnimation.Idle);

        stopBoss = true;
        moveMonster_co = null;
    }

    IEnumerator BossAbyss_Skill04()
    {
        stopBoss = false;
        moveMonster_co = StartCoroutine(BossMove());
        yield return new WaitUntil(() => stopBoss == true);

        //* 5개의 패턴중 하나 랜덤으로 고름

        //보스 움직임 멈춤.
        monsterPattern_Abyss.SetMove_AI(false);
        monsterPattern_Abyss.SetAnimation(MonsterPattern.MonsterAnimation.Idle);

        //*----------------------------------------------------------------------------------//

        GameManager.Instance.cameraController.cameraShake.ShakeCamera(1f, 3, 3);
        Effect effect = GameManager.Instance.objectPooling.ShowEffect("Smoke_Effect_03");
        Vector3 effectPos = transform.position;
        effectPos.y -= 1.5f;
        effect.transform.position = effectPos;

        yield return new WaitForSeconds(2f);
        //*-----------------------------------------------------------------------------------//
        int count = 0;
        int curIndex = -1;
        while (count < 4)
        {
            //curRandomSkillPattern_num = 4;

            while (true)
            {
                curRandomSkillPattern_num = UnityEngine.Random.Range(1, 5);

                if (curIndex != curRandomSkillPattern_num)
                {
                    //중복 패턴 없도록
                    curIndex = curRandomSkillPattern_num;
                    break;
                }
                yield return null;
            }

            SettingSkill04Pattern(curRandomSkillPattern_num);
            if (curRandomSkillPattern_num < 4)
            {
                //*1~3 번 패턴
                CreateTargetMarker();
            }
            else if (curRandomSkillPattern_num == 4)
            {
                //* 4번 패턴
                CreateTargetMarker_Pattern04();
            }
            else if (curRandomSkillPattern_num == 5)
            {
                //* 5번 패턴
                CreateTargetMarker_5();
            }
            yield return new WaitUntil(() => skillOver == true && curTargetMarker.Count == 0);
            count++;
            isTrigger_si = false;
        }

        //! 스킬 끝
        monsterPattern_Abyss.EndSkill(MonsterPattern_Boss.BossMonsterMotion.Skill04);
        skill04_Co = null;

        yield return null;
    }

    //* 패턴 01 ~ 03-----------------------------------------------------------------------------------//
    public void CreateTargetMarker()
    {
        float mAngle = UnityEngine.Random.Range(0, 160);
        skillOver = false;
        for (int i = 0; i < createTargetMarker; i++)
        {
            if (i > 0)
            {
                mAngle += angle;
            }

            StartCoroutine(SkillActivation(mAngle, i, true));
        }
    }

    //* 패턴 04-----------------------------------------------------------------------------------//

    public void CreateTargetMarker_Pattern04()
    {
        skill04_Pattern04_Co = StartCoroutine(CreateTargetMarker_4_co());
    }

    GameObject firstMarker = null;

    IEnumerator CreateTargetMarker_4_co()
    {
        bool playerMove_Direction = false;
        firstMarker = null;
        //* 오른쪽 왼쪽 체크
        bool playerLocation = monsterPattern_Abyss.PlayerLocationCheck_LeftRight(monsterPattern_Abyss.playerTargetPos.position, transform);
        float mAngle = 0;
        float monsterY = transform.rotation.eulerAngles.y;

        if (playerLocation)
        {
            //* 플레이어가 몬스터의 오른쪽
            mAngle = monsterY - 20 + GameManager.instance.GetAngleSeparation(transform.position, transform.forward * 20, playerController.gameObject.transform.position);
        }
        else
        {
            //* 몬스터의 왼쪽
            mAngle = monsterY + 160 + GameManager.instance.GetAngleSeparation(transform.position, -transform.forward * 20, playerController.gameObject.transform.position);
        }

        float time = 0;
        skillOver = false;

        for (int i = 0; i < createTargetMarker; i++)
        {

            if (i > 0)
            {
                if (i == 1)
                {
                    //플레이어 왼쪽 오른쪽 체크
                    yield return new WaitUntil(() => firstMarker != null);
                    //firstMarker를 기준으로 플레이어가 왼쪽에 있는지 오른쪽에 있는지 체크하고,
                    // 플레이어의 위치를 기준으로 angle을 더하거나뺀다.
                    playerMove_Direction = monsterPattern_Abyss.PlayerLocationCheck_LeftRight(monsterPattern_Abyss.playerTargetPos.position, firstMarker.transform);
                }
                if (playerMove_Direction)
                    mAngle += angle;
                else if (!playerMove_Direction)
                    mAngle -= angle;
            }
            StartCoroutine(SkillActivation_04(mAngle, i, false));

            while (true)
            {
                if (i >= (createTargetMarker - 1))
                    break;
                time += Time.deltaTime;

                if (time > 0.8f)
                {
                    time = 0;
                    break;
                }
                yield return null;
            }
        }
        skill04_Pattern04_Co = null;
        yield return null;
    }

    //* 패턴 05-----------------------------------------------------------------------------------//
    public void CreateTargetMarker_5()
    {
        skillOver = false;
        if (targetMarker_Pattern05_obj == null)
        {
            targetMarker_Pattern05_obj = Instantiate(targetMarker_Pattern05_Prefabs, GameManager.instance.transform.position, Quaternion.identity);
            targetMarker_Pattern05_obj.transform.SetParent(GameManager.instance.transform);

            Transform targetMarker_trans = targetMarker_Pattern05_obj.GetComponent<Transform>();
            foreach (Transform child in targetMarker_trans)
            {
                Skill_Indicator skill_Indicator = child.GetComponent<Skill_Indicator>();
                if (skill_Indicator != null)
                {
                    if (skill_Indicator.gameObject.activeSelf == true)
                        targetMarker_Pattern05_List.Add(skill_Indicator);
                }
            }
        }
        StartCoroutine(SkillActivation_Pattern05(-1));
    }

    //* Marker 한개의 코루틴----------------------------------------------------------------------------------//
    IEnumerator SkillActivation(float mAngle, int index = 0, bool simultaneous = true)
    {
        //스킬 targetMarkerList
        //* 타임 세팅---------------------------//
        float waitTime = 2;//빨간색 경고후 기다리는 시간
        float electricity_DurationTime = 4;//빨간색 경고후, 번개 친 후 지속 시간
        float endSkillTime = 3 + electricity_DurationTime; //스킬이 끝나는 시간
                                                           //---------------------------------------//
        GameObject skillIndicator_obj;
        float posY = monsterPattern_Abyss.GetGroundPos(transform).y;
        //* 오브젝트 풀링 ---------------------------------------------------------------------------------//
        if (targetMarkerList.Count == 0)
        {
            skillIndicator_obj = Instantiate(targetMarker_Prefabs, transform.position, Quaternion.identity);
            skillIndicator_obj.transform.SetParent(transform);
        }
        else
        {
            skillIndicator_obj = targetMarkerList[0];
            targetMarkerList.RemoveAt(0);
            skillIndicator_obj.SetActive(true);
        }

        //* 세팅-------------------------------------------------------------------------------------------//
        skillIndicator_obj.transform.position = new Vector3(transform.position.x, posY + 0.05f, transform.position.z);
        Quaternion originRotate = skillIndicator_obj.transform.rotation;
        Skill_Indicator skill_Indicator = skillIndicator_obj.GetComponent<Skill_Indicator>();
        skill_Indicator.Init();
        curTargetMarker.Add(skill_Indicator);

        skill_Indicator.SetBounds();
        skill_Indicator.SetAngle(mAngle);

        Quaternion rotation = Quaternion.Euler(0f, mAngle, 0f);
        skillIndicator_obj.transform.rotation = skillIndicator_obj.transform.rotation * rotation;
        //마커 불투명도 조절
        MeshRenderer meshRenderer = skillIndicator_obj.GetComponentInChildren<MeshRenderer>();
        StartCoroutine(FadeInMarker(meshRenderer, 1.0f)); // 1초 동안 불투명하게 만들기
 
        //*------------------------------------------------------------------------------------------------//
        yield return new WaitForSeconds(waitTime); //* 8초 뒤 체크

        //* 번개, 파지직 번개
        StartCoroutine(ElectricityProduction(skill_Indicator, electricity_DurationTime, mAngle, simultaneous));
        //monsterPattern_Abyss.m_monster.SoundPlay("electronic_01", true);
        yield return new WaitForSeconds(endSkillTime); //* 7초후 스킬 종료
        monsterPattern_Abyss.m_monster.SoundPlayStop("Boss_Skill04");
        if (!skillOver)
            skillOver = true;

        for (int i = 0; i < skill_Indicator.electricity_Effects.Count; ++i)
        {
            skill_Indicator.electricity_Effects[i].StopEffect();
        }

        yield return new WaitUntil(() => skill_Indicator.electricity_Effects.Count == 0);

        skill_Indicator.CheckTrigger(false);

        skill_Indicator.gameObject.transform.rotation = originRotate;

        curTargetMarker.Remove(skill_Indicator);
        //*------------------------------------------------------------//
        if (targetMarkerList.Count > 8)
        {
            Destroy(skillIndicator_obj);
        }
        else
        {
            targetMarkerList.Add(skillIndicator_obj);
            skillIndicator_obj.SetActive(false);
        }

        if (index == (createTargetMarker - 1))
        {
            stopSkillPattern = true;
        }

    }
    //*-----------------------------------------------------------------------------------------------------//
    IEnumerator SkillActivation_04(float mAngle, int index = 0, bool simultaneous = true)
    {
        //스킬 targetMarkerList
        //* 타임 세팅---------------------------//
        float waitTime = 2;//빨간색 경고후 기다리는 시간
        float electricity_DurationTime = 4;//빨간색 경고후, 번개 친 후 지속 시간
        float endSkillTime = 3 + electricity_DurationTime; //스킬이 끝나는 시간

        GameObject skillIndicator_obj;
        float posY = monsterPattern_Abyss.GetGroundPos(transform).y;
        //* 오브젝트 풀링 ---------------------------------------------------------------------------------//
        if (targetMarkerList.Count == 0)
        {
            skillIndicator_obj = Instantiate(targetMarker_Prefabs, transform.position, Quaternion.identity);
            skillIndicator_obj.transform.SetParent(transform);
        }
        else
        {
            skillIndicator_obj = targetMarkerList[0];
            targetMarkerList.RemoveAt(0);
            skillIndicator_obj.SetActive(true);
        }

        if (index == 0)
        {
            // 플레이어 위치 파악
            firstMarker = skillIndicator_obj;
        }
        //* 세팅-------------------------------------------------------------------------------------------//
        skillIndicator_obj.transform.position = new Vector3(transform.position.x, posY + 0.05f, transform.position.z);
        Quaternion originRotate = skillIndicator_obj.transform.rotation;

        Skill_Indicator skill_Indicator = skillIndicator_obj.GetComponent<Skill_Indicator>();
        skill_Indicator.Init();
        curTargetMarker.Add(skill_Indicator);

        skill_Indicator.SetBounds();
        skill_Indicator.SetAngle(mAngle);

        Quaternion rotation = Quaternion.Euler(0f, mAngle, 0f);
        skillIndicator_obj.transform.rotation = skillIndicator_obj.transform.rotation * rotation;

        //불투명도 조절
        MeshRenderer meshRenderer = skillIndicator_obj.GetComponentInChildren<MeshRenderer>();
        StartCoroutine(FadeInMarker(meshRenderer, 1.0f)); // 1초 동안 불투명하게 만들기
        //*------------------------------------------------------------------------------------------------//
        yield return new WaitForSeconds(waitTime);

        //* 번개, 파지직 번개
        StartCoroutine(ElectricityProduction(skill_Indicator, electricity_DurationTime, mAngle, simultaneous));
        

        yield return new WaitForSeconds(endSkillTime);
        monsterPattern_Abyss.m_monster.SoundPlayStop("Boss_Skill04");
        if (!skillOver)
            skillOver = true;

        for (int i = 0; i < skill_Indicator.electricity_Effects.Count; ++i)
        {
            skill_Indicator.electricity_Effects[i].StopEffect();
        }

        yield return new WaitUntil(() => skill_Indicator.electricity_Effects.Count == 0);

        skill_Indicator.CheckTrigger(false);

        skill_Indicator.gameObject.transform.rotation = originRotate;

        curTargetMarker.Remove(skill_Indicator);
        //*------------------------------------------------------------//
        if (targetMarkerList.Count > 8)
        {
            Destroy(skillIndicator_obj);
        }
        else
        {
            targetMarkerList.Add(skillIndicator_obj);
            skillIndicator_obj.SetActive(false);
        }

        if (index == (createTargetMarker - 1))
        {
            stopSkillPattern = true;
        }

    }

    //*-------------------------------------------------------------------------------------------------------//
    //* ### 체크문양 패턴
    IEnumerator SkillActivation_Pattern05(float mAngle)
    {
        //스킬 targetMarkerList
        //* 타임 세팅---------------------------//
        float waitTime = 2;//빨간색 경고후 기다리는 시간
        float electricity_DurationTime = 4;//빨간색 경고후, 번개 친 후 지속 시간
        float endSkillTime = 3 + electricity_DurationTime; //스킬이 끝나는 시간
                                                           //---------------------------------------//
        GameObject skillIndicator_obj;
        float posY = monsterPattern_Abyss.GetGroundPos(transform).y;
        //* 오브젝트 풀링 ---------------------------------------------------------------------------------//
        skillIndicator_obj = targetMarker_Pattern05_obj;
        //* 세팅-------------------------------------------------------------------------------------------//
        skillIndicator_obj.transform.position = new Vector3(transform.position.x, posY + 0.05f, transform.position.z);
        for (int i = 0; i < targetMarker_Pattern05_List.Count; ++i)
        {
            if (!targetMarker_Pattern05_List[i].gameObject.activeSelf)
                targetMarker_Pattern05_List[i].gameObject.SetActive(true);
            curTargetMarker.Add(targetMarker_Pattern05_List[i]);
        }
        //불투명도 조절
        MeshRenderer meshRenderer = skillIndicator_obj.GetComponentInChildren<MeshRenderer>();
        StartCoroutine(FadeInMarker(meshRenderer, 1.0f)); // 1초 동안 불투명하게 만들기
        //*------------------------------------------------------------------------------------------------//
        yield return new WaitForSeconds(waitTime); //* 8초 뒤 체크

        //* 번개, 파지직 번개
        for (int i = 0; i < targetMarker_Pattern05_List.Count; ++i)
        {
            StartCoroutine(ElectricityProduction(targetMarker_Pattern05_List[i], electricity_DurationTime, mAngle));
        }
         // monsterPattern_Abyss.m_monster.SoundPlay("Boss_Skill04", true);
        yield return new WaitForSeconds(endSkillTime); //* 7초후 종료
        monsterPattern_Abyss.m_monster.SoundPlayStop("Boss_Skill04");

        //* 스킬끝났음.----------------------------------------------//
        if (!skillOver)
            skillOver = true;

        for (int i = 0; i < targetMarker_Pattern05_List.Count; ++i)
        {
            for (int j = 0; j < targetMarker_Pattern05_List[i].electricity_Effects.Count; ++j)
            {
                targetMarker_Pattern05_List[i].electricity_Effects[j].StopEffect();
            }
        }
        for (int i = 0; i < targetMarker_Pattern05_List.Count; ++i)
        {
            yield return new WaitUntil(() => targetMarker_Pattern05_List[i].electricity_Effects.Count == 0);
        }

        for (int i = 0; i < targetMarker_Pattern05_List.Count; ++i)
        {
            targetMarker_Pattern05_List[i].CheckTrigger(false);
            curTargetMarker.Remove(targetMarker_Pattern05_List[i]);
        }

        skillIndicator_obj.SetActive(false);

        stopSkillPattern = true;
    }

    bool isTrigger_si = false; // 동시 체크 트리거
    IEnumerator ElectricityProduction(Skill_Indicator skill_Indicator, float duration, float angle, bool simultaneous = true)
    {
        float durationTime = duration / 3;
        int count = 0;
        float time = 0;
        Vector3 randomPos = Vector3.zero;
        bool isTrigger = false;
        bool getBounds = false;


        float createEffectStopTime = 0.02f;

        while (time < 2f)
        {
            time += Time.deltaTime;

            if (angle == -1)
                getBounds = true;
            randomPos = skill_Indicator.GetRandomPos(getBounds);
            Effect effect = GameManager.Instance.objectPooling.ShowEffect("LightningStrike2_red", skill_Indicator.gameObject.transform);
            effect.transform.position = randomPos;
            
            if (!isTrigger_si)
            {
                if (time > 1f && !isTrigger)
                {
                    if (simultaneous)
                        SimultaneousCheck();
                    else
                    {
                        isTrigger = true;
                        skill_Indicator.CheckTrigger(true); // 트리거 ON
                        if (CheckPlayerInMarker_co == null)
                        {
                            CheckPlayerInMarker_co = StartCoroutine(CheckPlayerInMarker());
                        }
                    }
                }
            }
            if (angle != -1)
            {
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                effect.transform.localPosition = rotation * effect.transform.localPosition;
            }
monsterPattern_Abyss.m_monster.SoundPlay("Boss_Skill04");
            yield return new WaitForSeconds(createEffectStopTime);
            time += createEffectStopTime;
//monsterPattern_Abyss.m_monster.SoundPlayStop("electronic_01");
            yield return new WaitUntil(() => UIManager.gameIsPaused == false);
        }

        time = 0;
        while (count < 3)
        {
            while (true)
            {
                if (time < durationTime)
                {
                    time += Time.deltaTime;
                    if (angle == -1)
                        getBounds = true;
                    randomPos = skill_Indicator.GetRandomPos(getBounds);

                    Effect effect = monsterPattern_Abyss.GetDamage_electricity(randomPos, skill_Indicator.gameObject.transform, angle);
                    effect.finishAction = () =>
                    {
                        skill_Indicator.electricity_Effects.Remove(effect);
                    };
                    skill_Indicator.electricity_Effects.Add(effect);

                    yield return new WaitForSeconds(createEffectStopTime);
                    time += createEffectStopTime;

                    yield return new WaitUntil(() => UIManager.gameIsPaused == false);
                }
                else
                {
                    count++;
                    time = 0;
                    break;
                }
            }
        }

        yield return null;
    }

    private void SimultaneousCheck()
    {
        isTrigger_si = true;
        for (int i = 0; i < curTargetMarker.Count; i++)
        {
            curTargetMarker[i].CheckTrigger(true); // 트리거 ON
        }
        if (CheckPlayerInMarker_co == null)
        {

            CheckPlayerInMarker_co = StartCoroutine(CheckPlayerInMarker());
        }
    }


    IEnumerator CheckPlayerInMarker()
    {
        LayerMask layerMask = LayerMask.GetMask("Monster");
        int count = 0;
        stopSkillPattern = false;

        while (!stopSkillPattern)
        {

            if (!playerController._currentState.isElectricShock)
            {
                count = 0;
                // 플레이어의 위치와 방향에서 아래로 레이케스트를 쏴서 오브젝트를 탐지
                Collider[] overlappedColliders = Physics.OverlapSphere(playerController.gameObject.transform.position, 0.3f, layerMask);
                foreach (Collider hit in overlappedColliders)
                {
                    Skill_Indicator skill_Indicator = hit.gameObject.gameObject.GetComponent<Skill_Indicator>();
                    if (skill_Indicator != null)
                    {
                        if (skill_Indicator.checkTrigger)
                        {
                            count++;
                        }
                    }
                }

                if (count > 0)
                {
                    //* 만약 플레이어가 indicator에 플레이어가 포함 되어있다면?
                    playerMovement.PlayerElectricShock(true);
                    float damage = 20 * count;
                    m_monster.OnHit(damage);
                }
            }

            yield return null;
        }

        CheckPlayerInMarker_co = null;
    }


    //* 스킬 04번 정지--------------------------------------------------------------//
    public void Stop_MonsterSkill04()
    {
        if (moveMonster_co != null)
        {
            StopCoroutine(moveMonster_co);
            moveMonster_co = null;
        }
        stopBoss = false;
        if (skill04_Co != null)
        {
            StopCoroutine(skill04_Co);
            skill04_Co = null;
        }
        if (skill04_Pattern04_Co != null)
        {
            StopCoroutine(skill04_Pattern04_Co);
            skill04_Pattern04_Co = null;
        }
        if (CheckPlayerInMarker_co != null)
        {
            StopCoroutine(CheckPlayerInMarker_co);
            CheckPlayerInMarker_co = null;
        }

        if (stopSkillPattern == true)
            stopSkillPattern = false;

        if (curTargetMarker.Count != 0)
        {
            foreach (Skill_Indicator skill in curTargetMarker)
            {

                skill.gameObject.SetActive(false);
            }
        }

        monsterPattern_Abyss.EndSkill(MonsterPattern_Boss.BossMonsterMotion.Skill04);

    }
    
    //* 타겟마커 불투명도 조절
    IEnumerator FadeInMarker(MeshRenderer renderer, float duration)
    {
        Color color = renderer.material.color;
        float startAlpha = 0f; // 투명하게 시작
        float endAlpha = 1f; // 불투명하게 설정
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration); // 알파 값을 점차 증가
            color.a = alpha;
            renderer.material.color = color;
            yield return null;
        }

        // 최종적으로 알파 값을 완전 불투명으로 설정
        color.a = endAlpha;
        renderer.material.color = color;
    }

}
