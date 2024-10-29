namespace Library;

public class Io(Action<string> output, Func<string?> input)
{
    public Action<string> Output { get; } = output;
    public Func<string?> Input { get; } = input;
}