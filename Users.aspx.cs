using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WebApplication1
{
    public partial class Users : System.Web.UI.Page
    {

        #region NewData
        public void NewData()
        {
            txtMaNV.Text = "";
            txtTenNV.Text = "";
            txtEmail.Text = "";
            txtMK.Text = "";
            txtTel.Text = "";
            lblmsg.Text = "";
            txtMaNV.Enabled = true;
        }
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
            DataTable dt = GetDataFromDatabase();
        }
        #endregion

        #region BindData
        private void BindData()
        {
            DataTable dt = GetDataFromDatabase();
            ListView1.DataSource = dt;
            ListView1.DataBind();
        }
        #endregion

        #region GetDataFromDatabase
        private DataTable GetDataFromDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("GetAllUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                return dt;

            }
        }
        #endregion

        #region modal_Click
        protected void modal_Click(object sender, EventArgs e)
        {
            string script = "$('#mymodal').modal('show');";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        }
        #endregion


        #region ListView1_ItemUpdating
        protected void ListView1_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {

                string updatedUserName = e.NewValues["UserName"] as string;
                string updatedEmail = e.NewValues["Email"] as string;
                string updatedPassword = e.NewValues["Passwords"] as string;
                string updatedTel = e.NewValues["Tel"] as string;


                string userId = e.Keys["UserID"].ToString();


                UpdateItemInDatabase(userId, updatedUserName, updatedEmail, updatedPassword, updatedTel);

                // Cancel the update operation
                e.Cancel = true;

                // Exit edit mode after updating
                ListView1.EditIndex = -1;

                // Rebind the data to the ListView
                BindData();
            }
            catch (Exception ex)
            {
                // Handle the exception (log or display an error message)
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region UpdateItemInDatabase
        private void UpdateItemInDatabase(string userId, string updatedUserName, string updatedEmail, string updatedPassword, string updatedTel)
        {
            try
            {
                // Replace the following code with your actual database update logic
                string connectionString = ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Users SET UserName=@UserName, Email=@Email, Passwords=@Passwords, Tel=@Tel WHERE UserID=@UserID", conn);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@UserName", updatedUserName);
                    cmd.Parameters.AddWithValue("@Email", updatedEmail);
                    cmd.Parameters.AddWithValue("@Passwords", updatedPassword);
                    cmd.Parameters.AddWithValue("@Tel", updatedTel);

                    cmd.ExecuteNonQuery();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log or display an error message)
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception to notify the calling code
            }
        }
        #endregion

        #region rptr1_ItemDataBound
        protected void rptr1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Footer && ListView1.Items.Count == 0)
                {
                    Label lblNoData = (Label)e.Item.FindControl("lblNoData");
                    if (lblNoData != null)
                    {
                        lblNoData.Visible = true;
                    }
                    BindData();

                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }
        }
        #endregion


        #region btnCreate_Click
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
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

                    BindData();

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
        }
        #endregion
        #region btnupdate_Command
        protected void btnupdate_Command(object sender, CommandEventArgs e)
        {
            try
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
                    BindData();
                }


                ScriptManager.RegisterStartupScript(this, GetType(), "OpenModalScript", "$('#mymodal').modal('show');", true);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }
        }
        #endregion
        #region btndlt_Command
        protected void btndlt_Command(object sender, CommandEventArgs e)
        {
            try
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
                BindData();

            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString()); }

        }
        #endregion
        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
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
                        ListView1.DataSourceID = null; // Remove any direct assignment of DataSource
                        ListView1.DataSource = dt;

                        ListView1.DataBind();
                        ListView1.Visible = true; // Ensure Repeater is visible
                        lblNoData.Visible = false; // Hide the label when there is data
                    }
                    else
                    {
                        // No data, handle it as needed
                        ListView1.Visible = false; // Optionally, hide the Repeater
                        lblNoData.Visible = true; // Show a label with a message
                    }

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message.ToString());
            }
        }
        #endregion


        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            NewData();
        }
        protected void DataPager1_PreRender(object sender, EventArgs e)
        {
            BindData();
        }
        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindData();
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }
        // Set Count File Downloading
        private int j
        {
            get { return (int)(Session["JCount"] ?? 0); }
            set { Session["JCount"] = value; }
        }
        private void ExportToExcel()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllUsers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.HasRows)
                    {
                        ListView1.DataSource = rdr;
                        

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            DataTable dt = new DataTable();
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                dt.Columns.Add(rdr.GetName(i));
                            }

                            while (rdr.Read())
                            {
                                DataRow row = dt.NewRow();
                                for (int i = 0; i < rdr.FieldCount; i++)
                                {
                                    row[i] = rdr[i];
                                }
                                dt.Rows.Add(row);
                            }

                            wb.Worksheets.Add(dt, "Users");
                           
                            j++;
                            // Save the Excel file
                            string fileName = Path.Combine(Server.MapPath("~/ExportedFiles"), $"Users_{j}.xlsx");
                            wb.SaveAs(fileName);

                            // Provide the file for download
                            Response.Clear();
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", $"attachment;filename={Path.GetFileName(fileName)}");
                            Response.TransmitFile(fileName);
                            Response.End();
                        }
                    }
                    else
                    {
                        lblNoData.Visible = true;
                    }
                }
            }
        }
        }
    }