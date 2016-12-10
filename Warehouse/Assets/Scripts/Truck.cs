using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour
{
	[SerializeField]
	bool _isDeliveryTruck;
	[SerializeField]
	bool _randomTruckSize;
	[SerializeField]
	[Range(1, 3)]
	int _truckSize;
	[SerializeField]
	Image _truckImage;

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
	float _startTime;

	int _deliveryValue;

	public bool IsOffscreen { get { return _currState == TruckState.OffScreen; } }

	// Use this for initialization
	void Start()
	{
		_slotAnimator = GetComponentInParent<Animator>();
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

		_waitTime = _truckSize * 3.0f;
		_deliveryValue = 0;
	}

	public void Arrive()
	{
		Init();
		_currState = TruckState.Arriving;
		_slotAnimator.SetTrigger("Park");
		GetComponent<AudioSource>().Play();
	}

	public void Leave()
	{
		_currState = TruckState.Leaving;
		_slotAnimator.SetTrigger("Leave");
	}

	void Arriving()
	{
		if (_slotAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_slotAnimator.IsInTransition(0))
		{
			_currState = TruckState.Waiting;
			GetComponent<AudioSource>().Stop();
		}
	}

	void Waiting()
	{
		_waitTime -= Time.deltaTime;

		if (_waitTime <= 0)
		{
			ProcessPayment();

			_currState = TruckState.Leaving;
			Leave();
		}
	}

	// Pays the player or penalizes them accordingly
	void ProcessPayment()
	{

	}

	void Leaving()
	{
		if (_slotAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_slotAnimator.IsInTransition(0))
		{
			_currState = TruckState.OffScreen;
		}
	}
}