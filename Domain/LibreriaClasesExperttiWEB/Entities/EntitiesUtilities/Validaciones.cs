using Microsoft.VisualBasic;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesUtilities
{
    public class Validaciones
    {
        public bool ValidaGuias(string No_Guia, string Identificador)
        {
            string numero;
            int Digito_calculado;
            string Digito_verificador;

            try
            {
                if (No_Guia.Length == 10)
                {
                    switch (Identificador ?? "")
                    {
                        case "H":
                            {
                                if (Strings.Len(No_Guia) > 10)
                                {
                                    return false;
                                }

                                numero = Strings.Mid(No_Guia, 1, 9);
                                Digito_verificador = Strings.Mid(No_Guia, 10, 1);
                                Digito_calculado = int.Parse(numero) % 7;

                                if (int.Parse(Digito_verificador) == Digito_calculado)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                                return true;
                            }

                        default:
                            {
                                return false;
                            }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}
