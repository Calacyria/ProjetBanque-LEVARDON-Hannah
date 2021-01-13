using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie2.BusinessLogic
{

    public class Transaction
    {

        public uint Identifiant { get; set; }
        public DateTime DateEffet { get; set; }
        public double Montant { get; set; }
        public uint Emetteur { get; set; }
        public uint Destinataire { get; set; }
        


        public Transaction(uint identifiant, DateTime dateEffet, double montant, uint emetteur, uint destinataire)
        {
            Identifiant = identifiant;
            DateEffet = dateEffet;
            Montant = montant;
            Emetteur = emetteur;
            Destinataire = destinataire;
        }


}
}
