function administrativeItem(validationErrorIcon, validationRules, validationMessages, newItemCallback) {

    $('input.smallTextBox').watermark();

    $('form.delete').each(function () {
        setupDeleteForm($(this));
    });

    $('#create').validate(
                                {
                                    rules: validationRules,
                                    messages: validationMessages,
                                    submitHandler: function (form) {
                                        var options = {
                                            dataType: 'json',
                                            beforeSubmit: function () {
                                                $('div.validationBox').hide();
                                            },
                                            success: function (result) {
                                                if (result.model != null) {
                                                    $('input.smallTextBox').val('').get(0).focus();

                                                    var rawHtml = newItemCallback(result.model);

                                                    var newItem = $('table#list>tbody').prepend(rawHtml).find('tr:first');

                                                    setupDeleteForm(newItem.find('form.delete'));
                                                }
                                                else if (result.modelStates != null) {
                                                    showErrorBox('Please correct the following errors and try again.', result.modelStates);
                                                }
                                            },
                                            error: function (xml, status, e) {
                                                var modalState = {
                                                    key: 'form',
                                                    errors: []
                                                };

                                                modalState.errors[0] = xml.statusText;

                                                showErrorBox('An unexpected error has occurred, please try again.', new Array(modalState));
                                            }
                                        };

                                        $(form).ajaxSubmit(options);
                                        return false;
                                    },
                                    errorClass: 'input-validation-error',
                                    errorPlacement: onErrorPlacement,
                                    highlight: onHighlight,
                                    unhighlight: onUnhighlight
                                }
                            );

    function setupDeleteForm(form) {
        form.submit(function () {
            var options = {
                dataType: 'json',
                success: function (result) {
                    if (result.modelStates != null) {
                        showErrorBox('Please correct the following errors and try again.', result.modelStates);
                    }
                    else {
                        form.parents('tr:first').remove();
                    }
                },
                error: function (xml, status, e) {
                    var modalState = {
                        key: 'form',
                        errors: []
                    };

                    modalState.errors[0] = xml.statusText;

                    showErrorBox('An unexpected error has occurred, please try again.', new Array(modalState));
                }
            };

            form.ajaxSubmit(options);
            return false;
        });
    }

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

    function onErrorPlacement(error, element) {
        var errorText = error.text();

        var html = buildErrorIcon(errorText);

        element.next('span:first').html(html);
    }

    function onHighlight(element, cssClass) {
        $(element).addClass(cssClass).next('span:first').show();
    }

    function onUnhighlight(element, cssClass) {
        $(element).removeClass(cssClass).next('span:first').hide();
    }

    function buildErrorIcon(errorText) {
        return '<img src=\"' + validationErrorIcon + '\" alt=\"\" title=\"' + errorText + '\"/>';
    }
}