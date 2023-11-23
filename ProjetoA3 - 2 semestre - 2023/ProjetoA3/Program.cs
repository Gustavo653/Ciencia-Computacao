using System;
using System.Collections.Generic;

public class Token
{
    public TokenType Type { get; }
    public string Lexeme { get; }
    public object Literal { get; }
    public int Line { get; }

    public Token(TokenType type, string lexeme, object literal, int line)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
        Line = line;
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal}";
    }
}

public enum TokenType
{
    // Tokens simples
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,

    // Operadores de comparação
    BANG, BANG_EQUAL, EQUAL, EQUAL_EQUAL, GREATER, GREATER_EQUAL, LESS, LESS_EQUAL,

    // Literais
    IDENTIFIER, STRING, NUMBER,

    // Palavras-chave
    AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
    PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

    // Fim de arquivo
    EOF
}

public class Scanner
{
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source)
    {
        this.source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        tokens.Add(new Token(TokenType.EOF, "", null, line));
        return tokens;
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/':
                if (Match('/'))
                {
                    // Comentário de linha
                    while (Peek() != '\n' && !IsAtEnd()) Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case ' ':
            case '\r':
            case '\t':
                // Ignorar caracteres em branco
                break;
            case '\n':
                line++;
                break;
            case '"': ScanString(); break;
            default:
                if (IsDigit(c))
                {
                    ScanNumber();
                }
                else if (IsAlpha(c))
                {
                    ScanIdentifier();
                }
                else
                {
                    Console.WriteLine($"Erro na linha {line}: Caractere inesperado.");
                }
                break;
        }
    }

    private void ScanString()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Console.WriteLine($"Erro na linha {line}: String não terminada.");
            return;
        }

        // Consumir o '"'
        Advance();

        // Trim the surrounding quotes
        string value = source.Substring(start + 1, current - start - 2);
        AddToken(TokenType.STRING, value);
    }

    private void ScanNumber()
    {
        while (IsDigit(Peek())) Advance();

        // Verificar parte fracionária
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance(); // Consumir o ponto decimal

            while (IsDigit(Peek())) Advance();
        }

        AddToken(TokenType.NUMBER, double.Parse(source.Substring(start, current - start)));
    }

    private void ScanIdentifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        string text = source.Substring(start, current - start);

        // Verificar se é uma palavra-chave
        TokenType type = TokenType.IDENTIFIER;
        if (keywords.ContainsKey(text))
        {
            type = keywords[text];
        }

        AddToken(type);
    }

    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private char PeekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        return source[current + 1];
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;

        current++;
        return true;
    }

    private char Advance()
    {
        current++;
        return source[current - 1];
    }

    private void AddToken(TokenType type, object literal = null)
    {
        string text = source.Substring(start, current - start);
        tokens.Add(new Token(type, text, literal, line));
    }

    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
    {
        {"and", TokenType.AND},
        {"class", TokenType.CLASS},
        {"else", TokenType.ELSE},
        {"false", TokenType.FALSE},
        {"for", TokenType.FOR},
        {"fun", TokenType.FUN},
        {"if", TokenType.IF},
        {"nil", TokenType.NIL},
        {"or", TokenType.OR},
        {"print", TokenType.PRINT},
        {"return", TokenType.RETURN},
        {"super", TokenType.SUPER},
        {"this", TokenType.THIS},
        {"true", TokenType.TRUE},
        {"var", TokenType.VAR},
        {"while", TokenType.WHILE}
    };
}

public class Parser
{
    private readonly List<Token> tokens;
    private int current = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public void Parse()
    {
        try
        {
            while (!IsAtEnd())
            {
                Declaration();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na linha {Peek().Line}: {ex.Message}");
        }
    }

    private void Declaration()
    {
        if (Match(TokenType.VAR))
        {
            VarDeclaration();
        }
        else
        {
            Statement();
        }
    }

    private void VarDeclaration()
    {
        Consume(TokenType.IDENTIFIER, "Esperava-se um nome de variável após 'var'.");
        if (Match(TokenType.EQUAL))
        {
            Expression();
        }
        Consume(TokenType.SEMICOLON, "Esperava-se ';' após a declaração da variável.");
    }

    private void Statement()
    {
        if (Match(TokenType.PRINT))
        {
            PrintStatement();
        }
        else if (Match(TokenType.LEFT_BRACE))
        {
            Block();
        }
        else if (Match(TokenType.IF))
        {
            IfStatement();
        }
        else
        {
            ExpressionStatement();
        }
    }

    private void PrintStatement()
    {
        Expression();
        Consume(TokenType.SEMICOLON, "Esperava-se ';' após a instrução 'print'.");
    }

    private void Block()
    {
        while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            Declaration();
        }

        Consume(TokenType.RIGHT_BRACE, "Esperava-se '}' após o bloco.");
    }

    private void IfStatement()
    {
        Consume(TokenType.LEFT_PAREN, "Esperava-se '(' após 'if'.");
        Expression();
        Consume(TokenType.RIGHT_PAREN, "Esperava-se ')' após a condição do 'if'.");
        Statement();

        if (Match(TokenType.ELSE))
        {
            Statement();
        }
    }

    private void ExpressionStatement()
    {
        Expression();
        Consume(TokenType.SEMICOLON, "Esperava-se ';' após a expressão.");
    }

    private void Expression()
    {
        Equality();
    }

    private void Equality()
    {
        Comparison();

        while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token op = Previous();
            Comparison();
        }
    }

    private void Comparison()
    {
        Addition();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token op = Previous();
            Addition();
        }
    }

    private void Addition()
    {
        Multiplication();

        while (Match(TokenType.PLUS, TokenType.MINUS))
        {
            Token op = Previous();
            Multiplication();
        }
    }

    private void Multiplication()
    {
        Unary();

        while (Match(TokenType.STAR, TokenType.SLASH))
        {
            Token op = Previous();
            Unary();
        }
    }

    private void Unary()
    {
        if (Match(TokenType.BANG, TokenType.MINUS))
        {
            Token op = Previous();
            Unary();
        }
        else
        {
            Primary();
        }
    }

    private void Primary()
    {
        if (Match(TokenType.FALSE)) { }
        else if (Match(TokenType.TRUE)) { }
        else if (Match(TokenType.NIL)) { }
        else if (Match(TokenType.NUMBER, TokenType.STRING)) { }
        else if (Match(TokenType.IDENTIFIER))
        {
            // Pode ter uma chamada de função aqui
            if (Match(TokenType.LEFT_PAREN))
            {
                FinishCall();
            }
        }
        else if (Match(TokenType.LEFT_PAREN))
        {
            Expression();
            Consume(TokenType.RIGHT_PAREN, "Esperava-se ')' após a expressão.");
        }
        else
        {
            throw Error(Peek(), "Esperava-se uma expressão.");
        }
    }

    private void FinishCall()
    {
        while (!Check(TokenType.RIGHT_PAREN) && !IsAtEnd())
        {
            Expression();
            if (!Match(TokenType.COMMA)) break;
        }

        Consume(TokenType.RIGHT_PAREN, "Esperava-se ')' após os argumentos da chamada de função.");
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }

    private Token Advance()
    {
        if (!IsAtEnd()) current++;
        return Previous();
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return tokens[current];
    }

    private Token Previous()
    {
        return tokens[current - 1];
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();

        throw Error(Peek(), message);
    }

    private ParseException Error(Token token, string message)
    {
        Console.WriteLine($"Erro na linha {token.Line}: {message}");
        return new ParseException();
    }
}

public class ParseException : Exception { }

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
