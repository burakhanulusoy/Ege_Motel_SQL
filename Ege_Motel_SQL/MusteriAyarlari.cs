using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ege_Motel_SQL
{
    public partial class MusteriAyarlari: Form
    {
        public MusteriAyarlari()
        {
            InitializeComponent();
        }
        DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter odayaKayıt = new DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter();

        private void btn_listele_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = odayaKayıt.oda_son_kez();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //dataya dokununca çalısan metot
            try
            {
                txt_AD.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txt_Tc.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                dtm_cikis.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                txt_Oda.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                txt_TelNo.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtmpic_giris.Value = DateTime.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
            catch
            {
                MessageBox.Show("Hata oluştu. Lütfen tekrar deneyiniz");
            }

        }

        private void btn_sil_Click(object sender, EventArgs e)
        {
            DialogResult a=MessageBox.Show("Silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (a == DialogResult.Yes)
            {
                if (txt_Tc.Text.Trim().Length == 11)
                {
                    odayaKayıt.TarihiBiteniSil(txt_Tc.Text.Trim());
                    MessageBox.Show("Silme işlemi başarılı");
                    dataGridView1.DataSource = odayaKayıt.oda_son_kez();
                }
                else
                {
                    MessageBox.Show("Silme işlemi iptal edildi.11 haneli Tc Doğru giriniz! ");
                }
            }
            else
            {
                MessageBox.Show("Silme işlemi iptal edildi");
            }
        }

        private void btn_ara_Click(object sender, EventArgs e)
        {
            if(txt_Tc.Text.Trim().Length == 11)
            {
             dataGridView1.DataSource = odayaKayıt.TcAra(txt_Tc.Text.Trim());
            }
            else if(txt_Oda.Text.Trim().Length == 3)
            {
            dataGridView1.DataSource=odayaKayıt.OdaNuma(txt_Oda.Text.Trim());     
            }
            else if(txt_Tc.Text.Trim().Length ==11  && txt_Oda.Text.Trim().Length == 3)
            {
                dataGridView1.DataSource = odayaKayıt.OdaNuma(txt_Oda.Text.Trim());
            }
            else
            {
                MessageBox.Show("Arama işlemi iptal edildi.11 haneli Tc ya da Oda Numarası giriniz");
            }
        }




        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (dtmpic_giris.Value < dtm_cikis.Value)
            {
                tarih_Degistri();
            }
            else if(dtmpic_giris.Value == dtm_cikis.Value)
            {
                MessageBox.Show("Giriş tarihi çıkış tarihiyle aynı olamaz");
            }
            else 
            {
                MessageBox.Show("Giriş tarihi çıkış tarihinden büyük olamaz");
            }
            


        }
          
   

        private void dtmpic_giris_ValueChanged(object sender, EventArgs e)
        {
            dtm_cikis.Enabled = true;
        }

        private void dtm_cikis_ValueChanged(object sender, EventArgs e)
        {
           



        }



        SqlConnection baglanti = new SqlConnection("Data Source=HAN\\SQLEXPRESS;Initial Catalog=EgeMotel;Integrated Security=True");
        public void tarih_Degistri()
        {
            bool tarih_sonuc = false;
            baglanti.Open();

            SqlCommand komut = new SqlCommand("select * from OdayaKayıtYapanlar where OdaAd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1",txt_Oda.Text.Trim());

            SqlDataReader dr = komut.ExecuteReader();

            DateTime yeniGiriş = dtmpic_giris.Value.Date;
            DateTime yeniCıkış = dtm_cikis.Value.Date;

            while (dr.Read())
            {

                DateTime kayıtlıODATarihGiriş = DateTime.Parse(dr["GirisTarihi"].ToString());
                DateTime kayıtlıodaCıkıs = DateTime.Parse(dr["CikisTarihi"].ToString());


                if (yeniGiriş < kayıtlıodaCıkıs.Date && yeniCıkış > kayıtlıODATarihGiriş.Date)
                {
                    MessageBox.Show("Oda bu tarihlerde dolu.TARİH DEĞİŞTİRİN.");
                    tarih_sonuc = true;
                }


            }
            dr.Close();
            baglanti.Close();


            if (tarih_sonuc == false)
            {
                DialogResult a=MessageBox.Show("Oda müsait bu tarihler arası.İşlem yapılsın mı?","EGE MOTEL",MessageBoxButtons.YesNo);
                if (a == DialogResult.Yes)
                {
                    if (dtmpic_giris.Value > DateTime.Now)
                    {
                        odayaKayıt.KayıtGuncelle(dtmpic_giris.Value, dtm_cikis.Value, txt_Oda.Text.Trim(), "Rezerve",txt_Tc.Text.Trim());
                        dataGridView1.DataSource = odayaKayıt.oda_son_kez();
                    }
                    else if (dtmpic_giris.Value.Date == DateTime.Now.Date)
                    {
                        odayaKayıt.KayıtGuncelle(dtmpic_giris.Value, dtm_cikis.Value, txt_Oda.Text.Trim(), "Dolu", txt_Tc.Text.Trim());
                        dataGridView1.DataSource = odayaKayıt.oda_son_kez();
                    }
                    else
                    {
                        MessageBox.Show("Giriş tarihi bugünden küçük olamaz");
                    }
                }
                else if(a == DialogResult.No)
                {
                    MessageBox.Show("İşlem İptal edildi");
                }
            }
            else
            {
                MessageBox.Show("Oda dolu");
            }
        }

        private void MusteriAyarlari_Load(object sender, EventArgs e)
        {
            dtm_cikis.Enabled = false;
        }
        //musteri kayıta git
        private void button6_Click(object sender, EventArgs e)
        {
            MusteriKayit musteriKayit = new MusteriKayit();
            musteriKayit.Show();
            this.Hide();
        }
        //menüye düö
        private void button5_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }
    }
}
