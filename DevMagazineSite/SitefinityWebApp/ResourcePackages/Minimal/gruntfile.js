'use strict';
var path = require('path');

// match one level down:
// e.g. 'bar/foo/{,*/}*.js'
// use this if you want to recursively match all subfolders:
// e.g. 'bar/foo/**/*.js'

module.exports = function (grunt) {
	'use strict';

	// Project assets
	// Loads project js files which will be concatenated and minified in one file
	var jsfiles = grunt.file.readJSON('jsfiles.json').concatJsFiles;

	// Name of the folder that contains project specific assets (scss, js, images, etc.)
	// Rename this folder if needed
	var projectAssetsFolder = "project";

	// Load all grunt tasks
	require('load-grunt-tasks')(grunt);
	// Show elapsed time at the end
	require('time-grunt')(grunt);

	// Init
	grunt.initConfig({
		timestamp: '<%= new Date().getTime() %>',
		pkg: grunt.file.readJSON('package.json'),

		src: {
			path       : 'assets/src',
			sass       : '**/*.{scss,sass}'
		},

		dist: {
			path       : 'assets/dist'
		},

		// clean all generated files
		clean: {
			all: {
				files: [{
					src: [
						'<%= dist.path %>/**/*.css',
						'<%= dist.path %>/**/*.js',
						'<%= dist.path %>/**/*.{png,jpg,gif,jpeg}',
						'csslint_report'
					]
				}]
			},
			images: {
				files: [{
					src: [
						'<%= dist.path %>/**/*.{png,jpg,gif,jpeg}'
					]
				}]
			}
		},

		sass: {
			options: {
				outputStyle: 'nested'
			},
			sitefinity: {
				files: {
					'<%= dist.path %>/css/sitefinity.css': '<%= src.path %>/sitefinity/sass/sitefinity.scss'
				}
			},
			// Project assets
			// Will look for any .scss files in projectAssetsFolder directory
			project: {
				files: [{
					expand: true,
					cwd: '<%= src.path %>/' + projectAssetsFolder + '/sass',
					src: ['*.scss'],
					dest: '<%= dist.path %>/css/',
					ext: '.css'
				}]
			}
		},

		// use always with target e.g. `csslint:doc` or `csslint:dev`
		// there is no point to run csslint on compressed css so
		// csslint runs once, when you use `grunt` and it lints on documentation's css
		// csslint runs on every save when you use `grunt dev` and it lints the original file you are working on -> `style.css`
		csslint: {
			options: {
				csslintrc: 'csslint.json',
				quiet: true,
				formatters: [
					{id: 'csslint-xml', dest: 'csslint_report/csslint.xml'}
				]
			},
			dev: {
				expand: true,
				cwd: '<%= dist.path %>/css/',
				src: ['*.css', '!*.min.css']
			}
		},

		cssmin: {
			minify: {
				expand: true,
				cwd: '<%= dist.path %>/css/',
				src: ['*.css', '!*.min.css'],
				dest: '<%= dist.path %>/css/',
				ext: '.min.css'
			}
		},

		// Concat & minify
		// This processes the files described in 'jsfiles.json'
		uglify: {
			options: {
				report: 'gzip',
				warnings: true,
				mangle: true,
				compress: true
			},
			project: {
				files: {
					// Project assets
					// Concatenates and minifies js project files listed in jsfiles.json
					'<%= dist.path %>/js/project.min.js':  jsfiles
				}
			}
		},

		// Image Optimization
		imagemin: {
			sitefinity: {
				options: {
					optimizationLevel: 4,
					progressive: true
				},
				files: [
					{
						expand: true,
						cwd: 'assets/src/sitefinity',
						src: ['**/*.{png,jpg,gif,jpeg}', '!images/sprite/*.*'],
						dest: 'assets/dist'
					}
				]
			},
			project: {
				options: {
					optimizationLevel: 4,
					progressive: true
				},
				files: [{
					expand: true,
					cwd: 'assets/src/' + projectAssetsFolder,
					src: ['**/*.{png,jpg,gif,jpeg}'],
					dest: 'assets/dist'
				}]
			}
		},

		// Sprite generation
		sprite:{
			all: {
				src: 'assets/src/sitefinity/images/sprite/*.png',
				dest: 'assets/src/sitefinity/images/sprite.png',
				destCss: 'assets/src/sitefinity/sass/widgets/socialShare/_sf-sprite.scss',
				imgPath: '../images/sprite.png',
				cssTemplate: 'assets/src/sitefinity/sass/widgets/socialShare/sf-sprite.mustache'
			}
		},

		watch: {
			options: {
				spawn: false
			},
			styles: {
				files: ['<%= src.path %>/**/*.{scss,sass}'],
				tasks: ['sass:project', 'csslint:dev', 'cssmin']
			},
			images: {
				files: ['<%= src.path %>/**/*.{png,jpg,gif,jpeg}'],
				tasks: ['clean:images', 'sprite', 'imagemin']
			},
			js: {
				files: ['<%= src.path %>/**/*.js'],
				tasks: ['uglify:project']
			},
		},

		concurrent: {
			dev: {
				tasks: ['watch:styles', 'watch:js', 'watch:images'],
				options: {
					logConcurrentOutput: true
				}
			}
		}
	});


	// default task runs csslint once on startup on documentation's css
	grunt.registerTask('default', [
		'clean:all',
		'newer:sprite',
		'sass:sitefinity',
		'sass:project',
		'csslint:dev',
		'cssmin',
		'uglify',
		'newer:imagemin',
		'concurrent:dev'
	]);
};
