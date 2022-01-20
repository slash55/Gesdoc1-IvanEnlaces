using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    
    public class nivel_1
    {
        public int session { get; set; }

        public List<nivel_2> catalogo = new List<nivel_2>();

        public nivel_1() { }
    }
    
    public class nivel_2
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string siglas { get; set; }
        public int orden { get; set; }
        public int ambito { get; set; }

        public List<nivel_3> Subdependencias = new List<nivel_3>();

    }
}

public class nivel_3
{
    public int id { get; set; }
    public string nombre { get; set; }
    public string siglas { get; set; }
    public int orden { get; set; }
  
    public nivel_3(int id, string desc, string siglas, int orden)
    {

        this.id = id;

        this.nombre = desc;

        this.siglas = siglas;

        this.orden = orden;

    }
    public nivel_3()
    {
    }
    public List<nivel_4> enlaces = new List<nivel_4>();
}
public class nivel_4

{
    public int id { get; set; }
    public string nombre { get; set; }
 
    public nivel_4(int id, string desc)
    {

        this.id = id;

        this.nombre = desc;
   

    }
    public nivel_4()
    {
    }
}