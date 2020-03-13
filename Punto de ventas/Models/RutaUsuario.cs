﻿using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Models
{
   public class RutaUsuario
    {
        [PrimaryKey, Identity]
        public int id { get; set; }
        public string ruta { get; set; }
        public int idUsuario { get; set; }
    }
}
