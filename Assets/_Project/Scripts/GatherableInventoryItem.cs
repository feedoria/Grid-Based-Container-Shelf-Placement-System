public class GatherableInventoryItem
{
    public int Width { get; }   // grid columns the item occupies
    public int Height { get; }  // grid rows the item occupies
    public int X { get; private set; } // current grid column position
    public int Y { get; private set; } // current grid row position

    public GatherableInventoryItem(int width, int height)
    {
        Width = width;
        Height = height;
    }

    // places the item at the given grid coordinates
    public void SetPosition(int x, int y) { X = x; Y = y; }
}