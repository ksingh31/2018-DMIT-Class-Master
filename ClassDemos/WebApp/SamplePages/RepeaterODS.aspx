<%@ Page Title="Repeater ODS" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RepeaterODS.aspx.cs" Inherits="WebApp.SamplePages.RepeaterODS" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Demonstrate Repeader Control using ODS</h1>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <br />
    <asp:Repeater ID="EmployeeClientList" runat="server" 
        DataSourceID="EmployeeClientListODS"
        ItemType="ChinookSystem.Data.DTOs.SupportEmployee">  <%--this tells EnableTheming structure of data--%>
        <HeaderTemplate>
            <h3>Employee Customer Support List</h3>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <%# Item.Name %> ( <%# Item.ClientCount %> )
                    <br />
                    <asp:GridView ID="ClientsListing" runat="server"
                        DataSource= <%# Item.ClientList %>
                        CssClass="table" GridLines="Horizontal" BorderStyle="None">

                    </asp:GridView>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:ObjectDataSource ID="EmployeeClientListODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Employees_ListSupportEmployees"
        TypeName="ChinookSystem.BLL.EmployeeController"
            OnSelected ="CheckForException">

    </asp:ObjectDataSource>
</asp:Content>
