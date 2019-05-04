using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    class Mesh
    {
        public static SolidBrush[] color_data = new SolidBrush[255];

        public List<MeshPart> meshParts = new List<MeshPart>();

        List<PointF> vertexesBufferXY = new List<PointF>();
        List<float> vertexesBufferZ = new List<float>();
        List<Brush> colorBuffer = new List<Brush>();

        public string meshInformation;

        PointF[] f = new PointF[3];
        float bufferZ = 1001;

        public Mesh()
        {
            for(int x = 0; x < color_data.GetLength(0); x++)
                color_data[x] = new SolidBrush(Color.FromArgb(x, x, x));
        }

        public void rotate(Vector3f rotate)
        {
            float a_x = (float)(Math.PI * rotate.x / 180f);
            float cosx = (float)Math.Cos(a_x);
            float sinx = (float)Math.Sin(a_x);

            float a_y = (float)(Math.PI * rotate.y / 180f);
            float cosy = (float)Math.Cos(a_y);
            float siny = (float)Math.Sin(a_y);

            float a_z = (float)(Math.PI * rotate.z / 180f);
            float cosz = (float)Math.Cos(a_z);
            float sinz = (float)Math.Sin(a_z);

            for (int i = 0; i < vertexesBufferXY.Count; i++)
            {
                PointF f = vertexesBufferXY[i];
                //x
                if (rotate.x !=0)
                {
                    float yValue = vertexesBufferXY[i].Y;
                    f.Y = (cosx * vertexesBufferXY[i].Y - sinx * vertexesBufferZ[i]);
                    vertexesBufferZ[i] = (sinx * yValue + cosx * vertexesBufferZ[i]);
                }
              
                //y
                if(rotate.y !=0)
                {
                    float xValue = vertexesBufferXY[i].X;
                    f.X = cosy * vertexesBufferXY[i].X + siny * vertexesBufferZ[i];
                    vertexesBufferZ[i] = -siny * xValue + cosy * vertexesBufferZ[i];
                }
                
                //z
                if(rotate.z !=0)
                {
                    float xValue = vertexesBufferXY[i].X;
                    f.X = cosz * vertexesBufferXY[i].X - sinz * vertexesBufferXY[i].Y;
                    f.Y = sinz * xValue + cosz * vertexesBufferXY[i].Y;
                }

                f.X = f.X;
                f.Y = f.Y;
                vertexesBufferXY[i] = f;
                vertexesBufferZ[i] = vertexesBufferZ[i];

            }
        }

 

        public void draw(Graphics g, Vector3f position, Vector3f scale, Vector3f rotation)
        {
            vertexesBufferXY.Clear();
            vertexesBufferZ.Clear();
            colorBuffer.Clear();
      
            for(int i = 0; i < meshParts.Count; i++)
                if(meshParts[i]!=null)
                {
                    meshParts[i].drawPart();
                    vertexesBufferXY.AddRange(meshParts[i].getBufferVertexesXY());
                    vertexesBufferZ.AddRange(meshParts[i].getBufferVertexesZ());
                    colorBuffer.AddRange(meshParts[i].getBufferColor());
                }
                
            rotate(rotation);

            Vector3f light = Render.light;

            for (int i = vertexesBufferXY.Count - 1; i >= 0; i -= 3)
            {
                float vx1 = vertexesBufferXY[i].X - vertexesBufferXY[i - 1].X;
                float vy1 = vertexesBufferXY[i].Y - vertexesBufferXY[i - 1].Y;
                float vz1 = vertexesBufferZ[i] - vertexesBufferZ[i - 1];

                float vx2 = vertexesBufferXY[i].X - vertexesBufferXY[i - 2].X;
                float vy2 = vertexesBufferXY[i].Y - vertexesBufferXY[i - 2].Y;
                float vz2 = vertexesBufferZ[i] - vertexesBufferZ[i - 2];


                float nx = vy1 * vz2 - vz1 * vy2;
                float ny = vz1 * vx2 - vx1 * vz2;
                float nz = vx1 * vy2 - vy1 * vx2;       

                float nL = (float)Math.Sqrt(Math.Pow(nx, 2) + Math.Pow(ny, 2) + Math.Pow(nz, 2));

                nx /= nL;
                ny /= nL;
                nz /= nL;

                float scal = light.x*nx + light.y*ny +light.z*nz ;
                float cos = scal/ (float)(Math.Sqrt(Math.Pow(light.x, 2) + Math.Pow(light.y, 2) + Math.Pow(light.z, 2)));
                if (cos < 0) cos = 0 ;

                colorBuffer[i / 3] = color_data[(byte)(cos * 255)];
            
            }

            float fovx = (float)(1f / Math.Tan(Math.PI * 1f / 3)) /0.625f;
            float fovy = (float)(1f / Math.Tan(Math.PI * 1f / 6))/ 1.6f;

            for (int i = 0; i < vertexesBufferXY.Count; i++)
            {
                vertexesBufferZ[i] = vertexesBufferZ[i] * scale.z + position.z;
                PointF f = vertexesBufferXY[i];
                f.X = f.X * fovx * (bufferZ / vertexesBufferZ[i]) * scale.x + position.x;
                f.Y = f.Y * fovy * (bufferZ / vertexesBufferZ[i]) * scale.y + position.y;
                vertexesBufferXY[i] = f;
            }

            sortConnections();

            for (int i = vertexesBufferXY.Count-1; i >=0 ; i-=3)
            {
                f[0] = vertexesBufferXY[i];
                f[1] = vertexesBufferXY[i-1];
                f[2] = vertexesBufferXY[i-2];

                g.FillPolygon(colorBuffer[i/3], f);
            }

        }

        public void sortConnections()
        {
            for(int i = 0; i < vertexesBufferZ.Count-3; i+=3)
            {
                for (int j = i+3; j < vertexesBufferZ.Count; j+=3)
                {

                    if(vertexesBufferZ[i]+ vertexesBufferZ[i+1]+ vertexesBufferZ[i+2]> vertexesBufferZ[j] + vertexesBufferZ[j + 1] + vertexesBufferZ[j + 2])
                    {
                        Brush c = colorBuffer[i / 3];
                        colorBuffer[i / 3] = colorBuffer[j / 3];
                        colorBuffer[j / 3] = c;                        

                        PointF v = vertexesBufferXY[i];
                        vertexesBufferXY[i] = vertexesBufferXY[j];
                        vertexesBufferXY[j] = v;

                        float f = vertexesBufferZ[i];
                        vertexesBufferZ[i] = vertexesBufferZ[j];
                        vertexesBufferZ[j] = f;

                        v = vertexesBufferXY[i+1];
                        vertexesBufferXY[i + 1] = vertexesBufferXY[j+1];
                        vertexesBufferXY[j+1] = v;

                        f = vertexesBufferZ[i+1];
                        vertexesBufferZ[i+1] = vertexesBufferZ[j+1];
                        vertexesBufferZ[j+1] = f;

                        v = vertexesBufferXY[i+2];
                        vertexesBufferXY[i+2] = vertexesBufferXY[j+2];
                        vertexesBufferXY[j+2] = v;

                        f = vertexesBufferZ[i+2];
                        vertexesBufferZ[i+2] = vertexesBufferZ[j+2];
                        vertexesBufferZ[j+2] = f;
                    }

                }
            }
        }
    }
    class MeshPart
    {
        public string name;

        public Vector3f position = new Vector3f(0,0,0);
        public Vector3f scale = new Vector3f(1,1,1);
        public Vector3f rotation = new Vector3f(0, 0, 0);

        public List<PointF> vertexesXY = new List<PointF>();
        public List<float> vertexesZ = new List<float>();
        public List<Brush> color = new List<Brush>();

        public List<PointF> vertexesBufferXY = new List<PointF>();
        public List<float> vertexesBufferZ = new List<float>();
        public List<Brush> colorBuffer = new List<Brush>();

        public void drawPart()
        {
            vertexesBufferXY.Clear();
            vertexesBufferZ.Clear();
            colorBuffer.Clear();

            vertexesBufferXY.AddRange(vertexesXY);
            vertexesBufferZ.AddRange(vertexesZ);
            colorBuffer.AddRange(color);
            
            rotate(rotation);

            for (int i = 0; i < vertexesXY.Count; i++)
            {
                PointF f = vertexesBufferXY[i];
                f.X = f.X * scale.x + position.x;
                f.Y = f.Y * scale.y + position.y;
                vertexesBufferXY[i] = f;
                vertexesBufferZ[i] = vertexesBufferZ[i] * scale.z + position.z;
            }
           
            //sortConnections();

        }

        public List<PointF> getBufferVertexesXY()
        {
            return vertexesBufferXY;
        }

        public List<float> getBufferVertexesZ()
        {
            return vertexesBufferZ;
        }

        public List<Brush> getBufferColor()
        {
            return colorBuffer;
        }

        public void rotate(Vector3f rotate)
        {

            float a_x = (float)(Math.PI * rotate.x / 180f);
            float cosx = (float)Math.Cos(a_x);
            float sinx = (float)Math.Sin(a_x);

            float a_y = (float)(Math.PI * rotate.y / 180f);
            float cosy = (float)Math.Cos(a_y);
            float siny = (float)Math.Sin(a_y);

            float a_z = (float)(Math.PI * rotate.z / 180f);
            float cosz = (float)Math.Cos(a_z);
            float sinz = (float)Math.Sin(a_z);

            for (int i = 0; i < vertexesXY.Count; i++)
            {
                PointF f = vertexesXY[i];
                vertexesBufferZ[i] = vertexesZ[i];
                //x
                if (rotate.x != 0)
                {
                    float yValue = f.Y;
                    f.Y = (cosx * f.Y - sinx * vertexesBufferZ[i]);
                    vertexesBufferZ[i] = (sinx * yValue + cosx * vertexesBufferZ[i]);
                }

                //y
                if (rotate.y != 0)
                {
                    float xValue = f.X;
                    f.X = cosy * f.X + siny * vertexesBufferZ[i];
                    vertexesBufferZ[i] = -siny * xValue + cosy * vertexesBufferZ[i];
                }

                //z
                if (rotate.z != 0)
                {
                    float xValue = f.X;
                    f.X = cosz * f.X - sinz * f.Y;
                    f.Y = sinz * xValue + cosz * f.Y;
                }

                f.X = f.X;
                f.Y = f.Y;
                vertexesBufferXY[i] = f;
                vertexesBufferZ[i] = vertexesBufferZ[i];

            }
        }

        
            
        

    }


}
