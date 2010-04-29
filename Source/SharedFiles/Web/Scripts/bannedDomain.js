var bannedDomain =
{
    init: function (validationErrorIcon, deleteUrl) {

        var rules = { name: { required: true, rangelength: [4, 440] } };
        var messages = { name: { required: 'Name cannot be blank.', rangelength: 'Name must be 4-440 character long.'} };

        var newItem = function (model) {
            return '<tr>' +
                        '<td scope=\"col\">' +
                            '<a href=\"http://' + model.name + '\">' + model.name + '</a>' +
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