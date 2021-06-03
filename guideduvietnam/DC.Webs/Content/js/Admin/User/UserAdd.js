var validateEmail = function (email) {
    var validated = true;

    if (email !== undefined && email != null && email != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/User/ExistEmail",
            data: {
                "email": email
            },
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                // Stop progress
                if (data != "")
                    validated = false;
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                showPopupError();
            }
        });
    }

    return validated;
};
var validateUsername = function (username) {
    var validated = true;

    if (username !== undefined && username != null && username != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/User/ExistUsername",
            data: {
                "username": username
            },
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                // Stop progress
                if (data != "")
                    validated = false;
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                showPopupError();
            }
        });
    }

    return validated;
};
var viewModel = new function () {
    var self = this;


    // Thông tin chung
    self.FirstName = ko.observable('')
                    .extend({ required: { message: "Họ là bắt buộc" } })
                    .extend({ maxLength: 50 });
    self.LastName = ko.observable('')
                    .extend({ required: { message: "Tên là bắt buộc" } })
                    .extend({ maxLength: 50 });
    self.Email = ko.observable('')
                    .extend({ maxLength: 150 });
    self.UserName = ko.observable('')
                    .extend({ maxLength: 50 })
                    .extend({ required: { message: "Tên đăng nhập là bắt buộc" } })
                    .extend({
                        validation: {
                            validator: function (val) {
                                return validateUsername(val);
                            },
                            message: "Tên đăng nhập đã tồn tại"
                        }
                    });
    self.Password = ko.observable('')
                    .extend({ maxLength: 50 })
                    .extend({ required: { message: "Mật khẩu là bắt buộc" } });
    self.Phone = ko.observable('');
    self.Url = ko.observable('');
    self.Description = ko.observable('');
    self.IsLockedOut = ko.observable(false);
    self.ReceiveEmailNotification = ko.observable(false);


    self.RoleName = ko.observable('');
    self.RoleOptions = ko.observableArray(data.RoleOptions != null ? data.RoleOptions : []);


    // Submit
    self.Submit = function () {
        if (viewModel.errors().length == 0) {
            $('#frmAdd').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("addUser"));