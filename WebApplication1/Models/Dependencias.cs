using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class Dependencias
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string siglas { get; set; }
        public int orden { get; set; }
        public string estado { get; set; }
    }

    public class ListaDependencias
    {
        public int session { get; set; }

        public List<Dependencias> catalogo = new List<Dependencias>();

        public ListaDependencias() { }
    }

    public class RespuestaDependencias
    {
        public int session { get; set; }

        public Dependencias catalogo = new Dependencias();

        public RespuestaDependencias() { }
    }
}

namespace GesDoc21
{
    public class idDependencia
    {
    }
}