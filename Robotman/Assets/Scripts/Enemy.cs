using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
    public float speed = 0f;
    public float collisionRadius = 0.55f;
    public int health = 2;
    public GameObject explosion;
    public LayerMask playerLayer;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //float dist = Vector2.Distance(playerTransform.position, transform.position);       
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < playerTransform.position.x)
            transform.right = Vector2.left;
        else
            transform.right = Vector2.right;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        PlayerCollision();

        if (health == 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (distance > 18f)
            Destroy(gameObject);
    }

    public void TakeDamage(int damageValue)
    {
        health -= damageValue;       
    }

    void PlayerCollision() {

        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position, collisionRadius, playerLayer);

        if (hit != null)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Robot>().TakeDamage(1);
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }
}
