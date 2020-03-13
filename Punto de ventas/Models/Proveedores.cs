using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Proveedores
    {
        [PrimaryKey, Identity]
        public int IdProveedor { get; set; }
        public string Proveedor { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
