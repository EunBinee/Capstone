using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CameraController : MonoBehaviour
{
    public PlayerController playerController;
    public Transform playerHeadPos;
    [Header("카메라 오브젝트.")]
    public GameObject playerCamera;      //카메라 오브젝트
    public GameObject playerCameraPivot; //카메라 피봇
    public Camera cameraObj;             //카메라.
    public Transform cameraTrans;

    [Header("회전 스피트")]
    public float left_right_LookSpeed = 500; //왼 오 돌리는 스피드
    public float up_down_LookSpeed = 500;    //위아래로 돌리는 스피드
    [Header("위아래 고정 비율  >> 0이면 위아래로 카메라 안움직임")]
    public float minPivot = -35;              //위아래 고정 시키기 위한 Pivot -35로 아래 고정
    public float maxPivot = 35;               //35로 위 고정

    [Header("Camera Debug")]
    //카메라가 캐릭터를 쫒아가는 데 속력. zero로 초기화
    public Vector3 cameraFllowVelocity = Vector3.zero;
    public float left_right_LookAngle;
    public float up_down_LookAngle;

    public bool stopRotation = false;

    [Header("주목 기능")]
    public bool banAttention = false; // 주목 금지
    public bool isBeingAttention = false;
    public bool controlCam = true;
    public Monster curTargetMonster = null;
    public Transform targetTrans;
    public float normal_Z = -5f;
    public float attention_Z = -6.5f;
    public float longAttention_Z = -7.5f;

    public float aimSmootly = 0.55f;
    public Transform campos;

    bool isNormal_Z = false;
    bool isAttention_Z = false;
    bool isLongAttention_Z = false;

    float time_Z = 0;
    float duration = 1f;
    float distancePlayer = 0f;
    Coroutine resetCameraZ_co = null;

    //"벽 체크"
    public float originZ_Zoom = 0f;
    public bool isZoomedIn = false;

    private void Awake()
    {
        //CamReset();
        cameraTrans = cameraObj.gameObject.GetComponent<Transform>();

    }
    private void Start()
    {
        playerController = GameManager.Instance.gameData.player.GetComponent<PlayerController>();
        playerHeadPos = GameManager.instance.gameData.playerHeadPos;
        Check_Z();
        CamReset();
        stopRotation = false;
    }


    private void Update()
    {
        if (controlCam)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && !banAttention)
            {
                if (isBeingAttention)// 주목 기능
                {
                    //다른 몬스터로 다시 주목
                    if (GameManager.instance.monsterUnderAttackList.Count > 1)
                    {
                        ChangeAttentionMonster();
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            CameraRecovery();
        }
    }
    //* 처음 주목
    public void AttentionMonster()
    {
        if (GameManager.instance.monsterUnderAttackList.Count > 0)
        {
            if (resetCameraZ_co != null)
                StopCoroutine(resetCameraZ_co);
            Vector3 camPos = cameraTrans.localPosition;
            camPos.z = attention_Z;
            cameraTrans.localPosition = camPos;
            playerController._currentState.isStrafing = true;
            //처음 주목한 경우
            isBeingAttention = true;
            //* 처음에 주목할 때는 가장 가까이에 있는 몬스터부터 주목
            GameManager.instance.SortingMonsterList();
            curTargetMonster = GameManager.instance.monsterUnderAttackList[0];
        }
    }

    //*주목 대상 바꾸기
    public void ChangeAttentionMonster()
    {

        GameManager.instance.SortingMonsterList();

        if (GameManager.instance.monsterUnderAttackList.Count >= 2)//1마리 이상이면?
        {
            if (curTargetMonster == GameManager.instance.monsterUnderAttackList[0])
            {
                curTargetMonster = GameManager.instance.monsterUnderAttackList[1];
            }
            else
            {
                curTargetMonster = GameManager.instance.monsterUnderAttackList[0];
            }
        }
        else if (GameManager.instance.monsterUnderAttackList.Count == 1)
        {
            curTargetMonster = GameManager.instance.monsterUnderAttackList[0];
        }

    }

    //*주목 풀기
    public void UndoAttention()
    {
        ResetCameraZ();
        playerController._currentState.isStrafing = false;

        Vector3 playerCam = playerCamera.transform.rotation.eulerAngles;
        left_right_LookAngle = playerCam.y;
        up_down_LookAngle = playerCam.x;

        banAttention = false;
        isBeingAttention = false;
        curTargetMonster = null;
    }

    private void LateUpdate()
    {
        CameraActions();
        WallInFrontOfCamera();
    }




    //카메라 움직임
    private void CameraActions()
    {
        CameraFollowPlayer(); //플레이어를 따라다니는 카메라
        if (isBeingAttention)
        {
            TargetRotate();
            FixCamZ();
        }
        else
        {
            if (!stopRotation)
                CameraRotate();  //마우스 방향에 따른 카메라 방향
        }
    }

    private void CameraFollowPlayer()
    {
        //플레이어를 따라다니는 카메라
        //ref는 call by reference를 하겠다는 것.

        Vector3 cameraPos;
        if (playerController._currentState.isAim)
        {
            cameraPos = Vector3.Lerp(playerCamera.transform.position, playerController.gameObject.transform.position, aimSmootly);
        }
        else
        {
            cameraPos = Vector3.Lerp(playerCamera.transform.position, playerController.gameObject.transform.position, 1.5f);
        }

        playerCamera.transform.position = cameraPos;
    }

    private void CameraRotate() // 일반 카메라
    {
        //마우스 방향에 따른 카메라 방향
        Vector3 cameraRot;
        Quaternion targetCameraRot;
        left_right_LookAngle += (playerController._input.mouseX * left_right_LookSpeed) * Time.deltaTime;
        up_down_LookAngle -= (playerController._input.mouseY * up_down_LookSpeed) * Time.deltaTime;

        up_down_LookAngle = Mathf.Clamp(up_down_LookAngle, minPivot, maxPivot); //위아래 고정

        //가로 세로
        cameraRot = Vector3.zero;
        cameraRot.y = left_right_LookAngle;
        targetCameraRot = Quaternion.Euler(cameraRot);
        playerCamera.transform.rotation = targetCameraRot;
        //위아래
        cameraRot = Vector3.zero;
        cameraRot.x = up_down_LookAngle;
        targetCameraRot = Quaternion.Euler(cameraRot);
        playerCameraPivot.transform.localRotation = targetCameraRot;
    }

    private void TargetRotate()
    {
        if (curTargetMonster == null)
        {
            Debug.Log("카메라. 타겟 몬스터 null이다.");
            return;
        }

        SetCameraZ_AccDistance();

        Vector3 cameraRot;
        Quaternion targetCameraRot = Quaternion.identity;
        // 타겟의 위치로 향하는 방향 벡터를 구함
        Vector3 targetPos;

        targetPos = curTargetMonster.gameObject.transform.position;

        Vector3 directionToTarget = targetPos - playerCameraPivot.transform.position;
        cameraRot = Vector3.zero;
        cameraRot.y = directionToTarget.x;

        if (directionToTarget.sqrMagnitude >= 3)
        {
            targetCameraRot = Quaternion.LookRotation(directionToTarget);// 방향 벡터를 바라보도록 하는 Quaternion을 생성    
        }
        if (QuaternionAngleDifference(targetCameraRot, playerCamera.transform.rotation) < 1f)
        {
            //각도가 1보다 작으면.. 같은걸로 간주
            playerCamera.transform.rotation = targetCameraRot;
        }
        else if (targetCameraRot != Quaternion.identity)
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, targetCameraRot, 4f * Time.deltaTime); /* x speed */

        //playerCamera.transform.position = target.position - transform.forward * dis;
        //위아래
        cameraRot = Vector3.zero;
        targetCameraRot = Quaternion.Euler(cameraRot);
        playerCameraPivot.transform.localRotation = targetCameraRot;
    }


    public void ResetCameraZ()
    {
        if (cameraTrans == null)
        {
            cameraTrans = cameraObj.gameObject.GetComponent<Transform>();
        }
        if (resetCameraZ_co != null)
        {
            StopCoroutine(resetCameraZ_co);
        }
        resetCameraZ_co = StartCoroutine(ResetCameraZ_co(5f));
    }

    IEnumerator ResetCameraZ_co(float duration)
    {
        Check_Z();
        Vector3 camPos = cameraTrans.localPosition;
        Vector3 camPivotPos = playerCameraPivot.transform.localPosition;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            //camPos.z = -5f;
            float value = Mathf.Lerp(camPos.z, normal_Z, time / duration);
            camPos.z = value;
            cameraTrans.localPosition = camPos;

            value = Mathf.Lerp(camPivotPos.y, 1.7f, time / duration);
            camPivotPos.y = value;
            playerCameraPivot.transform.localPosition = camPivotPos;

            yield return null;
        }

        camPos = cameraTrans.localPosition;
        camPos.z = normal_Z;
        cameraTrans.localPosition = camPos;

        camPivotPos = playerCameraPivot.transform.localPosition;
        camPivotPos.y = 1.7f;
        playerCameraPivot.transform.localPosition = camPivotPos;
    }

    private void SetCameraZ_AccDistance()
    {
        //플레이어와 몬스터의 거리에 따른 z값 변경
        float distance = Vector3.Distance(playerController.gameObject.transform.position, curTargetMonster.gameObject.transform.position);

        Vector3 camPos = cameraTrans.localPosition;
        Vector3 camPivotPos = playerCameraPivot.transform.localPosition;

        float normalDistance = 10;
        float longAttention_Distance = 6;
        if (GameManager.instance.bossBattle)
        {
            normalDistance = 20;
            longAttention_Distance = 12;
        }

        if (distance > normalDistance)
        {
            //z 를 normal_Z(-5)로 변경
            if (camPos.z != normal_Z)
            {
                if (isNormal_Z == false)
                {
                    time_Z = 0;
                    isNormal_Z = true;
                    isAttention_Z = false;
                    isLongAttention_Z = false;
                }
                time_Z += Time.deltaTime;

                float value = Mathf.Lerp(camPos.z, normal_Z, time_Z / duration);
                camPos.z = value;
                cameraTrans.localPosition = camPos;

                value = Mathf.Lerp(camPivotPos.y, 1.7f, time_Z / duration);
                camPivotPos.y = value;
                playerCameraPivot.transform.localPosition = camPivotPos;
            }
        }
        else if (distance < longAttention_Distance)
        {
            //z를 longAttention_Z(-9)으로 변경
            if (camPos.z != longAttention_Z)
            {
                if (isLongAttention_Z == false)
                {
                    time_Z = 0;
                    isNormal_Z = false;
                    isAttention_Z = false;
                    isLongAttention_Z = true;
                }
                time_Z += Time.deltaTime;

                float value = Mathf.Lerp(camPos.z, longAttention_Z, time_Z / duration);
                camPos.z = value;
                cameraTrans.localPosition = camPos;

                if (camPivotPos.y != 1.5f)
                {
                    value = Mathf.Lerp(camPivotPos.y, 1.5f, time_Z / duration);
                    camPivotPos.y = value;
                    playerCameraPivot.transform.localPosition = camPivotPos;
                }
            }
        }
        else
        {
            //z를 -6으로 변경
            if (camPos.z != attention_Z)
            {
                if (isAttention_Z == false)
                {
                    time_Z = 0;
                    isNormal_Z = false;
                    isAttention_Z = true;
                    isLongAttention_Z = false;
                }
                time_Z += Time.deltaTime;

                float value = Mathf.Lerp(camPos.z, attention_Z, time_Z / duration);
                camPos.z = value;
                cameraTrans.localPosition = camPos;

                if (camPivotPos.y != 1.2f)
                {
                    value = Mathf.Lerp(camPivotPos.y, 1.2f, time_Z / duration);
                    camPivotPos.y = value;
                    playerCameraPivot.transform.localPosition = camPivotPos;
                }
            }
        }
    }

    public void Check_Z()
    {
        if (!GameManager.instance.bossBattle)
        {
            normal_Z = -6f;
            attention_Z = -7.5f;
            longAttention_Z = -9.5f;
        }
        else
        {
            normal_Z = -5f;
            attention_Z = -9f;
            longAttention_Z = -13f;
        }
    }

    float changeZ = 0f;      //바뀔 Z
    float duration_Z = 0.3f; //보간으로 Z를 바꿀때 시간
    bool replaceZ = false; //원래의 Z로 돌아감(줌인에서 다시 Z로)

    public void WallInFrontOfCamera()
    {
        Vector3 curDirection = cameraObj.gameObject.transform.position - playerHeadPos.position;
        Debug.DrawRay(playerHeadPos.position, curDirection * 20, Color.magenta);
        Ray ray = new Ray(playerHeadPos.position, curDirection);
        RaycastHit hit;
        // Ray와 충돌한 경우
        if (Physics.Raycast(ray, out hit))
        {
            float dist = Vector3.Distance(hit.point, cameraObj.gameObject.transform.position);//  (hit.point - cameraObj.gameObject.transform.position).magnitude;
            Debug.Log($"dist {dist}");
            bool isbehind = CheckObj_behindCamera(hit.point);
            float camPosZ = 0;
            if (isbehind)
            {
                //앞에있음
                camPosZ = cameraObj.gameObject.transform.localPosition.z + dist;
                Debug.Log($"a camPosZ {camPosZ}");
            }
            else
            {
                //뒤에 있음.
                camPosZ = cameraObj.gameObject.transform.localPosition.z - dist;
                Debug.Log($"b camPosZ {camPosZ}");
            }

            if (camPosZ >= -0.9f)
            {
                camPosZ = -0.9f;
            }
            if (camPosZ <= -5f)
            {
                camPosZ = -5f;
            }
            cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, camPosZ);

            // isbehind = CheckObj_behindCamera(cameraPos, hit.point);
            // if (isbehind)
            // {
            //     float distance = (hit.point - cameraPos).magnitude;

            //     cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, cameraObj.gameObject.transform.localPosition.z - distance);
            // }
            // else
            // {
            //     cameraObj.gameObject.transform.localPosition = cameraPos;
            // }

            //!아래 주석 사용!!
            /*
            float dist = (hit.point - cameraObj.gameObject.transform.localPosition).magnitude * 0.8f;
            Vector3 temp = cameraPos.normalized * dist;
            if (temp.z > -0.9)
            {
                temp.z = -0.9f;
            }
            else if (temp.z < -5f)
            {
                temp.z = -5f;
            }
            cameraObj.gameObject.transform.localPosition = temp;

            */


            /*
            Vector3 cameraPos = cameraObj.gameObject.transform.localPosition;
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
            bool isbehind = CheckObj_behindCamera(cameraObj.gameObject.transform.position, hit.point);
            if (isbehind)
            {
                float dist = (cameraPos - hit.point).magnitude * 0.5f;
                Vector3 camPos = cameraPos.normalized * dist;
                float camZ = camPos.z;
                if (camZ > -0.9)
                {
                    camZ = -0.9f;
                }
                else if (camZ < -5f)
                {
                    camZ = -5f;
                }
                cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, camPos.z);
            }
            else
            {
                cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, -5);
            }
            */
        }
    }

    public void WallInFrontOfCamera_02()
    {
        Vector3 curDirection = cameraObj.gameObject.transform.position - playerHeadPos.position;
        Debug.DrawRay(playerHeadPos.position, curDirection * 20, Color.magenta);
        Ray ray = new Ray(playerHeadPos.position, curDirection);
        RaycastHit hit;
        // Ray와 충돌한 경우
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 camPos = cameraObj.gameObject.transform.localPosition;
            float curZ = 0;
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
            bool isbehind = CheckObj_behindCamera(hit.point);
            if (!isbehind)
            {
                if (isZoomedIn)
                {
                    Vector3 checkPosition = new Vector3(0, 0, originZ_Zoom);
                    isbehind = CheckObj_behindCamera(hit.point);
                    if (!isbehind)
                    {
                        time_Z = 0;
                        replaceZ = true;
                        changeZ = 0;
                        //cameraObj.gameObject.transform.localPosition = checkPosition;
                    }
                }

                if (replaceZ)
                {
                    cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, originZ_Zoom);
                    //  Debug.Log("aaa b");
                    //  if (cameraObj.gameObject.transform.position.z != originZ_Zoom)
                    //  {
                    //      time_Z += Time.deltaTime;
                    //      float value = Mathf.Lerp(camPos.z, originZ_Zoom, time_Z / duration_Z);
                    //      if (value <= originZ_Zoom)
                    //      {
                    //          replaceZ = false;
                    //          value = originZ_Zoom;
                    //          isZoomedIn = false;
                    //          Debug.Log("aaa D");
                    //      }
                    //
                    //      Vector3 checkPosition = new Vector3(0, 0, value);
                    //      cameraObj.gameObject.transform.localPosition = checkPosition;
                    //  }
                    //
                    //  else
                    //  {
                    //      replaceZ = false;
                    //  }
                }
            }
            if (isbehind)
            {
                Debug.Log("aaa c");
                if (replaceZ)
                    replaceZ = false;

                if (!isZoomedIn) //처음 줌인 한경우에만.
                {
                    originZ_Zoom = cameraObj.gameObject.transform.localPosition.z;
                    changeZ = cameraObj.gameObject.transform.localPosition.z;
                    isZoomedIn = true;
                }

                //물체가 카메라 앞에 있다면?
                float distance = Vector3.Distance(hit.point, cameraObj.gameObject.transform.localPosition);
                float checkZ = originZ_Zoom + (distance + 1.5f);
                //z의 최대는 -.8
                if (checkZ > -0.9f)
                    checkZ = -0.9f;
                if (checkZ < originZ_Zoom)
                    checkZ = originZ_Zoom;

                float abs = Mathf.Abs(changeZ) - Mathf.Abs(checkZ);
                if (Mathf.Abs(abs) >= 0.1f)
                {
                    curZ = checkZ;
                    changeZ = curZ;
                    time_Z = 0;
                    cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, curZ);
                }

                // if (cameraObj.gameObject.transform.position.z != curZ)
                // {
                //     time_Z += Time.deltaTime;
                //
                //     float value = Mathf.Lerp(cameraObj.gameObject.transform.position.z, curZ, time_Z / duration_Z);
                //     Debug.Log($"aaa cameraObj.gameObject.transform.position.z {cameraObj.gameObject.transform.position.z}, curZ {curZ}");
                //     Debug.Log($"aaa curZ {curZ}, value {value}");
                //     cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, value);
                // }
            }
        }
    }

    public void WallInFrontOfCamera_01()
    {
        Vector3 curDirection = cameraObj.gameObject.transform.position - playerHeadPos.position;
        Debug.DrawRay(playerHeadPos.position, curDirection * 20, Color.magenta);
        Ray ray = new Ray(playerHeadPos.position, curDirection);
        RaycastHit hit;
        // Ray와 충돌한 경우
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray hit: " + hit.collider.gameObject.name);
            bool isbehind = CheckObj_behindCamera(hit.point);
            if (!isbehind)
            {
                if (isZoomedIn)
                {
                    Vector3 checkPosition = new Vector3(0, 0, originZ_Zoom);
                    isbehind = CheckObj_behindCamera(hit.point);
                    if (!isbehind)
                    {
                        isZoomedIn = false;
                        cameraObj.gameObject.transform.localPosition = checkPosition;
                    }
                }
            }
            if (isbehind)
            {
                if (!isZoomedIn) //처음 줌인 한경우에만.
                    originZ_Zoom = cameraObj.gameObject.transform.localPosition.z;

                isZoomedIn = true;

                //물체가 카메라 앞에 있다면?
                float distance = Vector3.Distance(hit.point, cameraObj.gameObject.transform.position);
                float curZ = originZ_Zoom + (distance + 1.5f);

                //z의 최대는 -.8
                if (curZ > -0.9f)
                    curZ = -0.9f;
                if (curZ < originZ_Zoom)
                    curZ = originZ_Zoom;
                cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, curZ);
            }
        }
    }
    //! 감지된 객체가 카메라의 뒤에 있는지 앞에 있는지 확인용 함수
    public bool CheckObj_behindCamera(Vector3 objPos)
    {
        // 충돌 지점이 현재 객체의 앞쪽에 있는지 뒷쪽에 있는지 확인
        Vector3 directionToHitPoint = objPos - cameraObj.gameObject.transform.position;
        float dotProduct = Vector3.Dot(directionToHitPoint, cameraObj.gameObject.transform.forward);

        if (dotProduct > 0)
        {
            // 충돌 지점이 현재 객체의 앞쪽에 있음
            Debug.Log("충돌 지점이 앞에 있습니다.");
            return true;
        }
        else
        {
            // 충돌 지점이 현재 객체의 뒷쪽에 있음
            Debug.Log("충돌 지점이 뒤에 있습니다.");
            return false;
        }
    }

    public void WallInFrontOfCamera_t()
    {
        //카메라 앞에 무언가있으면 줌인 없으면 줌아웃(Z값 만큼)
        //플레이어와 몬스터 제외
        Debug.DrawRay(cameraObj.gameObject.transform.position, cameraObj.gameObject.transform.forward * 10, Color.magenta);
        Ray ray = new Ray(cameraObj.gameObject.transform.position, cameraObj.gameObject.transform.forward);

        // Ray에 부딪힌 물체를 저장할 변수
        RaycastHit[] hits = Physics.RaycastAll(ray);//TODO: 플레이어와 몬스터 레이어 제외 시키기
                                                    // Ray와 충돌한 경우

        if (hits.Length != 0)
        {
            distancePlayer = Vector3.Distance(playerController.gameObject.transform.position, cameraObj.gameObject.transform.position);
            float longDistance = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (!hits[i].collider.CompareTag("Player") && !hits[i].collider.CompareTag("Monster"))
                {
                    float distance = Vector3.Distance(hits[i].collider.gameObject.transform.position, cameraObj.gameObject.transform.position);
                    if (distancePlayer >= distance)
                    {
                        //플레이어의 앞에 객체가 있는 것.
                        if (longDistance < distance)
                        {
                            longDistance = distance;
                        }
                    }
                }
            }


            if (longDistance == 0)
            {
                //앞에 아무것도 없는것. 뒤로 가야함.
                //if(curZ != nomalZ)
            }
            else
            {
                //* 앞에 객체 존재
                Debug.Log($"longDistance {longDistance}");
                float curZ = cameraObj.gameObject.transform.localPosition.z + (longDistance - 2f);
                cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, curZ);
            }

        }
    }


    //*----------------------------------------------------------------------------------------//
    void FixCamZ()
    {
        Vector3 pos = playerCamera.transform.rotation.eulerAngles;
        pos.z = 0;

        playerCamera.transform.rotation = Quaternion.Euler(pos);
    }
    // 두 Quaternion 간의 각도 차이를 반환하는 함수
    float QuaternionAngleDifference(Quaternion a, Quaternion b)
    {
        // Quaternion.Angle 함수를 사용하여 각도 차이 계산
        return Quaternion.Angle(a, b);
    }

    void CamReset()
    {
        cameraObj.gameObject.transform.localPosition = new Vector3(0, 0, normal_Z);
        cameraObj.gameObject.transform.localRotation = Quaternion.identity;
    }

    //* 보스전 끝난 후 주목 풀기.
    public void BossCameraReset(float stopTime)
    {
        StartCoroutine(BossCameraReset_Co(stopTime));
    }

    IEnumerator BossCameraReset_Co(float stopTime)
    {
        yield return new WaitForSeconds(stopTime);
        GameManager.instance.bossBattle = false;
        Check_Z();
        ResetCameraZ();
        controlCam = false;
        UndoAttention();

    }

    //*------------------------------------------------------------------------------------------//

    public void CameraRecovery()
    {
        //시네머신 이후 카메라 복구
        Check_Z();
        CamReset(); //rotation, position 변경
    }


    void OnPreCull() => GL.Clear(true, true, Color.black);

}
