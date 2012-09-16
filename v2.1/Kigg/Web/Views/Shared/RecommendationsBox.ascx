<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ OutputCache Duration="86400" VaryByParam="None" %><!-- doba -->
<!--<div style="display: block;">
    <ul class="sidebar-tabs-nav">
        <li class="sidebar-tabs-nav-item"><a href="#">Polecamy</a></li>
    </ul>
    <div id="recommendationsTab" class="sidebar-tabs-panel" style="display: block; text-align: center; height: 150px">
        <ul>
        <script type="text/javascript" language="JavaScript" charset="utf-8">
// Copyright 1996, Infohiway, Inc. (http://www.infohiway.com)
// Courtesy of SimplytheBest.net - http://simplythebest.net/scripts/
<!--
function adArray()
{
	for( i=0; i*3 < adArray.arguments.length; i++ )
	{
		this[i] = new Object();
		this[i].src = adArray.arguments[i*3];
		this[i].href = adArray.arguments[i*3+1];
		this[i].alt = adArray.arguments[i*3+2]
	}
	this.length = i;
}

function getAdNum()
{
	dat = new Date();
	dat = ( dat.getTime() + "").charAt(8);
	if ( dat.length == 1 )
	{
		ad_num = dat % ads.length;
	}
	else
	{
		ad_num = 0;
	}

	return ad_num;
}

var ads = new adArray(
"<%= Url.Image("redgate_small.jpg")%>","http://snurl.com/antsmp","Red Gate",
"<%= Url.Image("Microsoft_small.png") %>","http://microsoft.com/poland/","Microsoft",
"<%= Url.Image("logo_jetbrains_small.gif") %>","http://www.jetbrains.com/","JetBrains");

var ad_num = getAdNum();
document.write('<li><a href="' + ads[ad_num].href + '" target="_blank"><img src="' + ads[ad_num].src + '" border="0" name="js_ad" style="border: 0;" alt="' + ads[ad_num].alt + '"></a></li>');
link_num = document.links.length - 1;

function rotateSponsor()
{
	if (document.images)
	{
		ad_num = ( ad_num + 1 ) % ads.length;
		document.js_ad.src = ads[ad_num].src;
		document.links[link_num].href = ads[ad_num].href;
		setTimeout( "rotateSponsor()", 3500 );
	}
}
setTimeout( "rotateSponsor()", 3500 );
// -->
</script>
	    <%--<li><a href="http://www.microsoft.com/poland/edukacja/imaginecup/"><img src="<%= Url.Image("wspieramy-200.png")%>" alt="Imagine Cup" /></a></li>	    
            <li><a href="http://telerik.com">
                <img src="<%=Url.Image("Telerik_logo_small.png")%>" alt="Telerik" /></a></li>
	        <li><a href="http://red-gate.com"><img src="<%= Url.Image("redgate_small.jpg")%>" alt="Red Gate" /></a></li>
	        <li><a href="http://microsoft.com/poland/"><img src="<%= Url.Image("Microsoft_small.png") %>" alt="Microsoft" /></a></li>
	        <li><a href="http://www.jetbrains.com/"><img src="<%= Url.Image("logo_jetbrains_small.gif") %>" alt="JetBrains" /></a></li>
            <li><a href="http://polishwords.com.pl">
                <img src="<%=Url.Image("Polishwords150x33.png")%>" alt="Poliswords" /></a></li>
            <li><a href="http://af.studentlive.pl/">
                <img src="<%=Url.Image("AF_logo_czerwono_fioletowe_male.png")%>" alt="Academic Flash" /></a></li>--%>
        </ul>
    </div>
</div>
