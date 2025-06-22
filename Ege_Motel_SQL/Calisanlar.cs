using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ege_Motel_SQL
{
    public partial class Calisanlar: Form
    {
        public Calisanlar()
        {
            InitializeComponent();
        }
        DataSet1TableAdapters.CalisanlarTableAdapter calisanalr=new DataSet1TableAdapters.CalisanlarTableAdapter();
        DataSet1TableAdapters.TumCalisanlarTableAdapter tumCalisanlar = new DataSet1TableAdapters.TumCalisanlarTableAdapter();
        private void Calisanlar_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = calisanalr.CalisanKaydet();



        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ad.Text=dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_soyad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_tc.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_gorev.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_adres.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txt_maas.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            dateTimePicker1.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
        }

        private void btn_ekle_Click(object sender, EventArgs e)
        {
            kontrol();
            if (hersey_hazir)
            {

                calisanalr.CalisanEKLEEE(txt_ad.Text.Trim(),txt_soyad.Text.Trim(),txt_tc.Text.Trim(),txt_gorev.Text.Trim(),txt_adres.Text.Trim(),
                    int.Parse(txt_maas.Text.Trim()),dateTimePicker1.Value);

                tumCalisanlar.TumKisiEkle(txt_ad.Text.Trim(), txt_soyad.Text.Trim(), txt_tc.Text.Trim(), txt_gorev.Text.Trim(),int.Parse(txt_maas.Text),txt_gorev.Text,dateTimePicker1.Value);
                MessageBox.Show("ekleme işlemi başarılı bir şekilde gerçekleşti");
                dataGridView1.DataSource = calisanalr.CalisanKaydet();
                hersey_hazir = false;
            }





        }

        bool hersey_hazir=false;
        void kontrol()
        {
            if(string.IsNullOrEmpty(txt_ad.Text)||
                string.IsNullOrEmpty(txt_gorev.Text)||
                string.IsNullOrEmpty(txt_adres.Text) ||
                string.IsNullOrEmpty(txt_maas.Text )||
                string.IsNullOrEmpty(txt_tc.Text)||
                string.IsNullOrEmpty(txt_soyad.Text))
            {
                MessageBox.Show("bilgileri eksiksiz girmelisiniz");
                hersey_hazir = false;
            }
            else
            {
                if (dateTimePicker1.Value < DateTime.Now)
                {
                    MessageBox.Show("işe alım tarihi bugunden önce olamaz");
                    hersey_hazir = false;
                }
                else
                {
                    MessageBox.Show("her şey hazır kayıt için");
                    hersey_hazir=true;
                }

            }
              
        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = calisanalr.CalisanKaydet();
        }

        private void button_sil_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txt_tc.Text))
            {
                MessageBox.Show("silmek için tc kimlik numarasını girin");
            }
            else
            {
                calisanalr.CalisanSil(txt_tc.Text.Trim());
                MessageBox.Show("silme işlemi başarılı bir şekilde gerçekleşti");
                dataGridView1.DataSource = calisanalr.CalisanKaydet();
            }




        }
        //tüm çalısanlara bakmak için
        private void button2_Click(object sender, EventArgs e)
        {
            TuMCalisanlar calisanlar = new TuMCalisanlar();
            calisanlar.Show();
            this.Hide();
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            kontrol();
            if (hersey_hazir)
            {

                calisanalr.CalisanGuncelle(txt_ad.Text.Trim(), txt_soyad.Text.Trim(), txt_tc.Text.Trim(), txt_gorev.Text.Trim(), txt_adres.Text.Trim(),
                int.Parse(txt_maas.Text.Trim()), dateTimePicker1.Value,txt_tc.Text.Trim());
                MessageBox.Show("güncelleme işlemi başarılı bir şekilde gerçekleşti");
                dataGridView1.DataSource = calisanalr.CalisanKaydet();
                hersey_hazir = false;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }
    }
}
