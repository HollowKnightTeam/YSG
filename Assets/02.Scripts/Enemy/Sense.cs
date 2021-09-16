using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    Boss boss;

    private void Awake()
    {
        boss = GameObject.Find("MantisLords").GetComponent<Boss>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            boss.BossHead_Up();
            boss.canStart = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            boss.canStart = false;
    }

}
