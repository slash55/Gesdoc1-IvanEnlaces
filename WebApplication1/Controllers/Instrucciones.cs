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
    public class InstruccionesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public InstruccionesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdInstruccion,Descripcion,Status
                            from
                            dbo.CatInstrucciones
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


            List<Instrucciones> catalogo = new List<Instrucciones>();

            foreach (DataRow row in table.Rows)
            {
                Instrucciones instruccion = new Instrucciones();

                instruccion.id = Convert.ToInt32(row["IdInstruccion"]);
                instruccion.descripcion = row["Descripcion"].ToString().Trim();
                instruccion.estadoVigente = Convert.ToBoolean(row["Status"]);
                catalogo.Add(instruccion);

            }

            ListaInstrucciones Resultado = new ListaInstrucciones();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;
            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(Instrucciones Datos)
        {
            string query = @"
                           insert into CatInstrucciones
                                    (Descripcion,Status,iFecInserta,iUsrInserta,iFecModifica,iUsrModifica)
                            values  (@descripcion,@estadoVigente,getdate(),'Pruebas','','')
                            
                            SELECT @@IDENTITY AS Ultimo ";



            string Respuesta = @" select IdInstruccion,Descripcion,Status
                            from
                            dbo.CatInstrucciones
                            Where IdInstruccion = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);
                    myCommand.Parameters.AddWithValue("@estadoVigente", Datos.estadoVigente);
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
            Instrucciones instruccion = new Instrucciones();
            foreach (DataRow row in table2.Rows)
            {
                instruccion.id = Convert.ToInt32(row["IdInstruccion"]);
                instruccion.estadoVigente = Convert.ToBoolean(row["Status"]);
                instruccion.descripcion = row["Descripcion"].ToString().Trim();
            }
            RespuestaInstrucciones Resultado = new RespuestaInstrucciones();
            Resultado.session = 1;
            Resultado.catalogo = instruccion;

            return new JsonResult(Resultado);

        }

        [HttpPut]
        public JsonResult Put(Instrucciones Datos)
        {
            string query = @"Update CatInstrucciones
                    Set
                        Descripcion = @descripcion,
                        Status = @estadoVigente
                    
                    Where IdInstruccion = @id
                    
                   select IdInstruccion,Descripcion,Status
                            from
                            dbo.CatInstrucciones
                            Where IdInstruccion = @id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", Datos.id);
                    myCommand.Parameters.AddWithValue("@descripcion", Datos.descripcion);
                    myCommand.Parameters.AddWithValue("@estadoVigente", Datos.estadoVigente);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            Instrucciones instruccion = new Instrucciones();
            foreach (DataRow row in table.Rows)
            {
                instruccion.id = Convert.ToInt32(row["IdInstruccion"]);
                instruccion.estadoVigente = Convert.ToBoolean(row["Status"]);
                instruccion.descripcion = row["Descripcion"].ToString().Trim();

            }
            RespuestaInstrucciones Resultado = new RespuestaInstrucciones();
            Resultado.session = 1;
            Resultado.catalogo = instruccion;
            return new JsonResult(Resultado);
        }
    }
}
