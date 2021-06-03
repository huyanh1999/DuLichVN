﻿
var viewModel = new function () {
    var self = this;


    // Thông tin chung
    self.Name = ko.observable('')
                    .extend({ required: { message: "Tên loại sản phẩm là bắt buộc" } })
                    .extend({ maxLength: 512 });
    self.Description = ko.observable('');

    self.OrderBy = ko.observable(1).extend({ number: { message: "Chỉ được nhập số" } });


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
ko.applyBindings(viewModel, document.getElementById("addProductType"));