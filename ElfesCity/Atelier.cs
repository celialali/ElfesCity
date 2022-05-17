using System;
namespace ElfesCity
{
    public class Atelier:Batiment
    {

        // Sert pour créer un atelier déjà construit
        public Atelier(int x, int y, int niveauActuel) : base(false, "a", 27, niveauActuel, 17, x, y, 40, false)
        {
        }

        public Atelier(int x, int y) : base(false, "a", 27, 0, 17, x, y, 40, false)
        {
        }

    }
}
