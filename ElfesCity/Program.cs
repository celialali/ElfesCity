using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ElfesCity
{
    class Program
    {
        
        static void Main(string[] args)
        {

            char rejouer;
            do
            {
                Simulation simu = new Simulation();
                simu.SimulationPartie();

                Console.WriteLine("Voulez-vous rejouer ? (o=oui/ n=non)");
                do
                {
                    rejouer = Console.ReadKey().KeyChar;
                }
                while (rejouer != 'o' && rejouer != 'n');
            }
            while (rejouer == 'o');
        }
    }
}
