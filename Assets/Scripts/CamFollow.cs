using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {

    Transform player_transform;
	// Use this for initialization
	void Start () {
        player_transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(player_transform.position.x, player_transform.position.y, player_transform.position.z - 5);
	}
}
