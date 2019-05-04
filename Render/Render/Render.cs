using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Render
{
    class Render
    {
        private Thread renderThread;
        public static List<GameObject> objects;

        private static bool pause = false;
        private static bool enable = true;

        private static Form onDrawingForm;
        private static Graphics canvas;
        private static Bitmap[] buffer;
        private static Graphics[] bufferGraphics;
        private static byte currentBuffer = 0;

        public static Color clearColor = Color.Black;
        public static bool resizeRender = true;

        public static Vector3f light = new Vector3f(0, 0, 1);

        public Render(Form onDrawForm)
        {
            objects = new List<GameObject>();
            onDrawingForm = onDrawForm;
            buffer = new Bitmap[2] { new Bitmap(onDrawForm.Width, onDrawingForm.Height), new Bitmap(onDrawForm.Width, onDrawingForm.Height) };
            bufferGraphics = new Graphics[2] { Graphics.FromImage(buffer[0]), Graphics.FromImage(buffer[1]) };
            canvas = onDrawForm.CreateGraphics();

            renderThread = new Thread(new ThreadStart(draw));
        }
        
        private static void draw()
        {
            long last = 0;
            while(enable)
            {
                if (resizeRender)
                    renderSizeControl();

                if(!pause)
                {
                    canvas.DrawImage(buffer[currentBuffer], 0, 0);

                    if (currentBuffer == 0) currentBuffer++;
                    else currentBuffer--;

                    bufferGraphics[currentBuffer].Clear(clearColor);
                    for(int i = 0; i < objects.Count; i++)
                        objects[i].draw(bufferGraphics[currentBuffer]);
                }

             //   Console.WriteLine(DateTime.Now.Millisecond - last);
                last = DateTime.Now.Millisecond;
            }
        }

        private static void renderSizeControl()
        {
            if (onDrawingForm.Width != buffer[0].Width || onDrawingForm.Height != buffer[0].Height)
            {
                buffer = new Bitmap[2] { new Bitmap(onDrawingForm.Width, onDrawingForm.Height), new Bitmap(onDrawingForm.Width, onDrawingForm.Height) };
                bufferGraphics = new Graphics[2] { Graphics.FromImage(buffer[0]), Graphics.FromImage(buffer[1]) };
            }
        }

        public static void add(GameObject g)
        {
            objects.Add(g);
        }

        public void start()
        {
            if (!renderThread.IsAlive)
                renderThread.Start();
            else pause = false;
        }

        public void dispose()
        {
            enable = false;
        }

        public void stop()
        {
            pause = true;
        }
    }
}
