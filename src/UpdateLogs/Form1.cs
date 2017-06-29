using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace updatelogs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            string path = Application.StartupPath;
            try
            {
                
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path + @"\UPDATELOGS.md"))
                {
                    String line = await sr.ReadToEndAsync();
                    textBox1.Text = line;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "There is no update logs available now!\r\nPlease check the link : https://github.com/mostaphaRoudsari/Honeybee";
                //textBox1.Text = path;
            }
        }
    }
}
