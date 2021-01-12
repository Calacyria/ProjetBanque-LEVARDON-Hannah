using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie1.BusinessLogic
{
    class Banque
    {

        public Dictionary<uint, Compte> Comptes { get; set; }
        public List<Transaction> Transactions { get; set; }

        


        public Banque(Dictionary<uint, Compte> comptes, List<Transaction> transactions)
        {
            Comptes = comptes;
            Transactions = transactions;

        }

        //méthodes
        internal bool LectureTransaction(double montant, uint compteEmetteur, uint compteDestinaire)
        {
            if (compteEmetteur > 0 && compteDestinaire == 0)
            {
                Compte emetteur = Comptes[compteEmetteur];
                Console.WriteLine(emetteur.Identifiant);
              
                if (emetteur.Retrait(montant))
                {
                    return true;
                }
                return false;
            }
            else if (compteEmetteur == 0 && compteDestinaire > 0)
            {
                Compte destinataire = Comptes[compteDestinaire];

                if (destinataire.Depot(montant))
                {
                    return true;
                }
                return false;
            }
            else if (compteEmetteur > 0 && compteDestinaire > 0)
            {
                Compte destinataire = Comptes[compteDestinaire];
                Compte emetteur = Comptes[compteEmetteur];

                if (emetteur.Prelevement(montant))
                {
                    if (destinataire.Virement(montant))
                    {
                        return true;
                    }
                    return false;
                }

                return false;
            }
            else if(compteEmetteur == 0 && compteDestinaire == 0)
            {
                return false;
            }
            return false;

        }

        public Dictionary<uint, string> MiseAjourCompte(List<Transaction> transactions, Dictionary<uint, Compte> comptes)
        {
            Dictionary<uint, string> Sorties = new Dictionary<uint, string>();

            foreach (var transaction in transactions)
            {
                bool validationTransaction = LectureTransaction(transaction.Montant, transaction.Emetteur, transaction.Destinataire);

                if (validationTransaction == true)
                {
                    Sorties.Add(transaction.Identifiant, "OK");
                }
                else
                {
                    Sorties.Add(transaction.Identifiant, "KO");
                }

            }
            return Sorties;

        }

    }
}
