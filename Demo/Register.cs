using System.Collections;
using System.Collections.Generic;
using UnityEngine;
		
using MessageSystem;
public class Register : MonoBehaviour {
    MessageManager msgManater;

    // Use this for initialization
    void Start () {
        msgManater = MessageSystem.MessageManager.GetInstance();

        msgManater.AddListener<Color>("turnRed", OnReceive);
        msgManater.AddListener<Quaternion,float>("rotate", OnReceive);
        msgManater.AddListener("destroy", Destroy);
    }
	
	// Update is called once per frame
	void OnReceive (Color color) {
        GetComponent<Renderer>().material.color = color;
	}
    // Update is called once per frame
    void OnReceive(Quaternion rot,float time)
    {
        Debug.Log(time);
        transform.rotation = rot;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
