var viewModel = new function () {
    var self = this;
    // Thông tin chung
    self.COMPANYNAME = ko.observable(data.ParameterInfo.CompanyName);
    self.ADDRESS = ko.observable(data.ParameterInfo.Address);
    self.EMAIL = ko.observable(data.ParameterInfo.Email);
    self.PHONE = ko.observable(data.ParameterInfo.Phone);
    self.BUSINESSLICENSE = ko.observable(data.ParameterInfo.BusinessLicense);
    self.GMAP = ko.observable(data.ParameterInfo.Gmap);
    self.FOOTER = ko.observable(data.ParameterInfo.Footer);
    self.ABOUT = ko.observable(data.ParameterInfo.About);
    self.ABOUTHOME = ko.observable(data.ParameterInfo.AboutHome);
    self.FRMCONTACT = ko.observable(data.ParameterInfo.FrmContact);
    self.CUSTOMIZETOUR = ko.observable(data.ParameterInfo.CustomizeTour);
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmCompanyInfo').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("CompanyInfo"));