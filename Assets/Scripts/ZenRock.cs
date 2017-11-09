using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenRock : MonoBehaviour {

    void OnCollisionEnter(Collision collision )
    {
        string tag = collision.collider.tag;
        if (tag == "sand" || tag == "rock")
        {
            SoundManager.Instance.PlayRockClip();
        }
        print(tag);
    }
}
