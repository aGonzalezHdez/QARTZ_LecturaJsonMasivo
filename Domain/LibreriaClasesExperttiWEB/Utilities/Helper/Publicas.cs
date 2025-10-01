using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Utilities.Helper
{
    public class Publicas
    {
        public string ReadFileText(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }
        public string MyReadFile(string myPath)
        {
            string fileContent = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(myPath))
                {
                    fileContent = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                fileContent = ex.Message;
            }

            return fileContent;
        }
        public string EliminaEnter(string s)
        {
            string EliminaEnter = default;
            string straux;
            //string straux2 = "";
            long i;
            //int PosEnter = 0;
            straux = "";
            s = Strings.Trim(s);

            for (i = 1; i <= Strings.Len(s); i++)
            {
                switch (Strings.Asc(Strings.Mid(s, Convert.ToInt32(i), 1)))
                {
                    case 13:
                        {
                            break;
                        }

                    case 10:
                        {
                            break;
                        }

                    case 0:
                        {
                            break;
                        }

                    default:
                        {
                            straux = straux + Strings.Mid(s, Convert.ToInt32(i), 1);
                            break;
                        }
                }
            }

            EliminaEnter = straux;
            return EliminaEnter;
        }

        /// <remarks></remarks>
        public string EliminaComillas(string s)
        {
            string EliminaComillas = default;


            string straux;
            long i;
            int ExistenComillas = 0;

            straux = "";
            ExistenComillas = Strings.InStr(s, "\"");
            if (ExistenComillas != 0)
            {
                for (i = 1; i <= Strings.Len(s); i++)
                {
                    if (Strings.Mid(s, Convert.ToInt32(i), 1) != "\"")
                        straux = straux + Strings.Mid(s, Convert.ToInt32(i), 1);
                }
            }
            else
                straux = s;

            EliminaComillas = Strings.Trim(straux);
            return EliminaComillas;
        }
        public string BuscarRegistroDeError(string FileName, string MiLinea, string MiTipo, string MiCampo, string MiNumero, string MiRegex500)
        {
            string resultado = "";

            string NombreDeArchivo = "";
            string ExtencionDeArchivo = "";
            Regex re = new Regex(MiRegex500, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;

            NombreDeArchivo = Path.GetFileNameWithoutExtension(FileName);
            NombreDeArchivo = NombreDeArchivo.Substring(5, 3); // Usamos Substring en lugar de Mid
            ExtencionDeArchivo = Path.GetExtension(FileName);
            ContenidoDelFicheroOriginal = MyReadFile(FileName);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            for (int i = 0; i < mc.Count; i++)
            {
                if (i == Convert.ToInt32(MiLinea))
                {
                    Console.Beep(); // Emula el sonido de 'Beep()' en VB.NET
                }
            }

            return resultado;
        }       
    }
}
