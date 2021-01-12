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
        public const double RetraitAutorise = 1000;
        public List<Transaction> Historique { get; set; }



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

                double total = Historique.Count > 0 ? Historique.GetRange(Historique.Count >= 9 ? Historique.Count - 9 : 0, Historique.Count >= 9 ? 9 : Historique.Count).Sum(x => x.Montant) : 0;
                //double total;
                //List<Transaction> ts = new List<Transaction>();
                //int count = Historique.Count;
                //if (count > 9)
                //{
                //    total = Historique.GetRange(Historique.Count - 9, 9).Sum(x => x.Montant);
                //}
                //else
                //{
                //    total = Historique.Sum(x => x.Montant);
                //}


                if (total + montant < RetraitAutorise)
                {
                    Solde -= montant;
                    return true;
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
