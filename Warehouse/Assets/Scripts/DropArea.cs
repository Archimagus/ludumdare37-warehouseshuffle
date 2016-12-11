using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	public Color NutralColor = Color.white;
	public Color ValidDropColor = Color.green;
	public Color InvalidDropColor = Color.red;

	public int ValidDropID { get; set; }

	virtual public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			var draggable = eventData.pointerDrag.GetComponent<Draggable>();
			if (draggable != null)
			{
				if (HilightIfDroppable(draggable))
				{
					draggable.placeholderParent = transform;
				}
			}
		}
	}

	virtual public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			var draggable = eventData.pointerDrag.GetComponent<Draggable>();
			if (draggable != null)
			{
				if (HilightIfDroppable(draggable))
				{
					draggable.DroppedOn(transform);
				}
			}
		}
	}

	virtual public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			var draggable = eventData.pointerDrag.GetComponent<Draggable>();
			if (draggable != null)
			{
				draggable.placeholderParent = draggable.parentToReturnTo;
			}
		}
	}

	virtual public bool HilightIfDroppable(Draggable draggable)
	{
		if (ValidDropID == 0 || ValidDropID == draggable.gameObject.GetInstanceID())
		{
			GetComponent<Image>().color = ValidDropColor;
			return true;
		}
		GetComponent<Image>().color = InvalidDropColor;
		return false;
	}

	virtual public void ClearHilight()
	{
		GetComponent<Image>().color = NutralColor;
	}
}
