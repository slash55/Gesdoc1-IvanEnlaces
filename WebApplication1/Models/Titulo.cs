using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class Titulo
    {
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public bool estado { get; set; }
    }

    public class ListaTitulo
    {
        public int session { get; set; }

        public List<Titulo> catalogo = new List<Titulo>();

        public ListaTitulo() { }
    }

    public class RespuestaTitulo
    {
        public int session { get; set; }

        public Titulo catalogo = new Titulo();

        public RespuestaTitulo() { }
    }
}
