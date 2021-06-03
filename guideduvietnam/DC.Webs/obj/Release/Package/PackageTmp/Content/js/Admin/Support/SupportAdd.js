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


    self.Title = ko.observable('')
                    .extend({ required: { message: "Không được để trống" } })
                    .extend({ maxLength: 500 });
    self.NickName = ko.observable('')
                    .extend({ required: { message: "Không được để trống" } })
                    .extend({ maxLength: 250 });
    self.Phone = ko.observable('')
                    .extend({ required: { message: "Không được để trống" } })
                    .extend({ maxLength: 50 });

    //self.Picture = ko.observable('')
    //                .extend({ maxLength: 500 }).extend({
    //                    validation: {
    //                        validator: function (val) {
    //                            return validateTypeImage(val);
    //                        },
    //                        message: "Ảnh không đúng định dạng",
    //                        params: self.Picture
    //                    }
    //                }).extend({
    //                    validation: {
    //                        validator: function () {
    //                            return validateImageSize();
    //                        },
    //                        message: "Kích thước ảnh phải nhỏ hơn 1MB",
    //                        params: self.Picture
    //                    }
    //                });

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
ko.applyBindings(viewModel, document.getElementById("addSupport"));