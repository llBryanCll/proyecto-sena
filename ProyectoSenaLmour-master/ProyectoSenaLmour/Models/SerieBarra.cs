using ProyectoSenaLmour.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoSenaLmour.Models
{
	public class SerieBarra
	{
		public SerieBarra()
		{

		}

		public object[] GetDataDummy()
		{

			object[] data = new object[4];
			data[0] = new object[] { "Abonos", 1 };
			data[1] = new object[] { "Reservas", 1 };
			data[2] = new object[] { "Paquete", 3};
			data[3] = new object[] { "Servicios", 6 };
			return data;
		}

	}
}






