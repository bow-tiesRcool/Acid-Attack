using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public static ShieldController instance;
    int hit = 0;
    public bool activeShield;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (instance.hit >= 3)
        {
            instance.gameObject.SetActive(false);
            instance.activeShield = false;
            instance.hit = 0;
        }
    }

    public static void ShieldActive(bool active = false)
    {
        instance.gameObject.SetActive(active);
        instance.activeShield = active;
    }

    public static void HitShield(int damage)
    {
        instance.hit += damage;
        Debug.Log(instance.hit);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activeShield)
        {
            if (collision.gameObject.tag == "AlienAttack")
            {
                HitShield(1);
            }
            if (collision.gameObject.tag == "EyeAlien")
            {
                HitShield(1);
            }
            if (collision.gameObject.tag == "Alien")
            {
                HitShield(1);
            }
        }
    }
}
