var validateUrl = function (url, id) {
    var validated = true;

    if (url !== undefined && url != null && url != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/Tags/ExistUrl",
            data: {
                "id": id,
                "url": url
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
    self.Name = ko.observable('')
                    .extend({ required: { message: "Tên tags là bắt buộc" } })
                    .extend({ maxLength: 512 });
    self.Title = ko.observable('')
                    .extend({ required: { message: "Title web là bắt buộc" } })
                    .extend({ maxLength: 500 });
    self.Url = ko.observable('')
                    .extend({ required: { message: "Slug url là bắt buộc" } })
                    .extend({ maxLength: 512 }).extend({
                        validation: {
                            validator: function (val) {
                                return validateUrl(val, 0);
                            },
                            message: "Đường dẫn/Alias đã tồn tại",
                            params: self.Url
                        }
                    });;
    self.MetaKeyword = ko.observable('').extend({ maxLength: 160 });
    self.MetaKeywordsLength = ko.computed(function () {
        return self.MetaKeyword() != null ? self.MetaKeyword().length : 0;
    }, this);
    self.MetaDescription = ko.observable('').extend({ maxLength: 250 });
    self.MetaDescriptionLength = ko.computed(function () {
        return self.MetaDescription() != null ? self.MetaDescription().length : 0;
    }, this);

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
ko.applyBindings(viewModel, document.getElementById("addTags"));