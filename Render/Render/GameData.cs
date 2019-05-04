using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    abstract class GameData
    {
        private static List<Data> dataList = new List<Data>();

        public static void add(Object data, string name)
        {
            dataList.Add(new Data(data, name));
        }

        public static object get(string name)
        {
            foreach(Data d in dataList)
            {
                if(d.name == name)
                {
                    return d.data;
                }
            }

            return null;
        }

        public static T get<T>(string name)
        {
            foreach (Data d in dataList)
            {
                if (d.name == name)
                {
                    return (T)d.data;
                }
            }

            return default(T);
        }
    }

    class Data
    {
        public string name;
        public object data;

        public Data(object data, string name)
        {
            this.name = name;
            this.data = data;
        }
    }

}
