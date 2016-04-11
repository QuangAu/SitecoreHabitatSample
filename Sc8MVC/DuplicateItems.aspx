<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DuplicateItems.aspx.cs" Inherits="Sc8MVC.DuplicateItems" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Country Code: <asp:TextBox runat="server" ID="countryCode"></asp:TextBox> <br />
        Host name: <asp:TextBox runat="server" ID="hostName"></asp:TextBox>
    <asp:Button runat="server" Text="Click" OnClick="Unnamed_Click" />
        <asp:CheckBox runat="server" ID="IsPublish" Text="Publish new site" />
    </div>
    </form>
    <p><%=ErrorMessage %></p>
</body>
</html>
