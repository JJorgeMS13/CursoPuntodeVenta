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
   public class Compra : Coneccion
    {
        private string fecha = DateTime.Now.ToString("dd/MMM/yyy");
        private string dia = DateTime.Now.ToString("dd");
        private string mes = DateTime.Now.ToString("MMM");
        private string year = DateTime.Now.ToString("yyy");
        private Decimal enCaja = 0, deuda = 0, pago;
        private bool verificar = false, deudaProveedor = false;

        public List<Tempo_compras> getTempo_Compras()
        {
            return TempoCompras.ToList();
        }
        internal void guardarTempoCompras(string des, int cant, string precio)
        {
            var comprasTemp = TempoCompras.Where(c => c.Descripcion == des && c.PrecioCompra.Replace("$",
                "") == precio).ToList();

            if (0 < comprasTemp.Count)
            {
                int cant1;
                string importe1, valor;
                Decimal precio1, importe2, importe3;

                precio1 = Convert.ToDecimal(precio);
                importe2 = precio1 * cant;
                importe1 = comprasTemp[0].Importe.Replace("$", "");
                importe3 = Convert.ToDecimal(importe1);
                importe2 = importe2 + importe3;
                valor = Convert.ToString(importe2);
                valor = valor.Replace(" ", "");
                cant1 = cant + comprasTemp[0].Cantidad;
                TempoCompras.Where(c => c.IdCompra == comprasTemp[0].IdCompra)
                    .Set(c => c.Cantidad, cant1)
                    .Set(c => c.PrecioCompra, "$" + string.Format("{0: #,###,###,##0.00####}",
                    precio))
                    .Set(c => c.Importe, "$" + string.Format("{0: #,###,###,##0.00####}",valor))
                    .Update();
            }
            else
            {
                Decimal precio1, importe;
                precio1 = Convert.ToDecimal(precio);
                importe = precio1 * cant;
                TempoCompras.Value(t => t.Descripcion, des)
                    .Value(t => t.Cantidad, cant)
                    .Value(t => t.PrecioCompra, "$" + string.Format("{0: #,###,###,##0.00####}", precio))
                    .Value(t => t.Importe, "$" + string.Format("{0: #,###,###,##0.00####}", importe))
                    .Insert();
            }
        }

        public void searchCompras(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Tempo_compras> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == string.Empty)
            {
                query = TempoCompras.ToList();
            }
            else
            {
                query = TempoCompras.Where(c => c.Descripcion.StartsWith(campo));
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;
            dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.ForeColor = Color.Green;
            dataGridView.Columns[4].DefaultCellStyle.ForeColor = Color.Green;
        }
        public void importeTempo( Label label1, Label label2, Label label3)
        {
            deuda = 0;
            var compras = getTempo_Compras();
            Decimal importe;
            if (0 < compras.Count )
            {
                compras.ForEach(c =>
                {

                    importe = Convert.ToDecimal(c.Importe.Replace("$", ""));
                    deuda += importe;
                });
                var data = "$" + String.Format("{0: #,###,###,##0.00####}", deuda);
                label1.Text = data.Replace(" ", "");
                label2.Text = data.Replace(" ","");
                label3.Text = data.Replace(" ", "");
            }
        }
        internal void updateTempoCompras(int id, string des, int cant, string precio)
        {
            string valor;
            Decimal precio1, importe2;
            precio1 = Convert.ToDecimal(precio);
            importe2 = precio1 * cant;
            valor = Convert.ToString(importe2).Replace(" ", "");
            //valor = valor.Replace(" ", "");

            TempoCompras.Where(c => c.IdCompra == id)
                .Set(c => c.Descripcion, des)
                .Set(c => c.Cantidad, cant)
                .Set(c => c.PrecioCompra, "$" + string.Format("{0: #,###,###,##0.00####}", precio))
                .Set(c => c.Importe, "$" + string.Format("{0: #,###,###,##0.00####}", valor))
                .Update();
        }
        public void deleteCompras(int idCompra)
        {
            TempoCompras.Where(c => c.IdCompra == idCompra).Delete();
        }
        public void deleteTempo_Compras()
        {
            TempoCompras.Delete();
        }
        public void searchProveedor(DataGridView dataGridView, string campo)
        {
            
            if (campo == string.Empty)
            {
                var query = from p in Proveedor
                            join r in ReportesProveedores
                            on p.IdProveedor equals r.IdProveedor
                            where p.Proveedor.StartsWith(" ")
                            select new
                            {
                                p.IdProveedor,
                                p.Proveedor,
                                p.Email,
                                p.Telefono,
                                r.SaldoActual
                            };
                dataGridView.DataSource = query.ToList();
            }
            else
            {
                var query = from p in Proveedor
                            join r in ReportesProveedores
                            on p.IdProveedor equals r.IdProveedor
                            where p.Proveedor.StartsWith(campo) || p.Email.StartsWith(campo) || p.Telefono.StartsWith(campo)
                            select new
                            {
                                p.IdProveedor,
                                p.Proveedor,
                                p.Email,
                                p.Telefono,
                                r.SaldoActual
                            };
                dataGridView.DataSource = query.ToList();
            }
            dataGridView.Columns[0].Visible = false;

            dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }
        public void getIngresos(Label label)
        {
            enCaja = 0;
            var ingreso = CajasIngresos.Where(c => c.Fecha == fecha).ToList();      
            Decimal importe;
            if (0 < ingreso.Count)
            {
                ingreso.ForEach(item => {
                 importe = Convert.ToDecimal(item.Ingreso.Replace("$", ""));
                    enCaja += importe;
                });
                var data = "$" + String.Format("{0: #,###,###,##0.00####}",enCaja);
                label.Text = data.Replace(" ", "");
            }
        }
        public bool verificarPago(TextBox textBox, Label label, CheckBox checkBox, Label label1,
            Label label2, Label label3, int idProveedor,Label label_Titulo)
        {
            if (idProveedor != 0)
            {
                
                label.Text = "Pago con";
                label.ForeColor = Color.LightSlateGray;
                label1.ForeColor = Color.LightSlateGray;
                label_Titulo.Text = "Deuda";
                label_Titulo.ForeColor = Color.LightSlateGray;
                label1.Text = "$ 0.0";
                label2.Text = "$ 0.0";
                label3.Text = "$ 0.0";
                if (enCaja < deuda)
                {
                    if (checkBox.Checked == true && textBox.Text != string.Empty)
                    {
                        pago = Convert.ToDecimal(textBox.Text);
                        label.Text = "Se genera una deuda del sistema al proveedor";
                        label.ForeColor = Color.Red;

                        Decimal deudas, saldo;
                        if (pago < enCaja)
                        {
                            deudas = deuda - pago;
                        }
                        else
                        {
                            deudas = deuda - enCaja;
                            textBox.Text = enCaja.ToString();
                        }
                        
                        var data = "$" + String.Format("{0: #,###,###,##0.00####}", deudas);
                        label1.Text = data.Replace(" ", "");
                        label1.ForeColor = Color.Red;
                        label_Titulo.ForeColor = Color.Red;
                        label2.Text = data.Replace(" ", "");
                        var reporte = getReporte(idProveedor);
                        saldo = Convert.ToDecimal(reporte[0].SaldoActual.Replace("$", ""));
                        saldo += deudas;
                        var data1 = label3.Text = "$" + String.Format("{0: #,###,###,##0.00####}", saldo);
                        label3.Text = data1.Replace(" ", "");
                        deudaProveedor = true;
                        verificar = true;
                    }
                    else
                    {
                        if (checkBox.Checked == true && textBox.Text == string.Empty)
                        {
                            //pago = 0;
                            label.Text = "Se genera una deuda del sistema al proveedor";
                            label.ForeColor = Color.Red;

                            Decimal deudas, saldo;
                            deudas = deuda - 0;
                            var data = "$" + String.Format("{0: #,###,###,##0.00####}", deudas);
                            label1.Text = data.Replace(" ", "");
                            label1.ForeColor = Color.Red;
                            label_Titulo.ForeColor = Color.Red;
                            label2.Text = data.Replace(" ", "");
                            var reporte = getReporte(idProveedor);
                            saldo = Convert.ToDecimal(reporte[0].SaldoActual.Replace("$", ""));
                            saldo += deudas;
                            var data1 = label3.Text = "$" + String.Format("{0: #,###,###,##0.00####}", saldo);
                            label3.Text = data1.Replace(" ", "");
                            deudaProveedor = true;
                            verificar = true;
                        }
                        else
                        {
                            if (textBox.Text != string.Empty)
                            {
                                pago = Convert.ToDecimal(textBox.Text);
                            }
                            
                            if (pago > enCaja && textBox.Text != string.Empty)
                            {
                                
                                label.Text = "No hay saldo suficiente en caja";
                                label.ForeColor = Color.Red;
                            }
                            else
                            {
                                label.Text = "Pago insuficiente";
                                label.ForeColor = Color.Red;
                            }
                        }
                    }
                }
                else if( enCaja  >= deuda)
                {
                    if (textBox.Text != string.Empty)
                    {
                        pago = Convert.ToDecimal(textBox.Text);
                    }
                    if (textBox.Text != string.Empty && pago >= deuda && checkBox.Checked != true)
                    {
                        if (pago <= enCaja)
                        {
                            //pago = Convert.ToDecimal(textBox.Text);
                            Decimal deudas, saldo;
                            deudas = pago - deuda;
                            var data = "$" + String.Format("{0: #,###,###,##0.00####}", deudas);
                            label1.Text = data.Replace(" ", "");
                            label1.Text = data.Replace(" ", "");
                            label1.ForeColor = Color.LightSlateGray;
                            label_Titulo.Text = "Su cambio es";
                            label_Titulo.ForeColor = Color.LightSlateGray;
                            label2.Text = "$ 0.0";
                            label3.Text = "$ 0.0";
                            //deudaProveedor = true;
                            verificar = true;
                        }
                        else
                        {
                            label.Text = "No hay saldo suficiente en caja";
                            label.ForeColor = Color.Red;
                        }

                    }
                    else
                    {
                        if (checkBox.Checked == true && textBox.Text == string.Empty)
                        {
                            
                            label.Text = "Se genera una deuda del sistema al proveedor";
                            label.ForeColor = Color.Red;

                            Decimal deudas, saldo;
                            deudas = deuda - 0;
                            var data = "$" + String.Format("{0: #,###,###,##0.00####}", deudas);
                            label1.Text = data.Replace(" ", "");
                            label1.ForeColor = Color.Red;
                            label_Titulo.ForeColor = Color.Red;
                            label2.Text = data.Replace(" ", "");
                            var reporte = getReporte(idProveedor);
                            saldo = Convert.ToDecimal(reporte[0].SaldoActual.Replace("$", ""));
                            saldo += deudas;
                            var data1 = label3.Text = "$" + String.Format("{0: #,###,###,##0.00####}", saldo);
                            label3.Text = data1.Replace(" ", "");
                            deudaProveedor = true;
                            verificar = true;
                        }
                        else
                        {
                            if (textBox.Text != string.Empty && checkBox.Checked == true)
                            {
                                //pago = Convert.ToDecimal(textBox.Text);
                                label.Text = "Se genera una deuda del sistema al proveedor";
                                label.ForeColor = Color.Red;

                                Decimal deudas, saldo;
                                if (pago < deuda)
                                {
                                    deudas = deuda - pago;
                                }
                                else
                                {
                                    deudas = 0;
                                    textBox.Text = deuda.ToString();
                                }

                                var data = "$" + String.Format("{0: #,###,###,##0.00####}", deudas);
                                label1.Text = data.Replace(" ", "");
                                label1.ForeColor = Color.Red;
                                label_Titulo.ForeColor = Color.Red;
                                label2.Text = data.Replace(" ", "");
                                var reporte = getReporte(idProveedor);
                                saldo = Convert.ToDecimal(reporte[0].SaldoActual.Replace("$", ""));
                                saldo += deudas;
                                var data1 = label3.Text = "$" + String.Format("{0: #,###,###,##0.00####}", saldo);
                                label3.Text = data1.Replace(" ", "");
                                deudaProveedor = true;
                                verificar = true;
                            }
                            else
                            {
                                label.Text = "Pago insuficiente";
                                label.ForeColor = Color.Red;
                            }
                        }
                    }
                }
            }
            else
            {
                if (textBox.Text == string.Empty && checkBox.Checked == false )
                {
                    MessageBox.Show("Seleccione un proveedor");
                }
                textBox.Text = string.Empty;
                checkBox.Checked = false;
            }
            return verificar;
        }
        public void saveCompras(string proveedor, int idProveedor,string usuario, int idusuario,string role)
        {
            var compras = getTempo_Compras();
            compras.ForEach(t => {
                Compras.Value(c => c.Cantidad, t.Cantidad)
                .Value(c => c.Producto, t.Descripcion)
                .Value(c => c.Precio, t.PrecioCompra)
                .Value(c => c.Importe, t.Importe)
                .Value(c => c.IdProveedor, idProveedor)
                .Value(c => c.Proveedor, proveedor)
                .Value(c => c.IdUsuario, idusuario)
                .Value(c => c.Usuario, usuario)
                .Value(c => c.Role, role)
                .Value(c => c.Dia, Convert.ToInt16(dia))
                .Value(c => c.Mes, mes)
                .Value(c => c.Year, Convert.ToInt16(year))
                .Value(c => c.Fecha, fecha)
                .Insert();
            });
            if (deudaProveedor)
            {
                Decimal deudas, saldo;
                deudas = deuda - pago;
                var reporte = getReporte(idProveedor);
                saldo = Convert.ToDecimal(reporte[0].SaldoActual.Replace("$", ""));
                saldo += deudas;
                var dataSaldo = "$" + String.Format("{0: #,###,###,##0.00###}", saldo);
                ReportesProveedores.Where(r => r.IdRegistro == reporte[0].IdRegistro)
                    .Set(r => r.IdProveedor, idProveedor)
                    .Set(r => r.SaldoActual, dataSaldo.Replace(" ", ""))
                    .Set(r => r.FechaActual, fecha)
                    .Set(r => r.UltimoPago, reporte[0].UltimoPago)
                    .Set(r => r.FechaPago, reporte[0].FechaPago)
                    .Update();
            }
        }
        public List<Reportes_Proveedores> getReporte(int idProveedor)
        {
            return ReportesProveedores.Where(r => r.IdProveedor == idProveedor).ToList();
        }
    }
}
