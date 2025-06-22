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
    public partial class AdminAyarlari: Form
    {
        public AdminAyarlari()
        {
            InitializeComponent();
        }

        DataSet1TableAdapters.AdminTableAdapter admin = new DataSet1TableAdapters.AdminTableAdapter();

        private void AdminAyarlari_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource=admin.AdminKontrol();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_ad.Text.Trim() == null || txt_sifre.Text.Trim()==null)
            {
                MessageBox.Show("ilk önce şifre ve adı girin ");
            }
            else
            {
                if(txt_sifre.Text.Trim().Length ==6)
                {
                   DialogResult a=MessageBox.Show("ekleme işlemi için emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (a == DialogResult.Yes)
                    {
                        
                        admin.ekle222(txt_ad.Text.Trim(), int.Parse(txt_sifre.Text.Trim()));
                        dataGridView1.DataSource = admin.AdminKontrol();
                        MessageBox.Show("ekleme işlemi başarılı");

                    }
                    else
                    {
                        MessageBox.Show("Ekleme işlemi iptal edildi");
                    }



                }
                else
                {
                   MessageBox.Show("Şifre 6 haneli olmalıdır");
                }


            }




        }

        private void button2_Click(object sender, EventArgs e)
        {

            admin.sil(int.Parse(txt_id.Text.Trim()));
            MessageBox.Show("Silme işlemi başarılı");
            admin.AdminKontrol();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            MenuEkrani menuEkrani = new MenuEkrani();
            menuEkrani.Show();
            this.Hide();
        }
    }
}
