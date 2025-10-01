using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    public class ValidarEmail
    {
        public bool IsValidEmailFormat(string s)
        {
            return Regex.IsMatch(s, @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        }
    }
}
