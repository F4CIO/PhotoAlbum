    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhotoFolder.aspx.cs" Inherits="UIWeb.PhotoFolder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Photo Album</title>
     <meta name="viewport" content="width=device-width, initial-scale=1,minimum-scale=1"/>
    <link rel="stylesheet" type="text/css" href="CSS/Main.css" /><%--<%=UIWeb.Helper.GetAbsoluteLocalFilePath("/CSS/Main.css")%>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAllInDataset" TypeName="UIWeb.PhotosHandler">
        </asp:ObjectDataSource>            
    </div>
    <center>
        <div id="divGoBackOneLevel" runat="server">    
            <a id="aGoBackOneLevel" runat="server"></a>
            <br />
            <br />
            <asp:Label ID="lblAlbumName" runat="server"  CssClass="subtitle" Text=" "/>
            <br />
        </div>
        <br/>
         <asp:ListView ID="ListView1" runat="server" 
            DataSourceID="ObjectDataSource1" 
            GroupPlaceholderID="groupPlaceholder" 
            ItemPlaceholderID="itemPlaceholder"            
            ConvertEmptyStringToNull="False" 
            onitemdatabound="ListView1_ItemDataBound" 
            onload="ListView1_Load"
            >
            
            <LayoutTemplate>           
                <div class="photoFolderItemsContainer">                
                    <div runat="server" id="groupPlaceholder"></div>
                </div>
            </LayoutTemplate>

            <GroupTemplate>               
                <div runat="server" id="itemPlaceholder"></div>
            </GroupTemplate>

            <ItemTemplate> 
                    <div class="photoFolderItem">
                        <center>
                            <asp:Panel id="pnlPhoto" runat="server" CssClass="photoPanel" BorderStyle="Outset" BorderWidth="1">
                                <center>
                                <a id="aPhotoImage"  runat="server" target="_self"> 
                               
                                    <asp:Image id="imgPhoto" runat="server"/>         
                                                       
                                </a>                           
                                </center>
                            </asp:Panel> 
                        </center>
                        <div>
                            <a id="aPhotoCaption" runat="server" target="_self"/>  
                        </div>
                    </div>
            </ItemTemplate>
            
            <EmptyDataTemplate>
                <asp:Label ID="lblNoPhotos" runat="server"></asp:Label>
            </EmptyDataTemplate>
            
        </asp:ListView>
       <div style="clear:both"></div>
       <asp:DataPager ID="pgrAlbum" runat="server" PagedControlID="ListView1" QueryStringField="Page" PageSize="12">
                    <Fields>
                        <asp:NextPreviousPagerField 
                            ButtonType="Button" 
                            ShowFirstPageButton="False" 
                            ShowNextPageButton="False" 
                            ShowPreviousPageButton="True" />
                        <asp:NumericPagerField />
                        <asp:NextPreviousPagerField 
                            ButtonType="Button" 
                            ShowLastPageButton="False" 
                            ShowNextPageButton="True" 
                            ShowPreviousPageButton="False"
                            />
                    </Fields>                         
                </asp:DataPager>

    </center>
    
    </form>

    <script type="text/javascript" src="Scripts/jquery-3.2.0.min.js"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var thumbnailCountInRow = parseInt('<%=ViewState["thumbnailCountInRow"].ToString()%>');
            var photoFolderItemWidth =$('.photoFolderItem').width();
            var photoFolderItemMargin = parseInt($('.photoFolderItem').css('margin'));
            var maxWidth = thumbnailCountInRow * (photoFolderItemWidth + (photoFolderItemMargin * 2)) + 15;
            $('.photoFolderItemsContainer').attr("style", "max-width:" + maxWidth + "px")
        });
    </script>
</body>
</html>
