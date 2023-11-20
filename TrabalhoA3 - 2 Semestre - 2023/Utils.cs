namespace TrabalhoA3;

public static class Utils
{
    public static List<Distancia> LerDistanciasDoArquivo(string nomeArquivo)
    {
        List<Distancia> distancias = new();
        using StreamReader reader = new(nomeArquivo);
        while (!reader.EndOfStream)
        {
            string linha = reader.ReadLine()!;
            string[] valores = linha.Split(';');

            Distancia distancia = new()
            {
                PontoInicial = valores[0],
                PontoFinal = valores[1],
                DistanciaPontos = int.Parse(valores[2])
            };

            distancias.Add(distancia);
        }

        return distancias;
    }
}