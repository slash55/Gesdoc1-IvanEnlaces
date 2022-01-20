using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GesDoc21.Models
{
    public class TiposCatArchivo
    {
        public int session { get; set; }

        public List<TipoCatArchivo> catalogo = new List<TipoCatArchivo>();

        public TiposCatArchivo() { }
    }

    public class TiposRespuestaCatArchivo
    {
        public int session { get; set; }

        public TipoCatArchivo catalogo = new TipoCatArchivo();

        public TiposRespuestaCatArchivo() { }
    }

    public class TipoCatArchivo
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool eliminar { get; set; }

    }

}