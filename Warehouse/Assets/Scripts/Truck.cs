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
	public float InitialWaitTime { get; set; }
	public bool IsOffscreen { get { return _currState == TruckState.OffScreen; } }
	public bool IsDeliveryTruck { get { return _isDeliveryTruck; } set { _isDeliveryTruck = value; } }

	// Use this for initialization
	void Start()
	{
		_slotAnimator = GetComponentInParent<Animator>();
		_truckInventory = new List<WarehouseItem>();
		for (int i = 0; i < _itemAnchors.Length; i++)
		{
			_itemAnchors[i].GetComponent<DropArea>().Parent = this;
		}
	}

	// Update is called once per frame
	void Update()
	{
		switch (_currState)
		{
			case TruckState.Arriving:
				StateArriving();
				break;
			case TruckState.Waiting:
				StateWaiting();
				break;
			case TruckState.Leaving:
				StateLeaving();
				break;
			default:
				break;
		}
	}

	private void Init()
	{
		_truckInventory.Clear();

		if (IsDeliveryTruck)
		{
			if (_randomTruckSize)
				_truckSize = Random.Range(1, 4);

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
			_itemAnchors[i].GetComponent<Image>().enabled = !IsDeliveryTruck;
		}

		_maxWaitTime = _waitTime = _truckSize * InitialWaitTime;
		TruckSlider.value = 1;
		TruckSlider.gameObject.SetActive(false);
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

		TruckSlider.gameObject.SetActive(false);

		for (int i = 0; i < _itemAnchors.Length; i++)
		{
			var draggable = _itemAnchors[i].GetComponentInChildren<Draggable>();
			if (draggable != null)
				draggable.CanDrag = false;
		}

		_slotAnimator.SetTrigger("Leave");
	}

	void StateArriving()
	{
		if (AnimationFinished)
		{
			_currState = TruckState.Waiting;
			GetComponent<AudioSource>().Stop();
			TruckSlider.gameObject.SetActive(true);
		}
	}
	void StateWaiting()
	{
		_waitTime -= Time.deltaTime;

		TruckSlider.value = _waitTime / _maxWaitTime;

		if (_waitTime <= 0)
		{
			Leave();
		}
	}
	void StateLeaving()
	{
		if (AnimationFinished)
		{
			_currState = TruckState.OffScreen;
			ProcessPayment();
			GameManager.Instance.TruckIsOffscreen(_isDeliveryTruck);

			for (int i = 0; i < _truckInventory.Count; i++)
			{
				_truckInventory[i].transform.SetParent(null);
				Destroy(_truckInventory[i].gameObject);
			}
			_truckInventory.Clear();
		}
	}
	bool AnimationFinished
	{
		get { return _slotAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_slotAnimator.IsInTransition(0); }
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

	public void AddInventory(Transform targetTransform, WarehouseItem droppedObject)
	{
		_truckInventory.Add(droppedObject);
		if (_truckInventory.Count == 3)
			Leave();
	}

	public void RemoveInventory(WarehouseItem removedObject)
	{
		_truckInventory.Remove(removedObject);
		if (_truckInventory.Count == 0)
			Leave();
	}

	public bool IsValidDrop(Transform targetTx, WarehouseItem item)
	{
		return (!IsDeliveryTruck && targetTx.childCount == 0 && _currState == TruckState.Waiting);
	}
}