using System;
using System.Collections.Generic;
using System.Text;

namespace TraitementMessageConsommer.Implement
{
    class Traitement
    {
        public static async Task<string> Traitement(VenteItem vente)
        {
            //cnx sing bd

            string res = await Select(vente);

            Debug.WriteLine(" \n\n fin traitement \n\n");
            return res;
        }
    }
}
