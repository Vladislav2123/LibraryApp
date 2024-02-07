namespace LibraryApp.Application.Pagination;

public struct Page
{
    private const int DefaultNumber = 1;
    private const int DefaultSize = 10;
    private const int MinSize = 5;
    private const int MaxSize = 20;

    public int number;
    public int size;

    public Page(int number, int size)
    {
        if (number == 0) number = DefaultNumber;

        if (size == 0) size = DefaultSize;
        else size = Math.Clamp(number, MinSize, MaxSize);

        this.number = number;
        this.size = size;
    }
}