var input = File.ReadAllText("input.txt");

int i;
for (i = 4; i <= input.Length; i++)
    if (input[(i - 4)..i].Distinct().Count() == 4) break;
    
Console.WriteLine("Solution 1: " + i);

for (i = 14; i <= input.Length; i++)
    if (input[(i - 14)..i].Distinct().Count() == 14) break;
    
Console.WriteLine("Solution 2: " + i);