using System;

namespace ComplexNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            ComplexNumber x = new ComplexNumber(10.5);
            ComplexNumber y = null;

            if (y == null && x != null)
            {
                y = new ComplexNumber(7, 8.1);

                ComplexNumber z = x + y;
                Console.WriteLine(z); // 17,5+i8,1

                z -= 20;
                Console.WriteLine(z); // -2,5+i8,1

                z.Real += 13;
                z.Imaginary -= 8.1;
                Console.WriteLine(z); // 10,5

                if (z == x && z != y)
                {
                    Console.WriteLine("{0:A}", z - y); // 3,5-i8,1
                    Console.WriteLine("{0:P}", z - y); // (3,5, -8,1)
                }
            }

            x = y = null;
            if (x == y) Console.WriteLine("x == y == null");
        }
    }
}
