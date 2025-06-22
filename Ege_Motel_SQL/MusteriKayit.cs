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
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace Ege_Motel_SQL
{
    public partial class MusteriKayit: Form
    {

        public MusteriKayit()
        {
            InitializeComponent();
        }
        // 2 sorgu alta oldugu için mecburi 2 bağşlantı açmam gerek
        SqlConnection baglanti = new SqlConnection("Data Source=HAN\\SQLEXPRESS;Initial Catalog=EgeMotel;Integrated Security=True");
        SqlConnection baglanti2 = new SqlConnection("Data Source=HAN\\SQLEXPRESS;Initial Catalog=EgeMotel;Integrated Security=True");

        //aynı oda da hem rezervasyon hemde dolu olursa oda rengi mor olacak
        private void MusteriKayit_Load(object sender, EventArgs e)
        {
            dateTimePicker_çıkış.Enabled = false;//ilk önce giriş tarihini seçtirmek istiyorum 
            dateTimePicker_giriş.Enabled=false;//ilk önce giriş tarihini seçtirmek istiyorum




            //combobox degerlerini data tabloma göre dolduruyorum
            comboboxYaz();
            txt_telefonno.TextMaskFormat = MaskFormat.IncludePromptAndLiterals;//sadece 11 hanlei telefon numarasını alacak
            odaRenkleriKontrol();//form açıldığında odaların renklerini kontrol etme işlemi



        }
              
        void odaRenkleriKontrol()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from OdayaKayıtYapanlar", baglanti); //kayıt olan kişilere bakcam sadece 
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())//bunun sayesinde gelen tablodaki her bir degeri okuyorum
            {
               
                int odaAd = int.Parse(dr["OdaAD"].ToString().Trim());//trim kullanmalıyım cunku sql den okurken basında sonunda bolsuk olabilir 
                string odaDurumu = dr["OdaDurumu"].ToString().Trim();// rezerve mi dolu mu ?

                if (odaDurumu.Equals("Dolu"))
                {
                   //sadece dolu olduğu için renk kırmızı olacak
                    odaRed("btn_" + odaAd);
                    

                }
                else if (odaDurumu.Equals("Rezerve"))
                {
                    //sadece rezerve olduğu için mavi olcaka
                    odaBlue("btn_" + odaAd);
                    
                }
               
            }
            baglanti.Close();

            //burda hem dolu hem rezerve olan oda bakılcak
            DoluAndRezerve();


        }

        void DoluAndRezerve()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from OdayaKayıtYapanlar", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                bool dolu = false;  //dolu
                bool rezerve = false;//rezerve

                string oda_no = dr["OdaAD"].ToString().Trim();//oda numarsından kontrol ediyoruz kayıtlı olanların hepsini bunu aşağıda kullancam
               
                baglanti2.Open();//baglantı2 kullanmamızın nedeni 2sorguda 2 farklu commadn kulllanmamız
                SqlCommand komut2 = new SqlCommand("select * from OdayaKayıtYapanlar where OdaAD=@p1", baglanti2);//sorgum bu odaya ait olabları getir
                komut2.Parameters.AddWithValue("@p1", oda_no);
                SqlDataReader dr2 = komut2.ExecuteReader();
                while (dr2.Read())
                {
                    if (dr2["OdaDurumu"].ToString().Trim() == "Dolu")
                    {
                        dolu = true;
                    }
                    else if (dr2["OdaDurumu"].ToString().Trim() == "Rezerve")
                    {
                        rezerve = true;
                    }
                }
                baglanti2.Close();//kapatmamaın nedeni asagıda dr kullanmam 2 dr ayni anda çalişmaz...
                if (dolu && rezerve)//hem dolu hem rezerve ise rengi mor olcak
                {
                    int odaAd = int.Parse(dr["OdaAD"].ToString().Trim());//trim kullanmalıyım cunku sql den okurken basında sonunda bolsuk olabilir 
                    odaRenkPurple("btn_" + odaAd);
                }
                else if(dolu && !rezerve)//sadece dolu ise kırmızı olcak
                {
                    int odaAd = int.Parse(dr["OdaAD"].ToString().Trim());//trim kullanmalıyım cunku sql den okurken basında sonunda bolsuk olabilir 
                    odaRed("btn_" + odaAd);
                }
                else if(!dolu && rezerve)//sadece rezerve ise mavi olcak
                {
                    int odaAd = int.Parse(dr["OdaAD"].ToString().Trim());//trim kullanmalıyım cunku sql den okurken basında sonunda bolsuk olabilir 
                    odaBlue("btn_" + odaAd);
                }
                else if (!dolu && !rezerve)//oda boş ise yeşil olcak
                {
                    int odaAd = int.Parse(dr["OdaAD"].ToString().Trim());//trim kullanmalıyım cunku sql den okurken basında sonunda bolsuk olabilir 
                    odaGreen("btn_" + odaAd);
                }
                else
                {
                    MessageBox.Show("Oda durumu kontrol edilemedi");
                }



            }
            baglanti.Close();



        }


   

        private void comboboxYaz() 
        {
            //sqlden veri alıp comboboxa yazdırma
            SqlCommand komut = new SqlCommand("Select * from tbl_oda_bosSuan", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox_odalar.DataSource = dt;//bunu koymazsanız çalışmaz combobox
            comboBox_odalar.DisplayMember = "OdaAd";//görünene değer
            comboBox_odalar.ValueMember = "OdaAd";// döndürülen değer

            // projemi sonradan yeni eklemeler yapınca oda ADı tutması benim için daha kolay oldu

        }


        //KONTROL MEKANİZMASI ÇALIŞMASI TAMAMEN TEXTBOX VE RADİOBUTTUN İŞARETLEME MANTIĞINA DAYALI DAHA KISA DA OLABİLİR MİŞ....

         bool kontrol_dogrumu;//kontrol dogru ise 1 olacak
         string cinsiyet;//radiobuttondan gelen cinsiyet bilgisi
         private void kontrol()
        {
            kontrol_dogrumu = false;//her seferinde kontrol mekanizmasını sıfırlıyorum
            //kontrol makenizması boşsa işlem yapılamayack çünkü dataya hepsini kaydetcem
            if ( String.IsNullOrEmpty(textBox_ad.Text) ||
                String.IsNullOrEmpty(textBox_soayd.Text) ||   
                String.IsNullOrEmpty(txt_telefonno.Text) ||
                String.IsNullOrEmpty(textBox_mail.Text) ||
                String.IsNullOrEmpty(textBox_ucret.Text) ||
                String.IsNullOrEmpty(txt_tc.Text) )
            {

                MessageBox.Show("Lütfen boş alan bırakmayınız");

            }
            else
            {
                //tc 11 haneli olcaka 
                if (txt_tc.Text.Length == 11)
                {
                    //telefon no konrtrolu
                    if (txt_telefonno.Text.Length == 14)
                    {
                        //giriş ve cıkış tarihi ayaraları
                        if (dateTimePicker_çıkış.Value.Date == dateTimePicker_giriş.Value.Date)
                        {
                            MessageBox.Show("Çıkış tarihi ile giriş tarihi aynı olamaz");
                        }
                        if (dateTimePicker_giriş.Value.Date > dateTimePicker_çıkış.Value.Date)
                        {
                            MessageBox.Show("Giriş tarihi çıkış tarihinden büyük olamaz");
                        }
                        if (dateTimePicker_giriş.Value.Date < dateTimePicker_çıkış.Value.Date)
                        {
                            //cisiyet secim kıontrolu
                            if (radioButton_erkek.Checked)
                            {
                                cinsiyet = radioButton_erkek.Text.ToString();
                                kontrol_dogrumu = true;
                               

                            }
                            else if (radioButton2.Checked)
                            {
                                cinsiyet = radioButton2.Text.ToString();
                                kontrol_dogrumu = true;
                            }
                            else
                            {
                                MessageBox.Show("Lütfen cinsiyet seçiniz");
                            }

                        }




                    }
                    else
                    {
                        MessageBox.Show("Telefon numarası 11 haneli olmalıdır");
                    }



                }
                else
                {

                 MessageBox.Show("TC Kimlik numarası 11 haneli olmalıdır");

                }                  
            }
        }

        private void Btn_Kaydet_Click(object sender, EventArgs e)
        {

                       kontrol();// ilk önce kontrol çalıssın ve içinden 1 ya da 0 dödürsün bool ile yappılmasi daha sağlikli v

                        if (kontrol_dogrumu)
                        {
                            /*ODA_KONTROL();
                            if(oda_durumu==0)
                            {
                                MessageBox.Show("Oda dolu");
                            }
                            else
                            {
                                MessageBox.Show("99999");
                                tarihKontrolET();
                            }*/
                            tarihKontrolET();
                        }
                        else
                        {
                            MessageBox.Show("Kişi bilgileri kontrol et.");
                        }

            
        }

        short oda_durumu = 0;
        void ODA_KONTROL()
        {
            // amacım oda dolu mu yoksa dolu değil mi


            bool kayıt_var = false;//amacım datada kımse yoksa direk kayıt yaptırmak

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from OdayaKayıtYapanlar where OdaAd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", comboBox_odalar.SelectedValue.ToString());
            SqlDataReader dr = komut.ExecuteReader();

            while (dr.Read())
            {
                kayıt_var = true;
                if (dr["OdaDurumu"].ToString() == "Dolu")
                {
                    MessageBox.Show("Oda şuan dolu.Sadece rezerve işlemi yapılabilir");

                }
                else if (dr["OdaDurumu"].ToString() == "Rezerve")
                {
                    MessageBox.Show("Oda rezerve var.Rezerve tarihine dikkat ediniz","Ege Motel",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    oda_durumu = 1;

                }
                else
                {
                    MessageBox.Show("Oda boş.İstenilen Tarihe randevu verilebilir");
                    oda_durumu = 1;
                }
            }


            if (kayıt_var == false)
            {
                MessageBox.Show("kimse yok");
                oda_durumu = 1;  //istenilen odada kimse olmadığı için işlem yapılabilir
            }

            baglanti.Close();
        }
        
        bool odada_biri_varmı = false;
        public void tarihKontrolET()
        {
            bool kayıt_var=false;//amacım datada kımse yoksa direk kayıt yaptırmak
            baglanti.Open();
        
            SqlCommand komut=new SqlCommand("select * from OdayaKayıtYapanlar where OdaAd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", comboBox_odalar.SelectedValue.ToString());
            
            SqlDataReader dr = komut.ExecuteReader();
            
            DateTime yeniGiriş = dateTimePicker_giriş.Value;
            DateTime yeniCıkış = dateTimePicker_çıkış.Value;

            
            bool tarih_sonuc = false;
            
            while (dr.Read())
            {
                kayıt_var = true;

                DateTime kayıtlıODATarihGiriş =DateTime.Parse(dr["GirisTarihi"].ToString());
                DateTime kayıtlıodaCıkıs=DateTime.Parse(dr["CikisTarihi"].ToString());


                if (yeniGiriş < kayıtlıodaCıkıs && yeniCıkış > kayıtlıODATarihGiriş)
                {
                    MessageBox.Show("Oda bu tarihlerde dolu.TARİH DEĞİŞTİRİN.");
                    tarih_sonuc = true;
                }
              
               
            }
            dr.Close();
            baglanti.Close();


            if (tarih_sonuc == false)
            {
               MessageBox.Show("Oda müsait bu tarihler arası");
               odada_biri_varmı=true;
               KayıtVarmı(odada_biri_varmı);
            }
            else
            {
                MessageBox.Show("Oda dolu bu tarihler arasında");
            }

            if(kayıt_var == false  )
            {
                MessageBox.Show("kimse yok ODADA ŞUAN");   
                islemYap();

            }
            

        }


        private void islemYap()
        {
                int gunluk_ucret = Convert.ToInt32(textBox_ucret.Text);
                int toplam_ucret = gunluk_ucret * (dateTimePicker_çıkış.Value.Date - dateTimePicker_giriş.Value.Date).Days;
                int toplam_gun = (dateTimePicker_çıkış.Value.Date - dateTimePicker_giriş.Value.Date).Days;
                string tc = txt_tc.Text.Trim();
            //kişilerin hepsi tablosuna kayıt
            try
            {

                DataSet1TableAdapters.Kisiler_HepsiTableAdapter kisiler = new DataSet1TableAdapters.Kisiler_HepsiTableAdapter();


                kisiler.TumeEkle(tc,
                    textBox_ad.Text,
                    textBox_soayd.Text,
                    txt_telefonno.Text,
                    textBox_mail.Text,
                    comboBox_odalar.SelectedValue.ToString(),
                    gunluk_ucret,
                    dateTimePicker_giriş.Value,
                    dateTimePicker_çıkış.Value,
                    toplam_gun,
                    toplam_ucret,
                    cinsiyet);
                //Kayıtlı oda tablosuna kayıt

                DataSet1TableAdapters.OdayaKayıtYapanlarTableAdapter odayakayıt = new DataSet1TableAdapters.OdayaKayıtYapanlarTableAdapter();

                short sayi = short.Parse(comboBox_odalar.SelectedValue.ToString());
                MessageBox.Show(sayi.ToString());
                string odadurumu;
                if (dateTimePicker_giriş.Value.Date == DateTime.Now.Date)
                {
                    odadurumu = "Dolu";

                    odaRed("btn_" + sayi);

                }
                else if (dateTimePicker_giriş.Value.Date > DateTime.Now.Date)
                {
                    odadurumu = "Rezerve";
                    odaBlue("btn_" + sayi);

                }
                else
                {

                    MessageBox.Show("Oda tarih sıkıntısı var");
                    odadurumu = "SIKINTI";

                }

                odayakayıt.TumKisilerekaydet(tc, textBox_ad.Text, txt_telefonno.Text, dateTimePicker_giriş.Value, dateTimePicker_çıkış.Value,
                comboBox_odalar.SelectedValue.ToString(), odadurumu);
            }
            catch
            {
               MessageBox.Show("Kayıt işlemi ONAYLANDI.","Ege Motel",MessageBoxButtons.OK);
            }
     
        }
        
        void KayıtVarmı( bool odada_biri_varmı)
        {
            if (odada_biri_varmı)
            {
                bool kayit_var_mı = false;
                baglanti.Open();
                SqlCommand komut3 = new SqlCommand("select * from OdayaKayıtYapanlar", baglanti);
                SqlDataReader dr = komut3.ExecuteReader();
                while (dr.Read())
                {
                    string kontrol_tc = dr["KisiTc"].ToString().Trim();
                    if (kontrol_tc.Equals(txt_tc.Text.Trim()))
                    {
                        MessageBox.Show("kişi zaten otele randevusu var güncellemek istiyor musunuz ?");

                        //BURAYA MUSTERİ GUNCELLEME FORMU GELECEK YUKLENECEK


                        kayit_var_mı = true;
                        break;
                    }

                }

                if (kayit_var_mı)
                {
                    MessageBox.Show("Kayıt var efendim güncelleme yapılacak");
                }
                else
                {

                    islemYap();
                }

                baglanti.Close();

            }
            else
            {
                MessageBox.Show("Oda dolu bu tarihler arasında ama AMAAAAAAAAAAAAAAAAAAAAAA");
                islemYap();
            }



        }
        private void odaRed(string odaAdı)
        {
            foreach (Control ctrl in groupbox.Controls)
            {
                if (ctrl is Button btn && btn.Name == odaAdı)
                {
                    btn.BackColor = Color.Red;
                    return;
                }
            }
        }
        private void odaBlue(string odaAdı)
        {
            foreach (Control ctrl in groupbox.Controls)
            {
                if (ctrl is Button btn && btn.Name == odaAdı)
                {
                    btn.BackColor = Color.Blue;
                    return;
                }
            }
        }
        private void odaRenkPurple(string v)
        {
            foreach (Control ctrl in groupbox.Controls)
            {
                if (ctrl is Button btn && btn.Name == v)
                {
                    btn.BackColor = Color.Purple;
                    return;
                }
            }


        }
        private void odaGreen(string odaAdı)
        {
            foreach (Control ctrl in groupbox.Controls)
            {
                if (ctrl is Button btn && btn.Name == odaAdı)
                {
                    btn.BackColor = Color.Lime;
                    return;
                }
            }
        }



        public void odaDurumuGüncelle()
        {
            DateTime bugun = DateTime.Now;

            DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter odayaKayıtYapanlar = new DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from OdayaKayıtYapanlar", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                DateTime cikis =DateTime.Parse(dr["CikisTarihi"].ToString().Trim());
                string tc = dr["KisiTc"].ToString().Trim();
                string odaNo = dr["OdaAD"].ToString().Trim();
                string ad = dr["KisiAd"].ToString().Trim();


                if (bugun > cikis)
                {
                    DialogResult sonuc=MessageBox.Show(tc+ " " + ad + "  "+odaNo+"  numarali odanın süresi bitti güncelleme yapılsın mı?","EGE MOTEL",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                    if (sonuc == DialogResult.OK)
                    {
                        MusteriAyarlari musteriAyarlari = new MusteriAyarlari();
                        




                    }
                    else if (sonuc == DialogResult.Cancel)
                    {
                     DialogResult sonuc2=MessageBox.Show("Kayıt silinmesini oanaylıyor musunuz?","EGE MOTEL",MessageBoxButtons.OKCancel);

                        if(sonuc2== DialogResult.OK)
                        {

                            odayaKayıtYapanlar.TarihiBiteniSil(tc);
                            MessageBox.Show("Kayıt silindi","EGE MOTEL");



                        }
                        else if(sonuc2== DialogResult.Cancel)
                        {
                            MessageBox.Show("Kayıt silinmedi.İŞLEM YENİDEN BAŞLIYOR !!!", "EGE MOTEL");
                            odaDurumuGüncelle();
                            baglanti.Close();

                        }
                        else
                        {
                            MessageBox.Show("İşlem iptal edildi İŞLEM YENİDEN BAŞLIYOR !!!");
                            odaDurumuGüncelle();
                            baglanti.Close();
                        }


                    }
                    else
                    {
                        MessageBox.Show("İşlem iptal edildi  İŞLEM YENİDEN BAŞLIYOR !!!");
                        odaDurumuGüncelle();
                        baglanti.Close();
                    }
                                      



                }


            }

                  baglanti.Close();
         

        }







        private void comboBox_odalar_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void dateTimePicker_çıkış_ValueChanged(object sender, EventArgs e)
        {
            if(dateTimePicker_çıkış.Value.Date < dateTimePicker_giriş.Value.Date)
            {
                MessageBox.Show("Çıkış tarihi giriş tarihinden küçük olamaz");
                dateTimePicker_çıkış.Value = dateTimePicker_giriş.Value.AddDays(1);
            }
            try
            {
                textBox_toplamgun.Text = (dateTimePicker_çıkış.Value.Date - dateTimePicker_giriş.Value.Date).Days.ToString();
                int toplsmucret = Convert.ToInt32(textBox_ucret.Text) * (dateTimePicker_çıkış.Value.Date - dateTimePicker_giriş.Value.Date).Days;
                textBox_toplamucret.Text = toplsmucret.ToString();
            
            }
            catch
            {
                MessageBox.Show("Lütfen tarih bilgilerini doğru giriniz.");
            }




        }//cıkıs gırıs ayarla


        private void txt_telefonno_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btn_yenidenBaslat_Click(object sender, EventArgs e)
        {   
           
        }

        private void dateTimePicker_giriş_ValueChanged(object sender, EventArgs e)
        {
            




            dateTimePicker_çıkış.Enabled = true;
        }

        private void textBox_ucret_TextChanged(object sender, EventArgs e)
        {
            if(dateTimePicker_giriş.Value <= DateTime.Now.Date)
            {
                MessageBox.Show("Giriş tarihi bugünden küçük olamaz");
                dateTimePicker_giriş.Value = DateTime.Now.Date;
            }
            dateTimePicker_giriş.Enabled = true;

        }

        private void button_TEMİZLE_Click(object sender, EventArgs e)
        {
            dateTimePicker_çıkış.Enabled = false;
            dateTimePicker_giriş.Enabled = false;

            foreach (Control ctrl in groupBox1.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Clear();
               
            }
            txt_tc.Clear();
            txt_telefonno.Clear();
            radioButton_erkek.Checked = false;
            radioButton2.Checked = false;
        }

       

        private void btn_101_Click(object sender, EventArgs e)
        {

            Button basilanButon = sender as Button;
            string odaNo = basilanButon.Text;

            // Örnek: başka forma gönderme
            

            OdaDurumAnlık odaDurumAnlık = new OdaDurumAnlık();
            odaDurumAnlık.gelenoda(odaNo);
            odaDurumAnlık.Show();
            this.Hide();


        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }

        private void groupbox_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            odaDurumuGüncelle();
            odaRenkleriKontrol();
        }
    }
}
