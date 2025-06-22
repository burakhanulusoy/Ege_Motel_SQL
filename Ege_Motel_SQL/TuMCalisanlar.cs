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
    public partial class TuMCalisanlar: Form
    {
        public TuMCalisanlar()
        {
            InitializeComponent();
        }

        DataSet1TableAdapters.TumCalisanlarTableAdapter tumCalisanlar = new DataSet1TableAdapters.TumCalisanlarTableAdapter();
        private void TuMCalisanlar_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = tumCalisanlar.TumCalisanlarGoster();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Calisanlar calisanlar = new Calisanlar();
            calisanlar.Show();
            this.Hide();
        }
    }
}
