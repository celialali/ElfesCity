using System;
namespace ElfesCity
{
    
    public abstract class Elfe
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Absisse</param>
        /// <param name="y">Ordonnée</param>
        /// <param name="poste">Quel type d'elfe c'est ? (cueilleur, ramasseur, constructeur) </param>
        /// <param name="nbToursOccupe">Nb de tours pendant lesquels l'elfe est occupé</param>
        /// <param name="energie"></param>
        /// <param name="abs_objectif">Si l'elfe doit se rendre quelque part (chaudron ou construction)</param>
        /// <param name="ord_objectif"></param>
        public Elfe(int x, int y, string poste, int nbToursOccupe, int energie, int abs_objectif, int ord_objectif)
        {
            Abscisse = x;
            Ordonnee = y;
            Poste = poste;
            NbToursOccupe = nbToursOccupe;
            Energie = energie;
            Abs_objectif = abs_objectif;
            Ord_objectif = ord_objectif;
        }

        protected const int ABS_CHAUDRON = 12;  // On indique la position du chaudron (elle est fixée au début de la partie et ne bouge plus)
        protected const int ORD_CHAUDRON = 12;


        public int Abscisse { get; set; }

        public int Ordonnee { get; set; }

        public string Poste { get; set; }

        public int NbToursOccupe { get; set; }

        public int Energie { get; set; }

        public int Abs_objectif { get; set; }

        public int Ord_objectif { get; set; }

        /// <summary>
        /// L'elfe se déplace d'une case en ligne droite ou en diagonale vers son objectif. S'il est arrivé, l'objectif est supprimé.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Deplacer(int x, int y)
        {
            if (x > Abscisse)
            {
                Abscisse += 1;
            }
            if (x < Abscisse)
            {
                Abscisse -= 1;
            }
            if (y < Ordonnee)
            {
                Ordonnee -= 1;
            }
            if (y > Ordonnee)
            {
                Ordonnee += 1;
            }
            if (Abscisse==Abs_objectif && Ordonnee == Ord_objectif)
            {
                Abs_objectif = -1;
                Ord_objectif = -1;
            }
        }

        public void BesoinEnergie()
        {
            if (Energie == 0)
            {
                Abs_objectif = ABS_CHAUDRON;
                Ord_objectif = ORD_CHAUDRON;
            }
        }

        public abstract void Promener(Monde m);

        public override string ToString()
        {
            return ("Elfe " + Poste + ": énergie = " + Energie + ", occupé pdt " + NbToursOccupe + " tours");
        }

        public abstract void Action(Monde m);

    }
}
