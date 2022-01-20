
using System.Collections.Generic;


namespace GesDoc21.Models
{
    public class STipoDocumento
    {
        public int id { get; set; }
        public string descripcion {get; set; }
        public string siglas { get; set; }
        public int estado { get; set; }
        public int turnar { get; set; }
    }

    public class Tipos
    {
        public int session { get; set; }
       
        public List<TipoDocumento> catalogo = new List<TipoDocumento>();

        public Tipos() {}
    }

    public class TiposRespuesta
    {
        public int session { get; set; }

        public TipoDocumento catalogo = new TipoDocumento();

        public TiposRespuesta() { }
    }

    public class TipoDocumento
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string siglas { get; set; }
        public bool estadoVigente { get; set; }
        public bool esTurnada { get; set; }
        public bool eliminar { get; set; }

    }

}
