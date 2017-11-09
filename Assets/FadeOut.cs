using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour {
    Image image;
    Text text;
	// Use this for initialization
	void Start ()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - Time.deltaTime/10f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime/10f);

    }
}
