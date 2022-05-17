using System;
namespace ElfesCity
{
    public class Pierres:Objet
    {
        public Pierres(int x,int y,bool tranchante):base("p",x,y)
        {
            Tranchante = tranchante;
        }

        public bool Tranchante { get; set; }

    }
}
