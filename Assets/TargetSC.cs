using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSC : MonoBehaviour {

    public GameObject target;
    PlayerSC pSC;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerSC>().isRange = true;
            collision.gameObject.GetComponent<PlayerSC>().targetPos = this.transform.position;
            target.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerSC>().isRange = false;
            target.GetComponent<Renderer>().material.color = Color.green;
        }
    }
}
