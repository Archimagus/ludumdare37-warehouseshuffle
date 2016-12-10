using UnityEngine;

public class Warehouse : MonoBehaviour
{
	private const int MaxGridSize = 5;

	[SerializeField]
	private RectTransform _frameRect;

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
	private int _rows;
	private int _cols;


	enum Space
	{
		Unavailable,
		Occupied,
		Available
	}

	private Space[,] _spaces = new Space[MaxGridSize * 2, MaxGridSize * 2];


	void Start()
	{
		_rows = _startingRows;
		_cols = _startingCols;
		ResizeGrid();
	}

	void OnValidate()
	{
		_rows = _startingRows;
		_cols = _startingCols;
		ResizeGrid();
	}

	public void ExpandRight()
	{
		var offset = _frameRect.offsetMax;
		offset.x += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
	}
	public void ExpandLeft()
	{
		var offset = _frameRect.offsetMin;
		offset.x -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
	}
	public void ExpandUp()
	{
		var offset = _frameRect.offsetMax;
		offset.y += _gridElementSize + _gridSpacing;
		_frameRect.offsetMax = offset;
	}
	public void ExpandDown()
	{
		var offset = _frameRect.offsetMin;
		offset.y -= _gridElementSize + _gridSpacing;
		_frameRect.offsetMin = offset;
	}
	public void ResizeGrid()
	{
		var offset = _frameRect.offsetMax;
		offset.x = (_gridElementSize + _gridSpacing) * (_rows) + _borderSize;
		offset.y = (_gridElementSize + _gridSpacing) * (_cols) + _borderSize;
		_frameRect.offsetMax = offset;

		offset = _frameRect.offsetMin;
		offset.x = -((_gridElementSize + _gridSpacing) * (_rows) + _borderSize);
		offset.y = -((_gridElementSize + _gridSpacing) * (_cols) + _borderSize);
		_frameRect.offsetMin = offset;
	}
}
