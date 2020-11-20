<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="GridDyn.aspx.cs" Inherits="ChatASP.GridDyn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>

&nbsp;<asp:TextBox ID="tFilter" runat="server" Width="158px" placeholder="Поиск в AD"></asp:TextBox>
    <br />
Выберите колонки для отображения в таблице:
<asp:CheckBoxList runat="server" ID="chkFields" DataTextField="Column_name" DataValueField="Column_name" RepeatDirection="Horizontal" RepeatLayout="Flow" />
<p>
 
<asp:Button ID="btnSub" runat="server" Text="Таблица" OnClick="ShowGrid" />

       <asp:Button ID="Button2" runat="server" Text="Excel" OnClick="Button2_Click" />
    <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="false" />
</p>



</div>
    </form>
</body>
</html>
