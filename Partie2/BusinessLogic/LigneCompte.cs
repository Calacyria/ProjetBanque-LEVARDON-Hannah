using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie2.BusinessLogic
{
    class LigneCompte
    {
       
        // Propriétés
        public uint Identifiant { get; set; }
        public DateTime Date { get; set; }
        public double Solde { get; set; }
        public uint Entree { get; set; }
        public uint Sortie { get; set; }

        // Constructeur
        public LigneCompte(uint identifiant, DateTime date, double solde, uint entree, uint sortie)
        {
            Identifiant = identifiant;
            Date = date;
            Solde = solde;
            Entree = entree;
            Sortie = sortie;
        }



    }
}
