// [SL] Ship in World Checker
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInWorld : MonoBehaviour
{
    public ShipPreparer ShipPreparer;
    private Transform ship;
    private Rigidbody shipBody;

    public float BottomY = -10f;
    public float OnTrackIntv = 0.2f;  // should be a multiple of fixed timestep
    public float TimePenalty = 5.0f;

    private bool onTrack;
    private int counter;
    private List<Vector3> lastPosition = new List<Vector3>();
    private List<Quaternion> lastRotation = new List<Quaternion>();

    void Start()
    {
        ship = ShipPreparer.ship;
        transform.SetParent(ship);
        shipBody = ship.GetComponentInChildren<Rigidbody>();

        for (int i = 0; i < 3; i++) {  // buffer 3 valid states on track
            lastPosition.Add(Vector3.zero);
            lastRotation.Add(Quaternion.identity);
        }

        onTrack = false;
        counter = 0;
    }

    void FixedUpdate()
    {
        if (++counter == (int)(OnTrackIntv / Time.fixedDeltaTime)) {
            if (onTrack) {  // not falling
                lastPosition[0] = lastPosition[1];
                lastRotation[0] = lastRotation[1];
                
                lastPosition[1] = lastPosition[2];
                lastRotation[1] = lastRotation[2];

                lastPosition[2] = shipBody.position;
                lastRotation[2] = shipBody.rotation;
                
                // Debug.Log(lastPosition[2].ToString());
            }
            counter = 0;
        }

        if (ship.position.y < BottomY) {
            shipBody.position = lastPosition[0];
            shipBody.rotation = lastRotation[0];
            shipBody.velocity = Vector3.zero;

            // Debug.Log("You dropped out of world!");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "FinishLine" || other.name == "CheckPoint") return;
        onTrack = true;

        // Debug.Log("Start overlapping with " + other.name);
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.name == "FinishLine" || other.name == "CheckPoint") return;
        onTrack = false;

        // Debug.Log("End overlapping with " + other.name);
    }
}
