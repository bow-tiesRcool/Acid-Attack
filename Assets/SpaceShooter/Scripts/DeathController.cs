using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour {

    public void Die()
    {
        gameObject.SetActive(true);
        StartCoroutine("DeathTimer");
    }
    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }	
}
