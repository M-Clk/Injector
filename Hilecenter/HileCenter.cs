using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using operation;

namespace Hilecenter
{
    public partial class HileCenter : Form
    {
        
        int total_minutes;
        public HileCenter()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CheckStatus()
        {
            string result = "";
            try
            {

                result = Program.GetWebResponse("http://hilecim.net/versiyon/durum.php");
                if (result != null)
                {
                    string[] items = result.Split('|');
                    for (int i = 0; i < items.Length; i++)
                    {
                        PictureBox pictureBox = (PictureBox)Controls.Find("picture_" + items[i].Split('-')[0], true)[0];
                        pictureBox.Enabled = Convert.ToBoolean(Convert.ToInt16(items[i].Split('-')[1]));
                        Label lbl = (Label)Controls.Find("lbl_status_" + items[i].Split('-')[0], true)[0];
                        if (pictureBox.Enabled)
                        {
                            lbl.ForeColor = Color.LawnGreen;
                            lbl.Text = "Çalışıyor";
                        }
                        else
                        {
                            lbl.Text = "Çalışmıyor";
                            lbl.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Cursor == Cursors.No)
            {
                DialogResult result = MessageBox.Show("Maalesef uygulamamızı kullanmanız için verilen süre doldu. Tekrar hizmet almak için tekliflerimiz görmek ister misiniz?", "Yetkisiz Erişim", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    Process.Start("http://hilecim.net/center/index.php#satinal");
                return;
            }
            if (dllName == null)
            {
                MessageBox.Show("Lütfen bir hile seçin.", "Hatalı İşlem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Directory.CreateDirectory(Application.LocalUserAppDataPath + "\\File");
                
                if (dllName == "memphis")
                    File.WriteAllBytes(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Properties.Resources.memphis);
                else if (dllName == "osiris")
                    File.WriteAllBytes(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Properties.Resources.osiris);
                else if (dllName == "padisah")
                    File.WriteAllBytes(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Properties.Resources.padisah);
                else if (dllName == "fatih")
                    File.WriteAllBytes(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Properties.Resources.fatih);
                else if (dllName == "osmanli")
                    File.WriteAllBytes(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Properties.Resources.osmanli);

                DoIt doIt = new DoIt();
                if(doIt.Inject(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll", Program.gameProcessName))
                    MessageBox.Show("Hile başlatıldı...");
                else
                {
                    try
                    {
                        Process.Start("steam://rungameid/730");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hile başlatılamadı. Oyunun açık olduğundan emin olun.");
                    }
                    
                }
                timer1.Start();

            }
            catch (Exception)
            {
                MessageBox.Show("Hile zaten çalışıyor.");
            }
        }
        string totalTimeString;
        private void Form3_Load(object sender, EventArgs e)
        {
            NewVersion newVersion = new NewVersion();
            newVersion.ShowDialog();
            if (!Program.isUpdated)
                Application.ExitThread();
            Login frmLogin = new Login();
            frmLogin.ShowDialog();
            if (!Program.isTrue) Application.ExitThread();
            CheckStatus();
            TimeSpan timeSpan = Program.targetDate - Program.buyingDate;
            total_minutes = timeSpan.Minutes + timeSpan.Hours * 60 + timeSpan.Days * 24 * 60;

            if (timeSpan.Days > 0)
                totalTimeString = timeSpan.Hours>20?timeSpan.Days+1+ " gün":timeSpan.Days  +" gün";
            else totalTimeString = timeSpan.Hours + " saat";
            CalculateRemainingTime();
            remaining_timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
if (Process.GetProcessesByName(Program.gameProcessName).Length == 0)
            {
                Process[] pr =Process.GetProcessesByName(Program.gameProcessName);
                if (File.Exists(Application.LocalUserAppDataPath + "\\File\\"+dllName+".dll"))
                    File.Delete(Application.LocalUserAppDataPath + "\\File\\" + dllName + ".dll");
                timer1.Stop();
            }
            }
            catch (Exception)
            {
            }
            
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(Application.LocalUserAppDataPath + "\\File\\osiris.dll"))
                File.Delete(Application.LocalUserAppDataPath + "\\File\\osiris.dll");
            if (File.Exists(Application.LocalUserAppDataPath + "\\File\\memphis.dll"))
                File.Delete(Application.LocalUserAppDataPath + "\\File\\memphis.dll");
            if (File.Exists(Application.LocalUserAppDataPath + "\\File\\fatih.dll"))
                File.Delete(Application.LocalUserAppDataPath + "\\File\\fatih.dll");
            if (File.Exists(Application.LocalUserAppDataPath + "\\File\\padisah.dll"))
                File.Delete(Application.LocalUserAppDataPath + "\\File\\padisah.dll");
            if (File.Exists(Application.LocalUserAppDataPath + "\\File\\osmanli.dll"))
                File.Delete(Application.LocalUserAppDataPath + "\\File\\osmanli.dll");

            //for (int i = 0; i < length; i++)
            //{

            //}
            Application.ExitThread();
        }
        int remaining_days = 0;
        int remaining_hours = 0;
        int remaining_minutes = 0;
        string remainingTimeString = "";
        int remaining_percent = 0;
        private void remaining_timer_Tick(object sender, EventArgs e)
        {
            CalculateRemainingTime();
        }
        void StopGame()
        {
            try
            {
                Process.GetProcessesByName(Program.gameProcessName)[0].Kill();
            }
            catch (Exception)
            {
            }
            
        }

        void CalculateRemainingTime()
        {
            if (Program.GetTime() >= Program.targetDate)
            {
                //button2.Enabled = false;
                StopGame();
                timer1.Stop();
                button2.Cursor = Cursors.No;
                return;
            }
            TimeSpan timeSpan = Program.targetDate - Program.GetTime();
            remaining_days = timeSpan.Days;
            remaining_hours = timeSpan.Hours;
            remaining_minutes = timeSpan.Minutes;

            int remaining_total_minutes = remaining_minutes;

            remainingTimeString = remaining_minutes + " dakika kaldı.";
            if (remaining_hours > 0)
            {
                remainingTimeString = remaining_hours + " saat, " + remainingTimeString;
                remaining_total_minutes += remaining_hours * 60;
            }
            if (remaining_days > 0)
            {
                remainingTimeString = remaining_days + " gün, " + remainingTimeString;
                remaining_total_minutes += remaining_days * 24 * 60;
            }

            if (total_minutes > 0)
                remaining_percent = (total_minutes - remaining_total_minutes) * 100 / total_minutes;
            progressBarTime.Value = remaining_percent;
            lbl_time_status.Text = totalTimeString + " / " + remainingTimeString + " (%" + remaining_percent.ToString() + ")";
            if (remaining_percent > 90)
            {
                lbl_time_status.Text += " \nSüreniz dolmak üzere. Dolduğunda oyun otomatik kapanacaktır.";
                lbl_time_status.ForeColor = Color.Red;
            }
        }
        string dllName;
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            SetPicBackground();
            pic.BackColor = Color.Blue;

            dllName = pic.Name.Substring(pic.Name.IndexOf('_') + 1);
        }
        void SetPicBackground()
        {
            picture_osiris.BackColor = picture_memphis.BackColor = picture_fatih.BackColor = picture_osmanli.BackColor = picture_padisah.BackColor = Color.White;
        }
    }
    
}
