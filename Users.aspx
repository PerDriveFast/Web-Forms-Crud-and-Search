<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="WebApplication1.Users" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        table {
            border-collapse: collapse;
            width: 100%;
        }

        th,
        td {
            text-align: left;
            padding: 8px;
        }

        th {
            background-color: #4CAF50;
            color: white;
        }

        tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        tr.separator {
            border-top: 1px solid #ddd;
            border-bottom: 1px solid;
        }
    </style>

    <div class="container">
        <div class="card-header">
      <br />
   

</div>

        <div class="modal fade" id="mymodal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" data-backdrop="false">
            <div class="modal-dialog modal-dailog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Add User New</h4>
                        <asp:Label ID="lblmsg" Text="" runat="server" />
                        <button type="button" class="close" data-dismiss="modal" >&times;</button>
                    </div>
                    <div class="modal-body">
                        <formview>
                            <div class="form-group">
                                <label>Mã nhân viên</label>
                                <asp:TextBox ID="txtMaNV" CssClass="form-control" placeholder="Mã nhân viên" runat="server" />
                            </div>
                            <div class="form-group">
                                <label>Tên nhân viên</label>
                                <asp:TextBox ID="txtTenNV" CssClass="form-control" placeholder="Tên nhân viên" runat="server" />
                            </div>
                            <div class="form-group">
                                <label>Mật khẩu nhân viên</label>
                                <asp:TextBox ID="txtMK" CssClass="form-control" placeholder="Mật khẩu nhân viên" runat="server" />
                            </div>
                            <div class="form-group">
                                <label>Email nhân viên</label>
                                <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email nhân viên" runat="server" />
                            </div>
                            <div class="form-group">
                                <label>Số điện thoại nhân viên</label>
                                <asp:TextBox ID="txtTel" CssClass="form-control" placeholder="Số điện thoại nhân viên" runat="server" />
                            </div>
                     <asp:HiddenField  ID="hdid"  runat="server"/>

                        </formview>
                    </div>
                    <div class="modal-footer">
                        <asp:Button style="float: left" ID="btnCreate" Text="Save" class="btn btn-primary" OnClick="btnCreate_Click" runat="server" />
                        <asp:Button style="float: left" ID="btnClose" Text="Close" class="btn btn-danger"  OnClick="btnClose_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
 
    <section Id="section">
        <div class="row match-height">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <asp:Button Text="Add User" ID="modal" CssClass="btn btn-primary" OnClick="modal_Click" runat="server" />
                       <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter keyword"></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            <div class="col-md-3 text-md-left">
                        <asp:DropDownList ID="ddlExportFormat" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Excel" Value="Excel" />
                            <asp:ListItem Text="PDF" Value="PDF" />
                            <asp:ListItem Text="Word" Value="Word" />
                        </asp:DropDownList>
                        <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExportToExcel_Click" CssClass="btn btn-success ml-2" />
                    </div>
                    </div>
                    
               

                    <div class="card-content">
                        <div class="card-body">
                            <div class="col-md-12 col-12">
                                <table class="table">
                                   <asp:ListView ID="ListView1" runat="server" DataKeyNames="UserID" OnItemDataBound="ListView1_ItemDataBound" OnPagePropertiesChanging="ListView1_PagePropertiesChanging" OnItemUpdating="ListView1_ItemUpdating">
    <LayoutTemplate>
        <table class="table">
            <thead>
                <tr>
                    <th>Mã nhân viên</th>
                    <th>Tên nhân viên</th>
                    <th>Email nhân viên</th>
                    <th>Mật khẩu nhân viên</th>
                    <th>Số điện thoại nhân viên</th>
                    <th>Hành động</th>
                </tr>
            </thead>
            <tbody>
                <tr runat="server" id="itemPlaceholder"></tr>
            </tbody>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr class="separator">
            <td><%# Eval("UserID") %></td>
            <td><%# Eval("UserName") %></td>
            <td><%# Eval("Email") %></td>
            <td><%# Eval("Passwords") %></td>
            <td><%# Eval("Tel") %></td>
            <td>
                 <asp:LinkButton ID="btnupdate" CommandName="Update" OnCommand="btnupdate_Command" CommandArgument='<%#Eval("UserID") %>' CssClass="btn btn-sm brn-primary"  runat="server" ><i  class="glyphicon glyphicon-pencil"></i></asp:LinkButton>

                                                    <asp:LinkButton CommandName="Delete" ID="btndlt" CommandArgument='<%#Eval("UserID") %>' 
                                                            OnClientClick="return confirm('Bạn có muốn xóa không !');"
                                                            OnCommand="btndlt_Command" CssClass="btn btn-sm brn-danger" runat="server" ><i  class="glyphicon glyphicon-trash"></i></asp:LinkButton>
            </td>
        </tr>
    </ItemTemplate>
</asp:ListView>
 <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" PageSize="3">
    <Fields>
        <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="true" ShowNextPageButton="false" />
        <asp:NumericPagerField ButtonType="Button" />
        <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false" />
    </Fields>
</asp:DataPager>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <asp:SqlDataSource ID="ds1" ConnectionString="<%$ConnectionStrings:connection %>" runat="server" SelectCommand="Select * from Users" />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ConnectionStrings:connection %>" SelectCommand="GetAllUsers"></asp:SqlDataSource>
    <asp:Label ID="lblNoData" runat="server" Visible="false">Không có dữ liệu nhân viên</asp:Label>
    

</asp:Content>

