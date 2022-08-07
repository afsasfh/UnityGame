using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlier : MonoBehaviour {

	private Rigidbody2D rb;
	private Animator anim;
	private Collider2D coll;
	public float maxHp = 100.0f;
	public float currentHp = 10.0f;

	//没用
	private bool Ishurt;//默认是false
	public float speed;
	public float jumpforce;
	public LayerMask ground;
	public Transform groundCheck;
	//public Text CarrotNum;

	bool jumpPressed;
	public bool isGround, isJump;
	public int jumpCountmax;
	private int jumpCount;



    //public int Carrot=0;

    private void Awake()
    {
		currentHp = maxHp;
    }

    private float facedircetion;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		coll = GetComponent<Collider2D>();
		isJump = true;
		jumpCount = jumpCountmax;
	}
	
	// Update is called once per frame
	void Update () {

		isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
		Jump();


		if (!Ishurt)
        {
           Moventment();
        }
		

	}
	public void FixedUpdate()
	{
		GroundMovement();
		SwitchAnim();
		
	}


	void Moventment()
    {

		float horizontalmove = Input.GetAxis("Horizontal");//这个单词取值1、0、-1；
		facedircetion = Input.GetAxisRaw("Horizontal");
		//角色移动


		float SlowDownSpeed;
		if (facedircetion != 0)
			SlowDownSpeed = 1;
		else
			SlowDownSpeed = .1f;
		rb.velocity = new Vector2(horizontalmove * speed* SlowDownSpeed, rb.velocity.y);
		if (jumpCount==jumpCountmax)
		{
			anim.SetFloat("running", Mathf.Abs(facedircetion));
		}

		if (facedircetion!=0)
		{
			transform.localScale = new Vector2(facedircetion*-1.5f, 1.5f);
		}
		
	}

	void GroundMovement()
	{
		float horizontalMove = Input.GetAxisRaw("Horizontal");

		rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

		if (horizontalMove != 0)
		{
			transform.localScale = new Vector3(horizontalMove, 1, 1);
		}
	}
	void Jump()
	{
		if (Input.GetButtonDown("Jump") && jumpCount > 0)
		{
			jumpCount-=1;
			anim.SetFloat("running",0);
			jumpPressed = true;
			rb.velocity = new Vector2(rb.velocity.x, jumpforce);
			GetComponent<Animator>().Play("jump");
		}
		
	}

	public void IsJumping()
	{
		isJump = true;
	}
	public void Jumping()
    {
		isJump = false;
		jumpPressed = false;
	}
	public void JumpEnd()
    {	
		anim.Play("idle");
	}
	//切换动画效果	
	void SwitchAnim()
    {
     
        if (!isGround && !jumpPressed)
        {
			anim.Play("fall");
			isJump = true;
		}	
		if(isGround && isJump)
        {
			isJump = false;
			jumpCount = jumpCountmax;
			anim.Play("Drop");
        }
		
	}

	
	//收集物品
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collections")
        {
			Destroy(collision.gameObject);
			//Carrot += 1;
			//CarrotNum.text = Carrot.ToString();
        }else if(collision.tag == "EnemyAttack")
        {
			Debug.Log("I have hitted" + collision.name);
			currentHp -= 10.0f;
			Debug.Log("Current Hp is " + currentHp);
        }
    }
    //消灭敌人
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "Enemy")
		{

			if (anim.GetBool("falling"))
			{
				Destroy(collision.gameObject);
				rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
				anim.SetBool("jumping", true);
			}
			else if(transform.position.x < collision.gameObject.transform.position.x)
            {
				rb.velocity = new Vector2(-10, rb.velocity.y);
				Ishurt = true;
            }
			else if (transform.position.x > collision.gameObject.transform.position.x)
			{
				rb.velocity = new Vector2(10, rb.velocity.y);
				Ishurt = true;
			}
		}
    }*/
}
