using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropArea : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	public int ChildLimit;

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

	virtual public void PositionPlaceholder(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			var draggable = eventData.pointerDrag.GetComponent<Draggable>();
			if (draggable != null)
			{
				if (HilightIfDroppable(draggable))
				{
					var placeholder = draggable.placeholder;
					int index = transform.childCount;
					for (int i = 0; i < transform.childCount; i++)
					{
						var child = transform.GetChild(i);
						if (eventData.position.x < child.position.x)
						{
							index = i;
							if (index > placeholder.transform.GetSiblingIndex())
								index = i - 1;
							break;
						}
					}

					placeholder.transform.SetSiblingIndex(index);
				}
			}
		}
	}

	virtual public bool HilightIfDroppable(Draggable draggable)
	{
		int childCount = transform.childCount;
		if (draggable.placeholderParent == transform)
			childCount--;
		if (ChildLimit < 0 || childCount < ChildLimit)
		{
			GetComponent<Image>().color = new Color(1f, 0.8f, 0.2f, 0.4f);
			return true;
		}
		GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
		return false;
	}

	virtual public void ClearHilight()
	{
		GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
	}
}
