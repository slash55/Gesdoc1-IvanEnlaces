
using System.Collections.Generic;


namespace GesDoc21.Models
{
    public class TiposClasificacion
    {
        public int session { get; set; }
       
        public List<TipoClasificacion> catalogo = new List<TipoClasificacion>();

        public TiposClasificacion() {}
    }

    public class TiposRespuestaClasificacion
    {
        public int session { get; set; }

        public TipoClasificacion catalogo = new TipoClasificacion();

        public TiposRespuestaClasificacion() { }
    }

    public class TipoClasificacion
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string iniciales { get; set; }
        public bool eliminar { get; set; }

    }

}
