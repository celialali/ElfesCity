using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Collections.Generic;
using System.Linq;

namespace ElfesCity
{
    public class Simulation
    {
        public int i = 0;

        public int _jour = 0;

        public Monde LeMonde { get; }

        public Simulation()
        {
            LeMonde = new Monde(25, 25);
            InitialiseMap();
        }

        public void InitialiseMap()
        {
            Chaudron chaudron = new Chaudron(12, 12, 0);
            LeMonde.AjouterObjetPlateau(chaudron);

            Atelier atelier1 = new Atelier(2, 3, 27);  // atelier entièrement construit
            Atelier atelier2 = new Atelier(6, 8);
            Silo silo1 = new Silo(3, 2, 30);  // silo entièrement construit
            Silo silo2 = new Silo(5, 17);
            LeMonde.AjouterBatiment(atelier1);
            LeMonde.AjouterBatiment(atelier2);
            LeMonde.AjouterBatiment(silo1);
            LeMonde.AjouterBatiment(silo2);

            Elfe cueilleur1 = new ElfeCueilleur(1, 11, 0);
            Elfe cueilleur2 = new ElfeCueilleur(9, 11, 0);
            Elfe cueilleur3 = new ElfeCueilleur(20, 20, 0);
            Elfe constructeur1 = new ElfeConstructeur(6, 8, 0);
            Elfe constructeur3 = new ElfeConstructeur(8, 8, 0);
            Elfe constructeur2 = new ElfeConstructeur(14, 22, 0);
            Elfe ramasseur1 = new ElfeRamasseur(3, 2, 0);
            Elfe ramasseur2 = new ElfeRamasseur(19, 19, 0);
            LeMonde.AjouterElfe(cueilleur1);
            LeMonde.AjouterElfe(cueilleur2);
            LeMonde.AjouterElfe(cueilleur3);
            LeMonde.AjouterElfe(constructeur1);
            LeMonde.AjouterElfe(constructeur3);
            LeMonde.AjouterElfe(constructeur2);
            LeMonde.AjouterElfe(ramasseur1);
            LeMonde.AjouterElfe(ramasseur2);
            LeMonde.AjouterChampiMap(40);
            LeMonde.AjouterPierresMap(25);

            LeMonde.NbChampignons = 20;
            LeMonde.NbPierres = 40;
        }


        /// <summary>
        /// Permet d'actualiser la carte et de faire bouger les elfes à chaque heure
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        /*public void SimuleUneHeure(object source, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine("===================== Jour " + (_jour+1) + " =====================\n");
            Console.WriteLine("==================  " + (i) + " heure(s)  ==================");
            LeMonde.AfficherJeu();
            LeMonde.DefinirObjectifsConstruction();
            foreach (Elfe unElfe in LeMonde._listeElfes)
            {
                Console.WriteLine(unElfe);
                if (unElfe.Energie != 0 )
                {
                    unElfe.Action(LeMonde);  // chaque elfe réalise son action au début du tour s'il a de l'énergie et pas d'objectif à atteindre
                }
                else unElfe.BesoinEnergie();  // sinon il doit retourner au chaudron sans effectuer d'action

                if (unElfe.Abs_objectif != -1)  // si l'elfe a un objectif, il s'y rend
                {
                    Console.WriteLine("aaaaaa");
                    unElfe.Deplacer(unElfe.Abs_objectif, unElfe.Ord_objectif);
                }
                else unElfe.Promener(LeMonde);  // sinon il se promène
            }
            i++;
        }*/




        /// <summary>
        /// Création d'un timer qui appelera plusieurs fois 'SimuleUneHeure' et permettra de créer une impression de mouvement
        /// </summary>
        /// <param name="nbHeures"></param>
        /*public void SimuleHeures(int nbHeures)
        {
            i = 1;
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(this.SimuleUneHeure);
            myTimer.Interval = 1500;
            myTimer.Start();
            while (i <= nbHeures) ;
            myTimer.Stop();
        }*/

        public void SimuleDesHeures(int nbHeures)
        {
            i = 1;
            while (i <= nbHeures)
            {
                Console.Clear();
                Console.WriteLine("===================== Jour " + (_jour + 1) + " =====================\n");
                Console.WriteLine("==================  " + (i) + " heure(s)  ==================");
                LeMonde.AfficherJeu();
                LeMonde.DefinirObjectifsConstruction();
                List<Elfe> copieListe = CopieListe(LeMonde._listeElfes);
                foreach (Elfe unElfe in copieListe)
                {
                    if (unElfe.Energie != 0 && unElfe.Abs_objectif == -1)
                    {
                        unElfe.Action(LeMonde);  // chaque elfe réalise son action au début du tour s'il a de l'énergie et pas d'objectif à atteindre
                    }
                    else unElfe.BesoinEnergie();  // sinon il doit retourner au chaudron sans effectuer d'action

                    if (unElfe.Abs_objectif != -1)  // si l'elfe a un objectif, il s'y rend
                    {
                        unElfe.Deplacer(unElfe.Abs_objectif, unElfe.Ord_objectif);
                    }
                    else if (unElfe.NbToursOccupe==0) unElfe.Promener(LeMonde);  // sinon il se promène
                }
                Console.WriteLine("Appuyez sur n'importe quelle touche pour passer à l'heure suivante");
                Console.ReadKey();
                i++;
            }
        }

        /// <summary>
        /// Nécéssité de créer une copie de la liste pour ne pas avoir à modifier la liste pendant le foreach de la fonction SimuleDesHeures
        /// </summary>
        /// <param name="listeACopier"></param>
        /// <returns></returns>
        public List<Elfe> CopieListe(List<Elfe> listeACopier)
        {
            List<Elfe> listeCopiee = new List<Elfe> ();
            foreach(Elfe elfe in listeACopier)
            {
                listeCopiee.Add(elfe);
            }
            return listeCopiee;
        }

        public void SimuleUnJour()
        {
            LeMonde.AjouterObjetsMap();
            if (_jour /3 == 0)
            {
                LeMonde.NaissanceElfe();
            }
            LeMonde.EventuellePluieAcide();
            SimuleDesHeures(24);
            LeMonde.AfficherListes();
            LeMonde.Repas();
            LeMonde.DemanderConstruction();
            if (LeMonde.EstChaudron().NbToursAcidite > 0)
            {
                LeMonde.EstChaudron().NbToursAcidite -= 1;
            }

        }

        /// <summary>
        /// Permet de gérer la partie en cours (défaite ou victoire), fait appel aux méthodes précédentes
        /// </summary>
        public void SimulationPartie()
        {
            Console.WriteLine("\nPendant combien de jours voulez-vous jouer ?");
            int nbPartie;

            string caractNb = Console.ReadLine();

            while (int.TryParse(caractNb, out nbPartie) == false || int.Parse(caractNb) <= 0)
            {
                Console.Write("\nAttention! Veuillez rentrer un nombre positif.\n ");
                caractNb = Console.ReadLine();
            }
            nbPartie = int.Parse(caractNb);

            while (_jour<nbPartie)
            {

                if (LeMonde._listeElfes.Count != 0)
                {
                    SimuleUnJour();
                    Console.WriteLine("Appuyez sur une touche pour passer au jour suivant");
                    Console.ReadKey();
                    _jour += 1;
                }

                if (LeMonde._listeElfes.Count==0)
                {
                    _jour = nbPartie;
                    Console.WriteLine();
                    Console.WriteLine("Vous n'avez plus aucun elfe en vie.");
                    Console.WriteLine("GAME OVER");
                }
            }
            if (LeMonde._listeElfes.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("C'EST GAGNÉ BRAVO, vous avez réussi à faire survivre au moins un elfe !!!!");
            }
            Console.WriteLine();
            Console.WriteLine("Merci d'avoir joué !");
        }
    }
}
