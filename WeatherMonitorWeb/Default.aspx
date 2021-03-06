﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeatherMonitorWeb._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Web Weather Monitor</h1>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Main Chart</h2>
                <asp:Chart ID="MainChart" runat="server" Height="600" Width="800">
                    <series>
                        <asp:Series Name="MainSeries" ChartArea="MainChartArea" ChartType="Line" Color="Black"/>
                        <asp:Series Name="RealSeries" ChartArea="MainChartArea" ChartType="Point" Color="Red" />
                    </series>
                    <chartareas>
                        <asp:ChartArea Name="MainChartArea" />
                    </chartareas>
                </asp:Chart>
            <br /><br />
            <asp:DropDownList runat="server" AutoPostBack="True" ID="ddDispType">
                <asp:ListItem Value="Temperature" />
                <asp:ListItem Value="Pressure" />
                <asp:ListItem Value="Humidity" />
                <asp:ListItem Value="Luminocity" />
            </asp:DropDownList>
            <asp:DropDownList runat="server" AutoPostBack="True" ID="ddTimePeriod">
                <asp:ListItem Value="Day" />
                <asp:ListItem Value="Week" />
                <asp:ListItem Value="Month" />
                <asp:ListItem Value="Year" />
            </asp:DropDownList>
        </div>
    </div>

</asp:Content>
