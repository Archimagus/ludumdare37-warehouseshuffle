using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] DeliveryTruckSlots;
    public GameObject[] PickupTruckSlots;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            Truck t;

            if (Random.Range(0, 100) > 50)
                t = DeliveryTruckSlots[Random.Range(0, DeliveryTruckSlots.Length)].GetComponentInChildren<Truck>();              
            else
                t = PickupTruckSlots[Random.Range(0, PickupTruckSlots.Length)].GetComponentInChildren<Truck>();

            t = DeliveryTruckSlots[0].GetComponentInChildren<Truck>();

            if (t.IsOffscreen)
                t.Arrive();
        }
    }
}