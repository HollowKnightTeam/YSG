using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    Image soulBar;
    float maxSoul = 9f;//maxSoul = 9 / 적 3번 때리면 활성화 / 스킬 or hp회복 한 번 쓰면 -3
    public static float soul;
    GameObject player;

    void Start()
    {
        soulBar = GetComponent<Image>();
        soulBar.fillAmount = maxSoul;
    }

    void Update()
    {
        soulBar.fillAmount = soul / maxSoul;


        //20210825BJY=====================

        //===============================
        //플레이어 hp 상속 받아서 UI hp 등등 조절 
        //상속 안 받아도 될 듯? 아닌가???        
    }
}
