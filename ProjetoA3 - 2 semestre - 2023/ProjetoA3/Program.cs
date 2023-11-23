namespace ProjetoA3;

class Program
{
    static void Main()
    {
        string sourceCode = Utils.ReadFile("code.txt");

        Scanner scanner = new(sourceCode);
        List<Token> tokens = scanner.ScanTokens();

        Console.WriteLine("Tokens:");
        foreach (var token in tokens)
            Console.WriteLine(token);

        Console.WriteLine("\nAnálise Sintática:");
        Parser parser = new(tokens);
        parser.Parse();
    }
}
