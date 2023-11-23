namespace TrabalhoA3
{
    public static class Program
    {
        static void Main()
        {
            List<Distancia> distancias = Utils.LerDistanciasDoArquivo("distancias.txt");
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

                            Dijkstra.EncontrarDistancia(distancias, ponto1!, ponto2!);

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
    }
}