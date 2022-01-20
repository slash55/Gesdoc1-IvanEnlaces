using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GesDoc21.Models;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace GesDoc21.Controllers
{
    [Route("Catalogos/[controller]")]
    [ApiController]
    public class DiasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public DiasController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select IdDias,Fecha,Status
                            from
                            dbo.CatDias
                            Where Status <> 'false' ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<Dias> catalogo = new List<Dias>();

            foreach (DataRow row in table.Rows)
            {
                Dias days = new Dias();

                days.id = Convert.ToInt32(row["IdDias"]);
                days.fecha=Convert.ToDateTime(row["Fecha"]).ToString("O", CultureInfo.CreateSpecificCulture("en-US"));
                days.estado = Convert.ToBoolean(row["Status"]);
                //days.fecha = row["Fecha"].ToString().Trim();

                catalogo.Add(days);
            }

            ListaDias Resultado = new ListaDias();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;
            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(Dias Datos)
        {
            string query = @"
                           insert into CatDias
                                    (Fecha,Estado,Status,iFecInserta,iUsrInserta,iFecModifica,iUsrModifica)
                            values  (@fecha,1,@estado,getdate(),'Pruebas','','')
                            
                            SELECT @@IDENTITY AS Ultimo ";

            string Respuesta = @"select IdDias,Fecha,Status
                            from
                            dbo.CatDias
                            Where Status<> 'false' 
                            And IdDias = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@fecha", Datos.fecha);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estado);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            int Id = Convert.ToInt32(table.Rows[0]["Ultimo"]);
            DataTable table2 = new DataTable();
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(Respuesta, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", Id);

                    myReader = myCommand.ExecuteReader();
                    table2.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            Dias dias = new Dias();
            foreach (DataRow row in table2.Rows)
            {
                dias.id = Convert.ToInt32(row["IdDias"]);
                dias.fecha = Convert.ToDateTime(row["Fecha"]).ToString("O", CultureInfo.CreateSpecificCulture("en-US"));
                dias.estado = Convert.ToBoolean(row["Status"]);
                //dias.fecha = row["Fecha"].ToString().Trim();
            }
            RespuestaDias Resultado = new RespuestaDias();
            Resultado.session = 1;
            Resultado.catalogo = dias;

            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(Dias Datos)
        {
            string query = @"Update CatDias
                    Set
                        Fecha = @fecha,
                        Status = @estado 
                    
                    Where IdDias = @id
                    
                   select IdDias,Fecha,Status
                            from
                            dbo.CatDias
                            Where IdDias = @id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", Datos.id);
                    myCommand.Parameters.AddWithValue("@fecha", Datos.fecha);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estado);
                 
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            Dias dias = new Dias();
            foreach (DataRow row in table.Rows)
            {
                dias.id = Convert.ToInt32(row["IdDias"]);
                dias.estado = Convert.ToBoolean(row["Status"]);
                dias.fecha = Convert.ToDateTime(row["Fecha"]).ToString("O",CultureInfo.CreateSpecificCulture("en-US"));
              //dias.fecha = row["Fecha"].ToString().Trim();

            }
            RespuestaDias Resultado = new RespuestaDias();
            Resultado.session = 1;
            Resultado.catalogo = dias;
            return new JsonResult(Resultado);
        }
    }
}
