﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="MAKH.Admin.ContactUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script>

     window.onload = function () {
         var seconds = 5;
         setTimeout(function () {
             document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
         }, seconds * 1000);
     };
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pcoded-inner-content pt-0">
        <div class="align-align-self-end">
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>
        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <div class="col-sm-12 ">
                            <div class="card p-4">
                                <div class="card-header">
                                    <div class="row flex">

                                        <div class="col-12 mobile-inputs">
                                            <h4 class="sub-title">Contact Lists</h4>
                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">

                                                    <asp:Repeater ID="rContactUs" runat="server" OnItemCommand="rContactUs_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap  text-white-50">
                                                                <thead>
                                                                    <tr class="text-red">
                                                                        <th class="table-plus">SrNo</th>
                                                                        <th>User Name</th>
                                                                        <th>Email</th>
                                                                        <th>Subject</th>
                                                                        <th>Message</th>
                                                                        <th>Contact Date</th>
                                                                        <th class="datatable-nosort">Delete</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%# Eval("SrNo") %> </td>
                                                                <td><%# Eval("Name") %>   </td>
                                                                <td><%# Eval("Email") %>   </td>
                                                                <td><%# Eval("Subject") %>   </td>
                                                                <td><%# Eval("Message") %>   </td>
                                                                <td><%# Eval("CratedDate") %> </td>
                                                                <td>                                                          

                                                                    <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandName="delete"
                                                                        CssClass="badge bg-danger" CommandArgument='<%# Eval("ContactId") %> '
                                                                        OnClientClick="return confirm('Do you want to delete this Record?');">
                                                                    <i class="ti-trash"></i>
                                                                    </asp:LinkButton>

                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                    </table>
                                                        </FooterTemplate>

                                                    </asp:Repeater>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    

</asp:Content>
