using LinqToDB;
using LinqToDB.Data;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.Conexion
{
   public class Coneccion : DataConnection
    {
        public Coneccion() : base("Conectar")
        {

        }
        public ITable<Clientes> Cliente { get { return GetTable<Clientes>(); } }
        public ITable<reportes_clientes> Reportes_Clientes { get { return GetTable<reportes_clientes>(); } }

        public ITable<Proveedores> Proveedor { get { return GetTable<Proveedores>(); } }
        public ITable<Reportes_Proveedores> ReportesProveedores { get { return GetTable<Reportes_Proveedores>(); } }

        public ITable<Usuarios> Usuario { get { return GetTable<Usuarios>(); } }

        public ITable<Cajas> Cajas { get { return GetTable<Cajas>(); } }

        public ITable<Cajas_registros> Cajastemporal { get { return GetTable<Cajas_registros>(); } }

        public ITable<Departamentos> Departamento { get { return GetTable<Departamentos>(); } }

        public ITable<Categorias> Categoria { get { return GetTable<Categorias>(); } }
        public ITable<Tempo_compras> TempoCompras { get { return GetTable<Tempo_compras>(); } }

        public ITable<tempo_productos> TempoProductos { get { return GetTable<tempo_productos>(); } }
        public ITable<Compras> Compras { get { return GetTable<Compras>(); } }
        public ITable<Cajas_ingresos> CajasIngresos { get { return GetTable<Cajas_ingresos>(); } }
        public ITable<RutaUsuario> Rutas { get { return GetTable<RutaUsuario>(); } }

        public ITable<Productos> Producto { get { return GetTable<Productos>(); } }
        public ITable<Bodega> Bodegas { get { return GetTable<Bodega>(); } }
        public ITable<Tempo_ventas> TempoVentas { get { return GetTable<Tempo_ventas>(); } }
    }
}
