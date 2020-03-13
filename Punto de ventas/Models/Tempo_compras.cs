using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Tempo_compras
    {
        [PrimaryKey,Identity]
        public int IdCompra { set; get; }
        public string Descripcion { set; get; }
        public int Cantidad { set; get; }
        public string PrecioCompra { set; get; }
        public string Importe { set; get; }
    }
}
