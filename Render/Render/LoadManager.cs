using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;
namespace Render
{
    abstract class LoadManager
    {
        /*public static Object load(string path)
        {
            string[] getPathExtension = path.Split('.');
            string extension = getPathExtension[getPathExtension.Length-1].ToLower();

            if(extension == "obj")
            {
                loadMesh(path);
            }
        }
        */

        public static string readFile(string path)
        {
            return new StreamReader(path, Encoding.Default, false).ReadToEnd();
        }

        public static string[] readFile(string path, char split)
        {

            return new StreamReader(path, Encoding.Default, false).ReadToEnd().Split(split);
        }

        public static void loadMTL(string path)
        {

        }
      
        public static Mesh loadMesh(string path)
        {

            List<Vector3f> vertexes = new List<Vector3f>() ;
            List<int> connections = new List<int>();

            Mesh mesh = new Mesh();
            MeshPart currentPart = null;

            string file = readFile(path);

            file = file.Replace("  ", " ").Replace('\r', ' ').Replace('.',',');
            
            string[] lines = file.Split('\n');


            foreach (string line in lines)
            {
                if(line.Length>0 && line[0] == '#')
                {
                    mesh.meshInformation += line+"\n";
                }
                else if(line.Length>0 && line[0] == 'v' && line[1]!='t' && line[1] !='n')
                {
                    string[] parameters = line.Split(' ');
                    vertexes.Add(new Vector3f(float.Parse(parameters[1]), float.Parse(parameters[2]), float.Parse(parameters[3])));
                }
                else if(line.Length > 0 && line[0] == 'f')
                {
                    string[] parameters = line.Split(' ');
                    parameters[1] = parameters[1].Split('/')[0];
                    parameters[2] = parameters[2].Split('/')[0];
                    parameters[3] = parameters[3].Split('/')[0];

                    //Console.WriteLine(parameters[1]);

                    connections.Add(int.Parse(parameters[1]) -1);
                    connections.Add(int.Parse(parameters[2]) -1);
                    connections.Add(int.Parse(parameters[3]) -1);
                }
                else if(line.Length > 0 && (line[0] == 'g' || line[0] == 'o'))
                {
                    if (currentPart != null)
                    {
                        for (int i = 0; i < connections.Count; i++)
                        {
                            currentPart.vertexesXY.Add(new PointF(vertexes[connections[i]].x, vertexes[connections[i]].y));
                            currentPart.vertexesZ.Add(vertexes[connections[i]].z);
                        }
                        for (int i = 0; i < connections.Count/3; i++)
                        {
                            currentPart.color.Add(Brushes.White);
                        }
                        mesh.meshParts.Add(currentPart);
                    }
                    connections.Clear();
                    string[] parameters = line.Split(' ');
                    MeshPart m = new MeshPart();
                    m.name = parameters[1];
                    
                    currentPart = m;
                    Console.WriteLine(currentPart.vertexesXY);

                }

            
                
            }
            for (int i = 0; i < connections.Count; i++)
            {
                currentPart.vertexesXY.Add(new PointF(vertexes[connections[i]].x, vertexes[connections[i]].y));
                currentPart.vertexesZ.Add(vertexes[connections[i]].z);
            }
            for (int i = 0; i < connections.Count / 3; i++)
            {
                currentPart.color.Add(Brushes.White);
            }

            mesh.meshParts.Add(currentPart);

            // mesh.sortConnections();
            return mesh;
        }
    }
}
