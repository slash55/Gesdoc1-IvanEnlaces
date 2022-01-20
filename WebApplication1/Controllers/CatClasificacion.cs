
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
    public class CatClasificacionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public CatClasificacionController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdClasificacion, Descripcion, Iniciales
                            from
                            dbo.CatClasificacion
                            Where Status<>'false' order by IdClasificacion";

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


            List<TipoClasificacion> catalogo = new List<TipoClasificacion>();

            foreach (DataRow row in table.Rows)
            {
                TipoClasificacion TiposDoc = new TipoClasificacion();

                TiposDoc.id = Convert.ToInt32(row["IdClasificacion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.iniciales = row["Iniciales"].ToString().Trim();
           
                catalogo.Add(TiposDoc);

            }

            TiposClasificacion Resultado = new TiposClasificacion();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;

            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(TipoClasificacion Datos)
        {
            string query = @"
                           insert into CatClasificacion
                           (Descripcion,Iniciales,Status, iFecInserta, iUsrInserta, iFecModifica, iUsrModifica)
                            values (@Descripcion, @Iniciales, 'true', getdate(), 'ALVARO', '', '')
                            
                            SELECT @@IDENTITY AS Ultimo ";



            string Respuesta = @" select IdClasificacion, Descripcion, Iniciales
                            from
                            dbo.CatClasificacion
                            Where Status <> 'false' 
                            And IdClasificacion = @Id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Descripcion", Datos.descripcion);
                    myCommand.Parameters.AddWithValue("@Iniciales", Datos.iniciales);
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


            TipoClasificacion TiposDoc = new TipoClasificacion();
            foreach (DataRow row in table2.Rows)
            {

                TiposDoc.id = Convert.ToInt32(row["IdClasificacion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.iniciales = row["Iniciales"].ToString().Trim();
               }

            TiposRespuestaClasificacion Resultado = new TiposRespuestaClasificacion();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(TipoClasificacion Datos)
        {
            string query = @"Update CatClasificacion
                    Set
                    Descripcion = @Descripcion,
                    Iniciales = @Iniciales
                    Where IdClasificacion = @Id
                    
                    select IdClasificacion, Descripcion, Iniciales 
                    from
                    dbo.CatClasificacion
                    Where Status <> 'false' 
                    And IdClasificacion = @Id ";

            if (Datos.eliminar)
            {
                query = @"Update CatClasificacion
                    Set Status = 'false'
                    Where IdClasificacion = @Id
          
                    select IdClasificacion, Descripcion, Iniciales 
                    from
                    dbo.CatClasificacion
                    Where Status <> 'false' 
                    And IdClasificacion = @Id ";

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
                        myCommand.Parameters.AddWithValue("@Iniciales", Datos.iniciales);
                    }
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            TipoClasificacion TiposDoc = new TipoClasificacion();
            foreach (DataRow row in table.Rows)
            {
                TiposDoc.id = Convert.ToInt32(row["IdClasificacion"]);
                TiposDoc.descripcion = row["Descripcion"].ToString().Trim();
                TiposDoc.iniciales = row["Iniciales"].ToString().Trim();
             
            }

            TiposRespuestaClasificacion Resultado = new TiposRespuestaClasificacion();
            Resultado.session = 1;
            Resultado.catalogo = TiposDoc;

            return new JsonResult(Resultado);
        }

    }
}
