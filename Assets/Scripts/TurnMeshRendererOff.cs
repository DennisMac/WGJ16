﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnMeshRendererOff : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<MeshRenderer>().enabled = false;
		
	}
	
}
