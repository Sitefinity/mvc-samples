$(document).ready(function() {
  $('.js-Gallery').find('a').magnificPopup({
    gallery: {
      enabled: true
    },
    type:'image',
    zoom: {
      enabled: true,

      duration: 300,
      easing: 'ease-in-out',
      opener: function(openerElement) {
        return openerElement.is('img') ? openerElement : openerElement.find('img');
      }
    }

  });
});
