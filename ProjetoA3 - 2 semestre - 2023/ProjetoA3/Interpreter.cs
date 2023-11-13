namespace ProjetoA3;

public class Interpreter
{
    private readonly Automaton automaton = new();

    public void Interpret(string input)
    {
        Console.WriteLine(automaton.Accept(input) ? 
                          $"A entrada {input} foi aceita!" : 
                          $"A entrada {input} foi rejeitada!");
    }
}