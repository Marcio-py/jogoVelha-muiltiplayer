using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static char[,] matriz;
    static List<char> jogadores;
    static int tamanhoMatriz, sequenciaVitoria;
    static int indiceJogadorAtual = 0;
    static bool jogoTerminado = false;
    static Servidor servidor;
    const int PortaServidor = 5000;

    static async Task Main()
    {
        servidor = new Servidor(PortaServidor);
        _ = servidor.IniciarAsync();

        DefinirNumeroDeJogadores();
        DefinirTamanhoMatriz();
        DefinirSequenciaVitoria();
        InicializarMatriz();

        while (!jogoTerminado)
        {
            ExibirMatriz();
            RealizarJogada();
            VerificarVitoria();
            VerificarEmpate();
            AlternarJogador();
            await AtualizarServidor();
        }
    }

    static void DefinirNumeroDeJogadores()
    {
        Console.Write("Escolha o número de jogadores (mínimo 2): ");
        int numeroDeJogadores = int.Parse(Console.ReadLine());
        numeroDeJogadores = Math.Max(2, numeroDeJogadores);

        jogadores = new List<char>();
        for (int i = 0; i < numeroDeJogadores; i++)
        {
            jogadores.Add((char)('A' + i));
        }
        Console.WriteLine($"Jogadores: {string.Join(", ", jogadores)}");
    }

    static void DefinirTamanhoMatriz()
    {
        Console.Write("Escolha o tamanho da matriz (mínimo 3): ");
        tamanhoMatriz = Math.Max(3, int.Parse(Console.ReadLine()));
        matriz = new char[tamanhoMatriz, tamanhoMatriz];
    }

    static void DefinirSequenciaVitoria()
    {
        Console.Write("Escolha o comprimento da sequência vencedora (mínimo 3): ");
        sequenciaVitoria = Math.Max(3, int.Parse(Console.ReadLine()));
    }

    static void InicializarMatriz()
    {
        for (int i = 0; i < tamanhoMatriz; i++)
            for (int j = 0; j < tamanhoMatriz; j++)
                matriz[i, j] = ' ';
    }

    static void ExibirMatriz()
    {
        Console.Clear();
        Console.WriteLine("Tabuleiro:");
        for (int i = 0; i < tamanhoMatriz; i++)
        {
            for (int j = 0; j < tamanhoMatriz; j++)
            {
                Console.Write(matriz[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    static void RealizarJogada()
    {
        char jogadorAtual = jogadores[indiceJogadorAtual];
        int linha, coluna;
        while (true)
        {
            Console.WriteLine($"Jogador {jogadorAtual}, é sua vez.");
            Console.Write("Informe a linha: ");
            linha = int.Parse(Console.ReadLine());

            Console.Write("Informe a coluna: ");
            coluna = int.Parse(Console.ReadLine());

            if (linha >= 0 && linha < tamanhoMatriz && coluna >= 0 && coluna < tamanhoMatriz && matriz[linha, coluna] == ' ')
            {
                matriz[linha, coluna] = jogadorAtual;
                break;
            }
            else
            {
                Console.WriteLine("Posição inválida ou já ocupada. Tente novamente.");
            }
        }
    }

    static void VerificarVitoria()
    {
        char jogadorAtual = jogadores[indiceJogadorAtual];
        for (int i = 0; i < tamanhoMatriz; i++)
        {
            for (int j = 0; j < tamanhoMatriz; j++)
            {
                if (VerificarSequencia(i, j, 0, 1, jogadorAtual) ||  // Horizontal
                    VerificarSequencia(i, j, 1, 0, jogadorAtual) ||  // Vertical
                    VerificarSequencia(i, j, 1, 1, jogadorAtual) ||  // Diagonal principal
                    VerificarSequencia(i, j, 1, -1, jogadorAtual))   // Diagonal secundária
                {
                    Console.WriteLine($"Jogador {jogadorAtual} venceu!");
                    jogoTerminado = true;
                    return;
                }
            }
        }
    }

    static bool VerificarSequencia(int linha, int coluna, int deltaLinha, int deltaColuna, char jogador)
    {
        int contagem = 0;
        for (int k = 0; k < sequenciaVitoria; k++)
        {
            int novaLinha = linha + k * deltaLinha;
            int novaColuna = coluna + k * deltaColuna;

            if (novaLinha >= 0 && novaLinha < tamanhoMatriz &&
                novaColuna >= 0 && novaColuna < tamanhoMatriz &&
                matriz[novaLinha, novaColuna] == jogador)
            {
                contagem++;
            }
            else
            {
                break;
            }
        }
        return contagem == sequenciaVitoria;
    }

    static void VerificarEmpate()
    {
        foreach (var posicao in matriz)
        {
            if (posicao == ' ') return;
        }
        Console.WriteLine("O jogo empatou!");
        jogoTerminado = true;
    }

    static void AlternarJogador()
    {
        indiceJogadorAtual = (indiceJogadorAtual + 1) % jogadores.Count;
    }

    static async Task AtualizarServidor()
    {
        string estadoAtual = EstadoParaString();
        await servidor.EnviarEstadoParaClientes(estadoAtual);
    }

    static string EstadoParaString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < tamanhoMatriz; i++)
        {
            for (int j = 0; j < tamanhoMatriz; j++)
            {
                sb.Append(matriz[i, j]);
            }
        }
        return sb.ToString();
    }
}
