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
        internal bool LectureTransaction(Transaction transaction)
        {

            if (transaction.Emetteur > 0 && transaction.Destinataire == 0)
            {

                Compte emetteur = Comptes[transaction.Emetteur];
                Console.WriteLine(emetteur.Identifiant);

                if (emetteur.Retrait(transaction.Montant))
                {
                    emetteur.Historique.Add(transaction);
                    return true;
                }
                return false;
            }
            else if (transaction.Emetteur == 0 && transaction.Destinataire > 0)
            {
                Compte destinataire = Comptes[transaction.Destinataire];

                if (destinataire.Depot(transaction.Montant))
                {
                    return true;
                }
                return false;
            }
            else if (transaction.Emetteur > 0 && transaction.Destinataire > 0)
            {
                Compte destinataire = Comptes[transaction.Destinataire];
                Compte emetteur = Comptes[transaction.Emetteur];

                if (emetteur.Prelevement(transaction.Montant))
                {

                    if (destinataire.Virement(transaction.Montant))
                    {
                        if (emetteur.Historique is null)
                        {
                            emetteur.Historique = new List<Transaction>();
                            emetteur.Historique.Add(transaction);
                            return true;
                        }
                        else
                        {
                            emetteur.Historique.Add(transaction);
                            return true;
                        }
                    }
                    return false;
                }

                return false;
            }
            else if (transaction.Emetteur == 0 && transaction.Destinataire == 0)
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
                bool validationTransaction = LectureTransaction(transaction);

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
