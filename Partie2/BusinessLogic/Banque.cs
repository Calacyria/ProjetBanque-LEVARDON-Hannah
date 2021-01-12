using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie2.BusinessLogic
{
    class Banque
    {

        public Dictionary<uint, Compte> Comptes { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Dictionary<uint, List<Compte>> ComptesParGestionaire { get; set; }

        Gestionnaire gestionnaire { get; set; }

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
                if (!Comptes.ContainsKey(transaction.Emetteur))
                {
                    return false;
                }
                else
                {
                    Compte emetteur = Comptes[transaction.Emetteur];
                    if (emetteur.Retrait(transaction.Montant))
                    {
                        emetteur.Historique = new List<Transaction>();
                        emetteur.Historique.Add(transaction);
                        return true;
                    }
                    return false;
                }
            }
            else if (transaction.Emetteur == 0 && transaction.Destinataire > 0)
            {
                if (!Comptes.ContainsKey(transaction.Destinataire))
                {
                    return false;
                }
                else
                {
                    Compte destinataire = Comptes[transaction.Destinataire];
                    if (destinataire.Depot(transaction.Montant))
                    {
                        return true;
                    }
                    return false;
                }
            }
            else if (transaction.Emetteur > 0 && transaction.Destinataire > 0)
            {
                Compte destinataire = Comptes[transaction.Destinataire];
                Compte emetteur = Comptes[transaction.Emetteur];
                //Compte CompteEmetteurAtrouver = gestionnaire.Comptes.Find(x => x.Identifiant == emetteur.Identifiant);
                //Compte CompteDestinataireATrouver = gestionnaire.Comptes.Find(x => x.Identifiant == destinataire.Identifiant);


                if (gestionnaire.Comptes.Contains(emetteur) && gestionnaire.Comptes.Contains(destinataire))
                {
                    if (emetteur.Prelevement(transaction.Montant, gestionnaire, transaction) && destinataire.Virement(transaction.Montant))
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
                else
                {
                    double MontantReel = transaction.Montant - gestionnaire.CalculFraisGestion(transaction);
                    if (emetteur.Prelevement(MontantReel, gestionnaire, transaction) && destinataire.Virement(MontantReel))
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
            }
            else if (transaction.Emetteur == 0 && transaction.Destinataire == 0)
            {
                return false;
            }
            return false;

        }

        public List<string> MiseAjourCompte(List<Transaction> transactions, Dictionary<uint, Compte> comptes)
        {
            List<string> Sorties = new List<string>();
            foreach (var transaction in transactions)
            {

                if (!Sorties.Any(x => x.StartsWith(transaction.Identifiant.ToString())))
                {

                    bool validationTransaction = LectureTransaction(transaction);

                    if (validationTransaction == true)
                    {
                        Sorties.Add($"{transaction.Identifiant};OK");

                    }
                    else
                    {
                        Sorties.Add($"{transaction.Identifiant};KO");
                    }
                }
                else
                {
                    Sorties.Add($"{transaction.Identifiant};KO");
                }
            }
            return Sorties;
        }
    }
}
