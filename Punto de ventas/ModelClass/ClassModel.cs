using BarcodeLib;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto_de_ventas.ModelClass
{
   public class ClassModel
    {
        public static List<Cajas> listCaja;
        public static List<Usuarios> listUsuario;
        public static List<Clientes> numClientes;
        public static List<Proveedores> numProveedor;
        public static List<Tempo_compras> numTempoCompras;
        public static List<RutaUsuario> listruta = new List<RutaUsuario>();

        public static Caja caja = new Caja();
        public static Login login = new Login();
        public static Compra compra = new Compra();
        public static Cliente cliente = new Cliente();
        public static Proveedor proveedores = new Proveedor();
        public static Departamento dptocat = new Departamento();
        public static TextBoxEvent eventos = new TextBoxEvent();
        public static Usuario usuario = new Usuario();
        public static Producto producto = new Producto();
        public static Barcode barcode = new Barcode();
        public static Random rnd = new Random();

    }
}
