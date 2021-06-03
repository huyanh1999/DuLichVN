/**********************************Chọn menu***********************************/
var selectMenuTypeModel = new function () {
    var self = this;

    self.ItemId = ko.observable(0);
    self.MenuTypeId = ko.observable("");
    self.MenuTypeOptions = ko.observableArray(data.MenuTypeOptions != null ? data.MenuTypeOptions : []);

    self.SelectMenu = function () {
        frmMenuModel.SearchListMenu();
        frmMenuModel.Search();
    };

}

ko.applyBindings(selectMenuTypeModel, document.getElementById("SelectMenuType"));

/*********************************danh sách menu******************************/

//var menuListModel = new function () {
//    var self = this;

//    self.ItemId = ko.observable(0);    
//    self.MenuItems = ko.observableArray([]);

//    // Filter Data
//    function FilterData() {
//        openProcess();
//        var dataPost = {
//            "type": $('#MenuTypeId option:selected').val()
//        };

//        $.ajax({
//            type: "POST",
//            url: "/Menus/GetList",
//            data: dataPost,
//            dataType: "json",
//            success: function (data, textStatus, jqXHR) {
//                hideProcess();
//                // Set values                
//                menuListModel.MenuItems(data.MenuItems == null ? [] : data.MenuItems);
//            },
//            error: function (xhr, status, error) {
//                // Stop progress bar
//                hideProcess();
//                showPopupError();
//            }
//        });
//    }
//    // Search
//    self.Search = function () {
//        FilterData();
//    };
//    self.DeleteItems = function (item) {
//        menuListModel.ItemId(item.Id);
//        $('#popupDelete').modal('show');

//    };

//    self.DeleteConfirmed = function () {
//        var dataPost = { "id": menuListModel.ItemId() };
//        $.ajax({
//            type: "POST",
//            url: "/Menus/Delete",
//            data: dataPost,
//            dataType: "json",
//            success: function (data, textStatus, jqXHR) {
//                $('#popupDelete').modal('hide');                
//                menuListModel.Search();
//            },
//            error: function (xhr, status, error) {
//                showPopupError();
//            }
//        });
//    };

//    self.EditItems = function(item) {

//    };
//}

//ko.applyBindings(menuListModel, document.getElementById("MenuList"));


/*********************************Add menu******************************/

var frmMenuModel = new function () {
    var self = this;
    self.btnName = ko.observable('Thêm mới');
    //Nếu là 0 -> thêm mới, 1 -> Update
    self.FlagSubmit = ko.observable(0);
    //Danh sách menu
    self.ItemId = ko.observable(0);
    self.MenuItems = ko.observableArray([]);

    // filter data danh sách menu
    function FilterData() {
        openProcess();
        var dataPost = {
            "type": $('#MenuTypeId option:selected').val()
        };

        $.ajax({
            type: "POST",
            url: "/Admin/Menus/GetList",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values                
                frmMenuModel.MenuItems(data.MenuItems == null ? [] : data.MenuItems);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    }
    // Search danh sách menu
    self.SearchListMenu = function () {
        FilterData();
    };
    // Delete menu
    self.DeleteItems = function (item) {
        frmMenuModel.ItemId(item.Id);
        $('#popupDelete').modal('show');

    };

    self.DeleteConfirmed = function () {
        var dataPost = { "id": frmMenuModel.ItemId() };
        $.ajax({
            type: "POST",
            url: "/Admin/Menus/Delete",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                $('#popupDelete').modal('hide');
                //frmMenuModel.SearchListMenu();
                
                FilterData();
                FilterParentData();
                FilterMenuContentData();
                self.ParentId(0);
            },
            error: function (xhr, status, error) {
                showPopupError();
            }
        });
    };

    // Cập nhật menu
    self.EditItems = function (item) {
        frmMenuModel.ItemId(item.Id);
        frmMenuModel.FlagSubmit(1);
        FilterParentData();
        
        var dataPost = {
            "id": frmMenuModel.ItemId()
        };
        $.ajax({
            type: "POST",
            url: "/Menus/GetById",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                // Stop progress
                hideProcess();
                if (data != null) {
                    
                    frmMenuModel.Name(data.Name);
                    frmMenuModel.LinkUrl(data.Url);
                    frmMenuModel.OrderBy(data.OrderBy);
                    frmMenuModel.MenuSourceId(data.Type);
                    frmMenuModel.ParentId(data.ParentId);                    
                    frmMenuModel.MenuContentId(data.ItemId);
                    self.btnName('Cập nhật');

                }
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    };

    //End danh sách menu

    //nguồn nội dung
    self.MenuSourceId = ko.observable("");
    self.MenuSourceOptions = ko.observableArray(data.MenuSourceOptions != null ? data.MenuSourceOptions : []);

    self.Name = ko.observable('').extend({ maxLength: 512 });

    self.LinkUrl = ko.observable('');
    self.VisibleUrl = ko.computed(function () {        
        if (self.MenuSourceId() === "LINK") {
            return 0;
        }
        return 1;
    }, this);

    self.OrderBy = ko.observable(1).extend({number: { message: "Chỉ được nhập số" }});


    

    //Parent
    self.ParentId = ko.observable(0);
    self.ParentOptions = ko.observableArray([]);

    // combobox nội dung
    self.MenuContentId = ko.observable(0);
    self.MenuContentOptions = ko.observableArray([]);    

    // Filter Data danh mục cha
    function FilterParentData() {
        openProcess();
        var dataPost = {
            "type": $('#MenuTypeId option:selected').val()
        };

        $.ajax({
            type: "POST",
            url: "/Menus/GetMenuParentOptions",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values                
                frmMenuModel.ParentOptions(data.ParentOptions == null ? [] : data.ParentOptions);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    }

    // Search danh mục cha
    self.Search = function () {
        FilterParentData();
    };

    // Load combobox chọn danh mục theo loại nguồn danh mục (category, tag)

    self.MenuSourceId.subscribe(function () {
        FilterMenuContentData();
    });
    function FilterMenuContentData() {
        openProcess();
        var dataPost = {
            "type": frmMenuModel.MenuSourceId()
        };

        $.ajax({
            type: "POST",
            url: "/Menus/GetMenuConentOptions",
            data: dataPost,
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                hideProcess();
                // Set values                
                frmMenuModel.MenuContentOptions(data.MenuContentOptions == null ? [] : data.MenuContentOptions);
            },
            error: function (xhr, status, error) {
                // Stop progress bar
                hideProcess();
                showPopupError();
            }
        });
    }

    //Thêm mới, cập nhật
    
    self.Submit = function () {
        frmMenuModel.Name($('#Name').val());
        if ($('#Name').val() === '') {
            document.getElementById('Notice').innerHTML = "Tiêu đề menu không được để trống!";
            return false;
        }        
        if (frmMenuModel.errors().length == 0) {
            if (self.FlagSubmit() == 0) {
                //Thêm mới menu
                self.btnName('Thêm mới');
                self.FlagSubmit(0);
                var dataPost = {
                    "parentId": $('#ParentId option:selected').val(),
                    "name": frmMenuModel.Name(),
                    "type": frmMenuModel.MenuSourceId(),
                    "menuType": $('#MenuTypeId option:selected').val(),
                    "linkUrl": frmMenuModel.LinkUrl(),
                    "orderby": frmMenuModel.OrderBy(),
                    "itemId": $('#MenuContentId option:selected').val()
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/Menus/Add",
                    data: dataPost,
                    dataType: "json",
                    success: function(data, textStatus, jqXHR) {
                        // Stop progress
                        hideProcess();
                        // Set values                    
                        if (data == 'error') {
                            alert('Có lỗi xảy ra. Xin vui lòng liên hệ quản trị!');
                            return false;
                        }
                        FilterParentData();
                        FilterData();
                        frmMenuModel.LinkUrl('');
                        frmMenuModel.OrderBy('1');
                        frmMenuModel.Name('');
                        document.getElementById('Notice').innerHTML = "Thêm mới thành công!";
                        
                        
                    },
                    error: function(xhr, status, error) {
                        // Stop progress bar
                        hideProcess();
                        showPopupError();
                    }
                });
            } else {
                //update menu
                self.FlagSubmit(0);
                self.btnName('Thêm mới');
                var dataUpdatePost = {
                    "id":frmMenuModel.ItemId(),
                    "parentId": $('#ParentId option:selected').val(),
                    "name": frmMenuModel.Name(),
                    "type": frmMenuModel.MenuSourceId(),
                    "menuType": $('#MenuTypeId option:selected').val(),
                    "linkUrl": frmMenuModel.LinkUrl(),
                    "orderby": frmMenuModel.OrderBy(),
                    "itemId": $('#MenuContentId option:selected').val()
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/Menus/Update",
                    data: dataUpdatePost,
                    dataType: "json",
                    success: function (data, textStatus, jqXHR) {
                        // Stop progress
                        hideProcess();
                        // Set values                    
                        if (data == 'error') {
                            alert('Có lỗi xảy ra. Xin vui lòng liên hệ quản trị!');
                            return false;
                        }
                        FilterParentData();
                        FilterData();
                        frmMenuModel.LinkUrl('');
                        frmMenuModel.OrderBy('1');
                        frmMenuModel.Name('');
                        document.getElementById('Notice').innerHTML = "Cập nhật thành công!";
                    },
                    error: function (xhr, status, error) {
                        // Stop progress bar
                        hideProcess();
                        showPopupError();
                    }
                });
            }
            
        }
        else {
            frmMenuModel.errors.showAllMessages();
        }
        
    };

    self.Cancel = function () {
        frmMenuModel.LinkUrl('');
        frmMenuModel.OrderBy('1');
        frmMenuModel.FlagSubmit(0);
        frmMenuModel.Name('');
        document.getElementById('Notice').innerHTML = "";
        self.btnName('Thêm mới');
    };

}
frmMenuModel.errors = ko.validation.group(frmMenuModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(frmMenuModel, document.getElementById("frmMenu"));


//// Onload




// jQuery
$(document).ready(function () {
    // First Load
    frmMenuModel.SearchListMenu();
    frmMenuModel.Search();
});