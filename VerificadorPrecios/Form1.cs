using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VerificadorPrecios
{
    
    public partial class Form1 : Form
    {
        private int tiempo = 0;
        private String codigo = "";

        private bool proFind = true;

        private MySqlDataReader result;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width / 2, 
                Screen.PrimaryScreen.WorkingArea.Height / 2);
            pictureBox1.Location = new Point(this.Width / 13 - pictureBox1.Width / 2,
                this.Height / 12 - pictureBox1.Height + 30);
            pictureBox2.Location = new Point(this.Width / 2 - pictureBox2.Width / 2,
                this.Height / 2 + pictureBox2.Height - 260);
            label1.Location = new Point(this.Width / 2 - label1.Width / 2,
                this.Height / 2 + label1.Height + 60);
            label2.Location = new Point(this.Width / 2 - label2.Width / 2,
                20);
            product_price.Visible = false;
            product_image.Visible = false;
            product_Name.Visible = false;    
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    MySqlConnection server;
                    server = new MySqlConnection("server=127.0.0.1; user=root; database=verificador_pr; SSL Mode=None;");
                    server.Open();
                    String queryString = "SELECT producto_codigo, producto_nombre, producto_precio, producto_imagen " +
                        "FROM vp_productos WHERE producto_codigo = " + codigo + ";";
                    MySqlCommand query = new MySqlCommand(queryString, server);
                    result = query.ExecuteReader();

                    if (result.HasRows)
                    {
                        pictureBox2.Image = global::VerificadorPrecios.Properties.Resources.gifload;
                        pictureBox2.Size = new System.Drawing.Size(100, 100);
                        pictureBox2.Location = new Point(this.Width / 2 - pictureBox2.Width / 2,
                            this.Height / 2 - pictureBox2.Height + 40);
                        pictureBox2.Visible = true;
                        tiempo = 0;
                        timer1.Enabled = true;
                        label1.Visible = false;
                        label2.Visible = false;
                        pictureBox1.Visible = false;
                        product_price.Visible = false;
                        product_Name.Visible = false;
                        product_image.Visible = false;
                    }
                    else
                    {
                        proFind = false;
                        label1.Text = "Error al leer el código.\n     Intente otra vez";
                        label1.Location = new Point(this.Width / 2 - label1.Width / 2,
                            this.Height / 2 + label1.Height + 40);
                        pictureBox2.Image = global::VerificadorPrecios.Properties.Resources.error_msg;
                        pictureBox2.Size = new System.Drawing.Size(170, 170);
                        pictureBox2.Location = new Point(this.Width / 2 - pictureBox2.Width / 2,
                            this.Height / 2 + pictureBox2.Height - 300);
                        pictureBox2.Visible = true;
                        tiempo = 0;
                        timer1.Enabled = true;
                        label2.Visible = false;
                        pictureBox1.Visible = false;
                        product_price.Visible = false;
                        product_Name.Visible = false;
                        product_image.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Titulo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
                codigo = "";
            }
            else
            {
                codigo += e.KeyChar;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            tiempo++;
            if (tiempo == 2 && proFind)
            {
                result.Read();
                product_image.ImageLocation = result.GetString(3);
                product_image.SizeMode = PictureBoxSizeMode.StretchImage;
                product_image.Location = new Point((this.Width / 4) - (product_image.Width / 2),
                    (this.Height / 3) - product_image.Height / 2 + 50);
                product_image.Visible = true;
                product_Name.Text = result.GetString(1);
                product_Name.Location = new Point((this.Width / 2) - product_image.Width + 10, 
                    (this.Height / 3 ) + product_image.Height - 50);
                product_Name.Visible = true;
                product_price.Text = "Precio: $" + result.GetString(2);
                product_price.Location = new Point((this.Width / 2),
                    (this.Height / 2) - product_price.Height);
                product_price.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
                pictureBox2.Visible = false;
            }
            else if (tiempo == 5 && !proFind)
            {
                timer1.Enabled = false;
                returnToOriginal();
            }
            if (tiempo == 8)
            {
                timer1.Enabled = false;
                returnToOriginal();
            }
        }
        private void returnToOriginal()
        {
            pictureBox2.Visible = true;
            label1.Visible = true;
            label1.Text = "Coloque el código de barras por el sensor";
            label2.Visible = true;
            label2.Text = "";
            pictureBox1.Visible = true;
            label1.Location = new Point(this.Width / 2 - label1.Width / 2,
                this.Height / 2 + label1.Height + 60);
            product_price.Visible = false;
            product_Name.Visible = false;
            pictureBox2.Size = new System.Drawing.Size(218, 121);
            pictureBox2.Location = new Point(this.Width / 2 - pictureBox2.Width / 2,
                this.Height / 2 + pictureBox2.Height - 260);
            pictureBox2.Image = global::VerificadorPrecios.Properties.Resources.barcode_scan;
            product_image.Visible = false;
            product_image.ImageLocation = null;

            proFind = true;
        }
    }
}
