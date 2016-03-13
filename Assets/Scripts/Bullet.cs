using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int dmg = 1;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
