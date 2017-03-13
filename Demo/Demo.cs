using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageSystem;

public class Demo : MonoBehaviour {
    MessageManager msgManater;
    private void Awake()
    {
        msgManater = MessageSystem.MessageManager.GetInstance();
    }
    private void OnGUI()
    {
        if (GUILayout.Button("TurnRed"))
        {
            msgManater.NotifyObserver<Color>("turnRed", Color.red);
        }
        if (GUILayout.Button("Rot"))
        {
            msgManater.NotifyObserver<Quaternion, float>("rotate", Quaternion.identity, 3);
        }
        if (GUILayout.Button("Destroy"))
        {
            msgManater.NotifyObserver("destroy");
        }
        if (GUILayout.Button("delyTurnBlue"))
        {
            msgManater.NotifyObserver<Color>("turnRed", Color.blue, this, 2);
        }
    }
}
