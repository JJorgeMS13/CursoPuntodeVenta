using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LinqToDB;
using Punto_de_ventas.Conexion;
using Punto_de_ventas.Models;

namespace Punto_de_ventas.ModelClass
{
   public class Cliente : Coneccion
    {
        List<reportes_clientes> reporte;
        public List<Clientes> getClientes()
        {
            var query = from c in Cliente
                        select c;
            return query.ToList();
        }
        public void insertCliente( string id, string nombre, string apellido, string direccion, string telefono)
        {
            int pos, idCliente;
            using (var db = new Coneccion())
            {
                db.Insert(new Clientes() {
                    ID = id,
                    Nombre = nombre,
                    Apellido = apellido,
                    Direccion = direccion,
                    Telefono = telefono
                }
                );
                List<Clientes> cliente = getClientes();
                pos = cliente.Count;
                pos--;
                idCliente = cliente[pos].IdCliente;
                db.Insert(new reportes_clientes()
                {
                    IdCliente = idCliente,
                    SaldoActual = "$0.00",
                    FechaActual = "sin fecha",
                    UltimoPago = "$0.00",
                    FechaPago = "sin fecha",
                    ID = id
                });
            }
        }
        public void searchCliente(DataGridView dataGridView ,string campo, int num_pagina, int reg_por_pagina)
        {
            IEnumerable<Clientes> query;
            int inicio = (num_pagina - 1) * reg_por_pagina;
            if(campo == "")
            {
                query = from c in Cliente select c;
            }
            else
            {
                query = from c in Cliente
                        where c.ID.StartsWith(campo) || c.Nombre.StartsWith(campo) || c.Apellido.StartsWith(campo)
                        select c;
            }
            dataGridView.DataSource = query.Skip(inicio).Take(reg_por_pagina).ToList();
            dataGridView.Columns[0].Visible = false;

            dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dataGridView.Columns[5].DefaultCellStyle.BackColor = Color.WhiteSmoke;
        }
        public void getClienteReporte(DataGridView dataGridView, int idCliente)
        {
            var query = from c in Cliente
                        join r in Reportes_Clientes
                        on c.IdCliente equals r.IdCliente
                        where c.IdCliente == idCliente
                        select new
                        {
                            r.IdCliente,
                            c.Nombre,
                            c.Apellido,
                            r.SaldoActual,
                            r.FechaActual,
                            r.UltimoPago,
                            r.FechaPago
                        };
            dataGridView.DataSource = query.ToList();
            dataGridView.Columns[0].Visible = false;
        }
        public void updateCliente(string id, string nombre, string apellido, string direccion, string telefono, int idCliente)
        {
            Cliente.Where(c => c.IdCliente == idCliente)
                .Set(c => c.ID , id)
                .Set(c => c.Nombre, nombre)
                .Set(c => c.Apellido, apellido)
                .Set(c => c.Direccion, direccion)
                .Set(c => c.Telefono, telefono)
                .Update();
            reporte = getReporte(idCliente);
            Reportes_Clientes.Where(r => r.IdRegistro == reporte[0].IdRegistro)
                .Set(r => r.IdCliente, reporte[0].IdCliente)
                .Set(r => r.SaldoActual, reporte[0].SaldoActual)
                .Set(r => r.FechaActual, reporte[0].FechaActual)
                .Set(r => r.UltimoPago, reporte[0].UltimoPago)
                .Set(r => r.FechaPago, reporte[0].FechaPago)
                .Set(r => r.ID, id)
                .Update();
        }
        public List<reportes_clientes> getReporte(int idCliente)
        {
            return Reportes_Clientes.Where(r => r.IdCliente == idCliente).ToList();
        }
        public void deleteCliente(int idCliente, int idRegistro)
        {
            Reportes_Clientes.Where(r => r.IdRegistro == idRegistro).Delete();
            Cliente.Where(c => c.IdCliente == idCliente).Delete();
        }
        public void updateReporte(string deudaActual, string ultimoPago, int idCliente)
        {
            
            string fecha = System.DateTime.Now.ToString("dd/MMM/yyy");
            reporte = getReporte(idCliente);
            Reportes_Clientes.Where(r => r.IdRegistro == reporte[0].IdRegistro)
                .Set(r => r.IdCliente, reporte[0].IdCliente)
                .Set(r => r.SaldoActual, "$"+ deudaActual)
                .Set(r => r.FechaActual, fecha)
                .Set(r => r.UltimoPago, "$" + ultimoPago)
                .Set(r => r.FechaPago, fecha)
                .Set(r => r.ID, reporte[0].ID)
                .Update();
        }
    }
}
