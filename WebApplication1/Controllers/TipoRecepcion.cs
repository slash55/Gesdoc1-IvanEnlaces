
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
    public class TipoRecepcionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TipoRecepcionController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdTipoRecepcion, Descripcion
                            from
                            dbo.CatTipoRecepcion
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

            
            List<TipoRecepcion> catalogo = new List<TipoRecepcion>();

            foreach (DataRow row in table.Rows)
            {
                TipoRecepcion TiposDoc = new TipoRecepcion();
              
                TiposDoc.id = Convert.ToInt32(row["IdTipoRecepcion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
               
                catalogo.Add(TiposDoc);
               
            }

            TiposRecepcion Resultado = new TiposRecepcion();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;


            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(TipoRecepcion Datos)
        {
            string query = @"
                           insert into CatTipoRecepcion
                           (Descripcion,Status, iFecInserta, iUsrInserta, iFecModifica, iUsrModifica)
                            values (@Descripcion, 'true', getdate(), 'ALVARO', '', '')
                            
                            SELECT @@IDENTITY AS Ultimo ";

           

            string Respuesta = @" select IdTipoRecepcion, Descripcion
                            from
                            dbo.CatTipoRecepcion
                            Where Status <> 'false' 
                            And IdTipoRecepcion = @Id ";

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

 
            TipoRecepcion TiposDoc = new TipoRecepcion();
            foreach (DataRow row in table2.Rows)
            {
                
                TiposDoc.id = Convert.ToInt32(row["IdTipoRecepcion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                
                
            }

            TiposRespuestaRecepcion Resultado = new TiposRespuestaRecepcion();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;
            
            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(TipoRecepcion Datos)
        {
            string query = @"Update CatTipoRecepcion
                    Set
                    Descripcion = @Descripcion
                    Where IdTipoRecepcion = @Id
                    
                    select IdTipoRecepcion, Descripcion 
                    from
                    dbo.CatTipoRecepcion
                    Where Status <> 'false' 
                    And IdTipoRecepcion = @Id ";

            if (Datos.eliminar) {
                query = @"Update CatTipoRecepcion
                    Set Status = 'false'
                    Where IdTipoRecepcion = @Id
          
                    select IdTipoRecepcion, Descripcion
                    from
                    dbo.CatTipoRecepcion
                    Where Status <> 'false' 
                    And IdTipoRecepcion = @Id ";

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

                    if (!Datos.eliminar) {
                        myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);
                       
           
                    }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            TipoRecepcion TiposDoc = new TipoRecepcion();
            foreach (DataRow row in table.Rows)
            {
                TiposDoc.id = Convert.ToInt32(row["IdTipoRecepcion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
               
            }

            TiposRespuestaRecepcion Resultado = new TiposRespuestaRecepcion();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

    }
}
