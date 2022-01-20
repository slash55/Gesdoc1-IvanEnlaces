using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GesDoc21.Models;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Controllers
{
    [Route("Catalogos/[controller]")]
    [ApiController]
    public class SubDependenciasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public SubDependenciasController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdCatSubDependencia,IdCatDependencia,Nombre,Siglas,Orden,Status
                            from
                            dbo.CatSubDependencia
                            Where Status =1";

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


            List<SubDependencias> catalogo = new List<SubDependencias>();

            foreach (DataRow row in table.Rows)
            {
                SubDependencias subdependencia = new SubDependencias();

                subdependencia.id = Convert.ToInt32(row["IdCatSubDependencia"]);
                subdependencia.idDependencia = Convert.ToInt32(row["IdCatDependencia"]);
                subdependencia.nombre = row["Nombre"].ToString().Trim();
                subdependencia.siglas = row["Siglas"].ToString().Trim();
                subdependencia.orden = Convert.ToInt32(row["Orden"]);
                subdependencia.estado = row["Status"].ToString().Trim();
                //dependencia.estado = Convert.ToString(row["Status"]);

                catalogo.Add(subdependencia);

            }

            ListaSubDependencias Resultado = new ListaSubDependencias();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;
            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(SubDependencias Datos)
        {
            string query = @"
                           insert into CatSubDependencia
                                    (Nombre,Siglas,Orden,Status,iFecInserta,iUsrInserta,iFecModifica,iUsrModifica)
                            values  (@nombre,@siglas,@orden,1,getdate(),'Pruebas','','')
                            
                            SELECT @@IDENTITY AS Ultimo ";



            string Respuesta = @" select IdCatDependencias,Nombre,Siglas,Orden,Status
                            from
                            dbo.CatDependencia
                            Where IdCatDependencia = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nombre", Datos.nombre);
                    myCommand.Parameters.AddWithValue("@siglas", Datos.siglas);
                    myCommand.Parameters.AddWithValue("@orden", Datos.orden);
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
                    myCommand.Parameters.AddWithValue("@Id", Id);

                    myReader = myCommand.ExecuteReader();
                    table2.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            Dependencias dependencia = new Dependencias();
            foreach (DataRow row in table2.Rows)
            {
                dependencia.id = Convert.ToInt32(row["IdCatDependencia"]);
                dependencia.nombre = row["Nombre"].ToString().Trim();
                dependencia.siglas = row["Siglas"].ToString().Trim();
                dependencia.orden = Convert.ToInt32(row["Orden"]);
                dependencia.estado = row["Status"].ToString().Trim();
                
            }
            RespuestaDependencias Resultado = new RespuestaDependencias();
            Resultado.session = 1;
            Resultado.catalogo = dependencia;

            return new JsonResult(Resultado);

        }

        [HttpPut]
        public JsonResult Put(SubDependencias Datos)
        {
            string query = @"Update CatDependencias
                    Set
                        Nombre = @nombre,
                        Siglas = @siglas,
                        Orden  = @orden
                        Status = @estado
                    
                    Where IdCatDependencia = @id
                    
                  select IdCatDependencias,Nombre,Siglas,Orden,Status
                            from
                            dbo.CatDependencia
                            Where IdCatDependencia = @id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@idDependencia",Datos.idDependencia);
                    myCommand.Parameters.AddWithValue("@nombre", Datos.nombre);
                    myCommand.Parameters.AddWithValue("@siglas", Datos.siglas);
                    myCommand.Parameters.AddWithValue("@orden", Datos.orden);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estado);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            SubDependencias subdependencia = new SubDependencias();
            foreach (DataRow row in table.Rows)
            {
                subdependencia.id = Convert.ToInt32(row["IdCatSubDependencia"]);
                subdependencia.idDependencia = Convert.ToInt32(row["IdCatDependencia"]);
                subdependencia.nombre = row["Nombre"].ToString().Trim();
                subdependencia.siglas = row["Siglas"].ToString().Trim();
                subdependencia.orden = Convert.ToInt32(row["Orden"]);
                subdependencia.estado = row["Status"].ToString().Trim();
            }
            RespuestaSubDependencias Resultado = new RespuestaSubDependencias();
            Resultado.session = 1;
            Resultado.catalogo = subdependencia;
            return new JsonResult(Resultado);
        }
    }
}
