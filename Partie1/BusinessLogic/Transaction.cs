using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie1.BusinessLogic
{

    class Transaction
    {

        public uint Identifiant { get; set; }
        public double Montant { get; set; }
        public uint Emetteur { get; set; }
        public uint Destinataire { get; set; }

        public Transaction(uint identifiant, double montant, uint emetteur, uint destinataire)
        {
            Identifiant = identifiant;
            Montant = montant;
            Emetteur = emetteur;
            Destinataire = destinataire;
        }


}
}
