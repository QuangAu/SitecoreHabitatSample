<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div id="CenterColumn">
  <sc:Placeholder Key="centercolumn" runat="server"></sc:Placeholder>
</div>
<p><%=Sitecore.Context.Item.ID %></p>
<p><%=(Parent as Sublayout).DataSource %></p>
<p><%=Sitecore.Context.Item.Fields["ListItem"].Value %></p>