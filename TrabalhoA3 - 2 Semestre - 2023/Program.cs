namespace TrabalhoA3
{
    internal class Program
    {
        static void Main()
        {
            List<Distancia> distancias = LerDistanciasDoArquivo("distancias.txt");
            while (true)
            {
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
                                Console.WriteLine("A distância entre {0} e {1} é {2}", ponto1, ponto2,
                                    distancia.DistanciaPontos);
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

        private static Distancia? EncontrarDistancia(List<Distancia> distancias, string pontoInicial, string pontoFinal)
        {
            Dictionary<string, int> menorCaminho = new Dictionary<string, int>();

            foreach (var ponto in distancias.Select(d => d.PontoInicial)
                                                 .Union(distancias.Select(d => d.PontoFinal))
                                                 .Distinct())
            {
                menorCaminho[ponto] = (ponto == pontoInicial) ? 0 : int.MaxValue;
            }

            while (true)
            {
                string? pontoMenorDistancia = null;
                int menorDistancia = int.MaxValue;

                foreach (var ponto in menorCaminho.Where(pair => pair.Value < int.MaxValue))
                {
                    if (ponto.Value >= menorDistancia) continue;
                    pontoMenorDistancia = ponto.Key;
                    menorDistancia = ponto.Value;
                }

                if (pontoMenorDistancia == null) break;

                if (pontoMenorDistancia == pontoFinal) break;

                foreach (var distancia in distancias.Where(d => d.PontoInicial == pontoMenorDistancia))
                {
                    int novaDistancia = menorCaminho[pontoMenorDistancia] + distancia.DistanciaPontos;
                    if (novaDistancia < menorCaminho[distancia.PontoFinal])
                    {
                        menorCaminho[distancia.PontoFinal] = novaDistancia;
                    }
                }

                menorCaminho[pontoMenorDistancia] = int.MaxValue;
            }

            if (menorCaminho.TryGetValue(pontoFinal, out int value) && value < int.MaxValue)
            {
                return new Distancia
                {
                    PontoInicial = pontoInicial,
                    PontoFinal = pontoFinal,
                    DistanciaPontos = menorCaminho[pontoFinal]
                };
            }
            
            return null;
        }
    }
}