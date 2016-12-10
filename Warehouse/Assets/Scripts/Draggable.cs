using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class Draggable : MonoBehaviour, IBeginDragHandler , IDragHandler, IEndDragHandler
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
		var phLayout = placeholder.AddComponent<LayoutElement>();
		phLayout.flexibleWidth = 0;
		phLayout.flexibleHeight = 0;
		phLayout.preferredWidth = GetComponent<LayoutElement>().preferredWidth / 2;
		phLayout.preferredHeight = GetComponent<LayoutElement>().preferredHeight;

		transform.SetParent(transform.parent.parent);
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
			placeholder.transform.SetParent(placeholderParent);

		placeholderParent.GetComponent<DropArea>().PositionPlaceholder(eventData);
	}
	public void DroppedOn(Transform newParent)
	{
		parentToReturnTo.SendMessage("CardRemoved", SendMessageOptions.DontRequireReceiver);
		parentToReturnTo = newParent;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if(CanDrag)
			GetComponent<CanvasGroup>().blocksRaycasts = true;

		transform.SetParent(parentToReturnTo);
		transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
		Destroy(placeholder);
		foreach (var item in FindObjectsOfType<DropArea>())
		{
			item.ClearHilight();
		}
	}
}
