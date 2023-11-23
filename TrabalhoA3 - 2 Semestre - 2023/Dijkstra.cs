namespace TrabalhoA3;

public static class Dijkstra
{
    public static void EncontrarDistancia(List<Distancia> distancias, string pontoInicial, string pontoFinal)
    {
        var grafo = new Dictionary<string, List<Distancia>>();

        foreach (var item in distancias)
        {
            if (!grafo.ContainsKey(item.PontoInicial))
                grafo[item.PontoInicial] = new List<Distancia>();

            grafo[item.PontoInicial].Add(item);
        }

        var (caminho, distanciaTotal) = LogicaDijkstra(grafo, pontoInicial, pontoFinal);

        if (caminho.Count == 0)
        {
            Console.WriteLine("Nenhum caminho encontrado!");
            return;
        }

        Console.WriteLine($"Caminho verificado: {string.Join(" -> ", caminho)}");
        Console.WriteLine($"Caminho percorrido: {ObterCaminhoPercorrido(distancias, caminho)}");
        Console.WriteLine($"Distância total: {distanciaTotal}");
    }

    private static string ObterCaminhoPercorrido(IReadOnlyCollection<Distancia> distancias, IReadOnlyList<string> caminho)
    {
        var caminhoPercorrido = new List<string>();
        for (int i = 0; i < caminho.Count - 1; i++)
        {
            var pontoInicial = caminho[i];
            var pontoFinal = caminho[i + 1];

            var distancia = distancias.FirstOrDefault(d =>
                string.Equals(d.PontoInicial, pontoInicial, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.PontoFinal, pontoFinal, StringComparison.OrdinalIgnoreCase));

            if (distancia != null)
                caminhoPercorrido.Add($"{pontoInicial} -> {pontoFinal} ({distancia.DistanciaPontos})");
        }

        return string.Join(", ", caminhoPercorrido);
    }

    private static (List<string> caminho, int distanciaTotal) LogicaDijkstra(Dictionary<string, List<Distancia>> grafo, string pontoInicial, string pontoFinal)
    {
        var distancia = new Dictionary<string, int>();
        var anterior = new Dictionary<string, string?>();
        var naoVisitados = new List<string>();

        foreach (var ponto in grafo.Keys)
        {
            distancia[ponto] = int.MaxValue;
            anterior[ponto] = null;
            naoVisitados.Add(ponto);
        }

        distancia[pontoInicial] = 0;

        while (naoVisitados.Count > 0)
        {
            var pontoAtual = ObterPontoMenorDistancia(distancia, naoVisitados);
            naoVisitados.Remove(pontoAtual);

            if (pontoAtual == pontoFinal)
            {
                var caminho = new List<string>();
                var ponto = pontoFinal;

                while (ponto != null)
                {
                    caminho.Insert(0, ponto);
                    ponto = anterior[ponto];
                }

                return (caminho, distancia[pontoFinal]);
            }

            foreach (var vizinho in grafo[pontoAtual])
            {
                var distanciaAtualizada = distancia[pontoAtual] + vizinho.DistanciaPontos;

                if (distanciaAtualizada >= distancia[vizinho.PontoFinal])
                    continue;
                distancia[vizinho.PontoFinal] = distanciaAtualizada;
                anterior[vizinho.PontoFinal] = pontoAtual;
            }
        }

        return (new List<string>(), 0);
    }

    private static string ObterPontoMenorDistancia(IReadOnlyDictionary<string, int> distancia, IReadOnlyCollection<string> naoVisitados)
    {
        var pontoMenorDistancia = naoVisitados.First();

        foreach (var ponto in naoVisitados.Where(ponto => distancia[ponto] < distancia[pontoMenorDistancia]))
            pontoMenorDistancia = ponto;

        return pontoMenorDistancia;
    }
}