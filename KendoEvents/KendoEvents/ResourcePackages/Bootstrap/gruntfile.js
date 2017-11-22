'use strict';
var path = require('path');

// match one level down:
// e.g. 'bar/foo/{,*/}*.js'
// use this if you want to recursively match all subfolders:
// e.g. 'bar/foo/**/*.js'

module.exports = function(grunt) {
    'use strict';

    // Project assets
    // Loads project js files which will be concatenated and minified in one file
    var projectJsfiles = grunt.file.readJSON('jsfiles.json').concatJsFiles;

    // Name of the folder that contains project specific assets (scss, js, images, etc.)
    // Rename this folder if needed
    var projectAssetsFolder = "project";

    // Options needed for webfont task.
    // Starting code point for Sitefinity font icons.
    var sfCodePoints = {
        "file": 0xF001,
        "no-image": 0xF002
    }
    var webFontOptions = {
        destHtml: '',
        engine: 'node',
        font: 'sf-icon-font',
        stylesheet: 'scss',
        partialPrefix: true,
        relativeFontPath: '../fonts/',
        template: 'assets/src/sitefinity/sass/components/icons/feather-icons.css',
        types: 'eot,woff,ttf,svg',
        order: 'eot,woff,ttf,svg',
        codepoints: sfCodePoints,
        startCodepoint: 0x00b1,
        normalize: true,
        fontHeight: 4096,
        ascent: 4096,
        descent: 0,
        autoHint: false
    }


    // Load all grunt tasks
    require('load-grunt-tasks')(grunt);
    // Show elapsed time at the end
    require('time-grunt')(grunt);

    // Init
    grunt.initConfig({
        timestamp: '<%= new Date().getTime() %>',
        pkg: grunt.file.readJSON('package.json'),

        src: {
            path: 'assets/src',
            sass: '**/*.{scss,sass}'
        },

        dist: {
            path: 'assets/dist'
        },

        // Clean all generated files
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
            css: {
                files: [{
                    src: [
                        '<%= dist.path %>/**/*.css',
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
            dist: {
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
        // unfortunately there is no point to run csslint on compressed css so
        // csslint runs once, when you use `grunt` and it lints on documentation's css
        // csslint runs on every save when you use `grunt dev` and it lints the original file you are working on -> `style.css`
        csslint: {
            options: {
                csslintrc: 'csslint.json',
                quiet: true,
                formatters: [{
                    id: 'csslint-xml',
                    dest: 'csslint_report/csslint.xml'
                }]
            },
            dev: {
                expand: true,
                cwd: '<%= dist.path %>/css/',
                src: ['*.css', '!*.min.css']
            }
        },

        webfont: {
            icons: {
                src: ['assets/src/sitefinity/icons/*.svg', 'assets/src/' + projectAssetsFolder + '/icons/*.svg'],
                dest: 'assets/dist/fonts/',
                destCss: 'assets/src/sitefinity/sass/components/icons/',
                options: webFontOptions
            }
        },

        cssmin: {
            minify: {
                expand: true,
                cwd: '<%= dist.path %>/css/',
                src: ['*.css', '!*.min.css'],
                dest: '<%= dist.path %>/css/',
                ext: '.min.css',
                extDot: 'last'
            }
        },

        copy: {
            dist: {
                files: [
                    { expand: true, src: ['node_modules/bootstrap-sass/assets/fonts/bootstrap/*'], dest: 'assets/dist/fonts/bootstrap/', flatten: true, filter: 'isFile' },
                    { expand: true, cwd: 'assets/src/' + projectAssetsFolder + '/fonts', src: '**', dest: 'assets/dist/fonts/' }
                ]
            }
        },

        // Concatenates & minifies js files
        // Processes the files described in 'jsfiles.json' + bootstrap.js
        uglify: {
            options: {
                report: 'gzip',
                warnings: true,
                mangle: true,
                compress: true
            },
            dist: {
                files: {
                    // Project assets
                    // Concatenates project files listed in jsfiles.json
                    '<%= dist.path %>/js/project.min.js': projectJsfiles
                }
            }
        },

        // Image Optimization
        imagemin: {
            dist: {
                options: {
                    optimizationLevel: 4,
                    progressive: true
                },
                files: [
                    {expand: true, cwd: 'assets/src/sitefinity/images', src: ['**/*.{png,jpg,gif,jpeg,svg}', '!fonts/*', '!sprite/*.*'], dest: 'assets/dist/images'},
                    {expand: true, cwd: 'assets/src/' + projectAssetsFolder + '/images', src: ['**/*.{png,jpg,gif,jpeg,svg}', '!fonts/*', '!sprite/*.*'], dest: 'assets/dist/images'}
                ]
            }
        },

        // Sprite generation
        sprite: {
            dist: {
                src: ['assets/src/sitefinity/images/sprite/*.png', 'assets/src/' + projectAssetsFolder + '/images/sprite/*.png'],
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
                tasks: ['sass', 'csslint:dev', 'cssmin']
            },
            images: {
                files: ['<%= src.path %>/**/*.{png,jpg,gif,jpeg}'],
                tasks: ['clean:images', 'sprite', 'imagemin']
            },
            js: {
                files: ['<%= src.path %>/**/*.js'],
                tasks: ['uglify']
            }
        },

        concurrent: {
            dist: {
                tasks: ['watch:styles', 'watch:js', 'watch:images'],
                options: {
                    logConcurrentOutput: true
                }
            }
        }
    });

    // Tasks
    // Task to generate icon font
    grunt.registerTask('iconfont', [
        'webfont', 'copy'
    ]);

    // Default task
    grunt.registerTask('default', ' ', function() {
        grunt.task.run('clean:all');
        grunt.task.run('newer:sprite');
        // grunt.task.run('webfont');
        // grunt.task.run('copy');
        grunt.task.run('sass');
        grunt.task.run('uglify');
        grunt.task.run('csslint:dev');
        grunt.task.run('cssmin');
        grunt.task.run('newer:imagemin');
        grunt.task.run('concurrent');

    });
};
