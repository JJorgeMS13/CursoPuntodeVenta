using LinqToDB;
using Punto_de_ventas.Conexion;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Punto_de_ventas.ModelClass
{
    public class Usuario : Coneccion
    {
        private Caja caja = new Caja();
        private List<Usuarios> listUsuarios, listUsuario;
        private List<Cajas> listCajas, listCaja;
        private string fecha = DateTime.Now.ToString("dd/MMM/yyy");
        private string hora = DateTime.Now.ToString("hh:mm:ss");

        public Usuario()
        {
            listUsuario = new List<Usuarios>();
            listCaja = new List<Cajas>();
        }

        public object[] login(string usuario, string password)
        {
            listUsuario.Clear();
            listUsuarios = Usuario.Where(u => u.Usuario == usuario).ToList();
            if (0 < listUsuarios.Count)
            {
                String pass = Encriptar.DecryptData(listUsuarios[0].Password, listUsuarios[0].Usuario);
                if (pass == password)
                {
                    listUsuario = listUsuarios;
                    int idUsuario = listUsuarios[0].IdUsuario;
                    string nombre = listUsuarios[0].Nombre;
                    string apellido = listUsuarios[0].Apellido;
                    string user = listUsuarios[0].Usuario;
                    string role = listUsuarios[0].Role;
                    string foto = listUsuarios[0].Foto;
                    listCaja = caja.getCaja();
                    if (role == "Admin")
                    {
                        caja.insertCajastemporal(idUsuario, nombre, apellido, user, role, 0, 0, false,
                            hora, fecha);
                    }
                    else
                    {
                        if (0 < listCaja.Count)
                        {
                            listCajas = listCaja;
                            int idcaja = listCaja[0].IdCaja;
                            int cajas = listCaja[0].Caja;
                            bool estado = listCaja[0].Estado;
                            caja.updateCaja(listCaja[0].IdCaja, false);
                            caja.insertCajastemporal(idUsuario, nombre, apellido, user, role, idcaja,
                                cajas, estado, hora, fecha);
                        }
                    }
                }

            }
            object[] objects = { listUsuario, listCaja };
            return objects;
        }
        public void GuardarFotos(string nombre,string apellido, string telefono,string direccion,string email,string usuario,string pass, string rol, string foto)
        {
            Usuario.Value(f => f.Nombre, nombre)
                .Value(f => f.Apellido, apellido)
                .Value(f => f.Telefono, telefono)
                .Value(f => f.Direccion, direccion)
                .Value(f => f.Email, email)
                .Value(f => f.Usuario, usuario)
                .Value(f => f.Password, pass)
                .Value(f => f.Role, rol)
                .Value(f => f.Foto, foto)
                .Insert();
        }
        //Se obtiene la ruta del usuario 
        public List<RutaUsuario> ruta(int id)
        {
            return Rutas.Where(r => r.id == id)
                .ToList();
        }
    }
}
