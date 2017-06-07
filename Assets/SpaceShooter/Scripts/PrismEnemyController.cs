﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismEnemyController : MonoBehaviour {

    public float speed = 10;
    public float ShootSpeed = 50;
    private Rigidbody2D body;
    public Vector3 view;
    public int points = 10;
    GameObject death;

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
        yield return new WaitForSeconds(Random.Range(1, 4));
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
            for (int i = 0; i <= 20; i++)
            {
                GameObject bullet = Spawner.Spawn("AlienAttack");
                bullet.transform.position = transform.position;
                bullet.GetComponent<BulletControllerEnemy>().Fire(Vector2.left);
                yield return new WaitForSeconds(0);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);
            GameManager.Points(points);
            SpawnPowerUp();

        }

        if (collision.gameObject.tag == "shield")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);

        }
        if (collision.gameObject.tag == "Player")
        {
            death = Spawner.Spawn("EnemyDeath");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            AudioManager.PlayEffect("Randomize33", 1, 1);
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
}