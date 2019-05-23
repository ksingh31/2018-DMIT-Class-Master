<%@ Page Title="Using Linq Query" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinqQueryODS.aspx.cs" Inherits="WebApp.SamplePages.LinqQueryODS" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Using Linq/Entity Query</h1>

    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <br />
    <asp:ListView ID="AlbumArtistsList" runat="server" DataSourceID="AlbumArtistsListODS" GroupItemCount="3">

        <AlternatingItemTemplate>
            <td runat="server" style="">Title:
                <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /><br />
                Year:
                <asp:Label Text='<%# Eval("Year") %>' runat="server" ID="YearLabel" /><br />
                ArtistName:
                <asp:Label Text='<%# Eval("ArtistName") %>' runat="server" ID="ArtistNameLabel" /><br />
            </td>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <EmptyItemTemplate>
            <td runat="server" />
        </EmptyItemTemplate>
        <GroupTemplate>
            <tr runat="server" id="itemPlaceholderContainer">
                <td runat="server" id="itemPlaceholder"></td>
            </tr>
        </GroupTemplate>
        <ItemTemplate>
            <td runat="server" style="">Title:
                <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /><br />
                Year:
                <asp:Label Text='<%# Eval("Year") %>' runat="server" ID="YearLabel" /><br />
                ArtistName:
                <asp:Label Text='<%# Eval("ArtistName") %>' runat="server" ID="ArtistNameLabel" /><br />
            </td>
        </ItemTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table runat="server" id="groupPlaceholderContainer" style="" border="0">
                            <tr runat="server" id="groupPlaceholder"></tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                        <asp:DataPager runat="server" PageSize="12" ID="DataPager1">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                <asp:NumericPagerField></asp:NumericPagerField>
                                <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                            </Fields>
                        </asp:DataPager>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <SelectedItemTemplate>
            <td runat="server" style="">Title:
                <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /><br />
                Year:
                <asp:Label Text='<%# Eval("Year") %>' runat="server" ID="YearLabel" /><br />
                ArtistName:
                <asp:Label Text='<%# Eval("ArtistName") %>' runat="server" ID="ArtistNameLabel" /><br />
            </td>
        </SelectedItemTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="AlbumArtistsListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Album_ListAlbumArtists" 
        TypeName="ChinookSystem.BLL.AlbumController"
        OnSelected="CheckForException" >
    </asp:ObjectDataSource>
</asp:Content>
