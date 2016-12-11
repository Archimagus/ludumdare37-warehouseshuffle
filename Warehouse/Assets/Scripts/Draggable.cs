using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform parentToReturnTo { get; set; }
	public Transform placeholderParent { get; set; }
	public GameObject placeholder { get; set; }

	private bool _canDrag = true;
	public bool CanDrag
	{
		get { return _canDrag; }
		set
		{
			_canDrag = value;
			GetComponent<CanvasGroup>().blocksRaycasts = _canDrag;

		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			eventData.pointerDrag = null;
			return;
		}

		placeholderParent = parentToReturnTo = transform.parent;
		placeholder = new GameObject("Placeholder");
		placeholder.transform.SetParent(placeholderParent);
		placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
		placeholder.AddComponent<Image>();
		var phLayout = placeholder.AddComponent<LayoutElement>();
		phLayout.flexibleWidth = 0;
		phLayout.flexibleHeight = 0;
		phLayout.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
		phLayout.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
		transform.SetParent(transform.parent.parent);
		GetComponent<WarehouseItem>().DisplayTruckGraphic = false;
		foreach (var item in FindObjectsOfType<DropArea>())
		{
			item.HilightIfDroppable(this);
		}
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			eventData.pointerDrag = null;
			return;
		}

		transform.position = eventData.position;

		if (placeholder.transform.parent != placeholderParent)
			placeholder.transform.SetParent(placeholderParent, false);
	}
	public void DroppedOn(Transform newParent)
	{
		var warehouse = parentToReturnTo.GetComponentInParent<IInventoryContainer>();
		if (warehouse != null)
			warehouse.RemoveInventory(gameObject.GetComponent<WarehouseItem>());


		parentToReturnTo = newParent;


		warehouse = newParent.GetComponentInParent<IInventoryContainer>();
		if (warehouse != null)
			warehouse.AddInventory(newParent, gameObject.GetComponent<WarehouseItem>());
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (CanDrag)
			GetComponent<CanvasGroup>().blocksRaycasts = true;

		transform.SetParent(placeholderParent, false);
		transform.localPosition = Vector3.zero;
		Destroy(placeholder);
		foreach (var item in FindObjectsOfType<DropArea>())
		{
			item.ClearHilight();
		}
	}
}
