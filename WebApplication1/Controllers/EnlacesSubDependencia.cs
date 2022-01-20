using Microsoft.AspNetCore.Http;
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
    [Route("Catalogos/Enlaces/[controller]")]
    [ApiController]
    public class EnlacesSubDependencia : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EnlacesSubDependencia(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"Select * From CatEnlace Where IdCatSubDependencia =@id and Status <> 0 ";


            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<Enlaces> listaEnlaces = new List<Enlaces>();
            foreach (DataRow row in table.Rows)
            {
                Enlaces enlaces = new Enlaces();

                enlaces.id = Convert.ToInt32(row["IdEnlace"]);
                enlaces.nombre = row["Nombre"].ToString().Trim();
                enlaces.cuenta = row["Cuenta"].ToString().Trim();
                enlaces.orden = Convert.ToInt32(row["Orden"]);


                listaEnlaces.Add(enlaces);
            }
            Cabecera cabecera = new Cabecera();
            cabecera.catEnlaces = listaEnlaces;
            cabecera.session = 1;
            return new JsonResult(cabecera);

        }// Fin GET

    }//Fin Controlador
}