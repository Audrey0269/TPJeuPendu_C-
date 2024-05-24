using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPJeuDuPendu
{
    internal class Program
    {        
        static void Main(string[] args)
        {
            string rejouer = "o";
            string motCache = "";
            string motDecouvert = "";
            int coup = 10;
            string lettre = "";
            bool gagne = false;

            
            // déclare une liste pour stocker les mots
            //string[] mots = new string[20];
            List<string> mots = new List<string>() { "un", "deux", "cinq", "rouge", "membre", "conseil", "donner", "reponse", "etat", "son", "armement" };
            mots.Add("peu");
            mots.Add("apres");
            mots.Add("vacances");
            mots.Add("annonce");
            mots.Add("mercredi");
            mots.Add("evident");
            mots.Add("regime");
            mots.Add("affirmer");
            mots.Add("arme");

            // affiche du texte
            Console.WriteLine("Bienvenu dans le Jeu du pendu");

            // boucle tantque rejouer == "o"
            while (rejouer.ToLower() == "o") {

                // récupère la valeur retourné par ChoisirMot
                //motCache = ChoisirMot(mots);

                // récupère un mot à partir du fichier
                motCache = ChoisirMot("mots.txt");

                Console.WriteLine("Triche : " + motCache);

                // appel InitEtoile pour récupérer la valeur dans motDecouvert
                motDecouvert = InitEtoile(motCache);

                // remet coup à 10 pour une nouvelle partie
                coup = 10;

                // remet gagne à false pour une nouvelle partie
                gagne = false;

                // boucle tantque il reste des coups à jouer ou que le joueur n'a pas trouvé le mot
                while (coup > 0 && !gagne)
                {
                    // Affiche le nombre de coups restant
                    Console.WriteLine($"Il vous reste {coup} coups à jouer");
                    string pendu = Dessine(coup);
                    Console.WriteLine(pendu);

                    // Affiche motDecouvert
                    Console.WriteLine($"Le mot secret est : {motDecouvert}");

                    // appel GetCaractere
                    lettre = GetCaractere();

                    // appel TestCaractere
                    bool lettreTrouve = TestCaractere(lettre, motCache, ref motDecouvert);

                    // si la lettre n'est pas trouvé
                    if (!lettreTrouve)
                    {
                        // décrémente coup si le jouer n'a pas trouvé un caractère
                        coup--;
                    }


                    // appel TestGagne
                    gagne = TestGagne(motCache, motDecouvert);
                }

                // Affiche le message si le joueur a trouvé le mot
                if (gagne)
                {
                    Console.WriteLine($"Bravo vous avez gagné, le mot secret était : {motCache}");
                }
                // Affiche le message si le joueur n'a pas trouvé le mot
                else
                {
                    Console.WriteLine($"Il vous reste {coup} coups à jouer");
                    string pendu = Dessine(coup);
                    Console.WriteLine(pendu);

                    Console.WriteLine($"Désolé vous avez perdu ! le mot secret était : {motCache}");
                }

                // demande à l'utilisateur si il veut rejouer
                Console.WriteLine("Vouslez vous rejouer ? (o / n)");
                rejouer = Console.ReadLine();
            }
        }

        /// <summary>
        /// Génère un nombre aléatoire
        /// </summary>
        /// <param name="min">int min</param>
        /// <param name="max">int max</param>
        /// <returns>nombre aléatoire</returns>
        static int GetNombreAleatoire(int min, int max)
        {
            int nombre = 0;

            Random rnd = new Random();

            // génère un nombre aléatoire compris entre min et max
            nombre = rnd.Next(min, max);

            return nombre;
        }

        static string ChoisirMot(List<string> list)
        {
            // récupère un nombre aléatoire avec GetNombreAleatoire
            int nbAleatoire = GetNombreAleatoire(0, list.Count - 1);

            // retourne le mot qui se trouve dans la liste à la position nbAleatoire
            return list[nbAleatoire];
        }

        static string ChoisirMot(string path)
        {
            List<string> mots = new List<string>();
            string ligne = "";

            try
            {
                // crée un StreamReader avec le path du fichier
                StreamReader sr = new StreamReader(path);

                // lit une ligne
                ligne = sr.ReadLine();

                while (ligne != null)
                {
                    // ajoute la ligne avant de lire la suivante
                    mots.Add(ligne);

                    // lit la ligne suivante
                    ligne = sr.ReadLine();
                }

                // ferme le fichier
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }


            // retourne le mot qui se trouve dans la liste à la position nbAleatoire
            return mots[GetNombreAleatoire(0, mots.Count - 1)];
        }

        // GetCaractere
        static string GetCaractere()
        {
            Console.Write("Proposez une lettre : ");

            return Console.ReadLine();
        }

        // InitEtoile
        static string InitEtoile(string motCache)
        {
            string motEtoile = "";

            // boucle sur les char de motCache
            foreach (char c in motCache)
            {
                // ajoute * dans motEtoile
                motEtoile += "*";
            }

            return motEtoile;
        }

        // TestCaractère
        static bool TestCaractere(string lettre, string motCache, ref string motDecouvert) {
            bool trouve = false;
            string lettreDecouverte = "";

            for (int i = 0; i < motCache.Length; i++)
            {
                if (motCache[i] == lettre[0])
                {
                    // ajoute la lettre découverte
                    lettreDecouverte += lettre;
                    // il a trouve une lettre
                    trouve = true;
                }
                else
                {
                    // ajoute la lettre qu'il a déjà découverte
                    lettreDecouverte += motDecouvert[i];
                }
            }

            // modifier motDecouvert avec lettreDecouverte
            motDecouvert = lettreDecouverte;

            return trouve;
        }

        // TestGagne
        static bool TestGagne(string motCache, string motDecouvert)
        {
            return motCache.Equals(motDecouvert);
        }

        static string Dessine(int coup)
        {
            string[] pendues = new string[]
            {
            " -----\n |  | \n |  o \n | /|\\\n | / \\\n_|_   \n",
            " -----\n |  | \n |  o \n | /|\\\n |    \n_|_   \n",
            " -----\n |  | \n |  o \n |    \n |    \n_|_   \n",
            " -----\n |  | \n |    \n |    \n |    \n_|_   \n",
            " -----\n |    \n |    \n |    \n |    \n_|_   \n",
            "      \n |    \n |    \n |    \n |    \n_|_   \n",
            "      \n      \n |    \n |    \n |    \n_|_   \n",
            "      \n      \n      \n |    \n |    \n_|_   \n",
            "      \n      \n      \n      \n |    \n_|_   \n",
            "      \n      \n      \n      \n      \n_|_   \n",
            "      \n      \n      \n      \n      \n      \n"
            };

            return pendues[coup];
        }
    }
}
