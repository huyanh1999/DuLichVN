var viewModel = new function () {
    var self = this;

    // Attrs
    self.NewPassword = ko.observable('')
        .extend({ minLength: 8, maxLength: 20, required: { message: "Mật khẩu là bắt buộc" } });
        //.extend({
        //    pattern: {
        //        message: 'Mật khẩu nhập tối thiểu 8 ký tự, gồm chữ hoa, thường, số, ký tự đặc biệt',
        //        params: PASSWORD_EXPRESSION
        //    }
        //});
    self.ConfirmPassword = ko.observable('')
        .extend({ required: { message: "Xác nhận mật khẩu mới là bắt buộc" } })
        .extend({
            validation: {
                validator: function (val, someOtherVal) {
                    return val === someOtherVal();
                },
                message: "Mật khẩu không trùng nhau",
                params: self.NewPassword
            }
        });

    // Submit, Next button
    self.Submit = function () {
        if (viewModel.errors().length === 0) {
            $('#frmChangePasswordSettings').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("ChangePasswordSettings"));