using NUnit.Framework;
using System.Collections.Generic;

namespace ProjetoA3.Tests
{
    [TestFixture]
    public class ScannerTests
    {
        [Test]
        public void ScanTokens_ShouldReturnCorrectTokensForValidInput()
        {
            // Arrange
            string source = "var x = 42;";
            Scanner scanner = new(source);

            // Act
            List<Token> tokens = scanner.ScanTokens();

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(6));
            Assert.Multiple(() =>
            {
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.VAR));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.IDENTIFIER));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EQUAL));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.NUMBER));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.SEMICOLON));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.EOF));
            });
        }

        [Test]
        public void ScanTokens_ShouldHandleCommentsAndWhiteSpace()
        {
            // Arrange
            string source = "var x = 42; // This is a comment";
            Scanner scanner = new(source);

            // Act
            List<Token> tokens = scanner.ScanTokens();

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(6));
            Assert.Multiple(() =>
            {
                Assert.That(tokens[0].Type, Is.EqualTo(TokenType.VAR));
                Assert.That(tokens[1].Type, Is.EqualTo(TokenType.IDENTIFIER));
                Assert.That(tokens[2].Type, Is.EqualTo(TokenType.EQUAL));
                Assert.That(tokens[3].Type, Is.EqualTo(TokenType.NUMBER));
                Assert.That(tokens[4].Type, Is.EqualTo(TokenType.SEMICOLON));
                Assert.That(tokens[5].Type, Is.EqualTo(TokenType.EOF));
            });
        }
    }
}
