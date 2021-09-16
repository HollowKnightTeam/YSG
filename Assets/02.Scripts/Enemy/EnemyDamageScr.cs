using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageScr : MonoBehaviour
{

    public float hp = 20; //몬스터 체력

    SpriteRenderer sr;
    Shader normal;
    Shader white;


    Vector3 dir;

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
            if (TryGetComponent<monsterAI>(out monsterAI monster)|| TryGetComponent<Boss>(out Boss boss) )
            {
                hitEffect = false;
                StartCoroutine(HIT(0,null));
            }
            else if(TryGetComponent<Shade>(out Shade shade))
            {
                hitEffect = false;
                StartCoroutine(HIT(1,shade));
            }

        }
        if (hp <= 0)//체력이 0 이하일시 (enemy가 죽었을 시)
        {
            if (TryGetComponent<monsterAI>(out monsterAI monster))
            {
                monster.state = monsterAI.State.DIE;//monsterAI state에 접근
                monster.gameObject.GetComponent<BoxCollider2D>().enabled = false; //사망 시 콜라이더 비활성화
            }
            else if (TryGetComponent<Shade>(out Shade shade))
            {
                shade.state = Shade.State.DEATH;
                shade.isDie = true;
            }
        }
    }

    private IEnumerator HIT(int type, Shade shade)
    {
        switch (type)
        {
            case 0:
                sr.material.shader = white;
                yield return new WaitForSeconds(0.5f);
                sr.material.shader = normal;
                break;

            case 1:
                shade.damageVoid.transform.forward = -dir;
                shade.damageVoid.Play();
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            dir = ((transform.position + new Vector3(0,0,0.13f))-collision.gameObject.transform.position).normalized;
            print(dir);
            hitEffect = true;
            hp -= collision.gameObject.GetComponent<AttackTrail>().power;//player 데미지 변수 만들어서 깎아 주기
        }

    }
}
