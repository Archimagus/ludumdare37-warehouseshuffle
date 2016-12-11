using UnityEngine;
using UnityEngine.UI;

enum Space
{
	Available,
	Occupied,
}

public class Warehouse : MonoBehaviour, IInventoryContainer
{
	private const int MaxGridSize = 10;

	[Header("Game Objects")]
	[SerializeField]
	private RectTransform _frameRect;
	[SerializeField]
	private Image _floorImage;

	[Header("Prefabs")]
	[SerializeField]
	private DropArea _tilePrefab;

	[Header("Settings")]
	[SerializeField]
	[Range(1, MaxGridSize)]
	private int _startingRows = 2;
	[SerializeField]
	[Range(1, MaxGridSize)]
	private int _startingCols = 2;
	[SerializeField]
	private int _maxUpgradesPerDirection = 4;

	[SerializeField]
	private int _gridElementSize = 64;
	[SerializeField]
	private int _gridSpacing = 1;
	[SerializeField]
	private int _borderSize = 80;

	[SerializeField]
	private int _costPerExpansionSquare = 200;

	public int HorizontalUpgradeCost { get { return _tiles.Rows * _costPerExpansionSquare; } }
	public int VerticalUpgradeCost { get { return _tiles.Cols * _costPerExpansionSquare; } }

	private int[] _upgradeCounts = new int[4];

	public bool CanUpgrade(ExpandDir direction)
	{
		return _upgradeCounts[(int)direction] <= _maxUpgradesPerDirection;
	}

	DynaGrid<int> _spaces;
	DynaGrid<DropArea> _tiles;

	void Start()
	{
		ResizeGrid();
		RepopulateGrid();
	}


	public void ExpandRight()
	{
		GameManager.Instance.AdjustCash(-HorizontalUpgradeCost);
		var offset = _frameRect.offsetMax;
		offset.x += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Right);
		_tiles.Expand(ExpandDir.Right);
		_upgradeCounts[(int)ExpandDir.Right]++;
		RepopulateGrid();
	}

	public void ExpandLeft()
	{
		GameManager.Instance.AdjustCash(-HorizontalUpgradeCost);
		var offset = _frameRect.offsetMin;
		offset.x -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Left);
		_tiles.Expand(ExpandDir.Left);
		_upgradeCounts[(int)ExpandDir.Left]++;
		RepopulateGrid();
	}

	public void ExpandUp()
	{
		GameManager.Instance.AdjustCash(-VerticalUpgradeCost);
		var offset = _frameRect.offsetMax;
		offset.y += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
		_spaces.Expand(ExpandDir.Up);
		_tiles.Expand(ExpandDir.Up);
		_upgradeCounts[(int)ExpandDir.Up]++;
		RepopulateGrid();
	}

	public void ExpandDown()
	{
		GameManager.Instance.AdjustCash(-VerticalUpgradeCost);
		var offset = _frameRect.offsetMin;
		offset.y -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
		_spaces.Expand(ExpandDir.Down);
		_tiles.Expand(ExpandDir.Down);
		_upgradeCounts[(int)ExpandDir.Down]++;
		RepopulateGrid();
	}

	public bool IsValidDrop(Transform targetTx, WarehouseItem item)
	{
		var index = targetTx.GetSiblingIndex();
		var row = index / _spaces.Cols;
		var col = index % _spaces.Cols;
		var itemId = item.GetInstanceID();
		var id = _spaces[row, col];
		return id == 0 || id == itemId;
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
					t.Parent = this;
				}
				t.ValidDropID = id;
				var rt = t.GetComponent<RectTransform>();
				rt.localPosition = new Vector3(startx + ((_gridElementSize + _gridSpacing) * c),
					starty - ((_gridElementSize + _gridSpacing) * r));
				rt.SetAsLastSibling();
			}
		}

	}

	[ContextMenu("ResizeGrid")]
	public void ResizeGrid()
	{
		_spaces = new DynaGrid<int>(_startingRows, _startingCols);
		_tiles = new DynaGrid<DropArea>(_startingRows, _startingCols);

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
	bool IsValidDrop(Transform targetTx, WarehouseItem item);
}