using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie1.BusinessLogic
{
    class Compte
    {

        // Propriétés
        public uint Identifiant { get; }
        public double Solde { get; set; }
        public double RetraitAutorise { get; set; }
        public List<Transaction> Historique { get; set; }



        // Constructeur
        public Compte(uint identifiant, double solde, double retraitAutorise, List<Transaction> historique)
        {
            Identifiant = identifiant;
            Solde = solde;
            RetraitAutorise = retraitAutorise;
            Historique = historique;
        }
        public Compte(uint identifiant, double solde)
        {
            Identifiant = identifiant;
            Solde = solde;

        }



        // Méthodes

        internal bool Depot(double montant)
        {
            if (montant > 0)
            {
                Solde += montant;
                return true;
            }


            return false;
        }// end Depot

        internal bool Retrait(double montant)
        {
            if (montant < Solde)
            {
                if (Historique is null)
                {
                    Solde -= montant;
                    return true;
                }
                else
                {
                    double MontantTotal = Historique.Sum(x => x.Montant);
                    if (MontantTotal < 1000)
                    {
                        Solde -= montant;
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool Prelevement(double montant)
        {

            return Retrait(montant);
        }

        internal bool Virement(double montant)
        {
            if (Depot(montant))
            {
                return true;
            }
            return false;

        }



    }
}
