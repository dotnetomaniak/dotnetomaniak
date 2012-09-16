<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<StoryDetailViewData>" %>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        GenerateScripts();
    }

    private void GenerateScripts()
    {
        jQueryScriptManager scriptManager = jQueryScriptManager.Current;

        scriptManager.RegisterOnReady("ImageCode.set_promoteText('{0}');".FormatWith(Model.PromoteText));
        scriptManager.RegisterOnReady("ImageCode.init();");
        scriptManager.RegisterOnDispose("ImageCode.dispose();");
    }

</script>

<% string rootUrl = Model.RootUrl; %>
<div id="imageCode" class="customize">
    <% string kiggUrl = "{0}{1}".FormatWith(rootUrl, Url.RouteUrl("Detail", new { name = Model.Story.UniqueName })); %>
    <% string originalUrl = Model.Story.Url; %>
    <% string imageUrl = "{0}/{1}".FormatWith(rootUrl, "image.axd"); %>    
    <%= Html.Hidden("hidKiggUrl", kiggUrl)%>
    <%= Html.Hidden("hidOriginalUrl", originalUrl)%>
    <%= Html.Hidden("hidImageUrl", imageUrl)%>    
    <div class="customize-left">        
        <div class="customize-row">
            <label for="txtBorderColor">Kolor ramki:</label>
            <input id="txtBorderColor" type="text" class="color-hash" maxlength="6"/>
            <input id="hidBorderColor" type="hidden" value="<%= Model.CounterColors.BorderColor %>"/>
            <span id="spnBorderColor" class="color"></span>
        </div>
        <div class="customize-row">
            <label for="txtTextBackColor">Tło linku <%= Model.PromoteText %>:</label>
            <input id="txtTextBackColor" type="text" class="color-hash" maxlength="6"/>
            <input id="hidTextBackColor" type="hidden" value="<%= Model.CounterColors.TextBackColor %>"/>
            <span id="spnTextBackColor" class="color"></span>
        </div>
        <div class="customize-row">
            <label for="txtTextForeColor">Kolor linku <%= Model.PromoteText %>:</label>
            <input id="txtTextForeColor" type="text" class="color-hash" maxlength="6"/>
            <input id="hidTextForeColor" type="hidden" value="<%= Model.CounterColors.TextForeColor %>"/>
            <span id="spnTextForeColor" class="color"></span>
        </div>
        <div class="customize-row">
            <label for="txtCountBackColor">Kolor ramki licznika:</label>
            <input id="txtCountBackColor" type="text" class="color-hash" maxlength="6"/>
            <input id="hidCountBackColor" type="hidden" value="<%= Model.CounterColors.CountBackColor %>"/>
            <span id="spnCountBackColor" class="color"></span>
        </div>
        <div class="customize-row">
            <label for="txtCountForeColor">Kolor licznika:</label>
            <input id="txtCountForeColor" type="text" class="color-hash" maxlength="6"/>
            <input id="hidCountForeColor" type="hidden" value="<%= Model.CounterColors.CountForeColor %>"/>
            <span id="spnCountForeColor" class="color"></span>
        </div>
    </div>
    <div class="customize-right">
        <p>
            <img id="imgPreview" alt="Preview" class="smoothImage" src="" onload="javascript:SmoothImage.show(this)"/>
        </p>
        <div class="show-code">
            <textarea id="txtImageCode" cols="40" rows="9" readonly="readonly"></textarea>
        </div>
        <p>
            <a id="lnkUpdateCode" class="actionLink zmien" href="javascript:void(0)">zmień</a>
            <a id="lnkResetCode" class="actionLink resetuj" href="javascript:void(0)">reset</a>
        </p>
    </div>
</div>