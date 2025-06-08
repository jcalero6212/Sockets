using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calculadora;

namespace Cliente
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string operacion = txtOperacion.Text;
            string operacionCifrada = Encriptador.Encriptar(operacion);

            using (TcpClient cliente = new TcpClient("127.0.0.1", 9001))
            using (NetworkStream stream = cliente.GetStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(operacionCifrada);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);
                string respuestaCifrada = Encoding.UTF8.GetString(buffer, 0, bytes);
                string respuesta = Encriptador.Desencriptar(respuestaCifrada);

                txtResultado.Text = respuesta;
            }
        }
    }
    }

