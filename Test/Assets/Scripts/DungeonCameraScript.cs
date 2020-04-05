using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCameraScript : MonoBehaviour {
    public Vector2 speed = new Vector2(30, 30);

    // 2 - направление движения
    private Vector3 movement;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        movement = new Vector3(
          speed.y * inputY,0,
          -speed.x * inputX);
        transform.position += movement;
    }
}
