using System.Text;

namespace ProjetoA3;

public static class Utils
{
    public static string LerCodigoArquivo(string nomeArquivo)
    {
        StringBuilder stringBuilder = new();
        using StreamReader reader = new(nomeArquivo);

        while (!reader.EndOfStream)
        {
            string linha = reader.ReadLine()!;
            stringBuilder.AppendLine(linha);
        }

        return stringBuilder.ToString();
    }
}