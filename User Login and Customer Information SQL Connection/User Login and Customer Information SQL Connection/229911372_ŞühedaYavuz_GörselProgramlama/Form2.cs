using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _229911372_ŞühedaYavuz_GörselProgramlama
{
    public partial class Form2 : Form
    {
        // Veritabanı bağlantı dizesi
        private string connectionString = "Data Source=SUHEDAYAVUZ\\SQLEXPRESS;Initial Catalog=AraçFabrikası;Integrated Security=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        public Form2()
        {
            InitializeComponent();
        }

        // Verileri DataGridView'e yükleme metodu
        public void Listele()
        {
            using (connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM AracBilgileri";
                command = new SqlCommand(query, connection);
                adapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        // Form yüklendiğinde çalışacak kodlar
        private void Form2_Load(object sender, EventArgs e)
        {
            // Veritabanından verileri yükle
            this.aracBilgileriTableAdapter4.Fill(this.araçFabrikasıDataSet4.AracBilgileri);
            Listele();

            // DataGridView sütun adlarını kontrol etme
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                Console.WriteLine(column.Name);
            }
        }

        // DataGridView'de bir hücreye tıklandığında çalışacak kodlar
        private void dgvtablo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Seçilen satırın bilgilerini TextBox'lara yazma
            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
            textıd.Text = row.Cells["Arac_ID"].Value.ToString();
            txtaractürü.Text = row.Cells["Arac_Turu"].Value.ToString();
            txtaracmarkasi.Text = row.Cells["Arac_Markasi"].Value.ToString();
            txtaracmodeli.Text = row.Cells["Arac_Modeli"].Value.ToString();
            txturetimbaslangic.Text = row.Cells["Arac_Uretim_Baslangic_Tarihi"].Value.ToString();
            txturetimbitis.Text = row.Cells["Arac_Uretim_Bitis_Tarihi"].Value.ToString();
            txtmaliyettutari.Text = row.Cells["Arac_Maliyet_Tutari"].Value.ToString();
            txtaracsatisdurumu.Text = row.Cells["Arac_Satis_Durumu"].Value.ToString();

            // Arac_Ödenen_Miktar sütununu okuyup textaracodenmiktar TextBox'ına yazma
            if (dataGridView1.Columns.Contains("Arac_Ödenen_Miktar"))
            {
                txtaracodenmiktar.Text = row.Cells["Arac_Ödenen_Miktar"].Value.ToString();
            }
        }

        // Kaydet butonuna tıklandığında çalışacak kodlar
        private void btnkaydet_Click(object sender, EventArgs e)
        {
            // Arac_ID boş olamaz kontrolü
            if (string.IsNullOrEmpty(textıd.Text))
            {
                MessageBox.Show("Arac_ID boş olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Veritabanına yeni kayıt ekleme
            using (connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO AracBilgileri (Arac_ID, Arac_Turu, Arac_Markasi, Arac_Modeli, Arac_Uretim_Baslangic_Tarihi, Arac_Uretim_Bitis_Tarihi, Arac_Maliyet_Tutari, Arac_Satis_Durumu, Arac_Ödenen_Miktar) " +
                               "VALUES (@Arac_ID, @Arac_Turu, @Arac_Markasi, @Arac_Modeli, @Arac_Uretim_Baslangic_Tarihi, @Arac_Uretim_Bitis_Tarihi, @Arac_Maliyet_Tutari, @Arac_Satis_Durumu, @Arac_Ödenen_Miktar)";

                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Arac_ID", textıd.Text);
                command.Parameters.AddWithValue("@Arac_Turu", txtaractürü.Text);
                command.Parameters.AddWithValue("@Arac_Markasi", txtaracmarkasi.Text);
                command.Parameters.AddWithValue("@Arac_Modeli", txtaracmodeli.Text);

                // Üretim başlangıç tarihi kontrolü
                DateTime uretimBaslangic;
                if (DateTime.TryParse(txturetimbaslangic.Text, out uretimBaslangic))
                {
                    if (uretimBaslangic < SqlDateTime.MinValue.Value || uretimBaslangic > SqlDateTime.MaxValue.Value)
                    {
                        MessageBox.Show("Üretim başlangıç tarihi geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    command.Parameters.AddWithValue("@Arac_Uretim_Baslangic_Tarihi", uretimBaslangic);
                }
                else
                {
                    MessageBox.Show("Üretim başlangıç tarihi geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Üretim bitiş tarihi kontrolü
                DateTime uretimBitis;
                if (DateTime.TryParse(txturetimbitis.Text, out uretimBitis))
                {
                    if (uretimBitis < SqlDateTime.MinValue.Value || uretimBitis > SqlDateTime.MaxValue.Value)
                    {
                        MessageBox.Show("Üretim bitiş tarihi geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    command.Parameters.AddWithValue("@Arac_Uretim_Bitis_Tarihi", uretimBitis);
                }
                else
                {
                    MessageBox.Show("Üretim bitiş tarihi geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Maliyet tutarı kontrolü
                decimal maliyetTutari;
                if (decimal.TryParse(txtmaliyettutari.Text, out maliyetTutari))
                {
                    command.Parameters.AddWithValue("@Arac_Maliyet_Tutari", maliyetTutari);
                }
                else
                {
                    MessageBox.Show("Maliyet tutarı geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Satış durumu ComboBox'undan seçilen değeri alalım
                int satisDurumu = 0; // Default olarak satılmadı olarak ayarlayalım
                if (txtaracsatisdurumu.SelectedItem != null)
                {
                    if (txtaracsatisdurumu.SelectedItem.ToString() == "True")
                    {
                        satisDurumu = 1; // Satıldı ise 1 olarak ayarlayalım
                    }
                }

                command.Parameters.AddWithValue("@Arac_Satis_Durumu", satisDurumu);

                // Ödenen miktar TextBox'ından değeri alalım
                decimal odenenMiktar = 0;
                if (!string.IsNullOrEmpty(txtaracodenmiktar.Text))
                {
                    if (decimal.TryParse(txtaracodenmiktar.Text, out odenenMiktar))
                    {
                        command.Parameters.AddWithValue("@Arac_Ödenen_Miktar", odenenMiktar);
                    }
                    else
                    {
                        MessageBox.Show("Ödenen miktar geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@Arac_Ödenen_Miktar", DBNull.Value); // Eğer boşsa NULL olarak ekle
                }

                // Veritabanına bağlan ve komutu çalıştır
                connection.Open();
                command.ExecuteNonQuery();
            }

            Listele(); // DataGridView'i güncelleme işlemi

            // Arac_ID TextBox'ını temizle
            textıd.Clear();

            // DataGridView'de eklenen son satırı göster
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            dataGridView1.Rows[dataGridView1.RowCount - 1].Selected = true;
        }

        // Görüntüle butonuna tıklandığında çalışacak kodlar
        private void btngörüntüle_Click(object sender, EventArgs e)
        {
            // DataGridView'de bir satır seçilmiş mi kontrol ediyoruz
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Seçili satırı alıyoruz
                DataGridViewRow selectedRow = dataGridView1.Rows[dataGridView1.SelectedRows[0].Index];

                // Hücrelerden değerleri alıyoruz
                string aracID = selectedRow.Cells[0].Value.ToString(); // Arac_ID sütununun indeksi 0
                string aracTuru = selectedRow.Cells[1].Value.ToString(); // Arac_Turu sütununun indeksi 1
                string aracMarkasi = selectedRow.Cells[2].Value.ToString(); // Arac_Markasi sütununun indeksi 2
                string aracModeli = selectedRow.Cells[3].Value.ToString(); // Arac_Modeli sütununun indeksi 3
                string aracUretimBaslangic = selectedRow.Cells[4].Value.ToString(); // Arac_Uretim_Baslangic_Tarihi sütununun indeksi 4
                string aracUretimBitis = selectedRow.Cells[5].Value.ToString(); // Arac_Uretim_Bitis_Tarihi sütununun indeksi 5
                string aracMaliyetTutari = selectedRow.Cells[6].Value.ToString(); // Arac_Maliyet_Tutari sütununun indeksi 6
                string aracSatisDurumu = selectedRow.Cells[7].Value.ToString() == "1" ? "Satıldı" : "Satılmadı"; // Arac_Satis_Durumu sütununun indeksi 7
                string aracOdenenMiktar = selectedRow.Cells[8].Value.ToString(); // Arac_Ödenen_Miktar sütununun indeksi 8

                // Tüm bilgileri birleştirip MessageBox'ta gösteriyoruz
                string aracBilgileri = $"Arac ID: {aracID}\n" +
                                       $"Arac Turu: {aracTuru}\n" +
                                       $"Arac Markasi: {aracMarkasi}\n" +
                                       $"Arac Modeli: {aracModeli}\n" +
                                       $"Uretim Baslangic Tarihi: {aracUretimBaslangic}\n" +
                                       $"Uretim Bitis Tarihi: {aracUretimBitis}\n" +
                                       $"Maliyet Tutari: {aracMaliyetTutari}\n" +
                                       $"Satis Durumu: {aracSatisDurumu}\n" +
                                       $"Ödenen Miktar: {aracOdenenMiktar}";

                // Bilgileri göster
                MessageBox.Show(aracBilgileri, "Araç Bilgileri", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Satır seçilmemişse hata mesajı göster
                MessageBox.Show("Lütfen görüntülemek için bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnsil_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan silme işlemini onaylamasını isteyen dialog gösteriliyor
            DialogResult result = MessageBox.Show("Seçili satırı silmek istediğinizden emin misiniz?", "Silme işlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            
            if (result == DialogResult.Yes)
            {
                // DataGridView'de seçili satır var mı kontrol ediliyor
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;

                    // Seçili satırın Arac_ID değerini alıyoruz
                    DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];
                    DataGridViewCell cellAracID = selectedRow.Cells[0]; // Arac_ID sütununun indeksi 0

                    if (cellAracID != null && cellAracID.Value != null)
                    {
                        string aracID = cellAracID.Value.ToString();

                        // Veritabanından silme işlemi yapılıyor
                        using (connection = new SqlConnection(connectionString))
                        {
                            string query = "DELETE FROM AracBilgileri WHERE Arac_ID = @Arac_ID";
                            command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@Arac_ID", aracID);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        // DataGridView'i güncelle
                        Listele(); // DataGridView'i güncelle
                        MessageBox.Show("Seçili satır başarıyla silindi.", "Silme İşlemi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Geçersiz Arac_ID durumu için hata mesajı göster
                        MessageBox.Show("Seçili satırın Arac_ID değeri geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Satır seçilmemişse uyarı mesajı göster
                    MessageBox.Show("Lütfen bir satır seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btndüzenle_Click(object sender, EventArgs e)
        {
            // DataGridView'de bir satır seçilmiş mi kontrol ediyoruz
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Seçili satırın değerlerini TextBox'lara yazıyoruz
                textıd.Text = selectedRow.Cells[0].Value.ToString(); // Arac_ID sütununun indeksi 0
                txtaractürü.Text = selectedRow.Cells[1].Value.ToString(); // Arac_Turu sütununun indeksi 1
                txtaracmarkasi.Text = selectedRow.Cells[2].Value.ToString(); // Arac_Markasi sütununun indeksi 2
                txtaracmodeli.Text = selectedRow.Cells[3].Value.ToString(); // Arac_Modeli sütununun indeksi 3
                txturetimbaslangic.Text = selectedRow.Cells[4].Value.ToString(); // Arac_Uretim_Baslangic_Tarihi sütununun indeksi 4
                txturetimbitis.Text = selectedRow.Cells[5].Value.ToString(); // Arac_Uretim_Bitis_Tarihi sütununun indeksi 5
                txtmaliyettutari.Text = selectedRow.Cells[6].Value.ToString(); // Arac_Maliyet_Tutari sütununun indeksi 6
                txtaracsatisdurumu.Text = selectedRow.Cells[7].Value.ToString(); // Arac_Satis_Durumu sütununun indeksi 7
                txtaracodenmiktar.Text = selectedRow.Cells[8].Value.ToString(); // Arac_Ödenen_Miktar sütununun indeksi 8

                // Kullanıcıya düzenlemek isteyip istemediğini soruyoruz
                DialogResult result = MessageBox.Show("Seçili satırı düzenlemek istediğinizden emin misiniz?", "Düzenleme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Düzenlemeyi onayladıysa veritabanında güncelleme işlemi yapılıyor
                    string aracID = selectedRow.Cells[0].Value.ToString();

                    using (connection = new SqlConnection(connectionString))
                    {
                        string query = "UPDATE AracBilgileri SET Arac_Turu = @Arac_Turu, Arac_Markasi = @Arac_Markasi, Arac_Modeli = @Arac_Modeli, " +
                                       "Arac_Uretim_Baslangic_Tarihi = @Arac_Uretim_Baslangic_Tarihi, Arac_Uretim_Bitis_Tarihi = @Arac_Uretim_Bitis_Tarihi, " +
                                       "Arac_Maliyet_Tutari = @Arac_Maliyet_Tutari, Arac_Satis_Durumu = @Arac_Satis_Durumu, Arac_Ödenen_Miktar = @Arac_Ödenen_Miktar " +
                                       "WHERE Arac_ID = @Arac_ID";

                        command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Arac_ID", aracID);
                        command.Parameters.AddWithValue("@Arac_Turu", txtaractürü.Text);
                        command.Parameters.AddWithValue("@Arac_Markasi", txtaracmarkasi.Text);
                        command.Parameters.AddWithValue("@Arac_Modeli", txtaracmodeli.Text);
                        command.Parameters.AddWithValue("@Arac_Uretim_Baslangic_Tarihi", DateTime.Parse(txturetimbaslangic.Text));
                        command.Parameters.AddWithValue("@Arac_Uretim_Bitis_Tarihi", DateTime.Parse(txturetimbitis.Text));
                        command.Parameters.AddWithValue("@Arac_Maliyet_Tutari", decimal.Parse(txtmaliyettutari.Text));
                        command.Parameters.AddWithValue("@Arac_Ödenen_Miktar", decimal.Parse(txtaracodenmiktar.Text));

                        // Arac_Satis_Durumu kontrolünün değerine göre parametre ekleme
                        int satisDurumuValue = txtaracsatisdurumu.Text == "1" ? 1 : 0;
                        command.Parameters.AddWithValue("@Arac_Satis_Durumu", satisDurumuValue);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    Listele(); // DataGridView'i güncelle
                    Temizle(); // TextBox'ları temizle
                }
                else
                {
                    MessageBox.Show("Düzenleme işlemi iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen düzenlemek için bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Temizle()
        {
            textıd.Text = "";
            txtaractürü.Text = "";
            txtaracmarkasi.Text = "";
            txtaracmodeli.Text = "";
            txturetimbaslangic.Text = "";
            txturetimbitis.Text = "";
            txtmaliyettutari.Text = "";
            txtaracsatisdurumu.Text = "";
            txtaracodenmiktar.Text = "";
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnhesapla_Click(object sender, EventArgs e)
        {
            // Arac maliyet tutarını alalım
            decimal aracMaliyetTutari;
            if (!decimal.TryParse(txtmaliyettutari.Text, out aracMaliyetTutari))
            {
                MessageBox.Show("Arac maliyet tutarı geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Arac ödenen miktarı alalım
            decimal aracOdenenMiktar;
            if (!decimal.TryParse(txtaracodenmiktar.Text, out aracOdenenMiktar))
            {
                MessageBox.Show("Arac ödenen miktarı geçersiz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kalan borcu hesaplayalım
            decimal kalanBorc = aracMaliyetTutari - aracOdenenMiktar;

            // Sonucu kullanıcıya gösterelim
            MessageBox.Show($"Kalan Borç: {kalanBorc}", "Kalan Borç Hesaplama", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
