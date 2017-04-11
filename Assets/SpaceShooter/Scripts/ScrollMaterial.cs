using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMaterial : MonoBehaviour {

    public float scrollSpeed = 1;
    private float offset = 0;

	void Update ()
    {
        offset += scrollSpeed * Time.deltaTime;
        offset %= 1f;
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
	}
}
