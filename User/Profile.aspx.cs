﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace MAKH.User
{
    
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] == null) 
                {
                    Response.Redirect("Login.aspx");                   
                }
                else
                {
                    getUserDetails();
                }
            }
            void getUserDetails()
            {
                conn = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("User_Crud", conn);
                cmd.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                rUserProfile.DataSource=dt;
                rUserProfile.DataBind();
                if (dt.Rows.Count == 1)
                {
                    Session["name"] = dt.Rows[0]["Name"].ToString();
                    Session["email"] = dt.Rows[0]["Email"].ToString();
                    Session["imageUrl"] = (dt.Rows[0]["ImageUrl"].ToString());
                    Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
                }               
            }
        }
    }
}