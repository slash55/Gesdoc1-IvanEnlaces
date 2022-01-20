using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class Instrucciones
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estadoVigente { get; set; }

    }
    public class ListaInstrucciones
    {
        public int session { get; set; }

        public List<Instrucciones> catalogo = new List<Instrucciones>();

        public ListaInstrucciones() { }
    }

    public class RespuestaInstrucciones
    {
        public int session { get; set; }

        public Instrucciones catalogo = new Instrucciones();

        public RespuestaInstrucciones() { }
    }
}

