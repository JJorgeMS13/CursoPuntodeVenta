using Punto_de_ventas.ModelClass;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Punto_de_ventas
{
    public partial class Form1 : Form
    {

        /*************************************************************
         *                                                           *
         *            VARIABLES A UTILIZAR                           *
         *                                                           *    
         *************************************************************/
        #region
           private string 
            rutaOrigen = string.Empty, 
            nombreFoto, 
            rutaDestino;
           private string 
            accion = "insert",
            deudaActual, 
            pago,
            role,
            usuario,
            proveedor,
            saldoProveedor;
           private int 
            paginas = 4,
            pageSize = 2,
            maxReg,
            pageCount,
            numPagi = 1,
            idCliente = 0,
            idUsuario,
            cantidad,
            idProducto,
            funcion;
           private int 
            IdRegistro = 0,
            idProveedor,
            idDpto = 0,
            idCat = 0,
            idCompras = 0,
            idProveedorCp = 0,
            caja;
           private string fecha = DateTime.Now.ToString("dd/MMM/yyy");
           private string hora = DateTime.Now.ToString("hh:mm:ss");
           private String precioCompra;
           private GroupBox groupBox;
        private List<Label> labels = new List<Label>();
        #endregion
        public Form1(List<Usuarios> listUsuario, List<Cajas> listCaja)
        {
            InitializeComponent();
            ToolTip etiqueta = new ToolTip();
            etiqueta.SetToolTip(button_Ventas, "Para acceso rapido presione 'F1'");
            etiqueta.SetToolTip(button_Clientes, "Para acceso rapido presione 'F2'");
            etiqueta.SetToolTip(button_Proveedores, "Para acceso rapido presione 'F3'");
            etiqueta.SetToolTip(button_Productos, "Para acceso rapido presione 'F4'");
            etiqueta.SetToolTip(button_Dpto, "Para acceso rapido presione 'F5'");
            etiqueta.SetToolTip(button_Compras, "Para acceso rapido presione 'F6'");
            etiqueta.SetToolTip(button_Config, "Para acceso rapido presione 'F7'");
            etiqueta.SetToolTip(button_ConfUsuario, "Para acceso rapido presione la letra 'U'");
            
            ClassModel.listruta = ClassModel.usuario.ruta(1);
            rutaDestino = Seguridad.DesEncriptar(ClassModel.listruta[0].ruta);
            if (null != listUsuario)
            {
                role = listUsuario[0].Role;
                idUsuario = listUsuario[0].IdUsuario;
                usuario = listUsuario[0].Nombre;
                if ("Admin" == role)
                {
                    label_NombreApe.Text = listUsuario[0].Nombre + " " + listUsuario[0].Apellido;
                    label_Usuario.Text = listUsuario[0].Usuario;
                    label_Rol.Text = listUsuario[0].Role;
                    pictureBox_Foto.ImageLocation = Seguridad.DesEncriptar(listUsuario[0].Foto);
                    pictureBox_Foto.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pictureBox_Foto.SizeMode = PictureBoxSizeMode.Zoom;
                    label_Caja.Text = "0";
                    ClassModel.listUsuario = listUsuario;
                    caja = 0;
                }
                else if ("Vendedor" == listUsuario[0].Role)
                {
                    label_NombreApe.Text = listUsuario[0].Nombre + " " + listUsuario[0].Apellido;
                    label_Usuario.Text = listUsuario[0].Usuario;
                    label_Rol.Text = listUsuario[0].Role;
                    pictureBox_Foto.ImageLocation = listUsuario[0].Foto;
                    pictureBox_Foto.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox_Foto.SizeMode = PictureBoxSizeMode.Zoom;
                    label_Caja.Text = Convert.ToString(listCaja[0].Caja);
                    ClassModel.listCaja = listCaja;
                    ClassModel.listUsuario = listUsuario;
                    caja = listCaja[0].Caja;

                    //Codigo con el cual puedo no mostrar un Pagina del TapControl1 para un determinado usuario
                    button_Proveedores.Visible = false;
                    button_Productos.Visible = false;
                    button_Dpto.Visible = false;
                    button_Compras.Visible = false;
                    button_Config.Visible = false;
                    tabControl1.TabPages.Remove(tabPage3);
                    tabControl1.TabPages.Remove(tabPage4);
                    tabControl1.TabPages.Remove(tabPage5);
                    tabControl1.TabPages.Remove(tabPage6);
                    tabControl1.TabPages.Remove(tabPage7);
                    tabControl1.TabPages.Remove(tabPage8);
                    tabControl1.TabPages.Remove(tabPage9);
                }
                else
                {
                    label_NombreApe.Text = listUsuario[0].Nombre + " " + listUsuario[0].Apellido;
                    label_Usuario.Text = listUsuario[0].Usuario;
                    label_Rol.Text = listUsuario[0].Role;
                    pictureBox_Foto.ImageLocation = listUsuario[0].Foto;
                    pictureBox_Foto.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox_Foto.SizeMode = PictureBoxSizeMode.Zoom;
                    label_Caja.Text = Convert.ToString(listCaja[0].Caja);
                    ClassModel.listCaja = listCaja;
                    ClassModel.listUsuario = listUsuario;
                    caja = listCaja[0].Caja;
                }
            }
            /***************************************************************
            *                                                             *   
            *                CODIGO DEL CLIENTE                           *
            *                                                             *
            *                                                             *
            **************************************************************/
            #region
            if (role != "Admin")
            {
                radioButton_PagosDeudas.Enabled = false;
            }
            radioButton_IngresarCliente.Checked = true;
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            ClassModel.cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
            ClassModel.cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);
            #endregion
            /***************************************************************
            *                                                             *   
            *                CODIGO DEL PROVEEDOR                         *
            *                                                             *
            *                                                             *
            **************************************************************/
            #region
            if (role != "Admin")
            {
                radioButton_IngresarPagoPd.Enabled = false;
            }
            else if (role == "Admin" || role == "Almacenero")
            {
                radioButton_IngresarPd.Checked = true;
                radioButton_IngresarPd.ForeColor = Color.DarkCyan;
                ClassModel.proveedores.getProveedorReporte(dataGridViewPd_Reportes, idProveedor);
            }

            #endregion
            /***************************************************************
            *                                                             *   
            *                CODIGO DEL DEPARTAMENTO                      *
            *                                                             *
            *                                                             *
            **************************************************************/
            #region
            radioButton_Dpto.ForeColor = Color.DarkCyan;
            textBox_Cat.ReadOnly = true;

            #endregion
            /***************************************************************
             *                                                             *   
             *                CODIGO DE COMPRAS                            *
             *                                                             *
             *                                                             *
             **************************************************************/
            #region
            ClassModel.compra.searchProveedor(dataGridViewCP_Proveedores, "");
            #endregion
            /***************************************************************
             *                                                             *   
             *                CODIGO DE PRODUCTOS                          *
             *                                                             *
             *                                                             *
             **************************************************************/
            #region

            #endregion
            /***************************************************************
             *                                                             *   
             *                CODIGO DE VENTAS.                            *
             *                                                             *
             *                                                             *
             **************************************************************/
            #region
            labels.Add(label_Deuda);
            labels.Add(label_ReciboDeuda);
            labels.Add(label_ReciboDeudaTotal);
            labels.Add(label_ReciboNombre);
            labels.Add(label_ReciboDeudaAnterior);
            labels.Add(label_ReciboUltimoPago);
            labels.Add(label_ReciboFecha);

            labels.Add(label_Pago);
            labels.Add(label_MensajeCliente);
            button_Ventas.Enabled = false;
            restablacerVetas();
            #endregion
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Estas seguro de salir del sistema?", "Cerrar sesión!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClassModel.login.Visible = true;
                int idUsuario = ClassModel.listUsuario[0].IdUsuario;
                string nombre = ClassModel.listUsuario[0].Nombre;
                string apellido = ClassModel.listUsuario[0].Apellido;
                string user = ClassModel.listUsuario[0].Usuario;

                if (role == "Admin")
                {
                    ClassModel.caja.insertCajastemporal(idUsuario, nombre, apellido, user, role, 0, 0, false, hora, fecha);
                }
                else
                {
                    int idCaja = ClassModel.listCaja[0].IdCaja;
                    int cajas = ClassModel.listCaja[0].Caja;
                    ClassModel.caja.updateCaja(idCaja, true);
                    ClassModel.caja.insertCajastemporal(idUsuario, nombre, apellido, user, role, idCaja, cajas, false, hora, fecha);
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            ClassModel.imprimir.printDocument(e, groupBox);
        }
        /***************************************************************
         *                                                             *   
         *                CODIGO Paginador                             *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void cargarDatos()
        {
            switch (paginas)
            {
                case 1:
                    ClassModel.numClientes = ClassModel.cliente.getClientes();
                    ClassModel.cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
                    maxReg = ClassModel.numClientes.Count();
                    break;
                case 2:
                    ClassModel.numProveedor = ClassModel.proveedores.getProveedores();
                    ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, "", 1, pageSize);
                    maxReg = ClassModel.numProveedor.Count();
                    break;
                case 5:
                    ClassModel.numTempoCompras = ClassModel.compra.getTempo_Compras();
                    ClassModel.compra.searchCompras(dataGridView_ComprasProductos,"",1,pageSize);
                    maxReg = ClassModel.numTempoCompras.Count();
                    break;
                default:
                    break;
            }
            pageCount = (maxReg / pageSize);
            Console.WriteLine(pageCount);
            if ((maxReg % pageSize) > 0)
            {
                pageCount += 1;
            }
            switch (paginas)
            {
                case 1:
                    label_PaginasCliente.Text = "Paginas " + "1 " + "de " + pageCount.ToString();
                    break;
                case 2:
                    labelPd_Paginas.Text = "Paginas " + "1 " + "de " + pageCount.ToString();
                    break;
                case 5:
                    label_PaginasCompras.Text = "Paginas " + "1 " + "de " + pageCount.ToString();
                    break;
                default:
                    break;
            }
          
        }

        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DEL CLIENTE                           *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Clientes_Click(object sender, EventArgs e)
        {
            botonMenuClientes();
        }
        private void botonMenuClientes()
        {
            paginas = 1;
            restablecerCliente();
            //Llamamos a la pagina numero 1 del tabControll
            tabControl1.SelectedIndex = 1;
            button_Clientes.Enabled = false;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Compras.Enabled = true;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = true;
            button_Config.Enabled = true;
        }
        private void RadioButton_IngresarCliente_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            radioButton_PagosDeudas.ForeColor = Color.Black;
            textBox_Id.ReadOnly = false;
            textBox_Nombre.ReadOnly = false;
            textBox_Apellido.ReadOnly = false;
            textBox_Direccion.ReadOnly = false;
            textBox_Telefono.ReadOnly = false;
            textBox_PagoscCliente.ReadOnly = true;
            label_PagoCliente.Text = "Pagos de Deudas";
            label_PagoCliente.ForeColor = Color.DarkCyan;
        }
        private void RadioButton_PagosDeudas_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_PagosDeudas.ForeColor = Color.DarkCyan;
            radioButton_IngresarCliente.ForeColor = Color.Black;
            textBox_Id.ReadOnly = true;
            textBox_Nombre.ReadOnly = true;
            textBox_Apellido.ReadOnly = true;
            textBox_Direccion.ReadOnly = true;
            textBox_Telefono.ReadOnly = true;
            textBox_PagoscCliente.ReadOnly = false;
        }
        private void TextBox_Id_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Id.Text == "")
            {
                label_Id.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Id.Text = "ID";
                label_Id.ForeColor = Color.Green;
            }
        }
        private void TextBox_Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberKeyPress(e);
        }
        private void TextBox_Nombre_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Nombre.Text == "")
            {
                label_Nombre.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Nombre.Text = "Nombre Completo";
                label_Nombre.ForeColor = Color.Green;
            }
        }
        private void TextBox_Nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }
        private void TextBox_Apellido_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Apellido.Text == "")
            {
                label_Apellido.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Apellido.Text = "Apellidos";
                label_Apellido.ForeColor = Color.Green;
            }
        }
        private void TextBox_PagoscCliente_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView_ClienteReporte.CurrentRow == null)
            {
                label_PagoCliente.Text = "Seleccione el cliente";
                label_PagoCliente.ForeColor = Color.Red;
                textBox_PagoscCliente.Text = "";
            }
            else
            {
                if (textBox_PagoscCliente.Text != "")
                {
                    label_PagoCliente.Text = "Pagos de deudas";
                    label_PagoCliente.ForeColor = Color.LightSlateGray;
                    string deuda1;
                    Decimal deuda2, deuda3, deudaTotal;
                    deuda1 = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[3].Value);
                    deuda1 = deuda1.Replace("$", "");
                    deuda2 = Convert.ToDecimal(deuda1);
                    deuda3 = Convert.ToDecimal(textBox_PagoscCliente.Text);

                    deudaTotal = deuda2 - deuda3;
                    deudaActual = string.Format("{0: #,###,###,##0.00####}", deudaTotal);
                    pago = string.Format("{0: #,###,###,##0.00####}", textBox_PagoscCliente.Text);
                }
            }
        }
        private void TextBox_PagoscCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBox_PagoscCliente, e);
        }
        private void TextBox_Apellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }
        private void TextBox_Direccion_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Direccion.Text == "")
            {
                label_Direccion.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Direccion.Text = "Dirección";
                label_Direccion.ForeColor = Color.Green;
            }
        }
        private void TextBox_Direccion_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void TextBox_Telefono_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Telefono.Text == "")
            {
                label_Telefono.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Telefono.Text = "Telefono";
                label_Telefono.ForeColor = Color.Green;
            }
        }
        private void TextBox_Telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberKeyPress(e);
        }
        private void Button_GuardarCliente_Click(object sender, EventArgs e)
        {
            if (radioButton_IngresarCliente.Checked)
            {
                guardarCliente();
            }
            else
            {
                guardarPago();
            }
        }
        private void Button_Cancelar_Click(object sender, EventArgs e)
        {
            restablecerCliente();
        }
        private void Button_EliminarClientes_Click(object sender, EventArgs e)
        {
            if (idCliente > 0)
            {
                if (MessageBox.Show("Estas Seguro de eliminar este registro?", "Eliminar Registro",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClassModel.cliente.deleteCliente(idCliente, IdRegistro);
                    restablecerCliente();
                }
            }
        }
        private void guardarCliente()
        {
            if (textBox_Id.Text == "")
            {
                label_Id.Text = "Ingrese el ID del cliente";
                label_Id.ForeColor = Color.Red;
                textBox_Id.Focus();
            }
            else
            {
                if (textBox_Nombre.Text == "")
                {
                    label_Nombre.Text = "Ingrese el nombre del Cliente";
                    label_Nombre.ForeColor = Color.Red;
                    textBox_Nombre.Focus();
                }
                else
                {
                    if (textBox_Apellido.Text == "")
                    {
                        label_Apellido.Text = "Ingrese el apellido del Cliente";
                        label_Apellido.ForeColor = Color.Red;
                        textBox_Apellido.Focus();
                    }
                    else
                    {
                        if (textBox_Direccion.Text == "")
                        {
                            label_Direccion.Text = "Ingrese la dirección del Cliente";
                            label_Direccion.ForeColor = Color.Red;
                            textBox_Direccion.Focus();
                        }
                        else
                        {
                            if (textBox_Telefono.Text == "")
                            {
                                label_Telefono.Text = "Ingrese el telefono del Cliente";
                                label_Telefono.ForeColor = Color.Red;
                                textBox_Telefono.Focus();
                            }
                            else
                            {
                                string ID = textBox_Id.Text;
                                string Nombre = textBox_Nombre.Text;
                                string Apellido = textBox_Apellido.Text;
                                string Direccion = textBox_Direccion.Text;
                                string Telefono = textBox_Telefono.Text;
                                if (accion == "insert")
                                {
                                    ClassModel.cliente.insertCliente(ID, Nombre, Apellido, Direccion, Telefono);
                                }
                                if (accion == "update")
                                {
                                    if (role == "Admin")
                                    {
                                        if (MessageBox.Show("Estas seguro de Actualizar el registro", "Actualizar Registro",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            ClassModel.cliente.updateCliente(ID, Nombre, Apellido, Direccion, Telefono, idCliente);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("No cuenta con el permiso requerido");
                                    }
                                }
                                restablecerCliente();
                            }
                        }

                    }
                }
            }
        }
        private void Button_PrimerosClientes_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + " de " + pageCount.ToString();
            ClassModel.cliente.searchCliente(dataGridView_Cliente, "", 1, pageSize);
        }
        private void Button_AnteriosClientes_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.cliente.searchCliente(dataGridView_Cliente, "", numPagi, pageSize);
            }
        }
        private void Button_SiguientesClientes_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.cliente.searchCliente(dataGridView_Cliente, "", numPagi, pageSize);
            }
        }
        private void Button_UltimosClientes_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            numPagi = pageCount;
            label_PaginasCliente.Text = "Pagina " + numPagi.ToString() + " de " + pageCount.ToString();
            ClassModel.cliente.searchCliente(dataGridView_Cliente, "", pageCount, pageSize);
        }
        private void restablecerCliente()
        {
            numPagi = 1;
            cargarDatos();
            textBox_Id.Text = "";
            textBox_Nombre.Text = "";
            textBox_Apellido.Text = "";
            textBox_Direccion.Text = "";
            textBox_Telefono.Text = "";
            textBox_PagoscCliente.Text = "";

            textBox_Id.Focus();
            textBox_BuscarCliente.Text = "";
            label_Id.ForeColor = Color.LightSlateGray;
            label_Nombre.ForeColor = Color.LightSlateGray;
            label_Apellido.ForeColor = Color.LightSlateGray;
            label_Direccion.ForeColor = Color.LightSlateGray;
            label_Telefono.ForeColor = Color.LightSlateGray;
            label_PagoCliente.ForeColor = Color.LightSlateGray;
            label_PagoCliente.Text = "Pago de deudas";
            radioButton_IngresarCliente.Checked = true;
            radioButton_IngresarCliente.ForeColor = Color.DarkCyan;
            accion = "insert";
            idCliente = 0;
            IdRegistro = 0;
            label_NombreRB.Text = "Nombre";
            label_ApellidoRB.Text = "Apellido";
            label_ClienteDA.Text = "$ 0.0";
            label_ClienteUP.Text = "$ 0.0";
            label_FechaPG.Text = "Fecha";
            ClassModel.cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);
        }
        private void DataGridView_Cliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Cliente.Rows.Count != 0)
            {
                dataGridViewCliente();
            }
        }
        private void DataGridView_Cliente_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Cliente.Rows.Count != 0)
            {
                dataGridViewCliente();
            }
        }
        private void dataGridViewCliente()
        {
            accion = "update";
            idCliente = Convert.ToUInt16(dataGridView_Cliente.CurrentRow.Cells[0].Value);
            textBox_Id.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[1].Value);
            textBox_Nombre.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[2].Value);
            textBox_Apellido.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[3].Value);
            textBox_Direccion.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[4].Value);
            textBox_Telefono.Text = Convert.ToString(dataGridView_Cliente.CurrentRow.Cells[5].Value);

            ClassModel.cliente.getClienteReporte(dataGridView_ClienteReporte, idCliente);

            IdRegistro = Convert.ToInt16(dataGridView_ClienteReporte.CurrentRow.Cells[0].Value);
            label_NombreRB.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[1].Value);
            label_ApellidoRB.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[2].Value);
            label_ClienteDA.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[3].Value);
            label_ClienteUP.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[5].Value);
            label_FechaPG.Text = Convert.ToString(dataGridView_ClienteReporte.CurrentRow.Cells[4].Value);
        }
        private void TextBox_BuscarCliente_TextChanged(object sender, EventArgs e)
        {
            ClassModel.cliente.searchCliente(dataGridView_Cliente, textBox_BuscarCliente.Text, 1, pageSize);
        }
        private void guardarPago()
        {
            if (textBox_PagoscCliente.Text == "")
            {
                label_PagoCliente.Text = "Ingrese el pago";
                label_PagoCliente.ForeColor = Color.Red;
                textBox_PagoscCliente.Focus();
            }
            else
            {
                ClassModel.cliente.updateReporte(deudaActual, pago, idCliente);
                restablecerCliente();
            }
        }
        private void Button_ImprCliente_Click(object sender, EventArgs e)
        {
            groupBox = groupBox_ReciboCliente;
            printDocument1.Print();
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DEL Proveedor                         *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Proveedores_Click(object sender, EventArgs e)
        {
            botonMenuProveedores();
        }
        private void botonMenuProveedores()
        {
            tabControl1.SelectedIndex = 2;
            restablecerProveedor();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Compras.Enabled = true;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = false;
            button_Config.Enabled = true;
        }
        private void RadioButton_IngresarPd_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_IngresarPd.ForeColor = Color.DarkCyan;
            radioButton_IngresarPd.ForeColor = Color.Black;
            textBoxPd_Proveedor.ReadOnly = false;
            textBoxPd_Telefono.ReadOnly = false;
            textBoxPd_Email.ReadOnly = false;
            textBoxPd_PagoDeuda.ReadOnly = true;
            labelPd_PagoDeuda.Text = "Pagos de deudas";
            labelPd_PagoDeuda.ForeColor = Color.DarkCyan;
        }
        private void RadioButton_IngresarPagoPd_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_IngresarPd.ForeColor = Color.Black;
            radioButton_IngresarPd.ForeColor = Color.DarkCyan;
            textBoxPd_Proveedor.ReadOnly = true;
            textBoxPd_Telefono.ReadOnly = true;
            textBoxPd_Email.ReadOnly = true;
            textBoxPd_PagoDeuda.ReadOnly = false;
        }
        private void TextBoxPd_Proveedor_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPd_Proveedor.Text == "")
            {
                labelPd_Proveedor.ForeColor = Color.LightSlateGray;
                //labelPd_Proveedor.Text = "Ingrese un proveedor";
            }
            else
            {
                labelPd_Proveedor.Text = "Proveedor";
                labelPd_Proveedor.ForeColor = Color.Green;
            }
        }
        private void TextBoxPd_Telefono_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPd_Telefono.Text == "")
            {
                labelPd_Telefono.ForeColor = Color.LightSlateGray;
                //labelPd_Proveedor.Text = "Ingrese un proveedor";
            }
            else
            {
                labelPd_Telefono.Text = "Telefono";
                labelPd_Telefono.ForeColor = Color.Green;
            }
        }
        private void TextBoxPd_Telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberKeyPress(e);
        }
        private void TextBoxPd_Email_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPd_Email.Text == "")
            {
                labelPd_Email.ForeColor = Color.LightSlateGray;
                //labelPd_Proveedor.Text = "Ingrese un proveedor";
            }
            else
            {
                labelPd_Email.Text = "Email";
                labelPd_Email.ForeColor = Color.Green;
            }
        }
        private void TextBoxPd_PagoDeuda_TextChanged(object sender, EventArgs e)
        {
            if (dataGridView_Proveedores.CurrentCell == null)
            {
                labelPd_PagoDeuda.Text = "Seleccione el proveedor";
                labelPd_PagoDeuda.ForeColor = Color.Red;
                textBoxPd_PagoDeuda.Text = "";
            }
            else
            {
                if (textBoxPd_PagoDeuda.Text != "")
                {
                    labelPd_PagoDeuda.Text = "Pagos de deudas";
                    labelPd_PagoDeuda.ForeColor = Color.LightSlateGray;
                    String deuda1;
                    Decimal deuda2, deuda3, deudaTotal;
                    deuda1 = Convert.ToString(dataGridViewPd_Reportes.CurrentRow.Cells[2].Value);
                    deuda1 = deuda1.Replace("$", "");
                    deuda2 = Convert.ToDecimal(deuda1);
                    deuda3 = Convert.ToDecimal(textBoxPd_PagoDeuda.Text);

                    deudaTotal = deuda2 - deuda3;
                    deudaActual = String.Format("{0: #,###,###,##0.00####}", deudaTotal);
                    deudaActual = deudaActual.Replace(" ", "");
                    pago = String.Format("{0: #,###,###,##0.00####}", textBoxPd_PagoDeuda.Text);
                }
            }
        }
        private void guardarPagoPd()
        {
            if (textBoxPd_PagoDeuda.Text == "")
            {
                labelPd_PagoDeuda.Text = "Ingrese el pago";
                labelPd_PagoDeuda.ForeColor = Color.Red;
                textBoxPd_PagoDeuda.Focus();
            }
            else
            {
                ClassModel.proveedores.updateReporte(deudaActual, pago, idProveedor);
                restablecerProveedor();
            }
        }
        private void TextBoxPd_PagoDeuda_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBoxPd_PagoDeuda, e);
        }
        private void guardarProveedor()
        {
            if (textBoxPd_Proveedor.Text == "")
            {
                labelPd_Proveedor.Text = "Ingrese el nombre del Proveedor";
                labelPd_Proveedor.ForeColor = Color.Red;
                textBoxPd_Proveedor.Focus();
            }
            else
            {
                if (textBoxPd_Telefono.Text == "")
                {
                    labelPd_Telefono.Text = "Ingrese el Telefono del Proveedor";
                    labelPd_Telefono.ForeColor = Color.Red;
                    textBoxPd_Telefono.Focus();
                }
                else
                {
                    if (textBoxPd_Email.Text == "")
                    {
                        labelPd_Email.Text = "Ingrese el Email del Proveedor";
                        labelPd_Email.ForeColor = Color.Red;
                        textBoxPd_Email.Focus();
                    }
                    else
                    {
                        string proveedor = textBoxPd_Proveedor.Text;
                        string telefono = textBoxPd_Telefono.Text;
                        string email = textBoxPd_Email.Text;

                        switch (accion)
                        {
                            case "insert":
                                if (ClassModel.eventos.comprobarFormatoEmail(email))
                                {
                                    var data = ClassModel.proveedores.insertProveedor(proveedor, telefono, email);
                                    if (0 == data.Count)
                                    {
                                        restablecerProveedor();
                                    }
                                    else
                                    {
                                        if (data[0].Telefono == telefono)
                                        {
                                            labelPd_Telefono.Text = "El telefono ya esta registrado";
                                            labelPd_Telefono.ForeColor = Color.Red;
                                            textBoxPd_Telefono.Focus();
                                        }
                                        if (data[0].Email == email)
                                        {
                                            labelPd_Email.Text = "El email ya esta registrado";
                                            labelPd_Email.ForeColor = Color.Red;
                                            textBoxPd_Email.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    labelPd_Email.Text = "Email no valido";
                                    labelPd_Email.ForeColor = Color.Red;
                                }

                                break;
                            case "update":
                                if (role == "Admin")
                                {
                                    var data1 = ClassModel.proveedores.updateProveedor(proveedor, telefono, email, idProveedor);
                                    if (0 == data1.Count)
                                    {
                                        restablecerProveedor();
                                    }
                                    else
                                    {
                                        if (data1[0].IdProveedor != idProveedor)
                                        {
                                            labelPd_Telefono.Text = "El telefono ya esta registrado";
                                            labelPd_Telefono.ForeColor = Color.Red;
                                            textBoxPd_Telefono.Focus();
                                        }
                                        if (2 == data1.Count && data1[1].IdProveedor != idProveedor)
                                        {
                                            labelPd_Email.Text = "El email ya esta registrado";
                                            labelPd_Email.ForeColor = Color.Red;
                                            textBoxPd_Email.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("No cuenta con los permisos requeridos");
                                }
                                break;
                        }
                    }
                }
            }
        }
        private void ButtonPd_Guardar_Click(object sender, EventArgs e)
        {
            if (radioButton_IngresarPd.Checked)
            {
                guardarProveedor();
            }
            else
            {
                guardarPagoPd();
            }
        }
        private void restablecerProveedor()
        {
            paginas = 2;
            accion = "insert";
            cargarDatos();
            textBoxPd_Proveedor.Text = "";
            textBoxPd_Telefono.Text = "";
            textBoxPd_Email.Text = "";
            textBoxPd_PagoDeuda.Text = "";
            textBoxPd_Proveedor.Focus();
            labelPd_Proveedor.ForeColor = Color.LightSlateGray;
            labelPd_Telefono.ForeColor = Color.LightSlateGray;
            labelPd_Email.ForeColor = Color.LightSlateGray;
            idProveedor = 0;
            IdRegistro = 0;
            labelPd_ProveedorR.Text = "Proveedor";
            labelPd_Deuda.Text = "$0.00";
            labelPd_UltimoPago.Text = "$0.00";
            labelPd_FechaPago.Text = "fecha";
            radioButton_IngresarPagoPd.Checked = true;
            radioButton_IngresarPagoPd.ForeColor = Color.DarkCyan;
            ClassModel.proveedores.getProveedorReporte(dataGridViewPd_Reportes, idProveedor);
        }
        private void DataGridView_Proveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Proveedores.Rows.Count != 0)
            {
                dataGridViewProveedor();
            }
        }
        private void DataGridView_Proveedores_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Proveedores.Rows.Count != 0)
            {
                dataGridViewProveedor();
            }
        }
        private void dataGridViewProveedor()
        {
            accion = "update";
            idProveedor = Convert.ToInt16(dataGridView_Proveedores.CurrentRow.Cells[0].Value);
            textBoxPd_Proveedor.Text = Convert.ToString(dataGridView_Proveedores.CurrentRow.Cells[1].Value);
            textBoxPd_Telefono.Text = Convert.ToString(dataGridView_Proveedores.CurrentRow.Cells[3].Value);
            textBoxPd_Email.Text = Convert.ToString(dataGridView_Proveedores.CurrentRow.Cells[2].Value);
            ClassModel.proveedores.getProveedorReporte(dataGridViewPd_Reportes, idProveedor);
            IdRegistro = Convert.ToInt16(dataGridViewPd_Reportes.CurrentRow.Cells[0].Value);
            labelPd_ProveedorR.Text = Convert.ToString(dataGridViewPd_Reportes.CurrentRow.Cells[1].Value);
            labelPd_Deuda.Text = Convert.ToString(dataGridViewPd_Reportes.CurrentRow.Cells[2].Value);
            labelPd_UltimoPago.Text = Convert.ToString(dataGridViewPd_Reportes.CurrentRow.Cells[4].Value);
            labelPd_FechaPago.Text = Convert.ToString(dataGridViewPd_Reportes.CurrentRow.Cells[3].Value);
        }
        private void ButtonPd_Eliminar_Click(object sender, EventArgs e)
        {
            if (idProveedor > 0)
            {
                if (MessageBox.Show("Estas seguro de eliminar el registro", "Eliminar Proveedor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ClassModel.proveedores.deleteProveedor(idProveedor, IdRegistro);
                    restablecerProveedor();
                }
            }
        }
        private void TextBoxPd_Buscar_TextChanged(object sender, EventArgs e)
        {
            ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, textBoxPd_Buscar.Text, 1, pageSize);
        }
        private void ButtonPd_Primero_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            labelPd_Paginas.Text = "Paginas " + numPagi.ToString() + "de " + pageCount.ToString();
            ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, "", 1, pageSize);
        }
        private void ButtonPd_Anterior_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                labelPd_Paginas.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, "", numPagi, pageSize);
            }
        }
        private void ButtonPd_Siguiente_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                labelPd_Paginas.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, "", numPagi, pageSize);
            }
        }
        private void ButtonPd_Ultimo_Click(object sender, EventArgs e)
        {
            numPagi = pageCount;
            labelPd_Paginas.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
            ClassModel.proveedores.searchProveedor(dataGridView_Proveedores, "", pageCount, pageSize);
        }
        private void ButtonPd_Imprimir_Click(object sender, EventArgs e)
        {
            groupBox = groupBoxPd_Recibo;
            printDocument1.Print();
        }
        private void ButtonPd_Cancelar_Click(object sender, EventArgs e)
        {
            restablecerProveedor();
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DEL DEPARTAMENTO                      *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Dpto_Click(object sender, EventArgs e)
        {
            botonMenuDepartamentos();
        }
        private void botonMenuDepartamentos()
        {
            tabControl1.SelectedIndex = 4;
            restablecerDoptoCat();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Compras.Enabled = true;
            button_Proveedores.Enabled = true;
            button_Dpto.Enabled = false;
            button_Config.Enabled = true;
        }
        private void RadioButton_Dpto_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_Cat.ForeColor = Color.LightSlateGray;
            radioButton_Dpto.ForeColor = Color.DarkCyan;
            textBox_Dpto.ReadOnly = false;
            textBox_Dpto.Text = "";
            textBox_Cat.ReadOnly = true;
            label_Cat.Text = "Categoria";
            label_Cat.ForeColor = Color.LightSlateGray;
        }
        private void RadioButton_Cat_CheckedChanged(object sender, EventArgs e)
        {
            radioButton_Cat.ForeColor = Color.DarkCyan;
            radioButton_Dpto.ForeColor = Color.LightSlateGray;
            textBox_Dpto.ReadOnly = true;
            textBox_Cat.Text = "";
            textBox_Cat.ReadOnly = false;
            label_Dpto.Text = "Departamento";
            label_Dpto.ForeColor = Color.LightSlateGray;
        }
        private void TextBox_Dpto_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Dpto.Text == "")
            {
                label_Dpto.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Dpto.Text = "Departamento";
                label_Dpto.ForeColor = Color.DarkCyan;
            }
        }
        private void TextBox_Dpto_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }
        private void TextBox_Cat_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Cat.Text == "")
            {
                label_Cat.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Cat.Text = "Categoria";
                label_Cat.ForeColor = Color.DarkCyan;
            }
        }
        private void TextBox_Cat_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }
        private void Button_CuardarDpto_Click(object sender, EventArgs e)
        {
            if ("Admin" == role)
                GuardarDptoCat();
            else
                MessageBox.Show("No cuenta con los permisos necesarios");

        }
        private void GuardarDptoCat()
        {
            bool valor = true;
            //Departamentos
            if (radioButton_Dpto.Checked)
            {
                if (textBox_Dpto.Text == "")
                {
                    label_Dpto.Text = "Ingrese el Departamento";
                    label_Dpto.ForeColor = Color.Red;
                    textBox_Dpto.Focus();
                }
                else
                {
                    switch (accion)
                    {
                        case "insert":
                            valor = ClassModel.dptocat.insertDptoCat(textBox_Dpto.Text, 0, "dpto");
                            break;
                        case "update":
                            valor = ClassModel.dptocat.updateDptoCat(textBox_Dpto.Text, idDpto, 0, "dpto");
                            break;
                        default:
                            break;
                    }
                    if (valor == false)
                    {
                        label_Dpto.Text = "El departamento ya esta registrado";
                        label_Dpto.ForeColor = Color.Red;
                    }
                    else
                    {
                        restablecerDoptoCat();
                    }
                    
                }
            }
            //Categorias  
            if (radioButton_Cat.Checked)
            {

                if (textBox_Cat.Text == string.Empty)
                {
                    label_Cat.Text = "Ingrese la categoria";
                    label_Cat.ForeColor = Color.Red;
                    textBox_Cat.Focus();
                }
                else
                {
                    if (idDpto != 0)
                    {
                        switch (accion)
                        {
                            case "insert":
                                valor = ClassModel.dptocat.insertDptoCat(textBox_Cat.Text, idDpto, "cat");
                                break;
                            case "update":
                                valor = ClassModel.dptocat.updateDptoCat(textBox_Cat.Text, idDpto, idCat, "cat");
                                break;
                            default:
                                break;
                        }
                        if (valor == false)
                        {
                            label_Cat.Text = "La categoria ya esta registrada";
                            label_Cat.ForeColor = Color.Red;
                        }
                        else
                        {
                            restablecerDoptoCat();
                        }
                    }
                    else
                    {
                        label_Dpto.Text = "Seleccione un departamento";
                        label_Dpto.ForeColor = Color.Red;
                        textBox_Dpto.Focus();
                    }
                }
            }
        }
        private void restablecerDoptoCat()
        {
            idCat = 0;
            idDpto = 0;
            accion = "insert";
            textBox_Dpto.Text = "";
            textBox_Cat.Text = "";
            label_Dpto.Text = "Departamento";
            label_Dpto.ForeColor = Color.LightSlateGray;
            label_Cat.Text = "Categoria";
            label_Cat.ForeColor = Color.LightSlateGray;
            textBox_Dpto.Focus();
            radioButton_Dpto.Checked = true;
            radioButton_Dpto.ForeColor = Color.DarkCyan;
            textBox_Cat.ReadOnly = true;
            ClassModel.dptocat.searchDpto(dataGridView_Dpto, "", 0, 1);
            ClassModel.dptocat.searchDpto(dataGridView_Cat, "", 0, 2);
        }
        private void DataGridView_Dpto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Dpto.Rows.Count != 0)
            {
                dataGridViewDpto();
            }
        }
        private void DataGridView_Dpto_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Dpto.Rows.Count != 0)
            {
                dataGridViewDpto();
            }
        }
        public void dataGridViewDpto()
        {
            if (radioButton_Dpto.Checked)
            {
                accion = "update";
                idDpto = Convert.ToInt16(dataGridView_Dpto.CurrentRow.Cells[0].Value);
                textBox_Dpto.Text = Convert.ToString(dataGridView_Dpto.CurrentRow.Cells[1].Value);
            }
            if (radioButton_Cat.Checked)
            {
                textBox_Cat.Text = "";
                accion = "insert";
                label_Cat.Text = "Categoria";
                label_Cat.ForeColor = Color.DarkCyan;
                idDpto = Convert.ToInt16(dataGridView_Dpto.CurrentRow.Cells[0].Value);
                textBox_Dpto.Text = Convert.ToString(dataGridView_Dpto.CurrentRow.Cells[1].Value);

            }
            ClassModel.dptocat.searchDpto(dataGridView_Cat, "", idDpto, 2);
        }
        private void DataGridView_Cat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Cat.Rows.Count != 0)
            {
                dataGridViewCat();
            }
        }
        private void DataGridView_Cat_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Cat.Rows.Count != 0)
            {
                dataGridViewCat();
            }
        }
        private void dataGridViewCat()
        {
            idCat = Convert.ToInt16(dataGridView_Cat.CurrentRow.Cells[0].Value);
            textBox_Cat.Text = Convert.ToString(dataGridView_Cat.CurrentRow.Cells[1].Value);
            accion = "update";
        }

        private void Button_EliminarDpto_Click(object sender, EventArgs e)
        {
            if ("Admin" == role)
                eliminarDptoCat();
            else
                MessageBox.Show("No cuenta con los permisos necesarios");
        }

        private void eliminarDptoCat()
        {
            if (radioButton_Dpto.Checked)
            {
                if (idDpto > 0)
                {

                    if (MessageBox.Show("Estas seguro de eliminar este departamento?", "Eliminar Departamento",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        ClassModel.dptocat.deleteDptoCat(idDpto, 0, "dpto");
                        restablecerDoptoCat();
                    }
                }
            }
            else
            {
                if (idDpto > 0)
                {

                    if (MessageBox.Show("Estas seguro de eliminar esta categoria?", "Eliminar Categoria",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        ClassModel.dptocat.deleteDptoCat(0, idCat, "cat");
                        restablecerDoptoCat();
                    }
                }
            }
        }
        private void Button_DptoCancelar_Click(object sender, EventArgs e)
        {
            restablecerDoptoCat();
        }
        private void TextBox_BuscarDpto_TextChanged(object sender, EventArgs e)
        {
            ClassModel.dptocat.searchDpto(dataGridView_Dpto, textBox_BuscarDpto.Text, 0, 1);
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DE COMPRAS                            *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Compras_Click(object sender, EventArgs e)
        {
            botonMenuCompras();
        }
        private void botonMenuCompras()
        {
            tabControl1.SelectedIndex = 5;
            restablecerCompras();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Compras.Enabled = true;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = true;
            button_Compras.Enabled = false;
            button_Config.Enabled = true;
        }
        private void TextBox_DescpCompra_TextChanged(object sender, EventArgs e)
        {
            if (textBox_DescpCompra.Text == string.Empty)
            {
                label_DescpCompra.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_DescpCompra.Text = "Descripción";
                label_DescpCompra.ForeColor = Color.DarkCyan;
            }
        }
        private void TextBox_DescpCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }
        private void TextBox_CantidadCompra_TextChanged(object sender, EventArgs e)
        {
            if (textBox_CantidadCompra.Text == string.Empty)
            {
                label_CantidadCompra.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_CantidadCompra.Text = "Cantidad";
                label_CantidadCompra.ForeColor = Color.DarkCyan;
            }
        }
        private void TextBox_CantidadCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberKeyPress(e);
        }
        private void TextBox_PrecioCompra_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrecioCompra.Text == string.Empty)
            {
                label_PrecioCmpra.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_PrecioCmpra.Text = "Precio de Compra";
                label_PrecioCmpra.ForeColor = Color.DarkCyan;
            }
        }
        private void TextBox_PrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBox_PrecioCompra,e);
        }
        private void ButtonCP_Guardar_Click(object sender, EventArgs e)
        {
            if (tabControl_Compras.SelectedIndex == 0)
            {
                guardarComprasTemp();

            }
            else
            {
                label_ImprorteCompras.Text = "$0.00";
                labelCP_Importe2.Text = "$0.00";
                guardarCompras();
                
            }
            
        }
        private void guardarComprasTemp()
        {
            if (textBox_DescpCompra.Text == string.Empty)
            {
                label_DescpCompra.Text = "Ingrese la descripción";
                label_DescpCompra.ForeColor = Color.Red;
                textBox_DescpCompra.Focus();
            }
            else
            {
                if (textBox_CantidadCompra.Text == string.Empty)
                {
                    label_CantidadCompra.Text = "Ingrese la cantidad";
                    label_CantidadCompra.ForeColor = Color.Red;
                    textBox_CantidadCompra.Focus();
                }
                else
                {
                    if (textBox_PrecioCompra.Text == string.Empty)
                    {
                        label_PrecioCmpra.Text = "Ingrese el precio de la compra";
                        label_PrecioCmpra.ForeColor = Color.Red;
                        textBox_PrecioCompra.Focus();
                    }
                    else
                    {
                        string des = textBox_DescpCompra.Text;
                        int cant = Convert.ToInt16(textBox_CantidadCompra.Text);
                        string precio = textBox_PrecioCompra.Text;
                        switch (accion)
                        {
                            case "insert":
                                if (tabControl_Compras.SelectedIndex == 0)
                                {
                                    if (idProveedorCp != 0)
                                    {
                                        ClassModel.compra.guardarTempoCompras(des, cant, precio);
                                        restablecerCompras();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Seleccione un proveedor");
                                    }

                                }
                                break;
                            case "update":
                                if (tabControl_Compras.SelectedIndex == 0)
                                {
                                    if (idProveedorCp != 0)
                                    {
                                        ClassModel.compra.updateTempoCompras(idCompras, des, cant, precio);
                                        restablecerCompras();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Seleccione un proveedor");
                                    }

                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void restablecerCompras()
        {
            textBox_DescpCompra.Text = string.Empty;
            textBox_CantidadCompra.Text = string.Empty;
            textBox_PrecioCompra.Text = string.Empty;
            label_ImprorteCompras.Text = "$0.00";
            labelCP_Importe2.Text = "$0.00";

            numPagi = 1;
            paginas = 5;
            idCompras = 0;
            accion = "insert";
            tabControl_Compras.SelectedIndex = 0;
            textBox_DescpCompra.Text = string.Empty;
            textBox_CantidadCompra.Text = string.Empty;
            textBox_PrecioCompra.Text = string.Empty;
            textBox_BuscarCompras.Text = string.Empty;
            //textBoxCP_BuscarPD.Text = string.Empty;
            label_DescpCompra.Text = "Descripción";
            label_DescpCompra.ForeColor = Color.LightSlateGray;
            label_CantidadCompra.Text = "Cantidad";
            label_CantidadCompra.ForeColor = Color.LightSlateGray;
            label_PrecioCmpra.Text = "Precio de Compra";
            label_PrecioCmpra.ForeColor = Color.LightSlateGray;
            cargarDatos();
            ClassModel.compra.searchCompras(dataGridView_ComprasProductos, "", 1, pageSize);
            ClassModel.compra.importeTempo(label_ImprorteCompras, labelCP_Importe2,labelCP_TotalPagar);
            //ClassModel.compra.searchProveedor(dataGridViewCP_Proveedores,"");
        }
        private void DataGridView_ComprasProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_ComprasProductos.Rows.Count != 0)
            {
                dataGriedViewCompras();
            }
        }

        private void DataGridView_ComprasProductos_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_ComprasProductos.Rows.Count != 0)
            {
                dataGriedViewCompras();
            }
        }

        private void dataGriedViewCompras()
        {
            String precio;
            accion = "update";
            idCompras = Convert.ToInt16(dataGridView_ComprasProductos.CurrentRow.Cells[0].Value);
            textBox_DescpCompra.Text = Convert.ToString(dataGridView_ComprasProductos.CurrentRow.Cells[1].Value);
            textBox_CantidadCompra. Text = Convert.ToString(dataGridView_ComprasProductos.CurrentRow.Cells[2].Value);
            precio = Convert.ToString(dataGridView_ComprasProductos.CurrentRow.Cells[3].Value);
            textBox_PrecioCompra.Text = precio.Replace("$", "");
        }
        private void Button_EliminarCompras_Click(object sender, EventArgs e)
        {
            if (0 < idCompras)
            {
                if (MessageBox.Show("Estas seguro de eliminar esta compra?","Eliminar compra",MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ClassModel.compra.deleteCompras(idCompras);
                    restablecerCompras();
                }
            }
        }
        private void Button_CancelarCompras_Click(object sender, EventArgs e)
        {
            restablecerCompras();
        }
        private void Button_PrimerCompra_Click(object sender, EventArgs e)
        {
            numPagi = 1;
            label_PaginasCompras.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
            ClassModel.compra.searchCompras(dataGridView_ComprasProductos, "", 1, pageSize);
        }
        private void Button_AnteriorCompra_Click(object sender, EventArgs e)
        {
            if (numPagi > 1)
            {
                numPagi -= 1;
                label_PaginasCompras.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.compra.searchCompras(dataGridView_ComprasProductos,"",numPagi,pageSize);
            }
        }
        private void Button_SiguienteCompra_Click(object sender, EventArgs e)
        {
            if (numPagi < pageCount)
            {
                numPagi += 1;
                label_PaginasCompras.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.compra.searchCompras(dataGridView_ComprasProductos, "", numPagi, pageSize);
            }
        }
        private void Button_UltimaCompra_Click(object sender, EventArgs e)
        {

                numPagi = pageCount;
                label_PaginasCompras.Text = "Paginas " + numPagi.ToString() + " de " + pageCount.ToString();
                ClassModel.compra.searchCompras(dataGridView_ComprasProductos, "", numPagi, pageSize);   
        }
        private void TextBox_BuscarCompras_TextChanged(object sender, EventArgs e)
        {
            ClassModel.compra.searchCompras(dataGridView_ComprasProductos,textBox_BuscarCompras.Text,1,pageSize);
        }
        private void TextBoxCP_BuscarPD_TextChanged(object sender, EventArgs e)
        {
            ClassModel.compra.searchProveedor(dataGridViewCP_Proveedores,textBoxCP_BuscarPD.Text);
        }
        private void DataGridViewCP_Proveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCP_Proveedores.Rows.Count != 0)
            {
                DataGridViewCPproveedores();
            }
        }

        private void DataGridViewCP_Proveedores_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewCP_Proveedores.Rows.Count != 0)
            {
                DataGridViewCPproveedores();
            }
        }
        private void DataGridViewCPproveedores()
        {
            
            idProveedorCp = Convert.ToInt16(dataGridViewCP_Proveedores.CurrentRow.Cells[0].Value);
            labelCP_Proveedor.Text = dataGridViewCP_Proveedores.CurrentRow.Cells[1].Value.ToString();
            proveedor = labelCP_Proveedor.Text;
            labelCP_ProveedorR.Text = proveedor;
            saldoProveedor = Convert.ToString(dataGridViewCP_Proveedores.CurrentRow.Cells[4].Value);
            labelCP_Fecha.Text = fecha;
        }
        private void TabControl_Compras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl_Compras.SelectedIndex == 1)
            {
                ClassModel.compra.getIngresos(label_EnCaja);
                ClassModel.compra.importeTempo(label_MontoPagar, labelCP_Importe2, labelCP_TotalPagar);
            }
        }
        private void TextBoxCP_Pagos_TextChanged(object sender, EventArgs e)
        {
            ClassModel.compra.verificarPago(textBoxCP_Pagos, labelCP_Pago, checkBoxCP_Deuda, labelCP_Deudas,
                labelCP_Deuda, labelCP_Saldo, idProveedorCp, labelCP_Titulo);
        }
        private void TextBoxCP_Pagos_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBoxCP_Pagos, e);
        }
        private void CheckBoxCP_Deuda_CheckedChanged(object sender, EventArgs e)
        {
            ClassModel.compra.verificarPago(textBoxCP_Pagos, labelCP_Pago, checkBoxCP_Deuda,labelCP_Deudas,
                labelCP_Deuda, labelCP_Saldo,idProveedorCp, labelCP_Titulo);
        }
        private void guardarCompras()
        {
            if (textBoxCP_Pagos.Text == string.Empty)
            {
                labelCP_Pago.Text = "Ingrese el pago";
                labelCP_Pago.ForeColor = Color.Red;
                textBoxCP_Pagos.Focus();
            }
            else
            {
                if (idProveedorCp != 0)
                {
                    var valor = ClassModel.compra.verificarPago(textBoxCP_Pagos,labelCP_Pago,checkBoxCP_Deuda, labelCP_Deudas,
                labelCP_Deuda, labelCP_Saldo, idProveedorCp, labelCP_Titulo);
                    if (valor)
                    {
                        ClassModel.compra.saveCompras(proveedor,idProveedorCp, usuario, idUsuario,role);
                        //textBoxCP_Pagos.Text = string.Empty;
                        checkBoxCP_Deuda.Checked = false;
                        labelCP_Pago.Text = "Pago con";
                        labelCP_Pago.ForeColor = Color.LightSlateGray;
                        labelCP_Deudas.Text = "$ 0.0";
                        labelCP_Deuda.Text = "$ 0.0";
                        labelCP_Saldo.Text = "$ 0.0";
                        idProveedorCp = 0;
                        labelCP_Titulo.Text = "Deuda";
                        ClassModel.compra.deleteTempo_Compras();
                        label_MontoPagar.Text = "$ 0.0";
                        textBoxCP_BuscarPD.Text = string.Empty;
                        ClassModel.compra.searchProveedor(dataGridViewCP_Proveedores, textBoxCP_BuscarPD.Text);
                        ClassModel.compra.searchCompras(dataGridView_ComprasProductos, "", 1, pageSize);
                        MessageBox.Show("Los datos fueron guardados con exito");
                    }
                }
                else
                {
                    MessageBox.Show("Debe elejir un proveedor");
                }
                if (checkBoxCP_SoloPago.Checked)
                {
                    MessageBox.Show("Usted seleciono Solo pago");
                }
                
            }
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DE Config.                            *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Config_Click(object sender, EventArgs e)
        {
            botonMenuConfig();
        }
        private void botonMenuConfig()
        {
            tabControl1.SelectedIndex = 6;
            //restablecerCompras();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = true;
            button_Compras.Enabled = true;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = true;
            button_Compras.Enabled = true;
            button_Config.Enabled = false;
        }
        private void Button_ConfUsuario_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 9;
            button_Config.Enabled = true;
        }
        private void buttonUs_Guardar_Click(object sender, EventArgs e)
        {
            string rutaeImagen, pass;
            pass = Encriptar.EncryptData(textBoxUs_Password.Text,textBoxUs_Usuario.Text);
            if (vericarCampos())
            {
                try
                {

                    //File.Copy(rutaOrigen, rutaDestino + nombreFoto);
                    rutaeImagen = rutaDestino + nombreFoto;
                    //File.Delete(rutaOrigen);
                    string rutaEncriptada = Seguridad.Encriptar(rutaeImagen);
                    //ClassModel.usuario.GuardarFotos(textBoxUs_Nombre.Text,textBoxUs_Apellido.Text,textBoxUs_Telefono.Text,
                    // textBoxUs_Direccion.Text,textBoxUs_Email.Text,textBoxUs_Usuario.Text,pass,comboBoxUs_Rol.Text, rutaEncriptada);
                    restablecerUsuario();
                    MessageBox.Show("El datos " + nombreFoto + " se guardo exitosamente");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("Debe llenar todos los campos");
            }

        }

        private void buttonUs_AgregarFoto_Click(object sender, EventArgs e)
        {
            //Se crea un objeto de la clase OpenFileDialog
            OpenFileDialog openFoto = new OpenFileDialog();
            //Se define el tipo de archivo que se van poder abrir
            openFoto.Filter = "Archivos de Imagen (*.jpg)(*.jpeg)|*.jpg;*.jpeg|PNG(*.png)|*.png"; ;
            //Se pone un leyenda a la hora de abrir el exprorador de archivos
            openFoto.FileName = "Seleccione una Imagen";
            //Se le pone un titulo a el explorador de archivos
            openFoto.Title = "Abrir Foto";
            //Se le da un ruta predeterminada para abrir el explorador de archivos
            openFoto.InitialDirectory = rutaOrigen;

            if (openFoto.ShowDialog() == DialogResult.OK)
            {
                nombreFoto = openFoto.SafeFileName;
                rutaOrigen = openFoto.FileName;
                pictureBoxUs_Foto.ImageLocation = rutaOrigen;
                pictureBoxUs_Foto.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void restablecerUsuario()
        {
            textBoxUs_Nombre.Text = string.Empty;
            textBoxUs_Apellido.Text = string.Empty;
            textBoxUs_Telefono.Text = string.Empty;
            textBoxUs_Direccion.Text = string.Empty;
            textBoxUs_Email.Text = string.Empty;
            textBoxUs_Usuario.Text = string.Empty;
            comboBoxUs_Rol.SelectedText = string.Empty;
            rutaOrigen = string.Empty;
            nombreFoto = string.Empty;
            rutaDestino = string.Empty;
            pictureBoxUs_Foto.Image = null;
        }
        private void TextBoxUs_Nombre_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Nombre.Text == string.Empty)
            {
                labelUs_Nombre.ForeColor = Color.LightSlateGray;
            }
            else
            {
                    labelUs_Nombre.Text = "Nombre";
                    labelUs_Nombre.ForeColor = Color.DarkCyan;     
            }
        }

        private void TextBoxUs_Nombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }

        private void TextBoxUs_Apellido_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Apellido.Text == string.Empty)
            {
                labelUs_Apellido.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Apellido.Text = "Apellido";
                labelUs_Apellido.ForeColor = Color.DarkCyan;
            }
    }

        private void TextBoxUs_Apellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }

        private void TextBoxUs_Telefono_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Telefono.Text == string.Empty)
            {
                labelUs_Telefono.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Telefono.Text = "Telefono";
                labelUs_Telefono.ForeColor = Color.DarkCyan;
            }
        }

        private void TextBoxUs_Telefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberKeyPress(e);
        }

        private void TextBoxUs_Direccion_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Direccion.Text == string.Empty)
            {
                labelUs_Direccion.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Direccion.Text = "Dirección";
                labelUs_Direccion.ForeColor = Color.DarkCyan;
            }
        }

        private void TextBoxUs_Direccion_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }

        private void TextBoxUs_Email_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Email.Text == string.Empty)
            {
                labelUs_Email.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Email.Text = "Email";
                labelUs_Email.ForeColor = Color.DarkCyan;
            }
        }


        private void TextBoxUs_Email_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void TextBoxUs_Usuario_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Usuario.Text == string.Empty)
            {
                labelUs_Usuario.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Usuario.Text = "Usuario";
                labelUs_Usuario.ForeColor = Color.DarkCyan;
            }
        }


        private void TextBoxUs_Usuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }

        private void ComboBoxUs_Rol_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxUs_Rol.SelectedText != string.Empty)
            {
                comboBoxUs_Rol.ForeColor = Color.LightSlateGray;
            }
            else
            {
                comboBoxUs_Rol.SelectedText = "Rol";
                comboBoxUs_Rol.ForeColor = Color.DarkCyan;
            }
        }

        private void ComboBoxUs_Rol_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.textKeyPress(e);
        }

        private void TextBoxUs_Password_TextChanged(object sender, EventArgs e)
        {
            if (textBoxUs_Password.Text == string.Empty)
            {
                labelUs_Password.ForeColor = Color.LightSlateGray;
            }
            else
            {
                labelUs_Password.Text = "Contraseña";
                labelUs_Password.ForeColor = Color.DarkCyan;
            }
        }

        private void TextBoxUs_Password_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        //Metodo prueba
        private bool vericarCampos()
        {
            bool valor = false;
            if (textBoxUs_Nombre.Text != string.Empty && textBoxUs_Apellido.Text != string.Empty && 
                textBoxUs_Telefono.Text != string.Empty && textBoxUs_Direccion.Text != string.Empty &&
                textBoxUs_Email.Text != string.Empty && textBoxUs_Usuario.Text != string.Empty &&
                comboBoxUs_Rol.SelectedText != string.Empty && rutaOrigen != string.Empty  && textBoxUs_Password.Text != string.Empty)
            {
                valor = true;
            }
            return valor;
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DE PRODUCTOS.                         *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Productos_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            restablecerProductos();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = true;
            button_Productos.Enabled = false;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = true;
            if ("Admin" != role)
            {
               button_Compras.Enabled = false;
            }
            else
            {
                button_Compras.Enabled = true;
            }

        }


        private void restablecerProductos()
        {
            paginas = 4;
            idCompras = 0;
            accion = "Insert";
            idProducto = 0;
            funcion = 0;
            ClassModel.producto.getProductos(dataGridView_ProdCompra,"");
            comboBox_DepartamentoPDT.DataSource = ClassModel.producto.GetDepartamentos();
            comboBox_DepartamentoPDT.ValueMember = "IdDpto";
            comboBox_DepartamentoPDT.DisplayMember = "Departamento";
            ClassModel.producto.searchProducto(dataGridView_Productos, textBox_DescripcionPDT.Text,1,pageSize);
            textBox_DescripcionPDT.Text = string.Empty;
            textBox_PrecioVentaPDT.Text = string.Empty;
            ClassModel.producto.codigoBarra(panelCodigoB, "00000000", textBox_DescripcionPDT.Text, textBox_PrecioVentaPDT.Text);
            new Paginador(dataGridView_Productos, label_PaginasPDT,paginas,0);
        }
        private void DataGridView_ProdCompra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_ProdCompra.Rows.Count != 0)
            {
                dataGridViewProdCompra();
            }
        }

        private void DataGridView_ProdCompra_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_ProdCompra.Rows.Count != 0)
            {
                dataGridViewProdCompra();
            }
        }

        private void dataGridViewProdCompra() 
        {
            String producto;
            funcion = 1;
            accion = "Insert";
            idCompras = Convert.ToUInt16(dataGridView_ProdCompra.CurrentRow.Cells[0].Value);
            producto = Convert.ToString(dataGridView_ProdCompra.CurrentRow.Cells[1].Value);
            textBox_DescripcionPDT.Text = producto;
            labelPT_Producto.Text = producto;
            cantidad = Convert.ToInt16(dataGridView_ProdCompra.CurrentRow.Cells[2].Value);
            precioCompra = Convert.ToString(dataGridView_ProdCompra.CurrentRow.Cells[3].Value);
            ClassModel.producto.codigoBarra(panelCodigoB, "0",producto, textBox_PrecioVentaPDT.Text);
        }

        private void TextBox_DescripcionPDT_TextChanged(object sender, EventArgs e)
        {
            if (textBox_DescripcionPDT.Text == string.Empty)
            {
                label_DescripcionPDT.ForeColor = Color.LightSlateGray;
  
            }
            else
            {
                label_DescripcionPDT.Text = "Descripción";
                label_DescripcionPDT.ForeColor = Color.DarkCyan;
                if (funcion == 1)
                {
                    ClassModel.producto.codigoBarra(panelCodigoB, "0", textBox_DescripcionPDT.Text, textBox_PrecioVentaPDT.Text);
                    
                }
            }
            ClassModel.producto.searchProducto(dataGridView_Productos, textBox_DescripcionPDT.Text, 1, pageSize);
        }

        private void TextBox_DescripcionPDT_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void TextBox_PrecioVentaPDT_TextChanged(object sender, EventArgs e)
        {
            if (textBox_PrecioVentaPDT.Text == string.Empty)
            {
                label_PrecioVentaPDT.Text = "Precio Venta";
                label_PrecioVentaPDT.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_PrecioVentaPDT.Text = "Precio Venta";
                label_PrecioVentaPDT.ForeColor = Color.DarkCyan;
                if (funcion == 1 && precioCompra != null)
                {
                    ClassModel.producto.codigoBarra(panelCodigoB, "0", textBox_DescripcionPDT.Text, textBox_PrecioVentaPDT.Text);
                    ClassModel.producto.verificarPrecioVenta(label_PrecioVentaPDT, textBox_PrecioVentaPDT.Text,precioCompra,funcion);
                }
                
            }
        }

        private void TextBox_PrecioVentaPDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBox_PrecioVentaPDT,e);
        }

        private void ComboBox_DepartamentoPDT_SelectedIndexChanged(object sender, EventArgs e)
        {
            Departamentos dpto = (Departamentos)comboBox_DepartamentoPDT.SelectedItem;
            comboBox_Categorias.Text = string.Empty;

            comboBox_Categorias.DataSource = ClassModel.producto.GetCategorias(dpto.IdDpto);
            comboBox_Categorias.DisplayMember = "Categoria";
        }

        private void ComboBox_DepartamentoPDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            Departamentos dpto = (Departamentos)comboBox_DepartamentoPDT.SelectedItem;
            comboBox_Categorias.Text = string.Empty;

            comboBox_Categorias.DataSource = ClassModel.producto.GetCategorias(dpto.IdDpto);
            comboBox_Categorias.DisplayMember = "Categoria";
        }

        private void TextBox_CoprasProductos_TextChanged(object sender, EventArgs e)
        {
            ClassModel.producto.getProductos(dataGridView_ProdCompra, textBox_CoprasProductos.Text);
        }
        private void Button_PrimeroPDT_Click(object sender, EventArgs e)
        {
            new Paginador(dataGridView_Productos, label_PaginasPDT, paginas, 1).primero();
        }

        private void Button_AnteriorPDT_Click(object sender, EventArgs e)
        {
            new Paginador(dataGridView_Productos, label_PaginasPDT, paginas, 1).anterior();
        }

        private void Button_SiguientePDT_Click(object sender, EventArgs e)
        {
            new Paginador(dataGridView_Productos, label_PaginasPDT, paginas, 1).siguiente();
        }

        private void Button_UltimaPDT_Click(object sender, EventArgs e)
        {
            new Paginador(dataGridView_Productos, label_PaginasPDT, paginas, 1).ultimo();
        }
        private void guardarProducto()
        {
            if (textBox_DescripcionPDT.Text == string.Empty)
            {
                label_DescripcionPDT.Text = "Ingrese la descripción";
                label_DescripcionPDT.ForeColor = Color.Red;
                textBox_DescripcionPDT.Focus();
            }
            else
            {
                if (textBox_PrecioVentaPDT.Text == string.Empty)
                {
                    label_PrecioVentaPDT.Text = "Ingrese el precio de venta";
                    label_PrecioVentaPDT.ForeColor = Color.Red;
                    textBox_PrecioVentaPDT.Focus();
                }
                else
                {
                    var producto = textBox_DescripcionPDT.Text;
                    var precio = textBox_PrecioVentaPDT.Text;
                    var departamento = comboBox_DepartamentoPDT.Text;
                    var categoria = comboBox_Categorias.Text;
                    bool verificar = ClassModel.producto.verificarPrecioVenta(label_PrecioVentaPDT,textBox_PrecioVentaPDT.Text,precioCompra, funcion);

                    switch (accion)
                    {
                        case "Insert":
                            if (funcion == 1)
                            {
                                if (verificar)
                                {
                                    ClassModel.producto.saveProducto(producto, cantidad, precio, departamento, categoria, accion, idCompras);
                                    groupBox = groupBoxPT_CodeBarra;
                                    printDocument1.Print();
                                    restablecerProductos();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seleccione un Producto");
                            }
                                break;
                        case "Update":
                            if (funcion == 2)
                            {
                                ClassModel.producto.saveProducto(producto, cantidad, precio, departamento, categoria, accion, idProducto);
                                restablecerProductos();
                            }
                            else
                            {
                                MessageBox.Show("Seleccione un Producto");    
                            }
                            break;
                    }
                    
                }
            }
        }
        private void Button_GuardarPDT_Click(object sender, EventArgs e)
        {
            guardarProducto();
        }

        private void DataGridView_Productos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Productos.Rows.Count != 0)
            {
                dataGridViewPTProductos();
            }
        }

        private void DataGridView_Productos_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridView_Productos.Rows.Count != 0)
            {
                dataGridViewPTProductos();
            }
        }

        public void dataGridViewPTProductos()
        {
            accion = "Update";
            funcion = 2;
            String codigo, producto, precio;
            idProducto = Convert.ToInt16(dataGridView_Productos.CurrentRow.Cells[0].Value);
            codigo = Convert.ToString(dataGridView_Productos.CurrentRow.Cells[1].Value);
            textBox_DescripcionPDT.Text = Convert.ToString(dataGridView_Productos.CurrentRow.Cells[2].Value);
            precio = Convert.ToString(dataGridView_Productos.CurrentRow.Cells[3].Value);
            textBox_PrecioVentaPDT.Text = precio.Replace("$", "");
            comboBox_DepartamentoPDT.Text = Convert.ToString(dataGridView_Productos.CurrentRow.Cells[5].Value);
            comboBox_Categorias.Text = Convert.ToString(dataGridView_Productos.CurrentRow.Cells[6].Value);

            ClassModel.producto.codigoBarra(panelCodigoB,codigo,textBox_DescripcionPDT.Text, textBox_PrecioVentaPDT.Text);

        }
        private void Button_CancelarPDT_Click(object sender, EventArgs e)
        {
            restablecerProductos();
        }
        #endregion

        /***************************************************************
         *                                                             *   
         *                CODIGO DE VENTAS.                            *
         *                                                             *
         *                                                             *
         **************************************************************/
        #region
        private void Button_Ventas_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            restablacerVetas();
            button_Clientes.Enabled = true;
            button_Ventas.Enabled = false;
            button_Productos.Enabled = true;
            button_Dpto.Enabled = true;
            button_Proveedores.Enabled = true;
            if ("Admin" != role)
            {
                button_Compras.Enabled = false;
            }
            else
            {
                button_Compras.Enabled = true;
            }

        }
        private void restablacerVetas()
        {
            ClassModel.venta.searchVentatemp(dataGridView_Ventas, 1, pageSize,caja,idUsuario);
            ClassModel.venta.importes(label_ImportesVentas,caja,idUsuario);
            label_MensajeCliente.Text = string.Empty;
            ClassModel.venta.searchCliente(dataGridView_ClienteVenta, textBox_BuscarClienteVenta.Text);
        }
        private void Button_BuscarProducto_Click(object sender, EventArgs e)
        {
            if (textBox_BuscarProductos.Text == string.Empty)
            {
                label_MensajeVenta.Text = "Ingrese el códgo del producto";
                label_MensajeVenta.ForeColor = Color.Red;
                textBox_BuscarProductos.Focus();
            }
            else
            {
              var producto = ClassModel.venta.searchBodega(textBox_BuscarProductos.Text);
                if (0 < producto.Count)
                {
                    ClassModel.venta.saveVentasTempo(textBox_BuscarProductos.Text, 0,caja,idUsuario);
                    ClassModel.venta.searchVentatemp(dataGridView_Ventas, 1, pageSize,caja,idUsuario);
                    ClassModel.venta.importes(label_ImportesVentas,caja,idUsuario);
                }
                else
                {
                    label_MensajeVenta.Text = "Dicho producto no se encuentra registrado";
                }
            }
        }
        private void TextBox_Pagos_KeyPress(object sender, KeyPressEventArgs e)
        {
            ClassModel.eventos.numberDecimalKeyPRess(textBox_Pagos,e);
        }

        private void TextBox_Pagos_TextChanged(object sender, EventArgs e)
        {
            label_MensajeCliente.Text = string.Empty;
            ClassModel.venta.pagos(textBox_Pagos, label_SuCambio, label_Cambio, label_Pago);
            ClassModel.venta.dataCliente(checkBox_Credito,textBox_Pagos,textBox_BuscarClienteVenta,dataGridView_ClienteVenta,labels);
        }
        private void TextBox_BuscarClienteVenta_TextChanged(object sender, EventArgs e)
        {
            label_MensajeCliente.Text = string.Empty;
            ClassModel.venta.searchCliente(dataGridView_ClienteVenta,textBox_BuscarClienteVenta.Text);
        }
        private void CheckBox_Credito_CheckedChanged(object sender, EventArgs e)
        {
            ClassModel.venta.dataCliente(checkBox_Credito, textBox_Pagos, textBox_BuscarClienteVenta, dataGridView_ClienteVenta, labels);
        }
        private void DataGridView_ClienteVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_ClienteVenta.Rows.Count != 0)
            {
                label_MensajeCliente.Text = string.Empty;
                if (checkBox_Credito.Checked == true)
                {
                    if (textBox_Pagos.Text != string.Empty)
                    {
                        ClassModel.venta.dataCliente(checkBox_Credito, textBox_Pagos, textBox_BuscarClienteVenta, dataGridView_ClienteVenta, labels);
                    }
                    else
                    {
                        label_MensajeCliente.Text = "Ingrese el pago";
                        textBox_Pagos.Focus();
                    }
                }
                else
                {
                    label_MensajeCliente.Text = "Seleccione la opción de crédito";
                }
            }
        }
        private void Button_Cobrar_Click(object sender, EventArgs e)
        {

        }
        private void DataGridView_Ventas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView_Ventas.Rows.Count != 0)
            {
                string codigo = Convert.ToString(dataGridView_Ventas.CurrentRow.Cells[1].Value);
                int cantidad = Convert.ToInt16(dataGridView_Ventas.CurrentRow.Cells[4].Value);
                ClassModel.venta.deleteVenta(codigo,cantidad,caja,idUsuario);
                ClassModel.venta.importes(label_ImportesVentas,caja,idUsuario);
                ClassModel.venta.searchVentatemp(dataGridView_Ventas, 1, pageSize,caja,idUsuario);
            }
        }
        #endregion
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Acceso rapido para el menu de navegación
            if (e.KeyCode == Keys.F1)
            {
                tabControl1.SelectedIndex = 0;
            }
            if (e.KeyCode == Keys.F2)
            {
                botonMenuClientes();
            }
            if (e.KeyCode == Keys.F3)
            {
                botonMenuProveedores();
            }
            if (e.KeyCode == Keys.F4)
            {
                    tabControl1.SelectedIndex = 3;   
            }
            if (e.KeyCode == Keys.F5)
            {
                botonMenuDepartamentos();
            }
            if (e.KeyCode == Keys.F6)
            {
                botonMenuCompras();
            }
            if (e.KeyCode == Keys.F7)
            {
                botonMenuConfig();
            }
            //Acceso rapido para la parte de Configuración
            if (tabControl1.SelectedIndex == 6)
            {
                if (e.KeyCode == Keys.U)
                {
                    tabControl1.SelectedIndex = 9;
                    button_Config.Enabled = true;
                }
            }            
        }
    }
}
