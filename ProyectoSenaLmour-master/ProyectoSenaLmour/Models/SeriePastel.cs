using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSenaLmour.Models
{
    public class SeriePastel
    {
        //name: 'Chrome',
        //    y: 61.41,
        //    sliced: true,
        //    selected: true

        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }

        public SeriePastel()
        {

        }

        public SeriePastel(string name, double y, bool sliced = false, bool selected = false)
        {
            this.name = name;
            this.y = y;
            this.sliced = sliced;
            this.selected = selected;
        }

        public List<SeriePastel> GetDataDummy()
        {
            List<SeriePastel> lista = new List<SeriePastel>();

            lista.Add(new SeriePastel("Abonos", 1));
            lista.Add(new SeriePastel("Reservas", 1));
            lista.Add(new SeriePastel("Paquete", 3));
            lista.Add(new SeriePastel("Servicios", 6));
           

            return lista;
        }
    }
}