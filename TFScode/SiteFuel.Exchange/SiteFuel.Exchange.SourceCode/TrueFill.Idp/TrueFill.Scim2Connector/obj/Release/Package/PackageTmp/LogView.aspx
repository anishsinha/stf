<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogView.aspx.cs" 
    Inherits="TrueFill.Scim2Service.LogView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal runat="server" ID="llog" />
            <br />
            <asp:Button runat="server" ID="bclear" Text="Clear" OnClick="BClear_Click" />
            <asp:Button runat="server" ID="breload" Text="Reload" />
        </div>
    </form>
</body>
</html>
