using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie2.BusinessLogic
{
    class Compte
    {

        // Propriétés
        public uint Identifiant { get; }
        public double Solde { get; set; }
        public const double RetraitAutorise = 1000;
        public List<Transaction> Historique { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateResiliation { get; set; }



        // Constructeur

        public Compte(uint identifiant, double solde)
        {
            Identifiant = identifiant;
            Solde = solde;
            Historique = new List<Transaction>();


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
                //if (Historique is null)
                //{
                //    Solde -= montant;
                //    return true;
                //}
                //else
                //{
                double total = Historique.Count > 0 ? Historique.GetRange(0, Historique.Count >= 9 ? 9 : Historique.Count).Sum(x => x.Montant) : 0;
                //int NbTxn = Historique.Count();
                //if (NbTxn >= 9)
                //{
                //    NbTxn = 9;

                //}
                //double MontantTotal = Historique.GetRange(0, NbTxn).Sum(x => x.Montant);
                if (total + montant <= RetraitAutorise)
                {
                    Solde -= montant;
                    return true;
                }
                //    }
            }
            return false;
        }

        internal bool Prelevement(double montant, Gestionnaire gestionnaire, Transaction transaction)
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
