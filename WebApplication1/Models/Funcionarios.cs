using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class Funcionarios
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string cargo { get; set; }
        public string siglas { get; set; }
        public bool recibe { get; set; }
        public bool dirigido { get; set; }
        public bool estadoVigente { get; set; } 
        

    }

    public class ListaFuncionarios
    {
        public int session { get; set; }

        public List<Funcionarios> catalogo = new List<Funcionarios>();

        public ListaFuncionarios() { }
    }

    public class RespuestaFuncionario
    {
        public int session { get; set; }

        public Funcionarios catalogo = new Funcionarios();

        public RespuestaFuncionario() { }
    }
}
