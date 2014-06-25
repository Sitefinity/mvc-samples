module.exports = function (grunt) {
    'use strict';
	
	//Project Configuration
	grunt.initConfig({
		
		pkg: grunt.file.readJSON('package.json'),
		
		jshint: {
			//define the files to lint
			files: ['gruntfile.js',
					'Telerik.Sitefinity.Frontend/Designers/Scripts/*.js',
					'ListWidget/MVC/Scripts/*.js'
			]
		}
	});
	
	//Load the needed plugins
	grunt.loadNpmTasks('grunt-contrib-jshint');
	
	//Default task(s)
	grunt.registerTask('default', ['jshint']);
	
};