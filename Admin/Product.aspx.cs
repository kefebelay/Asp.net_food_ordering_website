﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace MAKH.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Product";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    getProducts();
                }

            }
            lblMsg.Visible = false;
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int productId = Convert.ToInt32(hdnId.Value);
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", productId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
            cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text.Trim());
            cmd.Parameters.AddWithValue("@CategoryId", ddlCategories.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);
            if (fuProductImage.HasFile)
            {
                if (Utils.IsValidExtension(fuProductImage.FileName))
                {
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuProductImage.FileName);
                    imagePath = "Images/Product/" + obj.ToString() + fileExtension;
                    fuProductImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "please select .jpg, .jpeg or .png image";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                isValidToExecute = true;
            }

            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    actionName = productId == 0 ? "inserted" : "updated";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Product " + actionName + " successfully!";
                    lblMsg.CssClass = "alert alert-success";
                    getProducts();
                    clear();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error- " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally { conn.Close(); }
            }
        }
        private void getProducts()
        {
            conn = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", conn);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rProduct.DataSource = dt;
            rProduct.DataBind();
        }

        public void clear()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtPrice.Text = string.Empty;
            ddlCategories.ClearSelection();
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
            imgProduct.ImageUrl = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void rProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            conn = new SqlConnection(Connection.GetConnectionString());
            if (e.CommandName == "edit")
                {
                    cmd = new SqlCommand("Product_Crud", conn);
                    cmd.Parameters.AddWithValue("@Action", "GETBYID");
                    cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                    cmd.CommandType = CommandType.StoredProcedure;
                    sda = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    sda.Fill(dt);
                    txtName.Text = dt.Rows[0]["Name"].ToString();
                    txtDescription.Text = dt.Rows[0]["Description"].ToString();
                    txtPrice.Text = dt.Rows[0]["Price"].ToString();
                    txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                    ddlCategories.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                    cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    imgProduct.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
                        "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                    imgProduct.Height = 200;
                    imgProduct.Width = 200;
                    hdnId.Value = dt.Rows[0]["ProductId"].ToString();
                    btnAddOrUpdate.Text = "Update";
                    LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                    btn.CssClass = "badge badge-warning";
                }
                else if (e.CommandName == "delete")
                {
                    cmd = new SqlCommand("Product_Crud", conn);
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        lblMsg.Visible = true;
                        lblMsg.Text = "Product deleted successfully!";
                        lblMsg.CssClass = "alert alert-success";
                        getProducts();
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Error-" + ex.Message;
                        lblMsg.CssClass = "alert alert-danger";
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
        }

        protected void rProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;
                if (lblIsActive.Text == "True")
                {
                    lblIsActive.Text = "Active";
                    lblIsActive.CssClass = "badge badge-success";
                }
                else
                {
                    lblIsActive.Text = "In-Active";
                    lblIsActive.CssClass = "badge badge-danger";
                }
                if (Convert.ToInt32(lblQuantity.Text) <= 5)
                {
                    lblQuantity.CssClass = "badge badge-danger";
                    lblQuantity.ToolTip = "Item about to be 'Out of stock'!";
                }
            }
        }
    }
}