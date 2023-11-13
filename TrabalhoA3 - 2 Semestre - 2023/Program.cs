namespace TrabalhoA3
{
    internal class Program
    {
        static void Main()
        {
            List<Distancia> distancias = LerDistanciasDoArquivo("distancias.txt");
            while (true)
            {

                if (Console.ReadLine() == " ")
                {
                    
                }
                
                Console.Clear();
                Console.WriteLine("Menu:");
                Console.WriteLine("1) Calcular a distância entre dois pontos e desenhar o mapa");
                Console.WriteLine("2) Criar uma Pilha");
                Console.WriteLine("3) Criar uma Fila");
                Console.WriteLine("4) Sair");

                if (int.TryParse(Console.ReadLine(), out int opcao))
                {
                    Console.Clear();
                    switch (opcao)
                    {
                        case 1:
                            Console.WriteLine("Informe o ponto de partida:");
                            var ponto1 = Console.ReadLine();
                            Console.WriteLine();
                            Console.WriteLine("Informe o ponto de destino:");
                            var ponto2 = Console.ReadLine();
                            Console.WriteLine();

                            var distancia = EncontrarDistancia(distancias, ponto1!, ponto2!);
                            Console.WriteLine();
                            if (distancia != null)
                                Console.WriteLine("A distância entre {0} e {1} é {2}", ponto1, ponto2, distancia.DistanciaPontos);
                            else
                                Console.WriteLine("Pontos não encontrados.");

                            break;

                        case 2:
                            {
                                Stack<string> pilha = new();

                                foreach (var dist in distancias)
                                    pilha.Push(dist.PontoInicial);

                                Console.WriteLine("Pilha:");
                                while (pilha.Count > 0)
                                    Console.WriteLine(pilha.Pop());

                                break;
                            }

                        case 3:
                            {
                                Queue<string> fila = new();

                                foreach (var dist in distancias)
                                    fila.Enqueue(dist.PontoInicial);

                                Console.WriteLine("Fila:");
                                while (fila.Count > 0)
                                    Console.WriteLine(fila.Dequeue());

                                break;
                            }

                        case 4:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Opção inválida");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Opção inválida. Por favor, insira um número válido.");
                }

                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para continuar");
                Console.ReadLine();
            }
        }

        private static List<Distancia> LerDistanciasDoArquivo(string nomeArquivo)
        {
            List<Distancia> distancias = new();
            using (StreamReader reader = new(nomeArquivo))
            {
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
            }
            return distancias;
        }

        private static Distancia? EncontrarDistancia(List<Distancia> distancias, string pontoInicial, string pontoFinal)
        {
            Dictionary<string, Distancia?> distanciasMinimas = new();
            Dictionary<string, int> distanciaTotal = new();

            distanciasMinimas[pontoInicial] = null;
            distanciaTotal[pontoInicial] = 0;

            Queue<string> fila = new();
            fila.Enqueue(pontoInicial);
            Console.WriteLine("Pontos verificados:");
            while (fila.Count > 0)
            {
                string pontoAtual = fila.Dequeue();

                foreach (var distancia in distancias)
                {
                    if (distancia.PontoInicial == pontoAtual)
                    {
                        string proximoPonto = distancia.PontoFinal;
                        if (!distanciasMinimas.ContainsKey(proximoPonto))
                        {
                            distanciasMinimas[proximoPonto] = distancia;
                            distanciaTotal[proximoPonto] = distanciaTotal[pontoAtual] + distancia.DistanciaPontos;
                            fila.Enqueue(proximoPonto);
                            Console.WriteLine($"Ponto atual: {pontoAtual}, Próximo ponto: {proximoPonto}, Distância: {distancia.DistanciaPontos}, Distância total: {distanciaTotal[proximoPonto]}");
                        }
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Pontos percorridos:");

            if (distanciasMinimas.ContainsKey(pontoFinal))
            {
                string ponto = pontoFinal;
                List<Distancia> caminho = new();
                while (ponto != pontoInicial)
                {
                    var distancia = distanciasMinimas[ponto];
                    caminho.Insert(0, distancia!);
                    ponto = distancia!.PontoInicial;
                }

                var distanciaTotalPercorrida = distanciaTotal[pontoFinal];
                caminho.Last().DistanciaPontos = distanciaTotalPercorrida;

                foreach (var passo in caminho)
                    Console.WriteLine($"Ponto Inicial: {passo.PontoInicial}, Ponto Final: {passo.PontoFinal}, Distância: {passo.DistanciaPontos}");

                return caminho.Last();
            }

            return null;
        }
    }
}
