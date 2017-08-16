using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeAlienBossController : MonoBehaviour {

    public float speed = 10;
    private Rigidbody2D body;
    public int points = 100;
    public int life = 50;
    GameObject death;

    public void Start()
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
            GameManager.instance.BossDefeated();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            LifeOfBoss(1);
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "shield")
        {
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
        if (collision.gameObject.tag == "HoldAttack")
        {
            LifeOfBoss(25);
            AudioManager.PlayEffect("Randomize33", 1, 1);
        }
    }

    void SpawnPowerUp()
    {
        if (Random.value < GameManager.instance.powerUpChance)
        {
            GameManager.instance.DropPowerUp(transform.position);
        }
    }

    void LifeOfBoss(int damage)
    {
        life -= damage;

        if (life <= 0)
        {
            death = Spawner.Spawn("EnemyDeathBoss");
            death.transform.position = transform.position;
            death.GetComponent<DeathController>().Die();
            GameManager.Points(points);
            AudioManager.PlayEffect("Randomize33", 1, 1);
            gameObject.SetActive(false);
            GameManager.instance.BossDefeated();
            life = 50;
        }
    }
}
