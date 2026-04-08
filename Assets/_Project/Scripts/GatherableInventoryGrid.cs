using System;
using System.Collections.Generic;

public class GatherableInventoryGrid
{
    int rows;
    int columns;
    GatherableInventoryItem[,] cells;
    List<GatherableInventoryItem> items;
    ContainerFloor _containerFloor;
    public int Columns => columns;
    public int Rows => rows;

    public GatherableInventoryGrid(int numberOfRows, int numberOfColumns, ContainerFloor containerFloor)
    {
        rows = numberOfRows;
        columns = numberOfColumns;
        cells = new GatherableInventoryItem[numberOfRows, numberOfColumns];
        items = new List<GatherableInventoryItem>();
        _containerFloor = containerFloor;
    }

    public bool CanPlaceItem(GatherableInventoryItem item, int x, int y)
    {
        // x -> start column
        // y -> start row 
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        if (x < 0 || y < 0)
            return false;

        if (x + item.Width > columns || y + item.Height > rows)
            return false;

        for (int row = y; row < y + item.Height; row++)
        {
            for (int col = x; col < x + item.Width; col++)
            {
                if (cells[row, col] != null)
                    return false;
            }
        }

        return true;
    }


    public bool PlaceItem(GatherableInventoryItem item, int x, int y)
    {
        // verif daca e stackable ++
        if (!CanPlaceItem(item, x, y)) return false;

        item.SetPosition(x, y);

        for (int row = y; row < y + item.Height; row++)
        {
            for (int col = x; col < x + item.Width; col++)
            {
                cells[row, col] = item;
            }
        }

        if (!items.Contains(item))
            items.Add(item);

        return true;
    }


    public bool RemoveItem(GatherableInventoryItem item)
    {
        // verif daca e stackable --
        if (item == null)
            return false;

        bool found = false;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (cells[row, column] == item)
                {
                    cells[row, column] = null;
                    found = true;
                }
            }
        }

        if (found)
        {
            items.Remove(item);
            item.SetPosition(-1, -1);
        }

        return found;
    }


    public GatherableInventoryItem GetItemAt(int x, int y)
    {
        if (x < 0 || y < 0) return null;
        if (x >= columns || y >= rows) return null;

        return cells[y, x];
    }

    // Dry-run check only — does NOT place
    public bool CanAutoPlace(GatherableInventoryItem item)
    {
        if (item == null) return false;

        for (int row = 0; row < rows; row++)
        for (int col = 0; col < columns; col++)
            if (CanPlaceItem(item, col, row)) return true;

        return false;
    }

    // Actually places the item
    public bool TryAutoPlaceItem(GatherableInventoryItem item)
    {
        if (item == null) return false;

        for (int row = 0; row < rows; row++)
        for (int col = 0; col < columns; col++)
            if (CanPlaceItem(item, col, row))
            {
                PlaceItem(item, col, row);
                return true;
            }

        return false;
    }
}