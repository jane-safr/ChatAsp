<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="A0Access.aspx.cs" Inherits="ChatASP.A0Access" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>

    <h3> <asp:Label ID="welcome" runat="server" Text="Добро пожаловать!"></asp:Label></h3>
    <h4>  Пользователи в программе А0  </h4>
    <asp:TextBox ID="tFilter" runat="server" Width="158px" placeholder="Поиск пользователя"></asp:TextBox>
    <br />
<p>
 
<asp:Button ID="btnSub" runat="server" Text="Найти" OnClick="ShowGrid" />

       <asp:Button ID="Button2" runat="server" Text="В Excel" OnClick="Button2_Click" />
    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="false" />
</p>



</div>
    </form>
</body>
</html>
