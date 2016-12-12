using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement), typeof(WarehouseItem))]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField]
	private GameObject _placeholderPrefab;
	public Transform parentToReturnTo { get; set; }
	public Transform placeholderParent { get; set; }
	public GameObject placeholder { get; set; }
	public WarehouseItem Item { get; set; }
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

	void Start()
	{
		Item = GetComponent<WarehouseItem>();
		if (Item == null)
			Debug.LogError("No warehouse item set on " + name + " ID:" + GetInstanceID());
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!CanDrag)
		{
			eventData.pointerDrag = null;
			return;
		}

		placeholderParent = parentToReturnTo = transform.parent;
		placeholder = Instantiate(_placeholderPrefab);
		placeholder.transform.SetParent(placeholderParent, false);
		placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
		var phLayout = placeholder.GetComponent<LayoutElement>();
		phLayout.flexibleWidth = 0;
		phLayout.flexibleHeight = 0;
		phLayout.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
		phLayout.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
		transform.SetParent(GameObject.Find("DragItemParent").transform);
		Item.DisplayTruckGraphic = false;
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
		{
			placeholder.transform.SetParent(placeholderParent, false);
			placeholder.transform.localPosition = Vector3.zero;
		}
	}
	public void DroppedOn(Transform newParent)
	{
		var warehouse = parentToReturnTo.GetComponentInParent<IInventoryContainer>();
		if (warehouse != null)
			warehouse.RemoveInventory(Item);

		parentToReturnTo = newParent;

		if (CanDrag)
			GetComponent<CanvasGroup>().blocksRaycasts = true;


		Destroy(placeholder);

		warehouse = newParent.GetComponentInParent<IInventoryContainer>();
		if (warehouse != null)
			warehouse.AddInventory(newParent, Item);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		Item.DisplayTruckGraphic = parentToReturnTo.GetComponent<Truck>() != null;
		transform.SetParent(parentToReturnTo, false);
		transform.localPosition = Vector3.zero;

		foreach (var item in FindObjectsOfType<DropArea>())
		{
			item.ClearHilight();
		}
	}
}
