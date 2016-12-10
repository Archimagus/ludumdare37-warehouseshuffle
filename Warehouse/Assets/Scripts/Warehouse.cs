using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum Space
{
	Available,
	Occupied,
}
public class Warehouse : MonoBehaviour, IDragHandler, IDropHandler
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

		var tile = eventData.hovered.FirstOrDefault(g => g.CompareTag("DropTile"));
		Debug.Log(tile);
	}
	public void OnDrop(PointerEventData eventData)
	{
		var tile = eventData.hovered.FirstOrDefault(g => g.CompareTag("DropTile"));
		if (tile != null)
		{
			var index = tile.transform.GetSiblingIndex();
			var row = index / _spaces.Cols;
			var col = index % _spaces.Cols;

			_spaces[row, col] = Space.Occupied;

			RepopulateGrid();
		}
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

		for (int r = 0; r < _spaces.Rows; r++)
		{
			for (int c = 0; c < _spaces.Cols; c++)
			{
				var t = Instantiate(_tilePrefab);
				t.color = _spaces[r, c] == Space.Available ? Color.green : Color.red;
				t.transform.SetParent(_floorImage.transform, false);
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
