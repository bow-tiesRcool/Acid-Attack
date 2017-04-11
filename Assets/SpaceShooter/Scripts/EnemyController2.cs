using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2 : MonoBehaviour {
    public float speed = 10;
    public float ShootSpeed = 50;
    private Rigidbody2D body;
    public Vector3 view;
    public int points = 10;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine("EnemyMovement");
    }

    void Update()
    {
        view = Camera.main.WorldToViewportPoint(transform.position);
        OffScreenCheck();
    }

    void OffScreenCheck()
    {
        if (view.x < 0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator EnemyMovement()
    {
        body.velocity = Vector2.left * speed;
        yield return new WaitForSeconds(Random.Range(1, 5));
        body.velocity = Vector2.up * speed;
        StartCoroutine("Attack");
        while (enabled)
        {
            
            if (view.y > .9f)
            {
                body.velocity = Vector2.down * speed;
            }
            if (view.y < 0.1f)
            {
                body.velocity = Vector2.up * speed;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Attack()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            GameObject bullet = Spawner.Spawn("AlienAttack");
            bullet.transform.position = transform.position;
            bullet.GetComponent<BulletControllerEnemy>().Fire(Vector2.left);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            //StartCoroutine("deathTimer");
            gameObject.SetActive(false);
            GameManager.Points(points);
            SpawnPowerUp();
            
        }

        if (collision.gameObject.tag == "shield")
        {
            //StartCoroutine("deathTimer");
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