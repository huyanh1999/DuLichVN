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
    var fileinput = document.getElementById("Images");
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
    
    self.Name = ko.observable('')
                    .extend({ required: { message: "Tên slide là bắt buộc" } })
                    .extend({ maxLength: 500 });
    self.Images = ko.observable('')
                    .extend({ required: { message: "Vui lòng chọn ảnh slide" } })
                    .extend({ maxLength: 250 }).extend({
                        validation: {
                            validator: function (val) {
                                return validateTypeImage(val);
                            },
                            message: "Ảnh không đúng định dạng",
                            params: self.Images
                        }
                    }).extend({
                        validation: {
                            validator: function () {
                                return validateImageSize();
                            },
                            message: "Kích thước ảnh phải nhỏ hơn 1MB",
                            params: self.Images
                        }
                    });
    self.Url = ko.observable('').extend({ maxLength: 500 });
    self.OrderBy = ko.observable(1).extend({ number: { message: "Chỉ được nhập số" } });    
    self.Description = ko.observable('');    

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
ko.applyBindings(viewModel, document.getElementById("addSlider"));