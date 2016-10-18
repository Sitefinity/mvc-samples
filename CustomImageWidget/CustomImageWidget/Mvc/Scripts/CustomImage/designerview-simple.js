var designerModule = angular.module('designer');
angular.module('designer').requires.push('sfFields');
angular.module('designer').requires.push('sfSelectors');

//// NOTE: Use this code only with Sitefinity version 9.1 or above. Otherwise the "ngSanitize" module should no be included. 
angular.module('designer').requires.push('ngSanitize');