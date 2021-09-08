
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exmonster : MonoBehaviour
{

    public enum State
    {
        IDLE,//IDLE
        ATTACK,//ATTACK
        WALK,//Mantis -> PATROL
        DIE,//죽었을 때
        HIT//맞았을 때
    }

    public State state = State.IDLE;//IDLE 초기 상태 지정

    Transform playerTr;//플레이어 위치 저장 변수
    Transform enemyTr;//적 위치 저장 변수

    public float movePower;
    Vector3 movement;
    int movementFlag = 0;
    GameObject traceTarget;
    public float attackDist = 5f; //공격 사거리
    public float traceDist = 5f;//추적 사거리
    public bool isDie = false;//사망 여부 판단 변수
    public bool isTracing = false;//추적 상태 판단 변수

    int Hp = 20;//Mantis Hp = 20

    WaitForSeconds ws;//시간 지연 변수
         
    Renderer renderer; //몬스터 죽었을 때 알파값을 조정하기 위한 렌더러

    Animator animator;
    readonly int Maintis_ATTACK = Animator.StringToHash("ATTACK");//공격 애니메이션
    readonly int Mantis_IDLE = Animator.StringToHash("IDLE");//idle 애니메이션
    readonly int Maintis_WALK = Animator.StringToHash("WALK");//Mantis-> PATROL 애니메이션



    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        StartCoroutine("ChangeMovement");

        var player = GameObject.FindGameObjectWithTag("Player");//player 태그 지정
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        enemyTr = GetComponent<Transform>();

        animator = GetComponent<Animator>();

        ws = new WaitForSeconds(0.3f);//시간 지연 변수 (코루틴 함수에서 사용)
    }

    private void Update()
    {
        ChangeMovement();
    }


    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);


        while (!isDie)
        {
            yield return ws;
            if (movementFlag == 0)
            {
                animator.SetBool("IDLE", true);
                animator.SetBool("ATTACK", false);
                animator.SetBool("WALK", false);
            }


            else if (Hp==0)//DIE
            {
                gameObject.tag = "Enemy";
                isDie = true;
                GetComponent<Collider2D>().enabled = false;//콜라이더 삭제
                Destroy(gameObject, 5f);//5초 뒤 몬스터 삭제 
                renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0f);
                //알파값 조정 ->  죽었을 때 알파값 서서히 낮춰짐
            }

            else if (state == State.HIT)
            {
                Hp -= 5;
                HIT();
            }

            else
            {
                animator.SetBool("ATTACK", true);
                animator.SetBool("WALK", true);
                animator.SetBool("IDLE", false);
            }

            yield return new WaitForSeconds(3f);

            StartCoroutine("ChangeMovement");
        }
       
    }

    private void OnEnable()//해당 스크립트가 활성화 될 때마다 실행됨
    {
        Action();
        CheckState();
        OnPlayerDie();
        ChangeMovement();
    }

    void FixedUpdate()
    {
        Move();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HIT();//player 공격 맞았을 때 Getcomponent 해주기
    }

    public IEnumerator HIT()//맞았을 때
    {
        for (int i = 0; i < 10; i++)
        {
            renderer.material.color = Color.white;//
        }
        yield return new WaitForSeconds(0.3f);//맞았을 때 깜빡임
    }

    public IEnumerator Action()
    {
        yield return new WaitForSeconds(1f);//다른 오브젝트 스크립트 초기화를 위한 대기 시간

        while (!isDie)//몬스터가 살아있는동안 계속 while문으로 실행 시킴
        {
            if (state == State.DIE)
                yield break;//몬스터가 죽으면 코루틴 함수 정지

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);//player와 몬스터 거리 계산 함수

            if (dist <= attackDist)//사정 거리 내일 때 공격으로 변경
            {
                state = State.ATTACK;//공격
            }
            else
            {
                state = State.WALK;
            }

            yield return ws;//위에서 설정한 지연시간 0.3초 대기
        }
    }

    public IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1f);//다른 오브젝트 스크립트 초기화를 위한 대기 시간

        while (!isDie)//몬스터가 살아있는동안 계속 while문으로 실행 시킴
        {
            if (state == State.DIE)
                yield break;//몬스터가 죽으면 코루틴 함수 정지

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);//player와 몬스터 거리 계산 함수

            if (dist <= attackDist)//사정 거리 내일 때 공격으로 변경
            {
                state = State.ATTACK;//공격
            }
            else
            {
                state = State.WALK;
            }

            yield return ws;//위에서 설정한 지연시간 0.3초 대기
        }
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        string dist = "";

        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;

            if (playerPos.x < transform.position.x)
                dist = "Left";
            else if (playerPos.x > transform.position.x)
                dist = "Right";
        }
        else
        {
            if (movementFlag == 1)
                dist = "Left";
            else if (movementFlag == 2)
                dist = "Right";
        }

        if (dist == "Left")
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dist == "Right")
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
        };

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            traceTarget = other.gameObject;

            StopCoroutine("ChangeMovement");
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = true;
            animator.SetBool("", true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTracing = false;
            StartCoroutine("ChangeMovement");
        }
    }
    public void OnPlayerDie()//플레이어가 죽었을 때
    {
        StopAllCoroutines();//모든 코루틴 함수 종료
    }
}
