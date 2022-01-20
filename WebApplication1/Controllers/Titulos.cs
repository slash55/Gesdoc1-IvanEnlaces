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
    public class TitulosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TitulosController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdTitulo,Titulo,Descripcion,Status
                            from
                            dbo.CatTitulos
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


            List<Titulo> catalogo = new List<Titulo>();

            foreach (DataRow row in table.Rows)
            {
                Titulo titulo = new Titulo();

                titulo.id = Convert.ToInt32(row["IdTitulo"]);
                titulo.titulo = row["Titulo"].ToString().Trim();
                titulo.descripcion = row["Descripcion"].ToString().Trim();
                titulo.estado = Convert.ToBoolean(row["Status"]);
                catalogo.Add(titulo);

            }

            ListaTitulo Resultado = new ListaTitulo();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;
            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(Titulo Datos)
        {
            string query = @"
                           insert into CatTitulos
                                    (Titulo,Descripcion,Status,iFecInserta,iUsrInserta,iFecModifica,iUsrModifica)
                            values  (@titulo,@descripcion,1,getdate(),'Pruebas','','')
                            
                            SELECT @@IDENTITY AS Ultimo ";



            string Respuesta = @" select IdTitulo,Titulo,Descripcion,Status
                            from
                            dbo.CatTitulos
                            Where IdTitulo = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@titulo", Datos.titulo);
                    myCommand.Parameters.AddWithValue("@descripcion", Datos.descripcion);
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
            Titulo titulo = new Titulo();
            foreach (DataRow row in table2.Rows)
            {
                titulo.id = Convert.ToInt32(row["IdTitulo"]);
                titulo.titulo = row["Titulo"].ToString().Trim();
                titulo.descripcion = row["Descripcion"].ToString().Trim();
                titulo.estado = Convert.ToBoolean(row["Status"]);
                
            }
            RespuestaTitulo Resultado = new RespuestaTitulo();
            Resultado.session = 1;
            Resultado.catalogo = titulo;

            return new JsonResult(Resultado);

        }

        [HttpPut]
        public JsonResult Put(Titulo Datos)
        {
            string query = @"Update CatTitulos
                    Set
                        Titulo = @titulo,
                        Descripcion = @descripcion,
                        Status = @estado
                    
                    Where IdTitulo = @id
                    
                   select IdTitulo,Titulo,Descripcion,Status
                            from
                            dbo.CatTitulos
                            Where IdTitulo = @id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", Datos.id);
                    myCommand.Parameters.AddWithValue("@titulo", Datos.titulo);
                    myCommand.Parameters.AddWithValue("@descripcion", Datos.descripcion);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estado);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            Titulo titulo = new Titulo();
            foreach (DataRow row in table.Rows)
            {
                titulo.id = Convert.ToInt32(row["IdTitulo"]);                
                titulo.titulo = row["Titulo"].ToString().Trim();
                titulo.descripcion = row["Descripcion"].ToString().Trim();
                titulo.estado = Convert.ToBoolean(row["Status"]);
                


            }
            RespuestaTitulo Resultado = new RespuestaTitulo();
            Resultado.session = 1;
            Resultado.catalogo = titulo;
            return new JsonResult(Resultado);
        }
    }
}
