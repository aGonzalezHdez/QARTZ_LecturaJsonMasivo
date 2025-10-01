using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion
{
    public class EnvioAlertaNotiRepository : IEnvioAlertaNotiRepository
    {
        public string sConexion { get; set; }
        private readonly IConfiguration _configuration;

        public EnvioAlertaNotiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> CargarRequisitosAlertasNoti()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using SqlConnection cn = new(sConexion);
                using SqlCommand cmd = new("NET_LOAD_REQUISITOSALERTASNOTI", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                using SqlDataReader reader = cmd.ExecuteReader();
                comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return comboList;
        }

        public List<DropDownListDatos> CargarSubRequisitoAlertasNoti(int IdRequisitoAlerta)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using SqlConnection cn = new(sConexion);
                using SqlCommand cmd = new("NET_LOAD_SUBREQUISITOSALERTASNOTI", cn);
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IdRequisitoAlerta", SqlDbType.Int).Value = IdRequisitoAlerta;
                using SqlDataReader reader = cmd.ExecuteReader();
                comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return comboList;
        }
    }
}
