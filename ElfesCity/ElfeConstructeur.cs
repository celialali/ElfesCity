using System;
namespace ElfesCity
{
    public class ElfeConstructeur : Elfe
    {
        public ElfeConstructeur(int x, int y, int nbToursOccupe) : base(x, y, "i", nbToursOccupe, 35, -1, -1)
        {
        }
        public ElfeConstructeur(int x, int y, int nbToursOccupe, int energie, int abs_obj, int ord_obj) : base(x, y, "i", nbToursOccupe, energie, abs_obj, ord_obj)
        {
        }

        public override void Action(Monde m)
        {
            if (this.NbToursOccupe > 0)
            {
                Batiment batEnConstruction = m.EstBatiment(Abscisse, Ordonnee);
                batEnConstruction.NiveauActuel += 1;
                NbToursOccupe -= 1;
                Energie -= 1;
            }

            if (Abscisse == ABS_CHAUDRON && Ordonnee == ORD_CHAUDRON)
            {
                if (m.EstChaudron().NbToursAcidite == 0)
                {
                    Energie = 35;
                    Console.WriteLine("L'elfe constructeur a regagné son énergie!");
                }
                else
                {
                    Console.WriteLine("Un elfe constructeur a bu de la potion acide, il s'est désintégré avant même d'avoir pu recracher... RIP");
                    m.TuerElfe(this);
                }
            }
        }

        public override void Promener(Monde m)
        {
            int alea = m.rng.Next(0, 4);
            if (alea == 0)
            {
                Abscisse += 1;
                if (Abscisse >= m.NbColonnes)
                {
                    Abscisse = 0;
                }
            }
            if (alea == 1)
            {
                Abscisse -= 1;
                if (Abscisse < 0)
                {
                    Abscisse = m.NbColonnes-1;
                }
            }
            if (alea == 2)
            {
                Ordonnee += 1;
                if (Ordonnee >= m.NbLignes)
                {
                    Ordonnee = 0;
                }
            }
            if (alea == 3)
            {
                Ordonnee -= 1;
                if (Ordonnee < 0)
                {
                    Ordonnee = m.NbLignes-1;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
