function AddCollectionItems(id,name, url,orderby,childItems,productItems) {
    var self = this;
    self.Id = ko.observable(id);
    self.Name = ko.observable(name);
    self.Url = ko.observable(url);
    self.OrderBy = ko.observable(orderby);
    self.Href = '/Admin/Collection/Edit/' + id;
    self.CollectionChildItems = ko.observableArray(childItems != null ? childItems : []);
    self.ProductItems = ko.observableArray([]);
    $.each(productItems, function () {
        self.ProductItems.push(new AddProductItems(this.Id, this.Name, this.Images,id));
    });
    self.DeleteProductItems = function (item){       
        var confirmdelete = confirm('Bạn chắc muốn xóa sản phẩm này khỏi box?');
        if (confirmdelete) {
            var dataPost = { "productId": item.Id(), "collectionId": item.CollectionId() };
            $.ajax({
                type: "POST",
                url: "/Admin/Collection/DeleteProductToCollection",
                data: dataPost,
                dataType: "json",
                success: function (data, textStatus, jqXHR) {
                    viewModel.Search();
                },
                error: function (xhr, status, error) {
                    showPopupError();
                }
            });

        }

    }
}

function AddProductItems(id, name, image, collectionId) {
    var self = this;
    self.Id = ko.observable(id);
    self.Name = ko.observable(name);
    self.Images = ko.observable(image);
    self.CollectionId = ko.observable(collectionId);
    
}


var viewModel = new function () {
    var self = this;

    self.ItemId = ko.observable(0);    
    self.CollectionItems = ko.observableArray([]);
    self.ProductSearchItems = ko.observableArray([]);

    // Paginate
    self.Query = ko.observable('');
    self.TotalCount = ko.observable(0);

    self.PageIndex = ko.observable(1);
    self.PageSize = ko.observable(10);
    self.TotalPages = ko.observable(0);
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
            SearchProduct();
        }
    };
    self.NextPage = function () {
        var pageIndex = viewModel.PageIndex();
        if (pageIndex < viewModel.TotalPages()) {
            viewModel.PageIndex(pageIndex + 1);
            SearchProduct();
        }
    };
    self.MoveToPage = function (index) {
        self.PageIndex(index);
        SearchProduct();
    };

    self.Message = ko.computed(function () {        
        if (self.TotalCount() === 0)
            return "Không có kết quả nào";       
    }, this);


    // Filter Data
    function FilterData() {
        openProcess();       
        viewModel.CollectionItems([]);
        $.ajax({
            type: "POST",
            url: "/Admin/Collection/GetList",
            data: {},
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values               
                //viewModel.CollectionItems(data.CollectionItems == null ? [] : data.CollectionItems);
                $.each(data.CollectionItems, function () {
                    self.CollectionItems.push(new AddCollectionItems(this.Id, this.Name, this.Url, this.OrderBy, this.CollectionChildItems, this.ProductItems));
                });
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
    self.DeleteItems = function (item) {
        viewModel.ItemId(item.Id);
        $('#popupDelete').modal('show');

    };

    self.DeleteConfirmed = function () {
        var dataPost = { "id": viewModel.ItemId() };
        $.ajax({
            type: "POST",
            url: "/Admin/Collection/Delete",
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

    //add product vào collection
    self.AddProductItems = function (item) {
        viewModel.ItemId(item.Id);
        $('#popupAddProduct').modal('show');
    }

    function SearchProduct() {
        openProcess();
        var dataPost = {
            "query": viewModel.Query(),            
            "pageIndex": viewModel.PageIndex(),
            "pageSize": viewModel.PageSize()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Collection/GetListProduct",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values
                viewModel.TotalCount(data.TotalCount);
                viewModel.TotalPages(data.TotalPages);                
                viewModel.ProductSearchItems(data.ProductItems == null ? [] : data.ProductItems);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    }
    self.ActionSearchProduct = function () {
        SearchProduct();
    };

    self.AddToBox = function (item) {
        //alert(item.OrderBy)
        var dataPost = { "productId": item.Id, "collectionId": viewModel.ItemId(), "orderBy": item.OrderBy };
        $.ajax({
            type: "POST",
            url: "/Admin/Collection/AddProduct",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                $('#popupAddProduct').modal('hide');
                viewModel.Search();
            },
            error: function (xhr, status, error) {
                showPopupError();
            }
        });
    }


    
}

ko.applyBindings(viewModel, document.getElementById("collectionList"));
//// Onload
// jQuery
$(document).ready(function () {
    // First Load
    viewModel.Search();
});