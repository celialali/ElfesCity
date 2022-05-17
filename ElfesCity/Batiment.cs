using System;
namespace ElfesCity
{
    public class Batiment
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="solidite"></param>
        /// <param name="fonction"></param>
        /// <param name="niveauRequis"></param>
        /// <param name="niveauActuel"></param>
        /// <param name="nbpierresrequis"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="capaciteMax"></param>
        /// <param name="constructeur">est-ce que le batiment en construction est associé à un Elfe</param>
        public Batiment(bool solidite, string fonction, int niveauRequis, int niveauActuel,int nbpierresrequis, int x, int y, int capaciteMax, bool constructeur)
        {
            Solidite = solidite;
            Fonction = fonction;
            NiveauRequis = niveauRequis;
            NiveauActuel = niveauActuel;
            NbPierresRequis = nbpierresrequis;
            Abscisse = x;
            Ordonnee = y;
            CapaciteMax = capaciteMax;
            Constructeur = constructeur;
        }

        public int Abscisse { get; }

        public int Ordonnee { get;  }

        public bool Solidite { get; set; }

        public string Fonction { get;  }

        public int NiveauRequis { get;  }

        public int NiveauActuel { get; set; }

        public int NbPierresRequis { get;  }

        public int CapaciteMax { get; }

        public int CapaciteActuelle { get; set; }

        public bool Constructeur { get; set; }

        
        /// <summary>
        /// Renvoie true si le bâtiment est entièrement construit, false sinon
        /// </summary>
        /// <returns></returns>
        public bool EstConstruit()
        {
            if (this.NiveauActuel == this.NiveauRequis) return true;
            else return false;
        }

        public override string ToString()
        {
            return "b";
        }

    }
}
