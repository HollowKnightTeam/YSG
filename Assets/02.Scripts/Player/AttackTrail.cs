using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public Rigidbody2D player;
    PlayerCtrl playerCtrl;
    int power=5;

    void Awake()
    {
        player = player.GetComponent<Rigidbody2D>();
        playerCtrl = player.gameObject.GetComponent<PlayerCtrl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCtrl.attackOffset = true;
        hitEffect.transform.position = collision.bounds.ClosestPoint(transform.position);
        hitEffect.gameObject.SetActive(true);
        hitEffect.Play();
    }

}
