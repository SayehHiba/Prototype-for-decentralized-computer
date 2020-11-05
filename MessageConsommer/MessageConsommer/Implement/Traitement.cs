using MessageConsommer.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace MessageConsommer.Implement
{
    public class Traitement
    {
        public static async Task<string> GérerStock(VenteItem vente)
        {
           MySqlConnection Connexion = await ConnexionSingletonBD.Connexion("localhost", "produitbase", "root", "");
            //cnx sing bd
            string query = "UPDATE `produit` SET `Stock`= Stock -" + vente.Quantite + " WHERE id =" + vente.IdProduit + " && Stock >=" + vente.Quantite;
            string reponse = "";
            //Create a list to store the result
            ProduitItem produit = new ProduitItem();

            //Open connection
            if (await AccesBD.OpenConnection(Connexion) == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, Connexion);
                //Create a data reader and Execute the command
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {

                    Debug.WriteLine("\n\n" + ex.Message + "\n\n");
                    reponse = "sql Exception : " + ex.Message;
                }

                //Read the data and store them in the list
                reponse = "Valide";
                //close Connection
                await AccesBD.CloseConnection(Connexion);

                //return list to be displayed
            }
            else
            {
                Debug.WriteLine("\n\n probleme con");
                reponse = "Probleme cnx";
            }
            Debug.WriteLine(" \n\n fin traitement \n\n");
            return reponse;

            
      
        }
    }
}
