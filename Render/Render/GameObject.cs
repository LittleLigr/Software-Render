using System.Drawing;

namespace Render
{
    /// <summary>
    /// Базовый объект пространства
    /// </summary>
    class GameObject
    {
        public Mesh mesh;

        public Vector3f position;
        public Vector3f scale;
        public Vector3f rotate;

        public Vector3f speed = new Vector3f(0,0,0);

        public GameObject()
        {
            position = new Vector3f();
            scale = new Vector3f(1,1,1);
            rotate = new Vector3f();

            Render.add(this);
        }

        public GameObject(Vector3f position, Vector3f scale, Vector3f rotate)
        {
            this.position = position;
            this.scale = scale;
            this.rotate = rotate;

            Render.add(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1">позиция по оси x</param>
        /// <param name="y1">позиция по оси y</param>
        /// <param name="z1">позиция по оси z</param>
        /// <param name="x2">размер по оси x</param>
        /// <param name="y2">размер по оси y</param>
        /// <param name="z2">размер по оси z</param>
        /// <param name="x3">поворот по оси x</param>
        /// <param name="y3">поворот по оси y</param>
        /// <param name="z3">поворот по оси z</param>
        public GameObject(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            position = new Vector3f(x1, y1, z1);
            scale = new Vector3f(x2, y2, z2);
            rotate = new Vector3f(x3, y3, z3);
        }

        public virtual void draw(Graphics g) { mesh.draw(g, position, scale, rotate); }
    }
}
