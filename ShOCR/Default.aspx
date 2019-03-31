<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ShOCR._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <p>
    <asp:FileUpload ID="imageUpload" runat="server" /> &nbsp; &nbsp;
        <asp:Button ID="Process" runat="server" Text="Process" OnClick="Process_Click" />
    </p>
    <p>
        Original Image<br />
        <asp:Image ID="imageOrigion" runat="server" />
    </p>

    <p>
        Gray Image <br />
        <asp:Image ID="imageGray" runat="server" />
    </p>

    <p>
        Black White Image <br />
        <asp:Image ID="imageBlackWhite" runat="server" />
    </p>

    <p>
        Noise removed image <br />
        <asp:Image ID="imageNoiseRmoved" runat="server" />
    </p>

    <p>
        Image after detection <br />
        <asp:Image ID="imageDetected" runat="server" />
    </p>

        <div id="txtResult" runat="server" />


</div>


</asp:Content>
