using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Bodega
    {
        [PrimaryKey, Identity]

        public int Id { set; get; }
        public int IdProducto { set; get; }
        public string Codigo { set; get; }
        public int Existencia { set; get; }
        public int Dia { set; get; }
        public string Mes { set; get; }
        public string Year { set; get; }
        public string Fecha { set; get; }
    }
}
