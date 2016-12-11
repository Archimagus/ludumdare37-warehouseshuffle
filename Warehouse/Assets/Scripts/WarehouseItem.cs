using UnityEngine;

public class WarehouseItem : MonoBehaviour
{
	[SerializeField]
	private string _itemName;
	[SerializeField]
	private int _itemValue;
	[SerializeField]
	private int _numBlocks;
	[SerializeField]
	private GameObject TruckGraphic;
	[SerializeField]
	private GameObject WarehouseGraphic;

	private bool _displayTruckGraphic;


	public int Value { get { return _itemValue; } set { _itemValue = value; } }

	public bool DisplayTruckGraphic
	{
		get { return _displayTruckGraphic; }
		set
		{
			_displayTruckGraphic = value;
			if (value)
			{
				TruckGraphic.SetActive(true);
				WarehouseGraphic.SetActive(false);
			}
			else
			{
				TruckGraphic.SetActive(false);
				WarehouseGraphic.SetActive(true);
			}
		}
	}

}