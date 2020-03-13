using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Productos
    {
        [PrimaryKey, Identity]
        public int IdProducto { set; get; }
        public string Codigo { set; get; }
        public string Producto { set; get; }
        public string Precio { set; get;  }
        public string Descuento { set; get; }
        public string Departamento { set; get; }
        public string Categoria { set; get; }
    }
}
