namespace ProjetoA3;

public class Interpreter
{
    private readonly Automaton automaton = new();

    public void Interpret(string input)
    {
        foreach (var t in input)
        {
            automaton.Next(t);
        }

        Console.WriteLine(automaton.Accept() ? $"A string {input} foi aceita!" : $"A string {input} foi rejeitada!");
    }
}