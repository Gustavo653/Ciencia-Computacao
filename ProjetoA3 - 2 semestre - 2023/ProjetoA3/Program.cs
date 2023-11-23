namespace ProjetoA3;

class Program
{
    static void Main()
    {
        string sourceCode = @"
            var a = 5;
            var b = 10;
            var c = a + b;
            print c;

            if (a > b) {
                print ""a é maior que b"";
            } else {
                print ""a não é maior que b"";
            }
        ";

        Scanner scanner = new Scanner(sourceCode);
        List<Token> tokens = scanner.ScanTokens();

        Console.WriteLine("Tokens:");
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }

        Console.WriteLine("\nAnálise Sintática:");
        Parser parser = new Parser(tokens);
        parser.Parse();
    }
}
