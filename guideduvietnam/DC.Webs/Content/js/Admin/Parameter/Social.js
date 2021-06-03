var viewModel = new function () {
    var self = this;
    // Thông tin chung    
    self.TWITTER = ko.observable(data.ParameterInfo.Twitter);
    self.FACEBOOK = ko.observable(data.ParameterInfo.Facebook);
    self.GOOGLEPLUS = ko.observable(data.ParameterInfo.GooglePlus);
    self.YOUTUBE = ko.observable(data.ParameterInfo.Youtube);
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmSocial').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("Social"));