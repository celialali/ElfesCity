using System;
namespace ElfesCity
{
    public class Objet
    {
        public Objet(string type, int x, int y)
        {
            Type = type;
            Abscisse = x;
            Ordonnee = y;
        }

        public string Type { get; set; }

        public int Abscisse { get; set; }

        public int Ordonnee { get; set; }

        
    }
}
