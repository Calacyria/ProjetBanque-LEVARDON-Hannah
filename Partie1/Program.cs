using System;
using System.Collections.Generic;
using System.Globalization;
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
            List<Transaction> transactions = new List<Transaction>();
            BusinessLogic.Banque banque = new BusinessLogic.Banque(comptes, transactions);
            Dictionary<uint, string> Sorties = new Dictionary<uint, string>();

            using (var file = new StreamReader(path + @"\Comptes.csv"))
            {
                while (!file.EndOfStream)
                {
                    string[] comptesTab = file.ReadLine().Split(';');

                    uint idCompte = uint.Parse(comptesTab[0]);
                    //Console.WriteLine($"Numero de compte {idCompte}");
                    double solde;

                   

                    if (comptes.ContainsKey(idCompte))
                    {
                        Console.WriteLine("Compte deja existant");

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(comptesTab[1]))
                        {
                            comptesTab[1] = "0";
                            solde = double.Parse(comptesTab[1], CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            solde = double.Parse(comptesTab[1], CultureInfo.InvariantCulture);
                        }
                        if (solde >= 0)
                        {
                            Compte compte = new Compte(idCompte, solde);
                            comptes.Add(idCompte, compte);
                        }
                    }
                }
            }
            foreach (var cpt in comptes)
            {
                Console.WriteLine($"Identifiant compte : {cpt.Value.Identifiant}");
            }
            using (var file = new StreamReader(path + @"\Transactions.csv"))
            {
                while (!file.EndOfStream)
                {
                    string[] transactionTab = file.ReadLine().Split(';');

                    uint idTransaction = uint.Parse(transactionTab[0]);
                    double montant = double.Parse(transactionTab[1], CultureInfo.InvariantCulture);
                    uint idCompteEmetteur = uint.Parse(transactionTab[2]);
                    uint idCompteDestinataire = uint.Parse(transactionTab[3]);

                    Transaction txATrouver = transactions.Find(x => x.Identifiant == idTransaction);

                    if (txATrouver != null)
                    {
                        Console.WriteLine("Transaction deja existante");
                    }
                    else
                    {
                        Transaction transaction = new Transaction(idTransaction, montant, idCompteEmetteur, idCompteDestinataire);
                        transactions.Add(transaction);
                    }
                }
            }
            foreach (var txn in transactions)
            {
                Console.WriteLine(txn.Identifiant);
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
