using System.Collections.Generic;

namespace GesDoc21.Models
{
    public class Cabecera
    {
        public int session { get; set; }
        
        public List<Dependencia> catalogo = new List<Dependencia>();

        public List<Enlaces> catEnlaces = new List<Enlaces>();
        public Cabecera() {}
    }

    public class Dependencia
    {
        //klsdfjksdhfjñhdjfhsdjkafhajklsdhfasjk
        public int id { get; set; }
        public string nombre { get; set; }
        public string siglas { get; set; }
        public int orden { get; set; }
        public int ambito { get; set; }
        
        public List<subDependencia> subDependencias = new List<subDependencia>();

        public List<Enlaces> enlaces = new List<Enlaces>();

        public Dependencia () {}
        
    }
    public class Enlaces
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string cuenta { get; set; }       
        public string correo { get; set; }
        public string telefono { get; set; }
        public int orden { get; set; }

        public Enlaces() {}
    }
  
    public class subDependencia
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string siglas { get; set; }
        public int orden { get; set; }  
        
        public List<Enlaces> enlaces = new List<Enlaces>();
        public subDependencia() { }
    }

}
