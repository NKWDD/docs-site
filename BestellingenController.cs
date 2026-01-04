using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Proeflokaal.Models;

namespace Proeflokaal.Controller
{
    public class BestellingenController
    {
        private string conString = "Data Source=LAPTOP-889JICD3;Initial Catalog=proeflokaaldb;Integrated Security=True;TrustServerCertificate=True;";

        private List<BestellingenModel> bestelingLijst = new List<BestellingenModel>();

        public List<BestellingenModel> Read()
        {
            bestelingLijst.Clear();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = "SELECT * FROM Bestellingen";

                using(SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        BestellingenModel bestellingen = new BestellingenModel();
                        bestellingen.BestellingID = (int)reader["BestellingID"];
                        bestellingen.ReserveringsnummerID = (int)reader["ReserveringsnummerID"];
                        bestellingen.TafelNummer = (int)reader["TafelNummer"];
                        bestellingen.BestelDatum = (DateTime)reader["BestelDatum"];
                        bestellingen.Status = (string)reader["Status"];

                        bestelingLijst.Add(bestellingen);
                    }
                }
            }
            return bestelingLijst;   // Now matches the return type
        }

        public int Create(BestellingenModel bestelling)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = "INSERT INTO Bestellingen (ReserveringsnummerID, TafelNummer, BestelDatum, Status) " +
                               "VALUES (@reserveringsnummerID, @tafelNummer, @bestelDatum, @status)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@reserveringsnummerID", bestelling.ReserveringsnummerID);
                    cmd.Parameters.AddWithValue("@tafelNummer", bestelling.TafelNummer);
                    cmd.Parameters.AddWithValue("@bestelDatum", bestelling.BestelDatum);
                    cmd.Parameters.AddWithValue("@status", bestelling.Status);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }

        public int Update(BestellingenModel bestelling)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = @"UPDATE Bestellingen
                                 SET ReserveringsnummerID = @reserveringsnummerID,
                                     TafelNummer = @tafelNummer,
                                     BestelDatum = @bestelDatum,
                                     Status = @status
                                 WHERE BestellingID = @id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", bestelling.BestellingID);
                    cmd.Parameters.AddWithValue("@reserveringsnummerID", bestelling.ReserveringsnummerID);
                    cmd.Parameters.AddWithValue("@tafelNummer", bestelling.TafelNummer);
                    cmd.Parameters.AddWithValue("@bestelDatum", bestelling.BestelDatum);
                    cmd.Parameters.AddWithValue("@status", bestelling.Status);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }
        public int Delete(int bestellingID)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = "DELETE FROM Bestellingen WHERE BestellingID = @id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", bestellingID);
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }
    }
}
