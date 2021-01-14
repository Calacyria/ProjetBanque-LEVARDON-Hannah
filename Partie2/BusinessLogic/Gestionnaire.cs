using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Partie2.BusinessLogic;

namespace Partie2.BusinessLogic
{
    class Gestionnaire
    {
        // Propriétés
        public uint Identifiant { get; set; }
        public string Type { get; set; }
        public uint nombreTransactionMax { get; set; }
        public double FraisGestion { get; set; }
        public List<Compte> Comptes { get; set; }

        public const string Particulier = "Particulier";
        public const string Entreprise = "Entreprise";




        // Constructeur

        public Gestionnaire(uint identifiant, string type, uint nombreTransactionMax, double fraisGestion, List<Compte> comptes)
        {
            Identifiant = identifiant;
            Type = type;
            this.nombreTransactionMax = nombreTransactionMax;
            FraisGestion = fraisGestion;
            Comptes = new List<Compte>();
        }
       
        public Gestionnaire(uint identifiant, string type, uint nombreTransactionMax )
        {
            Identifiant = identifiant;
            Type = type;
            this.nombreTransactionMax = nombreTransactionMax;
            Comptes = new List<Compte>();

        }

        public Gestionnaire(uint identifiant, string type, uint nombreTransactionMax, double fraisGestion) : this(identifiant, type, nombreTransactionMax)
        {
            FraisGestion = fraisGestion;
        }

        public Gestionnaire()
        {

        }


        // Méthodes
        public double CalculFraisGestion(string type)
        {
          
            if (type == Particulier)
            {
                
              
                //return transaction.Montant * 1 / 100;
                return 1 / 100;
            }
            else if (type == Entreprise)
            {
                return 10;
            }

            return 0;
        }//end FraisGestion

        public bool CreationCompte(Compte compte)
        {
            if (compte.DateCreation != null)
            {
                Comptes.Add(compte);
                return true;
            }
            return false;
        }

        public bool ResiliationCompte(Compte compte)
        {
            Comptes.Remove(compte);
            return false;
        }

        public bool CessionCompte(Compte compte, Gestionnaire gestionnaireEmetteur, Gestionnaire gestionnaireDestinataire)
        {
            if (gestionnaireEmetteur.Comptes.Contains(compte) && gestionnaireDestinataire != null)
            {
                gestionnaireEmetteur.Comptes.Remove(compte);
                return true;
            }

            return false;
        }

        public bool ReceptionCompte(Compte compte, Gestionnaire gestionnaireEmetteur, Gestionnaire gestionnaireDestinataire)
        {
            if (compte != null && gestionnaireEmetteur != null)
            {
                gestionnaireDestinataire.Comptes.Add(compte);
                return true; 
            }
            
            return false;
        }


    }
}
