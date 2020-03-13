using LinqToDB;
using Punto_de_ventas.Conexion;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.ModelClass
{
   public class Caja : Coneccion
    {
        public List<Cajas> getCaja()
        {
            return Cajas.Where(c => c.Estado == true).ToList();
        }
        public void updateCaja(int idCaja, bool estado)
        {
            Cajas.Where(c => c.IdCaja == idCaja)
                .Set(c => c.Estado, estado)
                .Update();
        }
        public void insertCajastemporal(int idUsuario, string nombre, string apellido, string usuario,
            string role, int idCaja, int caja, bool estado, string hora, string fecha)
        {
            Cajastemporal.Value(c => c.IdUsuario, idUsuario)
                .Value(c => c.Nombre, nombre)
                .Value(c => c.Apellido, apellido)
                .Value(c => c.Usuario, usuario)
                .Value(c => c.Role, role)
                .Value(c => c.IdCaja, idCaja)
                .Value(c => c.Caja, caja)
                .Value(c => c.Estado, estado)
                .Value(c => c.Hora, hora)
                .Value(c => c.Fecha, fecha)
                .Insert();
        }
    }
}
