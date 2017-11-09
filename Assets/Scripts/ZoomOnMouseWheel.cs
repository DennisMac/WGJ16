using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOnMouseWheel : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
        //if (Input.GetKey(KeyCode.LeftShift))
        {
            float dZ = 10 * Input.GetAxis("Mouse ScrollWheel");
            this.transform.localPosition += new Vector3(0, 0, dZ);
        }
	}
}
