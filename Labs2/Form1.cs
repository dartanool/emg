using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Labs2
{
    internal class ImageFilter1
    {
        public static byte ClampValue(double value, double min = 0, double max = 255)
        {
            return (byte)Math.Max(min, Math.Min(max, value));
        }
    }
    public partial class Form1 : Form
    {
        private Image<Bgr, byte> sourceImage; //глобальная переменная
        public Form1()
        {
            InitializeComponent();
        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // открытие диалога выбора файла
            if (result == DialogResult.OK) // открытие выбранного файла
            {
                string fileName = openFileDialog.FileName;
                sourceImage = new Image<Bgr, byte>(fileName);
            }
            imageBox1.Image = sourceImage.Resize(640, 480, Inter.Linear);

        }
    }
}
