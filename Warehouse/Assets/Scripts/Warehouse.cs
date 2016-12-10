using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum Space
{
	Unavailable,
	Occupied,
	Available
}
public class Warehouse : MonoBehaviour, IDragHandler
{
	private const int MaxGridSize = 5;

	[SerializeField]
	private RectTransform _frameRect;
	[SerializeField]
	private Image _floorImage;

	[Header("Prefabs")]
	[SerializeField]
	private Image _tilePrefab;

	[SerializeField]
	[Range(1, MaxGridSize)]
	private int _startingRows = 2;
	[SerializeField]
	[Range(1, MaxGridSize)]
	private int _startingCols = 2;
	[SerializeField]
	private int _gridElementSize = 64;
	[SerializeField]
	private int _gridSpacing = 1;
	[SerializeField]
	private int _borderSize = 80;
	DynaGrid<Space> _spaces;


	void Start()
	{
		ResizeGrid();
		RepopulateGrid();
	}

	void OnValidate()
	{
		ResizeGrid();
	}

	public void OnDrag(PointerEventData eventData)
	{
		Debug.Log(new string(eventData.hovered.SelectMany(h => h.name + " ").ToArray()));
	}

	//void Update()
	//{
	//	var mouse = Input.mousePosition;
	//	mouse = _floorImage.rectTransform.InverseTransformPoint(mouse);
	//	mouse.x += _floorImage.rectTransform.rect.width / 2;
	//	mouse.y += _floorImage.rectTransform.rect.height / 2;
	//	Debug.Log(mouse);
	//}

	public void ExpandRight()
	{
		var offset = _frameRect.offsetMax;
		offset.x += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Right);
		RepopulateGrid();
	}
	public void ExpandLeft()
	{
		var offset = _frameRect.offsetMin;
		offset.x -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Left);
		RepopulateGrid();
	}
	public void ExpandUp()
	{
		var offset = _frameRect.offsetMax;
		offset.y += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Up);
		RepopulateGrid();
	}
	public void ExpandDown()
	{
		var offset = _frameRect.offsetMin;
		offset.y -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Down);
		RepopulateGrid();
	}
	public void RepopulateGrid()
	{
		while (_floorImage.transform.childCount > 0)
		{
			var c = _floorImage.transform.GetChild(0);
			c.SetParent(null);
			Destroy(c.gameObject);
		}

		for (int i = 0; i < _spaces.Rows; i++)
		{
			for (int j = 0; j < _spaces.Cols; j++)
			{
				var c = Instantiate(_tilePrefab);
				c.color = Color.green;
				c.transform.SetParent(_floorImage.transform);
			}
		}

	}
	public void ResizeGrid()
	{
		_spaces = new DynaGrid<Space>(_startingRows, _startingCols);

		var offset = _frameRect.offsetMax;
		offset.x = (_gridElementSize + _gridSpacing) * (_startingCols / 2f) + _borderSize;
		offset.y = (_gridElementSize + _gridSpacing) * (_startingRows / 2f) + _borderSize;
		_frameRect.offsetMax = offset;

		offset = _frameRect.offsetMin;
		offset.x = -((_gridElementSize + _gridSpacing) * (_startingCols / 2f) + _borderSize);
		offset.y = -((_gridElementSize + _gridSpacing) * (_startingRows / 2f) + _borderSize);
		_frameRect.offsetMin = offset;
	}

}
