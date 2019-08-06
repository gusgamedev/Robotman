using UnityEngine;

public class Shot : MonoBehaviour 
{
    [SerializeField] private float speed = 20f;
	[SerializeField] private int damage = 1;
	[SerializeField] private GameObject _fireParticle;
	
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        
		rb2d = GetComponent<Rigidbody2D>();        

        rb2d.velocity = transform.right * speed;
        Destroy(gameObject, 2f);
	}

	private void OnTriggerEnter2D(Collider2D other) 
    {
        //TODO    
    }
    

}