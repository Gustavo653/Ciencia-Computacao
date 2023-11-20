namespace TrabalhoA3;

public class Dijkstra
{
    public static Distancia? EncontrarDistancia(IReadOnlyCollection<Distancia> distancias, string pontoInicial,
        string pontoFinal)
    {
        Dictionary<string, int> menorCaminho = new Dictionary<string, int>();
        Dictionary<string, string> caminhoAnterior = new Dictionary<string, string>();
        Dictionary<string, int> custoTotalCaminhos = new Dictionary<string, int>();
        HashSet<string> visitados = new HashSet<string>();

        foreach (var ponto in distancias.Select(d => d.PontoInicial)
                     .Union(distancias.Select(d => d.PontoFinal))
                     .Distinct())
        {
            menorCaminho[ponto] = (ponto == pontoInicial) ? 0 : int.MaxValue;
            custoTotalCaminhos[ponto] = 0;
        }

        while (menorCaminho.Any(pair => pair.Value < int.MaxValue))
        {
            string pontoMenorDistancia = menorCaminho
                .Where(pair => !visitados.Contains(pair.Key))
                .OrderBy(pair => pair.Value)
                .FirstOrDefault().Key;

            if (pontoMenorDistancia == null) break;

            visitados.Add(pontoMenorDistancia);

            foreach (var distancia in distancias.Where(d => d.PontoInicial == pontoMenorDistancia))
            {
                int novaDistancia = menorCaminho[pontoMenorDistancia] + distancia.DistanciaPontos;
                if (novaDistancia >= menorCaminho[distancia.PontoFinal]) continue;
                menorCaminho[distancia.PontoFinal] = novaDistancia;
                caminhoAnterior[distancia.PontoFinal] = pontoMenorDistancia;
                custoTotalCaminhos[distancia.PontoFinal] = custoTotalCaminhos[pontoMenorDistancia] + distancia.DistanciaPontos;
            }
        }

        if (!menorCaminho.TryGetValue(pontoFinal, out int distanciaFinal) || distanciaFinal >= int.MaxValue)
            return null;
        Distancia distanciaEncontrada = new Distancia
        {
            PontoInicial = pontoInicial,
            PontoFinal = pontoFinal,
            DistanciaPontos = distanciaFinal
        };

        IEnumerable<string> caminhoOficial = ConstruirCaminho(caminhoAnterior, pontoInicial, pontoFinal);

        Console.WriteLine("Caminho Verificado:");
        foreach (var caminho in caminhoAnterior)
        {
            Console.WriteLine($"{caminho.Value} -> {caminho.Key}, Distância: {custoTotalCaminhos[caminho.Key]}");
        }

        Console.WriteLine("\nCaminho Percorrido:");
        Console.WriteLine(string.Join(" -> ", caminhoOficial) + $", Distância: {distanciaFinal}");

        return distanciaEncontrada;
    }

    private static IEnumerable<string> ConstruirCaminho(IReadOnlyDictionary<string, string> caminhoAnterior, string pontoInicial, string pontoFinal)
    {
        List<string> caminho = new List<string>();
        string pontoAtual = pontoFinal;

        while (caminhoAnterior.ContainsKey(pontoAtual))
        {
            caminho.Insert(0, pontoAtual);
            pontoAtual = caminhoAnterior[pontoAtual];
        }

        caminho.Insert(0, pontoInicial);
        return caminho;
    }
}