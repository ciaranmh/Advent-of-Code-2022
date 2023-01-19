using System.Numerics;

var input = File.ReadAllLines("input.txt");

var solution = input
    .Aggregate(Vector2.Zero, (total, hand) => total + new Vector2(hand switch
    {
        "A X" => 1 + 3,
        "A Y" => 2 + 6,
        "A Z" => 3 + 0,
        "B X" => 1 + 0,
        "B Y" => 2 + 3,
        "B Z" => 3 + 6,
        "C X" => 1 + 6,
        "C Y" => 2 + 0,
        "C Z" => 3 + 3,
        _ => throw new ArgumentException("How did we get here?")
    }, hand switch
    {
        "A X" => 3 + 0,
        "A Y" => 1 + 3,
        "A Z" => 2 + 6,
        "B X" => 1 + 0,
        "B Y" => 2 + 3,
        "B Z" => 3 + 6,
        "C X" => 2 + 0,
        "C Y" => 3 + 3,
        "C Z" => 1 + 6,
        _ => throw new ArgumentException("How did we get here?")
    }));

Console.WriteLine("Solution 1: " + solution.X); 
Console.WriteLine("Solution 2: " + solution.Y); 