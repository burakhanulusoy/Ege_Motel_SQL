using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ege_Motel_SQL
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
            label_saat.Text = saat.ToString();
            label_dakika.Text = saat.ToString();
            lbl_tarih.Text=DateTime.Now.ToShortDateString();

        }
       
        int saat = DateTime.Now.Hour;
        int dakika = DateTime.Now.Minute;
        int saniye = DateTime.Now.Second;

        private void timer1_Tick(object sender, EventArgs e)
        {
            saniye++;
            if(saniye==60)
            {
                saniye = 0;
                dakika++;
            }
            if(dakika == 60)
            {
                dakika = 0;
                saat++;
            }
            if (saat == 24)
            {
                saat = 0;
            }
            label_saat.Text = saat.ToString();
            label_dakika.Text = dakika.ToString();
            label_saniye.Text = saniye.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet1TableAdapters.AdminTableAdapter admin = new DataSet1TableAdapters.AdminTableAdapter();

            var sonuc = admin.AdminKontrolET(txt_KullancıAdi.Text, int.Parse(txt_Sifre.Text));
            if(sonuc.Rows.Count>0)
            {
                MenuEkrani menuEkrani = new MenuEkrani();
                menuEkrani.Show();
                this.Hide();


            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
            }



        }
    }
}


