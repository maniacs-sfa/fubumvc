﻿


var FubuDiagnostics = {
	currentScreen: {
		deactivate: function(){}
	},
	
	components: {},
	
	showScreen: function(screen, element, section){
		this.currentScreen.deactivate();
		this.activeSection = section;
		
		var pane = document.getElementById('main-pane');

		screen.activate(pane);
		
		this.currentScreen = screen;
		
		// set title and description
		$('#main-heading').html(element.title);
		$('#main-description').html(element.description);
		
		this.refreshNavigation(section);
	},
	
	refreshNavigation: function(section){
		this.activeSection = section;
		this.navBar.setProps(this);
	},
	

    start: function(navBar) {
		this.navBar = navBar;
		this.defaultScreen = {
			activate: function(){
				$('#home-view').show();
				$('main-pane').hide();
			},
			
			deactivate: function(){
				$('#home-view').hide();
				$('main-pane').show();
			}
		};

		var router = new Backbone.Router();
		router.route('*actions', 'defaultRoute', function(){
			FubuDiagnostics.showScreen(FubuDiagnostics.defaultScreen, {title: 'Welcome to FubuMVC!', description: 'The .Net web framework that gets out of your way'}, {});
		});
		
		
		_.each(this.sections, function(s){
			s.addRoutes(router);
		});
		
		this.router = router;
		
		Backbone.history.start();
    },


	
    sections: [],
        

		
    addSection: function(section) {
        this.sections.push(section);
            
		FubuDiagnosticsSection(section);
			
        this.lastSection = section;
		
		return section;
    },
    
    section: function(key) {
        throw "I have not done this yet";
    },

    addView: function(view) {
		this.lastSection.add(view);
    },

	activeSection: {},
};