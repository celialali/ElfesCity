using System;
namespace ElfesCity
{
    public class Chaudron:Objet
    {
        public Chaudron(int x, int y, int nbToursAcidite):base("o",x,y)

        {
            NbToursAcidite = nbToursAcidite;
        }

        public int NbToursAcidite { get; set; }

       


    }
}
