using NUnit.Framework;
using System.Collections.Generic;

namespace ProjetoA3.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parse_ShouldNotThrowExceptionForValidInput()
        {
            // Arrange
            List<Token> tokens = new()
            {
                new Token(TokenType.VAR, "var", null, 1),
                new Token(TokenType.IDENTIFIER, "x", null, 1),
                new Token(TokenType.EQUAL, "=", null, 1),
                new Token(TokenType.NUMBER, "42", 42, 1),
                new Token(TokenType.SEMICOLON, ";", null, 1),
                new Token(TokenType.EOF, "", null, 1),
            };

            Parser parser = new(tokens);

            // Act & Assert
            Assert.DoesNotThrow(() => parser.Parse());
        }
    }
}
