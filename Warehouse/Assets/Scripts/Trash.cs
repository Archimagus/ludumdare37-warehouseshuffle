using UnityEngine;

public class Trash : MonoBehaviour, IInventoryContainer
{
	void Start()
	{
		GetComponent<DropArea>().Parent = this;
	}
	public void AddInventory(Transform targetTransform, WarehouseItem droppedObject)
	{
		GameManager.Instance.AdjustCash(-droppedObject.Value / 2);
		var ph = droppedObject.GetComponent<Draggable>().placeholder;
		if (ph != null)
			Destroy(ph);
		Destroy(droppedObject.gameObject);
	}

	public void RemoveInventory(WarehouseItem removedObject)
	{
	}

	public bool IsValidDrop(Transform targetTx, WarehouseItem item)
	{
		return true;
	}
}
