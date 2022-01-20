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
    public class EnlacesDependencia : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EnlacesDependencia(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select IdCatDependencia,Nombre,Siglas,Orden,rTipo
                            from CatDependencias 
                            Where Status = 'true' "; 

            string query2 = @"Select  IdCatSubDependencia,Nombre,Siglas,Orden 
                              From CatSubDependencia 
                              Where IdCatDependencia =@Id and Status = 1 "; 

            string query3 = @"Select IdEnlace,Cuenta,Nombre,Correo,Telefono,Orden,IdCatDependencia,IdCatSubDependencia,Status
                              From CatEnlace
                              Where IdCatDependencia =@Id and IdCatSubDependencia =@IdSub and Status = 1 ";

    
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

            List<Dependencia> listaDependencias = new List<Dependencia>();
            foreach (DataRow dep in table.Rows)
            {
                Dependencia dependencia = new Dependencia();

                dependencia.id = Convert.ToInt32(dep["IdCatDependencia"]);
                dependencia.nombre = dep["Nombre"].ToString().Trim();
                dependencia.siglas = dep["Siglas"].ToString().Trim();
                dependencia.orden = Convert.ToInt32(dep["Orden"]);
                dependencia.ambito = Convert.ToInt32(dep["rTipo"]);
                // List<Enlaces> Enlaces = new List<Enlaces>();


                List<subDependencia> SubDependencias = new List<subDependencia>();
                
                DataTable table2 = new DataTable();
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query2, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", dependencia.id);
                        myReader = myCommand.ExecuteReader();
                        table2.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                
                foreach (DataRow sub in table2.Rows)
                {
                    subDependencia SubDep = new subDependencia();

                    SubDep.id = Convert.ToInt32(sub["IdCatSubDependencia"]);
                    SubDep.nombre = sub["Nombre"].ToString().Trim();
                    SubDep.siglas = sub["Siglas"].ToString().Trim();
                    SubDep.orden = Convert.ToInt32(sub["Orden"]);
                    SubDependencias.Add(SubDep);

                    List<Enlaces> EnlacesSub = new List<Enlaces>();
                
                    DataTable table4 = new DataTable();
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query3, myCon))
                        {
                            myCommand.Parameters.AddWithValue("@Id", dependencia.id);
                            myCommand.Parameters.AddWithValue("@IdSub", SubDep.id);
                            myReader = myCommand.ExecuteReader();
                            table4.Load(myReader);
                            myReader.Close();
                            myCon.Close();
                        }
                    }
                    
                    foreach (DataRow enlcesub in table4.Rows)
                    {
                        Enlaces enlacesub = new Enlaces();
                
                        enlacesub.id = Convert.ToInt32(enlcesub["IdEnlace"]);
                        enlacesub.nombre = enlcesub["Nombre"].ToString().Trim();
                        enlacesub.cuenta = enlcesub["Cuenta"].ToString().Trim();
                        enlacesub.correo = enlcesub["Correo"].ToString().Trim();
                        enlacesub.telefono = enlcesub["Telefono"].ToString().Trim();
                        enlacesub.orden = Convert.ToInt32(enlcesub["Orden"]);
                        EnlacesSub.Add(enlacesub);
                        
                    }
                    SubDep.enlaces = EnlacesSub;
                    
                }
                dependencia.subDependencias = SubDependencias;

                List<Enlaces> Enlaces = new List<Enlaces>();
                
                DataTable table3 = new DataTable();
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query3, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", dependencia.id);
                        myCommand.Parameters.AddWithValue("@IdSub", 0);
                        myReader = myCommand.ExecuteReader();
                        table3.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }
                
                foreach (DataRow enlce in table3.Rows)
                {
                    Enlaces enlace = new Enlaces();
            
                    enlace.id = Convert.ToInt32(enlce["IdEnlace"]);
                    enlace.nombre = enlce["Nombre"].ToString().Trim();
                    enlace.cuenta = enlce["Cuenta"].ToString().Trim();
                    enlace.correo = enlce["Correo"].ToString().Trim();
                    enlace.telefono = enlce["Telefono"].ToString().Trim();
                    enlace.orden = Convert.ToInt32(enlce["Orden"]);
                    Enlaces.Add(enlace);
                    
                }
                dependencia.enlaces = Enlaces;


                listaDependencias.Add(dependencia);
            }
            Cabecera cabecera = new Cabecera();
            cabecera.catalogo = listaDependencias;
            cabecera.session = 1;
            return new JsonResult(cabecera);

        }// Fin GET

    }//Fin Controlador
}
