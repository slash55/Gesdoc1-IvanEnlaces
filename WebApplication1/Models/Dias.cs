using System.Collections.Generic;
namespace GesDoc21.Models
{
    public class Dias
    {
        public int id { get; set; }
        public string fecha { get; set; }
        public  bool estado { get; set; }
      
    }
    public class ListaDias
    {
        public int session { get; set; }

        public List<Dias> catalogo = new List<Dias>();

        public ListaDias() { }
    }

    public class RespuestaDias
    {
        public int session { get; set; }

        public Dias catalogo = new Dias();

        public RespuestaDias() { }
    }
}
