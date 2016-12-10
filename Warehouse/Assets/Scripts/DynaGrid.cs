using System;

public enum ExpandDir
{
	Left, Up, Right, Down
}
public class DynaGrid<T>
{
	public int Rows { get; private set; }
	public int Cols { get; private set; }

	private T[,] _grid;

	public DynaGrid(int rows, int cols)
	{
		Rows = rows;
		Cols = cols;

		_grid = new T[rows, cols];
	}

	public T this[int r, int c]
	{
		get { return _grid[r, c]; }
		set { _grid[r, c] = value; }
	}

	public void Expand(ExpandDir dir)
	{
		switch (dir)
		{
			case ExpandDir.Left:
				{
					Cols++;
					var g = new T[Rows, Cols];
					for (int r = 0; r < Rows; r++)
					{
						for (int c = 1; c < Cols; c++)
						{
							g[r, c] = _grid[r, c - 1];
						}
					}
					_grid = g;
				}
				break;
			case ExpandDir.Right:
				{
					Cols++;
					var g = new T[Rows, Cols];
					for (int r = 0; r < Rows; r++)
					{
						for (int c = 0; c < Cols - 1; c++)
						{
							g[r, c] = _grid[r, c];
						}
					}
					_grid = g;
				}
				break;
			case ExpandDir.Up:
				{
					Rows++;
					var g = new T[Rows, Cols];
					for (int r = 1; r < Rows; r++)
					{
						for (int c = 0; c < Cols; c++)
						{
							g[r, c] = _grid[r - 1, c];
						}
					}
					_grid = g;
				}
				break;
			case ExpandDir.Down:
				{
					Rows++;
					var g = new T[Rows, Cols];
					for (int r = 0; r < Rows - 1; r++)
					{
						for (int c = 0; c < Cols; c++)
						{
							g[r, c] = _grid[r, c];
						}
					}
					_grid = g;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("dir", dir, null);
		}
	}
}