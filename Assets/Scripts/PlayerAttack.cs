using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerAttack : MonoBehaviour
{
    public int damage;
    private Animator anim;
    private PolygonCollider2D coll2D;
    public float time;
    public float Starttime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll2D = GetComponent<PolygonCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (Input.GetButton("Attack"))
        {
           
            anim.SetTrigger("Attack");
            StartCoroutine(StartAttack());
        }
    }
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(Starttime);
        coll2D.enabled = true;
        StartCoroutine(disableHitBox());
    }
    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(time);
        coll2D.enabled = false;
    }
    
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
          // other.GetComponent<FSM>().TakeDamage(damage);
           

        }
    }

}
