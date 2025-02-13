﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace VeriTabanıproje
{
    public partial class Form_ManagementPanel : Form
    {
        public Form_ManagementPanel()
        {
            InitializeComponent();
        }
        SqlConnection connect = new SqlConnection("Data Source=" + Properties.Settings.Default.Sunucu + ";Initial Catalog=" + Properties.Settings.Default.VeriTabani + ";Integrated Security=True");
        SqlDataAdapter da;
        DataSet ds, dataset2,dataset3;
        private void MusteriListeYukle()
        {
            DataTable dt = new DataTable();
            string sql = "MusteriListele";
            da = new SqlDataAdapter(sql, connect);
            ds = new DataSet();
            connect.Open();
            da.Fill(ds, "tablo");
            connect.Close();
            dataGridView1.DataSource = ds.Tables["tablo"];
        }
        private void SiparisListesiYukle()
        {
            DataTable dt = new DataTable();
            string sql = "MusteriSiparisListele";
            da = new SqlDataAdapter(sql, connect);
            ds = new DataSet();
            connect.Open();
            da.Fill(ds, "tablo");
            connect.Close();
            dataGridView2.DataSource = ds.Tables["tablo"];
        }
        //private void MusteriListeArama(string sorgu)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        da = new SqlDataAdapter(sorgu, connect);
        //        ds = new DataSet();
        //        connect.Open();
        //        da.Fill(ds, "tablo");
        //        connect.Close();
        //        dataGridView1.DataSource = ds.Tables["tablo"];
        //    }
        //    catch
        //    {
        //        connect.Close();
        //        //MessageBox.Show("Filtre içinde aranan değer bulunamadı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        
        //}
        private void AlanYukle()
        {
            try
            {
                cmbAlanAdi.Items.Clear();
                connect.Open();
                SqlCommand command2 = new SqlCommand("AlanCek", connect);
                command2.CommandType = CommandType.StoredProcedure;
                SqlDataReader datareader = command2.ExecuteReader();
                while (datareader.Read())
                    cmbAlanAdi.Items.Add(datareader["AlanAdi"].ToString());
                connect.Close();
                cmbAlanAdi.Text = cmbAlanAdi.Items[0].ToString();
            }
            catch { }
           
        }
            
        private void Form_ManagementPanel_Load(object sender, EventArgs e)
        {
            
            
            SiparisListesiYukle();
            MusteriListeYukle();
            AlanYukle();
        }

        private void btnMusteriListe_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = tabMusteriListe;
            MusteriListeYukle();
        }

        private void btnMusteriListeYenile_Click(object sender, EventArgs e)
        {
            MusteriListeYukle();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_FirmaAdi.Text = ds.Tables["tablo"].Rows[e.RowIndex]["FirmaAdi"].ToString();
                txtEposta.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Eposta"].ToString();
                txtTelefon.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Telefon"].ToString();
                txtMensei.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Mensei"].ToString();
                txtSokakCadde.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Sokak"].ToString();
                txtSokakCadde.Text += "  No : " + ds.Tables["tablo"].Rows[e.RowIndex]["No"].ToString();
                txtIl.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Il"].ToString();
                txtIlce.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Ilce"].ToString();
                txtNumara.Text = ds.Tables["tablo"].Rows[e.RowIndex]["FirmaId"].ToString();
                txtSifre.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Sifre"].ToString();
                txtYetkiliIsim.Text = ds.Tables["tablo"].Rows[e.RowIndex]["YetkiliIsim"].ToString();
                txtMahalle.Text = ds.Tables["tablo"].Rows[e.RowIndex]["Mahalle"].ToString();
                txtNo.Text = ds.Tables["tablo"].Rows[e.RowIndex]["No"].ToString();
            }
            catch {
                foreach (Control control in groupBox1.Controls)
                {
                    if (control is TextBox)
                        control.Text = "";
                }
            }
        
        }

        private void btnUyekaldir_Click(object sender, EventArgs e)
        {
            if(txtNumara.Text!="")
            {
                try
                {
                    DialogResult sorgu = MessageBox.Show("Silmek istediğinize emin misiniz ?", "Soru", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (sorgu == DialogResult.Yes)
                    {
                        SqlCommand command = new SqlCommand("FirmaSil", connect);
                        command.CommandType = CommandType.StoredProcedure;
                        connect.Open();
                        command.Parameters.AddWithValue("@FirmaId", txtNumara.Text);
                        command.ExecuteNonQuery();
                        connect.Close();
                        MusteriListeYukle();
                        foreach (Control control in groupBox1.Controls)
                        {
                            if (control is TextBox)
                                control.Text = "";
                        }
                        MessageBox.Show("Silmek işlemi başarılı", "Soru", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch(Exception hata) {
                    connect.Close();
                    MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBilgileriGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();
                SqlCommand command = new SqlCommand("update MusteriFirma set FirmaAdi=@FirmaAdi,Eposta=@Eposta,Telefon=@Telefon,Mensei=@Mensei,YetkiliIsim=@YetkiliIsim,Sifre=@Sifre where FirmaId=" + txtNumara.Text, connect);
                command.Parameters.AddWithValue("@FirmaAdi", txt_FirmaAdi.Text);
                command.Parameters.AddWithValue("@Eposta", txtEposta.Text);
                command.Parameters.AddWithValue("@Telefon", txtTelefon.Text);
                command.Parameters.AddWithValue("@Mensei", txtMensei.Text);
                command.Parameters.AddWithValue("@YetkiliIsim", txtYetkiliIsim.Text);
                command.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                command.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("update FirmaAdresi set Il=@Il,Ilce=@Ilce,Mahalle=@MAhalle,Sokak=@Sokak,No=@No where FirmaId=" + txtNumara.Text, connect);
                command2.Parameters.AddWithValue("@Il", txtIl.Text);
                command2.Parameters.AddWithValue("@Ilce", txtIlce.Text);
                command2.Parameters.AddWithValue("@Mahalle", txtMahalle.Text);
                command2.Parameters.AddWithValue("@Sokak", txtSokakCadde.Text);
                command2.Parameters.AddWithValue("@No", txtNo.Text);
                command2.ExecuteNonQuery();
                connect.Close();
                MusteriListeYukle();
                MessageBox.Show("Güncelleme başarılı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            {
                connect.Close();
                MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form_Login login = new Form_Login();
            login.Show();
            this.Close();
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            //string sql = "Select MusteriFirma.FirmaId, FirmaAdi, Eposta,Telefon,Mensei,YetkiliIsim,Sifre,Il,Ilce,Mahalle,Sokak,No from MusteriFirma join FirmaAdresi on MusteriFirma.FirmaId = FirmaAdresi.FirmaId where " + cmbArama.Text + " Like '%" + txtArama.Text + "%'";

            //MusteriListeArama(sql);
            
            connect.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("Musteri_Ara", connect);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.Parameters.AddWithValue("@x", txtArama.Text.Trim());
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView1.DataSource = dt;

            connect.Close();


        }
   
        private void txtSiparisArama_TextChanged(object sender, EventArgs e)
        {
            connect.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("Siparis_Ara", connect);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.Parameters.AddWithValue("@s", txtSiparisArama.Text.Trim());
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            dataGridView2.DataSource = dt;

            connect.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = tabMusteriSiparisleri;
        }

       

        private string ReturnMax()
        {

            SqlDataAdapter dataadapter = new SqlDataAdapter("select max(ParcaId) from ParcaBilgileri", connect);
            DataSet newdata = new DataSet();
            dataadapter.Fill(newdata);
            return (newdata.Tables[0].Rows[0][0].ToString());
             
        }
        private void UrunleriListele()
        {
            try
            {
                connect.Open();
                SqlCommand command = new SqlCommand("ParcaBilgileriCek", connect);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataadapter = new SqlDataAdapter();
                dataadapter.SelectCommand = command;
                dataset2 = new DataSet();
                dataadapter.Fill(dataset2);
                dataGridView3.DataSource = dataset2.Tables[0];
                connect.Close();
            }
            catch { connect.Close(); }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = tabUrunler;
            UrunleriListele();
            AlanYukle();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form_Login login = new Form_Login();
            login.Show();
            this.Close();
        }

        private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                if (dataGridView3.SelectedCells[0].Value !=null)
                {
                    txtParcaId.Text = (Convert.ToInt64(dataset2.Tables[0].Rows[e.RowIndex]["ParcaId"])).ToString();
                }
                else
                {
                    txtParcaId.Text = (Convert.ToDouble(ReturnMax()) ).ToString();
                }
                txtResimNo.Text = dataset2.Tables[0].Rows[e.RowIndex]["ResimNo"].ToString();
                txtIslemeResimNo.Text = dataset2.Tables[0].Rows[e.RowIndex]["IslemeResimNo"].ToString();
                txtParcaAdi.Text = dataset2.Tables[0].Rows[e.RowIndex]["ParcaAdi"].ToString();
                txtHamAgirlik.Text = dataset2.Tables[0].Rows[e.RowIndex]["HamAgirlik"].ToString();
                txtIslenmisAgirlik.Text = dataset2.Tables[0].Rows[e.RowIndex]["IslenmisAgirlik"].ToString();
                txtAmbalajAdedi.Text = dataset2.Tables[0].Rows[e.RowIndex]["AmbalajAdedi"].ToString();
                txtResimNo.Text = dataset2.Tables[0].Rows[e.RowIndex]["ResimNo"].ToString();
                cmbAmbalajSekli.Text = dataset2.Tables[0].Rows[e.RowIndex]["AmbalajSekli"].ToString();

                connect.Open();
                SqlCommand command = new SqlCommand("UrunEkle_AlanBilgiCek", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlanId", Convert.ToInt32(dataset2.Tables[0].Rows[e.RowIndex]["AlanId"].ToString()));
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    cmbAlanAdi.Text = datareader["AlanAdi"].ToString();
                    txtAciklama.Text = datareader["Bilgi"].ToString();

                }
                connect.Close();

                picResim.ImageLocation = dataset2.Tables[0].Rows[e.RowIndex]["ResimNo"].ToString()+ dataset2.Tables[0].Rows[e.RowIndex]["IslemeResimNo"].ToString();
            }
            catch
            {
                connect.Close();
            }


        }

        private void btnResimEkle_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosyasec = new OpenFileDialog();
            dosyasec.Title = "Dosya Seç;";
            dosyasec.Filter = "Jpeg Dosyalari|*.jpg|Png Dosyalari|*png|Tüm Dosyalar|*.*";
            if (dosyasec.ShowDialog() == DialogResult.OK)
                txtResimyol.Text = dosyasec.FileName;
            picResim.ImageLocation = txtResimyol.Text;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string yoneticiID ="Y1545";
            try
            {
                connect.Open();

                SqlCommand command2 = new SqlCommand("AlanIdCek", connect);
                command2.CommandType = CommandType.StoredProcedure;
                command2.Parameters.AddWithValue("@AlanAdi", cmbAlanAdi.Text);
                SqlDataReader datareader = command2.ExecuteReader();
                int alanid=0;
                while (datareader.Read())
                    alanid = Convert.ToInt32(datareader["AlanId"].ToString());
                datareader.Close();

                SqlCommand command = new SqlCommand("UrunEkle", connect);
                command.CommandType = CommandType.StoredProcedure;
                Random sayiuret = new Random();
                string sayi = sayiuret.Next(11111111, 99999999).ToString();
                if (!(System.IO.Directory.Exists(Application.StartupPath + @"\Gorsel")))
                    System.IO.Directory.CreateDirectory(Application.StartupPath + @"\Gorsel");
                System.IO.File.Copy(txtResimyol.Text, Application.StartupPath + @"\Gorsel\R" + sayi);
                command.Parameters.AddWithValue("@ResimNo", Application.StartupPath + @"\Gorsel\R");
                command.Parameters.AddWithValue("@IslemeResimNo", sayi);
                command.Parameters.AddWithValue("@ParcaAdi", txtParcaAdi.Text);
                command.Parameters.AddWithValue("@HamAgirlik", txtHamAgirlik.Text);
                command.Parameters.AddWithValue("@IslenmisAgirlik", txtIslenmisAgirlik.Text);
                command.Parameters.AddWithValue("@AmbalajSekli", cmbAmbalajSekli.Text);
                command.Parameters.AddWithValue("@AmbalajAdedi", txtAmbalajAdedi.Text);
                command.Parameters.AddWithValue("@YöneticiId", yoneticiID);
                command.Parameters.AddWithValue("@AlanId", alanid);
                
                command.ExecuteNonQuery();
                connect.Close();
                UrunleriListele();
                MessageBox.Show("Ürün başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception hata)
            { MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();
                SqlCommand command2 = new SqlCommand("AlanIdCek", connect);
                command2.CommandType = CommandType.StoredProcedure;
                command2.Parameters.AddWithValue("@AlanAdi", cmbAlanAdi.Text);
                SqlDataReader datareader = command2.ExecuteReader();
                int alanid = 0;
                while (datareader.Read())
                    alanid = Convert.ToInt32(datareader["AlanId"].ToString());
                datareader.Close();
                SqlCommand command = new SqlCommand("UrunGuncelle", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ParcaId", txtParcaId.Text);
                command.Parameters.AddWithValue("@ResimNo", Application.StartupPath + @"\Gorsel\R");
                if (txtResimyol.Text != "")
                {
                    Random sayiuret = new Random();
                    string sayi = sayiuret.Next(11111111, 99999999).ToString();
                    if (!(System.IO.Directory.Exists(Application.StartupPath + @"\Gorsel")))
                        System.IO.Directory.CreateDirectory(Application.StartupPath + @"\Gorsel");
                    System.IO.File.Copy(txtResimyol.Text, Application.StartupPath + @"\Gorsel\R" + sayi);
                    command.Parameters.AddWithValue("@IslemeResimNo", sayi);
                }
                else
                    command.Parameters.AddWithValue("@IslemeResimNo", txtIslemeResimNo.Text);
                command.Parameters.AddWithValue("@ParcaAdi", txtParcaAdi.Text);
                command.Parameters.AddWithValue("@HamAgirlik", txtHamAgirlik.Text);
                command.Parameters.AddWithValue("@IslenmisAgirlik", txtIslenmisAgirlik.Text);
                command.Parameters.AddWithValue("@AmbalajSekli", cmbAmbalajSekli.Text);
                command.Parameters.AddWithValue("@AmbalajAdedi", txtAmbalajAdedi.Text);
                command.Parameters.AddWithValue("@AlanId", alanid);
                command.ExecuteNonQuery();
                connect.Close();
                UrunleriListele();
                button13.PerformClick();
                MessageBox.Show("Ürün başarıyla guncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            { MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }
            private void button11_Click(object sender, EventArgs e)
        {
            Form_Login login = new Form_Login();
            login.Show();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try {
                DialogResult soru = MessageBox.Show("Urunu silmek istediginize emin misiniz ?", "Dikkat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (soru == DialogResult.Yes)
                {
                    connect.Open();
                    SqlCommand command = new SqlCommand("UrunSil", connect);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ParcaId", txtParcaId.Text);
                    command.ExecuteNonQuery();
                    connect.Close();
                    UrunleriListele();
                    MessageBox.Show("Urun basarıyla silindi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception hata)
            { MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            picResim.ImageLocation = null;
            txtResimyol.Text = "";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            AlanListele();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try {
                connect.Open();
                SqlCommand command = new SqlCommand("AlanEkle", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlanAdi",txtAlanAdi.Text);
                command.Parameters.AddWithValue("@Bilgi",txtBilgi.Text);
                command.ExecuteNonQuery();
                connect.Close();
                AlanListele();
                MessageBox.Show("Alan basarıyla Eklendi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            { MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
}

        private void dataGridView4_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtAlanId.Text = dataset3.Tables[0].Rows[e.RowIndex]["AlanId"].ToString();
                txtAlanAdi.Text = dataset3.Tables[0].Rows[e.RowIndex]["AlanAdi"].ToString();
                txtBilgi.Text = dataset3.Tables[0].Rows[e.RowIndex]["Bilgi"].ToString();
            }
            catch { }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try { 
            connect.Open();
            SqlCommand command = new SqlCommand("AlanGuncelle", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AlanId",txtAlanId.Text);
            command.Parameters.AddWithValue("@AlanAdi", txtAlanAdi.Text);
            command.Parameters.AddWithValue("@Bilgi", txtBilgi.Text);
            command.ExecuteNonQuery();
                connect.Close();
            AlanListele();
                  MessageBox.Show("Alan basariyla guncellendi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            { MessageBox.Show(hata.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                connect.Open();
                SqlCommand command = new SqlCommand("AlanSil", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlanId", txtAlanId.Text);
                command.ExecuteNonQuery();
                connect.Close();
                AlanListele();
                MessageBox.Show("Alan basariyla silindi", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception hata)
            { MessageBox.Show("Alan kullanımda silinemez!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connect.Close();
            }
        }
      
        private void button6_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = tabGrafik;
           
        }

        private string AyIntToStr(int ayno)
        {
            string[] aylar = { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            return aylar[ayno - 1];
        }
        private void btnGoruntule_Click(object sender, EventArgs e)
        {

            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            connect.Open();
            SqlCommand command = new SqlCommand("sp_ParcaAylikSiparisMiktari", connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ParcaAdi", SqlDbType.NVarChar).Value = textBox1.Text.Trim();
            command.ExecuteNonQuery();
            SqlDataReader oku = command.ExecuteReader();

            while (oku.Read())
            {
                chart1.Series["Aylar"].Points.AddXY(AyIntToStr(Convert.ToInt32(oku[2].ToString())), oku[3].ToString());
            }
   
            connect.Close();
        }

        void Rapor1()
        {
            connect.Open();
            tabRapor.SelectedTab = tabPage1;
            da = new SqlDataAdapter(" select ay,yil,m.ParcaId,ParcaAdi,sum(ParcaAdedi) as Adet from AyYilMaxParca m, ParcaBilgileri where m.ParcaId = ParcaBilgileri.ParcaId group by m.ParcaId, ParcaAdi, ay,yil having sum(ParcaAdedi) >= all(select max(ParcaAdedi)  from AyYilMaxParca n where n.ay = m.ay and n.yil = m.yil) ", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataTable newdt = new DataTable();
            newdt.Columns.Add(dt.Columns[0].ColumnName);
            newdt.Columns.Add(dt.Columns[1].ColumnName);
            newdt.Columns.Add(dt.Columns[2].ColumnName);
            newdt.Columns.Add(dt.Columns[3].ColumnName);
            newdt.Columns.Add(dt.Columns[4].ColumnName);



            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newRow = newdt.NewRow();
                newRow[dt.Columns[0].ColumnName] = AyIntToStr(Convert.ToInt32(dt.Rows[i][0].ToString()));
                newRow[dt.Columns[1].ColumnName] = dt.Rows[i][1].ToString();
                newRow[dt.Columns[2].ColumnName] = dt.Rows[i][2].ToString();
                newRow[dt.Columns[3].ColumnName] = dt.Rows[i][3].ToString();
                newRow[dt.Columns[4].ColumnName] = dt.Rows[i][4].ToString();
                newdt.Rows.Add(newRow);
            }

            dataGridView7.DataSource = newdt;

            

            connect.Close();
        }
        void Rapor2()
        {
            connect.Open();
            da = new SqlDataAdapter("select m.FirmaId,FirmaAdi,m.ParcaId,ParcaAdi,sum(ParcaAdedi) as Adet from FirmaMaxParca m,ParcaBilgileri ,MusteriFirma where m.ParcaId = ParcaBilgileri.ParcaId and m.FirmaId = MusteriFirma.FirmaId group by m.ParcaId,ParcaAdi,m.FirmaId,FirmaAdi having sum(ParcaAdedi) >=all (select max(ParcaAdedi)  from FirmaMaxParca n where n.FirmaId=m.FirmaId ) ", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewRapor2.DataSource = dt;
            connect.Close();
        }
        void Rapor3()
        {
            connect.Open();
            //da = new SqlDataAdapter("select month (s.SiparisTarihi) as Ay, p.ParcaAdi,s.SiparisKod,sum(ParcaAdedi) as Adet from Siparis s, ParcaBilgileri p where p.ParcaId = s.ParcaId group by p.ParcaAdi, s.SiparisKod, s.SiparisTarihi having sum(ParcaAdedi) >= ALL(select sum(ParcaAdedi) from Siparis s, ParcaBilgileri p where p.ParcaId = s.ParcaId group by p.ParcaAdi, s.SiparisKod)", connect);

            da = new SqlDataAdapter("select p.ParcaAdi, sum(ParcaAdedi) as Adet from Siparis s, ParcaBilgileri p where p.ParcaId = s.ParcaId group by p.ParcaAdi having sum(ParcaAdedi) >= ALL(select sum(ParcaAdedi) as adet  from Siparis s, ParcaBilgileri p where p.ParcaId = s.ParcaId group by p.ParcaAdi)", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewRapor3.DataSource = dt;
            connect.Close();
        }
        private void button20_Click(object sender, EventArgs e)
        {
            Rapor1();
            Rapor2();
            Rapor3();
        }
  
        void silinenMusteriListele()
        {
            connect.Open();
            da = new SqlDataAdapter("Select distinct IptalMusteriId,FirmaId,FirmaAdi,Eposta,Telefon From IptalMusteri", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewSilinenMusteri.DataSource = dt;
            connect.Close();
        }
        void silinenParcalarListele()
        {
            connect.Open();
            da = new SqlDataAdapter("Select distinct IptalParcaId,ParcaId,ParcaAdi From IptalParca", connect);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewSilinenParcalar.DataSource = dt;
            connect.Close();
        }
        private void btnSilinenMusteri_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = TabSilinen;
            silinenMusteriListele();
            silinenParcalarListele();
        }

        private void btnsilinenUrun_Click(object sender, EventArgs e)
        {
            tabRapor.SelectedTab = TabSilinen;
            silinenMusteriListele();
            silinenParcalarListele();
        }

        private void AlanListele()
        {
            try
            {
                connect.Open();
                SqlCommand command = new SqlCommand("AlanBilgiListele", connect);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataadapter = new SqlDataAdapter();
                dataadapter.SelectCommand = command;
                dataset3 = new DataSet();
                dataadapter.Fill(dataset3);
                dataGridView4.DataSource = dataset3.Tables[0];
                connect.Close();
            }
            catch {connect.Close(); }
        }

        
        private void button4_Click(object sender, EventArgs e)
        {

            tabRapor.SelectedTab = tabUrunAlanlari;
            AlanListele();


        }

        //private void grafik()
        //{
        //    string[] aylar = { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
        //    chart1.Titles.Add(dateTimePicker1.Text + " - " + dateTimePicker2.Text + " Arası");


        //    int[] aysayi = new int[12];
        //    connect.Open();
        //    SqlCommand command = new SqlCommand("Select *from Siparis where SiparisTarihi BETWEEN @Baslangic AND @Bitis", connect);
        //    command.Parameters.AddWithValue("@Baslangic", dateTimePicker1.Value.Date);
        //    command.Parameters.AddWithValue("@Bitis", dateTimePicker2.Value.Date);
        //    SqlDataReader datareader = command.ExecuteReader();
        //    while (datareader.Read())
        //    {
        //        string ay = datareader["SiparisTarihi"].ToString();
        //        ay = ay.Substring(ay.IndexOf(".")+1,2);
        //        if (ay.Contains("."))
        //            ay = ay.Substring(ay.IndexOf("."));
        //       // MessageBox.Show(ay);
        //       aysayi[Convert.ToInt32(ay)-1] += 1;

        //    }
        //    datareader.Close();
        //    connect.Close();
        //    for(int ayno=1; ayno<13; ayno++)
        //    {
        //        chart1.Series[0].Points.Add( aysayi[ayno-1]).Label= aylar[ayno - 1].ToString();
        //        chart1.Series[0].IsValueShownAsLabel = true;
        //    }


        //}



    }
}
