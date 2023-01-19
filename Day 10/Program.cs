var input = File.ReadLines("input.txt").ToArray();

long total = 0;
var x = 1;
var index = 0;
foreach (var instr in input)
{
    if (instr != "noop")
    {
        CheckInts();
        CheckInts();
        var toAdd = int.Parse(instr[5..]);
        x += toAdd;
    }
    else
        CheckInts();
}

Console.WriteLine($"\nSolution 1: {total}");

void CheckInts()
{
    if (index % 40 == 0) Console.WriteLine();
    Console.Write(Math.Abs(x - index % 40) <= 1 ? '#' : '.');
    
    if (++index is not (20 or 60 or 100 or 140 or 180 or 220)) return;
    total += x * index;
}