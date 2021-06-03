var validateUrl = function (url, id) {
    var validated = true;

    if (url !== undefined && url != null && url != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: "/Admin/Product/ExistUrl",
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
    self.Name = ko.observable(data.PostInfo.Name)
                    .extend({ required: { message: "Tên bài viết là bắt buộc" } })
                    .extend({ maxLength: 512 });

    // category
    self.CategoryId = ko.observable(data.PostInfo.CateId);
    self.CategoryOptions = ko.observable(data.CategoryOptions != null ? data.CategoryOptions : []);

    //tags
    self.SuggestTags = ko.observableArray(data.TagsOptions != null ? data.TagsOptions : []);
    self.TagsId = ko.observable(data.PostInfo.TagsId);

    self.Title = ko.observable(data.PostInfo.Title)
                    .extend({ required: { message: "Title web là bắt buộc" } })
                    .extend({ maxLength: 500 });
    self.Url = ko.observable(data.PostInfo.Url)
                    .extend({ required: { message: "Slug url là bắt buộc" } })
                    .extend({ maxLength: 512 }).extend({
                        validation: {
                            validator: function (val) {
                                return validateUrl(val, data.PostInfo.Id);
                            },
                            message: "Đường dẫn/Alias đã tồn tại",
                            params: self.Url
                        }
                    });
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
    self.Description = ko.observable(data.PostInfo.Description);
    self.MetaKeyword = ko.observable(data.PostInfo.MetaKeyword).extend({ maxLength: 160 });
    self.MetaKeywordsLength = ko.computed(function () {
        return self.MetaKeyword() != null ? self.MetaKeyword().length : 0;
    }, this);
    self.MetaDescription = ko.observable(data.PostInfo.MetaDescription).extend({ maxLength: 250 });
    self.MetaDescriptionLength = ko.computed(function () {
        return self.MetaDescription() != null ? self.MetaDescription().length : 0;
    }, this);
    self.Content = ko.observable(data.PostInfo.Content);
    self.OverView = ko.observable(data.PostInfo.OverView);
    self.Target = ko.observable(data.PostInfo.Target);
    //status
    self.StatusId = ko.observable(data.PostInfo.Status);
    self.StatusOptions = ko.observable(data.StatusOptions != null ? data.StatusOptions : []);

    self.PictureItems = ko.observableArray(data.PostImageItems != null ? data.PostImageItems : []);
    self.removePictureItem = function (pictureItem) {
        self.PictureItems.remove(pictureItem);
    };

    self.TagNameRemove = ko.observable('');
    self.SelectTag = function (item) {
        if (!item.Selected) {
            $('#TagId').addTag(item.Text);
            $('.tag-' + item.Value).addClass('disabled');
            item.Selected = true;
        }

    }
    self.UnSelectTag = ko.computed(function () {
        var tag = self.TagNameRemove();
        $.each(self.SuggestTags(), function () {
            var tagname = this.Text;
            if (tag === tagname) {
                this.Selected = false;
                $('.tag-' + this.Value).removeClass('disabled');
            }
        });
        return 1;
    }, this);


    // Submit
    self.Submit = function () {
        if (viewModel.errors().length === 0) {
            $('#frmAdd').submit();
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("editProduct"));

$(document).ready(function () {

    $('#fileuploads').fileupload({
        dataType: 'json',
        url: '/Admin/Attachments/AddFileLocation',
        autoUpload: true,
        done: function (e, data) {
            if (data.result != null) {
                for (var i = 0; i < data.result.length; i++) {
                    viewModel.PictureItems.push({
                        Url: data.result[i].Url,
                        Name: data.result[i].Name
                    });
                }
            }
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('.progress .progress-bar').css('width', progress + '%');
    });

});