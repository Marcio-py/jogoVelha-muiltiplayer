//using System;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//class Cliente
//{
//    static async Task Main()
//    {
//        const string EnderecoServidor = "127.0.0.1";
//        const int PortaServidor = 5000;

//        using TcpClient cliente = new TcpClient();
//        await cliente.ConnectAsync(EnderecoServidor, PortaServidor);
//        Console.WriteLine("Conectado ao servidor!");

//        var stream = cliente.GetStream();
//        byte[] buffer = new byte[1024];

//        while (true)
//        {
//            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
//            if (bytesRead == 0) break;

//            string estado = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//            ExibirTabuleiro(estado);
//        }
//    }

//    static void ExibirTabuleiro(string estado)
//    {
//        int tamanho = (int)Math.Sqrt(estado.Length);
//        Console.Clear();
//        Console.WriteLine("Tabuleiro Atualizado:");
//        for (int i = 0; i < tamanho; i++)
//        {
//            for (int j = 0; j < tamanho; j++)
//            {
//                Console.Write(estado[i * tamanho + j] + " ");
//            }
//            Console.WriteLine();
//        }
//    }
//}
