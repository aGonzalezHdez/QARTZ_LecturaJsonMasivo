using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    public class ValidarCurp
    {


        // Método para validar CURP
        public void ValidateCurp(string curp)
        {
            curp = curp.ToUpper();

            Regex CurpRegex = new(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$");
            if (string.IsNullOrWhiteSpace(curp) || curp.Length != 18)
            {
                throw new Exception("CURP no válido, favor de verificar.");
            }


            if (!CurpRegex.IsMatch(curp))
            {
                throw new Exception($"Estimado usuario, el CURP {curp} no cumple con las características establecidas por el SAT.");
            }

            // Validar la homoclave
            //string homoclave = curp.Substring(11, 2);
            //if (!char.IsDigit(homoclave[0]) || !char.IsDigit(homoclave[1]))
            //{
            //    throw new Exception("La homoclave del CURP no válido, favor de verificar.");
            //}           
        }
    }
}
