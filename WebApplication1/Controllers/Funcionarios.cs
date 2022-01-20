using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using GesDoc21.Models;
using Microsoft.AspNetCore.Hosting;

namespace GesDoc21.Controllers
{
    [Route("Catalogos/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public FuncionariosController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @" select IdFuncionario,Nombre,Cargo,Iniciales,Estado,Recibe,Dirigido,Status
                            from
                            dbo.CatFuncionarios
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
                        
            List<Funcionarios> catalogo = new List<Funcionarios>();

            foreach (DataRow row in table.Rows)
            {
                Funcionarios titular = new Funcionarios();
              
                titular.id = Convert.ToInt32(row["IdFuncionario"]);
                titular.nombre = row["Nombre"].ToString().Trim();
                titular.cargo = row["Cargo"].ToString().Trim();
                titular.siglas= row["Iniciales"].ToString().Trim();
                titular.recibe = Convert.ToBoolean(row["Recibe"]);
                titular.dirigido = Convert.ToBoolean(row["Dirigido"]);
                titular.estadoVigente = Convert.ToBoolean(row["Estado"]);
                catalogo.Add(titular);
            }

            ListaFuncionarios Resultado = new ListaFuncionarios();
            Resultado.session = 1;
            Resultado.catalogo = catalogo;


            return new JsonResult(Resultado);
        }

        [HttpPost]
        public JsonResult Post(Funcionarios Datos)
        {
            string query = @"
                           insert into CatFuncionarios
                                    (Nombre,Cargo,Iniciales,Estado,Recibe,Dirigido,Status,iFecInserta,iUsrInserta,iFecModifica,iUsrModifica)
                            values  (@nombre,@cargo,@siglas,1,@recibe,@dirigido,'true',getdate(),'PRUEBAS','','')
                            
                            SELECT @@IDENTITY AS Ultimo ";

            string Respuesta = @"select IdFuncionario,Nombre,Estado,Cargo,Iniciales,Recibe,Dirigido,Status
                            from
                            dbo.CatFuncionarios
                            Where Status <> 'false' 
                            And IdFuncionario = @id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nombre", Datos.nombre);
                    myCommand.Parameters.AddWithValue("@cargo", Datos.cargo);
                    myCommand.Parameters.AddWithValue("@siglas", Datos.siglas);
                    myCommand.Parameters.AddWithValue("@recibe", Datos.recibe);
                    myCommand.Parameters.AddWithValue("@dirigido", Datos.dirigido);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estadoVigente);
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
                    myCommand.Parameters.AddWithValue("@id", Id);
         
                    myReader = myCommand.ExecuteReader();
                    table2.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

 
            Funcionarios titular = new Funcionarios();
            foreach (DataRow row in table2.Rows)
            {
                titular.id = Convert.ToInt32(row["IdFuncionario"]);
                titular.nombre = row["Nombre"].ToString().Trim();
                titular.cargo= row["Cargo"].ToString().Trim();
                titular.siglas= row["Iniciales"].ToString().Trim();
                titular.recibe = Convert.ToBoolean(row["Recibe"]);
                titular.dirigido = Convert.ToBoolean(row["Dirigido"]);
                titular.estadoVigente = Convert.ToBoolean(row["Estado"]);
            }

            RespuestaFuncionario Resultado = new RespuestaFuncionario();
            Resultado.session = 1;
            Resultado.catalogo = titular;
            
            return new JsonResult(Resultado);
        }

        [HttpPut]
        public JsonResult Put(Funcionarios Datos)
        {
            string query = @"Update CatFuncionarios
                    Set
                    Nombre = @nombre,
                    Cargo = @cargo,
                    Iniciales = @siglas,
                    Recibe = @recibe,
                    Dirigido = @dirigido,
                    Estado = @estado

                    Where IdFuncionario = @id
                    
                   select IdFuncionario,Nombre,Estado,Iniciales,Cargo,Recibe,Dirigido,Status
                            from
                            dbo.CatFuncionarios
                            Where  IdFuncionario = @id";
       
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", Datos.id);
                    myCommand.Parameters.AddWithValue("@nombre", Datos.nombre);
                    myCommand.Parameters.AddWithValue("@cargo", Datos.cargo);
                    myCommand.Parameters.AddWithValue("@siglas", Datos.siglas);
                    myCommand.Parameters.AddWithValue("@recibe", Datos.recibe);
                    myCommand.Parameters.AddWithValue("@dirigido", Datos.dirigido);
                    myCommand.Parameters.AddWithValue("@estado", Datos.estadoVigente);
                                        
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            Funcionarios titular = new Funcionarios();
            foreach (DataRow row in table.Rows)
            {               
                titular.id = Convert.ToInt32(row["IdFuncionario"]);
                titular.nombre = row["Nombre"].ToString().Trim();
                titular.cargo= row["Cargo"].ToString().Trim();
                titular.siglas= row["Iniciales"].ToString().Trim();
                titular.recibe = Convert.ToBoolean(row["Recibe"]);
                titular.dirigido = Convert.ToBoolean(row["Dirigido"]);
                titular.estadoVigente = Convert.ToBoolean(row["Estado"]);             
            }

            RespuestaFuncionario Resultado = new RespuestaFuncionario();
            Resultado.session = 1;
            Resultado.catalogo = titular;

            return new JsonResult(Resultado);
        }
    }
}
