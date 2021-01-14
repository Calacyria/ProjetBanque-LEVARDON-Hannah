using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie2.BusinessLogic
{
    class Banque
    {


        public List<LigneCompte> LignesCompte { get; set; }

        public Dictionary<uint, Compte> Comptes { get; set; }
        public List<Transaction> Transactions { get; set; }

        //public Dictionary<uint, List<Compte>> ComptesParGestionaire { get; set; }

        List<Gestionnaire> Gestionnaires { get; set; }

        public Banque(List<LigneCompte> lignesCompte, Dictionary<uint, Compte> comptes, List<Transaction> transactions, List<Gestionnaire> gestionnaire)
        {
            LignesCompte = lignesCompte;
            Comptes = comptes;
            Transactions = transactions;
            Gestionnaires = gestionnaire;
        }

        internal bool LectureOperation(LigneCompte operation, List<Gestionnaire> gestionnaires, Dictionary<uint, Compte> comptes)
        {
            // Si le dico compte ne contient pas la cle du compte : on lance l'operation creation
            if (!Comptes.ContainsKey(operation.Identifiant))
            {
                if (operation.Entree > 0 && operation.Sortie == 0)
                {
                    // Déplacement de la ligne de compte vers un compte
                    Compte compteACreer = new Compte(operation.Identifiant, operation.Solde, operation.Date, new DateTime(), new DateTime());

                    //Ajout du compte dans la dictionnaire de comptes
                    Comptes.Add(compteACreer.Identifiant, compteACreer);

                    // Recherche du gestionnaire en entrée pour ajouter le compte à sa liste
                    Gestionnaire gestionnaireCreation = gestionnaires.Find(x => x.Identifiant == operation.Entree);
                    if (gestionnaireCreation.CreationCompte(compteACreer))
                    {
                        return true;
                    }
                    return false;

                }
            }
            // Si le dico de compte contient la clé : on lance les opération résiliation, cession ou reception
            else
            {
                // Cas : RESILIATION
                if (operation.Entree == 0 && operation.Sortie > 0)
                {
                    // Cas : Si le gestionnaire de Sortie Existe et que le compte lui appartient
                    Gestionnaire gestionnaireResiliation = gestionnaires.Find(x => x.Identifiant == operation.Sortie) != null ? gestionnaires.Find(x => x.Identifiant == operation.Sortie) : new Gestionnaire();
                    Compte compteAResilier = gestionnaireResiliation.Comptes.Find(x => x.Identifiant == operation.Identifiant);

                    if (gestionnaireResiliation != new Gestionnaire() && compteAResilier != null)
                    {
                        // Ajout de la datte de résiliation au compte et implémentation du dictionnaire
                        compteAResilier.DateResiliation = operation.Date;
                        Comptes.Remove(compteAResilier.Identifiant);
                        Comptes.Add(compteAResilier.Identifiant, compteAResilier);


                        // Résiliation via gestionnaire pour le supprimer de sa liste de comptes
                        return gestionnaireResiliation.ResiliationCompte(compteAResilier) == true ? true : false;
                    }
                    else if (gestionnaireResiliation != new Gestionnaire())
                    {
                        return false;
                    }
                    return false;
                }
                // Cas : Echange de compte
                else if (operation.Entree > 0 && operation.Sortie > 0)
                {
                    // Verification des gestionnaire de sortie et d'entée
                    Gestionnaire gestionnaireEntree = gestionnaires.Find(x => x.Identifiant == operation.Sortie) != null ? gestionnaires.Find(x => x.Identifiant == operation.Entree) : new Gestionnaire();
                    Gestionnaire gestionnaireSortie = gestionnaires.Find(x => x.Identifiant == operation.Sortie) != null ? gestionnaires.Find(x => x.Identifiant == operation.Sortie) : new Gestionnaire();
                    Compte compteAEchanger = gestionnaireEntree.Comptes.Find(x => x.Identifiant == operation.Identifiant);

                    if (gestionnaireEntree != null && gestionnaireSortie != null && compteAEchanger != null)
                    {
                        // Appel des méthode de cession et reception de compte et verification 
                        bool verifCession = gestionnaireEntree.CessionCompte(compteAEchanger, gestionnaireEntree, gestionnaireSortie);
                        bool verifReception = gestionnaireSortie.ReceptionCompte(compteAEchanger, gestionnaireEntree, gestionnaireSortie);

                        if (verifCession && verifReception)
                        {
                            compteAEchanger.DateTransfert = operation.Date;
                            return true;
                        }

                        return verifCession == true && verifReception == true;
                    }
                    return false;
                }
            }

            return false;
        }

        //méthodes
        internal bool LectureTransaction(Transaction transaction, Gestionnaire gestionnaire)
        {

            if (transaction.Emetteur > 0 && transaction.Destinataire == 0)
            {
                //if (!Comptes.ContainsKey(transaction.Emetteur))
                if (gestionnaire == null)
                {
                    return false;
                }
                else
                {
                    Compte emetteur = gestionnaire.Comptes.Single(x => x.Identifiant == transaction.Emetteur);
                    gestionnaire.CalculFraisGestion(gestionnaire.Type);
                    if (emetteur.Retrait(transaction.Montant))
                    {
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
                Compte emetteur = Comptes.ContainsKey(transaction.Emetteur) ? Comptes[transaction.Emetteur] : null;
                Compte destinataire = Comptes.ContainsKey(transaction.Destinataire) ? Comptes[transaction.Destinataire] : null;

                if (emetteur == null)
                {
                    return false;
                }

                if (destinataire == null)
                {
                    return false;
                }

                if (gestionnaire.Comptes.Contains(emetteur) && gestionnaire.Comptes.Contains(destinataire))
                {
                    if (emetteur.Prelevement(transaction.Montant, gestionnaire.CalculFraisGestion(gestionnaire.Type)) && destinataire.Virement(transaction.Montant))
                    {
                        if (emetteur.Historique is null)
                        {
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
                    if (emetteur.Prelevement(transaction.Montant, gestionnaire.CalculFraisGestion(gestionnaire.Type)) && destinataire.Virement(transaction.Montant))
                    {
                        if (emetteur.Historique is null)
                        {
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

        public List<string> MiseAjourCompte(List<Transaction> transactions, Dictionary<uint, Compte> comptes, Gestionnaire gestionnaire)
        {
            List<string> Sorties = new List<string>();
            foreach (var transaction in transactions)
            {

                if (!Sorties.Any(x => x.StartsWith(transaction.Identifiant.ToString())))
                {

                    bool validationTransaction = LectureTransaction(transaction, gestionnaire);

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
