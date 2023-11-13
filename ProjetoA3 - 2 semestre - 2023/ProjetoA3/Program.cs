using ProjetoA3;

Console.WriteLine("Hello, World!");

while (true)
{
    Interpreter interpreter = new Interpreter();
    interpreter.Interpret(Console.ReadLine() ?? "");
}