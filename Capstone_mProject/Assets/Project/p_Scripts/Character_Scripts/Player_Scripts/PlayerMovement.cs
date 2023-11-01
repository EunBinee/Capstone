using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController _controller;// = new PlayerController();
    private PlayerController P_Controller => _controller;
    private PlayerComponents P_Com => P_Controller._playerComponents;
    private PlayerInput P_Input => P_Controller._input;
    private CurrentState P_States => P_Controller._currentState;
    private CurrentValue P_Value => P_Controller._currentValue;
    private CheckOption P_COption => P_Controller._checkOption;
    private PlayerFollowCamera P_Camera => P_Controller._playerFollowCamera;

    public float comboClickTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (P_States.isStartComboAttack && (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1) || P_Input.jumpMovement == 1))
        {
            P_Value.index = 1;
            P_Value.time = 0;
            P_Value.isCombo = false;
            P_States.isStartComboAttack = false;
            P_Com.animator.SetInteger("comboCount", P_Value.index);
            P_Com.animator.SetBool("p_Locomotion", true);
            //P_Com.animator.Play("locomotion");
            P_Com.animator.Rebind();
        }
        Inputs();
        //캐릭터의 애니메이션 변경을 수행하는 함수
        if (!UIManager.gameIsPaused)
        {
            AnimationParameters();
        }
    }
    void FixedUpdate()
    {
        P_Controller.CheckedGround();
        if (!P_States.isPerformingAction) //액션 수행중이 아닐 때만..
        {
            //캐릭터의 실제 이동을 수행하는 함수
            AllPlayerLocomotion();
        }
    }

    private void Inputs()
    {
        if (!HandleJump())
        {
            HandleSprint();
            HandleWalkOrRun();
            P_Input.mouseY = Input.GetAxis("Mouse Y");  //마우스 상하
            P_Input.mouseX = Input.GetAxis("Mouse X");  //마우스 좌우
            if (Input.GetKey(KeyCode.W))
            {
                P_Input.verticalMovement = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                P_Input.verticalMovement = -1;
            }
            else
            {
                P_Input.verticalMovement = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                P_Input.horizontalMovement = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                P_Input.horizontalMovement = -1;
            }
            else
            {
                P_Input.horizontalMovement = 0;
            }
            if (P_States.isGround && Input.GetMouseButtonDown(0) && !P_States.isStartComboAttack)
            {
                P_States.isStartComboAttack = true;
                StartCoroutine(Attacking());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Vector3 knockback_Dir = transform.position - curEnemy.transform.position;
                //knockback_Dir = knockback_Dir.normalized;
                //Vector3 KnockBackPos = transform.position + knockback_Dir * 1.5f;
                //transform.position = Vector3.Lerp(transform.position, KnockBackPos, 5 * Time.deltaTime);

                Vector3 skillDir = this.gameObject.transform.forward;
                skillDir = skillDir.normalized;
                Vector3 skillPos = transform.position + skillDir * 15f;
                transform.position = Vector3.Lerp(transform.position, skillPos, 5 * Time.deltaTime);
                //P_Com.rigidbody.AddForce(skillDir * 10.0f, ForceMode.Impulse);
                P_Controller.skill_E.OnClicked();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //P_Com.rigidbody.AddForce(Vector3.up * P_COption.jumpPower, ForceMode.Impulse);
                P_Controller.skill_Q.OnClicked();
            }

            //Clamp01 >> 0에서 1의 값을 돌려줍니다. value 인수가 0 이하이면 0, 이상이면 1입니다
            P_Value.moveAmount = Mathf.Clamp01(Mathf.Abs(P_Input.verticalMovement) + Mathf.Abs(P_Input.horizontalMovement) + P_Input.jumpMovement);
            if (P_Input.horizontalMovement == 0 && P_Input.verticalMovement == 0 && P_Input.jumpMovement == 0)
                P_States.isNotMoving = true;
            else
                P_States.isNotMoving = false;
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftControl) && P_Value.moveAmount > 0)
        {
            //moveAmount > 0 하는 이유
            //제자리에서 멈춰서 자꾸 뛴다.
            P_States.isSprinting = true;
            P_States.isWalking = false;
            P_States.isRunning = false;
            //P_States.isJumping = false;
        }
        else
        {
            P_States.isSprinting = false;
        }
    }

    private void HandleWalkOrRun()
    {
        //shift => 걷기
        //뛰기중이면, 걷기 생략
        if (P_States.isSprinting)
            return;

        P_States.isWalking = false;
        P_States.isRunning = true;
    }

    private bool HandleJump()
    {
        if (Input.GetKey(KeyCode.Space) && !P_States.isJumping)
        {
            //Debug.Log(P_Value.hitDistance);
            P_Input.jumpMovement = 1;
            return true;
        }
        return false;
    }
    private void PlayerJump()
    {
        if (P_Input.jumpMovement == 1 && !P_States.isJumping)
        {
            P_States.isJumping = true;
            P_Value.gravity = P_COption.gravity;
            P_Com.rigidbody.AddForce(Vector3.up * P_COption.jumpPower, ForceMode.Impulse);
            P_Com.animator.Play("jump 0");
            P_Com.animator.SetBool("isJump_Up", true);
        }
        if (P_States.isJumping && P_States.isGround)
        {
            P_Com.animator.SetBool("isJump_Up", false);
            P_States.isJumping = false;
            P_Input.jumpMovement = 0;
            P_Value.gravity = 0;
        }
    }

    private void HandleDodge()
    {
        P_States.currentDodgeKeyPress = (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1));
        if (P_States.previousDodgeKeyPress && P_States.currentDodgeKeyPress && P_States.isDodgeing
            && P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("dodge"))
        {
            //Debug.Log("이전 프레임에도 누름!");
            return;
        }
        else if (!P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("dodge") && P_States.currentDodgeKeyPress && !P_States.isDodgeing && P_Value.moveAmount > 0)
        {
            P_States.isDodgeing = true;
        }
        P_States.previousDodgeKeyPress = P_States.currentDodgeKeyPress;
    }
    private void dodgeOut()
    {
        P_States.isDodgeing = false;
        // 대쉬 종료 후 Rigidbody 속도를 다시 원래 속도로 변경
        P_Com.rigidbody.velocity = Vector3.zero;
    }

    private void AllPlayerLocomotion()
    {
        //캐릭터의 실제 이동을 수행하는 함수.
        PlayerRotation(); //플레이어의 방향 전환을 수행하는 함수
        PlayerMovements(); //플레이어의 움직임을 수행하는 함수.
        PlayerJump();
        HandleDodge();
    }
    private void PlayerRotation()
    {
        if (P_States.isStartComboAttack && !P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("locomotion"))
        {
            //return;
        }
        if (P_States.isJumping)
        {

        }
        else
        {
            //걷기와 뛰기는 동일하게
            Vector3 targetDirect = Vector3.zero;
            targetDirect = P_Camera.cameraObj.transform.forward * P_Input.verticalMovement;
            targetDirect = targetDirect + P_Camera.cameraObj.transform.right * P_Input.horizontalMovement;
            targetDirect.Normalize(); //대각선 이동이 더 빨라지는 것을 방지하기 위해서
            targetDirect.y = 0;
            if (targetDirect == Vector3.zero)
            {
                //vector3.zero는 0,0,0 이다.
                //방향 전환이 없기에 캐릭터의 방향은 고냥 원래 방향.
                targetDirect = transform.forward;
            }
            Quaternion turnRot = Quaternion.LookRotation(targetDirect);
            Quaternion targetRot = Quaternion.Slerp(transform.rotation, turnRot, P_COption.rotSpeed * Time.deltaTime);
            transform.rotation = targetRot;
        }
    }

    private void PlayerMovements()
    {
        if (P_States.isStartComboAttack && !P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("locomotion"))
        {
            //Debug.Log($"P_States.isStartComboAttack {P_States.isStartComboAttack}");
            //Debug.Log($"P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName(locomotio) {P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("locomotion")}");
            return;
        }
        //플레이어의 움직임을 수행하는 함수.
        //**마우스로 화면을 돌리기때문에 카메라 방향으로 캐릭터가 앞으로 전진한다.
        P_Value.moveDirection = P_Camera.cameraObj.transform.forward * P_Input.verticalMovement;
        P_Value.moveDirection = P_Value.moveDirection + P_Camera.cameraObj.transform.right * P_Input.horizontalMovement;
        //Debug.Log($" P_Camera.cameraObj.transform.forward    {P_Camera.cameraObj.transform.forward}");
        //Debug.Log($" P_Input.verticalMovement {P_Input.verticalMovement}");
        //Debug.Log($" P_Value.moveDirection    {P_Value.moveDirection}");
        //Debug.Log($"  P_Camera.cameraObj.transform.right {P_Camera.cameraObj.transform.right}");

        P_Value.moveDirection.Normalize(); //정규화시켜준다.


        if (P_States.isJumping)
        {
            //Time.timeScale = 0.1f;
            Vector3 p_velocity = P_Com.rigidbody.velocity + Vector3.up * (P_Value.gravity) * Time.fixedDeltaTime;
            P_Com.rigidbody.velocity = p_velocity;
        }
        else if (P_States.isDodgeing)
        {
            //P_Com.animator.SetTrigger("isDodge");
            P_Com.animator.Play("dodge", 0);
            P_Value.moveDirection.y = 0;
            P_Com.rigidbody.velocity += P_Value.moveDirection * P_COption.dodgingSpeed;

            Invoke("dodgeOut", 0.15f);    //대시 유지 시간

        }
        else if (P_States.isSprinting || P_States.isRunning)
        {
            //Time.timeScale = 1f;
            P_Value.moveDirection.y = 0;
            if (P_States.isSprinting)    //전력질주
                P_Value.moveDirection = P_Value.moveDirection * P_COption.sprintSpeed;
            else if (P_States.isRunning) //뛸때
                P_Value.moveDirection = P_Value.moveDirection * P_COption.runningSpeed;

            Vector3 p_velocity = Vector3.ProjectOnPlane(P_Value.moveDirection, P_Value.groundNormal);
            p_velocity = p_velocity + Vector3.up * (P_Value.gravity);
            P_Com.rigidbody.velocity = p_velocity;
            return;
        }
    }

    //애니메이터 블랜더 트리의 파라미터 변경
    private void AnimationParameters()
    {
        //캐릭터의 애니메이션 변경을 수행하는 함수
        //isStrafing에 쓰인다. >주목기능; 현재 카메라가 바라보고 있는 방향을 주목하면서 이동
        float snappedVertical = 0.0f;
        float snappedHorizontal = 0.0f;
        #region Horizontal 
        if (P_Input.horizontalMovement > 0 && P_Input.horizontalMovement <= 0.5f)
        {
            //0보다 큰데 0.5보다 같거나 작은 경우
            snappedHorizontal = 0.5f;
        }
        else if (P_Input.horizontalMovement > 0.5f)
        {
            //0.5보다 큰경우
            snappedHorizontal = 1;
        }
        else if (P_Input.horizontalMovement < 0 && P_Input.horizontalMovement >= -0.5f)
        {
            //0보다 작은데 -0.5보다 같거나 큰 경우
            snappedHorizontal = -0.5f;
        }
        else if (P_Input.horizontalMovement < -0.5f)
        {
            //-0.5보다 작은 경우
            snappedHorizontal = -1;
        }
        else
        {
            //아무것도 누르지 않은 경우
            snappedHorizontal = 0;
        }
        #endregion
        #region Vertical
        if (P_Input.verticalMovement > 0 && P_Input.verticalMovement <= 0.5f)
        {
            //0보다 큰데 0.5보다 같거나 작은 경우
            snappedVertical = 0.5f;
        }
        else if (P_Input.verticalMovement > 0.5f)
        {
            //0.5보다 큰경우
            snappedVertical = 1;
        }
        else if (P_Input.verticalMovement < 0 && P_Input.verticalMovement >= -0.5f)
        {
            //0보다 작은데 -0.5보다 같거나 큰 경우
            snappedVertical = -0.5f;
        }
        else if (P_Input.verticalMovement < -0.5f)
        {
            //-0.5보다 작은 경우
            snappedVertical = -1;
        }
        else
        {
            //아무것도 누르지 않은 경우
            snappedVertical = 0;
        }
        #endregion
        if (P_States.isStartComboAttack && !P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName("locomotion"))
        {
            P_Com.animator.SetFloat("Vertical", 0, 0f, Time.deltaTime);   //상
            P_Com.animator.SetFloat("Horizontal", 0, 0f, Time.deltaTime); //하
            return;
        }
        if (P_States.isSprinting)
        {
            //전력질주
            P_States.isStrafing = false; //뛸때는 주목 해제
            P_Com.animator.SetFloat("Vertical", 2, 0.2f, Time.deltaTime);   //상
            P_Com.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
        }
        else //뛰기 아닐 경우
        {
            if (P_States.isStrafing)
            {
                //주목기능; 현재 카메라가 바라보고 있는 방향을 주목하면서 이동
                //걷기일 경우
                if (P_States.isWalking)
                {
                    //P_Com.animatorator.SetFloat("애니파라미터", value , damptime, Time.deltaTime);
                    //value는 내가 할당하고 싶은 값
                    //dampTime은 이전값에서 value에 도달하는데 걸리는데 소요될것이라 가정하는 "지연시간"
                    //Time.deltaTime: 직전의 실행과 현재 실행 사이의 시간 차가 Time.deltaTime만큼 나오므로 Time.deltaTime을 할당
                    P_Com.animator.SetFloat("Vertical", snappedVertical / 2, 0.2f, Time.deltaTime);   //상
                    P_Com.animator.SetFloat("Horizontal", snappedHorizontal / 2, 0.2f, Time.deltaTime);               //하
                }
                else
                {
                    //뛰기일 경우
                    P_Com.animator.SetFloat("Vertical", snappedVertical, 0.2f, Time.deltaTime);   //상
                    P_Com.animator.SetFloat("Horizontal", snappedHorizontal, 0.2f, Time.deltaTime);               //하
                }
            }
            else
            {
                //걷기 일 경우
                if (P_States.isWalking)
                {
                    // Debug.Log("걷기");
                    //P_Com.animatorator.SetFloat("애니파라미터", value , damptime, Time.deltaTime);
                    //value는 내가 할당하고 싶은 값
                    //dampTime은 이전값에서 value에 도달하는데 걸리는데 소요될것이라 가정하는 "지연시간"
                    //Time.deltaTime: 직전의 실행과 현재 실행 사이의 시간 차가 Time.deltaTime만큼 나오므로 Time.deltaTime을 할당
                    P_Com.animator.SetFloat("Vertical", P_Value.moveAmount / 2, 0.2f, Time.deltaTime);   //상
                    P_Com.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);               //하

                    //Vertical에만 값을 넣어주는 이유
                    //걷기 모션은 Front만 쓴다.
                    //이유 : 애니메이션은 딱 하나 Front만 쓰기 때문
                    // 몸을 돌리는 건 코드에서 돌려준다.
                    //그리고 주목 기능을 쓰게 되면, 몸이 한 방향을 주목하고 움직여야하기에
                    //다른 애니메이션도 쓰이게 된다. 
                    //그래서 snappedVertical과 snappedHorizontal을 통해서.. 모든 값을 준다. 그래야 여러 애니메이션을 쓸 수 있기 때문
                }
                else
                {
                    //뛰기의 경우
                    //Debug.Log("뛰기");
                    P_Com.animator.SetFloat("Vertical", P_Value.moveAmount, 0.2f, Time.deltaTime);   //상
                    P_Com.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);          //하
                }
                if (P_Value.moveAmount == 0)
                {
                    // Debug.Log("멈춤");
                    //아무것도 안누른 경우. >idle
                    P_Com.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);   //상
                    P_Com.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime); //하
                }
            }
        }
    }

    IEnumerator Attacking() //클릭해서 들어오면
    {
        P_Com.animator.SetInteger("comboCount", 0);

        string comboName01 = "Attack_Combo_1";
        string comboName02 = "Attack_Combo_2";
        string comboName03 = "Attack_Combo_3";
        string comboName04 = "Attack_Combo_4";
        string comboName05 = "Attack_Combo_5";
        string curAnimName = "";

        P_Value.index = 1;
        P_Value.time = 0;
        P_Value.isCombo = false;

        while (true)
        {
            P_Value.isCombo = false;
            P_Controller.ChangePlayerState(PlayerState.ComboAttack);
            //AnimState(PlayerState.ComboAttack, index);

            switch (P_Value.index)
            {
                case 1:
                    curAnimName = comboName01;
                    break;
                case 2:
                    curAnimName = comboName02;
                    break;
                case 3:
                    curAnimName = comboName03;
                    break;
                case 4:
                    curAnimName = comboName04;
                    break;
                case 5:
                    curAnimName = comboName05;
                    break;
                default:
                    curAnimName = "";
                    break;
            }
            //Time.timeScale = 0.1f;
            Effect effect = GameManager.Instance.objectPooling.ShowEffect(curAnimName);
            effect.gameObject.transform.position = this.gameObject.transform.position + Vector3.up;
            Quaternion effectRotation = this.gameObject.transform.rotation;
            effectRotation.x = 0;
            effectRotation.z = 0;
            effect.gameObject.transform.rotation = effectRotation;
            P_Com.animator.Play(curAnimName, 0);
            //Debug.Log("effect play " + P_Value.curAnimName);

            yield return new WaitUntil(() => P_Com.animator.GetCurrentAnimatorStateInfo(0).IsName(curAnimName));
            yield return new WaitUntil(() => P_Com.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);

            P_Controller.ChangePlayerState(PlayerState.FinishComboAttack);
            P_Controller.AnimState(PlayerState.FinishComboAttack, P_Value.index);

            int curIndex = P_Value.index;
            P_Value.time = 0;

            while (P_Value.time <= comboClickTime)
            {
                P_Value.time += Time.deltaTime;
                yield return new WaitUntil(() => P_Com.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);

                if (Input.GetMouseButton(0) && curIndex == P_Value.index)
                {
                    if (P_Value.index == 5)
                    {
                        yield return new WaitUntil(() => P_Com.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);
                        P_Value.index = 0;
                        P_Value.time = 0;
                        P_Value.isCombo = false;
                        //P_States.isPerformingAction = false;
                    }
                    else
                    {
                        P_Value.index++;
                        P_Value.isCombo = true;
                    }
                    break;
                }
            }
            if (P_Value.isCombo == false)
            {
                P_Com.animator.SetInteger("comboCount", P_Value.index);
                P_Com.animator.SetBool("p_Locomotion", true);
                break;
            }
        }
        P_States.isStartComboAttack = false;
    }
}
