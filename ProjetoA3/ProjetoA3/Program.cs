using ProjetoA3;

Console.WriteLine("Hello, World!");

Interpreter interpreter = new Interpreter();
interpreter.Interpret("f12.34");
while (true)
{
    interpreter.Interpret(Console.ReadLine() ?? "");
}