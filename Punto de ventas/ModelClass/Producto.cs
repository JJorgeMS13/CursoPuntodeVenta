using BarcodeLib;
using LinqToDB;
using Punto_de_ventas.Conexion;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_ventas.ModelClass
{
   public class Producto : Coneccion
    {

        private string fecha = DateTime.Now.ToString("dd/MMM/yyy");
        private string dia = DateTime.Now.ToString("dd");
        private string mes = DateTime.Now.ToString("MMM");
        private string year = DateTime.Now.ToString("yyy");
        private string codeBarra;
        public void getProductos( DataGridView dataGridView)
        {
            var query = Compras.Join(TempoProductos, c => c.IdCompra, t => t.IdCompra,(
                c,t)=> new
                {
                    c.IdCompra,
                    c.Producto,
                    c.Cantidad,
                    c.Precio,
                    c.Importe,
                    c.Proveedor,
                    c.Fecha
                });

            dataGridView.DataSource = query.ToList();
            dataGridView.Columns[0].Visible = false;
        }

        internal void codigoBarra(Panel panelCodigo, String codigosBarras, String producto,String precio)
        {
            int codigo = 0, count = 0;
            if (codigosBarras == "0")
            {
                var product1 = Producto.Where(p => p.Producto.Equals(producto) && p.Precio.Equals("$" + precio)).ToList();
                if (0 < product1.Count)
                {
                    codeBarra = product1[0].Codigo;
                    ClassModel.barcode.IncludeLabel = true;
                    panelCodigo.BackgroundImage = ClassModel.barcode.Encode(TYPE.CODE128, codeBarra,
                        Color.Black, Color.White, 198, 59);
                }
                else
                {
                    do
                    {
                        for (int i = 1; i <= 20; i++)
                        {
                            codigo = ClassModel.rnd.Next(1000000, 10000001);
                        }
                        codeBarra = Convert.ToString(codigo);
                        var query = Producto.Where(p => p.Codigo.Equals(codeBarra)).ToList();
                        count = query.Count();
                        if (count != 0)
                        { 
                                count = 0;
                        }
                        ClassModel.barcode.IncludeLabel = true;
                        panelCodigo.BackgroundImage = ClassModel.barcode.Encode(TYPE.CODE128, codeBarra,
                            Color.Black, Color.White, 198, 59);
                    } while (count > 0);
                }

            }
            else
            {
                codeBarra = codigosBarras;
                ClassModel.barcode.IncludeLabel = true;
                panelCodigo.BackgroundImage = ClassModel.barcode.Encode(TYPE.CODE128, codeBarra,
                    Color.Black, Color.White, 198, 59);
            }

        }

        public List<Departamentos> GetDepartamentos()
        {
            return Departamento.ToList();
        }

      internal object GetCategorias(int dpto)
        {
            var query = Categoria.Where(c => c.IdDpto.Equals(dpto));
            return query.ToList();
        }

        public void saveProducto(String producto, int cantidad, String precio, String departamento, String categoria,
            String accion, int IdProducto)
        {
            int count1 = 0, cant, count2 = 0;

            switch (accion)
            {
                case "Insert":

                    var product1 = Producto.Where(p => p.Producto.Equals(producto) && p.Precio.Equals(precio)).ToList();
                    if (0 == product1.Count)
                    {
                        Producto.Value(p => p.Codigo, codeBarra)
                            .Value(p => p.Producto, producto)
                            .Value(p => p.Precio, "$" + String.Format("{0: #,###,###,##0.00####}", precio))
                            .Value(p => p.Descuento, "$0.00")
                            .Value(p => p.Departamento, departamento)
                            .Value(p => p.Categoria, categoria)
                            .Insert();
                    }
                    var bodega = Bodegas.Where(b => b.Codigo.Equals(codeBarra)).ToList();

                    if (0 < bodega.Count())
                    {
                        cant = cantidad + bodega[0].Existencia;
                        Bodegas.Where(b => b.Id.Equals(bodega[0].Id))
                            .Set(b => b.IdProducto, bodega[0].IdProducto)
                            .Set(b => b.Codigo, codeBarra)
                            .Set(b => b.Existencia, cant)
                            .Set(b => b.Dia, Convert.ToInt16(dia))
                            .Set(b => b.Mes, mes)
                            .Set(b => b.Year, year)
                            .Set(b => b.Fecha, fecha)
                            .Update();
                    }

                    break;
                case "Update":
                    var product = Producto.Where(p => p.IdProducto.Equals(IdProducto)).ToList();
                    Producto.Where(p => p.IdProducto.Equals(IdProducto))
                       .Set(p => p.Codigo, codeBarra)
                       .Set(p => p.Producto, producto)
                       .Set(p => p.Precio, "$" + String.Format("{0: #,###,###,##0.00####}", precio))
                       .Set(p => p.Descuento, product[0].Descuento)
                       .Set(p => p.Departamento, departamento)
                       .Set(p => p.Categoria, categoria)
                       .Update();
                    break;

            }

        }

        public bool verificarPrecioVenta(Label label, String precioVenta, String precioCompra, int funcion)
        {
            Decimal venta, compra;

            bool verifica = false;

            if (funcion == 1)
            {
                if (precioCompra != string.Empty && precioCompra != string.Empty)
                {
                    compra = Convert.ToDecimal(precioCompra.Replace("$", ""));
                    venta = Convert.ToDecimal(precioVenta);
                    if (compra > venta || compra == venta)
                    {
                        label.Text = "Ingrese un precio mayor al precio de compra";
                        label.ForeColor = Color.Red;
                        verifica = false;
                    }
                    else
                    {
                        label.Text = "Precio Venta";
                        label.ForeColor = Color.DarkCyan;
                        verifica = true;
                    }
                }

            }
            else
            {
                label.Text = "Precio Venta";
                label.ForeColor = Color.DarkCyan;
                verifica = true;
            }
            return verifica;
        }

        public void searchProducto(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Productos> query;

            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == string.Empty)
            {
                query = Producto.ToList();
            }
            else
            {
                query = Producto.Where(c => c.Producto.StartsWith(campo));
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
        }
    }
}
