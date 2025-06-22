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
    public partial class OdaDurumAnlık: Form
    {
        public OdaDurumAnlık()
        {
            InitializeComponent();
        }

        string odaDurum;
        public void gelenoda(string odaAd)
        {
            odaDurum = odaAd;



        }

        private void OdaDurumAnlık_Load(object sender, EventArgs e)
        {

            DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter odayaKayıtYapanlar = new DataSet1TableAdapters.OdayaKayıtYapanlar1TableAdapter();
            dataGridView1.DataSource = odayaKayıtYapanlar.OdaNuma(odaDurum);



        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusteriKayit musteriKayit= new MusteriKayit();
            musteriKayit.Show();
            this.Hide();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();


        }
    }
}
