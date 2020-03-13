using Punto_de_ventas.ModelClass;
using Punto_de_ventas.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Punto_de_ventas
{
    public partial class Login : Form
    {
        private Usuario usuario = new Usuario();
        public Login()
        {
            InitializeComponent();
            label_Mensaje.Text = "";
            textBox_User.Focus();
        }

        private void TextBox_User_TextChanged(object sender, EventArgs e)
        {
            if (textBox_User.Text == "")
            {
                label_User.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_User.Text = "Usuario";
                label_User.ForeColor = Color.Green;
            }
            label_Mensaje.Text = "";
        }

        private void TextBox_Pass_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Pass.Text == "")
            {
                label_Pass.ForeColor = Color.LightSlateGray;
            }
            else
            {
                label_Pass.Text = "Contraseña";
                label_Pass.ForeColor = Color.Green;
            }
            label_Mensaje.Text = "";
        }
        private void iniciar()
        {
            if (textBox_User.Text == "")
            {
                label_User.Text = "Ingrese el usuario";
                label_User.ForeColor = Color.Red;
                textBox_User.Focus();
            }
            else
            {
                if (textBox_Pass.Text == "")
                {
                    label_Pass.Text = "Ingrese la contraseña";
                    label_Pass.ForeColor = Color.Red;
                    textBox_Pass.Focus();
                }
                else
                {
                    object[] objects = usuario.login(textBox_User.Text, textBox_Pass.Text);
                    List<Usuarios> listUsuario = (List<Usuarios>)objects[0];
                    List<Cajas> listCaja = (List<Cajas>)objects[1];
                    if (0 < listUsuario.Count)
                    {
                        if ("Admin" == listUsuario[0].Role)
                        {
                            Form1 form1 = new Form1(listUsuario, listCaja);
                            form1.Show();
                            Visible = false;
                        }
                        else
                        {
                            if (0 < listCaja.Count)
                            {
                                Form1 form1 = new Form1(listUsuario, listCaja);
                                form1.Show();
                                Visible = false;
                            }
                            else
                            {
                                label_Mensaje.Text = "No hay cajas disponibles";
                            }
                        }
                    }
                    else
                    {
                        textBox_User.Text = string.Empty;
                        textBox_Pass.Text = string.Empty;
                        label_Mensaje.Text = "Usuario o Contraseña incorrecta";
                    }

                }
            }
        }

        private void Button_Iniciar_Click(object sender, EventArgs e)
        {
            iniciar();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void TextBox_Pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(textBox_Pass.Text))
                {
                    button_Iniciar.Focus();
                }
                else
                {
                    label_Pass.Text = "Ingrese la contraseña";
                    label_Pass.ForeColor = Color.Red;
                    textBox_Pass.Focus();
                }
            }
        }
        //Se utiliza para que cada que de enter en los textbox pase al siguiente
        private void TextBox_User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)

            {
                if (!string.IsNullOrEmpty(textBox_User.Text))
                {
                    textBox_Pass.Focus();
                }
                else
                {
                    label_User.Text = "Ingrese el usuario";
                    label_User.ForeColor = Color.Red;
                    textBox_User.Focus();
                }
            }
        }

        private void CheckBox_VerPass_CheckedChanged(object sender, EventArgs e)
        {
            //Codigo para mostrar la contraseña si se requiere
            if (checkBox_VerPass.Checked)
            {
                textBox_Pass.PasswordChar = '\0';

            }
            else
            {
                textBox_Pass.PasswordChar = '*';

            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox_Pass.Text != string.Empty && textBox_User.Text != string.Empty)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    iniciar();

                }
            }            
        }
    }
}
