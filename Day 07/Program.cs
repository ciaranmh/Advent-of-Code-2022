var input = File.ReadLines("input.txt");

// path as stack with size as dict is much nicer but oh well
var outermost = new Directory("/");
var curDir = outermost;
foreach (var command in input)
{
    switch (command.Split(' '))
    {
        case ["$", "cd", var dir]:
            curDir = dir switch
            {
                ".." => curDir?.Parent,
                "/" => outermost,
                _ => curDir?.SubDirectories[dir]
            };
            break;
        case ["dir", var dir]:
            curDir?.AddSubDir(dir);
            break;
        case ["$", "ls"]:
            break;
        case [var fSize, var fName]:
            curDir?.AddFile(fName, int.Parse(fSize));
            break;
    }
}

var flattened = outermost.FlattenDirectories();
var spaceNeeded = 30000000 - (70000000 - outermost.Size);
Console.WriteLine("Solution 1: " + flattened.Where(d => d.Size <= 100000).Sum(d => d.Size));

Console.WriteLine("Solution 2: " + flattened.Where(d => d.Size >= spaceNeeded).MinBy(d => d.Size)!.Size);

#region data types

record Directory(string Name, Directory? Parent = null)
{
    public Dictionary<string, Directory> SubDirectories { get; } = new();
    public Dictionary<string, AFile> Files { get; } = new();
    public int Size => Files.Sum(f => f.Value.Size) + SubDirectories.Sum(d => d.Value.Size);

    public void AddSubDir(string name) 
        => SubDirectories[name] = new Directory(name, this);
    
    public void AddFile(string name, int size) 
        => Files[name] = new AFile(name, size);

    public List<Directory> FlattenDirectories()
    {
        var temp = new List<Directory> { this };
        temp.AddRange(SubDirectories.Values.SelectMany(d => d.FlattenDirectories()));
        return temp;
    }

}

record AFile(string Name, int Size);

#endregion
