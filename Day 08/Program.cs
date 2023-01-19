using Extensions;

var input = File.ReadLines("input.txt").ToMatrix(1, chars => chars.First() - '0');

var total = 0;
for (var y = 0; y < input.GetLength(0); y++)
for (var x = 0; x < input.GetLength(1); x++)
    if (Visible(x, y, input))
        total++;

Console.WriteLine("Solution 1: " + total);

var highest = 0;
for (var y = 0; y < input.GetLength(0); y++)
for (var x = 0; x < input.GetLength(1); x++)
    highest = Math.Max(Score(x, y, input), highest);

Console.WriteLine("Solution 2: " + highest);

bool Visible(int x, int y, int[,] mat)
{
    var visible = new[] { true, true, true, true };
    var height = mat[y, x];
    // check south
    for (var y1 = y + 1; y1 < mat.GetLength(0); y1++)
        if (mat[y1, x] >= height)
            visible[0] = false;
    // check north
    for (var y1 = y - 1; y1 >= 0; y1--)
        if (mat[y1, x] >= height)
            visible[1] = false;
    // check east
    for (var x1 = x + 1; x1 < mat.GetLength(1); x1++)
        if (mat[y, x1] >= height)
            visible[2] = false;
    // check west
    for (var x1 = x - 1; x1 >= 0; x1--)
        if (mat[y, x1] >= height)
            visible[3] = false;
    return visible.Any(b => b) || x == 0 || y == 0 || y == mat.GetLength(0) - 1 || x == mat.GetLength(1) - 1;
}

int Score(int x, int y, int[,] ints)
{
    var total1 = 1;
    var height = ints[y, x];
    var subtotal = 0;
    // check south
    for (var y1 = y + 1; y1 < ints.GetLength(0); y1++)
    {
        subtotal++;
        if (ints[y1, x] >= height)
            break;
    }

    total1 *= subtotal;
    subtotal = 0;

    // check north
    for (var y1 = y - 1; y1 >= 0; y1--)
    {
        subtotal++;
        if (ints[y1, x] >= height)
            break;
    }

    total1 *= subtotal;
    subtotal = 0;

    // check east
    for (var x1 = x + 1; x1 < ints.GetLength(1); x1++)
    {
        subtotal++;
        if (ints[y, x1] >= height)
            break;
    }

    total1 *= subtotal;
    subtotal = 0;

    // check west
    for (var x1 = x - 1; x1 >= 0; x1--)
    {
        subtotal++;
        if (ints[y, x1] >= height)
            break;
    }

    total1 *= subtotal;
    return total1;
}