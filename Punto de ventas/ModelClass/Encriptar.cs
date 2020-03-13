using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.ModelClass
{
   public class Encriptar
    {
      private static RijndaelManaged rm = new RijndaelManaged();

        public Encriptar()
        {
            // establece el modo para el funcionamiento del algoritmo
            rm.Mode = CipherMode.CBC;
            //establece el modo de relleno utilizado en el algoritmo.
            rm.Padding = PaddingMode.PKCS7;
            //establece el tamaño, en bits, para la clave secreta.
            rm.KeySize = 0x80;
            // establece el tamaño del bloque en bits para la operacion criptografica.
            rm.BlockSize = 0x80;
        }
        public static string EncryptData(string textData, string EncryptionKey)
        {
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            // establece el vector de inicialización (IV) para el algoritmo simétrico
            byte[] EncryptionKeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,};
            int len = passBytes.Length;

            if (len > EncryptionKeyBytes.Length)
            {
                len = EncryptionKeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionKeyBytes, len);
            rm.Key = EncryptionKeyBytes;
            rm.IV = EncryptionKeyBytes;
            //Crea un objeto AES simétrico con la clave actual y el vector de inicialización IV.
            ICryptoTransform objtransform = rm.CreateEncryptor();
            byte[] textDatabyte = Encoding.UTF8.GetBytes(textData);
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDatabyte, 0,
                textDatabyte.Length));
        }

        public static string DecryptData( string Encryptedext, string EncryptionKey)
        {
            byte[] encryptedTextByte = Convert.FromBase64String(Encryptedext);
            byte[] passBytes = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] EncryptionKeyBytes = new byte[0x10];
            int len = passBytes.Length;

            if (len > EncryptionKeyBytes.Length)
            {
                len = EncryptionKeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionKeyBytes, len);
            rm.Key = EncryptionKeyBytes;
            rm.IV = EncryptionKeyBytes;
            byte[] TextByte = rm.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0,
                encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);
        }
    }
}
