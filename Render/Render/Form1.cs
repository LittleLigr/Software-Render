using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Render
{
    public partial class Form1 : Form
    {
        Render render;
        
        GameObject model;
        Mesh mesh;
        public Form1()
        {
            InitializeComponent();
            render = new Render(this);


            mesh = LoadManager.loadMesh("two_sphere.obj");
            model = new GameObject();
            model.mesh = mesh;
            model.rotate.x = 180;
            model.rotate.y = -30;
            model.position = new Vector3f(500, 350, 100);
            model.scale = new Vector3f(11, 10, 10);

            render.start();        
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            render.dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            model.scale = new Vector3f(trackBar1.Value, trackBar1.Value, trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            model.position.x = trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            model.position.y = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            model.position.z = trackBar4.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Render.light = new Vector3f(0, 0, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Render.light = new Vector3f(1, 0, 0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Render.light = new Vector3f(-1, 1, 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mesh = LoadManager.loadMesh("two_sphere.obj");
            model.mesh = mesh;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mesh = LoadManager.loadMesh("shahter5.obj");
            model.mesh = mesh;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mesh = LoadManager.loadMesh("Ship.obj");
            model.mesh = mesh;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            model.rotate.x = trackBar7.Value;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            model.rotate.y = trackBar6.Value;
        }

       
    }
}
