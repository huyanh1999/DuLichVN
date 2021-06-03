var viewModel = new function () {
    var self = this;

    self.ItemId = ko.observable(0);
    //status   
    self.TypeOptions = ko.observableArray(data.TypeOptions != null ? data.TypeOptions : []);

    self.CategoryItems = ko.observableArray([]);

    // Paginate
    self.Query = ko.observable('');
    self.Type = ko.observable('');
    self.SortBy = ko.observable('ORDERBY');
    self.OrderBy = ko.observable('ASC');
    self.PageIndex = ko.observable(1);
    self.PageSize = ko.observable(10);
    self.TotalPages = ko.observable(0);
    self.TotalCount = ko.observable(0);
    self.PageSlide = ko.observable(4);

    self.LastPage = ko.computed(function () {
        return Math.floor((self.TotalCount() - 1) / self.PageSize()) + 1;
    });
    self.Pages = ko.computed(function () {
        var pageCount = self.LastPage();
        var pageFrom = Math.max(1, self.PageIndex() - self.PageSlide());
        var pageTo = Math.min(pageCount, self.PageIndex() + self.PageSlide());
        pageFrom = Math.max(1, Math.min(pageTo - 2 * self.PageSlide(), pageFrom));
        pageTo = Math.min(pageCount, Math.max(pageFrom + 2 * self.PageSlide(), pageTo));

        var result = [];
        for (var i = pageFrom; i <= pageTo; i++) {
            result.push({ pageNumber: (i) });
        }
        return result;
    });
    // Prev | Next page
    self.PrevPage = function () {
        var pageIndex = viewModel.PageIndex();
        if (pageIndex > 1) {
            viewModel.PageIndex(pageIndex - 1);
            viewModel.Search();
        }
    };
    self.NextPage = function () {
        var pageIndex = viewModel.PageIndex();
        if (pageIndex < viewModel.TotalPages()) {
            viewModel.PageIndex(pageIndex + 1);
            viewModel.Search();
        }
    };
    self.MoveToPage = function (index) {
        self.PageIndex(index);
        viewModel.Search();
    };
    // Showing
    self.Message = ko.computed(function () {
        var startIndex = (self.PageIndex() - 1) * self.PageSize() + 1;
        var endIndex = self.PageIndex() * self.PageSize() <= self.TotalCount() ?
                        self.PageIndex() * self.PageSize() : self.TotalCount();

        if (self.TotalCount() != 0)
            return "Results from " + startIndex + " - " + endIndex + " on total " + viewModel.TotalCount();
        else
            return "No results";
    }, this);


    // Filter Data
    function FilterData() {
        openProcess();
        var dataPost = {
            "query": self.Query(),
            "type": self.Type(),
            "sortBy": self.SortBy(),
            "orderBy": self.OrderBy(),
            "pageIndex": self.PageIndex(),
            "pageSize": self.PageSize()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Category/GetList",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values
                viewModel.TotalCount(data.TotalCount);
                viewModel.TotalPages(data.TotalPages);
                viewModel.CategoryItems(data.CategoryItems == null ? [] : data.CategoryItems);
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
        self.PageIndex(1);
        FilterData();
    };

    self.MessageDelete = ko.observable('');

    self.DeleteItems = function (item) {
        viewModel.ItemId(item.Id);
        $('#popupDelete').modal('show');
        self.MessageDelete('Bạn chắc muốn xóa danh mục này?');
    };



    self.DeleteConfirmed = function () {
        openProcess();
        var dataPost = { "id": viewModel.ItemId() };
        $.ajax({
            type: "POST",
            url: "/Admin/Category/Delete",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                $('#popupDelete').modal('hide');
                if (data === 'ERROR') {
                    showPopupError();
                } else {
                    viewModel.PageIndex(1);
                    viewModel.Search();
                }
            },
            error: function (xhr, status, error) {
                hideProcess();
                showPopupError();
            }
        });
    };
}

ko.applyBindings(viewModel, document.getElementById("cateList"));
//// Onload
// jQuery
$(document).ready(function () {
    // First Load
    viewModel.Search();
});