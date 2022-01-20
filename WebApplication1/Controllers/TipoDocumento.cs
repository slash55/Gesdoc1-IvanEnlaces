
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
    public class TipoDocumentoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TipoDocumentoController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdTipoDoc, Descripcion, Siglas, Estado, Turno as Turnar
                            from
                            dbo.CatTipoDocumento
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

            
            List<TipoDocumento> catalogo = new List<TipoDocumento>();

            foreach (DataRow row in table.Rows)
            {
                TipoDocumento TiposDoc = new TipoDocumento();
              
                TiposDoc.id = Convert.ToInt32(row["IdTipoDoc"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.siglas = row["Siglas"].ToString().Trim();
                TiposDoc.estadoVigente = Convert.ToBoolean(row["Estado"]);
                TiposDoc.esTurnada = Convert.ToBoolean(row["Turnar"]);
                catalogo.Add(TiposDoc);
               
            }

            Tipos Resultado = new Tipos();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;


            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(TipoDocumento Datos)
        {
            string query = @"
                           insert into CatTipoDocumento
                           (Descripcion,Siglas,Estado,Turno, Status, iFecInserta, iUsrInserta, iFecModifica, iUsrModifica)
                            values (@Descripcion,@Siglas,@Estado,@Turno, 'true', '16/11/2021', 'PRUEBAS', '', '')
                            
                            SELECT @@IDENTITY AS Ultimo ";

           

            string Respuesta = @" select IdTipoDoc, Descripcion, Siglas, Estado, Turno as Turnar
                            from
                            dbo.CatTipoDocumento
                            Where Status <> 'false' 
                            And IdTipoDoc = @Id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);
                    myCommand.Parameters.AddWithValue("@Siglas", Datos.siglas);
                    myCommand.Parameters.AddWithValue("@Estado", Datos.estadoVigente);
                    myCommand.Parameters.AddWithValue("@Turno",Datos.esTurnada);
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

 
            TipoDocumento TiposDoc = new TipoDocumento();
            foreach (DataRow row in table2.Rows)
            {
                
                TiposDoc.id = Convert.ToInt32(row["IdTipoDoc"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.siglas = row["Siglas"].ToString().Trim();
                TiposDoc.estadoVigente = Convert.ToBoolean(row["Estado"]);
                TiposDoc.esTurnada = Convert.ToBoolean(row["Turnar"]);
               
            }

            TiposRespuesta Resultado = new TiposRespuesta();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;
            
            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(TipoDocumento Datos)
        {
            string query = @"Update CatTipoDocumento
                    Set
                    Descripcion = @Descripcion,
                    Siglas = @Siglas,
                    Estado = @Estado,
                    Turno = @Turno
                    Where IdTipoDoc = @Id
                    
                    select IdTipoDoc, Descripcion, Siglas, Estado, Turno as Turnar
                    from
                    dbo.CatTipoDocumento
                    Where Status <> 'false' 
                    And IdTipoDoc = @Id ";

            if (Datos.eliminar) {
                query = @"Update CatTipoDocumento
                    Set Status = 'false'
                    Where IdTipoDoc = @Id
          
                    select IdTipoDoc, Descripcion, Siglas, Estado, Turno as Turnar
                    from
                    dbo.CatTipoDocumento
                    Where Status <> 'false' 
                    And IdTipoDoc = @Id ";

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
                        myCommand.Parameters.AddWithValue("@Siglas", Datos.siglas);
                        myCommand.Parameters.AddWithValue("@Estado", Datos.estadoVigente);
                        myCommand.Parameters.AddWithValue("@Turno", Datos.esTurnada);
                    }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            TipoDocumento TiposDoc = new TipoDocumento();
            foreach (DataRow row in table.Rows)
            {
               
                TiposDoc.id = Convert.ToInt32(row["IdTipoDoc"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.siglas = row["Siglas"].ToString().Trim();
                TiposDoc.estadoVigente = Convert.ToBoolean(row["Estado"]);
                TiposDoc.esTurnada = Convert.ToBoolean(row["Turnar"]);
             
            }

            TiposRespuesta Resultado = new TiposRespuesta();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

    }
}
