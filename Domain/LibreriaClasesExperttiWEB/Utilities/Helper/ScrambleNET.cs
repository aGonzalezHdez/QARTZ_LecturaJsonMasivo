using Microsoft.VisualBasic;
using System.Text;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class ScrambleNET
    {
        //Función simétrica para encriptar/desencriptar una cadena utilizando como llave una segunda cadena.
        public string Scramble(string InputString)
        {
            string ScrambleRet = default;

            string OutputString = "";
            string InputKey;
            int j = 1;
            int i = 1;
            string ScrambleKey = string.Empty;




            //ScrambleKey = System.Convert.ToString((char)25 + (char)27 + (char)26 + (char)28 + (char)29 + (char)31);
            //ScrambleKey = Chr(25) + Chr(27) + Chr(26) + Chr(28) + Chr(29) + Chr(31);
            //ScrambleKey = "" + (char)25 + (char)27 + (char)26 + (char)28 + (char)29 + (char)31;
            ScrambleKey = "" + (char)25 + (char)27 + (char)26 + (char)28 + (char)29 + (char)31;
            InputKey = ScrambleKey;
            var loopTo = InputString.Length;



            for (j = 1; j <= loopTo; j++)
            {

                OutputString = OutputString + Convert.ToString(Strings.Chr(Strings.Asc(Strings.Mid(InputString, j, 1)) ^ Strings.Asc(Strings.Mid(InputKey, i, 1))));

                i = i == InputKey.Length ? 1 : i + 1;

            }

            ScrambleRet = OutputString;
            return ScrambleRet;

        }
        static string Chr(int CharCode)
        {
            if (CharCode > 255)
                throw new ArgumentOutOfRangeException("CharCode", CharCode, "CharCode must be between 0 and 255.");
            return Encoding.Default.GetString(new[] { (byte)CharCode });
        }

    }
}
