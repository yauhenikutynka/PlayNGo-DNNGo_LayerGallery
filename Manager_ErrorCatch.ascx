﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_ErrorCatch.ascx.cs" Inherits="DNNGo.Modules.LayerGallery.Manager_ErrorCatch" %>
<asp:Literal ID="liException" runat="server"></asp:Literal>


 <asp:Button CssClass="input_button btn" ID="cmdReturn" resourcekey="cmdReturn" runat="server"  OnClientClick="CancelValidation();"
         Text="Return" CausesValidation="False" OnClick="cmdReturn_Click"></asp:Button>