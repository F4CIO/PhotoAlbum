<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoDetails.aspx.cs" Inherits="UIWeb.PhotoDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Photo Details</title>
    <meta name="viewport" content="width=device-width, initial-scale=1,minimum-scale=1"/>
    <link rel="stylesheet" type="text/css" href="CSS/Main.css" /><%--<%=UIWeb.Helper.GetAbsoluteLocalFilePath("/CSS/Main.css")%>--%>
</head>
<body style="margin:0;">
    <div style="text-align:center;margin-left:auto;margin-right:auto;">  
        <div id="divAboveImage">
           <br />
            <a id="aGoBack" runat ="server"></a>
            <br />           
            <br />
            <asp:Label ID="lblCaption" runat="server" CssClass="subtitle"></asp:Label>
            <br />
            <br />
        </div>
        <center>
        <a id="aDownloadHiRes2" runat ="server" style="text-align:center;">
        <asp:Image id="imgPhoto" runat="server" Visible="true" BorderWidth="1" Width="90%" />
        </a>
        </center>
        <div id="divBelowImage">
            <br />
            <asp:Label ID="lblResolution" runat="server"></asp:Label>
            <br />
            <a id="aDownloadHiRes" runat ="server"></a>
            <br />
            <asp:Label ID="lblSize" runat="server"></asp:Label>
            <br />
            <div style="position:fixed;bottom:20px;width:100%;">
               <a id="aPrevious" runat ="server" style="float:left;margin-left:20px"></a>
                <a id="aNext" runat ="server" style="float:right;margin-right:20px;"></a>                   
            </div>
            <br />
        </div>
    </div>


    <script type="text/javascript" src="Scripts/jquery-3.2.0.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            fixImageSize();
        });

        window.onresize = function (event) {
            fixImageSize();
        }

        function fixImageSize() {
            var winHeight = $(window).height();
            var winWidth = $(window).width();
            var marginHeight = $('#divAboveImage').height() + $('#divBelowImage').height();
            var height = winHeight - marginHeight;
            var width = winWidth * 0.9;
            $('#imgPhoto').attr("style", "max-width:" + width + "px; max-height:" + height + "px;");
            $('body').css('overflow-y', 'hidden');
        }
    </script>
</body>
</html>
