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
   public class Venta : Coneccion
    {
       private Decimal importe = 0, totalPagar = 0m;
        private string fecha = DateTime.Now.ToString("dd/MMM/yyy");
        public List<Bodega> searchBodega(string codigo)
        {
            return Bodegas.Where(b => b.Codigo.Equals(codigo)).ToList();
        }

        public void saveVentasTempo(string codigo,int funcion, int caja,int IdUsuario)
        {
            string importe, precios;
            int idTempo, cantidad = 1, existencia;
            Decimal descuento, precio, importes;

            var ventatemp = TempoVentas.Where(t => t.Codigo.Equals(codigo) && t.Caja.Equals(caja) && 
            t.IdUsuario.Equals(IdUsuario)).ToList();
            var product = Producto.Where(p => p.Codigo.Equals(codigo)).ToList();
            descuento = Convert.ToDecimal(product[0].Descuento.Replace("%", ""));
            precio = Convert.ToDecimal(product[0].Precio.Replace("$", ""));
            descuento = descuento / 100;
            descuento = precio * descuento;
            precio = precio - descuento;
            precios = String.Format("${0:#,###,###,##0.00####}",precio);

            if (0 < ventatemp.Count())
            {
                cantidad = ventatemp[0].Cantidad;
                if (funcion == 0)
                
                    cantidad += 1;
                
                else
                
                    cantidad--;
                  importes = precio * cantidad;
                  importe = String.Format("${0:#,###,###,##0.00####}", importes);
                  TempoVentas.Where(b => b.IdTempo.Equals(ventatemp[0].IdTempo) && b.Caja.Equals(caja) && b.IdUsuario.Equals(IdUsuario))
                        .Set(b => b.Codigo, product[0].Codigo)
                        .Set(b => b.Descripcion, product[0].Producto)
                        .Set(b => b.Precio, product[0].Precio)
                        .Set(b => b.Cantidad, cantidad)
                        .Set(b => b.Importe, importe)
                        .Set(b => b.Caja,caja)
                        .Set(b => b.IdUsuario,IdUsuario)
                        .Update();
                
            }
            else
            {
                TempoVentas.Value(b => b.Codigo, product[0].Codigo)
                    .Value(b => b.Descripcion, product[0].Producto)
                    .Value(b => b.Precio, precios)
                    .Value(b => b.Cantidad, 1)
                    .Value(b => b.Importe, precios)
                    .Value(b => b.Caja, caja)
                    .Value(b => b.IdUsuario,IdUsuario)
                    .Insert();
            }

            var bodega = Bodegas.Where(b => b.Codigo.Equals(codigo)).ToList();

            existencia = bodega[0].Existencia;

            if (existencia == 1)
            {
                Bodegas.Where(b => b.Id.Equals(bodega[0].Id)).Delete();
            }
            else
            {
                existencia-- ;

                Bodegas.Where(b => b.Id.Equals(bodega[0].Id))
                    .Set(b => b.Codigo, bodega[0].Codigo)
                    .Set(b => b.Existencia, existencia)
                    .Set(b => b.Fecha, bodega[0].Fecha)
                    .Set(b => b.IdProducto, bodega[0].IdProducto)
                    .Update();
            }
        }

        public void searchVentatemp(DataGridView dataGridView, int num_pagina, int reg_por_pagina, int caja,int IdUsuario)
        {
            var query = TempoVentas.Where(t => t.Caja.Equals(caja) && t.IdUsuario.Equals(IdUsuario)).ToList();
            int inicio = (num_pagina - 1) * reg_por_pagina;
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[6].Visible = false;
        }
        internal void importes(Label label, int caja,int IdUsuario)
        {
            importe = 0;
            var ventaTempo = TempoVentas.Where(t => t.Caja.Equals(caja) && t.IdUsuario.Equals(IdUsuario)).ToList();
            if (0 < ventaTempo.Count)
            {
                ventaTempo.ForEach(iterator => {
                    importe += Convert.ToDecimal(iterator.Importe.Replace("$", ""));
                    label.Text = string.Format("${0:#,###,###,##0.00####}", importe);
                });
            }
            else
            {
                label.Text = "$0.00"; 
            }
        }

        public void deleteVenta(string codigo, int cant,int caja, int IdUsuario)
        {
            int cantidad = 0, existencia = 0;
            var ventatemp = TempoVentas.Where(t => t.Codigo.Equals(codigo) && t.Caja.Equals(caja) && t.IdUsuario.Equals(IdUsuario)).ToList();
            cantidad = ventatemp[0].Cantidad;

            var bodega = Bodegas.Where(b => b.Codigo.Equals(codigo)).ToList();
            existencia = bodega[0].Existencia;

            if (cant == 1)
            {
                existencia += cantidad;
                TempoVentas.Where(p => p.IdTempo == ventatemp[0].IdTempo && p.Caja.Equals(caja) && p.IdUsuario.Equals(IdUsuario)).Delete();
            }
            else
            {
                existencia++;
                saveVentasTempo(codigo, 1,caja,IdUsuario);
            }
            Bodegas.Where(b => b.Id.Equals(bodega[0].Id))
                .Set(b => b.Codigo, bodega[0].Codigo)
                .Set(b => b.Existencia, existencia)
                .Set(b => b.Fecha, bodega[0].Fecha)
                .Set(b => b.IdProducto, bodega[0].IdProducto)
                .Update();
        }

        public void pagos(TextBox textBox, Label label1, Label label2, Label label3)
        {
            Decimal pago, pagar;
            if (textBox.Text == string.Empty)
            {
                label1.Text = "Su cambio";
                label1.ForeColor = Color.LightSlateGray;
                label2.Text = "$0.00";
            }
            else
            {
                pagar = importe;
                pago = Convert.ToDecimal(textBox.Text);
                if (pago >= pagar)
                {
                    label1.Text = "Su cambio";
                    label1.ForeColor = Color.Green;
                    totalPagar = pago - pagar;
                }
                if (pago < pagar)
                {
                    label1.Text = "Pago insuficiente";
                    label1.ForeColor = Color.Red;
                    totalPagar = pagar - pago;
                }
                label2.Text = string.Format("${0:#,###,###,##0.00####}",totalPagar);
            }
            label3.Text = "Pagó con";
            label3.ForeColor = Color.Teal;
        }

        public void searchCliente(DataGridView dataGridView, string campo)
        {
            if (campo == string.Empty)
            {
                var query = Cliente.Join(Reportes_Clientes,
                    c => c.IdCliente,
                    r => 0,
                    (c,r) => new
                    {
                      c.IdCliente,
                      c.ID,
                      c.Nombre,
                      c.Apellido,
                      r.SaldoActual,
                      r.FechaActual,
                      r.UltimoPago,
                      r.FechaPago
                    }).Where(c => c.ID.StartsWith(campo));
                dataGridView.DataSource = query.ToList();
            }
            else
            {
                var query = Cliente.Join(Reportes_Clientes,
                    c => c.IdCliente,
                    r => r.IdCliente,
                    (c,r) => new
                    {
                      c.IdCliente,
                      c.ID,
                      c.Nombre,
                      c.Apellido,
                      r.SaldoActual,
                      r.FechaActual,
                      r.UltimoPago,
                      r.FechaPago
                    }).Where(c => c.ID.StartsWith(campo) || c.Nombre.StartsWith(campo));
                dataGridView.DataSource = query.ToList();
            }
            dataGridView.Columns[0].Visible = false;

            //dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            //dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            //dataGridView.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            //dataGridView.Columns[4].DefaultCellStyle.BackColor = Color.Green;
            //dataGridView.Columns[6].DefaultCellStyle.BackColor = Color.Green;
        }

        public void dataCliente( CheckBox checkBox, TextBox textBox_Pagos, TextBox textBox_Buscar, DataGridView dataGridView,
            List<Label> labels)
        {
            String deuda1, nombre, apellido;
            Decimal deuda2, deudaTotal;
            if (checkBox.Checked == true)
            {
                if (textBox_Pagos.Text == string.Empty)
                {
                    labels[0].Text = "$0.00";
                    labels[1].Text = "$0.00";
                    labels[2].Text = labels[0].Text;
                    labels[3].Text = "";
                    labels[4].Text = "$0.00";
                    labels[5].Text = "$0.00";
                    labels[6].Text = "--/--/--";
                }
                else
                {
                    if (textBox_Buscar.Text != string.Empty)
                    {
                        deuda1 = Convert.ToString(dataGridView.CurrentRow.Cells[4].Value);

                        deuda2 = Convert.ToDecimal(deuda1.Replace("$", ""));
                        deudaTotal = deuda2 + totalPagar;
                        labels[0].Text = string.Format("${0:#,###,###,##0.00####}",deudaTotal);
                        nombre = Convert.ToString(dataGridView.CurrentRow.Cells[2].Value);
                        apellido = Convert.ToString(dataGridView.CurrentRow.Cells[3].Value);

                        labels[3].Text = nombre + " " + apellido;
                        labels[1].Text = string.Format("${0:#,###,###,##0.00####}", totalPagar);
                        labels[4].Text = deuda1;
                        labels[2].Text = string.Format("${0:#,###,###,##0.00####}", deudaTotal);
                        labels[5].Text = Convert.ToString(dataGridView.CurrentRow.Cells[6].Value);
                        labels[6].Text = fecha;
                    }
                }
            }
            else
            {
                labels[0].Text = "$0.00";
                labels[1].Text = "$0.00";
                labels[2].Text = labels[0].Text;
                labels[3].Text = "";
                labels[4].Text = "$0.00";
                labels[5].Text = "$0.00";
                labels[6].Text = "--/--/--";
            }
        }

        public void cobrar(CheckBox checkBox, TextBox textBox_Pago, DataGridView dataGridView, List<Label> labels, int caja)
        {
            if (textBox_Pago.Text == string.Empty)
            {
                labels[7].Text = "Ingrese el pago";
                labels[7].ForeColor = Color.Red;
                textBox_Pago.Focus();
            }
            else
            {
                String saldoActual, fechaPago;
                Decimal deuda, deudaActual = 0m, pago, pagos;
                int idCliente;
                if (checkBox.Checked)
                {
                    if (dataGridView.CurrentRow != null)
                    {
                        idCliente = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value);

                        saldoActual = Convert.ToString(dataGridView.CurrentRow.Cells[3].Value);
                        deudaActual = Convert.ToDecimal(saldoActual.Replace("$",""));
                    }
                    else
                    {
                        labels[8].Text = "Seleccione un cliente"; 
                    }
                }

            }
        }
    }
}
