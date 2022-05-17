using System;
namespace ElfesCity
{
    public class ElfeRamasseur : Elfe
    {
        
        public ElfeRamasseur(int x, int y, int nbToursOccupe) : base(x, y, "~", nbToursOccupe, 10,-1,-1)
        {
        }

        public override void Promener(Monde m)
        {
            int alea1 = m.rng.Next(0, 2);  // haut ou bas
            int alea2 = m.rng.Next(0, 2);  // gauche ou droite
            if (alea1 == 0)
            {
                Abscisse += 1;
                if (Abscisse >= m.NbColonnes)
                {
                    Abscisse = 0;
                }

            }
            if (alea1 == 1)
            {
                Abscisse -= 1;
                if (Abscisse < 0)
                {
                    Abscisse = m.NbColonnes-1;
                }

            }
            if (alea2 == 0)
            {
                Ordonnee += 1;
                if (Ordonnee >= m.NbLignes)
                {
                    Ordonnee = 0;
                }
            }
            if (alea2 == 1)
            {
                Ordonnee -= 1;
                if (Ordonnee < 0)
                {
                    Ordonnee = m.NbLignes-1;
                }
            }
        }


        public override void Action(Monde m)
        {
            if (m.EstPierre(Abscisse, Ordonnee) != null)
            {
                Pierres pierres = m.EstPierre(Abscisse, Ordonnee);
                if (pierres.Tranchante == true)
                {
                    Console.WriteLine("L'elfe a trébuché sur une pierre tranchante et a succombé de ses blessures... RIP");
                    m.TuerElfe(this);
                }
                else
                {
                    m.RemplirStock("pierre", 3);
                    Console.WriteLine("Un ramasseur a ramassé des pierres !");
                }
                m.RetirerObjetMap(pierres);
                this.Energie -= 1;
            }
            
            if (Abscisse == ABS_CHAUDRON && Ordonnee == ORD_CHAUDRON)
            {
                Energie = 10;
                Console.WriteLine("L'elfe ramasseur a regagné son énergie!");
            }
        }


        public override string ToString()
        {
            return base.ToString();
        }
    }
}