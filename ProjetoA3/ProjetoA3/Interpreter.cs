namespace ProjetoA3;

public class Interpreter
{
    private readonly Automaton automaton = new();

    public void Interpret(string input)
    {
        Console.WriteLine(automaton.Accept(input) ? 
                          $"A string {input} foi aceita!" : 
                          $"A string {input} foi rejeitada!");
    }
}