﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEnemyController : MonoBehaviour {

    public float speed = 15;
    public float ShootSpeed = 30;
    private Rigidbody2D body;
    public Vector3 view;
    public int points = 20;
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
        yield return new WaitForSeconds(Random.Range(1, 2));
        body.velocity = Vector2.left * 0;
        yield return new WaitForSeconds(Random.Range(1, 2));
        body.velocity = Vector2.left * ShootSpeed;
        yield return new WaitForEndOfFrame();

    }

    IEnumerator Attack()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            body.velocity = Vector2.left * ShootSpeed;
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
