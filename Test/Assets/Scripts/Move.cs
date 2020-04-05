using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	public int x;
	public int y;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float m_x = Input.GetAxis("Horizontal");
		float m_y = Input.GetAxis("Vertical");
		transform.Translate(new Vector3( Time.deltaTime*x*m_x, Time.deltaTime*y*m_y));
	}
}
