using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExpansionButton : MonoBehaviour
{
	[SerializeField]
	private Warehouse _targetWarehouse;
	[SerializeField]
	private ExpandDir _expandDirection;
	// Use this for initialization
	void Start()
	{
		GetComponent<Button>().onClick.AddListener(DoUpgrade);
	}

	void DoUpgrade()
	{
		switch (_expandDirection)
		{
			case ExpandDir.Left:
				_targetWarehouse.ExpandLeft();
				break;
			case ExpandDir.Up:
				_targetWarehouse.ExpandUp();
				break;
			case ExpandDir.Right:
				_targetWarehouse.ExpandRight();
				break;
			case ExpandDir.Down:
				_targetWarehouse.ExpandDown();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		if (!_targetWarehouse.CanUpgrade(_expandDirection))
			gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		switch (_expandDirection)
		{
			case ExpandDir.Left:
			case ExpandDir.Right:
				GetComponentInChildren<Text>().text = _targetWarehouse.HorizontalUpgradeCost.ToString("C");
				break;
			case ExpandDir.Up:
			case ExpandDir.Down:
				GetComponentInChildren<Text>().text = _targetWarehouse.VerticalUpgradeCost.ToString("C");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
