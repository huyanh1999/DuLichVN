
var viewModel = new function () {
    var self = this;

    self.ItemId = ko.observable(0);
    self.ActivityLogItems = ko.observableArray([]);
    self.TypeId = ko.observable(0);
    self.ActivityLogTypeOptions = ko.observableArray(data.ActivityLogTypeOptions != null ? data.ActivityLogTypeOptions : []);
       
        
    // Paginate
    self.Query = ko.observable('');
    self.FromDate = ko.observable('');
    self.ToDate = ko.observable('');
    self.Status = ko.observable('');
    self.SortBy = ko.observable('CREATEDATE');
    self.OrderBy = ko.observable('DESC');
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
            return "Hiển thị kết quả từ " + startIndex + " - " + endIndex + " trên tổng " + viewModel.TotalCount();
        else
            return "Không có kết quả nào";
    }, this);

    function FilterData() {
        var dataPost = {
            "query": viewModel.Query(),
            "logeTypeId": viewModel.TypeId(),
            "from": $('#FromDate').val(),
            "to": $('#ToDate').val(),
            "sortBy": viewModel.SortBy(),
            "orderBy": viewModel.OrderBy(),
            "pageIndex": viewModel.PageIndex(),
            "pageSize": viewModel.PageSize()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Activities/GetList",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                // Stop progress
                hideProcess();

                // Set values
                viewModel.TotalCount(data.TotalCount);
                viewModel.TotalPages(data.TotalPages);
                viewModel.ActivityLogItems(data.ActivityLogItems == null ? [] : data.ActivityLogItems);
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

 
}

ko.applyBindings(viewModel, document.getElementById("Activity"));
//// Onload
// jQuery
$(document).ready(function () {
    // First Load
    viewModel.Search();
});