using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCreatorScript : MonoBehaviour {

    public Transform cube;
    public Material[] materials;
	// Use this for initialization
	void Start ()
    {
        System.Random rnd = new System.Random();
        for (int i = -128; i < 128; i++)
        {
            for (int j = -128; j < 128; j++)
            {
                int y = 10 * rnd.Next(-3, 0);
                int mat = rnd.Next(materials.Length);
                Transform t = Instantiate(cube, new Vector3(10*i,y, 10*j), new Quaternion(0, 0, 0, 0));
                t.GetComponent<MeshRenderer>().materials = new Material[] { materials[mat] };
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
