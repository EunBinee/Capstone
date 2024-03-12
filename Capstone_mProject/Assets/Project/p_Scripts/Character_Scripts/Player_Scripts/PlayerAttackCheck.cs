using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerAttackCheck : MonoBehaviour
{
    public bool isEnable = false;
    [SerializeField] private Monster monster;
    private Rigidbody rigid;

    public PlayerController _playerController;// = new PlayerController();
    public PlayerController P_Controller => _playerController;
    private CurrentValue P_Value => _playerController._currentValue;
    private CurrentState P_States => _playerController._currentState;

    // HashSet을 사용하여 이미 처리된 몬스터를 추적합니다.
    HashSet<GameObject> seenMonsters = new HashSet<GameObject>();

    private GameObject player;
    private bool isArrow = false;
    private bool goShoot = false;
    private bool incoArrow = false;
    Vector3 dir = Vector3.zero;
    Transform nowArrow;
    public float deltaShootTime = 0.0f;

    //계산식
    //bool attackEnemy = false;

    void Start()
    {
        player = GameManager.Instance.gameData.player;
        _playerController = player.GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody>();
        if (this.gameObject.tag == "Arrow")  //* 화살인지 확인을 해
        {
            isArrow = true;
        }
        _playerController.hitMonsters.Clear();
    }
    void FixedUpdate()
    {
        if (_playerController.hitMonsters.Count > 1)
            checkMon();

        if (isArrow && !goShoot && P_States.startAim)
        {
            transform.position = P_Controller.shootPoint.position;
            transform.rotation = P_Controller.shootPoint.rotation;
            //Debug.Log("[arrow test] if(isArrow && !goShoot && P_States.startAim)");
            if (!incoArrow)
                StartCoroutine(Arrowing());
        }
    }
    IEnumerator Arrowing()
    {
        //Debug.Log("[arrow test] IEnumerator Arrowing()");
        incoArrow = true;
        dir = Vector3.zero;
        yield return new WaitUntil(() => !P_States.isAim);  //* isAim이 거짓이 되면
        //if (!P_States.isAim)   
        {
            //Debug.Log("[arrow test] WaitUntil(() => !P_States.isAim)");
            transform.position = P_Controller.shootPoint.position;
            transform.rotation = P_Controller.shootPoint.rotation;
            //* 키네매틱 끄기
            GetComponent<Rigidbody>().isKinematic = false;
            if (dir == Vector3.zero)    //* 방향 지정
            {
                dir = GameManager.Instance.gameData.cameraObj.transform.forward;
            }
            rigid.velocity = dir.normalized * 55f; ; //* 발사
            goShoot = true;
            ArrowRay();
            //attackEnemy = false;
        }
        incoArrow = false;
        yield return new WaitUntil(() => P_States.hadAttack == true || shootDeltaTime() >= 5.0f);
        this.gameObject.SetActive(false);
        P_States.hadAttack = false;
        deltaShootTime = 0.0f;
        yield return null;
    }
    private float shootDeltaTime()
    {
        deltaShootTime = deltaShootTime + Time.deltaTime;
        return deltaShootTime;
    }

    private void isBouncingToFalse()
    {
        P_States.isBouncing = false;
        P_Value.maxHitScale = 1.2f;
        P_Value.minHitScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("[attack test]콜라이더 충돌");
        if (isEnable)
        {
            if (other.gameObject.tag == "Monster")
            {
                monster = other.GetComponentInParent<Monster>();

                if (monster == null)
                {
                    Debug.LogError("몬스터 : null");
                    return;
                }

                if (monster.monsterPattern.GetCurMonsterState() != MonsterPattern.MonsterState.Death)
                {
                    _playerController.hitMonsters.Add(other.gameObject);
                    //Debug.Log($"hit monster ,  curState  {monster.monsterPattern.GetCurMonsterState()}");
                    if (P_States.hadAttack == false || P_States.notSameMonster)
                    {
                        // Debug.Log("[attack test]몬스터 피격");
                        // 충돌한 객체의 Transform을 얻기
                        Transform collidedTransform = other.transform;
                        // 충돌 지점의 좌표를 얻기
                        Vector3 collisionPoint = other.ClosestPoint(transform.position);
                        Quaternion otherQuaternion = Quaternion.FromToRotation(Vector3.up, collisionPoint.normalized);
                        playerHitMonster(collisionPoint, otherQuaternion);
                        //사운드
                        SoundManager.Instance.Play_PlayerSound(SoundManager.PlayerSound.Hit, false);
                    }
                    else
                    {
                        //이미 한번 때린 상태
                        //todo: 때리기 전 몬스터와 현재 때린 몬스터가 같은지 확인하기
                        //Debug.Log("[attack test]P_States.hadAttack : " + P_States.hadAttack);
                        /*if (_playerController.hitMonsters.Count >= 2)
                        {
                            Debug.Log("[attack test] _playerController.hitMonsters.Count: " + _playerController.hitMonsters.Count);
                            for (int i = _playerController.hitMonsters.Count - 1; i > 1; i--)
                            {
                                GameObject curmon = _playerController.hitMonsters[i];
                                GameObject premon = _playerController.hitMonsters[i - 1];

                                if (curmon != premon && P_States.hasAttackSameMonster == false)   // 다음 꺼랑 비교해서 다르면
                                {
                                    P_States.notSameMonster = true;
                                    //P_States.hasAttackSameMonster = true;
                                    Debug.Log("[attack test]curmon != premon");
                                    // 충돌한 객체의 Transform을 얻기
                                    Transform collidedTransform = other.transform;
                                    // 충돌 지점의 좌표를 얻기
                                    Vector3 collisionPoint = other.ClosestPoint(transform.position);
                                    Quaternion otherQuaternion = Quaternion.FromToRotation(Vector3.up, collisionPoint.normalized);
                                    playerHitMonster(collisionPoint, otherQuaternion);
                                    //사운드
                                    SoundManager.Instance.Play_PlayerSound(SoundManager.PlayerSound.Hit, false);
                                    //return;
                                }
                                else if (curmon == premon)
                                {
                                    Debug.Log("[attack test]curmon == premon");
                                    P_States.notSameMonster = false;
                                    if (_playerController.hitMonsters.Count > 0)
                                        _playerController.hitMonsters.RemoveAt(i);
                                    //_playerController.hitMonsters.RemoveAt(i - 1);
                                    //return;
                                }
                            }
                        }*/
                    }

                }
                else
                {
                    //Debug.Log("[attack test]몬스터 상태 : " + monster.monsterPattern.GetCurMonsterState());
                }
            }
            else
            {
                //Debug.Log("[attack test]몬스터 아님 : " + other.gameObject.tag);
            }
        }

    }

    public void checkMon()
    {
        //Debug.Log("[attack test] _playerController.hitMonsters.Count: " + _playerController.hitMonsters.Count);
        // for (int i = _playerController.hitMonsters.Count - 1; i > 1; i--)
        // {
        //     GameObject curmon = _playerController.hitMonsters[i];
        //     GameObject premon = _playerController.hitMonsters[i - 1];

        //     if (curmon != premon)   //* 다음 꺼랑 비교해서 다르면
        //     {
        //         P_States.notSameMonster = true;
        //         P_States.hasAttackSameMonster = true;
        //     }
        //     else if (curmon == premon)  //* 다음 꺼랑 비교해서 같으면
        //     {
        //         P_States.notSameMonster = false;
        //         if (_playerController.hitMonsters.Count > 0)
        //             _playerController.hitMonsters.RemoveAt(i);  //* 삭제
        //     }
        // }


        // 리스트를 거꾸로 순회합니다. 이렇게 하는 이유는 리스트를 순회하면서 항목을 제거할 때 문제가 발생하지 않도록 하기 위함입니다.
        for (int i = _playerController.hitMonsters.Count - 1; i >= 0; i--)
        {
            GameObject curmon = _playerController.hitMonsters[i];

            if (seenMonsters.Contains(curmon))
            {
                //P_States.notSameMonster = false;
                // 이미 처리된 몬스터이면 리스트에서 제거합니다.
                _playerController.hitMonsters.RemoveAt(i);
            }
            else
            {
                // 처음 보는 몬스터이면 HashSet에 추가합니다.
                seenMonsters.Add(curmon);
                P_States.notSameMonster = true;
                //P_States.hasAttackSameMonster = true;
            }
        }
    }

    private void playerHitMonster(Vector3 collisionPoint, Quaternion otherQuaternion)
    {
        //TODO: 나중에 연산식 사용.
        int damageValue = (isArrow ? 400 : 350);


        if (P_Value.hits % 5 != 0)
        {
            GameManager.instance.damageCalculator.damageExpression = "A+B";
            GameManager.instance.damageCalculator.CalculateAndPrint();
            damageValue = GameManager.instance.damageCalculator.result;
        }
        else if (P_Value.hits % 5 == 0 && P_Value.hits != 0)
        {
            GameManager.instance.damageCalculator.damageExpression = "A+C";
            GameManager.instance.damageCalculator.CalculateAndPrint();
            damageValue = GameManager.instance.damageCalculator.result;
        }
        monster.GetDamage(damageValue, collisionPoint, otherQuaternion);
        _playerController.playAttackEffect("Attack_Combo_Hit"); //* 히트 이펙트 출력

        P_Value.nowEnemy = monster.gameObject;  //* 몬스터 객체 저장
        P_Value.curHitTime = Time.time; //* 현재 시간 저장

        P_Controller.CheckHitTime();
        P_Value.hits = P_Value.hits + 1;    //* 히트 수 증가
        P_States.hadAttack = true;
        P_States.notSameMonster = false;

        P_States.isBouncing = true;     //* 히트 UI 출력효과
        Invoke("isBouncingToFalse", 0.3f);  //* 히트 UI 출력효과 초기화

    }

    private void ArrowRay()//float curArrowDistance)
    {
        //Debug.Log("[arrow test] ArrowRay()");
        goShoot = false;
        float range = 100f;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(this.transform.position, this.transform.forward, range);

        float shortDist = 1000f;
        RaycastHit shortHit = hits[0];
        //RaycastHit m_Hit;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.name != this.gameObject.name)
            {
                //자기 자신은 패스
                float distance = hit.distance;
                if (/*curArrowDistance < distance &&*/ shortDist > distance)    //범위 내 라면
                {
                    shortHit = hit;
                    shortDist = distance;

                    if (hit.collider.tag == "Monster")
                    {
                        //Debug.Log("[arrow test] arrow hit");
                        monster = hit.collider.GetComponentInParent<Monster>();
                        if (monster == null)
                        {
                            Debug.LogError("몬스터 : null");
                            return;
                        }
                        if (monster.monsterPattern.GetCurMonsterState() != MonsterPattern.MonsterState.Death
                            && P_States.hadAttack == false)
                        {
                            //attackEnemy = true;
                            P_States.hadAttack = true;
                            //m_Hit = hit;
                            Vector3 collisionPoint = hit.point;
                            Quaternion otherQuaternion = Quaternion.FromToRotation(Vector3.up, hit.normal);

                            playerHitMonster(collisionPoint, otherQuaternion);
                        }
                    }
                    else
                    {
                        //attackEnemy = false;
                        //P_States.hadAttack = false;
                    }
                }
            }
        }

        // if (shortDist != 1000)
        //     targetDistance = shortDist;
    }
}
