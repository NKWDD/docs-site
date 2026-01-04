using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Proeflokaal.Models;

namespace Proeflokaal.Controller
{
    public class ReserveringenController
    {
        private string conString = "Data Source=LAPTOP-889JICD3;Initial Catalog=proeflokaaldb;Integrated Security=True;TrustServerCertificate=True;";

        private List<ReserveringenModel> reserveringLijst = new List<ReserveringenModel>();

        public List<ReserveringenModel> Read()
        {
            reserveringLijst.Clear();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = "SELECT * FROM Reserveringen";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ReserveringenModel reservering = new ReserveringenModel();
                        reservering.ReserveringsnummerID = (int)reader["ReserveringsnummerID"];
                        reservering.AantalPersonen = (int)reader["AantalPersonen"];
                        reservering.Status = (string)reader["Status"];
                        reservering.KlantID = (int)reader["KlantID"];
                        reservering.Datum = (DateTime)reader["Datum"];
                        reservering.Tijdslot = (TimeSpan)reader["Tijdslot"];
                        reserveringLijst.Add(reservering);
                    }
                }
            }
            return reserveringLijst;
        }
         public int Create(ReserveringenModel reserveringen)
         {
            int result = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = "INSERT INTO Reserveringen (AantalPersonen, Status, Datum, Tijdslot, KlantID) " +
                                  "VALUES (@aantalpersonen, @status, @datum, @tijdslot, @klantid)";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@aantalpersonen", reserveringen.AantalPersonen);
                    command.Parameters.AddWithValue("@status", reserveringen.Status);
                    command.Parameters.AddWithValue("@datum", reserveringen.Datum);
                    command.Parameters.AddWithValue("@tijdslot", reserveringen.Tijdslot);
                    command.Parameters.AddWithValue("@klantid", reserveringen.KlantID); // belangrijk

                    con.Open();
                    result = command.ExecuteNonQuery();
                }
            }
            return result;
          }


        public int Update(int reseveringnummerid, int aantalpersonen, string status, DateTime datum, TimeSpan tijdslot, int klantId)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                string query = @"UPDATE Reserveringen
                         SET AantalPersonen = @aantalPersonen,
                             Status = @status,
                             Datum = @datum,
                             Tijdslot = @tijdslot,
                             KlantID = @klantId
                         WHERE ReserveringsnummerID = @id";


                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@id", reseveringnummerid);
                    command.Parameters.AddWithValue("@aantalPersonen", aantalpersonen);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@datum", datum);
                    command.Parameters.AddWithValue("@tijdslot", tijdslot);
                    command.Parameters.AddWithValue("@klantId", klantId);

                    con.Open();
                    result = command.ExecuteNonQuery();
                }
            }
            return result ;

        }
        

        public int Delete(int Reserveringsnummerid)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string sql = "DELETE FROM Reserveringen WHERE ReserveringsnummerID = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", Reserveringsnummerid);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
