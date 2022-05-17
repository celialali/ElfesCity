using System;
namespace ElfesCity
{
    public class Silo:Batiment
    {
        public Silo(int x, int y, int niveauActuel) : base(false, "s", 30, niveauActuel, 14, x, y, 20, false)
        {
        }
        public Silo(int x, int y) : base(false, "s", 30, 0, 14, x, y, 20, false)
        {
        }



    }
}
