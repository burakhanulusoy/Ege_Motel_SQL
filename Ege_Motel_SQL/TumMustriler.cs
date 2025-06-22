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
    public partial class TumMustriler: Form
    {
        public TumMustriler()
        {
            InitializeComponent();
        }


        private void TumMustriler_Load(object sender, EventArgs e)
        {
            DataSet1TableAdapters.Kisiler_Hepsi1TableAdapter kisiler= new DataSet1TableAdapters.Kisiler_Hepsi1TableAdapter();
            dataGridView1.DataSource = kisiler.herkes();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }
    }
}
