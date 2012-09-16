<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ICollection<ICategory>>" %>
<div class="radios">
<%foreach (ICategory category in Model.OrderBy(c => c.Name)) %>
<%{%>
    <div class="radio-wrapper"><input type="radio" name="category" value="<%= Html.AttributeEncode(category.UniqueName) %>"/><label class="radio-label"><%= Html.Encode(category.Name)%></label></div>
<%}%>    
</div>