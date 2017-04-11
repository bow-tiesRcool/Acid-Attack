using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    private Rigidbody2D body;

    public int speed = 20;

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

}
