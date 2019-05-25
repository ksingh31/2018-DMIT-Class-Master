<%@ Page Title="Album list" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlbumInfoODS.aspx.cs" Inherits="WebApp.SamplePages.AlbumInfoODS" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:MessageUserControl runat="server" id="MessageUserControl" />
    <br />

    <asp:Repeater ID="InfoAlbumList" runat="server" DataSourceID="AlbuminfoList" ItemType="ChinookSystem.Data.DTOs.InfoAlbum">
        <HeaderTemplate>
            <h3>Employee Customer Support List</h3>
        </HeaderTemplate>
        <ItemTemplate>
            <div class="row">
                <div class="col-md-6">
                    <%# Item.AName %> ( <%# Item.ATitle %> )
                    <br />
                    <asp:GridView ID="GridView1" runat="server"
                        DataSource= <%# Item.Songs %>
                        CssClass="table" GridLines="Horizontal" BorderStyle="None">

                    </asp:GridView>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:ObjectDataSource ID="AlbuminfoList" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Info_AlbumSongList" 
        TypeName="ChinookSystem.BLL.InfoAlbumController"
        OnSelected ="CheckForException"></asp:ObjectDataSource>
</asp:Content>
