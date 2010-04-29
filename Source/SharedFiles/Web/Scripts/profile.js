var profile =
{
    init: function (validationErrorIcon) {

        var options = {
            dataType: 'json',
            beforeSubmit: function () {
                $('div.validationBox').hide();
                $('div.messageBox').hide();
                $('form#generate input.largeButton').attr('disabled', true).val('Wait...');
            },
            success: function (result) {
                $('form#generate input.largeButton').attr('disabled', false).val('Reset Api Key');
                if (result.model != null) {
                    $('#apiKey').val(result.model.apiKey);
                }
                else if (result.modelStates != null) {
                    showErrorBox('Please correct the following errors and try again.', result.modelStates);
                }
            },
            error: function (xml, status, e) {
                $('form#generate input.largeButton').attr('disabled', false).val('Reset Api Key');
                var modalState = {
                    key: 'form',
                    errors: []
                };

                modalState.errors[0] = xml.statusText;

                showErrorBox('An unexpected error has occurred, please try again.', new Array(modalState));
            }
        };

        function showErrorBox(message, modelStates) {
            var html = '<span class=\"validation-summary-errors\">' + message + '</span>' +
                       '<ul class="validation-summary-errors">';

            for (var i = 0; i < modelStates.length; i++) {
                var element = $('#' + modelStates[i].key);
                var errorIcon = buildErrorIcon(modelStates[i].errors[0]);

                element.addClass('input-validation-error');
                element.next('span:first').addClass('field-validation-error').html(errorIcon).fadeIn('slow');

                for (var j = 0; j < modelStates[i].errors.length; j++) {
                    html += '<li><label for=\"' + modelStates[i].key + '\">' + modelStates[i].errors[j] + '</label></li>';
                }
            }

            html += '</ul>';

            $('div.validationBox').html(html)
                                  .css({ backgroundColor: '#e8e8e8' })
                                  .animate({ opacity: 'show' }, 'slow')
                                  .animate({ backgroundColor: '#f7cbca' }, 'slow');
        }

        $('#generate').ajaxForm(options);

        function buildErrorIcon(errorText) {
            return '<img src=\"' + validationErrorIcon + '\" alt=\"\" title=\"' + errorText + '\"/>';
        }
    }
}