using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class Cajas_ingresos
    {
        [PrimaryKey,Identity]
        public int Id { set; get; }
        public int Caja { set; get; }
        public string Ingreso { set; get; }
        public string Type { set; get; }
        public int Dia { set; get; }
        public string Mes { set; get; }
        public int Year { set; get; }
        public string Fecha { set; get; }
        public int IdUsuario { set; get; }
    }
}
