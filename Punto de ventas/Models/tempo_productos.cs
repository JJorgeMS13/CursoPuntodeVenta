using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
    public class tempo_productos
    {
        [PrimaryKey, Identity]

        public int Id { get; set; }
        public int IdCompra { get; set; }
    }
}
