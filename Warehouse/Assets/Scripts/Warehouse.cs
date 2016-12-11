using UnityEngine;
using UnityEngine.UI;

enum Space
{
	Available,
	Occupied,
}

public class Warehouse : MonoBehaviour, IInventoryContainer
{
	private const int MaxGridSize = 5;

	[Header("Game Objects")]
	[SerializeField]
	private RectTransform _frameRect;
	[SerializeField]
	private Image _floorImage;

	[Header("Prefabs")]
	[SerializeField]
	private Image _tilePrefab;

	[Header("Settings")]
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
	DynaGrid<int> _spaces;
	DynaGrid<Image> _tiles;

	void Start()
	{
		ResizeGrid();
		RepopulateGrid();
	}

	void OnValidate()
	{
		ResizeGrid();
	}

	public bool IsValidDrop(Transform tx)
	{
		var index = tx.GetSiblingIndex();
		var row = index / _spaces.Cols;
		var col = index % _spaces.Cols;
		return _spaces[row, col] == 0;
	}


	public void ExpandRight()
	{
		var offset = _frameRect.offsetMax;
		offset.x += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Right);
		_tiles.Expand(ExpandDir.Right);
		RepopulateGrid();
	}

	public void ExpandLeft()
	{
		var offset = _frameRect.offsetMin;
		offset.x -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Left);
		_tiles.Expand(ExpandDir.Left);
		RepopulateGrid();
	}

	public void ExpandUp()
	{
		var offset = _frameRect.offsetMax;
		offset.y += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Up);
		_tiles.Expand(ExpandDir.Up);
		RepopulateGrid();
	}

	public void ExpandDown()
	{
		var offset = _frameRect.offsetMin;
		offset.y -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Down);
		_tiles.Expand(ExpandDir.Down);
		RepopulateGrid();
	}

	public void RemoveInventory(WarehouseItem removedObject)
	{
		var id = removedObject.GetInstanceID();
		for (int r = 0; r < _spaces.Rows; r++)
		{
			for (int c = 0; c < _spaces.Cols; c++)
			{
				if (_spaces[r, c] == id)
					_spaces[r, c] = 0;
			}
		}
		RepopulateGrid();
	}
	public void AddInventory(Transform targetTransform, WarehouseItem droppedObject)
	{
		var id = droppedObject.GetInstanceID();
		var index = targetTransform.GetSiblingIndex();
		var r = index / _spaces.Cols;
		var c = index % _spaces.Cols;
		_spaces[r, c] = id;
		RepopulateGrid();
	}
	public void RepopulateGrid()
	{
		var tx = _floorImage.rectTransform;
		float startx = _gridSpacing + _gridElementSize / 2f;
		float starty = -(_gridSpacing + _gridElementSize / 2f);
		for (int r = 0; r < _spaces.Rows; r++)
		{
			for (int c = 0; c < _spaces.Cols; c++)
			{
				var id = _spaces[r, c];
				var t = _tiles[r, c];
				if (t == null)
				{
					t = Instantiate(_tilePrefab);
					_tiles[r, c] = t;
					t.transform.SetParent(tx, false);
				}
				t.GetComponent<DropArea>().ValidDropID = id;
				t.rectTransform.localPosition = new Vector3(startx + ((_gridElementSize + _gridSpacing) * c), starty - ((_gridElementSize + _gridSpacing) * r));
				t.rectTransform.SetAsLastSibling();
			}
		}

	}

	public void ResizeGrid()
	{
		_spaces = new DynaGrid<int>(_startingRows, _startingCols);
		_tiles = new DynaGrid<Image>(_startingRows, _startingCols);

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

public interface IInventoryContainer
{
	void AddInventory(Transform targetTransform, WarehouseItem droppedObject);
	void RemoveInventory(WarehouseItem removedObject);
}