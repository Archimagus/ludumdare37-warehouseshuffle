using System;
using UnityEngine;
using UnityEngine.Serialization;

public class WarehouseItem : MonoBehaviour
{
	[SerializeField]
	private string _itemName;
	[SerializeField]
	private int _itemValue;
	[FormerlySerializedAs("_blockSpaces")]
	public IntVector2[] BlockSpaces;
	[SerializeField]
	private GameObject TruckGraphic;
	[SerializeField]
	private GameObject WarehouseGraphic;

	private bool _displayTruckGraphic;

	void OnValidate()
	{
		if (BlockSpaces == null || BlockSpaces.Length == 0)
			BlockSpaces = new[] { new IntVector2 { x = 0, y = 0 } };
		BlockSpaces[0].x = 0;
		BlockSpaces[0].y = 0;
	}

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
[Serializable]
public struct IntVector2
{
	public int x;
	public int y;
}