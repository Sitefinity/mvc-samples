;(function ( $, window, document, undefined ) {
  "use strict";

  var pluginName = "fixit",
      defaults = {
        top: 0,
        fixClass: "is-fixed",
        dummyClass: "fixed-dummy",
        classOnly: true,
        renderDummy: true
      };

  function Plugin ( element, options ) {
    this.element = element;
    this.$element = $(element);
    this.options = $.extend( {}, defaults, options );
    this._name = pluginName;

    this.$window = $(window);
    this.position = this.$element.css("position");
    this.fixed = false;
    this.working = false;

    this.width = this.$element.width();
    this.height = this.$element.height();
    this.offset = this.$element.offset();

    this.$window
      .on("scroll.fixit", $.proxy(this.onscroll, this))
      .trigger("scroll.fixit");

  }

  Plugin.prototype = {
    onscroll: function() {
      this.scrollTop = this.$window.scrollTop();
      this.requestTick();
    },

    requestTick: function(method) {
      method = method || "check";
      if (!this.working) {
        window.requestAnimationFrame($.proxy(this[method], this));
        this.working = true;
      }
    },

    check: function() {
      if (this.scrollTop + parseInt(this.options.top, 10) > this.offset.top) {
        if (!this.fixed) { this.fix(); }
      } else {
        if (this.fixed) { this.reset(); }
      }
      this.working = false;
    },

    addDummy: function() {
      this.$dummy = $("<div>", {"class": this.options.dummyClass}).insertBefore(this.$element);
    },

    fix: function() {
      if (this.options.renderDummy) {
        this.addDummy();
      }

      this.$element.addClass(this.options.fixClass);

      if (!this.options.classOnly) {
        this.$element.css({
          width: this.width,
          height: this.height,
          position: "fixed",
          top: this.options.top + "px",
          left: this.offset.left
        });
      }

      this.fixed = true;
    },

    reset: function() {
      this.$element
        .removeClass(this.options.fixClass);

      if (!this.options.classOnly) {
        this.$element.css({position: this.position});
      }

      if (this.options.renderDummy) {
        this.$dummy.remove();
      }

      this.fixed = false;
    }
  };

  $.fn[ pluginName ] = function ( options ) {
    return this.each(function() {
      if ( !$.data( this, "plugin_" + pluginName ) ) {
        $.data( this, "plugin_" + pluginName, new Plugin( this, options ) );
      }
    });
  };

})( jQuery, window, document );
