using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour {

    public ParticleSystem explosion;
    List<ParticleSystem> particlePool = new List<ParticleSystem>();

    public void Explode()
    {
        ParticleSystem explosionPrefab = null;
        for (int i = 0; i < particlePool.Count; i++)
        {
            ParticleSystem p = particlePool[i];
            if (p.isStopped)
            {
                explosion = p;
                Debug.Log("reusing from my pool");
                break;
            }
        }

        if (explosion == null)
        {
            explosion = Instantiate(explosionPrefab) as ParticleSystem;
            particlePool.Add(explosion);
        }
        explosion.gameObject.SetActive(true);
        explosion.Play();
    }
}
