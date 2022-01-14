using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEA3
{
    class Result
    {
        private int[] path = new int[] { };
        

        public Result(int[] path)
        {
            this.path = (int[])path.Clone();
            
        }

        public ref int[] getpath() {

            return ref path;
        
        }

        public void display() {
            for (int i = 0; i < path.Length -1 ; i++)
            {
                Console.Write(path[i] + "-");
            }
            Console.Write("0");
            Console.WriteLine();
            

        }

    }
}
