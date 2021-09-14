using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.Find("MantisLords").GetComponent<Boss>().BossHead_Up();
        }
    }


    void Start()
    {

    }


    void Update()
    {

    }
}
