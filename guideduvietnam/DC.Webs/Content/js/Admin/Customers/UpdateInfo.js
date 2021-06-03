var validateEmail = function (email, id) {
    var validated = true;

    if (email !== undefined && email != null && email != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/Customer/ExistEmail",
            data: {
                "id": id,
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

var validateTypeImage = function (image) {
    var validated = true;

    if (image !== undefined && image != null && image != "") {
        var extension = image.substring(image.lastIndexOf('.'));

        // Only process image files.
        var validFileType = ".jpg , .png , .gif";
        if (validFileType.toLowerCase().indexOf(extension.toLowerCase()) < 0) {
            validated = false;
        }
    }
    return validated;
};

var validateImageSize = function () {
    var validated = true;
    var fileinput = document.getElementById("Picture");
    if (fileinput.files.length > 0) {
        var sfilezise = fileinput.files[0].size;
        if (sfilezise > 1048576) { //
            validated = false;
        }
    }
    return validated;
};
var viewModel = new function () {
    var self = this;


    // Thông tin chung
    self.FirstName = ko.observable(data.CustomerInfo.FirstName)
                    .extend({ required: { message: "Họ là bắt buộc" } })
                    .extend({ maxLength: 50 });
    self.LastName = ko.observable(data.CustomerInfo.LastName)
                    .extend({ required: { message: "Tên là bắt buộc" } })
                    .extend({ maxLength: 250 });
    self.ServiceName = ko.observable(data.CustomerInfo.ServiceName)                    
                    .extend({ maxLength: 250 });    
    self.Phone = ko.observable(data.CustomerInfo.Phone)                   
                    .extend({ maxLength: 50 });
    self.Email = ko.observable(data.CustomerInfo.Email)
                    .extend({ required: { message: "Email là bắt buộc" } })
                    .extend({ maxLength: 150 }).extend({
                        validation: {
                            validator: function (val) {
                                return validateEmail(val, data.CustomerInfo.Id);
                            },
                            message: "Email này đã tồn tại",
                            params: self.Email
                        }
                    });
    self.Address = ko.observable(data.CustomerInfo.Address).extend({ maxLength: 500 });    
    
    // category
    self.GenderId = ko.observable(data.CustomerInfo.Gender);
    self.GenderOptions = ko.observable(data.GenderOptions != null ? data.GenderOptions : []);
    self.Picture = ko.observable('').extend({ maxLength: 500 }).extend({
        validation: {
            validator: function (val) {
                return validateTypeImage(val);
            },
            message: "Ảnh không đúng định dạng",
            params: self.Picture
        }
    }).extend({
        validation: {
            validator: function () {
                return validateImageSize();
            },
            message: "Kích thước ảnh phải nhỏ hơn 1MB",
            params: self.Picture
        }
    });
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
ko.applyBindings(viewModel, document.getElementById("UpdateInfo"));