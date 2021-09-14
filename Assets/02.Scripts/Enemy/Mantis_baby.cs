
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis_baby : MonoBehaviour
{

    public enum State
    {
        IDLE,//IDLE
        ATTACK,//ATTACK
        FLY,//Mantis_baby -> PATROL
        TURN,//Mantis_baby -> 플레이어가 일정거리 안에 들어왔을 시 딱 1번만 재생되는 애니메이션    ----> FLY랑 연결됨
        DIE,//죽었을 때
        HIT,//맞았을 때
        TRACE//추적할 떄

    }

    public State state = State.IDLE;//IDLE 초기 상태 지정

    Transform playerTr;//플레이어 위치 저장 변수
    Transform enemyTr;//적 위치 저장 변수
    public Vector3 defalut_direction;
    public Vector3 direction;//몬스터가 갈 방향


    float stopDistance = 1.5f;
    public float speed = 2f;//몬스터 이동 속도
    public float attackDist = 5f; //공격 사거리
    public float traceDist = 5f;//추적 사거리

    public bool isDie = false;//사망 여부 판단 변수
    public bool isTracing = false;//추적 상태 판단 변수
    bool turn = false;//턴 초기 실행
                      // bool attack = false;//어택 초기 실행


    //public float defult_velocity;
    //public float accelaration;//몬스터 가속도


    int Hp = 15;//Mantis_baby Hp = 15

    WaitForSeconds ws;//시간 지연 변수

    Renderer renderer; //몬스터 죽었을 때 알파값을 조정하기 위한 렌더러

    Animator animator;
    int Maintis_Baby_FLY = Animator.StringToHash("FLY");//Mantis_baby -> PATROL 애니메이션
    int Mantis_Baby_Turn = Animator.StringToHash("TURN");///Mantis_baby 이벤트 애니메이션 ↑ enum 함수 참고
    int Maintis_Baby_ATTACK = Animator.StringToHash("ATTACK");//공격 애니메이션
    int Mantis_Baby_IDLE = Animator.StringToHash("IDLE");//idle 애니메이션



    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");//player 태그 지정
        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        enemyTr = GetComponent<Transform>();//몬스터 위치 저장

        animator = GetComponent<Animator>();

        ws = new WaitForSeconds(0.1f);//시간 지연 변수 (코루틴 함수에서 사용)


        defalut_direction.x = Random.Range(-1.0f, 1.0f);
        defalut_direction.y = Random.Range(-1.0f, 1.0f);

        //accelaration = 0.1f;
        //defult_velocity = 0.1f;
    }

    private void Update()
    {
        Move_();
    }

    private void OnEnable()//해당 스크립트가 활성화 될 때마다 실행됨
    {
        StartCoroutine(Action());
        StartCoroutine(CheckState());
    }

    public void Move_()//플립 및 추적 
    {

        Vector3 moveVelocity = Vector3.zero;



        float dist = Vector3.Distance(playerTr.position, enemyTr.position);
        float traceDist = Vector3.Distance(playerTr.position, enemyTr.position);

        if (Vector2.Distance(transform.position, playerTr.position) < stopDistance)
        {
            //Debug.Log("작동");
            //transform.Translate(Vector2.zero * 0f * Time.deltaTime);
            speed = 0f;
        }
        else
        {
            speed = 2f;
        }

        Vector3 flip = transform.localScale;
        if (playerTr.transform.position.x > this.transform.position.x)
        {
            flip.x = 1f;

            transform.Translate(Vector2.right * speed * Time.deltaTime);

        }
        else
        {
            flip.x = -1f;
            transform.Translate(Vector2.left * speed * Time.deltaTime);

        }
        this.transform.localScale = flip;


        //transform.position += moveVelocity * Time.deltaTime;



    }

    public IEnumerator Action()//애니메이션 파라미터
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.IDLE:
                    animator.SetBool(Mantis_Baby_IDLE, true);
                    animator.SetBool(Maintis_Baby_FLY, false);
                    animator.SetBool(Maintis_Baby_ATTACK, false);
                    animator.SetBool(Mantis_Baby_Turn, false);
                    break;

                case State.FLY:
                    animator.SetBool(Maintis_Baby_FLY, true);
                    animator.SetBool(Mantis_Baby_IDLE, false);
                    animator.SetBool(Maintis_Baby_ATTACK, false);
                    animator.SetBool(Mantis_Baby_Turn, false);
                    break;

                case State.ATTACK:
                    animator.SetBool(Maintis_Baby_ATTACK, true);
                    animator.SetBool(Mantis_Baby_IDLE, false);
                    animator.SetBool(Maintis_Baby_FLY, false);
                    animator.SetBool(Mantis_Baby_Turn, false);
                    break;

                case State.TURN:
                    animator.SetBool(Maintis_Baby_ATTACK, false);
                    animator.SetBool(Mantis_Baby_IDLE, false);
                    animator.SetBool(Maintis_Baby_FLY, false);
                    animator.SetBool(Mantis_Baby_Turn, true);
                    break;

                case State.TRACE:
                    animator.SetBool(Maintis_Baby_ATTACK, false);
                    animator.SetBool(Mantis_Baby_IDLE, false);
                    animator.SetBool(Maintis_Baby_FLY, false);
                    animator.SetBool(Mantis_Baby_Turn, false);
                    break;

                case State.DIE:

                    animator.SetBool(Maintis_Baby_ATTACK, false);
                    animator.SetBool(Mantis_Baby_IDLE, true);//죽었을 때 애니메이션을 멈추기 위함
                    animator.SetBool(Maintis_Baby_FLY, false);
                    animator.SetBool(Mantis_Baby_Turn, false);

                    gameObject.tag = "Enemy";
                    isDie = true;
                    GetComponent<Collider2D>().enabled = false;//콜라이더 삭제
                    Destroy(gameObject, 0.5f);//0.5초 뒤 몬스터 삭제 
                    //알파값 조정 ->  죽었을 때 알파값 서서히 낮춰짐
                    break;

                case State.HIT:
                    Hp -= 5;
                    break;
            }
        }
        yield return ws;
    }

    public IEnumerator delay()
    {
        turn = true;
        state = State.TURN;
        yield return new WaitForSeconds(0.04f);//TURN 한 번만 재생 시키기 위함
        //attack = true;
    }

    public IEnumerator delay2()
    {
        yield return new WaitForSeconds(0.04f);
    }

    public IEnumerator CheckState()
    {
        yield return new WaitForSeconds(2f);//다른 오브젝트 스크립트 초기화를 위한 대기 시간

        while (!isDie)//몬스터가 살아있는동안 계속 while문으로 실행 시킴
        {
            if (state == State.DIE)
                yield break;//몬스터가 죽으면 코루틴 함수 정지

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);//player와 몬스터 거리 계산 함수

            if (dist <= attackDist)//사정 거리 내일 때 공격으로 변경
            {

                if (turn == false || state == State.FLY)
                {
                    yield return StartCoroutine(delay());//TURN 애니메이션 호출
                    state = State.ATTACK;//공격

                    //state = State.ATTACK;//공격
                    //yield return StartCoroutine(delay());//TURN 애니메이션 호출
                    Debug.Log("턴 실행");

                }

                else if (state == State.TURN || state == State.ATTACK)
                {
                    StartCoroutine(delay2());
                    state = State.FLY;
                    Debug.Log("FLY 실행");
                }

                Debug.Log("조건문 실행");

            }



            else if (dist >= attackDist)
            {
                state = State.FLY;
            }

            yield return new WaitForSeconds(3f);//위에서 설정한 지연시간 0.3초 대기

        }
    }



}
