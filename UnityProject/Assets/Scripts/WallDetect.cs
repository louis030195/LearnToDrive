using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetect : MonoBehaviour {

    public DriveAgent agent;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wall"))
            agent.HasHitWall();
    }
}
