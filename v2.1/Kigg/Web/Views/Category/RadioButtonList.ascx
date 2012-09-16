<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %><!-- doba -->
<span class="categoryRadioList">
<%foreach (ICategory category in Model.OrderBy(c => c.Name)) %>
<%{%>
    <label><input type="radio" name="category" value="<%= Html.AttributeEncode(category.UniqueName) %>"/> <%= Html.Encode(category.Name)%></label>
<%}%>
    <br class="clearLeft"/>
</span>