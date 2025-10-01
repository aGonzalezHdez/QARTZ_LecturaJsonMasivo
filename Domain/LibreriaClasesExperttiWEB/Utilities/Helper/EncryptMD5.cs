using System.Security.Cryptography;
using System.Text;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class EncryptMD5
    {
        public string Encrypt(string mensaje)
        {
            string hash = "Codigo en C encriptado";
            byte[] data = Encoding.UTF8.GetBytes(mensaje);

            MD5 md5 = MD5.Create();
            TripleDES tripides = TripleDES.Create();
            tripides.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
            tripides.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripides.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);

        }

        public string Decrypt(string menasjeEn)
        {
            string hash = "Codigo en C encriptado";
            byte[] data = Convert.FromBase64String(menasjeEn);

            MD5 md5 = MD5.Create();
            TripleDES tripides = TripleDES.Create();
            tripides.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(hash));
            tripides.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripides.CreateDecryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Encoding.UTF8.GetString(result);
        }
    }
}
