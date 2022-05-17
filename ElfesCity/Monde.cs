using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;


namespace ElfesCity
{
    public class Monde
    {
        public Random rng = new Random();

        private string[,] _mat;

        public int NbLignes { get; set; }

        public int NbColonnes { get; set; }

        public List<Elfe> _listeElfes;

        public List<Objet> _listeObjets;

        public List<Batiment> _listeBatiments;

        public int NbChampignons { get; set; }
        public int NbPierres { get; set; }


        //======================== INITIALISATIONS ========================

        /// <summary>
        /// crée une matrice remplie d'espaces " " représentant le plateau de jeu
        /// </summary>
        private void InitialiseMatrice()
        {
            _mat = new string[NbLignes, NbColonnes];
            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    _mat[i, j] = " ";
                }
            }
        }

        public Monde(int nbLignes, int nbColonnes)
        {
            NbLignes = nbLignes;
            NbColonnes = nbColonnes;
            InitialiseMatrice();
            _listeElfes = new List<Elfe>();
            _listeObjets = new List<Objet>();
            _listeBatiments = new List<Batiment>();
            NbChampignons = 0;
            NbPierres = 10;
        }

        /// <summary>
        /// génere des coord aléatoires pour placer des éléments
        /// </summary>
        /// <param name="nbLignes"></param>
        /// <param name="nbColonnes"></param>
        /// <returns> un couple (x,y) de coordonnées </returns>
        private int[] GenererCoord(int nbLignes, int nbColonnes)
        {
            int x = rng.Next(0, nbLignes);
            int y = rng.Next(0, nbColonnes);
            int[] coord = new int[] { x, y };

            return coord;
        }

        public bool EstVide(int x, int y)
        {
            if (_mat[x, y] == " ")
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Remplit le stock de champignons ou de pierres
        /// </summary>
        /// <param name="typeRessource">"pierre" pour remplir l'atelier de pierres, "champi" pour remplir le silo de champignons</param>
        /// <param name="nbARajouter"> nb de ressources à rajouter </param>
        public void RemplirStock(string typeRessource, int nbARajouter)
        {
            if (typeRessource == "champi")
            {
                if (this.NbChampignons <= this.CapaciteMaxTotale("s") - nbARajouter)
                {
                    this.NbChampignons += nbARajouter;
                }
                else
                {
                    this.NbChampignons = this.CapaciteMaxTotale("s");
                }
            }
            if (typeRessource == "pierre")
            {
                if (this.NbPierres <= this.CapaciteMaxTotale("a") - nbARajouter)
                {
                    this.NbPierres += nbARajouter;
                }
                else
                {
                    this.NbPierres = this.CapaciteMaxTotale("a");
                }
            }
        }

        public void AjouterElfe(Elfe elfe)
        {
            _listeElfes.Add(elfe);
        }

        public void TuerElfe(Elfe elfe)
        {
            _listeElfes.Remove(elfe);
        }


        public List<ElfeConstructeur> ConstructeursLibres()
        {
            List<ElfeConstructeur> listeConstructeursLibres = new List<ElfeConstructeur>();

            foreach (Elfe elfe in _listeElfes)
            {
                if (elfe is ElfeConstructeur)
                {
                    if (elfe.NbToursOccupe == 0 && elfe.Energie > 0)
                    {
                        listeConstructeursLibres.Add((ElfeConstructeur)elfe);
                    }
                }
            }
            return listeConstructeursLibres;
        }

        /// <summary>
        /// Renvoie la capacité maximum de tous les bâtiments de stockage d'un même type (silo ou atelier) réunis
        /// </summary>
        /// <param name="typeBatiment"> "s" pour silo ou "a" pour atelier </param>
        /// <returns></returns>
        public int CapaciteMaxTotale(string typeBatiment)
        {
            int capaciteTot = 0;

            foreach (Batiment batiment in _listeBatiments)
            {
                if (typeBatiment == "s" && batiment is Silo && batiment.EstConstruit())
                {
                    capaciteTot += batiment.CapaciteMax;
                }
                else if (typeBatiment == "a" && batiment is Atelier && batiment.EstConstruit())
                {
                    capaciteTot += batiment.CapaciteMax;
                }
            }
            return capaciteTot;
        }

        //======================== AFFICHAGE DE LA MAP ========================

        /// <summary>
        /// Ajoute un objet quelconque à la map 
        /// </summary>
        /// <param name="objet"></param>
        /// <returns></returns>
        public bool AjouterObjetPlateau(Objet objet) // place un objet sur le plateau si la case est libre
        {
            if (EstVide(objet.Abscisse, objet.Ordonnee) == true)
            {
                _mat[objet.Abscisse, objet.Ordonnee] = objet.Type;
                _listeObjets.Add(objet);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Ajoute nb champignons sur la carte. Les champignons ont 1 chance sur 10 d'être toxiques
        /// </summary>
        /// <param name="nb"></param>
        public void AjouterChampiMap(int nb)
        {
            int nbChampisAjoutes = 1;
            while (nbChampisAjoutes != nb)
            {
                int[] coord = GenererCoord(this.NbLignes, this.NbColonnes);
                while (this.EstVide(coord[0], coord[1]) != true)
                {
                    coord = GenererCoord(this.NbLignes, this.NbColonnes);
                }
                int probaToxique = rng.Next(10);
                bool toxique;
                if (probaToxique == 2)
                {
                    toxique = true;
                }
                else toxique = false;
                Champignons unChampi = new Champignons(coord[0], coord[1], toxique);
                this.AjouterObjetPlateau(unChampi);
                nbChampisAjoutes += 1;
            }
        }

        /// <summary>
        /// Ajoute nb pierres sur la carte. Les pierres ont 1 chance sur 10 d'être tranchantes
        /// </summary>
        /// <param name="nb"></param>
        public void AjouterPierresMap(int nb)
        {
            int nbPierresAjoutes = 0;
            while (nbPierresAjoutes != nb)
            {
                int[] coord = GenererCoord(this.NbLignes, this.NbColonnes);
                while (this.EstVide(coord[0], coord[1]) != true)
                {
                    coord = GenererCoord(this.NbLignes, this.NbColonnes);
                }
                int probaTranchant = rng.Next(10);
                bool tranchant;
                if (probaTranchant == 2)
                {
                    tranchant = true;
                }
                else tranchant = false;
                Pierres unePierre = new Pierres(coord[0], coord[1], tranchant);
                this.AjouterObjetPlateau(unePierre);
                nbPierresAjoutes += 1;
            }
        }

        /// <summary>
        /// Méthode appelée au début du jour afin de gérer l'apport d'objet
        /// </summary>
        public void AjouterObjetsMap()
        {
            if (this.NbChampiMap() < 40)  // s'il y a moins de 40 champis sur la carte, on en rajoute entre 1 et 3
            {
                int nbChampis = rng.Next(1, 4);
                AjouterChampiMap(nbChampis);
            }
            if (this.NbPierresMap() < 40)  // idem
            {
                int nbPierres = rng.Next(1, 4);
                AjouterPierresMap(nbPierres);
            }
        }

        public void RetirerObjetMap(Objet objet)
        {
            _listeObjets.Remove(objet);
            _mat[objet.Abscisse, objet.Ordonnee] = " ";
        }

        /// <summary>
        /// Ajoute un bâtiment sur le plateau. Si impossible (case occupée), renvoie false
        /// </summary>
        /// <param name="batiment"> le bâtiment à ajouter au plateau </param>
        /// <returns> True si l'action a réussi, false sinon </returns>
        public bool AjouterBatiment(Batiment batiment)
        {
            if (EstVide(batiment.Abscisse, batiment.Ordonnee) == true)
            {
                _mat[batiment.Abscisse, batiment.Ordonnee] = batiment.Fonction;
                _listeBatiments.Add(batiment);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Cette fonction ajoute les elfes sur le plateau de jeu pour pouvoir les afficher uniquement sur les cases où il n'y a pas d'objets.
        /// </summary>
        public void AjouterElfesDansMonde()
        {
            foreach (Elfe unElfe in _listeElfes)
            {
                if (_mat[unElfe.Abscisse, unElfe.Ordonnee] == " ")
                {
                    _mat[unElfe.Abscisse, unElfe.Ordonnee] = unElfe.Poste;
                }
                else _mat[unElfe.Abscisse, unElfe.Ordonnee] = unElfe.Poste + _mat[unElfe.Abscisse, unElfe.Ordonnee];
            }
        }

        /// <summary>
        /// Cette fonction retire les elfes du plateau de jeu (mais pas du monde). Elle est utilisée une fois le plateau affiché.
        /// </summary>
        public void RetirerElfesDansMonde()
        {
            for (int i = 0; i < _mat.GetLength(0); i++)
            {
                for (int j = 0; j < _mat.GetLength(1); j++)
                {
                    if (_mat[i, j].Contains("°") || _mat[i, j].Contains("i") || _mat[i, j].Contains("~"))
                    {
                        _mat[i, j] = _mat[i, j].Replace("°", string.Empty);
                        _mat[i, j] = _mat[i, j].Replace("i", string.Empty);
                        _mat[i, j] = _mat[i, j].Replace("~", string.Empty);
                    }
                }
            }
        }

        public void AfficherJeu()
        {
            AjouterElfesDansMonde();
            Console.WriteLine();

            for (int i = 0; i < _mat.GetLength(0); i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.WriteLine();

                for (int j = 0; j < _mat.GetLength(1); j++)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGray;

                    if (_mat[i, j].Contains("c"))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write(" ");
                    }

                    else if (_mat[i, j].Contains("o"))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write(" ");
                    }
                    else if (_mat[i, j].Contains("p"))
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" ");
                    }
                    else if (_mat[i, j].Contains("s"))
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write(" ");
                    }
                    else if (_mat[i, j].Contains("a"))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.Write(" ");
                    }

                    else
                    {
                        Console.Write(" ");
                    }

                    if (_mat[i, j].Contains("°") || _mat[i, j].Contains("i") || _mat[i, j].Contains("~"))
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(_mat[i, j][0]);
                    }
                    else Console.Write(" ");
                }
            }
            Console.ResetColor();
            Console.WriteLine(" ");
            RetirerElfesDansMonde();
        }


        //======================== INFO DE LA MAP ========================


        public Champignons EstChampi(int x, int y)
        {
            foreach (Objet objet in _listeObjets)
            {
                if (objet.Abscisse == x && objet.Ordonnee == y && objet is Champignons)
                {
                    return (Champignons)objet;
                }
            }
            return null;
        }

        public Pierres EstPierre(int x, int y)
        {
            foreach (Objet objet in _listeObjets)
            {
                if (objet.Abscisse == x && objet.Ordonnee == y && objet is Pierres)
                {
                    return (Pierres)objet;
                }
            }
            return null;
        }

        /// <summary>
        /// Renvoie le chaudron pour pouvoir utiliser ses propriétés
        /// </summary>
        /// <returns></returns>
        public Chaudron EstChaudron()
        {
            foreach (Object objet in _listeObjets)
            {
                if (objet is Chaudron) return (Chaudron)objet;
            }
            return null;
        }

        public Batiment EstBatiment(int x, int y)
        {
            foreach (Batiment unBatiment in _listeBatiments)
            {
                if (unBatiment.Abscisse == x && unBatiment.Ordonnee == y)
                {
                    return unBatiment;
                }
            }
            return null;
        }

        public int NbSilo()
        {
            int nbsilo = 0;

            foreach (Batiment batiment in _listeBatiments)
            {
                if (batiment is Silo)
                {
                    nbsilo += 1;
                }
            }
            return nbsilo;
        }

        public int NbAtelier()
        {
            int nbatelier = 0;

            foreach (Batiment batiment in _listeBatiments)
            {
                if (batiment is Atelier)
                {
                    nbatelier += 1;
                }
            }
            return nbatelier;
        }

        public int NbElfeCueilleur()
        {
            int nbcueilleur = 0;

            foreach (Elfe elfe in _listeElfes)
            {
                if (elfe is ElfeCueilleur)
                {
                    nbcueilleur += 1;
                }
            }
            return nbcueilleur;
        }

        public int NbElfeRamasseur()
        {
            int nbramasseur = 0;

            foreach (Elfe elfe in _listeElfes)
            {
                if (elfe is ElfeRamasseur)
                {
                    nbramasseur += 1;
                }
            }
            return nbramasseur;
        }

        public int NbElfeConstructeur()
        {
            int nbconstructeur = 0;

            foreach (Elfe elfe in _listeElfes)
            {
                if (elfe is ElfeConstructeur)
                {
                    nbconstructeur += 1;
                }
            }
            return nbconstructeur;
        }

        /// <summary>
        /// renvoie le nombre de champignons présents sur la carte
        /// </summary>
        /// <returns></returns>
        public int NbChampiMap()
        {
            int nbchampi = 0;

            foreach (Objet objet in _listeObjets)
            {
                if (objet is Champignons)
                {
                    nbchampi += 1;
                }
            }
            return nbchampi;
        }

        /// <summary>
        /// renvoie le nombre de pierres présentes sur la carte
        /// </summary>
        /// <returns></returns>
        public int NbPierresMap()
        {
            int nbpierre = 0;

            foreach (Objet objet in _listeObjets)
            {
                if (objet is Pierres)
                {
                    nbpierre += 1;
                }
            }
            return nbpierre;
        }


        //======================== CHAUDRON ========================

        public void EventuellePluieAcide()
        {
            int probaPluieAcide = rng.Next(20);
            if (probaPluieAcide == 2)  // une chance sur 20
            {
                EstChaudron().NbToursAcidite = 5;
            }
        }


        //======================== NAISSANCE ========================

        public void NaissanceElfe()
        {
            int[] coord = GenererCoord(NbLignes, NbColonnes);
            while (EstVide(coord[0], coord[1]) == false)
            {
                coord = GenererCoord(NbLignes, NbColonnes);
            }

            int aleaTypeElfe = rng.Next(3);  // pour choisir aléatoirement un type d'elfe
            Elfe nouvelElfe;
            if (aleaTypeElfe == 0)
            {
                nouvelElfe = new ElfeCueilleur(coord[0], coord[1], 0);
                Console.WriteLine("Un elfe cueilleur est né !");
            }
            else if (aleaTypeElfe == 1)
            {
                nouvelElfe = new ElfeRamasseur(coord[0], coord[1], 0);
                Console.WriteLine("Un elfe ramasseur est né !");
            }
            else
            {
                nouvelElfe = new ElfeConstructeur(coord[0], coord[1], 0);
                Console.WriteLine("Un elfe constructeur est né !");
            }
            this.AjouterElfe(nouvelElfe);
        }



        //======================== CONSTRUCTION ========================

        /// <summary>
        /// S'il y a des bâtiments non terminés et sur lesquels aucun elfe n'est posté, la fonction affecte des elfes constructeurs libres sur ces bâtiments
        /// </summary>
        public void DefinirObjectifsConstruction()
        {
            foreach (Batiment unBatiment in _listeBatiments)
            {
                if (unBatiment.EstConstruit() == false && unBatiment.Constructeur == false)
                {
                    List<ElfeConstructeur> listeConstructeursLibres = ConstructeursLibres();
                    if (listeConstructeursLibres.Count > 0)
                    {
                        ElfeConstructeur unElfeLibre = listeConstructeursLibres[0];
                        unElfeLibre.Abs_objectif = unBatiment.Abscisse;
                        unElfeLibre.Ord_objectif = unBatiment.Ordonnee;
                        unBatiment.Constructeur = true;
                        unElfeLibre.NbToursOccupe = (unBatiment.NiveauRequis - unBatiment.NiveauActuel);
                        Console.WriteLine("Un elfe est affecté a une construction en {0},{1}. Il va être occupé pendant {2} tours", unBatiment.Abscisse, unBatiment.Ordonnee, unElfeLibre.NbToursOccupe);
                    }
                }
            }
        }

        /// <summary>
        /// Demande à l'utilisateur si il souhaite construire un bâtiment seulement si ses ressources le lui permettent
        /// </summary>
        public void DemanderConstruction()
        {
            //si le joueur n'a pas assez de pierre ou de constructeur libre, ça ne sert a rien de lui demande de faire une construction
            if (NbPierres >= 14 && ConstructeursLibres().Count>0 )
            {
                Console.WriteLine("\nSouhaitez-vous construire un silo ou un atelier? Tapez a pour un atelier, s pour un silo. Pour ne rien construire, tapez x. ");
                char reponse = Console.ReadKey().KeyChar;

                int x;
                int y;

                Console.WriteLine();
                while (reponse != 'a' && reponse != 's' && reponse != 'x')
                {
                    Console.WriteLine("\nVous avez saisi un caractère erroné. Tapez a pour construire un atelier, s pour un silo ou x pour ne rien construire.");

                    reponse = Console.ReadKey().KeyChar;
                }
                if (reponse == 'a' || reponse == 's')
                {
                    //on verifie qu'il y a assez de pierres dispo pour construire un atelier
                    if (reponse == 'a' && NbPierres >= 17)
                    {
                        Console.WriteLine();
                        
                        int[] coord = DeplacerCurseur("a");
                        x = coord[0];
                        y = coord[1];
                        Atelier atelier = new Atelier(x, y);
                        atelier = new Atelier(x, y);
                        _listeBatiments.Add(atelier);
                    }

                    else if (reponse == 'a' && NbPierres < 17)
                    {
                        Console.WriteLine("Vous n'avez assez de pierres pour construire un atelier.");

                        Console.WriteLine("\nEst-ce que vous voulez construire un autre bâtiment ? Tapez s pour un silo ou x pour ne rien construire.");
                        Console.WriteLine();

                        reponse = Console.ReadKey().KeyChar;
                    }

                    if (reponse == 's' && NbPierres >= 14)
                    {
                        Console.WriteLine();
                        int[] coord = DeplacerCurseur("s");
                        x = coord[0];
                        y = coord[1];
                        Silo silo = new Silo(x, y);
                        silo = new Silo(x, y);
                        _listeBatiments.Add(silo);
                    }
                    else if (reponse == 's' && NbPierres < 14)
                    {
                        Console.WriteLine("Vous n'avez assez de pierres pour construire un silo.");
                    }
                }
            }
            else if (NbPierres < 14)
            {
                Console.WriteLine();
                Console.WriteLine("Vous n'avez pas assez de pierres pour construire un bâtiment.");
            }
            else if (ConstructeursLibres().Count==0)
            {
                Console.WriteLine();
                Console.WriteLine("Vous n'avez pas d'elfe constructeur libre.");
            }
        }


        //v=valider
        //z=monter
        //s=descendre
        //q=gauche
        //d=droite
        /// <summary>
        /// Cette fonction permet de choisir où placer un bâtiment grace aux touches du clavier
        /// </summary>
        /// <param name="bat"></param>
        /// <returns></returns>
        public int[] DeplacerCurseur(string bat)
        {
            // Création matrice de comparaison
            string[,] _matcopie = new string[NbLignes, NbColonnes];

            for (int i = 0; i < NbLignes; i++)
            {
                for (int j = 0; j < NbColonnes; j++)
                {
                    _matcopie[i, j] = _mat[i, j];
                }
            }

            char curseur = ' ';
            bool validation = false;

            int x = 0;
            int y = 0;
            _mat[x, y] = bat.ToString();
            MajjeuConstruction();

            while (validation == false)
            {
                curseur = Console.ReadKey().KeyChar;
                if (curseur == 'z' && x - 1 >= 0)
                {
                    _mat[x, y] = _matcopie[x, y];
                    x -= 1;
                    _mat[x, y] = bat.ToString();
                    MajjeuConstruction();
                }

                else if (curseur == 's' && x + 1 < NbLignes)
                {
                    _mat[x, y] = _matcopie[x, y];
                    x += 1;
                    _mat[x, y] = bat.ToString();
                    MajjeuConstruction();
                }

                else if (curseur == 'd' && y + 1 < NbColonnes)
                {
                    _mat[x, y] = _matcopie[x, y];
                    y += 1;
                    _mat[x, y] = bat.ToString();
                    MajjeuConstruction();
                }

                else if (curseur == 'q' && y - 1 >= 0)
                {
                    _mat[x, y] = _matcopie[x, y];
                    y -= 1;
                    _mat[x, y] = bat.ToString();
                    MajjeuConstruction();
                }

                else if (curseur != 'v')
                {
                    MajjeuConstruction();
                    Console.WriteLine("Déplacement impossible");
                }

                else if (curseur == 'v')
                {

                    foreach (Batiment batiment in _listeBatiments)
                    {
                        if (x == batiment.Abscisse && y == batiment.Ordonnee)
                        {
                            MajjeuConstruction();
                            Console.WriteLine("Placement impossible : il y a déjà un bâtiment ici.");
                            validation = false;
                            break;
                        }
                        else
                        {
                            foreach (Objet objet in _listeObjets)
                            {
                                if (x == objet.Abscisse && y == objet.Ordonnee)
                                {
                                    MajjeuConstruction();
                                    Console.WriteLine("Placement impossible : il y a un objet ici.");
                                    validation = false;
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    AfficherJeu();
                                    validation = true;
                                }

                            }
                        }
                    }
                    
                }
            }

            Console.WriteLine("Bâtiment placé");
            int[] coord = new int[] { x, y };
            return coord;
        }

        public void MajjeuConstruction()
        {
            Console.Clear();
            AfficherJeu();
            Console.WriteLine("Déplacez votre curseur sur la map avec les touches z, q, s, d et appuyez sur v pour valider votre choix.");
        }



        //======================== REPAS ET MORT ========================

        public void Repas()
        {
            int NbElfesATuer = 0;
            int NbElfesATuerBis = 0;
            Console.WriteLine();
            Console.Write("Vous avez {0} elfe(s) à nourrir et {1} champignon(s) en stock.", _listeElfes.Count, NbChampignons);

            if (NbChampignons < _listeElfes.Count)  // s'il n'y a pas assez de champignons pour tout le monde
            {
                NbElfesATuer = _listeElfes.Count - NbChampignons;  // diminue au fur et à mesure que des elfes meurent
                NbElfesATuerBis = _listeElfes.Count - NbChampignons;  // reste fixe
                NbChampignons = 0;
                Console.WriteLine(" {0} elfes vont donc succomber à la faim ce soir.", NbElfesATuer);
            }
            else // s'il y a assez de champignons, le stock descend
            {
                NbChampignons -= _listeElfes.Count;
                Console.WriteLine(" Tous vos elfes se remplissent allègrement la panse.");
            }
            if (NbElfesATuer > 0 && NbElfeCueilleur()>0)
            {
                NbElfesATuer = DemanderMortCueilleur(NbElfesATuer);
            }

            if (NbElfesATuer > 0 && NbElfeRamasseur() > 0)
            {
                NbElfesATuer = DemanderMortRamasseur(NbElfesATuer);
                Console.WriteLine("\nIl vous reste " + NbElfesATuer+" elfe(s) à sacrifier.");
            }

            if (NbElfesATuer > 0 && NbElfeConstructeur() > 0)
            {

                NbElfesATuer = DemanderMortConstructeur(NbElfesATuer);
                Console.WriteLine("\nIl vous reste " + NbElfesATuer+" elfe(s) à sacrifier.");
            }

            if (NbElfesATuer > 0)
            {
                Console.WriteLine("Vous n'avez pas sacrifié assez d'elfes (il en manque {0}). Ces derniers vont être sélectionnés au hasard dans la liste.",NbElfesATuer);
                for (int NbRestant = 0; NbRestant<NbElfesATuer; NbRestant++)
                {
                    TuerElfe(this._listeElfes[0]);
                }
            }

            if (NbElfesATuerBis > 0)
            {
                Console.WriteLine("\nAprès cette terrible journée, il vous reste: {0} cueilleurs, {1} ramasseurs et {2} constructeurs.",NbElfeCueilleur(),NbElfeRamasseur(),NbElfeConstructeur());
            }
        }


        //Les trois méthodes suivantes permettent de demander à l'utilisateur quel type d'elfe doit mourrir si la reserve de champi est trop faible

        public int DemanderMortCueilleur(int NbElfesATuer)
        {
            Console.WriteLine("Vous avez {0} elfes cueilleurs en jeu. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon. Si vous choisissez de ne pas en tuer, c'est irréversible pour ce tour.", NbElfeCueilleur());
            char reponse = Console.ReadKey().KeyChar;
            while (reponse != 'v' && reponse != 'x')
            {
                Console.WriteLine("\nVous avez saisi un caractère erroné. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon.");
                reponse = Console.ReadKey().KeyChar;
            }

            if (reponse == 'v')
            {
                Console.WriteLine();
                Console.WriteLine("Combien souhaitez-vous en sacrifier? (entre 0 et {0})", Math.Min(NbElfeCueilleur(), NbElfesATuer));
                int NbVoulu;  // le nombre d'elfes a tuer souhaité par l'utilisateur
                string caractNb = Console.ReadLine();
                // verification que la saisie est correcte
                while (int.TryParse(caractNb, out NbVoulu) == false || int.Parse(caractNb) > NbElfeCueilleur() || int.Parse(caractNb) < 0 || int.Parse(caractNb) > NbElfesATuer)
                {
                    Console.Write("\nAttention! Veuillez rentrer un nombre entre 0 et {0} : ", Math.Min(NbElfeCueilleur(), NbElfesATuer));
                    caractNb = Console.ReadLine();
                }
                NbVoulu = int.Parse(caractNb);

                int NbMort = 0;
                Console.WriteLine("\nVous avez choisi de tuer " + NbVoulu + " elfe(s) cueilleur(s).");
                //Parcours de la liste et seuls les elfes cueilleurs sont tués
                int i= 0;
                while (NbMort != NbVoulu)
                {
                    if (_listeElfes[i] is ElfeCueilleur)
                    {
                        TuerElfe(_listeElfes[i]);
                        NbMort += 1;
                    }
                    else i+=1;
                }
                NbElfesATuer -= NbMort;
                return NbElfesATuer;
            }

            else return NbElfesATuer;

        }

        public int DemanderMortRamasseur(int NbElfesATuer)
        {
            Console.WriteLine("\n\nVous avez {0} elfes ramasseurs en jeu. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon. Si vous choisissez de ne pas en tuer, c'est irréversible pour ce tour.", NbElfeRamasseur());
            char reponse = Console.ReadKey().KeyChar;
            while (reponse != 'v' && reponse != 'x')
            {
                Console.WriteLine("\nVous avez saisi un caractère erroné. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon.");
                reponse = Console.ReadKey().KeyChar;
            }

            if (reponse == 'v')
            {
                Console.WriteLine();
                Console.WriteLine("Combien souhaitez-vous en sacrifier? (entre 0 et {0})", Math.Min(NbElfeRamasseur(), NbElfesATuer));
                int NbVoulu;  // le nombre d'elfes a tuer souhaité par l'utilisateur
                string caractNb = Console.ReadLine();
                // verification que la saisie est correcte
                while (int.TryParse(caractNb, out NbVoulu) == false || int.Parse(caractNb) > NbElfeRamasseur() || int.Parse(caractNb) < 0 || int.Parse(caractNb) > NbElfesATuer)
                {
                    Console.Write("\nAttention! Veuillez rentrer un nombre entre 0 et {0} : ", Math.Min(NbElfeRamasseur(), NbElfesATuer));
                    caractNb = Console.ReadLine();
                }
                NbVoulu = int.Parse(caractNb);

                int NbMort = 0;
                Console.WriteLine("\nVous avez choisi de tuer " + NbVoulu + " elfe(s) ramasseur(s).");
                //Parcours de la liste et seuls les elfes ramasseurs sont tués
                int i = 0;
                while (NbMort != NbVoulu)
                {
                    if (_listeElfes[i] is ElfeRamasseur)
                    {
                        TuerElfe(_listeElfes[i]);
                        NbMort += 1;
                    }
                    else i += 1;
                }
                NbElfesATuer -= NbMort;
                return NbElfesATuer;
            }

            else return NbElfesATuer;

        }

        public int DemanderMortConstructeur(int NbElfesATuer)
        {
            Console.WriteLine("\n\nVous avez {0} elfes constructeurs en jeu. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon. Si vous choisissez de ne pas en tuer, c'est irréversible pour ce tour.", NbElfeConstructeur());
            char reponse = Console.ReadKey().KeyChar;
            while (reponse != 'v' && reponse != 'x')
            {
                Console.WriteLine("\nVous avez saisi un caractère erroné. Saisissez 'v' si vous souhaitez en éliminer, 'x' sinon.");
                reponse = Console.ReadKey().KeyChar;
            }

            if (reponse == 'v')
            {
                Console.WriteLine();
                Console.WriteLine("Combien souhaitez-vous en sacrifier? (entre 0 et {0})", Math.Min(NbElfeConstructeur(), NbElfesATuer));
                int NbVoulu;  // le nombre d'elfes a tuer souhaité par l'utilisateur
                string caractNb = Console.ReadLine();
                // verification que la saisie est correcte
                while (int.TryParse(caractNb, out NbVoulu) == false || int.Parse(caractNb) > NbElfeConstructeur() || int.Parse(caractNb) < 0 || int.Parse(caractNb) > NbElfesATuer)
                {
                    Console.Write("\nAttention! Veuillez rentrer un nombre entre 0 et {0} : ", Math.Min(NbElfeConstructeur(), NbElfesATuer));
                    caractNb = Console.ReadLine();
                }
                NbVoulu = int.Parse(caractNb);

                int NbMort = 0;
                Console.WriteLine("\nVous avez choisi de tuer " + NbVoulu + " elfe(s) constructeurs(s).");
                //Parcours de la liste et seuls les elfes constructeurs sont tués
                int i = 0;
                while (NbMort != NbVoulu)
                {
                    if (_listeElfes[i] is ElfeConstructeur)
                    {
                        TuerElfe(_listeElfes[i]);
                        NbMort += 1;
                    }
                    else i += 1;
                }
                NbElfesATuer -= NbMort;
                return NbElfesATuer;
            }

            else return NbElfesATuer;

        }


        //======================== AFFICHAGE INVENTAIRE ========================

        public void AfficherListesObjets()
        {
            Console.WriteLine("======= OBJETS SUR LA MAP ========");

            Console.Write("Champignons ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            Console.ResetColor();
            Console.WriteLine(" : {0}", NbChampiMap());

            Console.Write("Pierres ");
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write(" ");
            Console.ResetColor();
            Console.WriteLine(" : {0}", NbPierresMap());

            Console.Write("Chaudron ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(" \n");
            Console.ResetColor();

        }

        public void AfficherListesElfes()
        {
            Console.WriteLine("============= ELFES ==============");

            Console.WriteLine("Elfes cueilleurs ° : {0}", NbElfeCueilleur());
            Console.WriteLine("Elfes ramasseurs ~ : {0}", NbElfeRamasseur());
            Console.WriteLine("Elfes constructeurs i : {0}", NbElfeConstructeur());
            AfficherEnergieElfes();
        }

        public void AfficherEnergieElfes()
        {
            int nbElfe = 1;
            foreach (Elfe elfe  in _listeElfes)
            {
                Console.WriteLine(elfe);
                nbElfe += 1;
            }
        }

        public void AfficherListesBat()
        {
            Console.WriteLine("=========== BATIMENTS ============");

            Console.Write("Ateliers ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(" ");
            Console.ResetColor();
            Console.WriteLine(" : " +NbAtelier());
            Console.WriteLine("     Nombre de pierre(s) dans les ateliers : " + NbPierres);

            Console.Write("Silos ");
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(" ");
            Console.ResetColor();
            Console.WriteLine(" : "+ NbSilo());
            Console.WriteLine("     Nombre de champi(s) dans les ateliers : " + NbChampignons +" (Capacité max : "+  CapaciteMaxTotale("s") +")");
            Console.WriteLine("==================================");

        }

        /// <summary>
        /// Permet d'afficher toutes les listes 
        /// </summary>
        public void AfficherListes()
        {
            Console.WriteLine();
            AfficherListesObjets();
            AfficherListesElfes();
            AfficherListesBat();
        }
    }
}
