namespace ProjetoA3;
/// <summary>
/// Análise Sintática e Semântica
/// </summary>
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
            _ = Previous();
            Comparison();
        }
    }

    private void Comparison()
    {
        Addition();

        while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            _ = Previous();
            Addition();
        }
    }

    private void Addition()
    {
        Multiplication();

        while (Match(TokenType.PLUS, TokenType.MINUS))
        {
            _ = Previous();
            Multiplication();
        }
    }

    private void Multiplication()
    {
        Unary();

        while (Match(TokenType.STAR, TokenType.SLASH))
        {
            _ = Previous();
            Unary();
        }
    }

    private void Unary()
    {
        if (Match(TokenType.BANG, TokenType.MINUS))
        {
            _ = Previous();
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

    private static ParseException Error(Token token, string message)
    {
        Console.WriteLine($"Erro na linha {token.Line}: {message}");
        return new ParseException();
    }
}
