<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="searchBox">
  <div class="fb-like" data-href="http://www.facebook.com/dotnetomaniakpl" data-send="false" data-layout="button_count"
    data-width="450" data-show-faces="false" data-colorscheme="light">
  </div>
</div>
<div id="fb-root">
</div>
<script>  (function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/pl_PL/all.js#xfbml=1";
    fjs.parentNode.insertBefore(js, fjs);
  } (document, 'script', 'facebook-jssdk'));</script>
