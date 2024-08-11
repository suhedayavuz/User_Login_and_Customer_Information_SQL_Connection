using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _229911372_ŞühedaYavuz_GörselProgramlama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGiriş_Click(object sender, EventArgs e)
        {
            //Kullanıcı adı ve parola için değişken atama burada yapıldı

            string username = txtKullaniciAdi.Text;
            string password = txtParola.Text;

            // Kullanıcı adı ve parolayı doğrulama işlemleri burada yapıldı

            if (username == "Şüheda" && password == "5353")
            {
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
            }
        }
    }
}
