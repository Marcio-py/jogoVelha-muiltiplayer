using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Servidor
{
    private TcpListener server;
    private List<TcpClient> clientes = new List<TcpClient>();

    public Servidor(int porta)
    {
        server = new TcpListener(IPAddress.Any, porta);
        server.Start();
        Console.WriteLine($"Servidor iniciado na porta {porta}");
    }

    public async Task IniciarAsync()
    {
        while (true)
        {
            var cliente = await server.AcceptTcpClientAsync();
            clientes.Add(cliente);
            Console.WriteLine("Cliente conectado");
        }
    }

    public async Task EnviarEstadoParaClientes(string estado)
    {
        byte[] dados = Encoding.UTF8.GetBytes(estado);

        foreach (var cliente in clientes)
        {
            if (cliente.Connected)
            {
                var stream = cliente.GetStream();
                await stream.WriteAsync(dados, 0, dados.Length);
            }
        }
    }
}
