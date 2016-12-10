using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] DeliveryTruckSlots;
	public GameObject[] PickupTruckSlots;

	[SerializeField]
	float _deliveryTruckSpawnDelay;
	[SerializeField]
	float _pickupTruckSpawnDelay;
	[SerializeField]
	int _maxDeliveryTrucks;
	[SerializeField]
	int _maxPickupTrucks;
	[SerializeField]
	float _deliveryTruckSpawnTimer;
	[SerializeField]
	float _pickupTruckSpawnTimer;
	[SerializeField]
	int _currDeliveryTrucks;
	[SerializeField]
	int _currPickupTrucks;

	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
				_instance = FindObjectOfType<GameManager>();
			if (_instance == null)
			{
				Debug.LogError("No GameManager in the scene");
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start()
	{
		Init();
	}

	// Update is called once per frame
	void Update()
	{
		SpawnTruck();
	}

	void Init()
	{
		_deliveryTruckSpawnTimer = _deliveryTruckSpawnDelay = 6;
		_pickupTruckSpawnTimer = _pickupTruckSpawnDelay = 6;
		_maxDeliveryTrucks = 3;
		_maxPickupTrucks = 3;
		_currDeliveryTrucks = 0;
		_currPickupTrucks = 0;
	}

	public void TruckIsOffscreen(bool isDeliveryTruck)
	{
		if (isDeliveryTruck)
			_currDeliveryTrucks--;
		else
			_currPickupTrucks--;
	}

	void SpawnTruck()
	{
		_deliveryTruckSpawnTimer -= Time.deltaTime;

		if (_deliveryTruckSpawnTimer <= 0.0f)
		{
			_deliveryTruckSpawnTimer = _deliveryTruckSpawnDelay;

			if (_currDeliveryTrucks < _maxDeliveryTrucks)
			{
				Truck t = DeliveryTruckSlots[Random.Range(0, DeliveryTruckSlots.Length)].GetComponentInChildren<Truck>();

				while (!t.IsOffscreen)
				{
					t = DeliveryTruckSlots[Random.Range(0, DeliveryTruckSlots.Length)].GetComponentInChildren<Truck>();
				}

				t.Arrive();
				_currDeliveryTrucks++;
			}
		}
	}
}