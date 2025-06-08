using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculadora;

namespace Servidor1
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() => IniciarServidor());
            txtLog.AppendText("Servidor iniciado...\r\n");
        }

        private void IniciarServidor()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 9001);
            listener.Start();

            while (true)
            {
                using (TcpClient cliente = listener.AcceptTcpClient())
                using (NetworkStream stream = cliente.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesLeidos = stream.Read(buffer, 0, buffer.Length);
                    string operacionCifrada = Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    string operacion = Encriptador.Desencriptar(operacionCifrada);

                    string resultado;
                    try
                    {
                        DataTable dt = new DataTable();
                        object eval = dt.Compute(operacion, "");
                        resultado = eval.ToString();
                    }
                    catch
                    {
                        resultado = "Error de operación";
                    }

                    string resultadoCifrado = Encriptador.Encriptar(resultado);
                    byte[] respuestaBytes = Encoding.UTF8.GetBytes(resultadoCifrado);
                    stream.Write(respuestaBytes, 0, respuestaBytes.Length);

                    Invoke((Action)(() => txtLog.AppendText($"Recibido: {operacion} → {resultado}\r\n")));
                }
            }
        }
    }
}

