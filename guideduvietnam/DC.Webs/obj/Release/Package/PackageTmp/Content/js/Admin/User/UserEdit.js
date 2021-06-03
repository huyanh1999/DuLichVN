var viewModel = new function () {
    var self = this;
    // Thông tin chung
    self.UserId = ko.observable(data.UserInfo.Id);
    self.UserName = ko.observable(data.UserInfo.UserName);
    self.Password = ko.observable('').extend({ maxLength: 50 });
    self.Email = ko.observable(data.UserInfo.Email).extend({ maxLength: 150 });
    self.FirstName = ko.observable(data.UserInfo.FirstName);
    self.LastName = ko.observable(data.UserInfo.LastName);
    self.Description = ko.observable(data.UserInfo.Description);
    self.Phone = ko.observable(data.UserInfo.Phone);
    self.Description = ko.observable(data.UserInfo.Description);
    self.IsLockedOut = ko.observable(data.UserInfo.IsLockedOut != null && data.UserInfo.IsLockedOut === true ? "LOCK" : "UNLOCK");
    self.ReceiveEmailNotification = ko.observable(data.UserInfo.ReceiveEmailNotification != null ? data.UserInfo.ReceiveEmailNotification : false);
    self.ChangePass = ko.observable(false);
    self.HideShowChangePass = ko.computed(function () {
        if (self.ChangePass === true)
            return 1;
        else return 0;
    }, this);
    self.RoleName = ko.observable(data.UserInfo.RoleName);
    self.RoleOptions = ko.observableArray(data.RoleOptions != null ? data.RoleOptions : []);
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmEdit').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("editUser"));