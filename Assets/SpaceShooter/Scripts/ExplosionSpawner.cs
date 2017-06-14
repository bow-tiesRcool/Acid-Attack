using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpawner : MonoBehaviour {

    public static ExplosionSpawner instance;

    public ExplosionController explosionPrefab;

    private List<ExplosionController> explosionPool = new List<ExplosionController>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Reset()
    {
        explosionPool = new List<ExplosionController>();
    } 

    public void SpawnExplosion(Vector3 position)
    {
        instance.ExplosionCreator(position);
    }

    void ExplosionCreator(Vector3 position)
    {
        ExplosionController explosion = null;
        for (int i = 0; i < explosionPool.Count; i++)
        {
            if (!explosionPool[i].gameObject.activeSelf)
            {
                explosion = explosionPool[i];
                break;
            }
        }
        if (explosion == null)
        {
            explosion = Instantiate(explosionPrefab) as ExplosionController;
            explosionPool.Add(explosion);
        }
        explosion.gameObject.SetActive(true);
        explosion.transform.position = position;
        explosion.Explode();
    }
}
