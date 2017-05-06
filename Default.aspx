<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align-last:center">

        
         <input type="hidden" id="hintID" value="5555" runat="server" /><br />
         <asp:Button CssClass="button"  runat="server" id="ALButton" OnClick="ALButton_Click"  style="font-size:larger"  Text="Anatomy Learning" /><br />
         <a href="" style="font-size:larger" >MLAS</a><br />
         <a href="" style="font-size:larger" >ORCS</a><br />

    </div>

    <div class="row">
       
    </div>
</asp:Content>
