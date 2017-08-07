(function ($) {
    'use strict';

    $(document).ready(function () {
        $('#form_counter').submit(function (event) {
            event.preventDefault();

            // setup button
            $('#btn_count').attr('disabled', 'disabled');
            $('#btn_count').html('Counting...');

            var url = $(this).attr('action');
            var req = {
                url: encodeURIComponent($('#url').val())
            };

            $.get(url, req)
                .done(function (res) {
                    console.log(res);

                    // show message
                    window.alert(Number(res));
                    // form reset
                    $('#btn_count').removeAttr('disabled');
                    $('#btn_count').html('Count Again!');
                    $('#form_counter').trigger('reset');
                });
        });
    });
})(jQuery)