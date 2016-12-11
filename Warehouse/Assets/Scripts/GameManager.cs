using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject[] DeliveryTruckSlots;
	public GameObject[] PickupTruckSlots;
	public WarehouseItem[] WarehouseItems;
	public UnityEngine.UI.Text MoneyText;

	[Header("System Variables")]
	[SerializeField]
	float _initialDeliveryTruckDelay = 0;
	[SerializeField]
	float _deliveryTruckSpawnDelay = 10;
	[SerializeField]
	private float _deliveryTruckWaitTime = 3.0f;
	[Space]
	[SerializeField]
	float _initialPickupTruckDelay = 60.0f;
	[SerializeField]
	float _pickupTruckSpawnDelay = 20;
	[SerializeField]
	private float _pickupTruckWaitTime = 3.0f;
	[Space]
	[SerializeField]
	int _maxDeliveryTrucks = 3;
	[SerializeField]
	int _maxPickupTrucks = 3;
	[SerializeField]
	int _playerCash = 1000;
	[Header("ReadOnly")]
	[SerializeField]
	[ReadOnly]
	float _deliveryTruckSpawnTimer;
	[SerializeField]
	[ReadOnly]
	float _pickupTruckSpawnTimer;
	[SerializeField]
	[ReadOnly]
	int _currDeliveryTrucks;
	[SerializeField]
	[ReadOnly]
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
		_currDeliveryTrucks = 0;
		_currPickupTrucks = 0;
		MoneyText.text = _playerCash.ToString("C");
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
		if (_initialDeliveryTruckDelay >= 0)
		{
			_initialDeliveryTruckDelay -= Time.deltaTime;
		}
		else
		{
			_deliveryTruckSpawnTimer -= Time.deltaTime;

			if (_deliveryTruckSpawnTimer <= 0.0f)
			{
				if (_currDeliveryTrucks < _maxDeliveryTrucks)
				{
					Truck t = DeliveryTruckSlots[Random.Range(0, DeliveryTruckSlots.Length)].GetComponentInChildren<Truck>();
					t.IsDeliveryTruck = true;

					while (!t.IsOffscreen)
					{
						t = DeliveryTruckSlots[Random.Range(0, DeliveryTruckSlots.Length)].GetComponentInChildren<Truck>();
						t.IsDeliveryTruck = true;
					}
					t.InitialWaitTime = _deliveryTruckWaitTime;
					t.Arrive();
					_currDeliveryTrucks++;
					_deliveryTruckSpawnTimer = Random.Range(_deliveryTruckSpawnDelay / 2.0f, _deliveryTruckSpawnDelay);
					;
				}
			}
		}

		if (_initialPickupTruckDelay >= 0)
		{
			_initialPickupTruckDelay -= Time.deltaTime;
		}
		else
		{
			_pickupTruckSpawnTimer -= Time.deltaTime;

			if (_pickupTruckSpawnTimer <= 0.0f)
			{
				if (_currPickupTrucks < _maxPickupTrucks)
				{
					Truck t = PickupTruckSlots[Random.Range(0, PickupTruckSlots.Length)].GetComponentInChildren<Truck>();
					t.IsDeliveryTruck = false;

					while (!t.IsOffscreen)
					{
						t = PickupTruckSlots[Random.Range(0, PickupTruckSlots.Length)].GetComponentInChildren<Truck>();
						t.IsDeliveryTruck = false;
					}

					t.InitialWaitTime = _pickupTruckWaitTime;
					t.Arrive();
					_currPickupTrucks++;
					_pickupTruckSpawnTimer = Random.Range(_pickupTruckSpawnDelay / 2.0f, _pickupTruckSpawnDelay); ;
				}
			}
		}
	}

	public WarehouseItem GetWarehouseItem()
	{
		return Instantiate(WarehouseItems[Random.Range(0, WarehouseItems.Length)]);
	}

	public void AdjustCash(int amount)
	{
		_playerCash += amount;
		MoneyText.text = _playerCash.ToString("C");
	}
}