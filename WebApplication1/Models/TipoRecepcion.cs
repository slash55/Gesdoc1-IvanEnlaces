
using System.Collections.Generic;


namespace GesDoc21.Models
{
    public class TiposRecepcion
    {
        public int session { get; set; }
       
        public List<TipoRecepcion> catalogo = new List<TipoRecepcion>();

        public TiposRecepcion() {}
    }

    public class TiposRespuestaRecepcion
    {
        public int session { get; set; }

        public TipoRecepcion catalogo = new TipoRecepcion();

        public TiposRespuestaRecepcion() { }
    }

    public class TipoRecepcion
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool eliminar { get; set; }

    }

}
