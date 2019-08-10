using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
    
    public float speed = 7f;
    public float jumpForce = 12f;
    public int health = 1;

    public Shot shot;
    public Transform spawnShot;

    public float fireRate = 0.2f;
    private bool canShoot = true;
    private bool canTakeDamage = true;

    public Transform foot;
    public float collisionRadius = 0.2f;
    public LayerMask groundLayer;

    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb2d;

    private bool onFloor = false;
    private bool jump = false;
    private bool isJumping = false;
    private AudioSource jumpFx;

    Animator anim;


    private void Start() 
    {
        rb2d = GetComponent<Rigidbody2D>();  
        anim = GetComponentInChildren<Animator>();  
        jumpFx = GetComponent<AudioSource>();
    }

    private void Update() 
    {
        if (health <= 0)
            return;
        
        if (Input.GetButtonDown("Jump") && onFloor)
            jump = true;

        if (Input.GetButton("Shoot") && canShoot)
            StartCoroutine(Shoot());

        GroundCheck();

        anim.SetBool("jumping", !onFloor);
        anim.SetBool("shooting", Input.GetButton("Shoot"));
        anim.SetBool("running", Input.GetAxisRaw("Horizontal") != 0);
            
    }

    private void FixedUpdate() 
    {
        if (health <= 0)
            return;
        
        Move(Input.GetAxisRaw("Horizontal"));

        if (jump)
        {
            jump = false;
            jumpFx.Play();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
         
    }

    private void Move(float move)
    {        
        if (move > 0)
            direction = Vector2.right;
        if (move < 0)
            direction = Vector2.left;

        transform.right = direction;

        if (move == 0)
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        else
            rb2d.velocity = new Vector2(direction.x * speed, rb2d.velocity.y);  
    }

    void GroundCheck()
    {
        if (rb2d.velocity.y > 0.1f && !onFloor)
            isJumping = true;
        else
            isJumping = false;

        onFloor = !isJumping && Physics2D.OverlapCircle((Vector2)foot.position, collisionRadius, groundLayer);

    }

    IEnumerator Shoot()
    {
        canShoot = false;
        Instantiate(shot, spawnShot.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage && health > 0)
        {
            canTakeDamage = false;
            health -= damage;
            //damageEffect.SetFlashDamage();           
            Invoke("SetCanTakeDamage", 1.5f);
        }

        if (health <= 0)
        {
            anim.SetTrigger("die");
            rb2d.bodyType = RigidbodyType2D.Static;
            Level.instance.LevelFailed();
            

        }
    }

    void SetCanTakeDamage() {
        canTakeDamage = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
            TakeDamage(health);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireSphere((Vector2)foot.position, collisionRadius);

    }

 
}