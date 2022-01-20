using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class SubDependencias
    {
        public int id { get; set; }
        public int idDependencia { get; set; }
        public string nombre { get; set; }
        public string siglas { get; set; }
        public int orden { get; set; }
        public string estado { get; set; }
    }

    public class ListaSubDependencias
    {
        public int session { get; set; }

        public List<SubDependencias> catalogo = new List<SubDependencias>();

        public ListaSubDependencias() { }
    }

    public class RespuestaSubDependencias
    {
        public int session { get; set; }

        public SubDependencias catalogo = new SubDependencias();

        public RespuestaSubDependencias() { }
    }
}
