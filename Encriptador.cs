using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Calculadora
{
    public static class Encriptador
    {
        private static readonly string clave = "clave12345678901"; // 16 caracteres
        private static readonly byte[] IV = new byte[16]; // Vector de inicialización (ceros)

        public static string Encriptar(string texto)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(clave);
                aes.IV = IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(texto);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string textoCifrado)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(clave);
                aes.IV = IV;

                byte[] bytesCifrados = Convert.FromBase64String(textoCifrado);

                using (MemoryStream ms = new MemoryStream(bytesCifrados))
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
