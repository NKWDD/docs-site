using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Proeflokaal.Models;

namespace Proeflokaal.Controller
{
    public class KlantenController
    {
        private string conString = "Data Source=LAPTOP-889JICD3;Initial Catalog=proeflokaaldb;Integrated Security=True;TrustServerCertificate=True;";


        private List<KlantenModel> klantenLijst = new List<KlantenModel>();
        public List<KlantenModel> Read()
        {
            klantenLijst.Clear();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = "SELECT * FROM Klant";
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        KlantenModel klanten = new KlantenModel();
                        klanten.KlantID = (int)reader["KlantID"];
                        klanten.Voornaam = (string)reader["Voornaam"];
                        klanten.Achternaam = (string)reader["Achternaam"];
                        klanten.Email = (string)reader["Email"];
                        klanten.Telefoonnummer = (string)reader["Telefoonnummer"];
                        klantenLijst.Add(klanten);
                        
                    }
                }
            }

            return klantenLijst;
        }
        public int Create(KlantenModel klanten)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {

                string sqlQuery = "INSERT INTO Klant (Voornaam, Achternaam, Email, Telefoonnummer) VALUES (@voornaam, @achternaam, @email, @telefoonnummer)";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@voornaam", klanten.Voornaam);
                    command.Parameters.AddWithValue("@achternaam", klanten.Achternaam);
                    command.Parameters.AddWithValue("@email", klanten.Email);
                    command.Parameters.AddWithValue("@telefoonnummer", klanten.Telefoonnummer);

                    con.Open();
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
        }
        public int Delete(int klantId)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string sql = "DELETE FROM Klant WHERE KlantID = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", klantId);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int UpdateKlant(int klantId, string voornaam, string achternaam, string email, string telefoonnummer)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = @"UPDATE Klant 
                     SET Voornaam = @voornaam, 
                         Achternaam = @achternaam, 
                         Email = @email, 
                         Telefoonnummer = @telefoonnummer 
                     WHERE KlantID = @id";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@id", klantId);
                    command.Parameters.AddWithValue("@voornaam", voornaam);
                    command.Parameters.AddWithValue("@achternaam", achternaam);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@telefoonnummer", telefoonnummer);

                    con.Open();
                    result = command.ExecuteNonQuery();
                }
            }
            return result; 
        }
        
    }
}
