using UnityEngine;
using System.Collections;

public class Robot : MonoBehaviour {
    
    public float speed = 7f;
    public float jumpForce = 12f;

    public Shot shot;
    public Transform spawnShot;

    public float fireRate = 0.2f;
    private bool canShoot = true;

    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb2d;

    private bool onFloor = true;
    private bool jump = false;
    

    private void Start() 
    {
        rb2d = GetComponent<Rigidbody2D>();  
        StartCoroutine(Example());      
    }

    private void Update() 
    {        
        if (Input.GetButtonDown("Jump") &&  onFloor)
            jump = true; 

        if (Input.GetButton("Shoot") && canShoot)
            StartCoroutine(Shoot());
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
        else if (move < 0)
            direction = Vector2.left;
        else
            direction = Vector2.zero;

        transform.right = direction;
        rb2d.velocity = new Vector2(direction.x * speed, rb2d.velocity.y);  
    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(5);
        print(Time.time);
    }

    IEnumerator Shoot() 
    {
        canShoot = false;
        Instantiate(shot, spawnShot.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        canShoot = true;        
    }
}