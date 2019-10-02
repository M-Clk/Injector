using System;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Hilecenter
{
    public partial class NewVersion : Form
    {
        public NewVersion()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                string hedef = "http://hilecim.net/versiyon/test.php";
                WebRequest istek = HttpWebRequest.Create(hedef);
                WebResponse yanit;
                yanit = istek.GetResponse();
                StreamReader bilgiler = new StreamReader(yanit.GetResponseStream());
                string gelen = bilgiler.ReadToEnd();
                int baslangic = gelen.IndexOf("<p>") + 3;
                int bitis = gelen.Substring(baslangic).IndexOf("</p>");
                string gelenbilgiler = gelen.Substring(baslangic, bitis);
                v = Convert.ToInt16(gelenbilgiler);
                VersionControl();
            }
            catch (Exception)
            {
                v = 0;
            }
            
        }

        int v = 0;
        private void VersionControl()
        {
            if (Program.versionNumber==v)
            {
                Program.isUpdated = true;
                this.Close();
            }
            else
            {
                button1.Visible = true;  
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://hilecim.net/versiyons/hilecenter_premium_new_version.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}
