﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

	public void Update()
    {
        if (Input.GetButtonUp("Fire2"))
        {
            Application.Quit();
        }
	}
}
