using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Compras
    {
        [PrimaryKey, Identity]
        public int IdCompra { set; get; }
        public string Producto { set; get; }
        public int Cantidad { set; get; }
        public string Precio { set; get; }
        public string Importe { set; get; }
        public int IdProveedor { set; get; }
        public string Proveedor { set; get; }
        public int IdUsuario { set; get; }
        public string Usuario { set; get; }
        public string Role { set; get; }
        public int Dia { set; get; }
        public string Mes { set; get; }
        public int Year { set; get; }
        public string Fecha { set; get; }
        public string Codigo { set; get; }
    }
}
