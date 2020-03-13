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
    public class Proveedor : Coneccion
    {
        private List<Proveedores> proveedores, proveedores1;
        private List<Reportes_Proveedores> reporte;
        public List<Proveedores> getProveedores()
        {

            return Proveedor.ToList();
        }
        public List<Reportes_Proveedores> getReporte(int idProveedor)
        {
            return ReportesProveedores.Where(r => r.IdProveedor == idProveedor).ToList();
        }
        public List<Proveedores> insertProveedor(string proveedor, string telefono, string email)
        {
            int pos, idProveedor;
            proveedores = Proveedor.Where(p => p.Telefono == telefono || p.Email == email).ToList();
            if(0 == proveedores.Count)
            {
                Proveedor.Value(p => p.Proveedor, proveedor)
                    .Value(p => p.Telefono, telefono)
                    .Value(p => p.Email, email)
                    .Insert();
                List<Proveedores> pd = getProveedores();
                pos = pd.Count;
                pos--;
                idProveedor = pd[pos].IdProveedor;
                ReportesProveedores
                    .Value(r => r.IdProveedor, idProveedor)
                    .Value(r => r.SaldoActual, "$0.00")
                    .Value(r => r.FechaActual, "Sin fecha")
                    .Value(r => r.UltimoPago, "$0.00")
                    .Value(r => r.FechaPago, "Sin fecha")
                    .Insert();
            }
            return proveedores;
        }
       public void searchProveedor(DataGridView dataGridView, string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Proveedores> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if (campo == "")
            {
                query = Proveedor.ToList();
            }
            else
            {
                query = Proveedor.Where(p => p.Proveedor.StartsWith(campo) || p.Email.StartsWith(campo) || p.Telefono.StartsWith(campo));
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;

            dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            
        }

        public List<Proveedores> updateProveedor(string proveedor, string telefono, string email, int idproveedor)
        {
            proveedores = Proveedor.Where(p => p.Telefono == telefono).ToList();
            proveedores1 = Proveedor.Where(p => p.Email == email).ToList();
            List<Proveedores> list = proveedores.Union(proveedores1).ToList();
            if (2 == list.Count)
            {
                if (idproveedor == proveedores[0].IdProveedor && idproveedor == proveedores1[0].IdProveedor)
                {
                    udate();
                }
            }
            else
            {
                if (0 == list.Count)
                {
                    udate();
                }
                else
                {
                    if ( 0 != proveedores.Count)
                    {
                        if (idproveedor == proveedores[0].IdProveedor)
                        {
                            udate();
                        }
                    }
                    if (0 != proveedores1.Count)
                    {
                        if (idproveedor == proveedores1[0].IdProveedor)
                        {
                            udate();
                        }
                    }
                }
            }
            void udate()
            {
                Proveedor.Where(p => p.IdProveedor == idproveedor)
                    .Set(p => p.Proveedor, proveedor)
                    .Set(p => p.Telefono, telefono)
                    .Set(p => p.Email, email)
                    .Update();
                list.Clear();
            }
            return list;
        }

        public void deleteProveedor(int idProveedor, int idRegistro)
        {
            ReportesProveedores.Where(p => p.IdRegistro == idRegistro).Delete();
            Proveedor.Where(p => p.IdProveedor == idProveedor).Delete();
        }

        public void getProveedorReporte(DataGridView dataGridView, int idProveedor)
        {
            var query = from p in Proveedor
                        join r in ReportesProveedores
                        on p.IdProveedor equals r.IdProveedor
                        where p.IdProveedor == idProveedor
                        select new
                        {
                            r.IdRegistro,
                            p.Proveedor,
                            r.SaldoActual,
                            r.FechaActual,
                            r.UltimoPago,
                            r.FechaPago
                        };
            dataGridView.DataSource = query.ToList();
            dataGridView.Columns[0].Visible = false;
        }

        public void updateReporte(string deudaActual, string ultimoPago, int idProveedor)
        {
            
            string fecha = System.DateTime.Now.ToString("dd/MMM/yyy");
            reporte = getReporte(idProveedor);
            ReportesProveedores.Where(r => r.IdRegistro == reporte[0].IdRegistro)
                .Set(r => r.IdProveedor, reporte[0].IdProveedor)
                .Set(r => r.SaldoActual, "$" + deudaActual)
                .Set(r => r.FechaActual, fecha)
                .Set(r => r.UltimoPago, "$" + ultimoPago)
                .Set(r => r.FechaPago, fecha)
                .Update();
        }
    }
}
