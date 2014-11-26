'use strict';
var path = require('path');

// match one level down:
// e.g. 'bar/foo/{,*/}*.js'
// use this if you want to recursively match all subfolders:
// e.g. 'bar/foo/**/*.js'

module.exports = function (grunt) {
  'use strict';

  // load all grunt tasks
  require('load-grunt-tasks')(grunt);

  // show elapsed time at the end
  require('time-grunt')(grunt);
  // Init
  grunt.initConfig({
    timestamp: '<%= new Date().getTime() %>',
    pkg: grunt.file.readJSON('package.json'),

    src: {
      sass       : 'sass/**/*.{scss,sass}',
      css        : 'css/**/*.min.css',
      cssdev     : 'css/**/*.css',
      sitefinity : 'css/**/*-sitefinity*.css'
    },

    // clean all generated files
    clean: {
      all: {
        files: [{
          src: [
            '<%= src.sitefinity %>'
          ]
        }]
      }
    },

    // use always with target e.g. `csslint:doc` or `csslint:dev`
    // unfortunately there is no point to run csslint on compressed css so
    // csslint runs once, when you use `grunt` and it lints on documentation's css
    // csslint runs on every save when you use `grunt dev` and it lints the original file you are working on -> `style.css`
    csslint: {
      options: {
        csslintrc: 'csslint.json'
      },
      dev: {
        src: ['<%= src.sitefinity %>']
      }
    },

    compass: {                  // Task
      dev: {                   // Target
        options: {              // Target options
          sassDir: 'sass/',
          cssDir: 'css/',
          relativeAssets: true,
          assetCacheBuster: false
        }
      }
    },

    cssmin: {
      minify: {
        expand: true,
        cwd: 'css/',
        src: ['*.css', '!*.min.css'],
        dest: 'css/',
        ext: '.min.css'
      }
    },

    // // Image Optimization
    // imagemin: {
    //   dist: {
    //     options: {
    //       optimizationLevel: 4,
    //       progressive: true
    //     },
    //     files: [
    //       {
    //         expand: true,
    //         cwd: '<%= pkg.name %>/images/src/',
    //         src: ['**/*.{png,jpg,gif,jpeg}'],
    //         dest: '<%= pkg.name %>/images/dist/'
    //       }
    //     ]
    //   }
    // },

    watch: {
      options: {
        spawn: false
      },
      styles: {
        // files: ['<%= src.sass %>'], doesn't work for some reason
        files: ['<%= src.sass %>'],
        tasks: ['compass:dev', 'cssmin:minify', 'newer:csslint:dev']
      }
    },

    concurrent: {
      dev: {
        tasks: ['watch:styles', 'compass'],
        options: {
          logConcurrentOutput: true
        }
      }
    }
  });

  // Tasks
  // default task runs csslint once on startup on documentation's css
  grunt.registerTask('default', [
    'clean:all',
    'compass:dev',
    'cssmin:minify',
    'newer:csslint:dev',
    // 'newer:imagemin',
    'concurrent:dev'
  ]);
};