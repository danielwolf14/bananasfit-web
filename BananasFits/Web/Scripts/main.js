//(function () {

//    // add a close button to the '.validation-summary-errors' div "flash"

//    function addCloseButton($container) {

//        $('.close', $container).remove();

//        $('<a>', {
//            'class': 'close',
//            'href': '#'
//        }).append('×').click(function (e) {

//            e.preventDefault();

//            var $div = $(this).parent('div');

//            $div.slideUp(function (e) {
//                $div.remove();
//            });

//        }).prependTo($container);

//    };

//    // detect the '.validation-summary-errors' div after full page postback
//    if ($('.validation-summary-errors').length !== 0) {
//        addCloseButton($('.validation-summary-errors'));
//    }

//    // the '.validation-summary-errors' has appeared without full page postback
//    window.onValidatorSummaryShown = function () {
//        addCloseButton(this);
//    };

//}());