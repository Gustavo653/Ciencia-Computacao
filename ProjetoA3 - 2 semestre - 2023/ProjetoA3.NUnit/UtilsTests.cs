using NUnit.Framework;
using System.IO;

namespace ProjetoA3.NUnit
{
    [TestFixture]
    public class UtilsTests
    {
        [Test]
        public void ReadFile_ShouldReturnFileContents()
        {
            // Arrange
            string nomeArquivo = "testfile.txt";
            string conteudoEsperado = "Conteúdo do arquivo de teste.\r\n";

            File.WriteAllText(nomeArquivo, conteudoEsperado);

            try
            {
                // Act
                string resultado = Utils.ReadFile(nomeArquivo);

                // Assert
                Assert.That(resultado, Is.EqualTo(conteudoEsperado));
            }
            finally
            {
                File.Delete(nomeArquivo);
            }
        }

        [Test]
        public void ReadFile_ShouldReturnEmptyStringForNonexistentFile()
        {
            // Arrange
            string nomeArquivo = "nonexistentfile.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => Utils.ReadFile(nomeArquivo));
        }
    }
}
