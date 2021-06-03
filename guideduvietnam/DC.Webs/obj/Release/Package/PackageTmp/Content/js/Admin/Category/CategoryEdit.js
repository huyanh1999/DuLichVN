var validateUrl = function (url, id) {
    var validated = true;

    if (url !== undefined && url != null && url != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/Category/ExistUrl",
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
    self.ItemId = ko.observable(data.CategoryInfo.Id);

    // Thông tin chung
    self.Name = ko.observable(data.CategoryInfo.Name)
                    .extend({ required: { message: "Tên danh mục là bắt buộc" } })
                    .extend({ maxLength: 512 });
    self.ParentId = ko.observable(data.CategoryInfo.ParentId);
    self.ParentItems = ko.observable(data.ParentItems != null ? data.ParentItems : []);
    self.TypeId = ko.observable(data.CategoryInfo.CateType);
    self.TypeOptions = ko.observable(data.TypeOptions != null ? data.TypeOptions : []);
    self.Title = ko.observable(data.CategoryInfo.Title)
                    .extend({ required: { message: "Title web là bắt buộc" } })
                    .extend({ maxLength: 500 });
    self.Url = ko.observable(data.CategoryInfo.Url)
                    .extend({ required: { message: "Slug url là bắt buộc" } })
                    .extend({ maxLength: 512 }).extend({
                        validation: {
                            validator: function (val, otherValue) {
                                return validateUrl(val, otherValue);
                            },
                            message: "Đường dẫn/Alias đã tồn tại",
                            params: self.ItemId
                        }
                    });;
    self.MetaKeyword = ko.observable(data.CategoryInfo.MetaKeyword).extend({ maxLength: 160 });
    self.MetaKeywordsLength = ko.computed(function () {
        return self.MetaKeyword() != null ? self.MetaKeyword().length : 0;
    }, this);
    self.MetaDescription = ko.observable(data.CategoryInfo.MetaDescription).extend({ maxLength: 250 });
    self.MetaDescriptionLength = ko.computed(function () {
        return self.MetaDescription() != null ? self.MetaDescription().length : 0;
    }, this);
    self.OrderBy = ko.observable(data.CategoryInfo.OrderBy).extend({ number: { message: "Chỉ được nhập số" } });

    self.UserView = ko.observable(data.CategoryInfo.UserView);

    self.TypeId.subscribe(function () {
        openProcess();
        var dataPost = {
            "type": self.TypeId()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Category/GetParent",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values
                self.ParentItems(data.ParentItems == null ? [] : data.ParentItems);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    });
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
ko.applyBindings(viewModel, document.getElementById("editCategory"));