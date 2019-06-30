<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplaySubCategory.aspx.cs" Inherits="PraxiLabs.DisplaySubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
    <div>  
    <asp:Menu ID="Menu1" runat="server" Orientation="Vertical">  
        <LevelMenuItemStyles>  
            <asp:MenuItemStyle CssClass="ParentMenu" />  
            
            <asp:MenuItemStyle CssClass="ChildMenu" />  
            <asp:MenuItemStyle CssClass="ChildMenu" />  
        </LevelMenuItemStyles>  
        <StaticSelectedStyle CssClass="selected" />  
    </asp:Menu>  
</div>  
</asp:Content>
