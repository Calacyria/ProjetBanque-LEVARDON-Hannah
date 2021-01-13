using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Partie2.BusinessLogic;

namespace Partie2
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = Directory.GetCurrentDirectory();
            Dictionary<uint, Compte> comptes = new Dictionary<uint, Compte>();
            List<LigneCompte> lignesCompte = new List<LigneCompte>();
            List<Transaction> transactions = new List<Transaction>();
            List<Gestionnaire> gestionnaires = new List<Gestionnaire>();
            Banque banque = new Banque(lignesCompte, comptes, transactions, gestionnaires);
            //List<string> StatutsOperation = new List<string>();

            LectureTransaction(path, transactions);
            foreach (var txn in transactions)
            {
                Console.WriteLine($"Identifiant de transaction : {txn.Identifiant}");
                Console.WriteLine($"Date de transaction : {txn.DateEffet}");
                Console.WriteLine($"Montant de transaction : {txn.Montant}");
                Console.WriteLine($"Comptes emmeteur et destinataire de transaction : {txn.Emetteur} et {txn.Destinataire}");
            }

            LectureGestionnaire(path, gestionnaires);
            //foreach (var gest in gestionnaires)
            //{
            //    Console.WriteLine($"Identifiant Gestionnaire : {gest.Identifiant}");
            //    Console.WriteLine($"Type Gestionnaire : {gest.Type}");
            //    Console.WriteLine($"Nb txn max Gestionnaire : {gest.nombreTransactionMax}");
            //}
            LectureComptes(path, lignesCompte, gestionnaires);
            //foreach (var cpt in comptes)
            //{
            //    Console.WriteLine($"Identifiant compte : {cpt.Identifiant}");
            //    Console.WriteLine($"Solde compte : {cpt.Solde}");
            //    Console.WriteLine($"Date Creation compte : {cpt.DateCreation}");
            //    Console.WriteLine($"Date Resiliation compte : {cpt.DateResiliation}");
            //    Console.WriteLine($"Date Transfert compte : {cpt.DateTransfert}");
            //}
            LectureOperation(path, lignesCompte, gestionnaires, banque, comptes, transactions);
            //Sorties = banque.MiseAjourCompte(transactions, comptes);
            //EcrireSorties(path, Sorties);

            Console.ReadLine();
        }

        private static void LectureGestionnaire(string path, List<Gestionnaire> gestionnaires)
        {
            using (var file = new StreamReader(path + @"\Gestionnaires_1.csv"))
            {
                while (!file.EndOfStream)
                {
                    string[] tableauGestionnaire = file.ReadLine().Split(';');

                    uint idGestionnaire = uint.Parse(tableauGestionnaire[0]);
                    string typeGestionnaire = tableauGestionnaire[1];
                    uint nbTransactionMax = uint.Parse(tableauGestionnaire[2]);

                    Gestionnaire gestionnaire = new Gestionnaire(idGestionnaire, typeGestionnaire, nbTransactionMax);
                    gestionnaire.FraisGestion = gestionnaire.CalculFraisGestion(gestionnaire.Type);
                    gestionnaires.Add(gestionnaire);
                }
            }
        }

        private static void LectureTransaction(string path, List<Transaction> transactions)
        {
            using (var fileTxn = new StreamReader(path + @"\Transactions_1.csv"))
            {
                while (!fileTxn.EndOfStream)
                {
                    string[] transactionTab = fileTxn.ReadLine().Split(';');

                    uint idTransaction = uint.Parse(transactionTab[0]);
                    DateTime dateEffet = DateTime.Parse(transactionTab[1]);
                    double montant = double.Parse(transactionTab[2], CultureInfo.InvariantCulture);
                    uint idCompteEmetteur = uint.Parse(transactionTab[3]);
                    uint idCompteDestinataire = uint.Parse(transactionTab[4]);

                    Transaction txATrouver = transactions.Find(x => x.Identifiant == idTransaction);

                    Transaction transaction = new Transaction(idTransaction, dateEffet, montant, idCompteEmetteur, idCompteDestinataire);
                    transactions.Add(transaction);
                }
            }
        }

        private static void LectureComptes(string path, List<LigneCompte> lignesCompte, List<Gestionnaire> gestionnaires)
        {
            using (var fileCompte = new StreamReader(path + @"\Comptes_1.csv"))
            {
                while (!fileCompte.EndOfStream)
                {
                    string[] comptesTab = fileCompte.ReadLine().Split(';');

                    uint idCompte = uint.Parse(comptesTab[0]);
                    DateTime date = DateTime.Parse(comptesTab[1]);
                    //double solde;
                    double solde = string.IsNullOrEmpty(comptesTab[2]) ? 0 : double.Parse(comptesTab[2]);
                    uint idGestionnaireEntree = string.IsNullOrEmpty(comptesTab[3]) ? 0 : uint.Parse(comptesTab[3]);
                    uint idGestionnaireSortie = string.IsNullOrEmpty(comptesTab[4]) ? 0 : uint.Parse(comptesTab[4]);

                    LigneCompte ligneCompte = new LigneCompte(idCompte, date, solde, idGestionnaireEntree, idGestionnaireSortie);

                    lignesCompte.Add(ligneCompte);
                }
            }
        }

        private  static void LectureOperation(string path, List<LigneCompte> LignesComptes, List<Gestionnaire> gestionnaires, Banque banque, Dictionary<uint, Compte> comptes, List<Transaction> transactions)
        {
            List<string> StatutsOperation = new List<string>();
            foreach (var ligne in LignesComptes)
            {
                //bool VerifOperation = banque.LectureOperation(ligne, gestionnaires, comptes);
                //if (VerifOperation == true)
                //{
                //    StatutsOperation.Add($"{ligne.Identifiant};OK");
                //}
                //else
                //{
                //    StatutsOperation.Add($"{ligne.Identifiant};KO");
                //}
                string statut = banque.LectureOperation(ligne, gestionnaires, comptes) ? "OK" : "KO";
                StatutsOperation.Add($"{ligne.Identifiant};{statut}");

                
                List<Transaction> txnDate = transactions.FindAll(x => x.DateEffet < ligne.Date);
                int index = txnDate.Count();

                while ( index != 0 )
                {
                    //.....BLA BLA BLA

                }
               

            }
        }
        private static void EcrireSorties(string path, List<string> Sorties)
        {

            using (StreamWriter file = new StreamWriter(path + @"\Sorties.csv", true))
            {
                foreach (var item in Sorties)
                {
                    file.WriteLine($"{item}");
                }
            }//end using
        }
    }
}
