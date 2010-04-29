var createShortUrl =
{
    init: function (validationErrorIcon) {
        $('input.largeTextBox[name=url]').watermark();
        $('input.largeTextBox[name=alias]').watermark();

        $.validator.addMethod(
                                'validAlias',
                                function (value, element) {
                                    return this.optional(element) || /^[a-zA-Z0-9]+$/.test(value);
                                },
                                'Alias is not valid, alias can only contain alphanumeric characters.'
                            );

        $('#create').validate(
                                {
                                    rules: {
                                        url: { required: true, url: true },
                                        alias: { validAlias: true }
                                    },
                                    messages: {
                                        url: { required: 'Url cannot be blank.', url: 'Url is not in valid format.' }
                                    },
                                    submitHandler: function (form) {
                                        var options = {
                                            dataType: 'json',
                                            beforeSubmit: function () {
                                                $('div.validationBox').hide();
                                                $('div.messageBox').hide();
                                                $('ul.form input').attr('disabled', true);
                                                $('ul.form input.largeButton').val('Wait...');
                                            },
                                            success: function (result) {

                                                $('ul.form input').attr('disabled', false);
                                                $('ul.form input.largeButton').val('Shrink');

                                                if (result.model != null) {
                                                    showMessageBox(result.model);
                                                    $('input.largeTextBox[name=url],input.largeTextBox[name=alias]').val('');
                                                }
                                                else if (result.modelStates != null) {
                                                    showErrorBox('Please correct the following errors and try again.', result.modelStates);
                                                }
                                            },
                                            error: function (xml, status, e) {

                                                $('ul.form input').attr('disabled', false);
                                                $('ul.form input.largeButton').val('Shrink');

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

        function showMessageBox(shortUrl) {
            function replaceText(text, oldText, newText) {
                var replacedText = text.split(oldText);

                replacedText = replacedText.join(newText);

                return replacedText;
            }

            var template = '<span>Your new urls are generated successfully.</span>' +
                            '<ul>' +
                                '<li>' +
                                    '<div>Your shrinked urls are:</div>' +
                                    '<div><label for=\"newUrl\">New:</label><input id=\"newUrl\" type=\"text\" value=\"{visitUrl}\" readonly=\"readonly\" class=\"largeTextBox readOnlyTextBox\"/> ({visitUrlLength} characters) <a href=\"{visitUrl}\" target=\"_blank\">[opens in new window]</a></div>' +
                                    '<div><label for=\"previewUrl\">Preview:</label><input id=\"previewUrl\" type=\"text\" value=\"{previewUrl}\" readonly=\"readonly\" class=\"largeTextBox readOnlyTextBox\"/> <a href=\"{previewUrl}\" target=\"_blank\">[opens in new window]</a></div>' +
                                '</li>' +
                                '<li>Your original url was {url} ({urlLength} characters) <a href=\"{url}\" target=\"_blank\">[opens in new window]</a>.</li>' +
                                '<li>We have made your url <strong>{reducedPercent}%</strong> (<strong>{reducedCharacters}</strong> characters) <strong>{type}</strong>.</li>' +
                            '</ul>';

            var type = shortUrl.hasReduced ? 'shorter' : 'larger';

            var html = template;

            html = replaceText(html, '{type}', type);
            html = replaceText(html, '{reducedCharacters}', shortUrl.reducedCharacters);
            html = replaceText(html, '{reducedPercent}', shortUrl.reducedPercent);
            html = replaceText(html, '{urlLength}', shortUrl.url.length.toString());
            html = replaceText(html, '{url}', shortUrl.url);
            html = replaceText(html, '{previewUrl}', shortUrl.previewUrl);
            html = replaceText(html, '{visitUrlLength}', shortUrl.visitUrl.length.toString());
            html = replaceText(html, '{visitUrl}', shortUrl.visitUrl);

            $('div.messageBox').html(html)
                               .css({ backgroundColor: '#e8e8e8' })
                               .animate({ opacity: 'show' }, 'slow')
                               .animate({ backgroundColor: '#c9ffca' }, 'slow')
                               .find('input.largeTextBox').focus(function () { this.select(); });
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

            // For some unknown reason the alias error message is not
            // populated in error text so hard coding it over here.
            if (element.is('input.largeTextBox[name=alias]')) {
                errorText = 'Alias is not valid, alias can only contain alphanumeric characters.';
            }

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
}