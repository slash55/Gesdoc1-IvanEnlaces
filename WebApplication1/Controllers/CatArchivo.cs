
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
    public class CatArchivoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CatArchivoController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdCatArchivo, Descripcion
                            from
                            dbo.CatArchivo
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


            List<TipoCatArchivo> catalogo = new List<TipoCatArchivo>();

            foreach (DataRow row in table.Rows)
            {
                TipoCatArchivo TiposDoc = new TipoCatArchivo();

                TiposDoc.id = Convert.ToInt32(row["IdCatArchivo"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();

                catalogo.Add(TiposDoc);

            }

            TiposCatArchivo Resultado = new TiposCatArchivo();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;


            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(TipoCatArchivo Datos)
        {
            string query = @"
                           insert into CatArchivo
                           (Descripcion,Status, iFecInserta, iUsrInserta, iFecModifica, iUsrModifica)
                            values (@Descripcion, 'true', getdate(), 'ALVARO', '', '')
                            
                            SELECT @@IDENTITY AS Ultimo ";



            string Respuesta = @" select IdCatArchivo, Descripcion
                            from
                            dbo.CatArchivo
                            Where Status <> 'false' 
                            And IdCatArchivo = @Id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);

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


            TipoCatArchivo TiposDoc = new TipoCatArchivo();
            foreach (DataRow row in table2.Rows)
            {

                TiposDoc.id = Convert.ToInt32(row["IdCatArchivo"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();


            }

            TiposRespuestaCatArchivo Resultado = new TiposRespuestaCatArchivo();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(TipoCatArchivo Datos)
        {
            string query = @"Update CatArchivo
                    Set
                    Descripcion = @Descripcion
                    Where IdCatArchivo = @Id
                    
                    select IdCatArchivo, Descripcion 
                    from
                    dbo.CatArchivo
                    Where Status <> 'false' 
                    And IdCatArchivo = @Id ";

            if (Datos.eliminar)
            {
                query = @"Update CatArchivo
                    Set Status = 'false'
                    Where IdCatArchivo = @Id
          
                    select IdCatArchivo, Descripcion
                    from
                    dbo.CatArchivo
                    Where Status <> 'false' 
                    And IdCatArchivo = @Id ";

            }

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", Datos.id);

                    if (!Datos.eliminar)
                    {
                        myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);


                    }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            TipoCatArchivo TiposDoc = new TipoCatArchivo();
            foreach (DataRow row in table.Rows)
            {
                TiposDoc.id = Convert.ToInt32(row["IdCatArchivo"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();

            }

            TiposRespuestaCatArchivo Resultado = new TiposRespuestaCatArchivo();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

    }
}