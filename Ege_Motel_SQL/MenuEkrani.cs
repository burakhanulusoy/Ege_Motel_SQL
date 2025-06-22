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
    public partial class MenuEkrani: Form
    {
        public MenuEkrani()
        {
            InitializeComponent();
        }

        private void btn_MusteriEkle_Click(object sender, EventArgs e)
        {
            MusteriKayit musteriKayit = new MusteriKayit();
            musteriKayit.Show();
            this.Hide();
        }

        private void btn_musterAyar_Click(object sender, EventArgs e)
        {
            MusteriAyarlari musteriAyarlari = new MusteriAyarlari();
            musteriAyarlari.Show();
            this.Hide();

        }

        private void btn_TumMusteriKayıt_Click(object sender, EventArgs e)
        {
            TumMustriler tumMusteri = new TumMustriler();
            tumMusteri.Show();
            this.Hide();


        }

        private void btn_AdminAyarları_Click(object sender, EventArgs e)
        {

            AdminAyarlari adminAyarlari = new AdminAyarlari();
            adminAyarlari.Show();
            this.Hide();


        }

        private void btn_Calisan_Click(object sender, EventArgs e)
        {
            Calisanlar calisanlar = new Calisanlar();
            calisanlar.Show();
            this.Hide();


        }
    }
}
