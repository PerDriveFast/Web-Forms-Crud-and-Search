using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication1
{
    public partial class Users : System.Web.UI.Page
    {
        public void NewData()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtEmail.Text = "";
            txtMK.Text = "";
            txtTel.Text = "";
            lblmsg.Text = "";
            // Disable the textbox after insertion
            txtMaNV.Enabled = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void modal_Click(object sender, EventArgs e)
        {
            string script = "$('#mymodal').modal('show');";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Command
                SqlCommand cmd;

                if (!string.IsNullOrEmpty(hdid.Value))
                {
                    string id = hdid.Value;
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "pcd_UpadteUser";
                    cmd.Parameters.AddWithValue("@userid", id);
                }
                else
                {
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "pcd_SaveUsers";
                }

                // Add parameters with correct names
                cmd.Parameters.AddWithValue("@username", txtTenNV.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@password", txtMK.Text);
                cmd.Parameters.AddWithValue("@tel", txtTel.Text);

                // For the case where it's not an update, add @userid parameter
                if (string.IsNullOrEmpty(hdid.Value))
                {
                    cmd.Parameters.AddWithValue("@userid", txtMaNV.Text);
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    lblmsg.Text = "Data Inserted Successfully";
                }
                else
                {
                    lblmsg.Text = "Error While Inserting Data";
                }

                string script = @"$('#mymodal').modal('hide');";
                ClientScript.RegisterStartupScript(this.GetType(), "HideModal", script, true);

                rptr1.DataBind();
                NewData();
            }
        }



        protected void btnupdate_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            hdid.Value = id;

            string connectionstring = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("GetUserData", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userid", id);

                SqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    txtMaNV.Text = dataReader["UserID"].ToString();
                    txtMaNV.Enabled = false;
                    txtTenNV.Text = dataReader["UserName"].ToString();
                    txtEmail.Text = dataReader["Passwords"].ToString();
                    txtMK.Text = dataReader["Email"].ToString();
                    txtTel.Text = dataReader["Tel"].ToString();
                }

                dataReader.Close();
            }


            ScriptManager.RegisterStartupScript(this, GetType(), "OpenModalScript", "$('#mymodal').modal('show');", true);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            NewData();
        }

        protected void btndlt_Command(object sender, CommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();

            string connectionstring = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "pcd_DeleteUsers";
                // Correct the parameter name to match the stored procedure
                cmd.Parameters.AddWithValue("userID", id);
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }

            rptr1.DataBind();
           
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            string connectionstring = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SearchUsers"; // Replace with your stored procedure name
                cmd.Parameters.AddWithValue("@SearchKeyword", keyword);
                cmd.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptr1.DataSourceID = null; // Remove any direct assignment of DataSource
                    rptr1.DataSource = dt;
          
                    rptr1.DataBind();
                    rptr1.Visible = true; // Ensure Repeater is visible
                    lblNoData.Visible = false; // Hide the label when there is data
                }
                else
                {
                    // No data, handle it as needed
                    rptr1.Visible = false; // Optionally, hide the Repeater
                    lblNoData.Visible = true; // Show a label with a message
                }

            }
        }

        protected void rptr1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer && rptr1.Items.Count == 0)
            {
                Label lblNoData = (Label)e.Item.FindControl("lblNoData");
                if (lblNoData != null)
                {
                    lblNoData.Visible = true;
                }
            }
        }


        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}