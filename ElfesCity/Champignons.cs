using System;
namespace ElfesCity
{
    public class Champignons:Objet
    {
        public Champignons(int x,int y, bool toxicite):base("c",x,y)
        {
            Toxicite= toxicite;
        }

        public bool Toxicite { get; set; }
    }
}
