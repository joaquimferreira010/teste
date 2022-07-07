using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blazored.SessionStorage;


namespace SE1426.Internet.ProdamSP.Blazor.Util
{
    public class Constantes
    {
        //Para criptografar o token de recupearar a senha
        public const string PALAVRA_HASH_TOKEN = "P@@SE1426T0M@e";
        public const string PASSWORD_HASH_TOKEN = "SE1426TM@e";
        public const string SALT_KEY_TOKEN = "SSE1426@ltT0M@e";
        public const string VI_KEY_TOKEN = "SE1426x3bb&T0M@e";
    }
        public class Util
    {
        /////// <summary>
        /////// Rotina utilizada para encriptar dados
        /////// </summary>
        /////// <param name="textoAEncriptar">Texto a ser encriptado</param>
        /////// <returns>Texto encriptado</returns>
        ////public static string Encriptar(string textoAEncriptar)
        ////{
        ////    byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoAEncriptar);

        ////    byte[] keyBytes = new Rfc2898DeriveBytes(Constantes.PASSWORD_HASH_TOKEN, Encoding.ASCII.GetBytes(Constantes.SALT_KEY_TOKEN)).GetBytes(256 / 8);
        ////    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
        ////    var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(Constantes.VI_KEY_TOKEN));

        ////    byte[] cipherTextBytes;

        ////    using (var memoryStream = new MemoryStream())
        ////    {
        ////        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        ////        {
        ////            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        ////            cryptoStream.FlushFinalBlock();
        ////            cipherTextBytes = memoryStream.ToArray();
        ////            cryptoStream.Close();
        ////        }
        ////        memoryStream.Close();
        ////    }
        ////    return Convert.ToBase64String(cipherTextBytes);
        ////}

        /////// <summary>
        /////// Rotina utilizada para decriptar dados
        /////// </summary>
        /////// <param name="textoADecriptar">Texto a ser decriptado</param>
        /////// <returns>Texto decriptado</returns>
        ////public static string Decriptar(string textoADecriptar)
        ////{
        ////    byte[] cipherTextBytes = Convert.FromBase64String(textoADecriptar);
        ////    byte[] keyBytes = new Rfc2898DeriveBytes(Constantes.PASSWORD_HASH_TOKEN, Encoding.ASCII.GetBytes(Constantes.SALT_KEY_TOKEN)).GetBytes(256 / 8);
        ////    var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

        ////    var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(Constantes.VI_KEY_TOKEN));
        ////    var memoryStream = new MemoryStream(cipherTextBytes);
        ////    var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        ////    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

        ////    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        ////    memoryStream.Close();
        ////    cryptoStream.Close();
        ////    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        ////}








        public static string Encriptar(string valor)
        {

            string texto = valor;

            string key = Constantes.PALAVRA_HASH_TOKEN + Constantes.PASSWORD_HASH_TOKEN;

            Criptografia crip = new Criptografia(CryptProvider.Rijndael);

            crip.Key = key;

            return crip.Encrypt(texto);

        }
        public static string Decriptar(string valor)

        {
            if (string.IsNullOrEmpty(valor))
            {
                return "";
            }
            string texto = valor;


            //Está chave tem que ser a mesma que a do texto Encriptado.

            string key = Constantes.PALAVRA_HASH_TOKEN + Constantes.PASSWORD_HASH_TOKEN;

            Criptografia crip = new Criptografia(CryptProvider.Rijndael);

            crip.Key = key;

            return crip.Decrypt(texto);

        }

    }

}

