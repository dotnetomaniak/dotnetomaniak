<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<BaseViewData>" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %>
<% bool isAuthenticated = Model.CanCurrentUserModerate; %>

<!-- doba -->
<div class="pageHeader">
    <div class="pageTitle">
        <h2>
            Polecamy</h2>
    </div>
</div>
<div class="recommend-left-column">
    <a href="http://kompy.staker.pl" title="Komputery"><img src="<%= Url.Image("kompy-staker-promo.png") %>" alt="antylama" /></a>
</div>
       <a id="lnkEditRecomendation" href="javascript:void(0)">Edytuj</a>