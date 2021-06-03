var viewModel = new function () {
    var self = this;
    // Thông tin chung    
    self.METATITLE = ko.observable(data.ParameterInfo.MetaTitle);
    self.METAKEYWORD = ko.observable(data.ParameterInfo.MetaKeyword);
    self.METADESCRIPTION = ko.observable(data.ParameterInfo.MetaDescription);
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmSettingSeo').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("SettingSeo"));