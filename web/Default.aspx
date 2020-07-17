<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ServerDocs.Main" EnableEventValidation="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="height: 642px;margin:0">
    <form id="form1" runat="server" >
    <div style="border-bottom:solid 1px #ccc;position:fixed;width:100%;padding:10px;background-color:#ccc;" >
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DDL_Date" runat="server" Height="16px" Width="202px" AutoPostBack="true" OnSelectedIndexChanged="DDL_Date_SelectedIndexChanged"  >
        </asp:DropDownList>
        &nbsp;
        <asp:Label ID="lblServer" runat="server" Text="Server:"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DDL_Server" runat="server" Height="16px" Width="202px" AutoPostBack="true" OnSelectedIndexChanged="DDL_Server_SelectedIndexChanged">
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnPDFExport" runat="server" Height="25px" OnClick="btnPDFExport_Click" Text="PDF Export" Width="80px" />
        &nbsp;&nbsp;
        <asp:Label ID="LblArchNum" runat="server" Text="Label" Visible="False"></asp:Label>
        <asp:Label ID="LblError" runat="server" Text="Label" Visible="False" ForeColor="Red"></asp:Label>
        </div>
        <div style="padding:40px 20px 20px 20px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="contents" runat="server"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    </div>
    </form>
</body>
</html>
