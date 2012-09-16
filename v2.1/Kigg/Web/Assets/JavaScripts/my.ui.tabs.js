var mytabs = {
	init: function()
	{
		jQuery('#commentTabs ul').addClass('sidebar-tabs-nav');
		jQuery('#commentTabs ul').click(function(e) {			
			var href = jQuery(e.originalTarget).attr('href');
			var dialog = href.substring(1);
			mytabs.showComments(dialog);
			return false;
		});
		mytabs.hideAll();
		mytabs.showComments('questions');
		jQuery('#commentTabs').show();
	},
	init_users: function()
	{
		jQuery('#topUserTabs ul').addClass('sidebar-tabs-nav');
		jQuery('#topUserTabs ul').click(function(e) {			
		        var elem = e.originalTarget;
			if (!elem)
				elem = e.srcElement;
			var href = jQuery(elem).attr('href');			
			var dialog = href.substring(1);
			mytabs.showUsers(dialog);
			return false;
		});
		mytabs.hideAllUsers();
		mytabs.showUsers('topMovers');
	},
	init_tags: function()
	{
		jQuery('#tagTabs ul').addClass('sidebar-tabs-nav');
		jQuery('#tagTabs ul').click(function(e) {			
			var href = jQuery(e.originalTarget).attr('href');
			var dialog = href.substring(1);
			mytabs.showTags(dialog);
			return false;
		});
		mytabs.hideAllTags();
		mytabs.showTags('popularTags');
	},
	showTags: function(name)
	{
		mytabs.hideAllTags();
		jQuery('#'+name).show().css('display','block').addClass('sidebar-tabs-panel');		
		jQuery('#'+name).parent().find('ul > li > a[href="#'+name+'"]').parent().addClass('sidebar-tabs-selected');
	},
	showUsers: function(name)
	{
		mytabs.hideAllUsers();
		jQuery('#'+name).show().css('display','block').addClass('sidebar-tabs-panel');		
		jQuery('#'+name).parent().find('ul > li > a[href="#'+name+'"]').parent().addClass('sidebar-tabs-selected');
	},
	showComments: function(name)
	{
		mytabs.hideAll();	
		jQuery('#'+name).show().css('display','block').addClass('ui-tabs-panel');
		jQuery('#'+name).parent().find('ul > li > a[href="#'+name+'"]').parent().addClass('ui-tabs-selected');
	},
	hideAll: function()
	{
		jQuery('#questions').hide();	
		jQuery('#questions').parent().find('ul > li > a[href="#questions"]').parent().removeClass('ui-tabs-selected');		
		jQuery('#comments').hide();
		jQuery('#comments').parent().find('ul > li > a[href="#comments"]').parent().removeClass('ui-tabs-selected');		
		jQuery('#votes').hide();
		jQuery('#votes').parent().find('ul > li > a[href="#votes"]').parent().removeClass('ui-tabs-selected');	
	},
	hideAllTags: function()
	{
		jQuery('#popularTags').hide();
		jQuery('#popularTags').parent().find('ul > li > a[href="#popularTags"]').parent().removeClass('sidebar-tabs-selected');		
		
		jQuery('#myTags').hide();
		jQuery('#myTags').parent().find('ul > li > a[href="#myTags"]').parent().removeClass('sidebar-tabs-selected');		
		
		jQuery("#tagTabs").show();
	},
	hideAllUsers: function()
	{
		jQuery('#topMovers').hide();
		jQuery('#topMovers').parent().find('ul > li > a[href="#topMovers"]').parent().removeClass('sidebar-tabs-selected');		
		
		jQuery('#topLeaders').hide();
		jQuery('#topLeaders').parent().find('ul > li > a[href="#topLeaders"]').parent().removeClass('sidebar-tabs-selected');		
		
		jQuery("#topUserTabs").show();
	}
};