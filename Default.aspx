<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableEventValidation="False" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">

        <div class="spider">
            <h1>VTB24</h1>
            <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" OnMenuItemClick="Menu1_MenuItemClick" Height="50px" CssClass="jimbo_menu">
                    <Items>                
                        <asp:MenuItem Text="Проекты" Value="Проекты">
                            <asp:MenuItem Text="DocsVision" Value="DV">
                                <asp:MenuItem Text="K1" Value="DVK1" NavigateUrl="~/Default.aspx?xml=DV/K1"></asp:MenuItem>
                                <asp:MenuItem Text="K4" Value="DVK4" NavigateUrl="~/Default.aspx?xml=DV/K4"></asp:MenuItem>
                            </asp:MenuItem>
                            <asp:MenuItem Text="MobileBank" Value="MB">
                                <asp:MenuItem Text="K1" Value="MBK1" NavigateUrl="~/Default.aspx?xml=MB/K1"></asp:MenuItem>
                                <asp:MenuItem Text="K4" Value="MBK4" NavigateUrl="~/Default.aspx?xml=MB/K4"></asp:MenuItem>
                            </asp:MenuItem>
                        </asp:MenuItem>    
                    </Items>
            </asp:Menu>
         
              
            <br />
         
            <asp:Button ID="Button1" runat="server" Height="50px" Text="Save" Width="100px" OnClick="Button1_Click" CssClass="spider_but" />
            <br />
        </div>
        <div class="cocojamba">
            <asp:Label ID="Label2" runat="server"></asp:Label>
            <br />
            <asp:Label ID="Label1" runat="server"></asp:Label>
                
        </div>
        <div class="jimbo">
             <asp:GridView ID="GridView1" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="1" ForeColor="Black" GridLines="Horizontal"
            OnRowEditing="GridView_RowEditing"         
            OnRowCancelingEdit="GridView_RowCancelingEdit"
            OnRowUpdating="GridView_RowUpdating"
            OnPageIndexChanging="GridView_PageIndexChanging"
            OnRowDataBound="OnRowDataBound" CellSpacing="10" Font-Size="Small" HorizontalAlign="Left" Width="100%" AllowCustomPaging="True" AllowPaging="True" PageSize="100">
                <Columns>
                    <asp:CommandField />
                    <asp:CommandField ShowEditButton="True"  />
                </Columns>
                <EditRowStyle Font-Size="Small" Width="300px" HorizontalAlign="Left" VerticalAlign="Middle" />
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#242121" />
            </asp:GridView>
        </div>
           
                                    
        
       
    </div>
    

</asp:Content>
