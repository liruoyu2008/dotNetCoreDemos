using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptDemo
{
    public class Sample
    {
        public int A { get; set; }

        public int B { get; set; }


        public Sample(int a, int b) { A = a; B = b; }

        public int GetValue()
        {
            return A + B;
        }
    }
}
