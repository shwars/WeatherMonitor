<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeatherMonitorWeb._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Web Weather Monitor</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <asp:Chart ID="MainChart" runat="server" Height="600" Width="800">
                <series>
                    <asp:Series Name="MainSeries" ChartArea="MainChartArea" ChartType="Line">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="MainChartArea">
                    </asp:ChartArea>
                </chartareas>
                </asp:Chart>
        </div>
    </div>

</asp:Content>
