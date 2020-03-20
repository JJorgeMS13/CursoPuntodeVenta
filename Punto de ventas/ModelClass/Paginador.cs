using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_ventas.ModelClass
{
   public class Paginador
    {
        private DataGridView dataGridView;
        private Label label;
        private static int maxReg, pageSize = 2, pageCount, numPagi = 1;
        private int paginas, res;

        public Paginador(DataGridView dataGridView, Label label,int paginas, int res)
        {
            this.dataGridView = dataGridView;
            this.label = label;
            this.paginas = paginas;
            this.res = res;
            cargarDatos();
        }
        private void cargarDatos()
        {
            switch (paginas)
            {
                case 1:
                    if (res == 0)
                    {
                        numPagi = 1;
                        ClassModel.numClientes = ClassModel.cliente.getClientes();
                        ClassModel.cliente.searchCliente(dataGridView, "", 1, pageSize);
                        maxReg = ClassModel.numClientes.Count();
                    }
                    break;
                case 2:
                    if (res == 0)
                    {
                        numPagi = 1;
                        ClassModel.numProveedor = ClassModel.proveedores.getProveedores();
                        ClassModel.proveedores.searchProveedor(dataGridView, "", 1, pageSize);
                        maxReg = ClassModel.numProveedor.Count();
                    }
                    break;
                case 4:
                    if (res == 0)
                    {
                         numPagi = 1;
                        ClassModel.listProductos = ClassModel.producto.getListProductos();
                        ClassModel.producto.searchProducto(dataGridView,"",1,pageSize);
                        maxReg = ClassModel.listProductos.Count();
                    }
                    break;
                case 5:
                    if (res == 0)
                    {
                        numPagi = 1;
                        ClassModel.numTempoCompras = ClassModel.compra.getTempo_Compras();
                        ClassModel.compra.searchCompras(dataGridView,"",1,pageSize);
                        maxReg = ClassModel.numTempoCompras.Count();
                    }
                    break;
                default:
                    break;
            }
            pageCount = (maxReg / pageSize);
            //Ajuste el número de páginas si la  última página contiene una parte de la página.
            if ((maxReg % pageSize) > 0)
            {
                pageCount += 1;
            }
            label.Text = "Paginas " + "1" + "/ " + pageCount.ToString();
        }

        public void primero()
        {
            numPagi = 1;
            label.Text = "Paginas " + numPagi.ToString() + "/ " + pageCount.ToString();
            switch (paginas)
            {
                case 1:
                    ClassModel.cliente.searchCliente(dataGridView,"",1,pageSize);
                    break;
                case 2:
                    ClassModel.proveedores.searchProveedor(dataGridView,"",1,pageSize);
                    break;
                case 4:
                    ClassModel.producto.searchProducto(dataGridView,"",1,pageSize);
                    break;
                case 5:
                    ClassModel.compra.searchCompras(dataGridView,"",1,pageSize);
                    break;

                default:
                    break;
            }
        }

        public void anterior()
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label.Text = "Paginas " + numPagi.ToString() + "/ " + pageCount.ToString();
                switch (paginas)
                {
                    case 1:
                        ClassModel.cliente.searchCliente(dataGridView,"",numPagi, pageSize);
                        break;
                    case 2:
                        ClassModel.proveedores.searchProveedor(dataGridView,"",numPagi,pageSize);
                        break;
                    case 4:
                        ClassModel.producto.searchProducto(dataGridView,"",numPagi,pageSize);
                        break;
                    case 5:
                        ClassModel.compra.searchCompras(dataGridView,"",numPagi,pageSize);
                        break;
                    default:
                        break;
                }
            }
        }
        public void siguiente()
        {
            if (numPagi == pageCount)
            {
                numPagi -= 1;
            }
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label.Text = "Paginas " + numPagi.ToString() + "/ " + pageCount.ToString();
                switch (paginas)
                {
                    case 1:
                        ClassModel.cliente.searchCliente(dataGridView,"",numPagi,pageSize);
                        break;
                    case 2:
                        ClassModel.proveedores.searchProveedor(dataGridView,"",numPagi,pageSize);
                        break;
                    case 4:
                        ClassModel.producto.searchProducto(dataGridView,"",numPagi,pageSize);
                        break;
                    case 5:
                        ClassModel.compra.searchCompras(dataGridView,"",numPagi,pageSize);
                        break;
                    default:
                        break;
                }
            }
        }
        public void ultimo()
        {
            numPagi = pageCount;
            label.Text = "Paginas " + numPagi.ToString() + "/ " + pageCount.ToString();
            switch (paginas)
            {
                case 1:
                    ClassModel.cliente.searchCliente(dataGridView,"",numPagi,pageSize);
                    break;
                case 2:
                    ClassModel.proveedores.searchProveedor(dataGridView,"",numPagi,pageSize);
                    break;
                case 4:
                    ClassModel.producto.searchProducto(dataGridView,"",numPagi,pageSize);
                    break;
                case 5:
                    ClassModel.compra.searchCompras(dataGridView,"",numPagi,pageSize);
                    break;
                default:
                    break;
            }
        }
    }
}
