using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    float moveSpeed = 10;
    float h;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move() //이동 함수
    {
        
        h = Input.GetAxisRaw("Horizontal");
       
        if (h > 0)
        {
            transform.Translate(new Vector3(h, 0, 0) * moveSpeed * Time.deltaTime);
        }

        else if (h < 0)
        {
            transform.Translate(new Vector3(-h, 0, 0) * moveSpeed * Time.deltaTime);
        }

    }





}
