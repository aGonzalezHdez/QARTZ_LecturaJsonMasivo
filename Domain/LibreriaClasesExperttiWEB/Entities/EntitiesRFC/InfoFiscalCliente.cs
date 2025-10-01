using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesRFC
{
    public class InfoFiscalCliente
    {
        /*Se deshabilita modelo antiguo para basarnos en sus validaciones
        //[Required(ErrorMessage = "El número de Guía House es requerido"), StringLength(50)]
        public string? Guia { get; set; } = string.Empty;

        public int? IdCustomAlert { get; set; }

        //[Required(ErrorMessage = "El Nombre Completo o Razón Social es requerido")]
        [StringLength(100, ErrorMessage = "El Nombre Completo o Razón Social no puede exceder 100 caracteres")]
        public string? Nombre { get; set; } = string.Empty;

        //[Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El número telefónico debe tener al menos 8 dígitos.")]
        [RegularExpression(@"^\d{8,}$", ErrorMessage = "El número telefónico debe contener solo dígitos, sin espacios ni símbolos.")]
        public string? Telefono { get; set; }

        //[Required(ErrorMessage = "El email es requerido")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
        ErrorMessage = "El correo no tiene un formato válido.")]
        [JsonProperty(PropertyName = "email", DefaultValueHandling = DefaultValueHandling.Populate)]
        [System.ComponentModel.DefaultValue("correo@ejemplo.com")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? Email { get; set; }

        [CustomValidation(typeof(Validaciones), "ValidarCURP")]
        public string? Curp { get; set; }

        //[Required(ErrorMessage = "El RFC es requerido")]
        [CustomValidation(typeof(Validaciones), "ValidarRFC")]
        public string? Rfc { get; set; }

        //[Required]
        public int? IdUsuario { get; set; }
        */

        public string? GuiaHouse { get; set; }
        public int? IdCustomAlert { get; set; }
        public string? Nombre { get; set; }
        public string? RFC { get; set; }
        public string? CURP { get; set; }
        public string? NSS { get; set; } = "";
        public string? Telefono { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Contacto { get; set; }
        public int IDCapturo { get; set; }
        public bool Activo { get; set; }
        public string? DocExtranjero { get; set; }
        public string? IDCIF { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdDatosEmpresa { get; set; }
        public bool? EsPersonaMoral { get; set; }
        public string? RazonSocial { get; set; }

    }

    public class InfoFiscalClienteGuia
    {
        [Required, StringLength(50)]
        public string GuiaHouse { get; set; } = string.Empty;
    }

    public class InfoFiscalClienteUpdate
    {
        /*Revisar validaciones para aplicar en el nuevo modelo
        //[Required(ErrorMessage = "El número de Guía House es requerido"), StringLength(50)]
        public string? Guia { get; set; }

        //[Required(ErrorMessage = "El Nombre Completo o Razón Social es requerido")]
        [StringLength(100, ErrorMessage = "El Nombre Completo o Razón Social no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }

        //[Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El número telefónico debe tener al menos 8 dígitos.")]
        [RegularExpression(@"^\d{8,}$", ErrorMessage = "El número telefónico debe contener solo dígitos, sin espacios ni símbolos.")]
        public string? Telefono { get; set; }

        //[Required(ErrorMessage = "El email es requerido")]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
        ErrorMessage = "El correo no tiene un formato válido.")]
        [JsonProperty(PropertyName = "email", DefaultValueHandling = DefaultValueHandling.Populate)]
        [System.ComponentModel.DefaultValue("correo@ejemplo.com")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? Email { get; set; }

        [CustomValidation(typeof(Validaciones), "ValidarCURP")]
        public string? Curp { get; set; }
        //[Required(ErrorMessage = "El RFC es requerido")]
        [CustomValidation(typeof(Validaciones), "ValidarRFC")]
        public string? Rfc { get; set; } */
        [Required]
        public string GuiaHouse { get; set; } = string.Empty;
        public string? Nombre { get; set; } = string.Empty;
        public string? Telefono { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Curp { get; set; } = string.Empty;
        public string? Rfc { get; set; } = string.Empty;
        public string? DocExtranjero { get; set; } = string.Empty;
    }

    public class DireccionesDestinatario
    {
        public int IdDestinatario { get; set; }
    }

    public class DireccionesDestinatarioInsert
    {
        public string Calle { get; set; }
        public string Colonia { get; set; }
        public string MunicipioAlcandia { get; set; }
        public string CodigoPostal { get; set; }
        public string NumeroExt { get; set; }
        public string? NumeroInt { get; set; }  // Opcional
        public string Ciudad { get; set; }
        public string ClaveEntidadFederativa { get; set; }
        public string? Localidad { get; set; }  // Opcional
        public int IdUsuarioAlta { get; set; }
    }

    public class DireccionesDestinatarioUpdate
    {
        public int IDDireccion { get; set; }
        public string? Calle { get; set; }
        public string? Colonia { get; set; }
        public string? MunicipioAlcandia { get; set; }
        public string? CodigoPostal { get; set; }
        public string? NumeroExt { get; set; }
        public string? NumeroInt { get; set; }
        public string? Ciudad { get; set; }
        public string? ClaveEntidadFederativa { get; set; }
        public string? Localidad { get; set; }
        public int IdUsuarioModifica { get; set; }
        public bool Activo { get; set; }
    }

    public class DireccionRelacion
    {
        public int IdDestinatario { get; set; }
        public int IDDireccion { get; set; }
    }

    public static class Validaciones
    {

        public static ValidationResult ValidarRFC(string rfc)
        {
            if (string.IsNullOrEmpty(rfc)) return ValidationResult.Success;

            rfc = rfc.ToUpperInvariant().Trim();

            if (rfc.Length != 12 && rfc.Length != 13)
                return new ValidationResult("RFC con longitud inválida");

            string pattern = @"^(((?!(([CcKk][Aa][CcKkGg][AaOo])|([Bb][Uu][Ee][YyIi])|([Kk][Oo](([Gg][Ee])|([Jj][Oo])))|([Cc][Oo](([Gg][Ee])|([Jj][AaEeIiOo])))|([QqCcKk][Uu][Ll][Oo])|((([Ff][Ee])|([Jj][Oo])|([Pp][Uu]))[Tt][Oo])|([Rr][Uu][Ii][Nn])|([Gg][Uu][Ee][Yy])|((([Pp][Uu])|([Rr][Aa]))[Tt][Aa])|([Pp][Ee](([Dd][Oo])|([Dd][Aa])|([Nn][Ee])))|([Mm](([Aa][Mm][OoEe])|([Ee][Aa][SsRr])|([Ii][Oo][Nn])|([Uu][Ll][Aa])|([Ee][Oo][Nn])|([Oo][Cc][Oo])))))[A-Za-zñÑ&][aeiouAEIOUxX]?[A-Za-zñÑ&]{2}(((([02468][048])|([13579][26]))0229)|(\d{2})((02((0[1-9])|1\d|2[0-8]))|((((0[13456789])|1[012]))((0[1-9])|((1|2)\d)|30))|(((0[13578])|(1[02]))31)))[a-zA-Z1-9]{2}[\dAa])|([Xx][AaEe][Xx]{2}010101000))$";

            var regexRFC = new Regex(pattern, RegexOptions.IgnoreCase);

            if (regexRFC.IsMatch(rfc))
                return ValidationResult.Success;

            return new ValidationResult("RFC con formato inválido");
        }

        public static ValidationResult ValidarCURP(string curp)
        {
            if (string.IsNullOrEmpty(curp)) return ValidationResult.Success;

            var regex = new Regex(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM]" +
                                  @"(?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)" +
                                  @"[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$");

            var match = regex.Match(curp);
            if (!match.Success)
                return new ValidationResult("CURP con formato inválido");

            string curp17 = match.Groups[1].Value;
            int digitoVerificador = CalcularDigitoVerificador(curp17);
            int digitoCURP = int.Parse(match.Groups[2].Value);

            if (digitoVerificador != digitoCURP)
                return new ValidationResult("CURP con dígito verificador inválido");

            return ValidationResult.Success;
        }

        private static int CalcularDigitoVerificador(string curp17)
        {
            const string diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
            int suma = 0;
            for (int i = 0; i < 17; i++)
            {
                int valor = diccionario.IndexOf(curp17[i]);
                suma += valor * (18 - i);
            }
            int digito = 10 - (suma % 10);
            return digito == 10 ? 0 : digito;
        }
    }
}
