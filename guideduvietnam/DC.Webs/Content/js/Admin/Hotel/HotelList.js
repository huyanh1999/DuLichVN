var viewModel = new function () {
    var self = this;

    self.ItemId = ko.observable(0);   
    self.HotelItems = ko.observableArray([]);
    var itemInfo = data.HotelInfo;
    // Paginate
    self.Query = ko.observable('');   

    // Filter Data
    function FilterData() {
        openProcess();
        var dataPost = {
            "query": viewModel.Query()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Hotel/GetList",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values
                viewModel.HotelItems(data.HotelItems == null ? [] : data.HotelItems);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    }
    // Search
    self.Search = function () {
        FilterData();
    };
    self.ActionSearch = function () {       
        FilterData();
    };

    self.MessageDelete = ko.observable('');
    self.DeleteItems = function (item) {
        viewModel.ItemId(item.Id);
        self.MessageDelete('Bạn chắc muốn xóa Hotel "' + item.Name + '"?');
        $('#popupDelete').modal('show');

    };

    self.DeleteConfirmed = function () {
        var dataPost = { "id": viewModel.ItemId() };
        $.ajax({
            type: "POST",
            url: "/Admin/Hotel/Delete",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                $('#popupDelete').modal('hide');                
                viewModel.Search();
            },
            error: function (xhr, status, error) {
                showPopupError();
            }
        });
    };

    self.Mode = ko.observable('CREATE');
    self.HotelName = ko.observable('')
                    .extend({ required: { message: "Tên khách sạn là bắt buộc" } })
                    .extend({ maxLength: 512 });
    self.OrderBy = ko.observable(1).extend({ number: { message: "Chỉ được nhập số" } });

    self.AddItems = function () {
        self.ItemId('0');
        self.Mode('CREATE');
        self.HotelName('');
        self.OrderBy('1');
        $('#popupHotel').modal('show');
    }
    self.EditItems=function(item) {
        self.ItemId(item.Id);
        self.HotelName(item.Name);
        self.OrderBy(item.OrderBy);
        self.Mode('UPDATE');
        $('#popupHotel').modal('show');
    }

    // Submit
    self.Submit = function () {
        if (viewModel.errors().length === 0) {
            itemInfo.Id = self.ItemId();
            itemInfo.Name = self.HotelName();
            itemInfo.OrderBy = self.OrderBy();
            // Submit Form
            openProcess();
            $.ajax({
                async: false,
                type: "POST",
                url: "/Admin/Hotel/Update",
                data: JSON.stringify({ item: itemInfo }),
                dataType: "json",
                contentType: 'application/json; charset=UTF-8',
                success: function (data, textStatus, jqXHR) {
                    hideProcess();
                    $('#popupHotel').modal('hide');                   
                    viewModel.Search();
                },
                error: function (xhr, status, error) {
                    hideProcess();
                    showPopupError();
                }
            });
        }
        else {
            viewModel.errors.showAllMessages();
        }
    }
}

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("hotelList"));
//// Onload
// jQuery
$(document).ready(function () {
    // First Load
    viewModel.Search();
});
