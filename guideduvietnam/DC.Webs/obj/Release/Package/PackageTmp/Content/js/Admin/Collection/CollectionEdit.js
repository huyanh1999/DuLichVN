function AddItems(name,url,orderby) {
    var self = this;
    self.Name = ko.observable(name).extend({ required: { message: "Không được để trống" } });
    self.Url = ko.observable(url).extend({ required: { message: "Nhập nội dung câu hỏi" } });
    self.OrderBy = ko.observable(orderby).extend({ number: { message: "Chỉ được nhập số" } });
}
var viewModel = new function () {
    var self = this;

    self.CollectionItems = ko.observableArray([]);
    // Thông tin chung
    self.Name = ko.observable(data.CollectionInfo.Name)
                    .extend({ required: { message: "Không được để trống" } })
                    .extend({ maxLength: 512 });    
    self.Url = ko.observable(data.CollectionInfo.Url);
    
    self.OrderBy = ko.observable(data.CollectionInfo.OrderBy).extend({ number: { message: "Chỉ được nhập số" } });

    $.each(data.CollectionItems, function () {
        self.CollectionItems.push(new AddItems(this.Name, this.Url, this.OrderBy));
    });


    self.AddCollectionItems = function () {       
        self.CollectionItems.push(new AddItems('','','1'));
    }

    self.DeleteItems = function (item) {
        var confirmdelete = confirm('Bạn chắc muốn xóa link này?');
        if (confirmdelete) {
            self.CollectionItems.remove(item);
        }

    }
    // Submit
    self.Submit = function () {
        document.getElementById("CollectionArray").value = ko.toJSON(viewModel.CollectionItems());
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
ko.applyBindings(viewModel, document.getElementById("editCollection"));