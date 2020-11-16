using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_QuoteApplicationAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Package Express. Please follow the instructions below.");
            //Package weight
            Console.Write("Please enter the package weight: ");
            double packWeight = Convert.ToDouble(Console.ReadLine());
            if (packWeight > 50)
            {
                Console.WriteLine("Package too heavy to be shipped via Package Express.Have a good day.");
            }
            else 
            {
                //Package width
                Console.Write("Please enter the package width: ");
                double packWidth = Convert.ToDouble(Console.ReadLine());
                //Package height
                Console.Write("Please enter the package height: ");
                double packHeight = Convert.ToDouble(Console.ReadLine());
                //Package length
                Console.Write("Please enter the package length: ");
                double packLength = Convert.ToDouble(Console.ReadLine());
                //Total of package
                double Total = packWidth + packHeight + packLength;
                if (Total > 50)
                {
                    Console.WriteLine("Package too big to be shipped via Package Express.");
                }
                else
                {
                    double price =((packWidth * packHeight * packLength) * packWeight) / 100;
                    Console.WriteLine("Your estimated total for shipping this package is: $"+ price + "\nThank You.");
                    
                }
            }
            Console.ReadLine();
        }
    }
}
