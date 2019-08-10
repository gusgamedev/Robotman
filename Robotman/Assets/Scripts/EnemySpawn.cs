using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Enemy[] enemies;
    public float spawnTime = 4f;

    private bool canSpawn = true;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > 10f && distance < 13f && canSpawn)
            StartCoroutine(Spawn());

        
    }

    IEnumerator Spawn()
    {
        canSpawn = false;
        Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;

    }
}
