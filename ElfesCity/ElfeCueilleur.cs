using System;
namespace ElfesCity
{
    public class ElfeCueilleur : Elfe
    {
        public ElfeCueilleur(int x, int y, int nbToursOccupe) : base(x, y, "°", nbToursOccupe, 10, -1,-1)
        {
        }

        public override void Promener(Monde m)
        {
            int alea = m.rng.Next(0, 4);
            if (alea == 0)  // droite
            {
                if (Abscisse >= (m.NbLignes - 1))
                {
                    Abscisse = 0;
                }
                else Abscisse += 1;
            }
            else if (alea == 1)  // gauche
            {
                if (Abscisse <= 0)
                {
                    Abscisse = m.NbLignes - 1;
                }
                else Abscisse -= 1;
            }
            else if (alea == 2)  // bas
            {
                if (Ordonnee >= m.NbColonnes - 1)
                {
                    Ordonnee = 0;
                }
                else Ordonnee += 1;
            }
            else if (alea == 3)  // haut
            {
                if (Ordonnee <= 0)
                {
                    Ordonnee = m.NbColonnes-1;
                }
                else Ordonnee -= 1;
            }
        }


        public override void Action(Monde m)
        {
            if (m.EstChampi(Abscisse, Ordonnee)!=null)
            {
                Champignons champi = m.EstChampi(Abscisse, Ordonnee);
                if (champi.Toxicite == true)
                {
                    Console.WriteLine("L'elfe est tombé sur un champignon toxique. Il n'a pas survécu à sa grosse gastro... RIP");
                    m.TuerElfe(this);
                }
                else
                {
                    m.RemplirStock("champi", 3);
                    Console.WriteLine("Un elfe a cueilli des champignons ! ");
                }
                m.RetirerObjetMap(champi);
                this.Energie -= 1;
            }

            if (Abscisse == ABS_CHAUDRON && Ordonnee == ORD_CHAUDRON)
            {
                if (m.EstChaudron().NbToursAcidite == 0)
                {
                    Energie = 10;
                    Console.WriteLine("L'elfe cueilleur a regagné son énergie!");
                }
                else
                {
                    Console.WriteLine("Un elfe cueilleur a bu de la potion acide, il s'est désintégré avant même d'avoir pu recracher... RIP");
                    m.TuerElfe(this);
                }
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
