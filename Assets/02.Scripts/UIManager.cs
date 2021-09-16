using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 ==================UI 설명==========================
    soulBar :   1. Max Bar Mp == 9
                   2. 적 때리면 1칸++
                   3. 장풍 쏘면 3칸 --
                    4. 회복하면 3칸--

    Hp :  1. Max Hp == 5;
            2. Enemy한테 맞으면 1칸 --;
            3. 의자 근처에 가면 완전히 회복
            4. soulBar를 이용하여 회복하면 1칸++;

    영혼 회수 안 했을 때 6이상 currMp 안 채워지게하기
    영혼 회수했을 시 currMp 제한 풀기

===================================================
 */


public class UIManager : MonoBehaviour
{
    [SerializeField] Image soulBar;
    GameObject player;
    PlayerCtrl playerCtrl;

    //플레이어 뒤졌는지 판단하고 부활하면 풀피

    int Hp = 5;
    float maxSoul; //초기체력//maxSoul = 9 / 적 3번 때리면 활성화 / 스킬 or hp회복 한 번 쓰면 -3
    float curSoul;//현재 체력
    public GameObject[] hpIcon;
    bool reset = false;

    void Start()
    {
        maxSoul = GameObject.Find("Player").GetComponent<PlayerCtrl>().maxMp;

    }

    void DisplayBar()
    {
        soulBar.fillAmount = (curSoul / maxSoul);
    }

    void Update()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        curSoul = GameObject.Find("Player").GetComponent<PlayerCtrl>().curMp;
        Hp = GameObject.Find("Player").GetComponent<PlayerCtrl>().hp;


        DisplayBar();
        if (curSoul < 0)
        {
            curSoul = 0;
        }
        StartCoroutine(isHit());
        Reset_();


    }


    IEnumerator isHit()
    {
        Damage__();

        Debug.Log("코루틴 작동");
        yield return null;

    }


    void Damage__()
    {
        if (Hp < 5)
        {
            hpIcon[Hp].SetActive(false);
        }

        
    }

    private void Reset_()
    {

        for (int a = 1, hp_ = Hp; (hp_ - a) >= 0; a++){
            hpIcon[hp_ - a].SetActive(true);
        }
    }


}
