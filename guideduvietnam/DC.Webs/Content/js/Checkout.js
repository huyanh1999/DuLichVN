var viewModel = new function () {
    var self = this;
    // Thông tin chung
    self.PaymentId = ko.observable('');
    self.PaymentOptions = ko.observableArray(data.PaymentOptions != null ? data.PaymentOptions : []);

    self.DeliveryId = ko.observable('');
    self.DeliveryOptions = ko.observableArray(data.DeliveryOptions != null ? data.DeliveryOptions : []);
    self.Name = ko.observable('')
                    .extend({ required: { message: "Vui lòng nhập họ tên" } })
                    .extend({ maxLength: 50 });
    
    self.Email = ko.observable('').extend({ required: { message: "Vui lòng nhập email" } }).extend({
        pattern: {
            message: 'Email không hợp lệ',
            params: EMAILADDRESS_EXPRESSION
        }
    });
    self.Phone = ko.observable('').extend({ required: { message: "Vui lòng nhập điện thoại" } });
    self.Address = ko.observable('')
                    .extend({ required: { message: "Vui lòng nhập địa chỉ" } })
                    .extend({ maxLength: 500 });
    self.DateShip = ko.observable(data.TabName).extend({ maxLength: 20 });
    self.Comment = ko.observable('');
    // Submit
    self.Submit = function () {
        if (viewModel.errors().length === 0) {
            $('#frmCheckout').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("frmCheckout"));