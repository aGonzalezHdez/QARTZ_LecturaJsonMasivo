using System.Text;
using System.Security.Cryptography;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class Cryptografia
    {
        // Encripta una cadena de texto usando el algoritmo de encriptacion de hash MD5.
        // el "Message Digest" es una encriptacion de 128-bit y es usado comunmente para
        // verificar datos chequeando el "Checksum MD5", mas informacion se puede
        // encontrar en: http://www.faqs.org/rfcs/rfc1321.html
        // 

        // cadena conteniendo el string a hashear a MD5.
        // Una cadena de texto conteniendo en forma encriptada la cadena ingresada.
        public static string MD5Hash(string Data)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
                stringBuilder.AppendFormat("{0:x2}", b);
            return stringBuilder.ToString();
        }

        // Encripta una cadena utilizando el algoritmo SHA256 (Secure Hash Algorithm)
        // Detalles: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        // Esto trabaja de misma manera que el MD5, solo que utilizando una
        // encriptacion en 256 bits.

        // Un string conteniendo los datos a encriptar.
        // Un string conteniendo al string de entrada, encriptado con el algoritmo SHA256.
        public static string SHA256Hash(string Data)
        {
            SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
                stringBuilder.AppendFormat("{0:x2}", b);
            return stringBuilder.ToString();
        }

        // Encripta una cadena utilizando el algoritmo SHA256 (Secure Hash Algorithm)
        // Detalles: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        // Esto trabaja de misma manera que el MD5, solo que utilizando una
        // encriptacion en 256bits.

        // Un string conteniendo los datos a encriptar.
        // Un string conteniendo al string de entrada, encriptado con el algoritmo SHA384.
        public static string SHA384Hash(string Data)
        {
            SHA384 sha = SHA384.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
                stringBuilder.AppendFormat("{0:x2}", b);
            return stringBuilder.ToString();
        }

        // Encripta una cadena utilizando el algoritmo SHA256 (Secure Hash Algorithm)
        // Detalles: http://www.itl.nist.gov/fipspubs/fip180-1.htm
        // Esto trabaja de misma manera que el MD5, solo que utilizando una
        // encriptacion en 512 bits.

        // Un string conteniendo los datos a encriptar.
        // Un string conteniendo al string de entrada, encriptado con el algoritmo SHA512.
        public static string SHA512Hash(string Data)
        {
            SHA512 sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
                stringBuilder.AppendFormat("{0:x2}", b);
            return stringBuilder.ToString();
        }
    }

}
