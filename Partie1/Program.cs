using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Partie1.BusinessLogic;

namespace Partie1
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = Directory.GetCurrentDirectory();
            Dictionary<uint, BusinessLogic.Compte> comptes = new Dictionary<uint, BusinessLogic.Compte>();
            List<BusinessLogic.Transaction> transactions = new List<BusinessLogic.Transaction>();
            BusinessLogic.Banque banque = new BusinessLogic.Banque(comptes, transactions);
            Dictionary<uint, string> Sorties = new Dictionary<uint, string>();

            using (var file = new StreamReader(path + @"\Comptes.csv"))
            {


                while (!file.EndOfStream)
                {
                    string[] comptesTab = file.ReadLine().Split(';');

                    uint idCompte = uint.Parse(comptesTab[0]);
                    double solde = double.Parse(comptesTab[1]);

                    Console.WriteLine($"Numéro de Compte : {idCompte} ");

                    BusinessLogic.Compte compte = new BusinessLogic.Compte(idCompte, solde);

                    comptes.Add(idCompte, compte);

                }


            }
            using (var file = new StreamReader(path + @"\Transactions.csv"))
            {


                while (!file.EndOfStream)
                {

                    string[] transactionTab = file.ReadLine().Split(';');

                    uint idTransaction = uint.Parse(transactionTab[0]);
                    double montant = double.Parse(transactionTab[1]);
                    uint idCompteEmetteur = uint.Parse(transactionTab[2]);
                    uint idCompteDestinataire = uint.Parse(transactionTab[3]);

                    Console.WriteLine($"Numéro de transaction : {idTransaction} ");

                    Transaction transaction = new Transaction(idTransaction, montant, idCompteEmetteur, idCompteDestinataire);
                    transactions.Add(transaction);

                }

            }
            foreach(var txn in transactions)
            {
                Console.WriteLine(txn.Montant);
            }

            Sorties = banque.MiseAjourCompte(transactions, comptes);

            using (StreamWriter file = new StreamWriter(path + @"\Sorties.csv", true))
            {
                foreach (KeyValuePair<uint, string> item in Sorties)
                {

                    file.WriteLine($"{item.Key};{item.Value}");
                }
            }//end using


            Console.ReadLine();
        }
    }
}
