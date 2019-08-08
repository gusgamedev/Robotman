using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
    
    public float speed = 7f;
    public float jumpForce = 12f;

    public Shot shot;
    public Transform spawnShot;

    public float fireRate = 0.2f;
    private bool canShoot = true;

    public Transform foot;
    public float collisionRadius = 0.2f;
    public LayerMask groundLayer;

    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb2d;

    private bool onFloor = false;
    private bool jump = false;
    private bool isJumping = false;

    Animator anim;


    private void Start() 
    {
        rb2d = GetComponent<Rigidbody2D>();  
        anim = GetComponentInChildren<Animator>();  
    }

    private void Update() 
    {        
        if (Input.GetButtonDown("Jump") &&  onFloor)
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
        Move( Input.GetAxisRaw("Horizontal") );      

        if (jump)
        {
            jump = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireSphere((Vector2)foot.position, collisionRadius);

    }

 
}