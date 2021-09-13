using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageScr : MonoBehaviour
{

    public float Hp = 20; //몬스터 체력

    SpriteRenderer sr;
    Shader normal;
    Shader white;

    bool hitEffect = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        normal = sr.material.shader;
        white = Shader.Find("GUI/Text Shader");
    }

    private void Update()
    {
        if (hitEffect)
        {
            hitEffect = false;
            StartCoroutine(HIT());
        }
    }

    private  IEnumerator HIT()
    {
        print("셰이더 변경");
        sr.material.shader = white;
        yield return new WaitForSeconds(0.5f);
        sr.material.shader = normal;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag ("ATTACK"))
        {
            hitEffect = true;
            
            Hp -= collision.gameObject.GetComponent<AttackTrail>().power;//player 데미지 변수 만들어서 깎아 주기
        }

        if (Hp <= 0)//체력이 0 이하일시 (enemy가 죽었을 시)
        {
            GetComponent<monsterAI>().state = monsterAI.State.DIE;//monsterAI state에 접근
            GetComponent<BoxCollider2D>().enabled = false;//죽으면 콜라이더 꺼주기 >> 이유: 죽은 애한테 피격 충돌 나지 않기 위해

        }
    }
}
