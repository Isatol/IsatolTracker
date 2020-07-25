using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encrypt
{
    public static class AES
    {

        /*
         * Extraido de: http://mx.answers.yahoo.com/question/index?qid=20120604195555AAX1FtS
         * 
         * Por ejemplo, para tu programa puedes usar el algoritmo 
         * AES (Rijndael) http://en.wikipedia.org/wiki/Advanced_Encryption_Standard 
         * es moderno y seguro, supuestamente lo usa el gobierno de los Estados Unidos para cifrar información clasificada. 
           Te dejo un ejemplo de la clase AES, con los métodos Encriptar() y Desencriptar() 
         * para fácil uso, los arreglos de bytes son las claves secretas que usa el algoritmo, 
         * pueden ser de 126, 128 o 256 bits… clave de ejemplo tiene 32 bytes 
         * o sea que cifra los datos en bloques de 256bits, es mas larga y segura 
         * contra los ataques de fuerza bruta.
         * 
         */
        private static readonly byte[] _key = { 1, 22, 19, 111, 24, 26,
           85, 45, 114, 184, 27, 111, 37, 112, 100, 200, 241,
           24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218,
           131, 236, 53, 209 };

        private static readonly byte[] _vector = { 146, 64, 191, 104, 123,
       125, 65, 173, 231, 121, 221, 112, 79, 32, 114, 231 };

        // private static byte[] clave = { 1, 22, 19, 111, 24, 26, 
        //    85, 45, 114, 184, 27, 111, 37, 112, 100, 200, 241,
        //    24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 
        //    131, 236, 53, 209 };

        // private static byte[] vector = { 146, 64, 191, 104, 123, 
        //3, 2, 1, 231, 121, 221, 112, 79, 32, 114, 1 };

        private static ICryptoTransform enc, dec;
        private static UTF8Encoding utf8;


        /// <summary>
        /// Con Llave y Candado Personalizados!
        /// </summary>
        /// <param name="key">vector de 32 bytes para mayor seguridad (puede ser 32, 16 u 8) ej: llave = { 1, 2, 3, ..., 32 }</param>
        /// <param name="vector">candado de 16 bytes para 32 bytes  ej: candado = { 1, 2, 3, ..., 16 }</param>
        private static void Setup(byte[] key = null, byte[] vector = null)
        {
            if (key == null)
                key = _key;

            if (vector == null)
                vector = _vector;

            RijndaelManaged rm = new RijndaelManaged();
            enc = rm.CreateEncryptor(key, vector);
            dec = rm.CreateDecryptor(key, vector);
            utf8 = new UTF8Encoding();
        }

        /// <summary>
        /// Con Candado Personalizados!
        /// </summary>
        /// <param name="candado">candado de 16 bytes para 32 bytes  ej: candado = { 1, 2, 3, ..., 16 }</param>
        private static void Setup(string vector = null, bool padlock = false)
        {
            if (padlock && !string.IsNullOrEmpty(vector))
            {
                // Garantiza que no sea de mas de 16 bytes
                if (vector.Length > 16)
                    vector = vector.Substring(0, 16);
                // Rellena si es de menos de 16 bytes
                for (int i = vector.Length; i < _vector.Length; i++)
                    vector += Convert.ToChar(_vector[i]).ToString();
            }
            Setup(_key, string.IsNullOrEmpty(vector) ? _vector : Encoding.ASCII.GetBytes(vector));
        }

        private static byte[] Transform(byte[] buffer, ICryptoTransform ict)
        {
            MemoryStream stream = new MemoryStream();
            using (var cs = new CryptoStream(stream, ict, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            return stream.ToArray();
        }

        public static string Encrypt(string data, byte[] key = null, byte[] vector = null)
        {
            Setup(key, vector);
            byte[] buffer = Transform(utf8.GetBytes(data), enc);
            return Convert.ToBase64String(buffer);
        }

        public static string Encrypt(string vector, string data, bool padlock = false)
        {
            Setup(vector, padlock);
            byte[] buffer = Transform(utf8.GetBytes(data), enc);
            return Convert.ToBase64String(buffer);
        }

        public static string Decrypt(string data, byte[] key = null, byte[] vector = null)
        {
            Setup(key, vector);
            byte[] buffer = Convert.FromBase64String(data);
            return utf8.GetString(Transform(buffer, dec));
        }

        public static string Decrypt(string vector, string data, bool padlock = false)
        {
            Setup(vector, padlock);
            byte[] buffer = Convert.FromBase64String(data);
            return utf8.GetString(Transform(buffer, dec));
        }


    }
}