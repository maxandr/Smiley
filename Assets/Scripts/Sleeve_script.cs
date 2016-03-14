using UnityEngine;
using System.Collections;

public class Sleeve_script : MonoBehaviour {
	public float velocity;
	// Use this for initialization
	void Start () {
		///transform.GetComponent<Rigidbody2D> ().AddForce(transform.up);
		//transform.GetComponent<Rigidbody2D> ().velocity = transform.up*velocity;
		Destroy (gameObject, 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Create(bool pFacing) {
		if (pFacing) {
			transform.GetComponent<Rigidbody2D> ().AddForce (transform.up);
			transform.GetComponent<Rigidbody2D> ().velocity = transform.up * velocity;
		} else {
			transform.GetComponent<Rigidbody2D> ().AddForce (transform.up*-1);
			transform.GetComponent<Rigidbody2D> ().velocity = transform.up * velocity*-1;
		}
		//transform.GetComponent<Rigidbody2D> ().AddForce(transform.up);
	}
}
