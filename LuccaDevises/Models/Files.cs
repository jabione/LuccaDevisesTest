using LuccaDevises.IServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises.Models
{
    public class Files
    {
        /// <summary>Devise initiale</summary>
        public string Depart { get; set; }
        /// <summary>Montant de devise initial</summary>
        public int MontantInitial { get; set; }
        /// <summary>Devise cible</summary>
        public string Cible { get; set; }

        /// <summary>
        /// Liste des taux des devises sous forme de devise initial, taux et devise cible
        /// </summary>
        public List<Tuple<string, double, string>> listTauxChange { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public Files()
        {
            Depart = "";
            MontantInitial = 0;
            Cible = "";
            listTauxChange = new();
        }
    }
}
