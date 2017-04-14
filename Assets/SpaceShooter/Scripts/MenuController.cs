using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public string spaceShooter;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        while (enabled)
        {
            Cursor.visible = false;
            if (Input.GetButtonDown("Fire1"))
            {
                SceneManager.LoadScene(spaceShooter);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
