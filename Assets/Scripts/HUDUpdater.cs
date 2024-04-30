// #define _S_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdater : MonoBehaviour {

    public FinishLine FinishLine;

    public Text Text;

    [HideInInspector]
    public PlayerData data;

    private void Start()
    {
        for (int i = 0; i < FinishLine.playerData.Count; i++)
        {
            if(FinishLine.playerData[i].Transform == GetComponentInChildren<Collider>().transform)
            {
                data = FinishLine.playerData[i];
                break;
            }
        }
    }

    void Update () {
        Vector3 velocity = GetComponent<Ship>().getVelocity();
        Vector2 velocity2 = new Vector2 (velocity.x, velocity.z);

        Text.text = "Time: " + data.CurrentTime.ToString("hh':'mm':'ss':'FF") +  // remove trailing 0's
                    "\nLap: " + data.Laps.ToString() +
                    "\nSpeed: " + velocity2.magnitude.ToString("f0");

#if _S_DEBUG
        Text.text += "\nReal Speed: " + velocity.magnitude.ToString("f0") +
                     "\nPosition: (" + data.Transform.position.x.ToString() +
                     ", " + data.Transform.position.y.ToString() + 
                     ", " + data.Transform.position.z.ToString() + ")";
#endif

        if (data.BestTime < new TimeSpan(1, 0, 0))
        {
            Text.text += "\nWorld Record: " + data.BestTime;
        }
	}
}
