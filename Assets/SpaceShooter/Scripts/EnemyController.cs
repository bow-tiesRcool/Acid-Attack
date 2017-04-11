using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float speed = 15;
    public float ShootSpeed = 50;
    private Rigidbody2D body;
    public int points = 20;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        body.velocity = Vector2.left * speed;
        OffScreenCheck();
    }

    void OffScreenCheck()
    {
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.x < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
           // StartCoroutine("deathTimer");
            gameObject.SetActive(false);
            GameManager.Points(points);
            SpawnPowerUp();
            
        }
        if (collision.gameObject.tag == "shield")
        {
           // StartCoroutine("deathTimer");
            gameObject.SetActive(false);
            
        }
    }

    void SpawnPowerUp()
    {
        if (Random.value < GameManager.instance.powerUpChance)
        {
            GameManager.instance.DropPowerUp(transform.position);
        }
    }

    IEnumerator deathTimer()
    {
        GameObject death = Spawner.Spawn("EnemyDeath");
        death.transform.position = transform.position;
        death.SetActive(true);
        yield return new WaitForSeconds(1);
        death.SetActive(false);
    }
}