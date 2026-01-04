using System.Data.SqlClient;
using Proeflokaal.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Proeflokaal.Controller
{
    public class MenuController
    {
        private string conString = @"Data Source=localhost;Initial Catalog=proeflokaaldb;Integrated Security=True;TrustServerCertificate=True;";

        private List<MenuItemModel> menuItemLijst = new List<MenuItemModel>();

        public List<MenuItemModel> Read(string category, string subCategory)
        {
            menuItemLijst.Clear();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = @"SELECT Id, MenuItem, Category, SubCategory 
                                    FROM Menu 
                                    WHERE Category = @category 
                                    AND SubCategory = @subCategory";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@category", category);
                    command.Parameters.AddWithValue("@subCategory", subCategory);

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        MenuItemModel item = new MenuItemModel();
                        item.Id = (int)reader["Id"];
                        item.MenuItem = (string)reader["MenuItem"];
                        item.Category = (string)reader["Category"];
                        item.SubCategory = (string)reader["SubCategory"];

                        menuItemLijst.Add(item);
                    }
                }
            }
            return menuItemLijst;
        }

        public int Create(MenuItemModel menuItem)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sqlQuery = "INSERT INTO Menu (MenuItem, Category, SubCategory) " +
                                  "VALUES (@menuItem, @category, @subCategory)";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@menuItem", menuItem.MenuItem);
                    command.Parameters.AddWithValue("@category", menuItem.Category);
                    command.Parameters.AddWithValue("@subCategory", menuItem.SubCategory);

                    con.Open();
                    result = command.ExecuteNonQuery();
                }
            }

            return result;
        }

        public int Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();

                string sql = "DELETE FROM Menu WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sql = "SELECT DISTINCT Category FROM Menu";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        categories.Add(reader["Category"].ToString());
                    }
                }
            }

            return categories;
        }

        public List<string> GetSubCategories(string category)
        {
            List<string> subCategories = new List<string>();

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sql = "SELECT DISTINCT SubCategory FROM Menu WHERE Category = @category";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@category", category);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        subCategories.Add(reader["SubCategory"].ToString());
                    }
                }
            }

            return subCategories;
        }

        public int Update(int id, string menuItem, string category, string subCategory)
        {
            int result = 0;

            using (SqlConnection con = new SqlConnection(conString))
            {
                string sql = @"UPDATE Menu
                       SET MenuItem = @menuItem,
                           Category = @category,
                           SubCategory = @subCategory
                       WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@menuItem", menuItem);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@subCategory", subCategory);

                    con.Open();
                    result = cmd.ExecuteNonQuery();
                }
            }
            return result;
        }


    }
}
