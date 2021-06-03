function getFormattedDate(date) {
  var year = date.getFullYear();
  var month = (1 + date.getMonth()).toString();
  month = month.length > 1 ? month : '0' + month;
  var day = date.getDate().toString();
  day = day.length > 1 ? day : '0' + day;
  return day + '/' + month + '/' + year;
}
var viewModel = new function () {
    var self = this;
    var date = new Date();
    // package
    self.PackageKey = ko.observable('');
    self.PackageOptions = ko.observable(data.PackageOptions != null ? data.PackageOptions : []);


    // Thông tin chung
    self.ContractCode = ko.observable('')
                    .extend({ required: { message: "Mã hợp đồng là bắt buộc" } })
                    .extend({ maxLength: 50 });
    self.ContractDate = ko.observable(getFormattedDate(date)).extend({ required: { message: "Ngày ký hợp đồng là bắt buộc" } });

    self.Price = ko.observable(0).extend({ number: { message: "Chỉ được nhập số" } });
    self.StartDate = ko.observable(getFormattedDate(date)).extend({ required: { message: "Ngày bắt đầu là bắt buộc" } });;

    self.ExpiredDay = ko.observable(0).extend({ required: { message: "Số ngày hiệu lực là bắt buộc" } }).extend({ number: { message: "Chỉ được nhập số" } });
    self.ContractTitle = ko.observable('')
                    .extend({ required: { message: "Tên liên hệ là bắt buộc" } })
                    .extend({ maxLength: 50 });
    self.Email = ko.observable('')
                    .extend({ maxLength: 150 }).extend({
                        pattern: {
                            message: "Nhập địa chỉ e-mail hợp lệ",
                            params: EMAILADDRESS_EXPRESSION
                        }
                    });
    self.Phone = ko.observable('').extend({ maxLength: 50 });
    self.Fax = ko.observable('').extend({ maxLength: 50 });    
    self.Address = ko.observable('').extend({ maxLength: 250 });    
    self.Note = ko.observable('').extend({ maxLength: 500 });    

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
ko.applyBindings(viewModel, document.getElementById("addUserContract"));