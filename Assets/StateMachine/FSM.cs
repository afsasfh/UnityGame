using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hit, Death
}

[Serializable]
public class Parameter
{

    [Header("怪物参数")]
    public float health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public float patroldistance;
    public float chasedistance;

    [Header("其他参数")]
    public List<Vector3> patrolPoints=new List<Vector3>();
    public List<Vector3> chasePoints = new List<Vector3>();
    public Transform target;
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea;
    public Animator animator;
    public bool getHit;
   
}
public class FSM : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;//记录怪物初始颜色

    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    public Parameter parameter;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        parameter.patrolPoints.Add(new Vector3(transform.position.x - parameter.patroldistance, transform.position.y, transform.position.z));
        parameter.patrolPoints.Add(new Vector3(transform.position.x + parameter.patroldistance, transform.position.y, transform.position.z));
        parameter.chasePoints.Add( new Vector3(transform.position.x - parameter.chasedistance, transform.position.y, transform.position.z));
        parameter.chasePoints.Add( new Vector3(transform.position.x + parameter.chasedistance, transform.position.y, transform.position.z));
        parameter.animator = transform.GetComponent<Animator>();
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));

        TransitionState(StateType.Idle);


    }

    public bool PlayerAttack =false;
    void Update()
    {
        currentState.OnUpdate();

        if (PlayerAttack)
        {
            parameter.getHit = true;
            PlayerAttack = false;
        }
    }

    void FlashColor(float time)
    {
        sr.color = Color.red;
        Invoke("ResetColor",time);
    }
     void ResetColor()
    {
        sr.color = originalColor;
    }
    public void TransitionState(StateType type)
    {
        if (currentState! != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    public void FlipTo(float x)
    {
            if (transform.position.x > x)
            {
                transform.localScale = new Vector3(-1*MathF.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
            else if (transform.position.x < x)
            {
                transform.localScale = new Vector3(MathF.Abs(transform.localScale.x), transform.localScale.y, 1);
            }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }
  /*  private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }*/
    

    public void TakeDamage(float damage)
    {
        parameter.health -= damage;
        PlayerAttack = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea);
    }


    public void EnemyDeath()
    {
        Destroy(gameObject);
    }
}