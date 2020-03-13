using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Reportes_Proveedores
    {
        [PrimaryKey, Identity]
        public int IdRegistro { get; set; }
        public int IdProveedor { get; set; }
        public string SaldoActual { get; set; }
        public string FechaActual { get; set; }
        public string UltimoPago { get; set; }
        public string FechaPago { get; set; }
    }
}
