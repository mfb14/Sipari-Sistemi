using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;


/**
 *SQL True Dönen Değerler
 * 
 * admin' or '1'='1
 * 1' or '1' = '1
 * ' or 'a'='a
 * 
 */

namespace VeriTabanıproje
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }
        SqlConnection connect = new SqlConnection("Data Source=" + Properties.Settings.Default.Sunucu + ";Initial Catalog=" + Properties.Settings.Default.VeriTabani + ";Integrated Security=True");


        /**
         *brief SQL injection engelleme firma girişi için
         * Hangi yazdıkların koidları değiştirince bak bişey anlamadım bu insandan insana oluyor
         */

        void sqlInjectionControl(string id, string passwd)
        {

            try
            {


                connect.Open();
                SqlCommand command = new SqlCommand("Select *from MusteriFirma where FirmaId=@id AND Sifre=@passwd", connect);//Veritabanından FirmaId ve FirmaId ye ait sifreyi sorgulama.
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@passwd", passwd);
                SqlDataReader datareader = command.ExecuteReader();
                if (datareader.Read())//Dondurulen degerin "True" olması durumunu kontrol etme.
                {
                    Form_CustomerPanel cus = new Form_CustomerPanel();
                    cus.KullaniciId = id.ToString();
                    cus.Show();

                    this.Hide();
                }
                else
                    MessageBox.Show("Kullanici Id veya Şifre hatalı !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                connect.Close();
            }
            catch { }
        }

        /**
         *brief Normal giriş yaparken id nin tipini string yap
         *
         */
        public void Giris(string id, string passwd)
        {
            //sqlInjectionControl(id, passwd);
            
            if (!(txtFirmaId.Text.ToLower().Contains("or")))
            {
                if (txtFirmaId.Text != "" && txtSifre.Text != "")
                    sqlInjectionControl(id, passwd);
                else
                    MessageBox.Show("Gerekli alanları doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //bu if de diyecekki eğer txtFirmaid içinde "or" ifadesi yoksa aşşağıyı çalıştır
            }
            else
                MessageBox.Show("Sen kim sqlinjection kim :D");
            
            

            //firmaGirisiKontrol(id, passwd);
        }


        /**
         *brief normal firma girisi kontrolü için
         *normal girişte string değer alıyor injection engellemek için Giris methodunu id sini int yapmak gerek
         */
        //private void firmaGirisiKontrol(string id, string passwd)
        //{
        //    try
        //    {


        //        connect.Open();
        //        SqlCommand command = new SqlCommand("Select *from MusteriFirma where FirmaId='" + id + "' AND Sifre='" + passwd + "'", connect);//Veritabanından FirmaId ve FirmaId ye ait sifreyi sorgulama.
        //        SqlDataReader datareader = command.ExecuteReader();
        //        if (datareader.Read())//Dondurulen degerin "True" olması durumunu kontrol etme.
        //        {
        //            Form_CustomerPanel cus = new Form_CustomerPanel();
        //            cus.KullaniciId = id;
        //            cus.Show();

        //            this.Hide();
        //        }
        //        else
        //            MessageBox.Show("Kullanici Id veya Şifre hatalı !", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        connect.Close();
        //    }
        //    catch { }
        //}


       /**
         *brief SQL injection engellemek için müşteri girişini değiştirme
         *
         */
  
        void sqlFirmaGiris()
        {
            
                foreach (Control textbox in this.Controls)
                    if (textbox is TextBox && textbox.Text == "")
                        errorProvider1.SetError(textbox, "Bu alan boş bırakılamaz.");
                if (txtFirmaId.Text != "" && txtSifre.Text != "")
                    Giris(txtFirmaId.Text, txtSifre.Text); //Giriş yordamını çalıştırma ve içerisine parametre gönderme.
                else
                    MessageBox.Show("Gerekli alanları doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
        }


        private void Form_Login_Load(object sender, EventArgs e)
        {
           
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (!(txtFirmaId.Text.ToLower().Contains("or")))
            {
                sqlFirmaGiris();

            }
            else
                MessageBox.Show("Hatalı Ifade Kullandınız", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //firmaGirisKontrol2();
        }


        /**
         *brief normal firma girişi için
         *
         */
        //private void firmaGirisKontrol2()
        //{
        //    foreach (Control textbox in this.Controls)
        //        if (textbox is TextBox && textbox.Text == "")
        //            errorProvider1.SetError(textbox, "Bu alan boş bırakılamaz.");
        //    if (txtFirmaId.Text != "" && txtSifre.Text != "")
        //        Giris(txtFirmaId.Text, txtSifre.Text); //Giriş yordamını çalıştırma ve içerisine parametre gönderme.
        //    else
        //        MessageBox.Show("Gerekli alanları doldurunuz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //}

        private void txtFirmaId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); //txtFirmaId isimli textbox içine sadece rakam girişine izin verme.
        }

        private void btnAdminGiris_Click(object sender, EventArgs e)
        {
            Form_AdminLogin AdminGiris = new Form_AdminLogin();
            AdminGiris.Show(); //Form geçisi
            this.Hide();
        }

        private void Form_Login_Load_1(object sender, EventArgs e)
        {
            if(Properties.Settings.Default.Sunucu=="")
            {
                Form_ServerConnect server = new Form_ServerConnect();
                server.ShowDialog();
            }
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            Form_CompanyRegistration reg = new Form_CompanyRegistration();
            reg.ShowDialog();
            
        }
    }
}
