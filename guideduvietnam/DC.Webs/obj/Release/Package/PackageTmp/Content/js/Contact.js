
var viewModel = new function () {
    var self = this;
    var language = data.Language;
    // Thông tin chung
    self.Name = ko.observable('')
                    .extend({ required: { message: language == 'en' ? 'required' : 'requis' } })
                    .extend({ maxLength: 50 });
    self.Email = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } }).extend({
        pattern: {
            message: language == 'en' ? 'invalid email' : 'email invalide',
            params: EMAILADDRESS_EXPRESSION
        }
    });
    self.Phone = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
    self.Content = ko.observable('')
                    .extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
    self.Message = ko.observable('');
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmContact').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("frmContact"));