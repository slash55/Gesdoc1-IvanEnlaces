
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GesDoc21.Models;
using Microsoft.AspNetCore.Hosting;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("Catalogos/[controller]")]
    [ApiController]
    public class Ejemplo4nController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public Ejemplo4nController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query_n2 = @"
                            select IdCatDependencia,Nombre,Siglas,Orden,rtipo from dbo.CatDependencias where Status<>0 and IdCatDependencia=5
                            ";
            string query_n3 = @"
                            select top 2 IdCatSubDependencia,IdCatDependencia,Nombre,Siglas,Orden
                            from CatSubDependencia
                            where Status<>0 and IdCatDependencia=@Id
                            ";
            string query_n4 = @"
                            select top 3 IdCatSubDependencia,IdCatDependencia,Nombre
                            from CatEnlace
                            where Status<>0 and IdCatSubDependencia=@Id
                            ";

            DataTable table_n2 = new DataTable();
            DataTable table_n3 = new DataTable();
            DataTable table_n4 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query_n2, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", 1);
                    myReader = myCommand.ExecuteReader();
                    table_n2.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }



            List<nivel_2> lista_n2 = new List<nivel_2>();

            foreach (DataRow row2 in table_n2.Rows)
            {

                nivel_2 lnivel_2 = new nivel_2();
                lnivel_2.id = Convert.ToInt32(row2["IdCatDependencia"]);
                lnivel_2.nombre = row2["Nombre"].ToString().Trim();
                lnivel_2.siglas = row2["Siglas"].ToString().Trim();
                lnivel_2.orden = Convert.ToInt32(row2["Orden"]);
                lnivel_2.ambito = Convert.ToInt32(row2["rtipo"]);

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))

                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query_n3, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", lnivel_2.id);
                        myReader = myCommand.ExecuteReader();
                        table_n3.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                List<nivel_3> lista_n3 = new List<nivel_3>();
                foreach (DataRow row3 in table_n3.Rows)
                {
                    nivel_3 lnivel_3 = new nivel_3
                    {
                        id = Convert.ToInt32(row3["IdCatSubDependencia"]),
                        nombre = row3["Nombre"].ToString().Trim(),
                        siglas = row3["Siglas"].ToString().Trim(),
                        orden = Convert.ToInt32(row3["Orden"])
                    };

                    lista_n3.Add(lnivel_3);


                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))

                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query_n4, myCon))
                        {
                            myCommand.Parameters.AddWithValue("@Id", lnivel_3.id);
                            myReader = myCommand.ExecuteReader();
                            table_n4.Load(myReader);
                            myReader.Close();
                            myCon.Close();
                        }
                    }

                    List<nivel_4> lista_n4 = new List<nivel_4>();
                    foreach (DataRow row4 in table_n4.Rows)
                    {
                        nivel_4 lnivel_4 = new nivel_4
                        {
                            id = Convert.ToInt32(row4["IdCatSubDependencia"]),
                            nombre = row4["Nombre"].ToString().Trim(),

                        };

                        lista_n4.Add(lnivel_4);
                        lnivel_3.enlaces = lista_n4;
                    }

                }

                lnivel_2.Subdependencias = lista_n3;

                lista_n2.Add(lnivel_2);

               

            }

            nivel_1 lista_n1 = new nivel_1();
            lista_n1.session = 1;
            lista_n1.catalogo = lista_n2;
            return new JsonResult(lista_n1);
        }
    }
}
