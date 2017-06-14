using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerLevel : MonoBehaviour {

    //public static MenuControllerLevel instance;
    public string level;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        instance.level = this.level;
    //        Destroy(gameObject);
    //    }
    //}

    private void OnEnable()
    {
        StartCoroutine("NextLevel");
    }

    IEnumerator NextLevel()
    {
        Debug.Log(name);
        yield return new WaitForSeconds(2);
        while (enabled)
        {
            Cursor.visible = false;
            if (Input.GetButton("Fire1"))
            {
                SceneManager.LoadScene(level);
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
