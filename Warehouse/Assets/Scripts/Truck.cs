using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour, IInventoryContainer
{
	[Header("SceneObjects")]
	[SerializeField]
	private Slider TruckSlider;
	[SerializeField]
	private Transform[] _itemAnchors;

	[SerializeField]
	[ReadOnly]
	bool _isDeliveryTruck;
	[SerializeField]
	bool _randomTruckSize;
	[SerializeField]
	[Range(1, 3)]
	int _truckSize;

	[SerializeField]
	[ReadOnly]
	private List<WarehouseItem> _truckInventory;

	enum TruckState
	{
		OffScreen,
		Arriving,
		Waiting,
		Leaving
	}

	TruckState _currState;
	Animator _slotAnimator;

	float _waitTime;
	float _maxWaitTime;

	int _deliveryValue;

	public bool IsOffscreen { get { return _currState == TruckState.OffScreen; } }
	public bool IsDeliveryTruck { get { return _isDeliveryTruck; } set { _isDeliveryTruck = value; } }

	// Use this for initialization
	void Start()
	{
		_slotAnimator = GetComponentInParent<Animator>();
		_truckInventory = new List<WarehouseItem>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (_currState)
		{
			case TruckState.Arriving:
				Arriving();
				break;
			case TruckState.Waiting:
				Waiting();
				break;
			case TruckState.Leaving:
				Leaving();
				break;
			default:
				break;
		}
	}

	private void Init()
	{
		if (_randomTruckSize)
			_truckSize = Random.Range(1, 4);


		_truckInventory.Clear();

		if (IsDeliveryTruck)
		{
			for (int i = 0; i < _truckSize; i++)
			{
				var go = GameManager.Instance.GetWarehouseItem();
				_truckInventory.Add(go);
				go.transform.SetParent(_itemAnchors[i], false);
				go.DisplayTruckGraphic = true;

			}
		}
		for (int i = 0; i < _itemAnchors.Length; i++)
		{
			_itemAnchors[i].GetComponent<DropArea>().enabled = !IsDeliveryTruck;
			_itemAnchors[i].GetComponent<Image>().enabled = !IsDeliveryTruck;
		}

		_maxWaitTime = _waitTime = _truckSize * 3.0f;
		TruckSlider.value = _waitTime / _maxWaitTime;
		TruckSlider.gameObject.SetActive(false);
		_deliveryValue = 0;
	}

	void RemoveWarehouseItem(WarehouseItem item)
	{

	}

	public void Arrive()
	{
		Init();
		_currState = TruckState.Arriving;
		_slotAnimator.SetTrigger("Park");
		GetComponent<AudioSource>().Play();
		TruckSlider.gameObject.SetActive(false);
	}

	public void Leave()
	{
		_currState = TruckState.Leaving;

		ProcessPayment();

		TruckSlider.gameObject.SetActive(false);

		for (int i = 0; i < _itemAnchors.Length; i++)
		{
			_itemAnchors[i].GetComponent<DropArea>().enabled = false;
			var draggable = _itemAnchors[i].GetComponentInChildren<Draggable>();
			if (draggable != null)
				draggable.CanDrag = false;
		}

		_currState = TruckState.Leaving;
		_slotAnimator.SetTrigger("Leave");
	}

	void Arriving()
	{
		if (_slotAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_slotAnimator.IsInTransition(0))
		{
			_currState = TruckState.Waiting;
			GetComponent<AudioSource>().Stop();
			TruckSlider.gameObject.SetActive(true);
		}
	}

	void Waiting()
	{
		_waitTime -= Time.deltaTime;

		TruckSlider.value = _waitTime / _maxWaitTime;

		if (_waitTime <= 0)
		{
			Leave();
		}
	}

	// Pays the player or penalizes them accordingly
	void ProcessPayment()
	{
		int penaltyAmount = 0;

		for (int i = 0; i < _truckInventory.Count; i++)
		{
			penaltyAmount += _truckInventory[i].Value;
		}

		if (_isDeliveryTruck)
			GameManager.Instance.AdjustCash(-penaltyAmount);
		else
			GameManager.Instance.AdjustCash(penaltyAmount);
	}

	void Leaving()
	{
		if (_slotAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_slotAnimator.IsInTransition(0))
		{
			_currState = TruckState.OffScreen;
			GameManager.Instance.TruckIsOffscreen(_isDeliveryTruck);

			for (int i = 0; i < _truckInventory.Count; i++)
			{
				_truckInventory[i].transform.SetParent(null);
				Destroy(_truckInventory[i]);
			}
			_truckInventory.Clear();
		}
	}

	public void AddInventory(Transform targetTransform, WarehouseItem droppedObject)
	{
		droppedObject.DisplayTruckGraphic = true;
		_truckInventory.Add(droppedObject);
	}

	public void RemoveInventory(WarehouseItem removedObject)
	{
		removedObject.DisplayTruckGraphic = false;
		_truckInventory.Remove(removedObject);
	}
}