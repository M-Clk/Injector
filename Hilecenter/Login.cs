
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hilecenter
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Kullanıcı adı şifre alanı boş geçilemez.", "Uyarı");
            }
            else
            {
                BeLogin();
            }

        }
        //MySqlConnection mySqlConnection;
        void BeLogin()
        {
            string result = "";
            try
            {
                result = Program.GetWebResponse("http://hilecim.net/versiyon/ss.php?kadi=" + txtUsername.Text + "&sifre=" + txtPassword.Text);
                if (result == "0")
                {
                    MessageBox.Show("Kullanici adi veya sifre hatali. Tekrar deneyin.", "Hatali Giris Denemesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    return;
                }
                try
                {
                    string[] dateTimes = result.Split('|');

                    Program.buyingDate = DateTime.Parse(dateTimes[0]);
                    Program.targetDate = DateTime.Parse(dateTimes[1]);
                    Program.isTrue = true;
                        this.Close();
                }
                catch (Exception)
                {
                    //MessageBox.Show("Kullanici adi veya sifre hatali. Tekrar deneyin.", "Hatali Giris Denemesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //mySqlConnection.Open();
                //MySqlCommand mySqlCommand = new MySqlCommand("select kadi, sifre, vip_bitis_tarihi from program where kadi='" + txtUsername.Text + "' and sifre='" + txtUsername.Text + "'", mySqlConnection);
                //MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                //if (dataReader.Read())
                //{
                //    Program.isTrue = true;
                //    Program.tagetDate=dataReader.GetDateTime("vip_bitis_tarihi");
                //}
                //if (Program.isTrue)
                //    this.Close();
                //else
                //{
                //    mySqlConnection.Close();
                //    MessageBox.Show("Kullanici adi veya sifre hatali. Tekrar deneyin.", "Hatali Giris Denemesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtUsername.Clear();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Her şey gayet iyiydi..." + Environment.NewLine + "Sunucularımızla bağlantı kurulurken bir hata ile karşılaştık. İnternete bağlı olduğunuzdan emin olun. Tekrar deneyin.\n" + ex.Message + "\n" + result, "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.targetDate < Program.GetTime() && Program.isTrue)
            {
                DialogResult result = MessageBox.Show("Maalesef uygulamamızı kullanmanız için verilen süre doldu. Tekrar hizmet almak için tekliflerimiz görmek ister misiniz?", "Yetkisiz Erişim", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start("https://hilecenter.com/faceitte-calisan-csgo-hilesi-satin-al");
            }
            if (!Program.isTrue) Application.ExitThread();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.Select();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Down) txtPassword.Select();
            
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) txtUsername.Select();
            else if (e.KeyCode == Keys.Down) button1.Select();
            else if (e.KeyCode == Keys.Enter && txtUsername.Text != "" && txtPassword.Text != "") BeLogin();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            txtUsername.Select();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            txtPassword.Select();
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) txtPassword.Select();
            else if (e.KeyCode == Keys.Down) txtUsername.Select();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}