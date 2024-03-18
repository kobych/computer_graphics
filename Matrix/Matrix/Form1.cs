using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matrix
{
    public partial class Form1 : Form
    {
        Bitmap image;
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files| *.png;*.jpg;*.bmp|All files(*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
            }

            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void свёрткаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void фильтрСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SobelFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void greyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GreyFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void luxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new LuxFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void фильтрСобеляToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Filters filter = new SobelFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void sharpnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SharpFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void embossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new EmbossFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void harpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new HarraFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedianFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Filters filter = new GreyWorld();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void wavesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new WavesFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void glassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GlassEffect();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void shiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Shift(50,50);
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Rotation(100, 0, 30);
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void binaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BinaryFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void mainColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MainColorFilter(Color.Aquamarine);
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void maxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MaxFilter();
            Bitmap resultImage = filter.processImage(image);
            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }

        private void edgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MaxFilter();
            Filters filter2 = new MedianFilter();
            Filters filter3 = new HarraFilter();

            Bitmap resultImage = filter2.processImage(image);
            resultImage = filter3.processImage(resultImage);
            resultImage = filter.processImage(resultImage);

            pictureBox1.Image = resultImage;
            pictureBox1.Refresh();
        }
    }
}
