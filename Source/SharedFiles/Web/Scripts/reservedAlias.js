var reservedAlias =
{
    init: function (validationErrorIcon, deleteUrl) {

        var rules = { aliasName: { required: true, rangelength: [1, 440]} };
        var messages = { aliasName: { required: 'Alias cannot be blank.', rangelength: 'Alias must be 1-440 character long.'} };

        var newItem = function (model) {
            return '<tr>' +
                        '<td scope=\"col\">' +
                            model.name +
                         '</td>' +
                         '<td scope=\"col\">' +
                            '<form method=\"post\" class=\"delete\" action=\"' + deleteUrl + '\">' +
                                '<div>' +
                                    ' <input type=\"hidden\" id=\"id-' + model.id + '\" name=\"id\" value=\"' + model.id + '\"/>' +
                                    ' <input type=\"submit\" class=\"smallButton\" value=\"remove\"/>' +
                                '</div>' +
                            '</form>' +
                        '</td>' +
                    '</tr>';
        };

        new administrativeItem(validationErrorIcon, rules, messages, newItem);
    }
}