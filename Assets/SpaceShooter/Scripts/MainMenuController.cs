using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	void Start ()
    {
		if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }
	}
}
